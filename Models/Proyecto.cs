using System.ComponentModel.DataAnnotations;

namespace tienda_web.Models
{
    public class Proyecto
    {
        [Key]
        public int ProyectoId { get; set; }
        public string ProyectoName { get; set; }
        public string EmpresaRfc { get; set; }
        public string UserId { get; set; }
        public string AuthSalida { get; set; }
        public string AuthEntrada { get; set; }
    }
}