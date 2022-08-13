//using Android.Net;
//using Android.SE.Omapi;
using LeadScraper.Domain.Contracts;
using LeadScraper.Domain.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;
using MatBlazor;
using LeadScraper.Infrastructure.Repositories;
using LeadScraper.Infrastructure.Contracts;
using LeadScraper.Infrastructure;
using LeadScraper.Domain.Mapper;

namespace LeadScraper.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddHttpClient();
            builder.Services.AddEntityFrameworkSqlite()
         .AddDbContext<ApplicationDbContext>();
            builder.Services.AddAutoMapper(typeof(LeadScraperProfile));
            
            builder.Services.AddTransient<ISearchSettingsRepository, SearchSettingsRepository>();
            builder.Services.AddTransient<ISettingsRepository, SettingsRepository>();
            builder.Services.AddTransient<IWhoIsServerRepository, WhoIsServerRepository>();
            builder.Services.AddTransient<ISearchService, BingService>();
            builder.Services.AddTransient<ILoggingService, LoggingService>();
            builder.Services.AddTransient<ISearchSettingService, SearchSettingService>();
            builder.Services.AddTransient<IWriteFileService, WritefileService>();
            builder.Services.AddTransient<ISettingsService, SettingsService>();
            
            builder.Services.AddMatBlazor();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif


            return builder.Build();
        }
    }
}