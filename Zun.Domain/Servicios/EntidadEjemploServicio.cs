using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Dominio.Interfaces;

namespace Zun.Dominio.Servicios
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