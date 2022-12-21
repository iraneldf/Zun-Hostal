using Microsoft.EntityFrameworkCore;
using Zun.Datos.ConfiguracionEntidades;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContexts
{
    public class TrazasDbContext : DbContext, ITrazasDbContext
    {
        #region Entidades
        public DbSet<Traza> Trazas { get; set; }
        #endregion

        public TrazasDbContext(DbContextOptions<TrazasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            TrazaBDConfiguracion.SetEntityBuilder(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
