using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.webui.EmailServices;
using shopapp.webui.Identity;

namespace shopapp.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options=>options.UseSqlite("Data Source=shopDbb"));
            services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options=>{
                options.Password.RequireDigit=true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;


            });

            services.ConfigureApplicationCookie(options =>{
                options.LoginPath="/account/login";
                options.LogoutPath="/account/logout";
                options.AccessDeniedPath="/account/accessdenied";
                options.SlidingExpiration= true;
                options.ExpireTimeSpan= TimeSpan.FromMinutes(5);
                options.Cookie = new CookieBuilder{
                    HttpOnly = true,
                    Name=".ShopApp.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>(); 
            services.AddScoped<IProductRepository,EfCoreProductRepository>(); 
            services.AddScoped<IAdminRepository,EfCoreAdminRepository>(); 
            services.AddScoped<ISepetRepository,EfCoreSepetRepository>(); 
            services.AddScoped<IOrderRepository,EfCoreOrderRepository>(); 


            services.AddScoped<IProductService,ProductManager>(); 
            services.AddScoped<ICategoryService,CategoryManager>(); 
            services.AddScoped<IAdminService,AdminManager>(); 
            services.AddScoped<ISepetService,SepetManager>(); 
            services.AddScoped<IOrderService,OrderManager>(); 


            services.AddScoped<IEmailSender,SmtpEmailSender>(i=>
                new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                    _configuration.GetValue<int>("EmailSender:Port"),
                    _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:UserName"],
                    _configuration["EmailSender:Password"]
                    )
            );

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(); // wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"                
            });

            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication(); 
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {  
                endpoints.MapControllerRoute(
                    name: "checkout", 
                    pattern: "checkout",
                    defaults: new {controller="Cart",action="Checkout"}
                );

                endpoints.MapControllerRoute(
                    name: "cart", 
                    pattern: "cart",
                    defaults: new {controller="Cart",action="Index"}
                );

                endpoints.MapControllerRoute(
                    name: "adminusers", 
                    pattern: "admin/users",
                    defaults: new {controller="Admin",action="users"}
                );

                endpoints.MapControllerRoute(
                    name: "adminindex", 
                    pattern: "admin/index",
                    defaults: new {controller="Admin",action="index"}
                );

                endpoints.MapControllerRoute(
                    name: "adminaddadmin", 
                    pattern: "admin/addadmin",
                    defaults: new {controller="Admin",action="addAdmin"}
                );

                endpoints.MapControllerRoute(
                    name: "adminaddproduct", 
                    pattern: "admin/addproducts",
                    defaults: new {controller="Admin",action="addProduct"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductlist", 
                    pattern: "admin/products",
                    defaults: new {controller="Admin",action="products"}
                );

                endpoints.MapControllerRoute(
                    name: "adminproductlist", 
                    pattern: "admin/products/{id?}",
                    defaults: new {controller="Admin",action="Edit"}
                );

                // localhost/about    
                endpoints.MapControllerRoute(
                    name: "about", 
                    pattern: "about",
                    defaults: new {controller="Shop",action="about"}
                );

                endpoints.MapControllerRoute(
                    name: "productdetails", 
                    pattern: "{url}",
                    defaults: new {controller="Shop",action="details"}
                );

                endpoints.MapControllerRoute(
                    name: "products", 
                    pattern: "products/{category?}",
                    defaults: new {controller="Shop",action="list"}
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
