using API.Data.DbContexts;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Repositorios.Seguridad
{
    public class PermisoRepository : BaseRepository<Permiso>, IPermisoRepository
    {
        public PermisoRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
