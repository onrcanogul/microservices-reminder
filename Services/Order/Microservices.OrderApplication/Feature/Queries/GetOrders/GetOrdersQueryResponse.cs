using Microservices.OrderApplication.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.OrderApplication.Feature.Queries.GetOrders
{
    public class GetOrdersQueryResponse
    {
        public ServiceResponse<List<OrderDto>> OrdersResponse { get; set; } = null!;
    }
}