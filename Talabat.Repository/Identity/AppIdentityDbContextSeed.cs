using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed 
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Khaled Maher",
                    Email = "khaledmaher.net@gmail.com",
                    UserName = "khaledmaher.net",
                    PhoneNumber = "0123456789",
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
