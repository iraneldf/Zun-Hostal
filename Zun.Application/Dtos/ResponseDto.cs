using System.ComponentModel.DataAnnotations;

namespace Zun.Aplicacion.Dtos
{
    public class ResponseDto
    {
        public int Status { get; internal set; }
        public object? Resultado { get; set; }
        public string? MensajeError { get; set; }
    }
}
