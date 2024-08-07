using MassTransit;
using Microservices.CatalogOutboxPublisherService.Models;
using Microservices.OrderOutboxTablePublisherService;

namespace Microservices.CatalogOutboxPublisherService.Services
{
    public class CatalogOutboxService(ISendEndpointProvider sendEndpointProvider, ICatalogOutboxSingletonDatabase database) : ICatalogOutboxService
    {
        public async Task<List<CatalogOutbox>> GetCatalogOutboxes()
            => (await database.QueryAsync<CatalogOutbox>("SELECT * FROM CATALOGOUTBOXES WHERE PROCESSEDON IS NULL ORDER BY OCCUREDON ASC")).ToList();
        public async Task<int> UpdateProcessed(CatalogOutbox catalogOutbox)
            => await database.ExecuteAsync($"UPDATE CATALOGOUTBOXES SET PROCESSEDON = GETDATE() WHERE IdempotentToken = '{catalogOutbox.IdempotentToken}'");
    }
}
