using identity.Data;
using identity.Data.Repositorys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils.Messaging;
using PersianTranslation.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity
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
            services.AddControllersWithViews();
            services.AddDbContext<App_Context>(option =>

            {
                option.UseSqlServer("Data Source=.;Initial Catalog=TestIdentity_DB;Integrated Security=true");
            }
          );

            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.Password.RequiredUniqueChars = 0;
                option.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<App_Context>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PersianIdentityErrorDescriber>();

            services.AddScoped<Data.Repositorys.IMessageSender, MessageSender>();


            services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientId = "747342199235-r3hbrg3up93hicapqd0t8dttvck4d3dk.apps.googleusercontent.com";
                option.ClientSecret = "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";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
