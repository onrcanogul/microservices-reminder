using AutoMapper;
using Microservices.DiscountAPI.Models;

namespace Microservices.DiscountAPI.Dtos.Mapping
{
    public class DiscountMapping : Profile
    {
        public DiscountMapping()
        {
            CreateMap<DiscountDto, Discount>().ReverseMap();
            CreateMap<CreateDiscountDto, Discount>().ReverseMap();
            CreateMap<UpdateDiscountDto, Discount>().ReverseMap();
        }
    }
}
