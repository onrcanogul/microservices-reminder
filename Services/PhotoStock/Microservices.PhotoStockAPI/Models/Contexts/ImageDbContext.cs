using Microsoft.EntityFrameworkCore;

namespace Microservices.PhotoStockAPI.Models.Contexts
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }

    }
}
