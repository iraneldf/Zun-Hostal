using API.Application.Mapper.Hostal;
using API.Application.Mapper.Seguridad;
using AutoMapper;

namespace API.Application.Mapper;

public static class AutoMapperConfiguration
{
    public static void AddAutoMappers(this IServiceCollection services, MapperConfigurationExpression cfg)
    {
        var mapper = new MapperConfiguration(cfg).CreateMapper();
        services.AddSingleton(mapper);
    }

    public static MapperConfigurationExpression AddAutoMapperLeadOportunidade(this MapperConfigurationExpression cfg)
    {
        cfg.AddProfile<UsuarioDtoProfile>();
        cfg.AddProfile<RolDtoProfile>();
        cfg.AddProfile<PermisoDtoProfile>();
        cfg.AddProfile<ClienteDtoProfile>();
        cfg.AddProfile<ReservaDtoProfile>();
        cfg.AddProfile<AmaDeLlaveDtoProfile>();
        cfg.AddProfile<HabitacionDtoProfile>();

        return cfg;
    }

    public static MapperConfigurationExpression CreateExpression()
    {
        return new MapperConfigurationExpression();
    }
}