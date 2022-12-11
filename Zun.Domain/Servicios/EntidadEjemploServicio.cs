using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Data.Entidades;
using Zun.Data.IUnitOfWork.Interfaces;
using Zun.Domain.Interfaces;

namespace Zun.Domain.Servicios
{
    public class EntidadEjemploServicio : ServicioBase<EntidadEjemplo>, IEntidadEjemploServicio
    {
        private readonly IRepositorioBase<EntidadEjemplo> baseRepository;

        public EntidadEjemploServicio(IUnitOfWork repositories, IRepositorioBase<EntidadEjemplo> baseRepository) : base(repositories, baseRepository)
        {
            this.baseRepository = baseRepository;
        }
    }
}