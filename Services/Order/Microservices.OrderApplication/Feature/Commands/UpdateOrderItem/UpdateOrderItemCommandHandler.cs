using MediatR;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.OrderApplication.Feature.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommandHandler(OrderDbContext orderDbContext) : IRequestHandler<UpdateOrderItemCommandRequest, UpdateOrderItemCommandResponse>
    {
        public async Task<UpdateOrderItemCommandResponse> Handle(UpdateOrderItemCommandRequest request, CancellationToken cancellationToken)
        {
            OrderItem? orderItem = await orderDbContext
                .OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.Id == request.Id);

            if (orderItem == null)
                return new(ServiceResponse<NoContent>.Failure("order item not found", 404));

            orderItem.UpdateOrderItem(request.ProductName, request.PictureUrl, request.Price);

            orderDbContext.OrderItems.Update(orderItem);
            await orderDbContext.SaveChangesAsync();

            return new(ServiceResponse<NoContent>.Success(204));
        }
    }
}
