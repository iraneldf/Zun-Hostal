using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zun.Datos.DbContexts;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;

namespace Zun.Datos.IUnitOfWork.Repositorios
{
    public class TrazasRepositorio : ITrazasRepositorio
    {
        protected readonly TrazasDbContext _context;
        public TrazasRepositorio(TrazasDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(Traza traza)
        {
            await _context.AddAsync(traza);
            await _context.SaveChangesAsync();
        }
    }
}
