using Microservices.Shared.Dtos;

namespace Microservices.OrderApplication.Feature.Commands.DeleteOrder
{
    public record DeleteOrderCommandResponse(ServiceResponse<NoContent> Response);
}