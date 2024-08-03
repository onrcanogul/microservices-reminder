using Azure;
using MediatR;
using Microservices.OrderApplication.Dtos;

namespace Microservices.OrderApplication.Feature.Commands.CreateOrder
{
    public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }

    }
}