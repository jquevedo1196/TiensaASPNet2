using System.ComponentModel.DataAnnotations;

namespace tienda_web.Models
{
    public class CatTipoArt
    {
        [Key] public int TipoArtId { get; set; }
        public string TipoArtDesc { get; set; }
    }
}