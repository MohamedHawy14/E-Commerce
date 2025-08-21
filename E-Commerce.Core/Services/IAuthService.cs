using BrainHope.Services.DTO.Authentication.SingUp;
using CleanArchitecture.Services.DTOs.Responses;
using E_Commerce.Core.DTO.Responses;
using E_Commerce.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUser registerUser);

        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user);
   
    }
}
