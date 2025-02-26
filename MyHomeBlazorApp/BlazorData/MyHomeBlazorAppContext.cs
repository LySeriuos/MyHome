using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace MyHomeBlazorApp.BlazorData
{
    public class MyHomeBlazorAppContext(DbContextOptions<MyHomeBlazorAppContext> options) : IdentityDbContext<MyHomeBlazorAppUser>(options)
    {
    }
}

