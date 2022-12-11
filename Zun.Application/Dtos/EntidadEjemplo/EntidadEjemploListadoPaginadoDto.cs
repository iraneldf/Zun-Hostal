using Zun.Application.Dtos;

namespace Zun.Application.Dtos.EntidadEjemplo
{
    public class EntidadEjemploListadoPaginadoDto : EntitidadBaseDto
    {
        public string Name { get; set; } = string.Empty;
        public int Edad { get; set; }
    }
}
