namespace API.Domain.Services.Hostal.Maqueta;

public class ClienteYReserva
{
    public string NombreCliente { get; set; }
    public string NumeroHabiracion { get; set; } = string.Empty;
    public DateTime FechaEntrada { get; set; }
    public DateTime FechaSalida { get; set; }
    public decimal? Importe { get; set; }
    public Guid ClienteId { get; set; }
    public Guid HabitacionId { get; set; }
}