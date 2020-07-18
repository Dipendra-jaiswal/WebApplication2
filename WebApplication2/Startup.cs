using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication2.Models;
using Serilog;
using Microsoft.Extensions.Logging;

namespace WebApplication2
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

            //serilog configuration
            //var seriLogger = new LoggerConfiguration()
            //    .WriteTo.RollingFile(@"E:\\dipendra.txt")
            //    .CreateLogger();

            //services.AddLogging(builder => {
            //    builder.AddFilter("Microsoft",LogLevel.Debug);
            //    builder.AddFilter("System", LogLevel.Error);

            //    builder.AddSerilog(logger: seriLogger,dispose:true);
            //});

            services.AddSession(option => {
                option.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            //DI resolve method
            services.AddSingleton<IStudentRepository,MockStudentRepository>();
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
                 app.UseExceptionHandler("/Error");

                //plaint text with status code and error msg
                // app.UseStatusCodePages();

                //redirect to other page or error page
                // app.UseStatusCodePagesWithRedirects("/Error/{0}");

                 app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
