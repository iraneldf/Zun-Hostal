using System.ComponentModel.DataAnnotations;

namespace API.Application.Dtos.Comunes;

public class ObtenerSelectListInputDto
{
    /// <summary>
    ///     Nombre del campo que se usuara para definir el valor del item
    /// </summary>
    [Required]
    public required string NombreCampoValor { get; set; }

    /// <summary>
    ///     Nombre del campo que se usará para mostrar los items
    /// </summary>
    [Required]
    public required string NombreCampoTexto { get; set; }

    /// <summary>
    ///     Valor del item que aparecerá seleccionado
    /// </summary>
    public string? ValorSeleccionado { get; set; }

    /// Secuencia de ordenamiento para ordenar el listado.
    /// FORMATO: Campo1:(asc/desc),Campo2:(asc/desc),...
    public string? SecuenciaOrdenamiento { get; set; }
}