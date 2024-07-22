using API.Application.Mapper;
using API.Application.Services;
using API.Data.DbContexts;
using API.Data.Entidades;
using API.Data.IUnitOfWorks;
using API.Data.IUnitOfWorks.Interfaces;
using API.Data.IUnitOfWorks.Repositorios;
using API.Domain.Interfaces;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Services;
using API.Domain.Services.Seguridad;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityModel.AspNetCore.OAuth2Introspection;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace API.Application.IoC
{
    public static class IoCRegister
    {
        public static IServiceCollection AddRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegistrarDataContext(configuration);
            services.RegistrarAutenticacion(configuration);
            services.RegistrarServicios();
            services.RegistrarRepositorios();
            services.RegistrarServiciosDominio();
            services.RegistrarSwagger();
            ConfigurarLog4Net();

            return services;
        }



        public static IServiceCollection RegistrarServicios(this IServiceCollection services)
        {

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                //evita que se validen automaticamente los Dto mostrando un formato de error distinto
                options.SuppressModelStateInvalidFilter = true;
            });

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

            //Add services to Hosting
            services.AddWindowsService();
            services.AddHostedService<WindowService>();

            //Add services to validation

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<Program>();

            // Ignore bucles in json
            services.AddMvc()
                    .AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                    .AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            return services;
        }

        private static void RegistrarSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API del Sistema",
                    Description = "Contiene las funcionalidades necesarias para el funcionamiento del sistema"
                });
                //permite insertar el token por el SaggerUI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Inserte el token en el campo de texto.'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
                options.CustomSchemaIds(type => type.ToString());
                //using System.Reflection;
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


            app.Run();

            return app;
        }

        public static IServiceCollection RegistrarDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            string databaseType = configuration["DatabaseType"] ?? "MSSqlServer";

            if (databaseType == "MSSqlServer")
                services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("APIContext")));
            else if (databaseType == "MySqlServer")
                services.AddDbContext<ApiDbContext>(options => options.UseMySql(configuration.GetConnectionString("APIContext"), ServerVersion.AutoDetect(configuration.GetConnectionString("APIContext"))));

            services.AddTransient<IApiDbContext, ApiDbContext>();

            //Aplica las migraciones pendientes. Crea la base datos si no existe.     
            services.BuildServiceProvider().GetRequiredService<ApiDbContext>().Database.Migrate();

            return services;
        }

        public static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            return services;
        }

        public static IServiceCollection RegistrarServiciosDominio(this IServiceCollection services)
        {
            services.AddScoped<IPermisoService, PermisoService>();
            services.AddScoped<IRolPermisoService, RolPermisoService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped(typeof(IBaseService<EntidadBase, AbstractValidator<EntidadBase>>), typeof(BasicService<EntidadBase, AbstractValidator<EntidadBase>>));

            return services;
        }

        private static void RegistrarAutenticacion(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("AuthKey")
                    .AddOAuth2Introspection("AuthKey", options =>
                    {
                        options.Authority = configuration["Apis:ZunSa"];
                        options.ClientId = configuration["Apis:ZunPosClientId"];
                        options.ClientSecret = configuration["Apis:ZunPosClientSecret"];
                        options.EnableCaching = true;
                        options.CacheDuration = TimeSpan.FromMinutes(60);
                    });
            services.AddHttpClient(OAuth2IntrospectionDefaults.BackChannelHttpClientName)
                    .ConfigureHttpMessageHandlerBuilder(builder =>
                    {
                        builder.PrimaryHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
                        };
                    });
        }

        private static void ConfigurarLog4Net()
        {
            //Se configuran los log del sistema con la librería Log4Net
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }
    }
}