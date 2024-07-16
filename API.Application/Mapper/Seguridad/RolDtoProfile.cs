using API.Application.Dtos.Seguridad.Rol;
using API.Data.Entidades.Seguridad;

namespace API.Application.Mapper.Seguridad
{
    public class RolDtoProfile : BaseProfile<Rol, RolDto, CrearRolInputDto, ActualizarRolInputDto, ListadoPaginadoRolDto>
    {
        public RolDtoProfile()
        {
            MapDetallesRolDto();
        }

        public void MapDetallesRolDto()
        {
            CreateMap<Rol, DetallesRolDto>()
            .ForMember(dto => dto.Permisos, opt => opt.MapFrom(e => e.RolPermiso.Select(rolPermiso => rolPermiso.Permiso)));
        }

        public override void MapActualizarEntityDto()
        {
            CreateMap<Rol, ActualizarRolInputDto>()
                .ForMember(dto => dto.PermisoIds, opt => opt.MapFrom(e => e.RolPermiso.Select(e => e.Id)));
            CreateMap<ActualizarRolInputDto, Rol>()
                .ForMember(opt => opt.RolPermiso, dto => dto.MapFrom(e => e.PermisoIds.Select(permisoId => new RolPermiso { PermisoId = permisoId, RolId = e.Id })));
        }

        public override void MapCrearEntityDto()
        {
            CreateMap<Rol, CrearRolInputDto>()
                .ForMember(dto => dto.PermisoIds, opt => opt.MapFrom(e => e.RolPermiso.Select(e => e.Id)));
            CreateMap<CrearRolInputDto, Rol>()
                .ForMember(opt => opt.RolPermiso, dto => dto.MapFrom(e => e.PermisoIds.Select(permisoId => new RolPermiso { PermisoId = permisoId, RolId = e.Id })));
        }
    }
}


