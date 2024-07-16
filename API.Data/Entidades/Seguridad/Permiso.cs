namespace API.Data.Entidades.Seguridad
{
    /// <summary>
    /// Tabla que guarda datos de los roles del sistema
    /// </summary>
    public class Permiso : EntidadBase
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public List<RolPermiso> RolPermiso { get; set; } = new();
    }
}