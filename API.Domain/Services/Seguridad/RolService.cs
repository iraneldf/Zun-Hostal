using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Seguridad
{
    public class RolService : BasicService<Rol, RolValidator>, IRolService
    {

        public RolService(IUnitOfWork<Rol> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }


        public override async Task ValidarAntesCrear(Rol Rol)
        {
            ///validando los datos insertados por el Rol
            await new RolValidator(_repositorios).ValidateAndThrowAsync(Rol);
        }


        public override async Task ValidarAntesActualizar(Rol Rol)
        {
            ///validando los datos insertados por el Rol
            await new RolValidator(_repositorios).ValidateAndThrowAsync(Rol);

            ///validando reglas de negocios    

            //se debe validar que el elemento exista antes de actualizar,
            //porque si el elemento no existe porque fue eliminado entonces lo crea
            //(aqui esta comentado porque en el metodo de Actualizar ya se hace esta validacion para el caso de los Rols)

            //if (!await _repositorios.BasicRepository.AnyAsync(e => e.Id == entity.Id))
            //    throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Elemento no encontrado." };
        }

    }
}