using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Seguridad
{
    public class UsuarioConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            EntidadBaseConfiguracionBD<Usuario>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Usuario>().Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Usuario>().Property(e => e.Correo).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Usuario>().Property(e => e.Username).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Usuario>().Property(e => e.Apellidos).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Usuario>().Property(e => e.Contrasenna).HasMaxLength(500).IsRequired();

            modelBuilder.Entity<Usuario>().HasIndex(e => new { e.Nombre, e.Apellidos }).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(e => new { e.Username }).IsUnique();

            modelBuilder.Entity<Usuario>().HasOne(e => e.Rol).WithMany(e => e.Usuarios).HasForeignKey(e => e.RolId).OnDelete(DeleteBehavior.Restrict);

            #region Seed
            Usuario usuario = new() { Id = new Guid("42717FB8-6E3F-4C94-B6B1-A88E8718D0A6"), Nombre = "Admin", Apellidos = "System", Username = "admin.system", Correo = "admin.system@api.cu", Contrasenna = "poner hash", RolId = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522") };
            modelBuilder.Entity<Usuario>().HasData(usuario);
            #endregion
        }
    }
}
