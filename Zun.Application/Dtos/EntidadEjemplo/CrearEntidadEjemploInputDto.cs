namespace Zun.Aplicacion.Dtos.EntidadEjemplo
{
    public class CrearEntidadEjemploInputDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string? Intereses { get; set; }
    }
}
