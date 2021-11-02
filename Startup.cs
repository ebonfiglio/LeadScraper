using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ElectronNET.API;
using LeadScraper.Infrastructure;
using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure.Repositories;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Services;
using AutoMapper;
using LeadScraper.Domain.Mapper;
using Microsoft.EntityFrameworkCore;
using ElectronNET.API.Entities;

namespace LeadScraper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using (var client = new ApplicationDbContext())
            {
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddEntityFrameworkSqlite()
         .AddDbContext<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAutoMapper(typeof(LeadScraperProfile));
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IWhoIsServerRepository, WhoIsServerRepository>();
            services.AddScoped<IWhoIsServerService, WhoIsServerService>();
            services.AddScoped<IWriteFileService, WritefileService>();
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
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

           Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
           // if (HybridSupport.IsElectronActive)
            //{
               // ElectronBootstrap();
            //}
        }
        //public async void ElectronBootstrap()
        //{
        //    WebPreferences wp = new WebPreferences();
        //    wp.NodeIntegration = false;
        //    var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
        //    {
        //        Width = 1152,
        //        Height = 940,
        //        Show = false,
        //        WebPreferences = wp
        //    });

        //    await browserWindow.WebContents.Session.ClearCacheAsync();

        //    browserWindow.OnReadyToShow += () => browserWindow.Show();
        //    browserWindow.SetTitle(Configuration["DemoTitleInSettings"]);
        //}
    }
}
