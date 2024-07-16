using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Seguridad
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        private readonly IUnitOfWork<Usuario> _repositorios;

        public UsuarioValidator(IUnitOfWork<Usuario> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.Nombre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .MaximumLength(50).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Apellidos).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                     .MaximumLength(50).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                     .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Correo).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .MaximumLength(50).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                  .EmailAddress().WithMessage("Formato incorrecto.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Username).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                    .MaximumLength(50).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                    .NotNull().WithMessage("Es un campo obligatorio.");
            RuleFor(m => m.Contrasenna).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                       .MinimumLength(6).WithMessage("Debe tener {MinLength} caracteres mínimo.")
                                       .MaximumLength(500).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                       .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m).MustAsync(async (usuario, cancelacion) => !await _repositorios.Usuarios.AnyAsync(e => e.Id != usuario.Id && e.Nombre == usuario.Nombre && e.Apellidos == usuario.Apellidos))
                           .OverridePropertyName(nameof(Usuario.NombreCompleto))
                           .WithMessage("Ya existe un usuario con este mismo nombre completo.");

            RuleFor(m => m).MustAsync(async (usuario, cancelacion) => !await _repositorios.Usuarios.AnyAsync(e => e.Id != usuario.Id && e.Username == usuario.Username))
                           .OverridePropertyName(nameof(Usuario.Username))
                           .WithMessage("Ya existe un usuario con este mismo username.");
        }

    }
}
