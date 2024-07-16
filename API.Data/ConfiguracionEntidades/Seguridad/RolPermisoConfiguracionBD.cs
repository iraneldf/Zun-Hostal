using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Seguridad
{
    public class RolPermisoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolPermiso>().ToTable("RolPermiso");

            EntidadBaseConfiguracionBD<RolPermiso>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<RolPermiso>().HasOne(e => e.Rol).WithMany(e => e.RolPermiso).HasForeignKey(e => e.RolId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolPermiso>().HasOne(e => e.Permiso).WithMany(e => e.RolPermiso).HasForeignKey(e => e.PermisoId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolPermiso>().HasIndex(e => new { e.RolId, e.PermisoId }).IsUnique();
        }
    }
}
