using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager);
    }
}
