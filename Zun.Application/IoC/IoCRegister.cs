using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MySql;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.Configuration;
using System.Reflection;
using System.Text.Json.Serialization;
using Zun.Aplicacion.AuthorizationFilter;
using Zun.Aplicacion.Mapper;
using Zun.Datos.ConfiguracionEntidades;
using Zun.Datos.DbContexts;
using Zun.Datos.Enum;
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

            services.AddControllers()

            //Para visualizar en los json los enum
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddAutoMappers(AutoMapperConfiguration.CreateExpression().AddAutoMapperLeadOportunidade());

            services.AddHttpContextAccessor();

            services.AddCors();

            //Add services to validation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Program>();

            // Ignorar Ciclos en los json
            services.AddMvc().AddJsonOptions(
                options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
            );

            return services;
        }
        
        // Registramos Hangfire dependiendo del tipo de Servidor
        public static void RegistrarHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            string servidor = configuration.GetSection("Servidor").Value;

            // Si alguien no escribió correctamente en el appsetings el servidor,
            // este se configura por defecto para SqlServer
            if (!servidor.Equals(ETipoServidor.MySql.ToString())
                && !servidor.Equals(ETipoServidor.SqlServer.ToString()))
            {
                servidor = ETipoServidor.SqlServer.ToString();
            }

            services.AddHangfire(ConfigurarHangFire(servidor, configuration));
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
            app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            app.UseAuthorization();
            app.MapControllers();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.Run();
            return app;
        }

        // Registrar los DbContext dependiendo del tipo de Servidor
        public static IServiceCollection RegistrarDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            string servidor = configuration.GetSection("Servidor").Value;

            // Si alguien no escribió correctamente en el appsetings el servidor,
            // este se configura por defecto para SqlServer
            if (!servidor.Equals(ETipoServidor.MySql.ToString())
                && !servidor.Equals(ETipoServidor.SqlServer.ToString()))
            {
                servidor = ETipoServidor.SqlServer.ToString();
            }

            services = DbContextToServer(servidor, services, configuration);
            return services;
        }

        public static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped<ICamareroRepositorio, CamareroRepositorio>();
            services.AddScoped<IModificadorRepositorio, ModificadorRepositorio>();
            services.AddScoped<IEntidadEjemploRepositorio, EntidadEjemploRepositorio>();
            services.AddScoped<ITrazasRepositorio, TrazasRepositorio>();
      
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection RegistrarServiciosDominio(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServicioBase<>), typeof(ServicioBase<>));
            services.AddScoped<IEntidadEjemploServicio, EntidadEjemploServicio>();
            services.AddScoped<ICamareroServicio, CamareroServicio>();
            services.AddScoped<IModificadorServicio, ModificadorServicio>();
            services.AddScoped<ITrazaServicio, TrazaServicio>();

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

        #region Metodos Auxiliares
        private static Action<IGlobalConfiguration> ConfigurarHangFire(string ts, IConfiguration configuration)
        {
            // definimos el tipo de servidor que nos llegó por parámetro
            // desde la variable en nuestro appsettings
            ETipoServidor tipoServidor = ts.Equals(ETipoServidor.MySql.ToString()) ? ETipoServidor.MySql : ETipoServidor.SqlServer;

            // agregamos la seccion de Zun y Trazas dependiendo del servidor
            string? zunSection = tipoServidor == ETipoServidor.MySql ? "ConnectionStrings:MySqlZunContext" : "ConnectionStrings:SqlServerZunContext";
            string? trazasSection = tipoServidor == ETipoServidor.MySql ? "ConnectionStrings:MySqlTrazasContext" : "ConnectionStrings:SqlServerTrazasContext";

            string zunConnectionString = configuration.GetSection(zunSection).Value;
            string trazasConnectionString = configuration.GetSection(trazasSection).Value;

            switch (tipoServidor)
            {
                case ETipoServidor.MySql:
                    return (configuration =>
                    {
                        IGlobalConfiguration<AutomaticRetryAttribute> globalConfiguration = configuration
                                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                            .UseSimpleAssemblyNameTypeSerializer()
                                            .UseRecommendedSerializerSettings(settings => settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                                            .UseFilter(new AutomaticRetryAttribute { Attempts = 2 });
                        globalConfiguration
                        .UseStorage(
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
                        )
                        .UseStorage(new MySqlStorage(
                                trazasConnectionString,
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
                            ));
                    });

                case ETipoServidor.SqlServer:
                    zunConnectionString = zunConnectionString.Replace("Trust Server Certificate=true;", "");
                    trazasConnectionString = trazasConnectionString.Replace("Trust Server Certificate=true;", "");
                  
                    return (configuration =>
                    {
                        IGlobalConfiguration<AutomaticRetryAttribute> globalConfiguration = configuration
                                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                            .UseSimpleAssemblyNameTypeSerializer()
                                            .UseRecommendedSerializerSettings(settings => settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                                            .UseFilter(new AutomaticRetryAttribute { Attempts = 2 });
                        globalConfiguration
                        .UseStorage(
                            new SqlServerStorage(
                                zunConnectionString,
                                new SqlServerStorageOptions
                                {
                                    QueuePollInterval = TimeSpan.FromSeconds(10),
                                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                    PrepareSchemaIfNecessary = true,
                                    DashboardJobListLimit = 25000,
                                    TransactionTimeout = TimeSpan.FromMinutes(1),
                                    SchemaName = "Hangfire",
                                }
                            )
                        )
                        .UseStorage(
                            new SqlServerStorage(
                                trazasConnectionString,
                                new SqlServerStorageOptions
                                {
                                    QueuePollInterval = TimeSpan.FromSeconds(10),
                                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                    PrepareSchemaIfNecessary = true,
                                    DashboardJobListLimit = 25000,
                                    TransactionTimeout = TimeSpan.FromMinutes(1),
                                    SchemaName = "Hangfire",
                                }
                            )
                        );
                    });

                default:
                    // Por defecto se configura SqlServer
                    return (configuration =>
                    {
                        IGlobalConfiguration<AutomaticRetryAttribute> globalConfiguration = configuration
                                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                            .UseSimpleAssemblyNameTypeSerializer()
                                            .UseRecommendedSerializerSettings(settings => settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                                            .UseFilter(new AutomaticRetryAttribute { Attempts = 2 });
                        globalConfiguration
                        .UseStorage(
                            new SqlServerStorage(
                                zunConnectionString,
                                new SqlServerStorageOptions
                                {
                                    QueuePollInterval = TimeSpan.FromSeconds(10),
                                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                    PrepareSchemaIfNecessary = true,
                                    DashboardJobListLimit = 25000,
                                    TransactionTimeout = TimeSpan.FromMinutes(1),
                                    SchemaName = "Hangfire",
                                }
                            )
                        )
                        .UseStorage(
                            new SqlServerStorage(
                                trazasConnectionString,
                                new SqlServerStorageOptions
                                {
                                    QueuePollInterval = TimeSpan.FromSeconds(10),
                                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                    PrepareSchemaIfNecessary = true,
                                    DashboardJobListLimit = 25000,
                                    TransactionTimeout = TimeSpan.FromMinutes(1),
                                    SchemaName = "Hangfire",
                                }
                            )
                        );
                    });
            }
        }
        /// <summary>
        /// Definimos las bases de datos a usar, con sus DbContext
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection DbContextToServer(string ts, IServiceCollection services, IConfiguration configuration)
        {
            // Definimos el tipo de servidor
            ETipoServidor tipoServidor = ts.Equals(ETipoServidor.MySql.ToString()) ? ETipoServidor.MySql : ETipoServidor.SqlServer;

            IConfigurationSection? zunConnectionStringSection;
            IConfigurationSection? trazasConnectionStringSection;

            if (tipoServidor == ETipoServidor.MySql)
            {
                zunConnectionStringSection = configuration.GetSection("ConnectionStrings:MySqlZunContext");
                trazasConnectionStringSection = configuration.GetSection("ConnectionStrings:MySqlTrazasContext");
            }
            else
            {
                zunConnectionStringSection = configuration.GetSection("ConnectionStrings:SqlServerZunContext");
                trazasConnectionStringSection = configuration.GetSection("ConnectionStrings:SqlServerTrazasContext");
            }

            switch (tipoServidor)
            {
                case ETipoServidor.MySql:
                    services.Configure<ConnectionStringSettings>(zunConnectionStringSection);
                    services.Configure<ConnectionStringSettings>(trazasConnectionStringSection);

                    string zunConnectionStringMySql = zunConnectionStringSection.Value;
                    string trazasConnectionStringMySql = trazasConnectionStringSection.Value;

                    services.AddDbContext<TrazasDbContext>(options => options.UseMySql(trazasConnectionStringMySql, ServerVersion.AutoDetect(trazasConnectionStringMySql)));
                    services.AddDbContext<ZunDbContext>(options => options.UseMySql(zunConnectionStringMySql, ServerVersion.AutoDetect(zunConnectionStringMySql)));

                    break;

                case ETipoServidor.SqlServer:
                    services.Configure<ConnectionStringSettings>(zunConnectionStringSection);
                    services.Configure<ConnectionStringSettings>(trazasConnectionStringSection);

                    string zunConnectionStringSqlServer = zunConnectionStringSection.Value.Replace("Trust Server Certificate=true;", ""); ;
                    string trazasConnectionStringSqlServer = trazasConnectionStringSection.Value.Replace("Trust Server Certificate=true;", ""); ;

                    services.AddDbContext<ZunDbContext>(options => options.UseSqlServer(connectionString: zunConnectionStringSqlServer));
                    services.AddDbContext<TrazasDbContext>(options => options.UseSqlServer(connectionString: trazasConnectionStringSqlServer));

                    break;
                default:
                    // Por defecto establecemos el SqlServer
                    services.Configure<ConnectionStringSettings>(zunConnectionStringSection);
                    services.Configure<ConnectionStringSettings>(trazasConnectionStringSection);

                    string zunConnectionString = zunConnectionStringSection.Value.Replace("Trust Server Certificate=true;", ""); ;
                    string trazasConnectionString = trazasConnectionStringSection.Value.Replace("Trust Server Certificate=true;", ""); ;

                    services.AddDbContext<ZunDbContext>(options => options.UseSqlServer(connectionString: zunConnectionString));
                    services.AddDbContext<TrazasDbContext>(options => options.UseSqlServer(connectionString: trazasConnectionString));
                    break;
            }

            services.AddTransient<IZunDbContext, ZunDbContext>();
            services.AddTransient<ITrazasDbContext, TrazasDbContext>();

            return services;
        }
        #endregion
    }
}