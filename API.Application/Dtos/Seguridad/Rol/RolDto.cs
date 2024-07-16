using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Seguridad.Rol
{
    public class RolDto : EntidadBaseDto
    {
        public required string Nombre { get; set; }
    }
}
