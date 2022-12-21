using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Dominio.Interfaces;

namespace Zun.Dominio.Servicios
{
    public class ServicioBase<TEntidad> : IServicioBase<TEntidad> where TEntidad : EntitidadBase
    {
        protected readonly IRepositorioBase<TEntidad> _repositorioBase;
        protected readonly IUnitOfWork _repositorios;
        protected readonly IHttpContextAccessor _httpContext;

        public ServicioBase(IUnitOfWork repositorios, IRepositorioBase<TEntidad> repositorioBase, IHttpContextAccessor httpContext)
        {
            _repositorioBase = repositorioBase;
            _repositorios = repositorios;
            _httpContext = httpContext;
        }

        public virtual async Task<EntityEntry<TEntidad>> Crear(TEntidad entidad) => await _repositorioBase.AddAsync(EstablecerDatosAuditoria(entidad));
        public virtual async Task CrearEntidades(List<TEntidad> entidades) => await _repositorioBase.AddRangeAsync(EstablecerDatosAuditoria(entidades));
        public virtual EntityEntry<TEntidad> Eliminar(TEntidad entidad) => _repositorioBase.Remove(entidad);
        public virtual async Task<EntityEntry<TEntidad>> Eliminar(int id)
        {
            TEntidad? entidad = await ObtenerPorId(id);
            if (entidad == null)
                throw new Exception("Elemento no encontrado");
            return _repositorioBase.Remove(entidad);
        }
        public virtual async Task<int> SaveChangesAsync() => await _repositorioBase.SaveChangesAsync();
        public virtual EntityEntry<TEntidad> Modificar(TEntidad entidad) => _repositorioBase.Update(EstablecerDatosAuditoria(entidad, esNuevoElemento: false));
        public virtual void ModificarEntidades(List<TEntidad> entidades) => _repositorioBase.UpdateRange(EstablecerDatosAuditoria(entidades, esNuevoElemento: false));
        public virtual async Task<TEntidad?> ObtenerPorId(int id) => await _repositorioBase.FirstAsync(entidad => entidad.Id == id);
        public virtual async Task<IEnumerable<TEntidad>> ObtenerTodos() => await _repositorioBase.GetAllAsync();

        /// <summary>
        /// Establece los valores para los datos auditables de la entidad
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        private TEntidad EstablecerDatosAuditoria(TEntidad entidad, bool esNuevoElemento = true)
        {
            if (esNuevoElemento)
            {
                entidad.FechaCreacion = DateTime.Now;
                entidad.CreadoPor = _httpContext.HttpContext.User.Identity?.Name ?? String.Empty;
            }
            entidad.FechaModificacion = DateTime.Now;
            entidad.ModificadoPor = _httpContext.HttpContext.User.Identity?.Name ?? String.Empty;
            return entidad;
        }

        private static List<TEntidad> EstablecerDatosAuditoria(List<TEntidad> entidades, bool esNuevoElemento = true)
        {
            entidades.ForEach(entidad =>
            {
                if (esNuevoElemento)
                {
                    entidad.FechaCreacion = DateTime.Now;
                    entidad.CreadoPor = String.Empty;
                }
                entidad.FechaModificacion = DateTime.Now;
                entidad.ModificadoPor = String.Empty;
            });

            return entidades;
        }

        public async Task GuardarTraza(string? usuario, string descripcion, string tablaBD, object elemento, object? elementoModificado = null)
        {
            Traza traza = new() { 
                Descripcion = descripcion,
                TablaBD = tablaBD,
                Elemento = JsonConvert.SerializeObject(elemento),
                ElementoModificado = elementoModificado == null? String.Empty : JsonConvert.SerializeObject(elementoModificado),
                FechaCreacion = DateTime.Now,
                CreadoPor = usuario ?? String.Empty,
                FechaModificacion = DateTime.Now,
                ModificadoPor = usuario ?? String.Empty
            };
            await _repositorios.Trazas.AddAsync(traza);
        }

        protected virtual IQueryable<TEntidad> CreateQuery()
           => _repositorioBase.GetQuery();

        public virtual async Task<(IEnumerable<TEntidad>, int)> ObtenerListadoPaginado(int skipCount, int? maxResultCount, params Expression<Func<TEntidad, bool>>[] filters)
        {
            IQueryable<TEntidad> query = CreateQuery();

            //Filtrando
            query = filters.Aggregate(query, (current, filters) => current.Where(filters));
            //Ordenando
            query = query.OrderByDescending(e => e.Id);
            //Contando
            int total = await query.CountAsync();
            //Paginando
            query = query.Skip(skipCount).Take(maxResultCount.GetValueOrDefault(total));

            return (await query.ToListAsync(), total);
        }

    }
}