using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Entites.Product
{
    public class Product:BaseModel<int>
    {
        public string Name { get; set; }
        public string description { get; set; }

        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        #region many2one with category
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]

        public virtual Category Category { get; set; }
        #endregion

        #region one2many with photo
        public virtual List<Photo> Photos { get; set; } 
        #endregion
    }
}
