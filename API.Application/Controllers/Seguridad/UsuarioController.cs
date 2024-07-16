using API.Application.Dtos.Comunes;
using API.Application.Dtos.Seguridad.Usuario;
using API.Application.Validadotors.Seguridad;
using API.Data.Entidades.Seguridad;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Web.Helpers;

namespace API.Application.Controllers.Seguridad
{
    public class UsuarioController : BasicController<Usuario, UsuarioValidator, DetallesUsuarioDto, CrearUsuarioInputDto, ActualizarUsuarioInputDto, ListadoPaginadoUsuarioDto, FiltrarConfigurarListadoPaginadoUsuarioIntputDto>
    {

        public UsuarioController(IMapper mapper, IUsuarioService servicioUsuario) : base(mapper, servicioUsuario)
        {
        }

        /// <summary>
        /// Cambiar contraseña de un usuario
        /// </summary>
        /// <param name="cambiarContrasennaDto">Elemento a editar</param>
        /// <response code="200">Completado con exito!</response>
        /// <response code="400">Ha ocurrido un error</response>
        /// <response code="404">Elemento no encontrado</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> CambiarContrasenna(CambiarContrasennaInputDto cambiarContrasennaDto)
        {
            await new CambiarContrasennaDtoValidator().ValidateAndThrowAsync(cambiarContrasennaDto);

            //si no se inserta la contraseña antigua es porque el endpoint lo esta llamando un administrador de usuarios
            if (string.IsNullOrWhiteSpace(cambiarContrasennaDto.ContrasennaAntigua))
            {
                _servicioBase.ValidarPermisos("gestionar");
                await ((IUsuarioService)_servicioBase).CambiarContrasenna(cambiarContrasennaDto.UsuarioId, cambiarContrasennaDto.NuevaContrasenna, true);
            }
            else
            {
                Usuario? usuario = await _servicioBase.ObtenerPorId(cambiarContrasennaDto.UsuarioId) ?? throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

                if (User.Identity?.Name == usuario.Username)
                    throw new CustomException { Status = StatusCodes.Status401Unauthorized, Message = "El usuario no tiene permisos para realizar esta acción." };

                if (!Crypto.VerifyHashedPassword(usuario.Contrasenna, cambiarContrasennaDto.ContrasennaAntigua))
                    throw new CustomException { Status = StatusCodes.Status500InternalServerError, Message = "La contraseña antigua es incorrecta." };

                await ((IUsuarioService)_servicioBase).CambiarContrasenna(cambiarContrasennaDto.UsuarioId, cambiarContrasennaDto.NuevaContrasenna);
            }

            await _servicioBase.GuardarTraza(usuario, $"Se ha camibado la contraseña para del usuario con id = {cambiarContrasennaDto.UsuarioId}", typeof(Usuario).Name);
            await _servicioBase.SalvarCambios();

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK });
        }

        protected override Task<(IEnumerable<Usuario>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoUsuarioIntputDto inputDto)
        {
            //agregando filtros
            List<Expression<Func<Usuario, bool>>> filtros = new();
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(usuario => usuario.Nombre.Contains(inputDto.TextoBuscar) ||
                                       usuario.Apellidos.Contains(inputDto.TextoBuscar) ||
                                       usuario.Correo.Contains(inputDto.TextoBuscar) ||
                                       usuario.Username.Contains(inputDto.TextoBuscar));
            }

            //IIncludableQueryable<Usuario, object> propiedadesIncluidas(IQueryable<Usuario> query) => query.Include(e => e.ShipmentItems);

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        protected override async Task<Usuario?> ObtenerElementoPorId(Guid id) 
            => await _servicioBase.ObtenerPorId(id, propiedadesIncluidas: query => query.Include(e => e.Rol));

        protected override async Task<IEnumerable<DetallesUsuarioDto>> ObtenerTodosElementos(string? secuenciaOrdenamiento = null) 
            => _mapper.Map<IEnumerable<DetallesUsuarioDto>>(await _servicioBase.ObtenerTodos(secuenciaOrdenamiento, propiedadesIncluidas: query => query.Include(e => e.Rol)));
    }
}
