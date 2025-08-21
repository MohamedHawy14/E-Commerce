using E_Commerce.Core.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Interfaces
{
    public interface ISocialAuthRepository
    {
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user);
        Task AddLoginAsync(ApplicationUser user, UserLoginInfo info);
        Task AddExternalLoginRecordAsync(ExternalLogin externalLogin);
    }
}
