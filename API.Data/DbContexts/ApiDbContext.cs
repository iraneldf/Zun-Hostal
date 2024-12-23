using API.Data.ConfiguracionEntidades.Hostal;
using API.Data.ConfiguracionEntidades.Seguridad;
using API.Data.Entidades.Hostal;
using API.Data.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace API.Data.DbContexts;

public class ApiDbContext : DbContext, IApiDbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public DbSet<Permiso> Permisos { get; set; }
    public DbSet<RolPermiso> RolPermiso { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Reserva> Reserva { get; set; }
    public DbSet<Habitacion> Habitacion { get; set; }
    public DbSet<AmaDeLlave> AmaDeLlave { get; set; }
    public DbSet<AmaDeLlaveHabitacion> AmaDeLlaveHabitacion { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        RolPermisoConfiguracionBD.SetEntityBuilder(modelBuilder);
        RolConfiguracionBD.SetEntityBuilder(modelBuilder);
        PermisoConfiguracionBD.SetEntityBuilder(modelBuilder);
        UsuarioConfiguracionBD.SetEntityBuilder(modelBuilder);
        ClienteConfiguracionBD.SetEntityBuilder(modelBuilder);
        ReservaConfiguracionBD.SetEntityBuilder(modelBuilder);
        AmaDeLlaveConfiguracionBD.SetEntityBuilder(modelBuilder);
        HabitacionConfiguracionBD.SetEntityBuilder(modelBuilder);
        AmaDeLlaveHabitacionConfiguracionBD.SetEntityBuilder(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}