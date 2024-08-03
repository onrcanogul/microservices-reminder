namespace Microservices.OrderApplication.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ProductId { get;  set; }
        public string ProductName { get;  set; }
        public string PictureUrl { get;  set; }
        public decimal Price { get;  set; }
        public int Count { get; set; }
    }
}
