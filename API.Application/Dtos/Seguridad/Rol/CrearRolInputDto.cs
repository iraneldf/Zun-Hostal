using System.Text.Json.Serialization;

namespace API.Application.Dtos.Seguridad.Rol
{
    public class CrearRolInputDto : RolDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        public required List<Guid> PermisoIds { get; set; } = new();

    }
}
