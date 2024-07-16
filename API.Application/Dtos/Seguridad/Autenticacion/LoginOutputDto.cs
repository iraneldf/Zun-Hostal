namespace API.Application.Dtos.Seguridad.Autenticacion
{
    public class LoginOutputDto
    {
        public required string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}
