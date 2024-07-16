using API.Data.Entidades.Seguridad;
using API.Domain.Validators.Seguridad;

namespace API.Domain.Interfaces.Seguridad
{
    public interface IRolPermisoService : IBaseService<RolPermiso, RolPermisoValidator>
    {
        Task EliminarPorRol(Guid id);
    }
}