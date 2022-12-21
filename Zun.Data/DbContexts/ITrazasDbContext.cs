using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContexts
{
    public interface ITrazasDbContext
    {
        #region Entidades

        DbSet<Traza> Trazas { get; set; }

        #endregion
    }
}
