using Microsoft.EntityFrameworkCore;
using Zun.Data.Entidades;

namespace Zun.Data.DbContext
{
    public interface IZunDbContext
    {
        #region Entidades

        DbSet<EntidadEjemplo> EntidadesEjemplo { get; set; }

        #endregion
    }
}
