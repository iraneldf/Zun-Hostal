namespace Zun.Aplicacion.Dtos.EntidadEjemplo
{
    public class FiltroEntidadEjemploListadoPaginadoDto : ConfiguracionListadoPaginadoDto
    {
        public string? Nombre { get; set; }
        public int? Edad { get; set; }
    }
}
