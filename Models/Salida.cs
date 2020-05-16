using System;

namespace tienda_web.Models
{
    public class Salida
    {
        public int SalidaId  { get; set; }
        public string ArtModelo { get; set; } 
        public int ProyectoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
    }
}