using Microservices.CatalogInboxPublisherService.Contexts;
using Microservices.CatalogInboxPublisherService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservices.CatalogInboxPublisherService.Services
{
    public class CatalogInboxService(CatalogInboxDbContext dbContext) : ICatalogInboxService
    {
        public async Task CreateIfNotExist(Guid idempotentToken, object @event)
        {
            var result = await dbContext.CatalogInboxes.AnyAsync(x => x.IdempotentToken == idempotentToken);

            if (!result)
            {
                CatalogInbox catalogInbox = new()
                {
                    IdempotentToken = idempotentToken,
                    Payload = JsonSerializer.Serialize(@event),
                    Processed = false
                };
                await dbContext.CatalogInboxes.AddAsync(catalogInbox);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<CatalogInbox>> GetCatalogInboxes() => await dbContext.CatalogInboxes
                .Where(ci => ci.Processed == false)
                .ToListAsync(); 

        public async Task UpdateProcessed(CatalogInbox catalogInbox)
        {
            catalogInbox.Processed = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
