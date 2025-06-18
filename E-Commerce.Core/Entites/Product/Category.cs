using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entites.Product
{
    public class Category:BaseModel<int>
    {
        public string Name { get; set; }
        public string description  { get; set; }

        #region one2many with products
        public ICollection<Product> Products { get; set; } = new HashSet<Product>(); 
        #endregion
    }
}
