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
    public class ProductDeletedEventInboxConsumer(CatalogInboxDbContext dbContext, ISendEndpointProvider sendEndpointProvider, ICatalogInboxService service) : IConsumer<ProductDeletedEvent>
    {

        public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
        {
            await service.CreateIfNotExist(context.Message.IdempotentToken, context.Message);

            List<CatalogInbox> catalogInboxes = await service.GetCatalogInboxes();

            foreach (var _catalogInbox in catalogInboxes)
            {
                ProductDeletedEvent? productDeletedEvent = JsonSerializer.Deserialize<ProductDeletedEvent>(_catalogInbox.Payload);
                if(productDeletedEvent != null)
                {
                    ProductDeletedInboxToConsumerEvent productDeletedInboxToConsumerEvent = new()
                    {
                        @event = productDeletedEvent
                    };

                    ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.CatalogInbox_ProductDeletedEventQueue}"));
                    await sendEndpoint.Send(productDeletedInboxToConsumerEvent);
                }
                await service.UpdateProcessed(_catalogInbox);
            }
        }
    }
}
