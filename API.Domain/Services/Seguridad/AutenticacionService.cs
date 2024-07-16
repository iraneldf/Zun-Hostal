using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Seguridad;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Helpers;

namespace API.Domain.Services.Seguridad
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AutenticacionService(IConfiguration configuration, IUsuarioService usuarioService)
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
        }


        public async Task<bool> Login(string username, string contrasenna)
        {
            Usuario usuario = await _usuarioService.ObtenerPorUsername(username) ??
                throw new CustomException { Status = StatusCodes.Status401Unauthorized, Message = "Usuario o contraseña no válido." };
          
            if(usuario.DebeCambiarContrasenna)
                throw new CustomException { Status = StatusCodes.Status307TemporaryRedirect, Message = "El usuario debe cambiar la contraseña." };
          
            return Crypto.VerifyHashedPassword(usuario.Contrasenna, contrasenna);
        }


        public async Task<(string, DateTime)> ConstruirToken(string username)
        {
            //creando claims
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //cargando permisos del usuario
            List<Permiso> tareas = await _usuarioService.ObtenerPermisos(username);

            //agregando permisos a los claims
            foreach (var tarea in tareas)
                claims.Add(new Claim(tarea.Nombre.ToLower(), tarea.Nombre.ToLower()));

            //construyendo token
            var llaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"] ?? "APSKP3KP4234KP2423K4P234K2P34K23P4K234K23423K42P3"));
            var credenciales = new SigningCredentials(llaveSecreta, SecurityAlgorithms.HmacSha256);
            var fechaExpiracion = DateTime.UtcNow.AddHours(double.Parse(_configuration["ValidationParameters:TimeSpan"] ?? "66565"));

            JwtSecurityToken token = new(
                issuer: _configuration["ValidationParameters:Issuer"],
                audience: _configuration["ValidationParameters:Audience"],
                claims: claims,
                expires: fechaExpiracion,
                signingCredentials: credenciales
                );

            return (new JwtSecurityTokenHandler().WriteToken(token), fechaExpiracion);
        }


    }
}