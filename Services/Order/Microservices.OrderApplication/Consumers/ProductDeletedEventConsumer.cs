using MassTransit;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace Microservices.OrderApplication.Consumers
{
    public class ProductDeletedEventConsumer(OrderDbContext orderDbContext) : IConsumer<ProductDeletedInboxToConsumerEvent>
    {
        public async Task Consume(ConsumeContext<ProductDeletedInboxToConsumerEvent> context)
        {
            List<OrderItem> orderItems = await orderDbContext.OrderItems
                .Where(oi => oi.ProductId == context.Message.@event.ProductId)
                .ToListAsync();

            orderDbContext.OrderItems.RemoveRange(orderItems);
            await orderDbContext.SaveChangesAsync();
        }
    }
}
