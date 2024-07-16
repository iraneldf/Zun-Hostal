namespace API.Application.Dtos.Comunes
{
    public class ConfiguracionListadoPaginadoDto
    {
        /// <summary>
        /// Cantidad de elementos a ignorar en el resultado
        /// </summary>
        public int CantidadIgnorar { get; set; } = 0;
        /// <summary>
        /// Cantidad de elemetos a mostrar en el listado
        /// </summary>
        public int? CantidadMostrar { get; set; }
        /// <summary>
        /// Secuencia de ordenamiento para ordenar el listado.
        /// FORMATO: Campo1:(asc/desc),Campo2:(asc/desc),...
        /// </summary>
        public string? SecuenciaOrdenamiento { get; set; }
        /// <summary>
        /// Texto para buscar en los campos del listado
        /// </summary>
        public string? TextoBuscar { get; set; }

    }
}
