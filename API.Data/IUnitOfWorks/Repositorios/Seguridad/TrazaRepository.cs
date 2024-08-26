using API.Data.DbContexts;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Repositorios.Seguridad
{
    public class TrazaRepository : ITrazaRepository
    {
        protected readonly TrazasDbContext _trazasContext;
        public TrazaRepository(TrazasDbContext trazasContext)
        {
            _trazasContext = trazasContext;
        }

        public virtual async Task Crear(Traza traza)
        {
            await _trazasContext.AddAsync(traza);
            await _trazasContext.SaveChangesAsync();
        }
    }
}
