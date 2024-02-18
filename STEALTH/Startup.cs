using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STEALTH.DAL;
using STEALTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(_configuration["ConnectionStrings:Default"]);
            });
            services.AddIdentity<User, Role>(
                x => {
                    x.Password.RequireUppercase = false;
                    x.Password.RequireLowercase = false;
                    x.Password.RequireNonAlphanumeric = false;
                    x.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<AppDbContext>();
             
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Dashbord}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var scopeF = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeF.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                if(!roleManager.RoleExistsAsync("Admin").Result)
                {
                    roleManager.CreateAsync(new Role { Name = "Admin" }).Wait();
                    User admin = new()
                    {
                        UserName = "admin",
                        Email = "admin@admin"
                    };
                    var result = userManager.CreateAsync(admin, "Admin123!").Result;
                    if (!result.Succeeded) throw new Exception("Admin don't creted");
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
                   
            }
        }
    }
}
