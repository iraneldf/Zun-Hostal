using API.Application.Dtos.Comunes;
using API.Application.Filters;
using API.Data.Entidades;
using API.Domain.Exceptions;
using API.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerFilter))]
    [Authorize]
    public class BasicController<TEntity, TEntityValidator, TEntityDto, CrearDto, ActualizarDto, ElementoListadoPaginadoDto, FiltrarConfigurarListadoPaginadoDto> : ControllerBase where TEntity : EntidadBase where TEntityValidator : AbstractValidator<TEntity> where TEntityDto : EntidadBaseDto where ActualizarDto : EntidadBaseDto where FiltrarConfigurarListadoPaginadoDto : ConfiguracionListadoPaginadoDto
    {
        protected string? usuario;
        protected readonly IMapper _mapper;
        protected readonly IBaseService<TEntity, TEntityValidator> _servicioBase;

        public BasicController(IMapper mapper, IBaseService<TEntity, TEntityValidator> servicioBase)
        {
            _servicioBase = servicioBase;
            _mapper = mapper;
            usuario = User?.Identity?.Name;
        }


        /// <summary>
        /// Obtener todos los elementos
        /// </summary>
        /// <param name="secuenciaOrdenamiento">Secuencia de ordenamiento para ordenar el listado.
        /// FORMATO: Campo1:(asc/desc),Campo2:(asc/desc),...</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        [HttpGet("[action]")]
        protected virtual async Task<IActionResult> ObtenerTodos(string? secuenciaOrdenamiento = null)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            IEnumerable<TEntityDto> result = await ObtenerTodosElementos(secuenciaOrdenamiento);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }


        /// <summary>
        /// Crea un nuevo elemento
        /// </summary>
        /// <param name="crearDto">Elemento a crear</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        [HttpPost("[action]")]
        public virtual async Task<IActionResult> Crear([FromBody] CrearDto crearDto)
        {
            _servicioBase.ValidarPermisos("gestionar");

            TEntity entity = _mapper.Map<TEntity>(crearDto);

            try
            {
                //se usa transaccion para asegurarse que el elemento se guarde
                //y que la traza guarde el id del elemento creado
                await _servicioBase.IniciarTransaccion();
                TEntityDto entityDto = await CrearElemento(entity);
                await _servicioBase.FinalizarTransaccion();

                return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = entityDto });
            }
            catch (Exception)
            {
                await _servicioBase.RevertirTransaccion();
                throw;
            }

        }


        /// <summary>
        /// Editar un elemento
        /// </summary>
        /// <param name="id">Id del elemento</param>
        /// <param name="actualizarDto">Elemento a editar</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPut("[action]/{id}")]
        public virtual async Task<IActionResult> Actualizar(Guid id, ActualizarDto actualizarDto)
        {
            if (id != actualizarDto.Id)
                return BadRequest(new ResponseDto { Status = StatusCodes.Status400BadRequest, ErrorMessage = "Error al actualizar" });

            _servicioBase.ValidarPermisos("gestionar");

            TEntity entity = _mapper.Map<TEntity>(actualizarDto);
            TEntityDto entityDto = await ActualizarElemento(entity);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = entityDto });
        }


        /// <summary>
        /// Obtener listado paginado
        /// </summary>
        /// <param name="filtrarDto">filtro</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        [HttpGet("[action]")]
        public virtual async Task<IActionResult> ObtenerListadoPaginado([FromQuery] FiltrarConfigurarListadoPaginadoDto filtrarDto)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            (IEnumerable<TEntity> listado, int cantidad) = await AplicarFiltrosIncluirPropiedades(filtrarDto);

            ListadoPaginadoDto<ElementoListadoPaginadoDto> listadoPaginadoDto = new()
            {
                Elementos = _mapper.Map<List<ElementoListadoPaginadoDto>>(listado),
                Cantidad = cantidad
            };

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = listadoPaginadoDto });
        }


        /// <summary>
        /// Obtener elemento por Id
        /// </summary>
        /// <param name="id">element id</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpGet("[action]/{id}")]
        public virtual async Task<IActionResult> ObtenerPorId(Guid id)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            TEntity? entity = await ObtenerElementoPorId(id);

            if (entity == null)
                return NotFound(new ResponseDto { Status = StatusCodes.Status404NotFound, ErrorMessage = "Elemento no encontrado" });

            TEntityDto entityDto = _mapper.Map<TEntityDto>(entity);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = entityDto });
        }


        /// <summary>
        /// Eliminar Elemento
        /// </summary>
        /// <param name="id">Id del elemento</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>

        [HttpDelete("[action]/{id}")]
        public virtual async Task<IActionResult> Eliminar(Guid id)
        {
            _servicioBase.ValidarPermisos("gestionar");

            EntityEntry<TEntity> entity = await EliminarElemento(id);

            TEntityDto entityDto = _mapper.Map<TEntityDto>(entity.Entity);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = entityDto });
        }

        /// <summary>
        /// Retorna un listado para un Select
        /// </summary>
        /// <param name="inputDto">Datos de entrada</param>
        /// <response code="200">Completado con exito! </response>
        /// <response code="400">Ha ocurrido un error </response>
        [HttpGet("[action]")]
        protected virtual async Task<IActionResult> ObtenerSelectList([FromQuery] ObtenerSelectListInputDto inputDto)
        {
            inputDto.NombreCampoTexto = typeof(TEntity).GetProperties().FirstOrDefault(e => e.Name.ToLower() == inputDto.NombreCampoTexto.ToLower())?.Name ?? String.Empty;
            inputDto.NombreCampoValor = typeof(TEntity).GetProperties().FirstOrDefault(e => e.Name.ToLower() == inputDto.NombreCampoValor.ToLower())?.Name ?? String.Empty;

            if (string.IsNullOrWhiteSpace(inputDto.NombreCampoValor) || string.IsNullOrWhiteSpace(inputDto.NombreCampoTexto))
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "Error en los nombres de los campos." };

            IEnumerable<TEntity> entities = await _servicioBase.ObtenerTodos(inputDto.SecuenciaOrdenamiento);

            SelectList selectList = new(entities, inputDto.NombreCampoValor, inputDto.NombreCampoTexto, inputDto.ValorSeleccionado);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = selectList });

        }

        /// <summary>
        /// Aplicar Filtros a la lista e incluir las propiedades navigacionales
        /// </summary>
        /// <param name="configuracionListado">Configuracion del listado paginado</param>
        protected virtual async Task<(IEnumerable<TEntity>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoDto configuracionListado) =>
             await _servicioBase.ObtenerListadoPaginado(configuracionListado.CantidadIgnorar, configuracionListado.CantidadMostrar, configuracionListado.SecuenciaOrdenamiento);

        protected virtual async Task<IEnumerable<TEntityDto>> ObtenerTodosElementos(string? secuenciaOrdenamiento = null) =>
             _mapper.Map<IEnumerable<TEntityDto>>(await _servicioBase.ObtenerTodos(secuenciaOrdenamiento));

        protected virtual async Task<TEntityDto> ActualizarElemento(TEntity entity)
        {
            EntityEntry<TEntity> result = await _servicioBase.Actualizar(entity);

            await _servicioBase.GuardarTraza(usuario, $"Actualizado elemento con id = {result.Entity.Id} en la tabla {typeof(TEntity).Name}s", typeof(TEntity).Name);
            await _servicioBase.SalvarCambios();

            return _mapper.Map<TEntityDto>(result.Entity);
        }

        protected virtual async Task<TEntityDto> CrearElemento(TEntity entity)
        {
            EntityEntry<TEntity> result = await _servicioBase.Crear(entity);
            await _servicioBase.SalvarCambios();

            await _servicioBase.GuardarTraza(usuario, $"Un nuevo elemento con id = {result.Entity.Id} fue creado en la tabla {typeof(TEntity).Name}s", typeof(TEntity).Name);
            await _servicioBase.SalvarCambios();

            return _mapper.Map<TEntityDto>(result.Entity);
        }

        protected virtual async Task<TEntity?> ObtenerElementoPorId(Guid id)
        {
            return await _servicioBase.ObtenerPorId(id);
        }

        protected virtual async Task<EntityEntry<TEntity>> EliminarElemento(Guid id)
        {
            EntityEntry<TEntity> result = await _servicioBase.Eliminar(id);

            await _servicioBase.GuardarTraza(usuario, $"Eliminado elemento con id = {id} en la tabla {typeof(TEntity).Name}s", typeof(TEntity).Name);
            await _servicioBase.SalvarCambios();
            return result;
        }
    }
}
