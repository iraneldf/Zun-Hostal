using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Validators.Seguridad;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Seguridad
{
    public class PermisoService : BasicService<Permiso, PermisoValidator>, IPermisoService
    {

        public PermisoService(IUnitOfWork<Permiso> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }
    }
}