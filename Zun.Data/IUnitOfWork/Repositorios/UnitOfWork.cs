using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Datos.DbContexts;

namespace Zun.Datos.IUnitOfWork.Repositorios
{
    public class UnitOfWork : Interfaces.IUnitOfWork
    {
        private readonly ZunDbContext _context;

        public IEntidadEjemploRepositorio EntidadesEjemplo { get; }
        public ITrazasRepositorio Trazas { get; }

        public UnitOfWork(ZunDbContext context, TrazasDbContext trazaContext)
        {
            _context = context;
            EntidadesEjemplo = new EntidadEjemploRepositorio(context);
            Trazas = new TrazasRepositorio(trazaContext);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() => _context.Dispose();
    }
}
