using FluentValidation;
using Zun.Aplicacion.Dtos;
using Zun.Aplicacion.Dtos.EntidadEjemplo;

namespace Zun.Aplicacion.Validadores
{
    public class ConfigurarListadoPaginadoValidator : ValidarPaginado<ConfiguracionListadoPaginadoDto>
    {
        public ConfigurarListadoPaginadoValidator()
        {  

        }
    }
}
