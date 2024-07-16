using API.Data.DbContexts;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Repositorios.Seguridad
{
    public class RolRepository : BaseRepository<Rol>, IRolRepository
    {
        public RolRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
