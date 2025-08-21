using CleanArchitecture.Services.DTOs.Responses;
using E_Commerce.Core.DTO.Authentication.External_Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services
{
    public interface ISocialAuthService
    {
        Task<ApiResponse<LoginResponse>> ExternalLoginAsync(ExternalLoginDTO model);
    }
}
