using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Seguridad
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class PermisoValidator : AbstractValidator<Permiso>
    {
        private readonly IUnitOfWork<Permiso> _repositorios;

        public PermisoValidator(IUnitOfWork<Permiso> repositorios)
        {
            _repositorios = repositorios;

            RuleFor(m => m.Nombre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .MaximumLength(50).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.Descripcion).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                       .MaximumLength(200).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                       .NotNull().WithMessage("Es un campo obligatorio.");
        }

    }
}
