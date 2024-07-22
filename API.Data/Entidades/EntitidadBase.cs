namespace API.Data.Entidades
{
    public class EntidadBase
    {
        public Guid Id { get; set; }
        public DateTime FechaCreado { get; set; } = new DateTime();
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaActualizado { get; set; } = new DateTime();
        public string ActualizadoPor { get; set; } = string.Empty;
    }
}
