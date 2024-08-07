using MassTransit;
using Microservices.OrderInboxConsumerService;
using Microservices.OrderInboxConsumerService.Entities;
using Microservices.Shared;
using Microservices.Shared.Events;
using Microservices.Shared.Events.Base;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Microservices.OrderInboxTableConsumerService.Consumers
{
    public class OrderCreatedEventInboxConsumer(OrderInboxDbContext dbContext, ISendEndpointProvider sendEndpointProvider) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var result = await dbContext.OrderInboxes.AnyAsync(x => x.IdempotentToken == context.Message.IdempotentToken);

            if (!result)
            {
                OrderInbox orderInbox = new()
                {
                    IdempotentToken = context.Message.IdempotentToken,
                    Processed = false,
                    Payload = JsonSerializer.Serialize(context.Message),
                };
                await dbContext.OrderInboxes.AddAsync(orderInbox);
                await dbContext.SaveChangesAsync();
            }
            List<OrderInbox> orderInboxes = await dbContext.OrderInboxes
                .Where(oi => oi.Processed == false)
                .ToListAsync();

            foreach (var _orderInbox in orderInboxes)
            {
                
                OrderCreatedEvent @event = JsonSerializer.Deserialize<OrderCreatedEvent>(_orderInbox.Payload);
                if (@event != null)
                {
                    OrderCreatedInboxToConsumerEvent orderInboxToConsumerMessage = new()
                    {
                        @event = @event
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.OrderInbox_OrderCreatedInboxEventQueue}"));
                    await sendEndpoint.Send(orderInboxToConsumerMessage);

                    _orderInbox.Processed = true;
                    await dbContext.SaveChangesAsync();
                }
                
            }

        }
    }
}