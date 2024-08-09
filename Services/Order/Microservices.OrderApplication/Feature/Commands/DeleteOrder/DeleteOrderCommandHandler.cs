using MediatR;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using Microservices.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Feature.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler(OrderDbContext orderDbContext) : IRequestHandler<DeleteOrderCommandRequest, DeleteOrderCommandResponse>
    {
        public async Task<DeleteOrderCommandResponse> Handle(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            Order? order = await orderDbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == request.Id);
            if (order is null)
                throw new NotFoundException("Order not found");
            
            orderDbContext.Orders.Remove(order);
            await orderDbContext.SaveChangesAsync();
            return new(ServiceResponse<NoContent>.Success(204));
        }
    }
}
