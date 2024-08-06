using MassTransit;
using Microservices.OrderOutboxTablePublisherService.Entities;
using Microservices.Shared.Events;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.OrderOutboxTablePublisherService.Jobs
{
    public class OrderOutboxPublishJob(IOrderOutboxSingletonDatabase database, IPublishEndpoint publishEndpoint) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if(database.DataReaderState)
            {
                database.DataReaderBusy();
                List<OrderOutbox> orderOutboxes = (await database.QueryAsync<OrderOutbox>("SELECT * FROM OrderOutboxes WHERE ProcessedOn IS NULL ORDER BY OCCUREDON ASC")).ToList();


                foreach (var orderOutbox in orderOutboxes)
                {
                    OrderCreatedEvent? orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutbox.Payload);
                    if(orderCreatedEvent != null)
                    {
                        await publishEndpoint.Publish(orderCreatedEvent);
                        await database.ExecuteAsync($"UPDATE ORDEROUTBOXES SET PROCESSED ON = GETDATE() WHERE IdempotentToken = '{orderOutbox.IdempotentToken}'");
                    }
                }
                database.DataReaderReady();
            }
        }
    }
}
