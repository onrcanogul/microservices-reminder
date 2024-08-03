using Microservices.Shared.Dtos;

namespace Microservices.OrderApplication.Feature.Commands.UpdateOrderItem
{
    public record UpdateOrderItemCommandResponse(ServiceResponse<NoContent> Response);
    
}