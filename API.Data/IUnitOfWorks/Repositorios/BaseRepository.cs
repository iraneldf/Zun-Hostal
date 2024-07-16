using API.Data.DbContexts;
using API.Data.Entidades;
using API.Data.IUnitOfWorks.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace API.Data.IUnitOfWorks.Repositorios
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntidadBase
    {
        protected readonly ApiDbContext _context;

        public BaseRepository(ApiDbContext context)
        {
            _context = context;
        }

        public virtual async Task<EntityEntry<TEntity>> AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _context.Set<TEntity>().AddRangeAsync(entities);
        public virtual async Task<bool> AnyAsync() => await _context.Set<TEntity>().AnyAsync();
        public virtual async Task<bool> ContainsAsync(TEntity entity) => await _context.Set<TEntity>().ContainsAsync(entity);
        public virtual async Task<int> CountAsync() => await _context.Set<TEntity>().CountAsync();
        public virtual async Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await GetQuery(propiedadesIncluidas).ToListAsync();
        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await GetQuery(propiedadesIncluidas).Where(condition).ToListAsync();
        public virtual async Task<TEntity?> GetByIdAsync(Guid id) => await GetQuery().FirstOrDefaultAsync(e => e.Id == id);
        public virtual async Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await GetQuery(propiedadesIncluidas).FirstOrDefaultAsync(e => e.Id == id);
        public virtual EntityEntry<TEntity> Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);
        public virtual void RemoveRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().RemoveRange(entities);
        public virtual EntityEntry<TEntity> Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
        public virtual void UpdateRange(List<TEntity> entities) => _context.Set<TEntity>().UpdateRange(entities);
        public virtual async Task<TEntity?> LastAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await GetQuery(propiedadesIncluidas).LastOrDefaultAsync();
        public virtual async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await GetQuery(propiedadesIncluidas).FirstOrDefaultAsync(condition);
        public virtual async Task<TEntity?> LastAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await GetQuery(propiedadesIncluidas).LastOrDefaultAsync(condition);
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition) => await _context.Set<TEntity>().AnyAsync(condition);
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> condition) => await _context.Set<TEntity>().CountAsync(condition);
        public virtual async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public virtual async Task<EntityEntry<TEntity>?> RemoveByIdAsync(Guid id)
        {
            TEntity? entity = await GetByIdAsync(id);

            if (entity != null)
                return _context.Set<TEntity>().Remove(entity);

            return null;
        }

        public IQueryable<TEntity> GetQuery(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();
            if (propiedadesIncluidas != null)
                query = propiedadesIncluidas(query);

            return query;
        }

        public virtual async Task<IDbContextTransaction> StartTransaction() => await _context.Database.BeginTransactionAsync();
        public virtual async Task CommitTransaction() => await _context.Database.CommitTransactionAsync();
        public virtual async Task RollbackTransaction() => await _context.Database.RollbackTransactionAsync();


    }
}
