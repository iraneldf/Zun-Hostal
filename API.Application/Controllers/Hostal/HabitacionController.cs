using System.Linq.Expressions;
using API.Application.Dtos.Comunes;
using API.Application.Dtos.Hostal.Habitacion;
using API.Data.Entidades.Hostal;
using API.Data.Entidades.Hostal.Enums;
using API.Domain.Interfaces.Hostal;
using API.Domain.Validators.Hostal;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers.Hostal;

public class HabitacionController : BasicReadOnlyController<Habitacion, HabitacionValidator, HabitacionDto,
    ListadoPaginadoHabitacionDto, FiltrarConfigurarListadoPaginadoHabitacionIntputDto>
{
    private readonly IHabitacionService _habitacionService;

    public HabitacionController(IMapper mapper, IHabitacionService servicioHabitacion, IHttpContextAccessor httpContext,
        IHabitacionService habitacionService) : base(mapper, servicioHabitacion, httpContext)
    {
        _habitacionService = habitacionService;
    }

    protected override Task<(IEnumerable<Habitacion>, int)> AplicarFiltrosIncluirPropiedades(
        FiltrarConfigurarListadoPaginadoHabitacionIntputDto inputDto)
    {
        //agregando filtros
        //buscar
        List<Expression<Func<Habitacion, bool>>> filtros = new();
        // if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
        //     filtros.Add(Habitacion => Habitacion.Nombre.Contains(inputDto.TextoBuscar) ||
        //                            Habitacion.Apellidos.Contains(inputDto.TextoBuscar) ||
        //                            Habitacion.CI.Contains(inputDto.TextoBuscar) ||
        //                            Habitacion.Telefono.Contains(inputDto.TextoBuscar));
        //filtrar
        if (inputDto.Estado != null)
            filtros.Add(habitacion => habitacion.Estado == inputDto.Estado);

        return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar,
            inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
    }

    /// <summary>
    ///     Obtener listado De habitaciones por ama de llaves
    /// </summary>
    /// <param name="amaDeLlavesId">filtro</param>
    /// <response code="200">Completado con exito!</response>
    /// <response code="400">Ha ocurrido un error</response>
    [HttpGet("[action]")]
    public virtual async Task<IActionResult> ObtenerHabitacionesPorAmaDeLlaves([FromQuery] Guid amaDeLlavesId)
    {

        var listado = await _habitacionService.ObtenerHabitacionesPorAmaDeLlaves(amaDeLlavesId);

        var lisresult = listado.Select(x =>
            new HabitacionDto()
            {
                Id = x.Id,
                Numero = x.Numero,
                Estado = x.Estado
            }
        ).ToList();

        return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = lisresult });
    }

    public IHabitacionService HabitacionService => _habitacionService;

    /// <summary>
    ///     Poner una habitacion Fuera de Servicio
    /// </summary>
    /// <param name="habitacionId">HabitacionId</param>
    /// <response code="200">Completado con exito!</response>
    /// <response code="400">Ha ocurrido un error</response>
    [HttpGet($"[action]")]
    public virtual async Task<IActionResult> PonerFueraDeServicio([FromQuery] Guid habitacionId)
    {
        var result = await _habitacionService.PonerFuerdaDeServicio(habitacionId);

        var resultDto =
            new HabitacionDto
            {
                Estado = result.Estado,
                Numero = result.Numero,
                Id = result.Id
            };

        return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = resultDto });
    }

    /// <summary>
    ///     Cambia el estado de una habitacion
    /// </summary>
    /// <param name="habitacionId">HabitacionId</param>
    /// <param name="nevoEstado">Estado nuevo</param>
    /// <response code="200">Completado con exito!</response>
    /// <response code="400">Ha ocurrido un error</response>
    [HttpGet("[action]")]
    public virtual async Task<IActionResult> CambiarEstado([FromQuery] Guid habitacionId,
        [FromQuery] EstadoHabitacion nevoEstado)
    {
        var result = await _habitacionService.CambiarEstado(habitacionId, nevoEstado);


        var resultDto =
            new HabitacionDto
            {
                Estado = result.Estado,
                Numero = result.Numero,
                Id = result.Id
            };

        return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = resultDto });
    }

    /// <summary>
    ///     Mostrar un listado de Habitaciones disponibles en un periodo de fecha
    /// </summary>
    /// <param name="fechaInicio">fechaInicio</param>
    /// <param name="fechaFin">fechaFin</param>
    /// <response code="200">Completado con exito!</response>
    /// <response code="400">Ha ocurrido un error</response>
    [HttpGet("[action]")]
    public virtual async Task<IActionResult> ListaDeHabitacionesDisponiblesPorFechaService(
        [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        var result = await _habitacionService.ListaDeHabitacionesDisponiblesPorFechaService(fechaInicio, fechaFin);

        var resultDto = result.Select(
            r =>
                new HabitacionDto
                {
                    Numero = r.Numero,
                    Estado = r.Estado,
                    Id = r.Id,

                });

        return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = resultDto });
    }
}
