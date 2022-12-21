using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.ConfiguracionEntidades
{
    public class TrazaBDConfiguracion
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            #region Configurando Entidad
            BDConfiguracionBase<Traza>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Traza>().ToTable("Trazas");
            modelBuilder.Entity<Traza>().Property(e => e.Descripcion).IsRequired();           
            modelBuilder.Entity<Traza>().Property(e => e.TablaBD).HasMaxLength(100).IsRequired();           
            modelBuilder.Entity<Traza>().Property(e => e.Elemento).IsRequired();           

            #endregion
        }
    }
}
