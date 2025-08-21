using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entites
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
      

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }


        //otp
        public string? ResetOtp { get; set; }
        public DateTime? OtpExpiration { get; set; }
        public bool OtpVerified { get; set; }
    }

}
