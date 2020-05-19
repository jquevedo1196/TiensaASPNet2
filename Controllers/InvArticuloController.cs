﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    [Authorize(Roles = "PM")]
    public class InvArticuloController : Controller
    {
        private TiendaContext _context;

        public InvArticuloController(TiendaContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "PM")]
        public IActionResult InvArticulos()
        {
            ViewBag.Context = _context;
            RegistraBitacora("InvArticulos", "Consulta");
            return View(_context.InvArticulos.ToList());
        }

        [Authorize(Roles = "PM")]
        [Route("InvArticulo/EditarInvArticulo/{artModelo}")]
        public IActionResult EditarInvArticulo(string artModelo)
        {
            var catArticulosList = new List<SelectListItem>();
            var catArticulos = _context.CatArticulos.ToList();
            foreach (var articulo in catArticulos)
            {
                catArticulosList.Add(new SelectListItem()
                    {Text = articulo.ArtId.ToString(), Value = articulo.ArtNombre});
            }
            ViewBag.CatArticulos = catArticulosList;
            InvArticulo invArticulo = _context.InvArticulos.Find(artModelo);
            return View(invArticulo);
        }

        [Authorize(Roles = "PM")]
        [HttpPost]
        [Route("InvArticulo/EditarInvArticulo/{artModelo}")]
        public IActionResult EditarInvArticulo(InvArticulo invArticulo)
        {
            if (ModelState.IsValid)
            {
                invArticulo.CantidadEnAlmacen = invArticulo.CantidadNeta - invArticulo.CantidadPrestada;
                ViewBag.Context = _context;
                _context.InvArticulos.Update(invArticulo);
                _context.SaveChanges();
                RegistraBitacora("InvArticulos", "Edición");
                return View("InvArticulos", _context.InvArticulos.ToList());
            }
            return View();
        }

        [Authorize(Roles = "PM")]
        public IActionResult CrearInvArticulo()
        {
            var catArticulosList = new List<SelectListItem>();
            var catArticulos = _context.CatArticulos.ToList();
            foreach (var articulo in catArticulos)
            {
                catArticulosList.Add(new SelectListItem()
                    {Text = articulo.ArtId.ToString(), Value = articulo.ArtNombre});
            }
            ViewBag.CatArticulos = catArticulosList;
            return View();
        }

        [Authorize(Roles = "PM")]
        [HttpPost]
        public IActionResult CrearInvArticulo(InvArticulo invArticulo)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Context = _context;
                _context.InvArticulos.Add(invArticulo);
                _context.SaveChanges();
                RegistraBitacora("InvArticulos", "Inserción");
                return View("InvArticulos", _context.InvArticulos.ToList());
            }

            return View();
        }

        [Authorize(Roles = "PM")]
        public void ExecuteQuery(string query)
        {
            SqlConnection conection =
                new SqlConnection(
                    "Server= localhost; Database= webstore; Integrated Security=SSPI; Server=localhost\\sqlexpress;");
            conection.Open();
            SqlCommand command = new SqlCommand(query, conection); // Create a object of SqlCommand class
            command.ExecuteNonQuery();
            conection.Close();
        }

        [Authorize(Roles = "PM")]
        public void RegistraBitacora(string tabla, string operacion)
        {
            ExecuteQuery($"exec RegistraBitacora {tabla}, {operacion}");
        }
    }
}