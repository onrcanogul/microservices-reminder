using Microservices.OrderDomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderDomain.OrderAggregates
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; } = null!;
        public string BuyerId { get; private set; } = null!;
        public decimal TotalPrice { get; set; }
        private readonly List<OrderItem> OrderItems;
        public IReadOnlyCollection<OrderItem> GetOrderItems => OrderItems;
        public Order(string buyerId, Address address)
        {
            OrderItems = new();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
            TotalPrice = OrderItems.Sum(oi => oi.Price * oi.Count);
        }
        public void AddOrderItem(string productId, string pictureUrl, decimal price, string productName)
        {
            OrderItem? existProduct = OrderItems.FirstOrDefault(x => x.ProductId == productId);
            if (existProduct == null)
            {
                OrderItem orderItem = new(productId, productName, pictureUrl, price);
                OrderItems.Add(orderItem);
            }
            else
                existProduct.Count++;
            
        }
    }
}
