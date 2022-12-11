using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Datos.DbContext;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;

namespace Zun.Datos.IUnitOfWork.Repositorios
{
    public class RepositorioBase<TEntidad> : IRepositorioBase<TEntidad> where TEntidad : EntitidadBase
    {
        protected readonly ZunDbContext _context;

        public RepositorioBase(ZunDbContext context)
        {
            _context = context;
        }

        public virtual async Task<EntityEntry<TEntidad>> AddAsync(TEntidad entidad) => await _context.Set<TEntidad>().AddAsync(entidad);
        public virtual async Task AddRangeAsync(IEnumerable<TEntidad> entidades) => await _context.Set<TEntidad>().AddRangeAsync(entidades);
        public virtual async Task<bool> AnyAsync() => await _context.Set<TEntidad>().AnyAsync();
        public virtual async Task<bool> ContainsAsync(TEntidad entidad) => await _context.Set<TEntidad>().ContainsAsync(entidad);
        public virtual async Task<int> CountAsync() => await _context.Set<TEntidad>().CountAsync();
        public virtual async Task<List<TEntidad>> GetAllAsync(params Expression<Func<TEntidad, object>>[] includeProperties) => await GetQuery(includeProperties).ToListAsync();
        public virtual async Task<List<TEntidad>> GetAllAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includeProperties) => await GetQuery(includeProperties).Where(condicion).ToListAsync();
        public virtual async Task<TEntidad?> GetByIdAsync(int id) => await _context.Set<TEntidad>().FindAsync(id);
        public virtual async Task<TEntidad?> GetByIdAsync(int id, params Expression<Func<TEntidad, object>>[] includeProperties) => await GetQuery(includeProperties).FirstOrDefaultAsync(e => e.Id == id);
        public virtual EntityEntry<TEntidad> Remove(TEntidad entidad) => _context.Set<TEntidad>().Remove(entidad);
        public virtual void RemoveRange(IEnumerable<TEntidad> entidades) => _context.Set<TEntidad>().RemoveRange(entidades);
        public virtual EntityEntry<TEntidad> Update(TEntidad entidad) => _context.Set<TEntidad>().Update(entidad);
        public virtual void UpdateRange(List<TEntidad> entidades) => _context.Set<TEntidad>().UpdateRange(entidades);
        public virtual async Task<TEntidad?> LastAsync(params Expression<Func<TEntidad, object>>[] includeProperties) => await GetQuery(includeProperties).LastOrDefaultAsync();
        public virtual async Task<TEntidad?> FirstAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includeProperties) => await GetQuery(includeProperties).FirstOrDefaultAsync(condicion);
        public virtual async Task<TEntidad?> LastAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includeProperties) => await GetQuery(includeProperties).LastOrDefaultAsync(condicion);
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntidad, bool>> condicion) => await _context.Set<TEntidad>().AnyAsync(condicion);
        public virtual async Task<int> CountAsync(Expression<Func<TEntidad, bool>> condicion) => await _context.Set<TEntidad>().CountAsync(condicion);
        public virtual async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public virtual async Task<EntityEntry<TEntidad>?> RemoveByIdAsync(int id)
        {
            TEntidad? entidad = await GetByIdAsync(id);

            if (entidad != null)
                return _context.Set<TEntidad>().Remove(entidad);

            return null;
        }

        public IQueryable<TEntidad> GetQuery(params Expression<Func<TEntidad, object>>[] includeProperties)
        {
            IQueryable<TEntidad> query = _context.Set<TEntidad>();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
