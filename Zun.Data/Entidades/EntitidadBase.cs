using System.ComponentModel.DataAnnotations;

namespace Zun.Data.Entidades
{
    public class EntitidadBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; } = String.Empty;
        public DateTime FechaModificacion { get; set; }
        public string ModificadoPor { get; set; } = String.Empty;
    }
}
