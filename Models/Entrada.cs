using System;

namespace tienda_web.Models
{
    public class Entrada
    {
        public int EntradaId  { get; set; }
        public string ArtModelo { get; set; } 
        public int ProyectoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}