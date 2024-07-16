using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Seguridad.Permiso
{
    public class PermisoDto : EntidadBaseDto
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
    }
}
