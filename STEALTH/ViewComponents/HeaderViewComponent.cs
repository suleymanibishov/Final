using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STEALTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        //UserManager<User> _userManager;
        //public HeaderViewComponent(UserManager<User> userManager)
        //=>    _userManager = userManager;
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //}
            return View();
        }
    }
}
