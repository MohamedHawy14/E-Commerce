using BrainHope.Services.DTO.Authentication.SignIn;
using BrainHope.Services.DTO.Authentication.SingUp;
using CleanArchitecture.Services.DTOs.Responses;
using E_Commerce.Core.DTO.Authentication.SingUp;
using E_Commerce.Core.DTO.Email;
using E_Commerce.Core.Entites;
using E_Commerce.Core.Services;
using E_Commerce.Infastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authServices;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager,
            IAuthService authServices, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._emailService = emailService;
            this._configuration = configuration;
            this._signInManager = signInManager;
            this._authServices = authServices;
            this._context = context;
        }



        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authServices.CreateUserWithTokenAsync(registerUser);

            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode, response);
            }

            // Generate email confirmation link
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { token = response.Response.Token, email = registerUser.Email }, Request.Scheme);

            #region Email Message
            var message = new Message(
                new string[] { registerUser.Email! },
                "Confirm Your Email",
                $@"
        <html>
        <body>
            <p>Hello {registerUser.Name},</p>
            <p>Thank you for registering. Please confirm your email by clicking the button below:</p>
            <p>
                <a href='{confirmationLink}' 
                   style='display: inline-block; padding: 10px 20px; font-size: 16px; color: white; 
                          background-color: #007bff; text-decoration: none; border-radius: 5px;'>
                    Confirm Email
                </a>
            </p>
            <p>Best regards,<br>Bi3ly Team</p>
        </body>
        </html>"
            );
            #endregion

            try
            {
                _emailService.SendEmail(message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Email error: " + ex.Message);
            }

            return Ok(response);
        }


        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                   new Response { Status = "Success", Message = "Email Verified Successfully.", IsSuccess = true });
                }

            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                  new Response { Status = "Error", Message = "This Use Don't Exist." });

        }
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromForm] SignInDTO signInDTO)
        {

            var user = await _userManager.FindByEmailAsync(signInDTO.Email);
            if (user == null)
            {
                return Unauthorized(new Response { IsSuccess = false, Message = "User not found.", Status = "Error" });
            }

            // Check if the email is confirmed.
            if (!user.EmailConfirmed)
            {
                return Unauthorized(new Response { IsSuccess = false, Message = "Please confirm your email to login.", Status = "Error" });
            }


            var passwordValid = await _userManager.CheckPasswordAsync(user, signInDTO.Password);
            if (!passwordValid)
            {
                return Unauthorized(new Response { IsSuccess = false, Message = "Invalid credentials.", Status = "Error" });
            }


            var tokenResponse = await _authServices.GetJwtTokenAsync(user);
            if (!tokenResponse.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, tokenResponse);
            }

            return Ok(tokenResponse);
        }


        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromForm] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new Response { IsSuccess = false, Message = "User not found.", Status = "Error" });
            }

            var otp = GenerateSimpleOtp();

            // Store OTP in database with expiration (5 minutes)
            user.ResetOtp = otp;
            user.OtpExpiration = DateTime.UtcNow.AddMinutes(5);
            await _userManager.UpdateAsync(user);

            var message = new Message(new string[] { user.Email! }, "Password Reset OTP", $"Your OTP is: {otp}");
            _emailService.SendEmail(message);



            return Ok(new Response { IsSuccess = true, Message = $"OTP sent to {user.Email}.", Status = "Success" });
        }

        [HttpPost("VerifyOtp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromForm] VerifyOtpRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.ResetOtp == request.Otp);

            if (user == null)
                return BadRequest("Invalid OTP");

            if (user.OtpExpiration < DateTime.UtcNow)
                return BadRequest("OTP has expired");

            user.OtpVerified = true;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "OTP verified successfully", email = user.Email });
        }


        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPassword request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user =  _context.Users.FirstOrDefault(u => u.Email == request.Email && u.OtpVerified == true);

            if (user == null)
                return BadRequest(new { message = "OTP verification required or user not found" });

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Clear OTP-related fields
            user.ResetOtp = null;
            user.OtpExpiration = null;
            user.OtpVerified = false;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Password reset successfully" });
        }


        #region Private Methods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secert"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninkey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        private string GenerateSimpleOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }
        private string GenerateSimpleOtp(string userId)
        {
            var secretKey = _configuration["OtpSecretKey"];
            var currentTime = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            var data = $"{userId}{secretKey}{currentTime}";

            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                var otp = (BitConverter.ToInt32(hash, 0) % 10000);
                return Math.Abs(otp).ToString("D4");
            }
        }
        #endregion




    }
}
