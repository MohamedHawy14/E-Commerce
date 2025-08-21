using E_Commerce.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Responses
{
    public class CreateUserResponse
    {
        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public string? Token { get; set; }
    }
}
