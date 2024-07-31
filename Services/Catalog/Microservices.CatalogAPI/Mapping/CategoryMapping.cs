using AutoMapper;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;

namespace Microservices.CatalogAPI.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping() 
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
        }
    }
}
