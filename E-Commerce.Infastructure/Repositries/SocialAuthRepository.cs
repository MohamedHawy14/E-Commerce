using E_Commerce.Core.Entites;
using E_Commerce.Core.Interfaces;
using E_Commerce.Infastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infastructure.Repositries
{
    public class SocialAuthRepository : ISocialAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SocialAuthRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo info)
        {
            await _userManager.AddLoginAsync(user, info);
        }

        public async Task AddExternalLoginRecordAsync(ExternalLogin externalLogin)
        {
            await _context.ExternalLogins.AddAsync(externalLogin);
            await _context.SaveChangesAsync();
        }
    }
}
