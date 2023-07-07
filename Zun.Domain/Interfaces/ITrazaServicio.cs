using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zun.Datos.Entidades;

namespace Zun.Dominio.Interfaces
{
    public interface ITrazaServicio
    {
        /// <summary>
        /// Retorna todos los elementos
        /// </summary>
        Task<IEnumerable<Traza>> ObtenerTodos();
    }
}
