using Microservices.OrderApplication.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.OrderApplication.Feature.Queries.GetOrderById
{
    public class GetOrderByIdQueryResponse
    {
        public ServiceResponse<OrderDto> OrderItemResponse { get; set; } = null!;
    }
}