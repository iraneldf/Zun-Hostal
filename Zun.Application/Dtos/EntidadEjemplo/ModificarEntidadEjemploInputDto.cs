using Zun.Application.Dtos;

namespace Zun.Application.Dtos.EntidadEjemplo
{
    public class ModificarEntidadEjemploInputDto : EntitidadBaseDto
    {
        public string Name { get; set; } = string.Empty;
        public int Edad { get; set; }
        public List<string> Intereses { get; set; } = new();
    }
}
