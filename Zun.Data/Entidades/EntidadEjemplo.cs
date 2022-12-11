﻿namespace Zun.Data.Entidades
{
    public class EntidadEjemplo : EntitidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public List<string> Intereses { get; set; } = new();

    }
}