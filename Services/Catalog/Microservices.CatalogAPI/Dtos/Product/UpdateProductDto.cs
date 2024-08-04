namespace Microservices.CatalogAPI.Dtos
{
    public class UpdateProductDto
    {
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string UserId { get; set; } = null!;
        public string CategoryId { get; set; }
    }
}
