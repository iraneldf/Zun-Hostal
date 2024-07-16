using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Seguridad.Usuario
{
    public class UsuarioDto : EntidadBaseDto
    {
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public string NombreCompleto { get => $"{Nombre} {Apellidos}"; }
        public required string Username { get; set; }
        public required string Correo { get; set; }
        public required Guid RolId { get; set; }
    }
}
