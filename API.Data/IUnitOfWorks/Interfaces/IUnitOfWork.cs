using API.Data.Entidades;
using API.Data.IUnitOfWorks.Interfaces.Seguridad;

namespace API.Data.IUnitOfWorks.Interfaces
{
    public interface IUnitOfWork<TEntity> : IDisposable where TEntity : EntidadBase
    {
        IPermisoRepository Permisos { get; }
        IRolPermisoRepository RolesPermisos { get; }
        IRolRepository Roles { get; }
        IUsuarioRepository Usuarios { get; }
        IBaseRepository<TEntity> BasicRepository { get; }
        ITrazaRepository Trazas { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveTrazasChangesAsync(CancellationToken cancellationToken = default);
    }
}
