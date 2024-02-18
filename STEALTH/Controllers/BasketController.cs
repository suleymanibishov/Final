using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using STEALTH.DAL;
using STEALTH.Models;
using STEALTH.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Controllers
{
    [Route("Basket")]
    public class BasketController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly AppDbContext _db;
        public BasketController
            (UserManager<User> userManager, AppDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        [Route("Index")]
        public async Task<IActionResult> Index() 
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return View(new BasketVM
                {
                    Basket = new Basket
                    {
                        BasketProducts = await GetBasketProductsFromCookieAsync()
                    }
                });
            }
            var basket = await _db.Baskets
            .FirstOrDefaultAsync(b => b.UserId == user.Id);
            foreach (var item in GetCartItemsFromCookie())
            {
                var basketProduct1 = await _db.BasketProducts.
                FirstOrDefaultAsync(bp => bp.ProductId == item.ProductId);
                if (basketProduct1 == null)
                {
                    basketProduct1 = new BasketProduct
                    {
                        BasketId = basket.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };
                    await _db.BasketProducts.AddAsync(basketProduct1);
                }
                else basketProduct1.Quantity += item.Quantity;
                await _db.SaveChangesAsync();
            }
            basket = await _db.Baskets.Include(x =>
            x.BasketProducts).ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(b => b.UserId == user.Id);
            Response.Cookies.Delete("GuestCart");
            return View(new BasketVM { Basket = basket});
        }
        [Route("AddBasket/{id}/{count?}")]
        public async Task<IActionResult> AddBasket(int id, int count = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            if (user == null)
            {
                AddToCartForGuest(id, count);
                return Ok();
            }
            var userbasket = await _db.Baskets.
                FirstOrDefaultAsync(b => b.UserId == user.Id);
            if(userbasket == null){
                userbasket = new Basket { UserId = user.Id };
                await _db.Baskets.AddAsync(userbasket);
                await _db.SaveChangesAsync();
            }
            var basketProduct = await _db.BasketProducts.
                FirstOrDefaultAsync(bp => bp.BasketId == 
                userbasket.Id && bp.ProductId == id);
            if (basketProduct == null)
            {
                basketProduct = new BasketProduct
                {
                    BasketId = userbasket.Id,
                    ProductId = product.Id,
                    Quantity = count
                };
                await _db.BasketProducts.AddAsync(basketProduct);
            }
            else basketProduct.Quantity += count;
            await _db.SaveChangesAsync();
            foreach (var item in GetCartItemsFromCookie())
            {
                var basketProduct1 = await _db.BasketProducts.
                FirstOrDefaultAsync(bp => bp.ProductId == item.ProductId);
                if (basketProduct1 == null)
                {
                    basketProduct1 = new BasketProduct
                    {
                        BasketId = userbasket.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };
                    await _db.BasketProducts.AddAsync(basketProduct1);
                } else basketProduct1.Quantity += item.Quantity;
                await _db.SaveChangesAsync();
            }
            Response.Cookies.Delete("GuestCart");
            return Ok();
        }


        [Route("DeleteBasket/{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            try
            {
                User user = await _userManager.GetUserAsync(User);
                if(user == null) { }
                var basket = await _db.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);
                if(basket != null)
                {

                    var ubp = await _db.BasketProducts
                        .FirstOrDefaultAsync(bp => bp.BasketId == basket.Id  && bp.ProductId == id);
                    if(ubp!=null)
                    {
                        _db.BasketProducts.Remove(ubp);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                string str = e.InnerException.Message;
            }
            return Ok();
        }


        [Route("UpdateQuantity/{id}/{count}")]
        public async Task<IActionResult> UpdateQuantity(int id, int count)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                UpdateQuantityForGuest(id, count);
                var bp = await GetBasketProductsFromCookieAsync();
                bp.FirstOrDefault(p => p.ProductId == id).Quantity = count;

                return PartialView("BasketTablePV", new BasketVM
                {
                    Basket = new Basket
                    {
                        BasketProducts = bp
                    }
                });
                
            }
            var basket = await _db.Baskets.Include(x =>
            x.BasketProducts).ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(b => b.UserId == user.Id);
            var basketProduct = await _db.BasketProducts.FirstOrDefaultAsync(bp => bp.ProductId == id);
            basketProduct.Quantity = count;

            //List<BasketProduct> items = await _db.BasketProducts.ToListAsync();
            //string userName = User.Identity.Name;
            //user = _db.Users.Include(b => b.Baskets).FirstOrDefault(u => u.Id == user.Id);
            //user.Baskets.FirstOrDefault(p => p.Id == id).Count = count;
            _db.SaveChanges();
            return PartialView("BasketTablePV",new BasketVM { Basket = basket});
        }

        //--------------------------------------------------------------------
        private void AddToCartForGuest(int productId, int quantity)
        {
            var cartItems = GetCartItemsFromCookie(); // Daha önce eklenmiş ürünleri al
                                                      // Yeni ürünü ekle
            var product = cartItems.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
                cartItems.Add(new BasketProduct
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            else product.Quantity+=quantity;

            // CartItems listesini bir JSON formatına çevir
            var cartJson = JsonConvert.SerializeObject(cartItems);

            // Cookie'e JSON verisini kaydet
            Response.Cookies.Append("GuestCart", cartJson);
        }

        private void UpdateQuantityForGuest(int productId, int quantity)
        {
            var cartItems = GetCartItemsFromCookie(); // Daha önce eklenmiş ürünleri al
                                                      // Yeni ürünü ekle
            var product = cartItems
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)  
                product.Quantity = quantity;

            // CartItems listesini bir JSON formatına çevir
            var cartJson = JsonConvert.SerializeObject(cartItems);

            // Cookie'e JSON verisini kaydet
            Response.Cookies.Append("GuestCart", cartJson);
        }

        private async Task<List<BasketProduct>> GetBasketProductsFromCookieAsync()
        {
            var basketProducts = GetCartItemsFromCookie();
            var products = await _db.Products.ToListAsync();
            foreach (var bp in basketProducts)
            {
                bp.Product = products.FirstOrDefault(p => p.Id == bp.ProductId);
            }
            return basketProducts;
        }
        private List<BasketProduct> GetCartItemsFromCookie()
        {
            var cartJson = Request.Cookies["GuestCart"];
            return string.IsNullOrEmpty(cartJson)
                ? new List<BasketProduct>()
                : JsonConvert.DeserializeObject<List<BasketProduct>>(cartJson);
        }





        //------------------------------------------------------


        //[HttpPost]
        //public async Task<IActionResult> AddBasket1(int id)
        //{

        //    try
        //    {
        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            AddToCartForGuest(id, 1);
        //            TempData["BasketCount"] = GetCartItemsFromCookie().Count;
        //            return Json(new { Count = GetCartItemsFromCookie().Count });
        //        }
        //        /**/
        //        var guestCartItems = GetCartItemsFromCookie();
        //        string userName = User.Identity.Name;
        //        ApplicationUser user = await _userManager.FindByNameAsync(userName);
        //        user = _db.Users.Include(b => b.Baskets).FirstOrDefault(u => u.Id == user.Id);
        //        /**/
        //        user.Baskets.AddRange(guestCartItems);
        //        _db.AddBasket(new Basket() { ItemId = id, UserId = user.Id });
        //        _db.SaveChanges();
        //        TempData["BasketCount"] = user.Baskets.Count;
        //        /**/
        //        Response.Cookies.Delete("GuestCart");
        //        return Json(new { Count = user.Baskets.Count });
        //    }
        //    catch (Exception e)
        //    {
        //        string str = e.InnerException.Message;
        //    }
        //    return Ok();
        //}

    }
}
