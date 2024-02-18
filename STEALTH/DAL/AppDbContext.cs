using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STEALTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, int>  
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<int>>().HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
            builder.Entity<CategoryProductBrig>()
                .HasKey(cp => new { cp.ProductId, cp.CategoryId });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<CategoryProductBrig> CategoryProductBrigs { get; set; }
        public DbSet<Basket> Baskets{ get; set; }
        public DbSet<BasketProduct> BasketProducts{ get; set; }


    }
}
