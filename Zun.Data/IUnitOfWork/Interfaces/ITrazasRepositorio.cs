using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Zun.Datos.Entidades;

namespace Zun.Datos.IUnitOfWork.Interfaces
{
    public interface ITrazasRepositorio
    {
        Task AddAsync(Traza traza);
        /// <summary>
        /// Retorna todas las entidades que cumplen la condicion
        /// </summary>
        /// <param name="condicion">Condicion de busqueda</param>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<List<Traza>> GetAllAsync(Expression<Func<Traza, bool>> condicion, params Expression<Func<Traza, object>>[] includeProperties);
        /// <summary>
        /// Retorna todas las entidades
        /// </summary>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        Task<List<Traza>> GetAllAsync(params Expression<Func<Traza, object>>[] includeProperties);
        /// <summary>
        /// Retorna un elemento IQueryable para la realizacion de consultas
        /// </summary>
        /// <param name="includeProperties">Propiedades navigacionales para incluir</param>
        IQueryable<Traza> GetQuery(params Expression<Func<Traza, object>>[] includeProperties);
    }
}
