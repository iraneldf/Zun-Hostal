
using AutoMapper;
using System.Linq.Expressions;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;
using Zun.Dominio.Interfaces;

namespace Zun.Aplicacion.Controllers
{
    public class EntidadEjemploController : ControladorBase<EntidadEjemplo, EntidadEjemploDto, CrearEntidadEjemploInputDto, ModificarEntidadEjemploInputDto, EntidadEjemploListadoPaginadoDto, FiltroEntidadEjemploListadoPaginadoDto>
    {
        public EntidadEjemploController(IMapper mapper, IServicioBase<EntidadEjemplo> servicioBase) : base(mapper, servicioBase)
        {

        }


        /// <summary>
        /// Retorna un listado de elementos que han sido filtrados y la cantidad de elementos
        /// El elemento sera retornado solo con los campos que seran modificados
        /// </summary>
        /// <param name="filtro">filtro y configuracion para el paginado</param>
        protected override Task<(IEnumerable<EntidadEjemplo>, int)> AplicarFiltros(FiltroEntidadEjemploListadoPaginadoDto filtro)
        {
            //agregando filtros
            Expression<Func<EntidadEjemplo, bool>>[] filters = new Expression<Func<EntidadEjemplo, bool>>[2];
            if (filtro.Edad != null)
                filters[0] = entidadEjemplo => entidadEjemplo.Edad == filtro.Edad;
            if (filtro.Nombre != null)
                filters[1] = entidadEjemplo => entidadEjemplo.Nombre == filtro.Nombre;

            return _servicioBase.ObtenerListadoPaginado(filtro.CantIgnorar, filtro.CantMostrar, filters);
        }
    }
}
