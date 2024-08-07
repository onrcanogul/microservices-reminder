using MassTransit;
using Microservices.OrderOutboxTablePublisherService.Entities;
using Microservices.OrderOutboxTablePublisherService.Services;
using Microservices.Shared;
using Microservices.Shared.Events;
using Microservices.Shared.Events.Base;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.OrderOutboxTablePublisherService.Jobs
{
    public class OrderOutboxPublishJob(IOrderOutboxSingletonDatabase database, ISendEndpointProvider sendEndpointProvider, IOrderOutboxService orderOutboxService) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if(database.DataReaderState)
            {
                database.DataReaderBusy();
                List<OrderOutbox> orderOutboxes = await orderOutboxService.GetOutboxes("SELECT * FROM OrderOutboxes WHERE ProcessedOn IS NULL ORDER BY OCCUREDON ASC");

                foreach (var orderOutbox in orderOutboxes)
                {
                    if(orderOutbox.Type == nameof(OrderCreatedEvent))
                    {
                        OrderCreatedEvent? orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutbox.Payload);

                        if (orderEvent != null && orderOutbox.Type == nameof(OrderCreatedEvent))
                            await orderOutboxService.SendEndpoint($"queue:{RabbitMqSettings.OrderOutbox_OrderCreatedInboxEventQueue}", orderEvent,orderOutbox);
                    }
                    
                             
                }
                database.DataReaderReady();
            }
        }
    }
}
