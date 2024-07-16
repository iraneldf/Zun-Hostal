using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Seguridad
{
    public class TrazaConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            #region Configurando Entity
            modelBuilder.Entity<Traza>().ToTable("Trazas");
            EntidadBaseConfiguracionBD<Traza>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Traza>().Property(e => e.Descripcion).IsRequired();
            modelBuilder.Entity<Traza>().Property(e => e.TablaBD).HasMaxLength(100).IsRequired();

            #endregion
        }
    }
}
