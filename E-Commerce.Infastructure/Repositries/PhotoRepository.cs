using E_Commerce.Core.Entites.Product;
using E_Commerce.Core.Interfaces;
using E_Commerce.Infastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infastructure.Repositries
{
    
      public class PhotoRepository : genricRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
