using API.Data.Entidades;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace API.Data.IUnitOfWorks.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : EntidadBase
    {
        /// <summary>
        /// Agrega un nuevo elemento
        /// </summary>
        /// <param name="entity">elemento a crear</param>
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
        /// <summary>
        /// Agrega varios elementos
        /// </summary>
        /// <param name="entities">elementos a crear</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// Verifica que exista algun elemento
        /// </summary>
        Task<bool> AnyAsync();
        /// <summary>
        /// Verifica que exista algun elemento que cumpla con la condicion definida
        /// </summary>
        /// <param name="condition">condicion a verificar</param>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> condition);
        /// <summary>
        /// Finaliza la transaccion
        /// </summary>
        Task CommitTransaction();

        /// <summary>
        /// verifica que exista el elemento
        /// </summary>
        /// <param name="entity">elementa a verificar</param>
        Task<bool> ContainsAsync(TEntity entity);
        /// <summary>
        /// Retorna la cantidad de elementos
        /// </summary>
        Task<int> CountAsync();
        /// <summary>
        /// Retorna la cantidad de elementos que cumplan la condicion
        /// </summary>
        /// <param name="condicion">condicion</param>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> condicion);
        /// <summary>
        /// Retorna el primer elemento que cumple la condicion
        /// </summary>
        /// <param name="condition">condicion</param>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        ///  Retorna los elementos que cumplen la condicion
        /// </summary>
        /// <param name="condition">condicion</param>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Retorna todos los elementos
        /// </summary>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Obtiene el elemento por el Id
        /// </summary>
        /// <param name="id">Id del elemento</param>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Obtiene un IQueryable del elemento para hacer consultas a la BD
        /// </summary>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        IQueryable<TEntity> GetQuery(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Incia una transaccion en la BD
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> StartTransaction();

        /// <summary>
        /// Retorna el ultimo elemento
        /// </summary>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        Task<TEntity?> LastAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Retorna el ultimo elemento que cumple con la condicion
        /// </summary>
        /// <param name="condition">condicion</param>
        /// <param name="propiedadesIncluidas">propiedades navigacionales a incluir</param>
        Task<TEntity?> LastAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Elimina un elemento
        /// </summary>
        /// <param name="entity">Elemento a eliminar</param>
        EntityEntry<TEntity> Remove(TEntity entity);
        /// <summary>
        /// Elimina un elemento
        /// </summary>
        /// <param name="id">Id del a eliminar</param>
        Task<EntityEntry<TEntity>?> RemoveByIdAsync(Guid id);
        /// <summary>
        /// Elimina varios elementos
        /// </summary>
        /// <param name="entities">elementos a eliminar</param>
        void RemoveRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Termina la transaccion sin persistir los cambios en BD
        /// </summary>
        Task RollbackTransaction();

        /// <summary>
        /// Persiste los cambios en BD
        /// </summary>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Actualiza los datos de un elemento
        /// </summary>
        /// <param name="entity">Elemento a actualizar</param>
        EntityEntry<TEntity> Update(TEntity entity);
        /// <summary>
        /// Actualiza los datos de varios elementos
        /// </summary>
        /// <param name="entities">Elementos a actualizar</param>
        void UpdateRange(List<TEntity> entities);
    }
}
