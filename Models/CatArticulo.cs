using System.ComponentModel.DataAnnotations;

namespace tienda_web.Models
{
    public class CatArticulo
    {
        [Key]
        public int ArtId { get; set; } 
        public string ArtNombre { get; set; }
        public int MarcaId { get; set; }
    }
}