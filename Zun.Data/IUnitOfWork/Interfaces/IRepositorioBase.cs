using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Zun.Datos.Entidades;

namespace Zun.Datos.IUnitOfWork.Interfaces
{
    public interface IRepositorioBase<TEntidad> where TEntidad : EntitidadBase
    {
        /// <summary>
        /// Agrega una nueva entidad
        /// </summary>
        /// <param name="entidad">Entidad a agregar</param>
        Task<EntityEntry<TEntidad>> AddAsync(TEntidad entidad);
        /// <summary>
        /// Agrega varias entidades
        /// </summary>
        /// <param name="entidades">Entidades a agregar</param>
        Task AddRangeAsync(IEnumerable<TEntidad> entidades);
        /// <summary>
        /// Comprueba que exista al menos una entidad
        /// </summary>
        Task<bool> AnyAsync();
        /// <summary>
        /// Comprueba que exista al menos una entidad que cumpla la condicion
        /// </summary>
        /// <param name="condicion">Condicion de busqueda</param>
        Task<bool> AnyAsync(Expression<Func<TEntidad, bool>> condicion);
        Task CommitTransaccionAsync();

        /// <summary>
        /// Comprueba que exista la entidad especificada
        /// </summary>
        /// <param name="entidad">Entidad a buscar</param>
        Task<bool> ContainsAsync(TEntidad entidad);
        /// <summary>
        /// Retorna la cantidad total de entidades
        /// </summary>
        Task<int> CountAsync();
        /// <summary>
        /// Retorna la cantidad de entidades que cumplen la condicion
        /// </summary>
        /// <param name="condicion">Condicion de busqueda</param>
        Task<int> CountAsync(Expression<Func<TEntidad, bool>> condicion);
        /// <summary>
        /// Retorna la primera entidad que cumpla la condicion
        /// </summary>
        /// <param name="condicion">Condicion de busqueda</param>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<TEntidad?> FirstAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includeProperties);
        /// <summary>
        /// Retorna todas las entidades que cumplen la condicion
        /// </summary>
        /// <param name="condicion">Condicion de busqueda</param>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<List<TEntidad>> GetAllAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includeProperties);
        /// <summary>
        /// Retorna todas las entidades
        /// </summary>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<List<TEntidad>> GetAllAsync(params Expression<Func<TEntidad, object>>[] includeProperties);
        /// <summary>
        /// Retorna la entidad que contiene el mismo id pasado por parametro
        /// </summary>
        /// <param name="id">id para buscar la entidad</param>
        Task<TEntidad?> GetByIdAsync(int id);
        /// <summary>
        /// Retorna la entidad que contiene el mismo id pasado por parametro
        /// </summary>
        /// <param name="id">id para buscar la entidad</param>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<TEntidad?> GetByIdAsync(int id, params Expression<Func<TEntidad, object>>[] includeProperties);
        /// <summary>
        /// Retorna un elemento IQueryable para la realizacion de consultas
        /// </summary>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        IQueryable<TEntidad> GetQuery(params Expression<Func<TEntidad, object>>[] includeProperties);
        Task<IDbContextTransaction> IniciarTransaccionAsync();

        /// <summary>
        /// Retorna la ultima entidad
        /// </summary>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<TEntidad?> LastAsync(params Expression<Func<TEntidad, object>>[] includeProperties);
        /// <summary>
        /// Retorna la ultima entidad que cumpla la condicion
        /// </summary>
        /// <param name="condicion">Condicion de busqueda</param>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<TEntidad?> LastAsync(Expression<Func<TEntidad, bool>> condicion, params Expression<Func<TEntidad, object>>[] includeProperties);
        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="entidad">Entidad a eliminar</param>
        EntityEntry<TEntidad> Remove(TEntidad entidad);
        /// <summary>
        /// Elimina la entidad que tiene el id pasado por parametro
        /// </summary>
        /// <param name="id">id de la entidad a eliminar</param>
        Task<EntityEntry<TEntidad>?> RemoveByIdAsync(int id);
        /// <summary>
        /// Elimina varias entidades
        /// </summary>
        /// <param name="entidades">Entidades a eliminar</param>
        void RemoveRange(IEnumerable<TEntidad> entidades);
        Task RollbackTransaccionAsync();

        /// <summary>
        /// Persiste los cambios en la Base de Datos
        /// </summary>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Modifica los datos de una entidad
        /// </summary>
        /// <param name="entidad">Entidad a modificar</param>
        EntityEntry<TEntidad> Update(TEntidad entidad);
        /// <summary>
        /// Modifica los datos de varias entidades
        /// </summary>
        /// <param name="entidades">Entidades a modificar</param>
        void UpdateRange(List<TEntidad> entidades);
    }
}
