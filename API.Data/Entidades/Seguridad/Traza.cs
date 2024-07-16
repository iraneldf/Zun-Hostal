namespace API.Data.Entidades.Seguridad
{
    /// <summary>
    /// Tabla que guarda el historial del sistema
    /// </summary>
    public class Traza : EntidadBase
    {
        /// <summary>
        /// Descripción de la accion realizada por el usuario
        /// </summary>
        public required string Descripcion { get; set; }
        /// <summary>
        /// Nombre de la tabla modificada en la Base Datos
        /// </summary>
        public required string TablaBD { get; set; }
    }
}