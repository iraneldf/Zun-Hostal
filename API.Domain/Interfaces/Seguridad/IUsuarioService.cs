using API.Data.Entidades.Seguridad;
using API.Domain.Validators.Seguridad;
using Microsoft.EntityFrameworkCore.Query;

namespace API.Domain.Interfaces.Seguridad
{
    public interface IUsuarioService : IBaseService<Usuario, UsuarioValidator>
    {
        Task CambiarContrasenna(Guid usuarioId, string contrasenna, bool debeCambiarContrasenna = false);
        Task<List<Permiso>> ObtenerPermisos(string username);
        Task<Usuario?> ObtenerPorUsername(string username, Func<IQueryable<Usuario>, IIncludableQueryable<Usuario, object>>? propiedadesIncluidas = null);
    }
}