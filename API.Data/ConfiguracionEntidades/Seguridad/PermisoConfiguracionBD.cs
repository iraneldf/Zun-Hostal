
using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Seguridad
{
    public class PermisoConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permiso>().ToTable("Permisos");
            EntidadBaseConfiguracionBD<Permiso>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Permiso>().Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Permiso>().Property(e => e.Descripcion).HasMaxLength(200).IsRequired();


        }
    }
}
