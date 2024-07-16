namespace API.Application.Dtos.Seguridad.Rol
{
    public class ActualizarRolInputDto : RolDto
    {
        public required List<Guid> PermisoIds { get; set; } = new();
    }
}
