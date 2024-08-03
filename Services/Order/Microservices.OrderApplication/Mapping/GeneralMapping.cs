using AutoMapper;
using Microservices.OrderApplication.Dtos;
using Microservices.OrderDomain.OrderAggregates;

namespace Microservices.OrderApplication.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        }
    }
}
