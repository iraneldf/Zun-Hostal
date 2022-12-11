using Microsoft.EntityFrameworkCore;
using Zun.Data.Entidades;

namespace Zun.Data.IUnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEntidadEjemploRepositorio EntidadesEjemplo { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
