using System.ComponentModel.DataAnnotations;

namespace Microservices.OrderInboxConsumerService.Entities
{
    public class OrderInbox
    {
        [Key]
        public Guid IdempotentToken { get; set; }
        public bool Processed { get; set; }
        public string Payload { get; set; }
    }
}
