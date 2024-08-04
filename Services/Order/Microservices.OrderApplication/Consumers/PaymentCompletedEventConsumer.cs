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
    public class PaymentCompletedEventConsumer(OrderDbContext orderDbContext) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Order? order = await orderDbContext.Orders.FirstOrDefaultAsync(o => o.Id == context.Message.OrderId);
            if (order is null)
                throw new Exception();

            order.CompleteOrder();
            await orderDbContext.SaveChangesAsync();
        }
    }
}
