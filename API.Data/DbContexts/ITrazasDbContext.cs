using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts
{
    public interface ITrazasDbContext
    {
        #region Entities

        DbSet<Traza> Trazas { get; set; }

        #endregion
    }
}
