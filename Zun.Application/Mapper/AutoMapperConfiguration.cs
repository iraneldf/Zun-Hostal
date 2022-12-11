using AutoMapper;
using Zun.Aplicacion.Mapper.EntidadesEjemplo;

namespace Zun.Aplicacion.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static void AddAutoMappers(this IServiceCollection services, MapperConfigurationExpression cfg)
        {
            IMapper mapper = new MapperConfiguration(cfg).CreateMapper();
            services.AddSingleton(mapper);
        }
        public static MapperConfigurationExpression AddAutoMapperLeadOportunidade(this MapperConfigurationExpression cfg)
        {
            #region EntidadEjemplo
                cfg.AddProfile<EntidadEjemploAEntidadEjemploDto>();
                cfg.AddProfile<EntidadEjemploAEditarEntidadEjemploDto>();
                cfg.AddProfile<EntidadEjemploACrearEntidadEjemploDto>();
                cfg.AddProfile<EntidadEjemploAEntidadEjemploListadoPaginadoDto>();
             #endregion

            return cfg;
        }
        public static MapperConfigurationExpression CreateExpression()
        {
            return new MapperConfigurationExpression();
        }
    }
}
