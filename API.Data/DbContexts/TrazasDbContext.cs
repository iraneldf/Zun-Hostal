using API.Data.ConfiguracionEntidades.Seguridad;
using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts
{
    public class TrazasDbContext : DbContext, ITrazasDbContext
    {
        public DbSet<Traza> Trazas { get; set; }

        public TrazasDbContext(DbContextOptions<TrazasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            TrazaConfiguracionBD.SetEntityBuilder(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
