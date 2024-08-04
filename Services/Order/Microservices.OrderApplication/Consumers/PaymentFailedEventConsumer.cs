using MassTransit;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Events;
using Microservices.Shared.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Consumers
{
    public class PaymentFailedEventConsumer(OrderDbContext orderDbContext) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        { 
            Order? order = await orderDbContext.Orders.FirstOrDefaultAsync(o => o.Id == context.Message.OrderId);

            if (order is null)
                throw new Exception(); //order not found event

            order.FailOrder();
            await orderDbContext.SaveChangesAsync();
            //servicelere alınıp tek bir yerden çağırılınabilir üşenilmezse :)
        }
    }
}
