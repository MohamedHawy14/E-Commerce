using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entites
{
    public class ExternalLogin
    {
        public int Id { get; set; }
        public string Provider { get; set; } // Google / Facebook
        public string ProviderKey { get; set; } // Sub ID من Google أو UserId من Facebook
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }
    }

}
