using MediatR;

namespace Microservices.OrderApplication.Feature.Queries.GetOrders
{
    public class GetOrdersQueryRequest : IRequest<GetOrdersQueryResponse>
    {
    }
}