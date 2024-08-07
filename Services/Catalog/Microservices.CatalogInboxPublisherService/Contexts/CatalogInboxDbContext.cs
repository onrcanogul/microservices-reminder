using Microservices.CatalogInboxPublisherService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.CatalogInboxPublisherService.Contexts
{
    public class CatalogInboxDbContext : DbContext
    {
        public CatalogInboxDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CatalogInbox> CatalogInboxes { get; set; }
    }
}
