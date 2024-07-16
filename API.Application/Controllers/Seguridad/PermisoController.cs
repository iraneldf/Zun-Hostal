using API.Application.Dtos.Comunes;
using API.Application.Filters;
using API.Data.Entidades.Seguridad;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Application.Controllers.Seguridad
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerFilter))]
    [Authorize]
    public class PermisoController : Controller
    {
        protected readonly IPermisoService _servicioPermiso;

        public PermisoController(IPermisoService servicioPermiso)
        {
            _servicioPermiso = servicioPermiso;
        }

        /// <summary>
        /// Retorna un listado para un Select
        /// </summary>
        /// <param name="inputDto">Datos de entrada</param>
        /// <response code="200">Completado con exito! </response>
        /// <response code="400">Ha ocurrido un error </response>
        [HttpGet("[action]")]
        public virtual async Task<IActionResult> ObtenerSelectList([FromQuery] ObtenerSelectListInputDto inputDto)
        {
            inputDto.NombreCampoTexto = typeof(Permiso).GetProperties().FirstOrDefault(e => e.Name.ToLower() == inputDto.NombreCampoTexto.ToLower())?.Name ?? string.Empty;
            inputDto.NombreCampoValor = typeof(Permiso).GetProperties().FirstOrDefault(e => e.Name.ToLower() == inputDto.NombreCampoValor.ToLower())?.Name ?? string.Empty;

            if (string.IsNullOrWhiteSpace(inputDto.NombreCampoValor) || string.IsNullOrWhiteSpace(inputDto.NombreCampoTexto))
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "Error en los nombres de los campos." };

            IEnumerable<Permiso> entities = await _servicioPermiso.ObtenerTodos(inputDto.SecuenciaOrdenamiento);

            SelectList selectList = new(entities, inputDto.NombreCampoValor, inputDto.NombreCampoTexto, inputDto.ValorSeleccionado);
            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = selectList });

        }
    }
}
