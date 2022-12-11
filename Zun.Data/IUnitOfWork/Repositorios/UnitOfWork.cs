using Microsoft.EntityFrameworkCore;
using Zun.Data.DbContext;
using Zun.Data.Entidades;
using Zun.Data.IUnitOfWork.Interfaces;

namespace Zun.Data.IUnitOfWork.Repositorios
{
    public class UnitOfWork : Interfaces.IUnitOfWork
    {
        private readonly ZunDbContext _context;

        public IEntidadEjemploRepositorio EntidadesEjemplo { get; }

        public UnitOfWork(ZunDbContext context)
        {
            _context = context;
            EntidadesEjemplo = new EntidadEjemploRepositorio(context);
        }        

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() => _context.Dispose();
    }
}
