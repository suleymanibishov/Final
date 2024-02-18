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
    public class DashbordController : Controller
    {
        UserManager<User> _userManager;
        public DashbordController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            var model = new AdminDashboardIndex();
            model.Users = new List<UserVM>();
            foreach (var user in _userManager.Users)
            {

                //if(_userManager.GetRolesAsync(user).IsCompletedSuccessfully)
                model.Users.Add(new UserVM { Name = user.UserName, Email = user.Email });            
            }
            return View(model);
          

        }
    }
}
