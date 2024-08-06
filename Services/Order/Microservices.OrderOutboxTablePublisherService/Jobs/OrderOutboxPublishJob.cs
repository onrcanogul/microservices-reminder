using MassTransit;
using Microservices.OrderOutboxTablePublisherService.Entities;
using Microservices.OrderOutboxTablePublisherService.Services;
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
    public class OrderOutboxPublishJob(IOrderOutboxSingletonDatabase database, IPublishEndpoint publishEndpoint, IOrderOutboxService orderOutboxService) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if(database.DataReaderState)
            {
                database.DataReaderBusy();
                List<OrderOutbox> orderOutboxes = await orderOutboxService.GetOutboxes("SELECT * FROM OrderOutboxes WHERE ProcessedOn IS NULL ORDER BY OCCUREDON ASC");


                foreach (var orderOutbox in orderOutboxes)
                {
                    IEvent? orderEvent = JsonSerializer.Deserialize<IEvent>(orderOutbox.Payload);
                    if(orderEvent != null)
                        await orderOutboxService.PublishEvent(orderEvent, orderOutbox);
                    
                }
                database.DataReaderReady();
            }
        }
    }
}
