using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Security;
using tienda_web.Data;
using tienda_web.Models;
using tienda_web.Areas.Identity.Data;

namespace tienda_web.Controllers
{
    public class RolesUsuario : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<tienda_webUsers> userManager;

        public RolesUsuario(RoleManager<IdentityRole> roleManager, UserManager<tienda_webUsers> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Roles()
        {
            var roles = roleManager.Roles;
            RegistraBitacora("AspNetRoles", "Consulta");
            return View(roles);
        }

        public IActionResult CrearRol()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> CrearRol(AspNetRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.Name
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    RegistraBitacora("AspNetRoles", "Inserción");
                    return RedirectToAction("Roles", "RolesUsuario");                                        
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        //[Route("RolesUsuario/AgregarUsuario/{roleId}")]
        public async Task<IActionResult> AgregarUsuario(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"No se encontró el rol especificado.";
                return View("NotFound");
            }
            else
            {
                var model = new List<AspNetUserRole>();

                foreach(var user in userManager.Users)
                {
                    var rol = new AspNetUserRole
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        rol.IsSelected = true;
                    }
                    else
                    {
                        rol.IsSelected = false;
                    }
                    model.Add(rol);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(List<AspNetUserRole> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"No se encontró el rol especificado";
                return View("NotFound");
            }

            for (int i =0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        TempData["Success"] = "Los roles han sido modificados con exito!\nPor favor reinicia la sesión del usuario en cuestión para aplicar los ajustes!";
                    return RedirectToAction("Roles", "RolesUsuario");
                }
            }

            return RedirectToAction("Roles", "RolesUsuario");
        }

            public void ExecuteQuery(string query)
        {
            SqlConnection conection = new SqlConnection("Server= localhost; Database= webstore; Integrated Security=SSPI; Server=localhost\\sqlexpress;");
            conection.Open();
            SqlCommand command = new SqlCommand(query, conection); // Create a object of SqlCommand class
            command.ExecuteNonQuery();
            conection.Close();
        }

        public void RegistraBitacora(string tabla, string operacion)
        {
            ExecuteQuery($"exec RegistraBitacora {tabla}, {operacion}");
        }
    }
}