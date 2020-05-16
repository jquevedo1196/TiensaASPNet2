using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    public class ProyectoController : Controller
    {
        private TiendaContext _context;
        
        public ProyectoController(TiendaContext context)
        {
            _context = context;
        }
        
        public IActionResult Proyectos()
        {
            ViewBag.Context = _context;
            RegistraBitacora("Proyectos", "Consulta");
            return View(_context.Proyectos.ToList());
        }

        [Route("Proyecto/EditarProyecto/{proyectoId}")]
        public IActionResult EditarProyecto(int proyectoId)
        {
            var empresasList = new List<SelectListItem>();
            var empresas = _context.Empresas.ToList();
            foreach (var empresa in empresas)
            {
                empresasList.Add(new SelectListItem(){Text = empresa.EmpresaRfc, Value = empresa.EmpresaRfc + " " + empresa.EmpresaRazSoc});
            }
            var userList = new List<SelectListItem>();
            var users = _context.AspNetUsers.ToList();
            foreach (var user in users)
            {
                userList.Add(new SelectListItem(){Text = user.Id, Value = user.RFC + " " + user.Nombres + " " + user.ApPat + " " + user.ApMat});
            }
            ViewBag.Empresas = empresasList;
            ViewBag.AspNetUsers = userList;
            Proyecto proyecto = _context.Proyectos.Find(proyectoId);
            return View(proyecto);
        }
        
        [HttpPost]
        [Route("Proyecto/EditarProyecto/{proyectoId}")]
        public IActionResult EditarProyecto(Proyecto proyecto)
        {
            proyecto.AuthEntrada = "NO";
            proyecto.AuthSalida = "NO";
            ViewBag.Context = _context;
            _context.Proyectos.Update(proyecto);
            _context.SaveChanges();
            RegistraBitacora("Proyectos", "Edición");
            return View("Proyectos", _context.Proyectos.ToList());
        }

        [Route("Proyecto/GenerarReporteSalida/{proyectoId}")]
        public IActionResult GenerarReportes(int proyectoId)
        {
            string sql =
                "SELECT Salidas.ArtModelo as 'Modelo/Serie', CatArticulos.ArtNombre as 'Artículo', Salidas.Cantidad, Salidas.Fecha FROM CatArticulos INNER JOIN InvArticulos ON CatArticulos.ArtId = InvArticulos.ArtId INNER JOIN Salidas ON InvArticulos.ArtModelo = Salidas.ArtModelo WHERE Salidas.ProyectoId = ";
            List<Salida> salidas = _context.Salidas.FromSqlRaw(sql + proyectoId).ToList();
            using (XmlWriter writer =
                XmlWriter.Create(@"C:\Users\jenri\OneDrive\Escritorio\DocsProyectoBD\XML\facturas.xml"))
            {
                writer.WriteStartElement("ProyectoId", salidas[0].ProyectoId.ToString());
                foreach (var salida in salidas)
                {
                    writer.WriteStartElement("ArtModelo", salida.ArtModelo.ToString());
                    writer.WriteElementString("ArtNombre", salida.ArtNombre.ToString());
                    writer.WriteElementString("Cantidad", salida.Cantidad.ToString());
                    writer.WriteElementString("Fecha", salida.Fecha.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();
                GeneratePdfFile(ventaId, facturas);
            }

            return View("Venta", facturas);
        }

        public IActionResult CrearProyecto()
        {
            var empresasList = new List<SelectListItem>();
            var empresas = _context.Empresas.ToList();
            foreach (var empresa in empresas)
            {
                empresasList.Add(new SelectListItem(){Text = empresa.EmpresaRfc, Value = empresa.EmpresaRfc + " " + empresa.EmpresaRazSoc});
            }
            var userList = new List<SelectListItem>();
            var users = _context.AspNetUsers.ToList();
            foreach (var user in users)
            {
                userList.Add(new SelectListItem(){Text = user.Id, Value = user.RFC + " " + user.Nombres + " " + user.ApPat + " " + user.ApMat});
            }
            ViewBag.Empresas = empresasList;
            ViewBag.AspNetUsers = userList;
            return View();
        }
        
        [HttpPost]
        public IActionResult CrearProyecto(Proyecto proyecto)
        {
            proyecto.AuthEntrada = "NO"; 
            proyecto.AuthSalida = "NO";
            ViewBag.Context = _context;
            _context.Proyectos.Add(proyecto);
            _context.SaveChanges();
            RegistraBitacora("Proyectos", "Inserción");
            return View("Proyectos", _context.Proyectos.ToList());
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