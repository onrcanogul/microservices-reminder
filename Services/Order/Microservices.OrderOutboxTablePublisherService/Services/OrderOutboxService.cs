using MassTransit;
using MassTransit.Transports;
using Microservices.OrderOutboxTablePublisherService.Entities;
using Microservices.Shared.Events;
using Microservices.Shared.Events.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderOutboxTablePublisherService.Services
{
    public class OrderOutboxService(IOrderOutboxSingletonDatabase database, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider) : IOrderOutboxService
    {
        public async Task<List<OrderOutbox>> GetOutboxes(string sql)
            => (await database.QueryAsync<OrderOutbox>(sql)).ToList();

        public async Task PublishEvent(BaseEvent @event, OrderOutbox orderOutbox)
        {
            await publishEndpoint.Publish(@event);
            await database.ExecuteAsync($"UPDATE ORDEROUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{orderOutbox.IdempotentToken}'");
        }

        public async Task SendEndpoint(string uri, OrderCreatedEvent @event,OrderOutbox orderOutbox)
        {
            ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new(uri));
            await sendEndpoint.Send(@event);
            Console.WriteLine("Sended");
            await database.ExecuteAsync($"UPDATE ORDEROUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{orderOutbox.IdempotentToken}'");
        }
        
    }
}
