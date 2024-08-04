using Microservices.CatalogAPI.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Microservices.CatalogAPI.Dtos
{
    public class ProductDto
    {
        
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
   
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }

        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; } = null!;
        public string CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
