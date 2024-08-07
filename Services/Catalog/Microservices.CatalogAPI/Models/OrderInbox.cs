using System.ComponentModel.DataAnnotations;

namespace Microservices.CatalogAPI.Models
{
    public class OrderInbox
    {
        [Key] // servise al
        public Guid IdempotentToken { get; set; }
        public bool Processed { get; set; }
        public string Payload { get; set; }
    }
}
