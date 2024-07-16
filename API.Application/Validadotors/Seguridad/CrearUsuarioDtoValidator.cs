using API.Application.Dtos.Seguridad.Usuario;
using API.Data.Entidades.Seguridad;
using FluentValidation;

namespace API.Application.Validadotors.Seguridad
{
    public class CrearUsuarioDtoValidator : AbstractValidator<CrearUsuarioInputDto>
    {

        public CrearUsuarioDtoValidator()
        {
            RuleFor(m => m).Must((usuario, cancelacion) => usuario.Contrasenna == usuario.ContrasennaConfirmada).OverridePropertyName(nameof(Usuario.Contrasenna)).WithMessage("Las contraseñas no coinciden.");
        }
    }
}
