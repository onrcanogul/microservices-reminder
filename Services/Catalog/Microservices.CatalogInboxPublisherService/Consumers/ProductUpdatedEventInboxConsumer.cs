using MassTransit;
using Microservices.CatalogInboxPublisherService.Contexts;
using Microservices.CatalogInboxPublisherService.Models;
using Microservices.CatalogInboxPublisherService.Services;
using Microservices.Shared;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.CatalogInboxPublisherService.Consumers
{
    public class ProductUpdatedEventInboxConsumer(CatalogInboxDbContext dbContext, ISendEndpointProvider sendEndpointProvider, ICatalogInboxService service) : IConsumer<ProductUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {

            await service.CreateIfNotExist(context.Message.IdempotentToken, context.Message);

            List<CatalogInbox> catalogInboxes = await service.GetCatalogInboxes();

            foreach (var _catalogInbox in catalogInboxes)
            {
                ProductUpdatedEvent? productUpdatedEvent = JsonSerializer.Deserialize<ProductUpdatedEvent>(_catalogInbox.Payload);
                if(productUpdatedEvent != null)
                {
                    ProductUpdatedInboxToConsumerEvent productUpdatedInboxToConsumerEvent = new()
                    {
                        @event = productUpdatedEvent
                    };
                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.CatalogInbox_ProductUpdatedEventQueue}"));
                    await sendEndpoint.Send(productUpdatedInboxToConsumerEvent);
                }
                await service.UpdateProcessed(_catalogInbox);
            }

        }
    }
}
