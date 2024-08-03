using MediatR;

namespace Microservices.OrderApplication.Feature.Commands.DeleteOrder
{
    public class DeleteOrderCommandRequest : IRequest<DeleteOrderCommandResponse>
    {
        public int Id { get; set; }
    }
}