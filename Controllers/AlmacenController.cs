using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    public class AlmacenController : Controller
    {
        private TiendaContext _context;
        
        public AlmacenController(TiendaContext context)
        {
            _context = context;
        }
        
        public IActionResult Informacion()
        {
            ViewBag.Context = _context;
            RegistraBitacora("Informacion", "Consulta");
            return View(_context.Proyectos.ToList());
        }
        
        [Route("Almacen/Salidas/VerSalidas/{proyectoId}")]
        public IActionResult VerSalidas(int proyectoId)
        {
            ViewBag.Context = _context;
            ViewBag.ProyectoId = proyectoId;
            List<Salida> salidas = _context.Salidas.Where(salida => salida.ProyectoId == proyectoId).ToList();
            return View("Salidas/VerSalidas", salidas);
        }
        
        [Route("Almacen/Salidas/AgregarSalidas/{proyectoId}")]
        public IActionResult AgregarSalidas(int proyectoId)
        {
            ViewBag.ProyectoId = proyectoId;
            var salidas = _context.Salidas.Where(sa => sa.ProyectoId == proyectoId);
            List<string> salidasModels = salidas.Select(s => s.ArtModelo).ToList();
            var invArticulosList = new List<SelectListItem>();
            var invArticulos = _context.InvArticulos.Where(x => !salidasModels.Contains(x.ArtModelo)).ToList();
            var catArticulos = _context.CatArticulos.ToList();
            foreach (var articulo in catArticulos)
            {
                foreach (var elemento in invArticulos)
                {
                    if (elemento.ArtId == articulo.ArtId)
                    {
                        if (elemento.CantidadEnAlmacen > 0)
                        {
                            invArticulosList.Add(new SelectListItem() {Text = elemento.ArtModelo, Value = elemento.ArtModelo + " " + articulo.ArtNombre});
                        }
                    }
                }
            }
            ViewBag.Articulos = invArticulosList;
            ViewBag.Context = _context;
            return View("Salidas/AgregarSalidas");
        }
        
        [HttpPost]
        [Route("Almacen/Salidas/AgregarSalidas/{proyectoId}")]
        public IActionResult AgregarSalidas(Salida salida)
        {
            ViewBag.Context = _context;
            ViewBag.ProyectoId = salida.ProyectoId;
            _context.Salidas.Add(salida);
            _context.SaveChanges();
            ActualizaSalidaStock(salida.ArtModelo, salida.Cantidad);
            RegistraBitacora("Salidas", "Inserción");
            List<Salida> salidas = _context.Salidas.Where(salidaOut => salidaOut.ProyectoId == salida.ProyectoId).ToList();
            return View("Salidas/VerSalidas", salidas);
        }

        [Route("Almacen/Entradas/VerEntradas/{proyectoId}")]
        public IActionResult VerEntradas(int proyectoId)
        {
            ViewBag.Context = _context;
            ViewBag.ProyectoId = proyectoId;
            List<Entrada> entradas = _context.Entradas.Where(entrada => entrada.ProyectoId == proyectoId).ToList();
            return View("Entradas/VerEntradas", entradas);
        }
        
        [Route("Almacen/Entradas/AgregarEntradas/{proyectoId}")]
        public IActionResult AgregarEntradas(int proyectoId)
        {
            ViewBag.ProyectoId = proyectoId;
            var salidas = _context.Salidas.Where(en => en.ProyectoId == proyectoId);
            var entradas = _context.Entradas.Where(en => en.ProyectoId == proyectoId);
            List<string> entradasModels = entradas.Select(e => e.ArtModelo).ToList();
            List<string> salidasModels = salidas.Select(s => s.ArtModelo).ToList();
            var invArticulosList = new List<SelectListItem>();
            var invArticulos1 = _context.InvArticulos.Where(x => !entradasModels.Contains(x.ArtModelo)).ToList();
            var invArticulos2 = _context.InvArticulos.Where(x => salidasModels.Contains(x.ArtModelo)).ToList();
            var invArticulos = invArticulos1.Where(x => invArticulos2.Contains(x)).ToList();
            var catArticulos = _context.CatArticulos.ToList();
            foreach (var articulo in catArticulos)
            {
                foreach (var elemento in invArticulos)
                {
                    if (elemento.ArtId == articulo.ArtId)
                    {
                        if (elemento.CantidadPrestada > 0)
                        {
                            invArticulosList.Add(new SelectListItem() {Text = elemento.ArtModelo, Value = elemento.ArtModelo + " " + articulo.ArtNombre});
                        }
                    }
                }
            }
            ViewBag.Articulos = invArticulosList;
            ViewBag.Context = _context;
            return View("Entradas/AgregarEntradas");
        }
        
        [HttpPost]
        [Route("Almacen/Entradas/AgregarEntradas/{proyectoId}")]
        public IActionResult AgregarEntradas(Entrada entrada)
        {
            ViewBag.Context = _context;
            ViewBag.ProyectoId = entrada.ProyectoId;
            _context.Entradas.Add(entrada);
            _context.SaveChanges();
            ActualizaEntradaStock(entrada.ArtModelo, entrada.Cantidad);
            RegistraBitacora("Salidas", "Inserción");
            List<Entrada> entradas = _context.Entradas.Where(entradaOut => entradaOut.ProyectoId == entrada.ProyectoId).ToList();
            return View("Entradas/VerEntradas", entradas);
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
        
        public void ActualizaSalidaStock(string artModelo, int cantidad)
        {
            string artModeloRender = "'" + artModelo + "'";
            ExecuteQuery($"exec ActualizaSalidaStock {artModeloRender}, {cantidad}");
        }
        
        public void ActualizaEntradaStock(string artModelo, int cantidad)
        {
            string artModeloRender = "'" + artModelo + "'";
            ExecuteQuery($"exec ActualizaEntradaStock {artModeloRender}, {cantidad}");
        }
    }
}