using Microservices.OrderOutboxTablePublisherService.Entities;
using Microservices.Shared.Events;
using Microservices.Shared.Events.Base;

namespace Microservices.OrderOutboxTablePublisherService.Services
{
    public interface IOrderOutboxService
    {
        Task<List<OrderOutbox>> GetOutboxes(string sql);
        Task PublishEvent(BaseEvent @event, OrderOutbox orderOutbox);
        Task SendEndpoint(string uri, OrderCreatedEvent @event, OrderOutbox orderOutbox);
    }
}
