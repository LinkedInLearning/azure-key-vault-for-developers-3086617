using FilePortal.Dal;
using FilePortal.Dal.Model;
using Microsoft.AspNetCore.Identity;

public static class DataSeed
{
    public static void SeedUsers(this WebApplication app)
    {
      
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            if (!db.Database.CanConnect()) return;

            if (userManager.FindByNameAsync("john").Result == null)
            {
                var user = new ApplicationUser();
                user.UserName = "john@portalapp.com";
                user.Email = "john@portalapp.com";
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync
                (user, "John@123#").Result;


            }


            if (userManager.FindByNameAsync("jane").Result == null)
            {
                var user = new ApplicationUser();
                user.UserName = "jane@portalapp.com";
                user.Email = "jane@portalapp.com";

                var result = userManager.CreateAsync
                (user, "Jane@123#").Result;
            }
        }


    }
}