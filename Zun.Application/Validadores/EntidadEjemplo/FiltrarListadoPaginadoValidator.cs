using FluentValidation;
using Zun.Aplicacion.Dtos;
using Zun.Aplicacion.Dtos.EntidadEjemplo;

namespace Zun.Aplicacion.Validadores.EntidadEjemplo
{
    public class FiltrarListadoPaginadoValidator : AbstractValidator<FiltroEntidadEjemploListadoPaginadoDto>
    {
        public FiltrarListadoPaginadoValidator()
        {
            RuleFor(dto => dto.CantIgnorar)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .NotNull().WithMessage("{PropertyName} es obligatorio")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0");

            RuleFor(dto => dto.CantMostrar)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .NotNull().WithMessage("{PropertyName} es obligatorio")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0");

           
        }
    }
}
