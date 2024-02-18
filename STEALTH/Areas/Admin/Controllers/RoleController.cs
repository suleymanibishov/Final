using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STEALTH.Areas.Admin.VM;
using STEALTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Areas.Admin.Controllers
{
    
 
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var values = _roleManager.Roles.ToList();
            return View(values);
        }

        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleVM model)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role()
                {
                    Name = model.Name
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }

        public IActionResult Update(int id)
        {
            Role role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            RoleVM model = new()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleVM model)
        {

            if (ModelState.IsValid)
            {
                Role role = _roleManager.Roles.FirstOrDefault(r => r.Id == model.Id);
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Role role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);

            await _roleManager.DeleteAsync(role);
            return RedirectToAction("index");
        }

        public IActionResult UserListRole()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
        public async Task<IActionResult> AssingRole(int id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            var roles = _roleManager.Roles.ToList();
            TempData["UserId"] = id;
            var userRoles = await _userManager.GetRolesAsync(user);
            List<RoleAssingVM> models = new List<RoleAssingVM>();
            foreach (var role in roles)
            {
                RoleAssingVM m = new RoleAssingVM();
                m.RoleId = role.Id;
                m.Name = role.Name;
                m.Exist = userRoles.Contains(role.Name);
                models.Add(m);
            }

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> AssingRole(List<RoleAssingVM> model)
        {
            int id = (int)TempData["UserId"];
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            foreach (var item in model)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }

            return RedirectToAction("UserListRole");
        }

    }
}
