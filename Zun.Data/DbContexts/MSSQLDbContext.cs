using Microsoft.EntityFrameworkCore;
using Zun.Datos.ConfiguracionEntidades;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContexts
{
    public class MSSQLDbContext : DbContext, IMSSQLDbContext
    {
        #region Entidades
        #endregion

        public MSSQLDbContext(DbContextOptions<MSSQLDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
