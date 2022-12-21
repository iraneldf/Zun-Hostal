using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContexts
{
    public interface IZunDbContext
    {
        #region Entidades

        DbSet<EntidadEjemplo> EntidadesEjemplo { get; set; }

        #endregion
    }
}
