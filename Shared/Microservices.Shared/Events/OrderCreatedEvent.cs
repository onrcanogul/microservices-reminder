using Microservices.Shared.Events.Base;
using Microservices.Shared.Messages;


namespace Microservices.Shared.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public Guid IdempotentToken { get; set; }
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
