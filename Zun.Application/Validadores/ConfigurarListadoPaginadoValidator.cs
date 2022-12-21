using FluentValidation;
using Zun.Aplicacion.Dtos;
using Zun.Aplicacion.Dtos.EntidadEjemplo;

namespace Zun.Aplicacion.Validadores
{
    public class ConfigurarListadoPaginadoValidator : AbstractValidator<ConfiguracionListadoPaginadoDto>
    {
        public ConfigurarListadoPaginadoValidator()
        {
            RuleFor(dto => dto.CantIgnorar)
                .NotNull().WithMessage("{PropertyName} es obligatorio");

            RuleFor(dto => dto.CantMostrar)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .NotNull().WithMessage("{PropertyName} es obligatorio")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0");


        }
    }
}
