namespace API.Application.Dtos.Seguridad.Autenticacion
{
    public class LoginInputDto
    {
        public required string Username { get; set; }
        public required string Contrasenna { get; set; }
    }
}
