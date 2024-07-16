namespace API.Data.Entidades
{
    public class EntidadBase
    {
        public Guid Id { get; set; }
        public DateTime FechaCreado { get; set; } = DateTime.Now;
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaActualizado { get; set; } = DateTime.Now;
        public string ActualizadoPor { get; set; } = string.Empty;
    }
}
