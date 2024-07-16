using API.Data.Entidades.Seguridad;
using API.Domain.Validators.Seguridad;

namespace API.Domain.Interfaces.Seguridad
{
    public interface IRolService : IBaseService<Rol, RolValidator>
    {
    }
}