using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zun.Datos.Entidades;

namespace Zun.Datos.IUnitOfWork.Interfaces
{
    public interface ITrazasRepositorio
    {
        Task AddAsync(Traza traza);
    }
}
