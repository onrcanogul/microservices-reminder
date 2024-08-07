using Microservices.CatalogInboxPublisherService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.CatalogInboxPublisherService.Services
{
    public interface ICatalogInboxService
    {
        Task CreateIfNotExist(Guid idempotentToken, object @event);
        Task<List<CatalogInbox>> GetCatalogInboxes();
        Task UpdateProcessed(CatalogInbox catalogInbox);
    }
}
