using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.Configuration;
using System.Reflection;
using Zun.Aplicacion.AuthorizationFilter;
using Zun.Aplicacion.Mapper;
using Zun.Datos.DbContexts;
using Zun.Datos.IUnitOfWork;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Datos.IUnitOfWork.Repositorios;
using Zun.Dominio.Interfaces;
using Zun.Dominio.Servicios;

namespace Zun.Aplicacion.IoC
{
    public static class IoCRegister
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegistrarDataContext(configuration);
            services.RegistrarServicios();
            services.RegistrarRepositorios();
            services.RegistrarServiciosDominio();
            services.RegistrarPoliticasAutorizacion();
            services.RegistrarSwagger();
            services.RegistrarHangfire(configuration);

            return services;
        }


        public static IServiceCollection RegistrarServicios(this IServiceCollection services)
        {

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddAutoMappers(AutoMapperConfiguration.CreateExpression().AddAutoMapperLeadOportunidade());

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyCorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                });
            });

            //Add services to validation

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Program>();


            return services;
        }

        public static void RegistrarHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var zunConnectionString = configuration.GetSection("ConnectionStrings:ZunContext")
                .Value.Replace("Trust Server Certificate=true;", "");

            services.AddHangfire(configuration =>
            {
                IGlobalConfiguration<AutomaticRetryAttribute> globalConfiguration = configuration
                                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                    .UseSimpleAssemblyNameTypeSerializer()
                                    .UseRecommendedSerializerSettings(settings => settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                                    .UseFilter(new AutomaticRetryAttribute { Attempts = 2 });

                globalConfiguration.UseStorage(
                    new MySqlStorage(
                        zunConnectionString,
                        new MySqlStorageOptions
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(10),
                            JobExpirationCheckInterval = TimeSpan.FromHours(1),
                            CountersAggregateInterval = TimeSpan.FromMinutes(5),
                            PrepareSchemaIfNecessary = true,
                            DashboardJobListLimit = 25000,
                            TransactionTimeout = TimeSpan.FromMinutes(1),
                            TablesPrefix = "Hangfire",
                        }
                    )
                );
            });
            services.AddHangfireServer();
        }

        private static void RegistrarSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ZUN API",
                    Description = "API para realizar las operaciones del sistema"
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        public static IApplicationBuilder AddRegistration(WebApplication app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.Run();

            return app;
        }

        public static IServiceCollection RegistrarDataContext(this IServiceCollection services, IConfiguration configuration)
        {

            var zunConnectionStringSection = configuration.GetSection("ConnectionStrings:ZunContext");
            var trazasConnectionStringSection = configuration.GetSection("ConnectionStrings:TrazaContext");
            var mssqlConnectionStringSection = configuration.GetSection("ConnectionStrings:MSSQLContext");

            services.Configure<ConnectionStringSettings>(zunConnectionStringSection);
            services.Configure<ConnectionStringSettings>(trazasConnectionStringSection);
            services.Configure<ConnectionStringSettings>(mssqlConnectionStringSection);

            string zunConnectionString = zunConnectionStringSection.Value;
            string trazasConnectionString = trazasConnectionStringSection.Value;
            string mssqlConnectionString = mssqlConnectionStringSection.Value;

            //"MSSqlServer":
            services.AddDbContext<MSSQLDbContext>(options => options.UseSqlServer(connectionString: mssqlConnectionString));
            //"MySQL":
            services.AddDbContext<ZunDbContext>(options => options.UseMySql(zunConnectionString, ServerVersion.AutoDetect(zunConnectionString)));
            services.AddDbContext<TrazasDbContext>(options => options.UseMySql(trazasConnectionString, ServerVersion.AutoDetect(trazasConnectionString)));

            services.AddTransient<IZunDbContext, ZunDbContext>();
            services.AddTransient<ITrazasDbContext, TrazasDbContext>();

            return services;
        }

        public static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped<IEntidadEjemploRepositorio, EntidadEjemploRepositorio>();
            services.AddScoped<ITrazasDbContext, TrazasDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection RegistrarServiciosDominio(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServicioBase<>), typeof(ServicioBase<>));
            services.AddScoped<IEntidadEjemploServicio, EntidadEjemploServicio>();

            return services;
        }

        public static IServiceCollection RegistrarPoliticasAutorizacion(this IServiceCollection services)
        {
            // Agregando las Politicas para la autorizacion           
            return services;
        }

        internal static void AddLogsRegistration(WebApplicationBuilder builder)
        {
            IConfiguration serilogConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(serilogConfiguration)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}