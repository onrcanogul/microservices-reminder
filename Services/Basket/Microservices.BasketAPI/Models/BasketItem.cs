namespace Microservices.BasketAPI.Models
{
    public class BasketItem
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
