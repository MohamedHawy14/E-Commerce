using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entites.Product
{
    public class Photo: BaseModel<int>
    {
        public string ImageName { get; set; }

        #region many2one with product
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } 
        #endregion

    }
}
