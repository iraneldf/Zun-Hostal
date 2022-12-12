using FluentValidation;
using Zun.Aplicacion.Dtos.EntidadEjemplo;

namespace Zun.Aplicacion.Validadores.EntidadEjemplo
{
    public class CrearEntidadEjemploValidator : AbstractValidator<CrearEntidadEjemploInputDto>
    {
        public CrearEntidadEjemploValidator()
        {
            RuleFor(dto => dto.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .NotNull().WithMessage("{PropertyName} es obligatorio");

            RuleFor(dto => dto.Edad)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
                .NotNull().WithMessage("{PropertyName} es obligatorio")
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0");

           
        }
    }
}
