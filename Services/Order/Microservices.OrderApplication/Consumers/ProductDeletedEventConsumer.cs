using MassTransit;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace Microservices.OrderApplication.Consumers
{
    public class ProductDeletedEventConsumer(OrderDbContext orderDbContext) : IConsumer<ProductDeletedEvent>
    {
        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            List<OrderItem> orderItems = await orderDbContext.OrderItems
                .Where(oi => oi.ProductId == context.Message.ProductId)
                .ToListAsync();

            orderDbContext.OrderItems.RemoveRange(orderItems);
            await orderDbContext.SaveChangesAsync();
        }
    }
}
