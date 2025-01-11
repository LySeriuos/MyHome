using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;
using MyHomeBlazorApp.BlazorData;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyHomeBlazorApp.Data;
using MyHomeBlazorApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Components.Authorization;
using MyHomeBlazorApp.Areas.Identity;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MyHomeBlazorAppContextConnection") ?? throw new InvalidOperationException("Connection string 'MyHomeBlazorAppContextConnection' not found.");

builder.Services.AddDbContext<MyHomeBlazorAppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MyHomeBlazorAppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyHomeBlazorAppContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
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
// routing url to physical path "Files"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
    RequestPath = "/Files"
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


namespace MyHomeBlazorApp
{
    public class Program
    {
        //TODO: create autogenerated path
        public class Constants
        {
            public const string XML_DATA_PATH = @"C:\Temp\usersListTestData111.xml";
            public const string SAVE_QR_CODE_PATH = "C:\\Users\\shiranco.DESKTOP-HRN41TE\\Desktop\\qrcodes\\qrCode.png";
            public const string SAVE_UPLOADED_FILES = "C:\\Users\\shiranco.DESKTOP-HRN41TE\\Desktop\\UploadedFiles\\";
            public const string BASE_API_URL = "https://www.google.se/search?q=";
        }
    }
}