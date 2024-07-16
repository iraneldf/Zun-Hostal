namespace API.Data.Entidades.Seguridad
{
    /// <summary>
    /// Tabla que guarda datos de los roles del sistema
    /// </summary>
    public class Rol : EntidadBase
    {
        public required string Nombre { get; set; }
        public List<Usuario> Usuarios { get; set; } = new();
        public List<RolPermiso> RolPermiso { get; set; } = new();
    }
}