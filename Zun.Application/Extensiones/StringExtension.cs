namespace Zun.Aplicacion.Extensiones
{
    public static class StringExtension
    {
        public static string PrimeraLetraMayuscula(this object objeto)
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
