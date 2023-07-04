using AutoMapper;
using Zun.Aplicacion.Dtos;
using Zun.Datos.Entidades;

namespace Zun.Aplicacion.Mapper
{
    /// <summary>
    /// Generar los mapeos para todas las entidades de forma genérica, deben especificarse
    /// los 5 tipos de datos que se van a estar mapeando
    /// </summary>
    /// <typeparam name="Entidad"></typeparam>
    /// <typeparam name="EntidadDto"></typeparam>
    /// <typeparam name="CrearEntidadDto"></typeparam>
    /// <typeparam name="ModificarEntidadDto"></typeparam>
    /// <typeparam name="ListarEntidadDto"></typeparam>
    public class ProfileBase<Entidad, EntidadDto, CrearEntidadDto, ModificarEntidadDto, ListarEntidadDto> : Profile
    where Entidad : EntidadBase where EntidadDto : EntidadBaseDto where ListarEntidadDto : EntidadBaseDto
    {
        public ProfileBase()
        {
            MapeoEntidad();
            MapeoCrearEntidad();
            MapeoModificar();
            MapeoListar();
            MapeoEntidadEntidad();
            MapeoEntidadDtoEntidadDto();
        }

        public virtual void MapeoEntidadDtoEntidadDto()
        {
            CreateMap<EntidadDto, EntidadDto>()
                         .ReverseMap();
        }

        public virtual void MapeoEntidadEntidad()
        {
            CreateMap<Entidad, Entidad>()
                         .ReverseMap();
        }

        public virtual void MapeoListar()
        {
            CreateMap<Entidad, ListarEntidadDto>()
                         .ReverseMap();
        }

        public virtual void MapeoModificar()
        {
            CreateMap<Entidad, ModificarEntidadDto>()
                          .ReverseMap();
        }

        public virtual void MapeoCrearEntidad()
        {
            CreateMap<Entidad, CrearEntidadDto>()
              .ReverseMap();
        }

        public virtual void MapeoEntidad()
        {
            CreateMap<Entidad, EntidadDto>()
             .ReverseMap();
        }
    }
}
