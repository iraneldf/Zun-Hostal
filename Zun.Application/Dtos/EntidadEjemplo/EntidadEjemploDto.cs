using Zun.Aplicacion.Dtos;

namespace Zun.Aplicacion.Dtos.EntidadEjemplo
{
    public class EntidadEjemploDto : EntidadBaseDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string? Intereses { get; set; }
    }
}
