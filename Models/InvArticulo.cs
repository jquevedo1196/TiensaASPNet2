using System.ComponentModel.DataAnnotations;

namespace tienda_web.Models
{
    public class InvArticulo
    {
        [Key]
        public string ArtModelo  { get; set; }
        public string ArtDesc  { get; set; }
        public int ArtId  { get; set; }
        public int CantidadNeta  { get; set; }
        public int CantidadPrestada  { get; set; }
        public int CantidadEnAlmacen  { get; set; }
        public decimal PrecioUnitario  { get; set; }
    }
}