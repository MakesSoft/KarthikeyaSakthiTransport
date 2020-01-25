using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace KarthikeyasakthiTransport
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
            services.AddMvc();

            //services.AddJsReport(new LocalReporting()
            //    .UseBinary(JsReportBinary.GetBinary())
            //    .AsUtility()
            //    .Create());

            services.AddMvc(options =>
                {
                    options.Filters.Add(new CustomExceptionFilterAttribute());
                    options.ReturnHttpNotAcceptable = true;
                    // options.OutputFormatters = xml
                })
               .AddJsonOptions(options =>
               {
                   //For Maintaining Json Format
                   options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
               });

            // For Setting Session Timeout
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".KarthikeyasakthiTransport";
            });

            var connection = Configuration.GetConnectionString("DatabaseConnection");

            services.AddDbContext<DatabaseContext>(options => options.UseMySql(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Error");
            }

            var supportedCultures = new[]
            {
             new CultureInfo("en-GB")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Login}/{id?}");
            });

            //RotativaConfiguration.Setup(env);
        }
    }
}