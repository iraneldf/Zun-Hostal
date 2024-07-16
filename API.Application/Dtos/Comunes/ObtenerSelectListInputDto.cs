namespace API.Application.Dtos.Comunes
{
    public class ObtenerSelectListInputDto
    {
        /// <summary>
        /// Nombre del campo que se usuara para definir el valor del item
        /// </summary>
        public string NombreCampoValor { get; set; } = string.Empty;
        /// <summary>
        /// Nombre del campo que se usará para mostrar los items
        /// </summary>
        public string NombreCampoTexto { get; set; } = string.Empty;
        /// <summary>
        /// Valor del item que aparecerá seleccionado
        /// </summary>
        public string? ValorSeleccionado { get; set; }
        /// Secuencia de ordenamiento para ordenar el listado.
        /// FORMATO: Campo1:(asc/desc),Campo2:(asc/desc),...
        public string? SecuenciaOrdenamiento { get; set; }
    }
}
