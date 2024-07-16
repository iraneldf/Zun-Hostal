using API.Application.Dtos.Comunes;
using API.Application.Dtos.Seguridad.Rol;
using API.Data.Entidades.Seguridad;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Application.Controllers.Seguridad
{

    public class RolController : BasicController<Rol, RolValidator, DetallesRolDto, CrearRolInputDto, ActualizarRolInputDto, ListadoPaginadoRolDto, FiltrarConfigurarListadoPaginadoRolIntputDto>
    {
        private readonly IRolPermisoService _rolPermisoService;

        public RolController(IMapper mapper, IRolService servicioRol, IRolPermisoService rolPermisoService) : base(mapper, servicioRol)
        {
            _rolPermisoService = rolPermisoService;
        }

        /// <summary>
        /// Retorna un listado para un Select
        /// </summary>
        /// <param name="inputDto">Datos de entrada</param>
        /// <response code="200">Completado con exito! </response>
        /// <response code="400">Ha ocurrido un error </response>
        [HttpGet("[action]")]
        public new async Task<IActionResult> ObtenerSelectList([FromQuery] ObtenerSelectListInputDto inputDto)
            => await base.ObtenerSelectList(inputDto);


        /// <summary>
        /// Editar un elemento
        /// </summary>
        /// <param name="id">Id del elemento</param>
        /// <param name="actualizarDto">Elemento a editar</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPut("[action]/{id}")]
        public override async Task<IActionResult> Actualizar(Guid id, ActualizarRolInputDto actualizarDto)
        {
            await _rolPermisoService.EliminarPorRol(id);
            return await base.Actualizar(id, actualizarDto);
        }

        protected override Task<(IEnumerable<Rol>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoRolIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Rol, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
                filtros.Add(Rol => Rol.Nombre.Contains(inputDto.TextoBuscar));

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        protected override async Task<Rol?> ObtenerElementoPorId(Guid id)
            => await _servicioBase.ObtenerPorId(id, propiedadesIncluidas: query => query.Include(e => e.RolPermiso).ThenInclude(e => e.Permiso));

        protected override async Task<IEnumerable<DetallesRolDto>> ObtenerTodosElementos(string? secuenciaOrdenamiento = null)
            => _mapper.Map<IEnumerable<DetallesRolDto>>(await _servicioBase.ObtenerTodos(secuenciaOrdenamiento, propiedadesIncluidas: query => query.Include(e => e.RolPermiso).ThenInclude(e => e.Permiso)));
    }
}
