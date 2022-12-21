using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.IUnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEntidadEjemploRepositorio EntidadesEjemplo { get; }
        ITrazasRepositorio Trazas { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
