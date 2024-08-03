using Microservices.OrderDomainCore;

namespace Microservices.OrderDomain.OrderAggregates
{
    public class OrderItem : Entity
    {
        public string ProductId { get; private set; }
        public string ProductName{ get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get; private set; }
        public int Count { get; set; }

        //shadow property
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public OrderItem()
        {
            
        }
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }
        public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }
    }
}
