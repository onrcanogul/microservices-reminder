using Microservices.CatalogOutboxPublisherService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.CatalogOutboxPublisherService.Services
{
    public interface ICatalogOutboxService
    {
        Task<List<CatalogOutbox>> GetCatalogOutboxes();
        Task<int> UpdateProcessed(CatalogOutbox catalogOutbox);
    }
}
