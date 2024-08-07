using Microservices.CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.CatalogAPI.Contexts
{
    public class CatalogOutboxDbContext : DbContext
    {
        public CatalogOutboxDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CatalogOutbox> CatalogOutboxes { get; set; }
    }
}
