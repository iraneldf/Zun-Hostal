using API.Data.DbContexts;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Repositorios.Seguridad
{
    public class TrazaRepository : ITrazaRepository
    {
        protected readonly ApiDbContext _context;
        public TrazaRepository(ApiDbContext context)
        {
            _context = context;
        }

        public virtual async Task Crear(Traza traza)
        {
            await _context.AddAsync(traza);
            await _context.SaveChangesAsync();
        }
    }
}
