using Microsoft.AspNetCore.Identity;
using MyHomeBlazorApp.BlazorData;

namespace MyHomeBlazorApp.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<MyHomeBlazorAppUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<MyHomeBlazorAppUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
