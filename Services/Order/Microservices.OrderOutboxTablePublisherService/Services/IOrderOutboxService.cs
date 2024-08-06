using Microservices.OrderOutboxTablePublisherService.Entities;
using Microservices.Shared.Events.Base;

namespace Microservices.OrderOutboxTablePublisherService.Services
{
    public interface IOrderOutboxService
    {
        Task<List<OrderOutbox>> GetOutboxes(string sql);
        Task PublishEvent(IEvent @event, OrderOutbox orderOutbox);
    }
}
