namespace Zun.Application.Dtos
{
    public class ListadoPaginadoDto<TEntidad>
    {
        public int CantidadTotal { get; set; }
        public List<TEntidad> Elementos { get; set; } = new();
    }
}
