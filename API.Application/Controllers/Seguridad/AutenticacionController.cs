using API.Application.Dtos.Comunes;
using API.Application.Dtos.Seguridad.Autenticacion;
using API.Application.Filters;
using API.Data.Entidades.Seguridad;
using API.Domain.Interfaces.Seguridad;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Application.Controllers.Seguridad
{

    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerFilter))]
    [Authorize]
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
        public ActionResult Login()
        {
            string? authHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authHeader))
               return Unauthorized(new ResponseDto { Status = StatusCodes.Status401Unauthorized, ErrorMessage = "Error al iniciar sesión." });

            string token = authHeader.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            bool result = HttpContext.User.Identity?.IsAuthenticated ?? false;

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = result });
        }
    }
}
