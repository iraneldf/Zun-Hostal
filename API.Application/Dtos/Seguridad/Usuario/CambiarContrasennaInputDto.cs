namespace API.Application.Dtos.Seguridad.Usuario
{
    public class CambiarContrasennaInputDto
    {
        public Guid UsuarioId { get; set; }
        public string? ContrasennaAntigua { get; set; }
        public required string NuevaContrasenna { get; set; }
        public required string ContrasennaConfirmada { get; set; }
    }
}
