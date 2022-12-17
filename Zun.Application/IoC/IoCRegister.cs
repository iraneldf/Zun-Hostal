using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;
using System.Reflection;
using Zun.Aplicacion.Mapper;
using Zun.Datos.DbContext;
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

            //swagger
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

            return services;
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

            app.Run();

            return app;
        }

        public static IServiceCollection RegistrarDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            var  connectionStringSection = configuration.GetSection("ConnectionStrings:ZunContext");
            string typeDatabase = configuration.GetSection("TypeDatabase").Value;

            services.Configure<ConnectionStringSettings>(connectionStringSection);
            string connectionString = connectionStringSection.Value;

            switch(typeDatabase)
            {
                case "MSSqlServer":
                    services.AddDbContext<ZunDbContext>(options => options.UseSqlServer(connectionString: connectionString));
                    break;
                 case "MySQL":
                    services.AddDbContext<ZunDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
                    break;
            }
            services.AddTransient<IZunDbContext, ZunDbContext>();

            return services;
        }

        public static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped<IEntidadEjemploRepositorio, EntidadEjemploRepositorio>();
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