using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MyHomeBlazorApp.BlazorData;
using System.Security.Cryptography.X509Certificates;

namespace MyHomeBlazorApp
{
    public class Program
    {
        public class Constants
        {
            public const string XML_DATA_PATH = @"C:\Temp\usersListTestData111.xml";
            public const string SAVE_QR_CODE_PATH = "C:\\Users\\shiranco.DESKTOP-HRN41TE\\Desktop\\qrcodes\\qrCode.png";
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddBlazorBootstrap();
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}