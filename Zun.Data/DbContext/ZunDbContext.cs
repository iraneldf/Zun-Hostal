using Microsoft.EntityFrameworkCore;
using Zun.Datos.ConfiguracionEntidades;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContext
{
    public class ZunDbContext : Microsoft.EntityFrameworkCore.DbContext, IZunDbContext
    {
        #region Entidades
        public DbSet<EntidadEjemplo> EntidadesEjemplo { get; set; }
        #endregion

        public ZunDbContext(DbContextOptions<ZunDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntidadEjemploBDConfiguracion.SetEntityBuilder(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
