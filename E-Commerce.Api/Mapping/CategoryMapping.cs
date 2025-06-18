using AutoMapper;
using E_Commerce.Core.DTO.Category;
using E_Commerce.Core.Entites.Product;

namespace E_Commerce.Api.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
        }
    }
}
