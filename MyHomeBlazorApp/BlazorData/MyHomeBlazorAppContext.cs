using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyHomeBlazorApp.BlazorData
{
    public class MyHomeBlazorAppContext(DbContextOptions<MyHomeBlazorAppContext> options) : IdentityDbContext<MyHomeBlazorAppUser>(options)
    {
    }
}
