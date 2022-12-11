using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContext
{
    public interface IZunDbContext
    {
        #region Entidades

        DbSet<EntidadEjemplo> EntidadesEjemplo { get; set; }

        #endregion
    }
}
