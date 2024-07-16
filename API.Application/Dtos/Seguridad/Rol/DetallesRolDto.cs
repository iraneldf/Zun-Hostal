using API.Application.Dtos.Seguridad.Permiso;

namespace API.Application.Dtos.Seguridad.Rol
{
    public class DetallesRolDto : RolDto
    {
        public List<PermisoDto> Permisos { get; set; } = new();
    }
}
