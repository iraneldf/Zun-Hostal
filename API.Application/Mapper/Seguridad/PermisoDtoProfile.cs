using API.Application.Dtos.Seguridad.Permiso;
using API.Data.Entidades.Seguridad;
using AutoMapper;

namespace API.Application.Mapper.Seguridad
{
    public class PermisoDtoProfile : Profile
    {
        public PermisoDtoProfile()
        {
            MapPermisoDto();
        }

        public void MapPermisoDto()
        {
            CreateMap<Permiso, PermisoDto>()
                .ReverseMap();
        }
    }
}


