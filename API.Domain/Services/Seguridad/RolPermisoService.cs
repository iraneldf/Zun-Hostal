using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Seguridad
{
    public class RolPermisoService : BasicService<RolPermiso, RolPermisoValidator>, IRolPermisoService
    {

        public RolPermisoService(IUnitOfWork<RolPermiso> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }

        public async Task EliminarPorRol(Guid rolId)
        {
            var rolesPermisos = await _repositorios.RolesPermisos.GetAllAsync(e => e.RolId == rolId);
            _repositorios.RolesPermisos.RemoveRange(rolesPermisos);
        }
    }
}