using AutoMapper;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;

namespace Microservices.CatalogAPI.Mapping
{
    public class FeatureMapping : Profile
    {
        public FeatureMapping()
        {
            CreateMap<FeatureDto, Feature>().ReverseMap();
        }
    }
}
