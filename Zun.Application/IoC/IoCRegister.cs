using Zun.Datos.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Reflection;
using System.Text;
using Zun.Datos.IUnitOfWork.Repositorios;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Datos.DbContext;
using Zun.Dominio.Interfaces;
using Zun.Dominio.Servicios;
using Zun.Aplicacion.Mapper;

namespace Zun.Aplicacion.IoC
{
    public static class IoCRegister
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegistrarDataContext(configuration);
            services.RegistrarServicios(configuration);
            services.RegistrarRepositorios();
            services.RegistrarServiciosDominio();
            services.RegistrarPoliticasAutorizacion();

            return services;
        }

        public static IServiceCollection RegistrarServicios(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddAutoMappers(AutoMapperConfiguration.CreateExpression().AddAutoMapperLeadOportunidade());

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyCorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                });
            });

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
            var connectionStringSection = configuration.GetSection("ConnectionStrings:ZunContext");

            services.Configure<ConnectionStringSettings>(connectionStringSection);
            var connectionString = connectionStringSection.Value;

            services.AddDbContext<ZunDbContext>(options => options.UseSqlServer(connectionString: connectionString));
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
    }
}