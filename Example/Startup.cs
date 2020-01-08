using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Example
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // получаем строку подключения из файла конфигурации
            string connection1 = Configuration.GetConnectionString("DefaultConnection");
            string connection2 = Configuration.GetConnectionString("UserConnection");
            // добавляем контекст MobileContext в качестве сервиса в приложение
            services.AddDbContext<ShortContext>(options =>
                options.UseSqlServer(connection1));
            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(connection2));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserContext>();
            services.AddRazorPages();
            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseExceptionHandler("/Home/Error");
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("redirect", "{controller=R}/{action=L}/{id}");
            });
        }
    }
}
