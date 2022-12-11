namespace Zun.Datos.Entidades
{
    public class EntidadEjemplo : EntitidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string? Intereses { get; set; }

    }
}