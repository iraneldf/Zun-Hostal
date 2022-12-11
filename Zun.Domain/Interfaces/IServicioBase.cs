using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Datos.Entidades;

namespace Zun.Dominio.Interfaces
{
    public interface IServicioBase<TEntidad> where TEntidad : EntitidadBase
    {
        /// <summary>
        /// Modifica los datos del elemento con el mismo id pasado por parametro
        /// </summary>
        /// <param name="entidad">elemento a modificar</param>
        EntityEntry<TEntidad> Modificar(TEntidad entidad);
        /// <summary>
        /// Elimina un elemento
        /// </summary>
        /// <param name="entidad">elemento a eliminar</param>
        EntityEntry<TEntidad> Eliminar(TEntidad entidad);
        /// <summary>
        /// Modifica varias entidades
        /// </summary>
        /// <param name="entidades">elementos a modificar</param>
        void ModificarEntidades(List<TEntidad> entidades);
        /// <summary>
        /// Retorna el elemento que contiene el mismo id pasado por parametro
        /// </summary>
        /// <param name="id">id del elemento</param>
        Task<TEntidad?> ObtenerPorId(int id);
        /// <summary>
        /// Persiste los cambios en la Base de Datos
        /// </summary>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Retorna un listado paginado y la cantidad total de elementos
        /// </summary>
        /// <param name="cantIgnorar">Cantidad de elementos a ignorar</param>
        /// <param name="cantMaxResultados">Cantidad de elementos a retornar</param>
        /// <param name="filtros">Condiciones para el filtrado</param>
        Task<(IEnumerable<TEntidad>, int)> ObtenerListadoPaginado(int cantIgnorar, int? cantMaxResultados, params Expression<Func<TEntidad, bool>>[] filtros);
        /// <summary>
        /// Retorna todos los elementos
        /// </summary>
        Task<IEnumerable<TEntidad>> ObtenerTodos();
        /// <summary>
        /// Crea un elemento
        /// </summary>
        /// <param name="entidad">Elemento a crear</param>
        Task<EntityEntry<TEntidad>> Crear(TEntidad entidad);
        /// <summary>
        /// Elimina el elemento que contiene el mismo id pasado como parametro
        /// </summary>
        /// <param name="id">Id del elemento</param>
        Task<EntityEntry<TEntidad>> Eliminar(int id);
    }
}