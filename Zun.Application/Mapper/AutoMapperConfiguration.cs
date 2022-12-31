using AutoMapper;
using Zun.Aplicacion.Mapper.EntidadesEjemplo;
using Zun.Aplicacion.Mapper.Seguridad;

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
            cfg.AddProfile<EntidadEjemploDtoProfile>();

            cfg.AddProfile<RolDtoProfile>();
            cfg.AddProfile<TareaDtoProfile>();

            return cfg;
        }
        public static MapperConfigurationExpression CreateExpression()
        {
            return new MapperConfigurationExpression();
        }
    }
}
