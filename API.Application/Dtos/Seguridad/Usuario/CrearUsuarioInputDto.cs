using System.Text.Json.Serialization;

namespace API.Application.Dtos.Seguridad.Usuario
{
    public class CrearUsuarioInputDto : UsuarioDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        public required string Contrasenna { get; set; }
        public required string ContrasennaConfirmada { get; set; }

    }
}
