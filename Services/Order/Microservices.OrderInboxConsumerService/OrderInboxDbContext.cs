using Microservices.OrderInboxConsumerService.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderInboxConsumerService
{
    public class OrderInboxDbContext : DbContext
    {
        public OrderInboxDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<OrderInbox> OrderInboxes { get; set; }
    }
}
