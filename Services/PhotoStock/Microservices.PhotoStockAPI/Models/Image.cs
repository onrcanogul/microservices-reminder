namespace Microservices.PhotoStockAPI.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Path { get; set; } = null!;
    }
}
