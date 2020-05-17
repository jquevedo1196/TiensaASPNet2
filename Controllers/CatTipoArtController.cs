using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    public class CatTipoArtController: Controller
    {
        private TiendaContext _context;
        
        public CatTipoArtController(TiendaContext context)
        {
            _context = context;
        }
        
        public IActionResult CatTipoArts()
        {
            ViewBag.Context = _context;
            RegistraBitacora("CatTipoArts", "Consulta");
            return View(_context.CatTipoArts.ToList());
        }
        
        [Route("CatTipoArt/EliminarTipoArt/{tipoArtId}")]
        public IActionResult EliminarCatTipoArt(int tipoArtId)
        {
            if (tipoArtId != 0)
            {
                if (_context.CatTipoArts.Find(tipoArtId) != null)
                {
                    _context.CatTipoArts.Remove(_context.CatTipoArts.Find(tipoArtId));
                    _context.SaveChanges();
                }
            }
            RegistraBitacora("CatTipoArts", "Borrado");
            return View("CatTipoArts", _context.CatTipoArts.ToList());
        }
        
        public IActionResult CrearTipoArt()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult CrearTipoArt(CatTipoArt tipoArt)
        {
            if (ModelState.IsValid)
            {
                _context.CatTipoArts.Add(tipoArt);
                _context.SaveChanges();
                RegistraBitacora("CatTipoArts", "Inserción");
                return View("CatTipoArts", _context.CatTipoArts.ToList());
            }

            return View();
        }
        
        [Route("CatTipoArt/EditarTipoArt/{tipoArtId}")]
        public IActionResult EditarTipoArt(int tipoArtId)
        {
            CatTipoArt tipoArt = _context.CatTipoArts.Find(tipoArtId);
            return View(tipoArt);
        }
        
        [HttpPost]
        [Route("CatTipoArt/EditarCatTipoArt/{tipoArtId}")]
        public IActionResult EditarCatTipoArt(CatTipoArt tipoArt)
        {
            if (ModelState.IsValid)
            {
                _context.CatTipoArts.Update(tipoArt);
                _context.SaveChanges();
                RegistraBitacora("CatTipoArts", "Edición");
                return View("CatTipoArts", _context.CatTipoArts.ToList());
            }

            return View();
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