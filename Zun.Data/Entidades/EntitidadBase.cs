using System.ComponentModel.DataAnnotations;

namespace Zun.Datos.Entidades
{
    public class EntidadBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string ModificadoPor { get; set; } = string.Empty;
    }
}
