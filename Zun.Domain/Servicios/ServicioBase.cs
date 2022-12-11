using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Data.Entidades;
using Zun.Data.IUnitOfWork.Interfaces;
using Zun.Domain.Interfaces;

namespace Zun.Domain.Servicios
{
    public class ServicioBase<TEntidad> : IServicioBase<TEntidad> where TEntidad : EntitidadBase
    {
        protected readonly IRepositorioBase<TEntidad> _repositorioBase;
        protected readonly IUnitOfWork _repositorios;

        public ServicioBase(IUnitOfWork repositorios, IRepositorioBase<TEntidad> repositorioBase)
        {
            _repositorioBase = repositorioBase;
            _repositorios = repositorios;
        }

        public virtual async Task<EntityEntry<TEntidad>> Crear(TEntidad entidad) => await _repositorioBase.AddAsync(entidad);
        public virtual EntityEntry<TEntidad> Eliminar(TEntidad entidad) => _repositorioBase.Remove(entidad);
        public virtual async Task<EntityEntry<TEntidad>> Eliminar(int id) => _repositorioBase.Remove(await ObtenerPorId(id));
        public virtual async Task<int> SaveChangesAsync() => await _repositorioBase.SaveChangesAsync();
        public virtual EntityEntry<TEntidad> Modificar(TEntidad entidad) => _repositorioBase.Update(entidad);
        public virtual void ModificarEntidades(List<TEntidad> entities) => _repositorioBase.UpdateRange(entities);
        public virtual async Task<TEntidad?> ObtenerPorId(int id) => await _repositorioBase.FirstAsync(entidad => entidad.Id == id);
        public virtual async Task<IEnumerable<TEntidad>> ObtenerTodos() => await _repositorioBase.GetAllAsync();


        protected virtual IQueryable<TEntidad> CreateQuery()
           => _repositorioBase.GetQuery();

        public virtual async Task<(IEnumerable<TEntidad>, int)> ObtenerListadoPaginado(int skipCount, int? maxResultCount, params Expression<Func<TEntidad, bool>>[] filters)
        {
            IQueryable<TEntidad> query = CreateQuery();

            //Filtering
            query = filters.Aggregate(query, (current, filters) => current.Where(filters));
            //Counting
            int total = await query.CountAsync();
            //Paginating
            query = query.Skip(skipCount).Take(maxResultCount.GetValueOrDefault(total));

            return (await query.ToListAsync(), total);
        }

    }
}