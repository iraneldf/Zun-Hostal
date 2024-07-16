namespace API.Data.Entidades.Seguridad
{
    /// <summary>
    /// Tabla de relacion entre rol y permiso
    /// </summary>
    public class RolPermiso : EntidadBase
    {
        public Guid RolId { get; set; }
        public Guid PermisoId { get; set; }

        public Rol Rol { get; set; } = null!;
        public Permiso Permiso { get; set; } = null!;
    }
}