namespace API.Application.Dtos.Comunes
{
    public class ListadoPaginadoDto<TEntity>
    {
        public int Cantidad { get; set; }
        public List<TEntity> Elementos { get; set; } = new();
    }
}
