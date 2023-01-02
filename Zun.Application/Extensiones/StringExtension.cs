namespace Zun.Aplicacion.Helper
{
    public static class StringExtension
    {
        public static string PrimeraLetraMayuscula(this Object objeto)
        {
            try
            {
                string input = objeto?.ToString()?.ToLower() ?? throw new Exception();
                return input.First().ToString().ToUpper() + input.Substring(1);
            }
            catch { return string.Empty; }
        }
    }
}
