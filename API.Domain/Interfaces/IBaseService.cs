using API.Data.Entidades;
using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace API.Domain.Interfaces
{
    public interface IBaseService<TEntity, TEntityValidator> where TEntity : EntidadBase where TEntityValidator : AbstractValidator<TEntity>
    {
        /// <summary>
        /// Actualiza los datos de un elemento
        /// </summary>
        /// <param name="entity">Elemento a actualizar</param>
        Task<EntityEntry<TEntity>> Actualizar(TEntity entity);
        /// <summary>
        /// Elimina un elemento
        /// </summary>
        /// <param name="id">Id del a eliminar</param>
        Task<EntityEntry<TEntity>> Eliminar(TEntity entity);
        /// <summary>
        /// Actualiza los datos de varios elementos
        /// </summary>
        /// <param name="entities">Elementos a actualizar</param>
        Task Actualizar(List<TEntity> entities);
        /// <summary>
        /// Obtener elemento por Id
        /// </summary>
        /// <param name="id">Id de elemento</param>
        Task<TEntity?> ObtenerPorId(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Persiste los cambios en BD
        /// </summary>
        Task<int> SalvarCambios();
        /// <summary>
        /// Obtener Listado Paginado
        /// </summary>
        /// <param name="cantidadIgnorar">cantidad de elementos a ignorar</param>
        /// <param name="cantidadMostrar">cantidad de elementos a mostrar</param>
        /// <param name="secuenciaOrdenamiento">secuencia de ordenamiento</param>
        /// <param name="filtros">filtros</param>
        Task<(IEnumerable<TEntity>, int)> ObtenerListadoPaginado(int cantidadIgnorar, int? cantidadMostrar, string? secuenciaOrdenamiento = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null, params Expression<Func<TEntity, bool>>[] filtros);
        /// <summary>
        /// Retorna todos los elementos
        /// </summary>
        Task<IEnumerable<TEntity>> ObtenerTodos(string? secuenciaOrdenamiento = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null);
        /// <summary>
        /// Agrega un nuevo elemento
        /// </summary>
        /// <param name="entity">elemento a crear</param>
        Task<EntityEntry<TEntity>> Crear(TEntity entity);
        /// <summary>
        /// Elimina un elemento
        /// </summary>
        /// <param name="id">Id del a eliminar</param>
        Task<EntityEntry<TEntity>> Eliminar(Guid id);
        /// <summary>
        /// Agrega varios elementos
        /// </summary>
        /// <param name="entities">elementos a crear</param>
        /// <returns></returns>
        Task Crear(List<TEntity> entities);
        /// <summary>
        /// Guarda un registro de la accion realizada por el usuario
        /// </summary>
        /// <param name="usuario"> usuario que realiza la accion</param>
        /// <param name="descripcion"> accion realizada por el usuario</param>
        /// <param name="tablaBD">nombre de la tabla utilizada</param>
        Task GuardarTraza(string? usuario, string descripcion, string tablaBD);
        /// <summary>
        /// Termina la transaccion sin persistir los cambios en BD
        /// </summary>
        Task RevertirTransaccion();
        /// <summary>
        /// Finaliza la transaccion
        /// </summary>
        Task FinalizarTransaccion();
        /// <summary>
        /// Incia una transaccion en la BD
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> IniciarTransaccion();
        /// <summary>
        /// Verifica que se cumplan las condiciones necesarias para crear un nuevo elemento
        /// </summary>
        /// <param name="entity">Elemento a validar</param>
        Task ValidarAntesCrear(TEntity entity);
        /// <summary>
        /// Verifica que se cumplan las condiciones necesarias para editar un elemento
        /// </summary>
        /// <param name="entity">Elemento a validar</param>
        Task ValidarAntesActualizar(TEntity entity);
        /// <summary>
        /// Verifica que se cumplan las condiciones necesarias para eliminar un elemento
        /// </summary>
        /// <param name="entity">Elemento a validar</param>
        Task ValidarAntesEliminar(Guid id);
        void ValidarPermisos(string permisos);
    }
}