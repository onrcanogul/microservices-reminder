using MassTransit;
using Microservices.CatalogOutboxPublisherService.Models;
using Microservices.CatalogOutboxPublisherService.Services;
using Microservices.OrderOutboxTablePublisherService;
using Microservices.Shared;
using Microservices.Shared.Events;
using Microservices.Shared.Events.Base;
using Quartz;
using System.Text.Json;

namespace Microservices.CatalogOutboxPublisherService.Jobs
{
    public class CatalogOutboxPublishJob(ICatalogOutboxSingletonDatabase database, ISendEndpointProvider sendEndpointProvider, ICatalogOutboxService service) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
           if(database.DataReaderState)
           {
                database.DataReaderBusy();
                List<CatalogOutbox> catalogOutboxes = await service.GetCatalogOutboxes();

                foreach (var catalogOutbox in catalogOutboxes)
                {
                    if(catalogOutbox.Type == nameof(ProductUpdatedEvent))
                    {
                        ProductUpdatedEvent? @event = JsonSerializer.Deserialize<ProductUpdatedEvent>(catalogOutbox.Payload);
                        if(@event != null)
                        {
                            ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.CatalogOutbox_ProductUpdatedEventQueue}"));
                            await sendEndpoint.Send(@event);
                        }
                    }
                    else if(catalogOutbox.Type == nameof(ProductDeletedEvent))
                    {
                        ProductDeletedEvent? @event = JsonSerializer.Deserialize<ProductDeletedEvent>(catalogOutbox.Payload);
                        if (@event != null) 
                        {
                            ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.CatalogOutbox_ProductDeletedEventQueue}"));
                            await sendEndpoint.Send(@event);
                        }
                    }
                    await service.UpdateProcessed(catalogOutbox);
                }
                database.DataReaderReady();
           }

        }
    }
}
