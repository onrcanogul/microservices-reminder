using MassTransit;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Consumers
{
    public class ProductUpdatedEventConsumer(OrderDbContext orderDbContext) : IConsumer<ProductUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            List<OrderItem> orderItems = await orderDbContext.OrderItems
                .Where(p => p.ProductId == context.Message.ProductId)
                .ToListAsync();

            orderItems.ForEach(oi => oi.UpdateOrderItem(context.Message.ProductName, context.Message.PictureUrl, context.Message.Price));

            await orderDbContext.SaveChangesAsync();
        }
    }
}
