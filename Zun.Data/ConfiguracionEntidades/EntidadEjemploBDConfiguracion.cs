using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.ConfiguracionEntidades
{
    public class EntidadEjemploBDConfiguracion
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            #region Configurando Entidad
            BDConfiguracionBase<EntidadEjemplo>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<EntidadEjemplo>().ToTable("EntidadEjemplo"/*, t => t.ExcludeFromMigrations()*/);

            modelBuilder.Entity<EntidadEjemplo>().Property(e => e.Nombre)
                                                 .IsRequired()
                                                 .HasMaxLength(100);

            modelBuilder.Entity<EntidadEjemplo>().Property(e => e.Edad)
                                                 .IsRequired()
                                                 .HasMaxLength(3);

            modelBuilder.Entity<EntidadEjemplo>().HasIndex(e => e.Nombre).IsUnique();

            #endregion
        }
    }
}
