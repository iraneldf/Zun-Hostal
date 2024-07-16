using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Extensions;
using API.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace API.Domain.Services
{
    public class BasicService<TEntity, TEntityValidator> : IBaseService<TEntity, TEntityValidator> where TEntity : EntidadBase where TEntityValidator : AbstractValidator<TEntity>
    {
        protected readonly IUnitOfWork<TEntity> _repositorios;
        protected readonly IHttpContextAccessor _httpContext;

        public BasicService(IUnitOfWork<TEntity> repositories, IHttpContextAccessor httpContext)
        {
            _repositorios = repositories;
            _httpContext = httpContext;
        }


        public virtual async Task<IDbContextTransaction> IniciarTransaccion() => await _repositorios.BasicRepository.StartTransaction();

        public virtual async Task FinalizarTransaccion() => await _repositorios.BasicRepository.CommitTransaction();

        public virtual async Task RevertirTransaccion() => await _repositorios.BasicRepository.RollbackTransaction();

        public virtual async Task<EntityEntry<TEntity>> Crear(TEntity entity)
        {
            await ValidarAntesCrear(entity);
            return await _repositorios.BasicRepository.AddAsync(EstablecerDatosAuditoria(entity));
        }

        public virtual async Task Crear(List<TEntity> entities)
        {
            foreach (var entity in entities)
                await ValidarAntesCrear(entity);

            await _repositorios.BasicRepository.AddRangeAsync(EstablecerDatosAuditoria(entities));
        }

        public virtual async Task<EntityEntry<TEntity>> Eliminar(TEntity entity)
        {
            await ValidarAntesEliminar(entity.Id);
            return _repositorios.BasicRepository.Remove(entity);
        }

        public virtual async Task<EntityEntry<TEntity>> Eliminar(Guid id)
        {
            await ValidarAntesEliminar(id);

            TEntity? entity = await ObtenerPorId(id) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

            return _repositorios.BasicRepository.Remove(entity);
        }

        public virtual async Task<int> SalvarCambios() => await _repositorios.BasicRepository.SaveChangesAsync();

        public virtual async Task<EntityEntry<TEntity>> Actualizar(TEntity entity)
        {
            await ValidarAntesActualizar(entity);
            return _repositorios.BasicRepository.Update(EstablecerDatosAuditoria(entity, esEntidadNueva: false));
        }

        public virtual async Task Actualizar(List<TEntity> entities)
        {
            foreach (var entity in entities)
                await ValidarAntesActualizar(entity);

            _repositorios.BasicRepository.UpdateRange(EstablecerDatosAuditoria(entities, esEntidadNueva: false));
        }

        public virtual async Task<TEntity?> ObtenerPorId(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null) => await _repositorios.BasicRepository.FirstAsync(entity => entity.Id == id, propiedadesIncluidas);

        public virtual async Task<IEnumerable<TEntity>> ObtenerTodos(string? secuenciaOrdenamiento = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null)
        {
            IQueryable<TEntity> query = _repositorios.BasicRepository.GetQuery(propiedadesIncluidas);

            return await OrdenarLista(query, secuenciaOrdenamiento).ToListAsync();
        }

        /// <summary>
        /// Establecer Datos de Auditoria
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected TEntity EstablecerDatosAuditoria(TEntity entity, bool esEntidadNueva = true)
        {
            if (esEntidadNueva)
                entity.CreadoPor = _httpContext?.HttpContext.User.Identity?.Name ?? string.Empty;

            entity.ActualizadoPor = _httpContext?.HttpContext.User.Identity?.Name ?? string.Empty;
            return entity;
        }

        protected List<TEntity> EstablecerDatosAuditoria(List<TEntity> entities, bool esEntidadNueva = true)
        {
            entities.ForEach(entity =>
            {
                if (esEntidadNueva)
                    entity.CreadoPor = _httpContext?.HttpContext.User.Identity?.Name ?? string.Empty;

                entity.ActualizadoPor = _httpContext?.HttpContext.User.Identity?.Name ?? string.Empty;
            });

            return entities;
        }

        public async Task GuardarTraza(string? usuario, string descripcion, string tablaBD)
        {
            Traza history = new()
            {
                Descripcion = descripcion,
                TablaBD = tablaBD,
                CreadoPor = usuario ?? string.Empty,
                ActualizadoPor = usuario ?? string.Empty
            };

            await _repositorios.Trazas.Crear(history);
        }

        public virtual async Task<(IEnumerable<TEntity>, int)> ObtenerListadoPaginado(int cantidadIgnorar, int? cantidadMostrar, string? secuenciaOrdenamiento, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? propiedadesIncluidas = null, params Expression<Func<TEntity, bool>>[] filtros)
        {
            if (cantidadIgnorar < 0)
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "La cantidad de elementos a ignorar debe ser mayor o igual a 0." };
            if (cantidadMostrar.HasValue && cantidadMostrar.Value <= 0)
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "La cantidad de elementos a mostrar debe ser mayor a 0." };

            IQueryable<TEntity> query = _repositorios.BasicRepository.GetQuery(propiedadesIncluidas);

            //Filtrando
            query = filtros.Aggregate(query, (current, filters) => current.Where(filters));
            //Ordenando
            query = OrdenarLista(query, secuenciaOrdenamiento);
            //Counting
            int total = await query.CountAsync();
            //Paginando
            query = query.Skip(cantidadIgnorar).Take(cantidadMostrar.GetValueOrDefault(total));

            return (await query.ToListAsync(), total);
        }

        public virtual async Task ValidarAntesCrear(TEntity entity)
        {
            //llamando al validador correspondiente 
            Type? objectType = Type.GetType(typeof(TEntityValidator).AssemblyQualifiedName ?? string.Empty);
            await (Activator.CreateInstance(objectType, _repositorios) as AbstractValidator<TEntity>).ValidateAndThrowAsync(entity);
        }

        public virtual async Task ValidarAntesActualizar(TEntity entity)
        {
            //llamando al validador correspondiente 
            Type? objectType = Type.GetType(typeof(TEntityValidator).AssemblyQualifiedName ?? string.Empty);
            await (Activator.CreateInstance(objectType, _repositorios) as AbstractValidator<TEntity>).ValidateAndThrowAsync(entity);
        }

        public virtual async Task ValidarAntesEliminar(Guid id)
        {
            if (!await _repositorios.BasicRepository.AnyAsync(e => e.Id == id))
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }

        public void ValidarPermisos(string permisos)
        {
            if (false)
                throw new CustomException { Status = StatusCodes.Status401Unauthorized, Message = "El usuario no tiene permisos para realizar esta acción" };
        }

        private IOrderedQueryable<TEntity> OrdenarLista(IQueryable<TEntity> query, string? secuenciaOrdenamiento)
        {
            try
            {
                IOrderedQueryable<TEntity>? queryOrdenada = null;
                if (!string.IsNullOrWhiteSpace(secuenciaOrdenamiento))
                {
                    Dictionary<string, string> dictionarioOrdenamiento = new();
                    foreach (var item in secuenciaOrdenamiento.Split(',', StringSplitOptions.RemoveEmptyEntries))
                        dictionarioOrdenamiento.Add(item.Split(':', StringSplitOptions.RemoveEmptyEntries)[0], item.Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);

                    if (dictionarioOrdenamiento.Any())
                    {
                        if (dictionarioOrdenamiento.First().Value == "asc")
                            queryOrdenada = query.OrderByColumn(dictionarioOrdenamiento.First().Key);
                        else if (dictionarioOrdenamiento.First().Value == "desc")
                            queryOrdenada = query.OrderByColumnDescending(dictionarioOrdenamiento.First().Key);
                        else throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Error al ordenar; verifique la secuencia de ordenamiento." };


                        for (int i = 1; i < dictionarioOrdenamiento.Count;)
                        {
                            if (dictionarioOrdenamiento.ElementAt(i).Value == "asc")
                                queryOrdenada = queryOrdenada.ThenByColumn(dictionarioOrdenamiento.ElementAt(i).Key);
                            else if (dictionarioOrdenamiento.ElementAt(i).Value == "desc")
                                queryOrdenada = queryOrdenada.ThenByColumnDescending(dictionarioOrdenamiento.ElementAt(i).Key);
                            else throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Error al ordenar; verifique la secuencia de ordenamiento." };

                            if (i + 1 == dictionarioOrdenamiento.Count)
                                break;
                            i++;
                        }
                    }
                }

                return queryOrdenada ?? query.OrderByDescending(e => e.Id);
            }
            catch (Exception)
            {
                throw new CustomException() { Status = StatusCodes.Status400BadRequest, Message = "Error al ordenar; verifique la secuenncia de ordenamiento." };
            }
        }
    }
}