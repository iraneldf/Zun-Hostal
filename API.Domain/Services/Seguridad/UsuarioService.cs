using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Web.Helpers;

namespace API.Domain.Services.Seguridad
{
    public class UsuarioService : BasicService<Usuario, UsuarioValidator>, IUsuarioService
    {

        public UsuarioService(IUnitOfWork<Usuario> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public override async Task<EntityEntry<Usuario>> Crear(Usuario entity)
        {

            //encriptando contraseña
            //HashPassword genera una cadena aleatoria en cada llamada (aunque se le pase el mismo texto)
            //para comprobar el pass usar la funcion VerifyHashedPassword pasandole por parametros
            //el texto generado y el texto plano a verificar

            entity.Contrasenna = Crypto.HashPassword(entity.Contrasenna);

            await ValidarAntesCrear(entity);

            return await _repositorios.BasicRepository.AddAsync(base.EstablecerDatosAuditoria(entity));

        }

        public override async Task<EntityEntry<Usuario>> Actualizar(Usuario entity)
        {
            Usuario? usuario = await ObtenerPorId(entity.Id) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

            entity.Contrasenna = usuario.Contrasenna;
            entity.DebeCambiarContrasenna = usuario.DebeCambiarContrasenna;

            return await base.Actualizar(entity);
        }

        public async Task CambiarContrasenna(Guid usuarioId, string nuevaContrasenna, bool debeCambiarContrasenna = false)
        {
            Usuario? usuario = await ObtenerPorId(usuarioId) ??
                throw new CustomException() { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };

            usuario.Contrasenna = Crypto.HashPassword(nuevaContrasenna);
            usuario.DebeCambiarContrasenna = debeCambiarContrasenna;

            await base.Actualizar(usuario);
        }


        public override async Task ValidarAntesActualizar(Usuario usuario)
        {
            ///validando los datos insertados por el usuario
            await new UsuarioValidator(_repositorios).ValidateAndThrowAsync(usuario);

            ///validando reglas de negocios    

            //se debe validar que el elemento exista antes de actualizar,
            //porque si el elemento no existe porque fue eliminado entonces lo crea
            //(aqui esta comentado porque en el metodo de Actualizar ya se hace esta validacion para el caso de los usuarios)

            //if (!await _repositorios.BasicRepository.AnyAsync(e => e.Id == entity.Id))
            //    throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }
        public async Task<Usuario?> ObtenerPorUsername(string username, Func<IQueryable<Usuario>, IIncludableQueryable<Usuario, object>>? propiedadesIncluidas = null) => await _repositorios.BasicRepository.FirstAsync(entity => entity.Username == username, propiedadesIncluidas);

        public async Task<List<Permiso>> ObtenerPermisos(string username)
            => (await _repositorios.Usuarios.FirstAsync(e => e.Username == username, query=> query.Include(e=>e.Rol.RolPermiso).ThenInclude(e=>e.Permiso)))?.Rol.RolPermiso.Select(e => e.Permiso).ToList() ?? new();
    }
}