namespace Microservices.BasketAPI.Models
{
    public class Basket
    {
        public Guid Id { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new();
        public string UserId { get; set; } = null!;
        public string? DiscountCode { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(bi => bi.Quantity * bi.Price); }
    }
}
