using E_Commerce.Core.DTO.Authentication.External_Login;
using E_Commerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialAuthController : ControllerBase
    {
        private readonly ISocialAuthService _socialAuthService;

        public SocialAuthController(ISocialAuthService socialAuthService)
        {
            _socialAuthService = socialAuthService;
        }

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _socialAuthService.ExternalLoginAsync(model);

            if (!response.IsSuccess)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
    }

}
