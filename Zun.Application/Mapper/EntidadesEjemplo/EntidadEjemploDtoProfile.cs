using AutoMapper;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;

namespace Zun.Aplicacion.Mapper.EntidadesEjemplo
{
    public class EntidadEjemploDtoProfile : Profile
    {
        public EntidadEjemploDtoProfile()
        {
            CreateMap<EntidadEjemplo, CrearEntidadEjemploInputDto>()
              .ReverseMap();

            CreateMap<EntidadEjemplo, ModificarEntidadEjemploInputDto>()
              .ReverseMap();

            CreateMap<EntidadEjemplo, EntidadEjemploDto>()
             .ReverseMap();

            CreateMap<EntidadEjemplo, EntidadEjemploListadoPaginadoOutputDto>()
             .ReverseMap();
        }
    }
}
