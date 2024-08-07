using Microservices.CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.CatalogAPI.Contexts
{
    public class OrderInboxDbContext : DbContext
    {
        public OrderInboxDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<OrderInbox> OrderInboxes { get; set; }

    }
}
