using API.Application.Dtos.Seguridad.Usuario;
using FluentValidation;

namespace API.Application.Validadotors.Seguridad
{
    public class CambiarContrasennaDtoValidator : AbstractValidator<CambiarContrasennaInputDto>
    {

        public CambiarContrasennaDtoValidator()
        {
            RuleFor(m => m).Must((usuario, cancelacion) => usuario.NuevaContrasenna == usuario.ContrasennaConfirmada).OverridePropertyName(nameof(CambiarContrasennaInputDto.NuevaContrasenna)).WithMessage("Las contraseñas no coinciden.");
        }
    }
}
