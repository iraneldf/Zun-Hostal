using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zun.Datos.Entidades;
using Zun.Datos.IUnitOfWork.Interfaces;
using Zun.Datos.IUnitOfWork.Repositorios;
using Zun.Dominio.Interfaces;

namespace Zun.Dominio.Servicios
{
    public class TrazaServicio : ITrazaServicio
    {
        protected readonly ITrazasRepositorio _trazaRepositorio;
        protected readonly IUnitOfWork _repositorios;
        protected readonly IHttpContextAccessor _httpContext;
        public TrazaServicio(IUnitOfWork repositorios, ITrazasRepositorio trazaRepositorio, IHttpContextAccessor httpContext) 
        {
            _trazaRepositorio = trazaRepositorio;
            _repositorios = repositorios;
            _httpContext = httpContext;
        }

        public virtual async Task<IEnumerable<Traza>> ObtenerTodos() => await _trazaRepositorio.GetAllAsync();
    }
}
