namespace API.Data.Entidades.Seguridad
{
    /// <summary>
    /// Tabla que guarda datos de los usuarios del sistema
    /// </summary>
    public class Usuario : EntidadBase
    {
        public required Guid RolId { get; set; }

        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public string NombreCompleto { get => $"{Nombre} {Apellidos}"; }
        public required string Username { get; set; }
        public required string Contrasenna { get; set; }
        public required string Correo { get; set; }
        public bool DebeCambiarContrasenna { get; set; }

        public Rol Rol { get; set; } = null!;
    }
}