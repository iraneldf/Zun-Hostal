using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Seguridad
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class RolValidator : AbstractValidator<Rol>
    {
        private readonly IUnitOfWork<Rol> _repositorios;

        public RolValidator(IUnitOfWork<Rol> repositorios)
        {
            _repositorios = repositorios;


            RuleFor(m => m.Nombre).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                  .MaximumLength(50).WithMessage("Debe tener {MaxLength} caracteres máximo.")
                                  .NotNull().WithMessage("Es un campo obligatorio.");
        }

    }
}
