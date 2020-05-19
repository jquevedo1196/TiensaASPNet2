using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace tienda_web.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult CrearRol()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> CrearRol(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return RedirectToAction("/");
        }
    }
}