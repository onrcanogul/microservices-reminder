using MassTransit;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace Microservices.OrderApplication.Consumers
{
    public class ProductNotFoundEventConsumer(OrderDbContext orderDbContext) : IConsumer<ProductNotFoundEvent>
    {
        public async Task Consume(ConsumeContext<ProductNotFoundEvent> context)
        {
            Order? order = await orderDbContext.Orders.FirstOrDefaultAsync(o => o.Id == context.Message.OrderId);

            if (order is null)
                throw new Exception(); //order not found event

            order.FailOrder();
            await orderDbContext.SaveChangesAsync();
        }
    }
}
