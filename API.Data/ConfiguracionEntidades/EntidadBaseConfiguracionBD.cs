using API.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades
{
    public class EntidadBaseConfiguracionBD<TEntity> where TEntity : EntidadBase
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>().HasKey(e => e.Id);

            modelBuilder.Entity<TEntity>().Property(entity => entity.Id).IsRequired()
                         .ValueGeneratedOnAdd();

            modelBuilder.Entity<TEntity>().Property(entity => entity.FechaCreado).IsRequired();
            modelBuilder.Entity<TEntity>().Property(entity => entity.FechaActualizado).IsRequired();

            modelBuilder.Entity<TEntity>().HasIndex(entity => entity.Id).IsUnique();
        }
    }
}
