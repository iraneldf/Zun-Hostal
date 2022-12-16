using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Dominio.Interfaces;

namespace Zun.Dominio.Servicios
{
    public class EntidadEjemploServicio : ServicioBase<EntidadEjemplo>, IEntidadEjemploServicio
    {
        public EntidadEjemploServicio(IUnitOfWork repositories, IRepositorioBase<EntidadEjemplo> baseRepository, IHttpContextAccessor httpContext) : base(repositories, baseRepository, httpContext)
        {
        }
    }
}