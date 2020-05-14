using System.ComponentModel.DataAnnotations;

namespace tienda_web.Models
{
    public class Empresa
    {
        [Key]
        public string EmpresaRfc { get; set; }
        public string EmpresaRazSoc { get; set; }
        public string EmpresaCalle { get; set; }
        public string EmpresaNumInt { get; set; }
        public string EmpresaNumExt { get; set; }
        public string EmpresaCp { get; set; }
        public string EmpresaCd { get; set; }
        public string EmpresaEdo { get; set; }
    }
}