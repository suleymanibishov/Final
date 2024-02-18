using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEALTH.DAL;
using STEALTH.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Controllers
{
    [Route("[controller]")]
    public class ShopController : Controller
    {
        AppDbContext _db;
        public ShopController(AppDbContext db)
        {
            _db = db;
        }
        [Route("[action]/{page?}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 4;
            int productCount = await _db.Products.CountAsync();
            var model = new ShopVM
            {
                Products = await _db.Products
                .Include(p => p.CategoryProducts)
                .ThenInclude(c => c.Category)
                .Skip(pageSize * page - pageSize)
                .Take(pageSize)
                .ToListAsync(),
                Categories = await _db.CategoryProducts
                .ToListAsync(),
                Page = page,
                PageCount = productCount / pageSize + 
                (productCount % pageSize > 0 ? 1 : 0)

            };
            return View(model);
        }
        private IActionResult Index(ShopVM model)
        {
            return View(model);
        }
        [Route("[action]/{filter}/{page?}")]
        public async Task<IActionResult> Filter(string filter, int page = 1)
        {
            int _page = page;
            int pageSize = 4;
            int productCount = await _db.Products.
                Where(p => p.CategoryProducts.
                Any(c => c.Category.Name == filter)).
                CountAsync();
            var model = new ShopVM
            {
                Products = await _db.Products
                .Include(p => p.CategoryProducts)
                .ThenInclude(c => c.Category)
                .Where(p => p.CategoryProducts.Any(c => c.Category.Name == filter))
                .Skip(pageSize * _page - pageSize)
                .Take(pageSize)
                .ToListAsync(),
                Categories = await _db.CategoryProducts
                .ToListAsync(),
                Categori = filter,
                Page = _page,
                PageCount = productCount / pageSize +
                (productCount % pageSize > 0 ? 1 : 0)
            };
            //if (page != null)
            return View(nameof(Index),model);
            //return PartialView("ShopProductsPV", model);
        }

    }
}
