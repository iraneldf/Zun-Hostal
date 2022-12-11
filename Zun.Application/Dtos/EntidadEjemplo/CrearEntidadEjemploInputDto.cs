namespace Zun.Application.Dtos.EntidadEjemplo
{
    public class CrearEntidadEjemploInputDto
    {
        public string Name { get; set; } = string.Empty;
        public int Edad { get; set; }
        public List<string> Intereses { get; set; } = new();
    }
}
