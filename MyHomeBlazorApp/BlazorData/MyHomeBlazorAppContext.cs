using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyHomeBlazorApp.BlazorData
{
    public class MyHomeBlazorAppContext(DbContextOptions<MyHomeBlazorAppContext> options) : IdentityDbContext<MyHomeBlazorAppUser>(options)
    {


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
    new IdentityRole
    {
        Id = "3470e658-222c-4991-8e98-8c029cac94b5",
        Name = "User",
        NormalizedName = "USER",
        ConcurrencyStamp = "fe2dad0d-c1e6-4bc4-a33d-60d7001cf2ce"
    },
    new IdentityRole
    {
        Id = "b6c33b21-8d39-4910-ac5e-20391b02b8a2",
        Name = "Admin",
        NormalizedName = "ADMIN",
        ConcurrencyStamp = "0b422cdc-a9ee-4c2e-ab86-02fec50f1865"
    });

        }
    }
}

