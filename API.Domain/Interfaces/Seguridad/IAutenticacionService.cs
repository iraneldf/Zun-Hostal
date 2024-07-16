namespace API.Domain.Interfaces.Seguridad
{
    public interface IAutenticacionService
    {
        Task<(string, DateTime)> ConstruirToken(string username);
        Task<bool> Login(string userName, string contrasenna);
    }
}