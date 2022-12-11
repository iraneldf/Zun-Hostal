using Zun.Datos.DbContext;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;

namespace Zun.Datos.IUnitOfWork.Repositorios
{
    public class EntidadEjemploRepositorio : RepositorioBase<EntidadEjemplo>, IEntidadEjemploRepositorio
    {
        public EntidadEjemploRepositorio(ZunDbContext context) : base(context)
        {
        }
    }
}
