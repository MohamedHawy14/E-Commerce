﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.DTO.product
{
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
