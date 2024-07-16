using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts
{
    public interface IApiDbContext
    {
        #region Entities

        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Traza> Trazas { get; set; }

        #endregion
    }
}
