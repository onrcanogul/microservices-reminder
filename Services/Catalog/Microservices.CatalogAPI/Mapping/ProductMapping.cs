using AutoMapper;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;

namespace Microservices.CatalogAPI.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping() 
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductDto, Product>().ReverseMap();
        }
    }
}
