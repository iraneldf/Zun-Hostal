using API.Data.DbContexts;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Repositorios.Seguridad
{
    public class RolPermisoRepository : BaseRepository<RolPermiso>, IRolPermisoRepository
    {
        public RolPermisoRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
