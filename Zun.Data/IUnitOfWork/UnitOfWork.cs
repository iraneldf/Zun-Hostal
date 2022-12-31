using Zun.Datos.DbContexts;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Datos.IUnitOfWork.Repositorios;

namespace Zun.Datos.IUnitOfWork
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
