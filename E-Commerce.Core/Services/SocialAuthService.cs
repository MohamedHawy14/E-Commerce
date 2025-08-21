using CleanArchitecture.Services.DTOs.Responses;
using E_Commerce.Core.DTO.Authentication.External_Login;
using E_Commerce.Core.Entites;
using E_Commerce.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services
{
    public class SocialAuthService : ISocialAuthService
    {
        private readonly ISocialAuthRepository _repository;
        private readonly IAuthService _authService;

        public SocialAuthService(ISocialAuthRepository repository, IAuthService authService)
        {
            _repository = repository;
            _authService = authService;
        }

        public async Task<ApiResponse<LoginResponse>> ExternalLoginAsync(ExternalLoginDTO model)
        {
            var user = await _repository.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = model.Email,
                    FullName = model.Name,
                    UserName = model.Email.Split('@')[0],
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _repository.CreateUserAsync(user);
                if (!result.Succeeded)
                {
                    return new ApiResponse<LoginResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Message = "Failed to create social user: " +
                            string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                var info = new UserLoginInfo(model.Provider, model.ProviderKey, model.Provider);
                await _repository.AddLoginAsync(user, info);

                await _repository.AddExternalLoginRecordAsync(new ExternalLogin
                {
                    Provider = model.Provider,
                    ProviderKey = model.ProviderKey,
                    UserId = user.Id
                });
            }

            return await _authService.GetJwtTokenAsync(user);
        }
    }
}
