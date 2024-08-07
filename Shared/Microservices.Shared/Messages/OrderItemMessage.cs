namespace Microservices.Shared.Messages
{
    public class OrderItemMessage
    { 
        public string ProductId { get;  set; }
        public string ProductName { get;  set; }
        public string PictureUrl { get;  set; }
        public decimal Price { get;  set; }
        public int Count { get; set; }
    }
}
