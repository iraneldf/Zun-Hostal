namespace Zun.Application.Dtos
{
    public class ConfiguracionListadoPaginadoDto
    {
        /// <summary>
        /// Cantidad de elementos a ignorar
        /// </summary>
        public int CantIgnorar { get; set; }
        /// <summary>
        /// Cantidad de elementos a mostrar
        /// </summary>
        public int CantMostrar { get; set; }
    }
}
