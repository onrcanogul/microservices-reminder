using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Microservices.CatalogAPI.Models
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; } = null!;
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public Feature Feature { get; set; }
        [BsonIgnore]
        public Category Category { get; set; }

    }
}
