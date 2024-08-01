namespace Microservices.DiscountAPI.Models
{
    [Dapper.Contrib.Extensions.Table("discount")]
    public class Discount
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
