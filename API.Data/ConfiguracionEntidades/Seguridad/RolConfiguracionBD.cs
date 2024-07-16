using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.ConfiguracionEntidades.Seguridad
{
    public class RolConfiguracionBD
    {
        public static void SetEntityBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().ToTable("Roles");
            EntidadBaseConfiguracionBD<Rol>.SetEntityBuilder(modelBuilder);

            modelBuilder.Entity<Rol>().Property(e => e.Nombre).HasMaxLength(50).IsRequired();


            #region Seed

            Rol rol = new()
            {
                Id = new Guid("C0B7E3B3-A06E-4580-B985-BB2FC4336522"),
                Nombre = "Administrador",
            };
            modelBuilder.Entity<Rol>().HasData(rol);

            List<Permiso> permisos = new()
            {
                new (){ Id = new Guid("56B3924B-209B-40FB-9F31-AD75C12F4528"), Nombre = "Listar usuarios", Descripcion = "Permite ver los usuarios existentes en el sistema y sus datos." },
                new (){ Id = new Guid("4129CF49-CC22-46A1-9625-501855F2DA8B"), Nombre = "Gestionar usuarios", Descripcion = "Permite ver, crear, modificar y eliminar usuarios en el sistema."  },
                new (){ Id = new Guid("E36D283C-8B25-42B6-83BD-56EDD953E770"), Nombre = "Listar roles", Descripcion = "Permite ver los roles existentes en el sistema y sus datos."  },
                new (){ Id = new Guid("90ABF232-A641-478D-8720-F0AE49E8A306"), Nombre = "Gestionar rol", Descripcion = "Permite ver, crear, modificar y eliminar roles en el sistema."  },
            };
            modelBuilder.Entity<Permiso>().HasData(permisos);


            IEnumerable<RolPermiso> listadoRolPermiso = permisos.Select(e => new RolPermiso { Id = e.Id, RolId = rol.Id, PermisoId = e.Id });
            modelBuilder.Entity<RolPermiso>().HasData(listadoRolPermiso);
            #endregion
        }
    }
}
