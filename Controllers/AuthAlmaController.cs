using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    public class AuthAlmaController : Controller
    {
        private TiendaContext _context;
        
        public AuthAlmaController(TiendaContext context)
        {
            _context = context;
        }
        
        public IActionResult Informacion()
        {
            ViewBag.Context = _context;
            RegistraBitacora("Informacion", "Consulta");
            return View(_context.Proyectos.ToList());
        }
        
        public void ExecuteQuery(string query)
        {
            SqlConnection conection = new SqlConnection("Server= localhost; Database= webstore; Integrated Security=SSPI; Server=localhost\\sqlexpress;");
            conection.Open();
            SqlCommand command = new SqlCommand(query,conection); // Create a object of SqlCommand class
            command.ExecuteNonQuery();
            conection.Close();
        }
        
        public void RegistraBitacora(string tabla, string operacion)
        {
            ExecuteQuery($"exec RegistraBitacora {tabla}, {operacion}");
        }
        
    }
}