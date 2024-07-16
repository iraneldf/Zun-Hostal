namespace API.Application.Dtos.Comunes
{
    public class ResponseDto
    {
        public int Status { get; internal set; }
        public object? Result { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
