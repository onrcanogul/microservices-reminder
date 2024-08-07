using System.ComponentModel.DataAnnotations;

namespace Microservices.CatalogAPI.Models
{
    public class CatalogOutbox
    {
        [Key]
        public Guid IdempotentToken { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public DateTime OccuredOn { get; set; }
        public string Payload { get; set; }
        public string Type { get; set; }
    }
}
