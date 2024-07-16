namespace API.Domain.Exceptions
{
    public class CustomException : Exception
    {
        public new string Message { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
