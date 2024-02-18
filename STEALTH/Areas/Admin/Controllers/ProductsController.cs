using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using STEALTH.Areas.Admin.VM;
using STEALTH.DAL;
using STEALTH.Models;

namespace STEALTH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        => View(await _context.Products.ToListAsync());
        
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.CategoryProducts)
                .ThenInclude(c=>c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Create() 
        {
            var categoryProducts = _context.CategoryProducts.ToListAsync();
            var model = new ProductVM();
            model.Categoris = new List<Categori>();
            foreach (var category in await categoryProducts)
            {
                model.Categoris.Add(new Categori()
                {
                    CategoriId = category.Id,
                    Name = category.Name,
                    Exist = false
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var product = new Product
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Image = model.Image
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            foreach (var categori in model.Categoris)
            {
                if (categori.Exist) await _context.CategoryProductBrigs.
                          AddAsync(new CategoryProductBrig
                          {
                              CategoryId = categori.CategoriId,
                              ProductId = product.Id
                          });
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products
                .Include(p => p.CategoryProducts)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            var categoryProducts = _context.CategoryProducts.ToListAsync();
            var model = new ProductVM
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price
            };
            model.Categoris = new List<Categori>();
            foreach (var category in await categoryProducts)
            {
                model.Categoris.Add(new Categori()
                {
                    CategoriId = category.Id,
                    Name = category.Name,
                    Exist = product.CategoryProducts.Any(c => c.CategoryId == category.Id)
                });
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductVM model)
        {
            ModelState.Remove("ImageFile");
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);
            var product = new Product
            {
                Id = id,
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                Image = model.Image,
                CategoryProducts = await _context.CategoryProductBrigs.Where(c => c.ProductId == id).ToListAsync()
            };

            try
            {
                
                foreach (var categori in model.Categoris)
                {
                    if(product.CategoryProducts.
                        Any(c=>c.CategoryId == categori.CategoriId) && !categori.Exist)
                    _context.CategoryProductBrigs.Remove(await _context.CategoryProductBrigs
                            .FirstOrDefaultAsync(cb => cb.CategoryId == categori.CategoriId && cb.ProductId == id));
                    
                    else if (!product.CategoryProducts.
                        Any(c => c.CategoryId == categori.CategoriId) && categori.Exist)
                        await _context.CategoryProductBrigs.
                              AddAsync(new CategoryProductBrig
                              {
                                  CategoryId = categori.CategoriId,
                                  ProductId = product.Id
                              });
                    
                }
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == product.Id))      
                    return NotFound();               
                else throw;          
            }
            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
