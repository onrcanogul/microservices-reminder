using MassTransit;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;


namespace Microservices.OrderApplication.Consumers
{
    public class ProductUpdatedEventConsumer(OrderDbContext orderDbContext) : IConsumer<ProductUpdatedInboxToConsumerEvent>
    {
        public async Task Consume(ConsumeContext<ProductUpdatedInboxToConsumerEvent> context)
        {
            List<OrderItem> orderItems = await orderDbContext.OrderItems
                .Where(p => p.ProductId == context.Message.@event.ProductId)
                .ToListAsync();

            orderItems.ForEach(oi => oi.UpdateOrderItem(context.Message.@event.ProductName, context.Message.@event.PictureUrl, context.Message.@event.Price));

            await orderDbContext.SaveChangesAsync();
        }
    }
}
