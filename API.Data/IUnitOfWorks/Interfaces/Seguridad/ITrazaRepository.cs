using API.Data.Entidades.Seguridad;

namespace API.Data.IUnitOfWorks.Interfaces.Seguridad
{
    public interface ITrazaRepository
    {
        Task Crear(Traza traza);
    }
}
