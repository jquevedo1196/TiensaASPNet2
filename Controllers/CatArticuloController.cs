using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    public class CatArticuloController : Controller
    {
        private TiendaContext _context;
        
        public CatArticuloController(TiendaContext context)
        {
            _context = context;
        }
        
        public IActionResult CatArticulos()
        {
            ViewBag.Context = _context;
            //RegistraBitacora("CatArticulos", "Consulta");
            return View(_context.CatArticulos.ToList());
        }

        [Route("CatArticulo/BorrarCatArticulo/{catArticuloId}")]
        public IActionResult BorrarCatArticulo(int catArticuloId)
        {
            ViewBag.Context = _context;
            if (catArticuloId != 0)
            {
                if (_context.CatArticulos.Find(catArticuloId) != null)
                {
                    _context.CatArticulos.Remove(_context.CatArticulos.Find(catArticuloId));
                    _context.SaveChanges();
                }
            }
            RegistraBitacora("CatArticulos", "Borrado");
            return View("CatArticulos", _context.CatArticulos.ToList());
        }
        
        [Route("CatArticulo/EditarCatArticulo/{catArticuloId}")]
        public IActionResult EditarCatArticulo(int catArticuloId)
        {
            var marcasLists = new List<SelectListItem>();
            var marcas = _context.Marcas.ToList();
            foreach (var marca in marcas)
            {
                marcasLists.Add(new SelectListItem(){Text = marca.MarcaId.ToString(), Value = marca.VcMarcaName});
            }
            var tipoArtsLists = new List<SelectListItem>();
            var tipoArts = _context.CatTipoArts.ToList();
            foreach (var tipoArt in tipoArts)
            {
                tipoArtsLists.Add(new SelectListItem(){Text = tipoArt.TipoArtId.ToString(), Value = tipoArt.TipoArtDesc});
            }
            ViewBag.Marcas = marcasLists;
            ViewBag.CatTipoArts = tipoArtsLists;
            CatArticulo catArticulo = _context.CatArticulos.Find(catArticuloId);
            return View(catArticulo);
        }
        
        [HttpPost]
        [Route("CatArticulo/EditarCatArticulo/{catArticuloId}")]
        public IActionResult EditarCatArticulo(CatArticulo catArticulo)
        {
            ViewBag.Context = _context;
            _context.CatArticulos.Update(catArticulo);
            _context.SaveChanges();
            RegistraBitacora("CatArticulos", "Edición");
            return View("CatArticulos", _context.CatArticulos.ToList());
        }
        
        public IActionResult CrearCatArticulo()
        {
            var marcasLists = new List<SelectListItem>();
            var marcas = _context.Marcas.ToList();
            foreach (var marca in marcas)
            {
                marcasLists.Add(new SelectListItem(){Text = marca.MarcaId.ToString(), Value = marca.VcMarcaName});
            }
            var tipoArtsLists = new List<SelectListItem>();
            var tipoArts = _context.CatTipoArts.ToList();
            foreach (var tipoArt in tipoArts)
            {
                tipoArtsLists.Add(new SelectListItem(){Text = tipoArt.TipoArtId.ToString(), Value = tipoArt.TipoArtDesc});
            }
            ViewBag.Marcas = marcasLists;
            ViewBag.CatTipoArts = tipoArtsLists;
            return View();
        }
        
        [HttpPost]
        public IActionResult CrearCatArticulo(CatArticulo catArticulo)
        {
            ViewBag.Context = _context;
            _context.CatArticulos.Add(catArticulo);
            _context.SaveChanges();
            RegistraBitacora("CatArticulos", "Inserción");
            return View("CatArticulos", _context.CatArticulos.ToList());
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