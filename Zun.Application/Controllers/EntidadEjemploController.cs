
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Zun.Aplicacion.Dtos;
using Zun.Aplicacion.Dtos.EntidadEjemplo;
using Zun.Datos.Entidades;
using Zun.Dominio.Interfaces;

namespace Zun.Aplicacion.Controllers
{
    public class EntidadEjemploController : ControladorBase<EntidadEjemplo, EntidadEjemploDto, CrearEntidadEjemploInputDto, ModificarEntidadEjemploInputDto, EntidadEjemploListadoPaginadoOutputDto, FiltroEntidadEjemploListadoPaginadoDto>
    {
        public EntidadEjemploController(IMapper mapper, IServicioBase<EntidadEjemplo> servicioBase, IBackgroundJobClient clientHangfire) : base(mapper, servicioBase, clientHangfire)
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
            List<Expression<Func<EntidadEjemplo, bool>>> filters = new();
            if (filtro.Edad != null)
                filters.Add(entidadEjemplo => entidadEjemplo.Edad.ToString().Contains(filtro.Edad.Value.ToString()));
            if (filtro.Nombre != null)
                filters.Add(entidadEjemplo => entidadEjemplo.Nombre.Contains(filtro.Nombre));

            return _servicioBase.ObtenerListadoPaginado(filtro.CantIgnorar, filtro.CantMostrar, filters.ToArray());
        }

        /// <summary>
        /// Metodo de ejemplo para realizar una transaccion. 
        /// Los datos no se guardan en BD hasta que no se realice un commit
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]        
        private async Task<IActionResult> EjemploTransaccion()
        {
            try
            {

                try
                {
                    await _servicioBase.IniciarTransaccion();

                    EntidadEjemplo a = (await _servicioBase.Crear(new EntidadEjemplo { Edad = 520, Intereses = "ee555ee", Nombre = "5555ddd" })).Entity;
                    await _servicioBase.SaveChanges();

                    await _servicioBase.CommitTransaccion();
                }
                catch (Exception)
                {
                    await _servicioBase.RollbackTransaccion();
                }

                return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Resultado = null });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDto
                {
                    Status = StatusCodes.Status400BadRequest,
                    MensajeError = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
