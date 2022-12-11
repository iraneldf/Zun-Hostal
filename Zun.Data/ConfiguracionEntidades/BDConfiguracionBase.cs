using Microsoft.EntityFrameworkCore;
using Zun.Datos.Entidades;

namespace Zun.Datos.ConfiguracionEntidades
{
    public class BDConfiguracionBase<TEntity> where TEntity : EntitidadBase
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            #region Configurando Entidad

            modelBuilder.Entity<TEntity>().Property(entity => entity.Id).IsRequired()
                         .ValueGeneratedOnAdd();

            modelBuilder.Entity<TEntity>().Property(entity => entity.FechaCreacion).IsRequired();
            modelBuilder.Entity<TEntity>().Property(entity => entity.FechaModificacion).IsRequired();

            modelBuilder.Entity<TEntity>().HasIndex(entity => entity.Id).IsUnique();

            #endregion
        }
    }
}
