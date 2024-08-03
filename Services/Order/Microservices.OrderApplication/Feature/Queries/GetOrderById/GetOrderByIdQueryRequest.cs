using MediatR;

namespace Microservices.OrderApplication.Feature.Queries.GetOrderById
{
    public class GetOrderByIdQueryRequest : IRequest<GetOrderByIdQueryResponse>
    {
        public int Id { get; set; } 
    }
}