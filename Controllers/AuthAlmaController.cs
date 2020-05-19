﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthAlmaController : Controller
    {
        private TiendaContext _context;

        public AuthAlmaController(TiendaContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Informacion()
        {
            ViewBag.Context = _context;
            RegistraBitacora("Informacion", "Consulta");
            return View(_context.Proyectos.ToList());
        }

        [Authorize(Roles = "Admin")]
        public void ExecuteQuery(string query)
        {
            SqlConnection conection = new SqlConnection("Server= localhost; Database= webstore; Integrated Security=SSPI; Server=localhost\\sqlexpress;");
            conection.Open();
            SqlCommand command = new SqlCommand(query,conection); // Create a object of SqlCommand class
            command.ExecuteNonQuery();
            conection.Close();
        }

        [Authorize(Roles ="Admin")]
        [Route("AuthAlma/AutorizarSalida/{proyectoId}")]
        public IActionResult AutorizarSalida(int proyectoId)
        {
            ViewBag.Context = _context;
            Proyecto proyecto = _context.Proyectos.Find(proyectoId);
            proyecto.AuthSalida = "SI";
            _context.Proyectos.Update(proyecto);
            _context.SaveChanges();
            TempData["Success"] = $"Salida autorizada para el proyecto {proyecto.ProyectoName}!";

            return View("Informacion", _context.Proyectos.ToList());
        }

        [Authorize(Roles = "Admin")]
        [Route("AuthAlma/AutorizarEntrada/{proyectoId}")]
        public IActionResult AutorizarEntrada(int proyectoId)
        {
            ViewBag.Context = _context;
            Proyecto proyecto = _context.Proyectos.Find(proyectoId);
            proyecto.AuthEntrada = "SI";
            _context.Proyectos.Update(proyecto);
            _context.SaveChanges();
            TempData["Success"] = $"Entrada autorizada para el proyecto {proyecto.ProyectoName}!";

            return View("Informacion", _context.Proyectos.ToList());
        }

        [Authorize(Roles = "Admin")]
        public void RegistraBitacora(string tabla, string operacion)
        {
            ExecuteQuery($"exec RegistraBitacora {tabla}, {operacion}");
        }
        
    }
}