﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Aplicacion.Dtos;
using Zun.Datos.Entidades;
using Zun.Dominio.Interfaces;

namespace Zun.Aplicacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ControladorBase<TEntidad, TEntidadDto, CrearDto, ModificarDto, ElementoListadoPaginadoDto, FiltroListadoPaginadoDto> : ControllerBase where TEntidad : EntitidadBase where TEntidadDto : EntitidadBaseDto where ModificarDto : EntitidadBaseDto where FiltroListadoPaginadoDto : ConfiguracionListadoPaginadoDto
    {
        protected readonly IMapper _mapper;
        protected readonly IServicioBase<TEntidad> _servicioBase;

        public ControladorBase(IMapper mapper, IServicioBase<TEntidad> servicioBase)
        {
            _servicioBase = servicioBase;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos los elementos
        /// </summary>
        /// <response code="200">Retorna listado de elementos</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        [HttpGet("[action]")]
        public virtual async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                IEnumerable<TEntidadDto> result = _mapper.Map<IEnumerable<TEntidadDto>>(await _servicioBase.ObtenerTodos());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo elemento 
        /// </summary>
        /// <param name="crearDto">elemento a crear</param>
        /// <response code="200">Retorna el elemento creado</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>

        [HttpPost("[action]")]
        public virtual async Task<IActionResult> Crear([FromBody] CrearDto crearDto)
        {
            try
            {
                TEntidad entity = _mapper.Map<TEntidad>(crearDto);
                EntityEntry<TEntidad> result = await _servicioBase.Crear(entity);
                await _servicioBase.SaveChangesAsync();

                TEntidadDto entityDto = _mapper.Map<TEntidadDto>(result.Entity);

                return Ok(entityDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Modifica los datos del elemento con el mismo id pasado por parametro
        /// </summary>
        /// <param name="id">id del elemento</param>
        /// <param name="modificarDto">elemento a modificar</param>
        /// <response code="200">Retorna el elemento modificado</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        [HttpPut("[action]/{id}")]
        public virtual async Task<IActionResult> Modificar(int id, ModificarDto modificarDto)
        {
            try
            {
                if (id != modificarDto.Id)
                    return BadRequest("Erro na atualização");

                TEntidad entity = _mapper.Map<TEntidad>(modificarDto);
                EntityEntry<TEntidad> result = _servicioBase.Modificar(entity);
                await _servicioBase.SaveChangesAsync();

                TEntidadDto entityDto = _mapper.Map<TEntidadDto>(result.Entity);

                return Ok(entityDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Retorna un listado paginado
        /// </summary>
        /// <param name="configuracionPaginadoDto">configuracion de paginacion</param>
        /// <response code="200">Retorna un listado paginado de elementos</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        [HttpGet("[action]")]
        public virtual async Task<IActionResult> ObtenerListadoPaginado([FromQuery] FiltroListadoPaginadoDto configuracionPaginadoDto)
        {
            try
            {
                (IEnumerable<TEntidad> result, int totalCount) = await AplicarFiltros(configuracionPaginadoDto);

                ListadoPaginadoDto<ElementoListadoPaginadoDto> pagedResultDto = new ListadoPaginadoDto<ElementoListadoPaginadoDto>
                {
                    Elementos = _mapper.Map<List<ElementoListadoPaginadoDto>>(result),
                    CantidadTotal = totalCount
                };

                return Ok(pagedResultDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        /// <summary>
        /// Retorna el elemento que contiene el mismo id pasado por parametro
        /// </summary>
        /// <param name="id">id del elemento</param>
        /// <response code="200">Retorna un elemento</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        /// <response code="404">Retorna el mensaje de error cuando no se encuentra el elemento</response>
        [HttpGet("[action]/{id}")]
        public virtual async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                TEntidad? result = await _servicioBase.ObtenerPorId(id);

                if (result == null)
                    return NotFound("Elemento no encontrado");

                TEntidadDto entityDto = _mapper.Map<TEntidadDto>(result);

                return Ok(entityDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Elimina el elemento que contiene el mismo id pasado por parametro
        /// </summary>
        /// <param name="id">id del elemento</param>
        /// <response code="200">Retorna un elemento eliminado</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>

        [HttpDelete("[action]")]
        public virtual async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                EntityEntry<TEntidad> result = await _servicioBase.Eliminar(id);
                await _servicioBase.SaveChangesAsync();

                TEntidadDto entityDto = _mapper.Map<TEntidadDto>(result.Entity);

                return Ok(entityDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Retorna todos los elementos para ser mostrados en un dropdown
        /// </summary>
        /// <param name="valorSeleccionado">Valor que debe mostrarse como seleccionado</param>
        /// <param name="nombreCampoTexto">Nombre del campo que se tomara para mostrar el texto del elemento</param>
        /// <param name="nombreCampoValor">Nombre del campo que se tomara para obtener el valor del elemento seleccionado</param>
        /// <response code="200">Retorna listao de elementos para incluirlos en un dropdown</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        [HttpGet("[action]")]
        public virtual async Task<IActionResult> ObtenetElementosParaDropdown([FromQuery] string nombreCampoValor, [FromQuery] string nombreCampoTexto, [FromQuery] object valorSeleccionado)
        {
            try
            {
                IEnumerable<TEntidad> entities = await _servicioBase.ObtenerTodos();

                SelectList selectList = new SelectList(entities, nombreCampoValor, nombreCampoTexto, valorSeleccionado);
                return Ok(selectList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }


        /// <summary>
        /// Retorna el elemento que contiene el id pasado como parametro.
        /// El elemento sera retornado solo con los campos que seran modificados
        /// </summary>
        /// <param name="id">Id del elemento</param>
        /// <response code="200">Retorna un elemento eliminado</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        /// <response code="404">Retorna el mensaje de error cuando no se encuentra el elemento</response>
        [HttpGet("GetForEdit/{id}")]
        public virtual async Task<IActionResult> ObetenerElementoParaEditar(int id)
        {
            try
            {
                TEntidad? result = await _servicioBase.ObtenerPorId(id);

                if (result == null)
                    return NotFound("Elemento no encontrado");

                ModificarDto entityDto = _mapper.Map<ModificarDto>(result);

                return Ok(entityDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Retorna un listado de elementos que han sido filtrados y la cantidad de elementos
        /// El elemento sera retornado solo con los campos que seran modificados
        /// </summary>
        /// <param name="filtro">filtro y configuracion para el paginado</param>
        protected virtual Task<(IEnumerable<TEntidad>, int)> AplicarFiltros(FiltroListadoPaginadoDto filtro)
        {
            return _servicioBase.ObtenerListadoPaginado(filtro.CantIgnorar, filtro.CantMostrar);
        }

    }
}
