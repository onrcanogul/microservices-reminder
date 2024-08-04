using Microservices.OrderDomain.Enums;
using Microservices.OrderDomainCore;


namespace Microservices.OrderDomain.OrderAggregates
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; } = null!;
        public string BuyerId { get; private set; } = null!;
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatu { get; set; }
        private readonly List<OrderItem> GetOrderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => GetOrderItems;
        public Order()
        {
            
        }
        public Order(string buyerId, Address address)
        {
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
            OrderStatu = OrderStatus.Pending;
        }    
        public void AddOrderItem(string productId, string pictureUrl, decimal price, string productName)
        {
            OrderItem? existProduct = GetOrderItems.FirstOrDefault(x => x.ProductId == productId);
            if (existProduct == null)
            {
                OrderItem orderItem = new(productId, productName, pictureUrl, price);
                GetOrderItems.Add(orderItem);
            }
            else
                existProduct.Count++; 
        }
        public void CompleteOrder()
        {
            OrderStatu = OrderStatus.Completed;
        }
        public void FailOrder()
        {
            OrderStatu = OrderStatus.Failed;
        }
        public decimal GetTotalPrice => GetOrderItems.Sum(x => x.Price);
    }
}
