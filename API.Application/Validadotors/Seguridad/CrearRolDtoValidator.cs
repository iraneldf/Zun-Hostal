using API.Application.Dtos.Seguridad.Rol;
using FluentValidation;

namespace API.Application.Validadotors.Seguridad
{
    public class CrearRolDtoValidator : AbstractValidator<CrearRolInputDto>
    {

        public CrearRolDtoValidator()
        {
            RuleFor(m => m.PermisoIds).NotEmpty().WithMessage("No puede ser una lista vacia.")
                                      .NotNull().WithMessage("Es un campo obligatorio.");
        }
    }
}
