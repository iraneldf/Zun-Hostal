namespace Zun.Datos.Entidades
{
    public class Traza : EntitidadBase
    {
        public string Descripcion { get; set; } = String.Empty;
        public string TablaBD { get; set; } = String.Empty;
        public string Elemento { get; set; } = String.Empty;
        public string ElementoModificado { get; set; } = String.Empty;
    }
}