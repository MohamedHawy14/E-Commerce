using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Authentication.External_Login
{
    public class ExternalLoginDTO
    {
        public string Provider { get; set; }   // Google or Facebook
        public string ProviderKey { get; set; } // Unique ID من Provider
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
