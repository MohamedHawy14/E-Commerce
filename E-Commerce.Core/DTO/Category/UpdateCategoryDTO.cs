using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.Category
{
    public record UpdateCategoryDTO(string Name, string Description, int id);
}
