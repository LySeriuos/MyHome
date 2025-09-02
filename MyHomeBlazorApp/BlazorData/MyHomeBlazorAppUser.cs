using Microsoft.AspNetCore.Identity;
using MyHome.Models;

namespace MyHomeBlazorApp.BlazorData
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class MyHomeBlazorAppUser : IdentityUser
    {
        public UserProfile? UserProfile { get; set; }
        ////public Role Role { get; set; }
        //public UserProfile ID;

        //  public int UserProfileID;

    }
//public enum Role
//    {
//        Admin,
//        User
//    }  

}
