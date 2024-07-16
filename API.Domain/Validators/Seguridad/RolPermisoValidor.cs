using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks.Interfaces;
using FluentValidation;

namespace API.Domain.Validators.Seguridad
{
    /// <summary>
    /// Valida que los datos de la entidad esten correctos antes de ser insertados a la BD
    /// </summary>
    public class RolPermisoValidator : AbstractValidator<RolPermiso>
    {
        private readonly IUnitOfWork<RolPermiso> _repositorios;

        public RolPermisoValidator(IUnitOfWork<RolPermiso> repositorios)
        {
            _repositorios = repositorios;

            RuleFor(m => m.RolId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                 .NotNull().WithMessage("Es un campo obligatorio.");

            RuleFor(m => m.PermisoId).NotEmpty().WithMessage("No puede ser un texto vacio.")
                                 .NotNull().WithMessage("Es un campo obligatorio.");
        }

    }
}
