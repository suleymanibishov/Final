using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEALTH.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriController : Controller
    {
        AppDbContext _db;
        public CategoriController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {           
            return View(await _db.CategoryProducts.ToListAsync());
        }
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Models.CategoryProduct category)
        {
            if (!ModelState.IsValid) return View(category);
            if (await _db.CategoryProducts.AnyAsync(c =>
            c.Name == category.Name)) return View(category);
            await _db.CategoryProducts.AddAsync(category);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.CategoryProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();
            _db.CategoryProducts.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
