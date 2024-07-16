using API.Application.Dtos.Comunes;
using API.Application.Dtos.Seguridad.Autenticacion;
using API.Application.Filters;
using API.Data.Entidades.Seguridad;
using API.Domain.Interfaces;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers.Seguridad
{

    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerFilter))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutenticacionController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly IAutenticacionService _autenticacionServicio;
        protected readonly IUsuarioService _usuarioService;
        protected readonly IBackgroundJobClient _clientHangfire;

        public AutenticacionController(IMapper mapper, IAutenticacionService autenticacionServicio, IBackgroundJobClient clientHangfire, IUsuarioService usuarioService)
        {
            _autenticacionServicio = autenticacionServicio;
            _mapper = mapper;
            _clientHangfire = clientHangfire;
            _usuarioService = usuarioService;
        }


        /// <summary>
        /// Retorna informacion del usuario logiado
        /// </summary>
        [HttpGet("[action]")]
        public ActionResult ObtenerInformacionUsuario()
            => Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = User.Identity?.Name });

        /// <summary>
        /// Inicia sesion de un usuario
        /// </summary>
        /// <response code="200">Retorna true si se cambio la contraseña</response>
        /// <response code="400">Retorna el mensaje del error ocurrido</response>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginInputDto login)
        {
            if (await _autenticacionServicio.Login(login.Username, login.Contrasenna))
            {
                (string token, DateTime fechaExpiracion) = await _autenticacionServicio.ConstruirToken(login.Username);

                await _usuarioService.GuardarTraza(login.Username, $"{login.Username} ha inciado sesion.", "Sesion");

                LoginOutputDto result = new() { FechaExpiracion = fechaExpiracion, Token = token };
                return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
            }
            else
                return Unauthorized(new ResponseDto { Status = StatusCodes.Status401Unauthorized, ErrorMessage = "Usuario o contraseña no válido." });
        }
    }
}
