using Zun.Data.DbContext;
using Zun.Data.Entidades;
using Zun.Data.IUnitOfWork.Interfaces;

namespace Zun.Data.IUnitOfWork.Repositorios
{
    public class EntidadEjemploRepositorio : RepositorioBase<EntidadEjemplo>, IEntidadEjemploRepositorio
    {
        public EntidadEjemploRepositorio(ZunDbContext context) : base(context)
        {
        }
    }
}
