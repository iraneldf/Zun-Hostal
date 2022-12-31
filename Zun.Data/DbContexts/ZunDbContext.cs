using Microsoft.EntityFrameworkCore;
using Zun.Datos.ConfiguracionEntidades;
using Zun.Datos.Entidades;

namespace Zun.Datos.DbContexts
{
    public class ZunDbContext : DbContext, IZunDbContext
    {
        public DbSet<EntidadEjemplo> EntidadesEjemplo { get; set; }

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
