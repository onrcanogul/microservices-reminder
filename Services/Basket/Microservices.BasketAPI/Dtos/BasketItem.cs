namespace Microservices.BasketAPI.Dtos
{
    public class BasketItemDto
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
