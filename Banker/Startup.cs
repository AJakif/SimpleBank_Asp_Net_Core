
using Banker.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Banker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICommonHelper, CommonHelper>(); //
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc().AddSessionStateTempDataProvider(); // service for session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.Cookie.Name = "Bank";
                
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/",
                    defaults: new { controller = "Home", action = "Index", });

                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "Registration",
                    defaults: new { controller = "Account", action = "Register", });

                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "Login",
                    defaults: new { controller = "Account", action = "Login", });

                endpoints.MapControllerRoute(
                    name: "logout",
                    pattern: "Logout",
                    defaults: new { controller = "Account", action = "Logout", });

                endpoints.MapControllerRoute(
                    name: "dashboard",
                    pattern: "Home/Dashboard",
                    defaults: new { controller = "Account", action = "Welcome", });

                endpoints.MapControllerRoute(
                    name: "balance",
                    pattern: "Home/Balance",
                    defaults: new { controller = "Transection", action = "Balance", });

                endpoints.MapControllerRoute(
                    name: "withdraw",
                    pattern: "Home/Withdraw",
                    defaults: new { controller = "Transection", action = "Withdraw", });

                endpoints.MapControllerRoute(
                    name: "deposit",
                    pattern: "Home/Deposit",
                    defaults: new { controller = "Transection", action = "Deposit", });

                endpoints.MapControllerRoute(
                    name: "report",
                    pattern: "Home/Report",
                    defaults: new { controller = "Report", action = "Report", });

                endpoints.MapControllerRoute(
                    name: "audit",
                    pattern: "Home/Audit",
                    defaults: new { controller = "Audit", action = "Index", });




                endpoints.MapRazorPages();
            });
        }
    }
}
