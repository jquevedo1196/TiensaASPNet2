﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using tienda_web.Models;

namespace tienda_web.Controllers
{
    [Authorize]
    public class TiendaController : Controller
    {
        private TiendaContext _context;
        
        public TiendaController(TiendaContext context)
        {
            _context = context;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}