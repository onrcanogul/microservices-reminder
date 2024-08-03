using MediatR;

namespace Microservices.OrderApplication.Feature.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommandRequest : IRequest<UpdateOrderItemCommandResponse>
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}