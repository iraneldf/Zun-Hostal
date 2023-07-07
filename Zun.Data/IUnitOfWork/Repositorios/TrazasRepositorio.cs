using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
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

        public virtual async Task<List<Traza>> GetAllAsync(params Expression<Func<Traza, object>>[] includeProperties) => await GetQuery(includeProperties).ToListAsync();
        public virtual async Task<List<Traza>> GetAllAsync(Expression<Func<Traza, bool>> condicion, params Expression<Func<Traza, object>>[] includeProperties) => await GetQuery(includeProperties).Where(condicion).ToListAsync();
        public IQueryable<Traza> GetQuery(params Expression<Func<Traza, object>>[] includeProperties)
        {
            IQueryable<Traza> query = _context.Set<Traza>().AsNoTracking();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
