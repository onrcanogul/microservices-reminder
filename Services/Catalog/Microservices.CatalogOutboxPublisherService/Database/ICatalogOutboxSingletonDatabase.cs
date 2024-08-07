using System.Data;

namespace Microservices.OrderOutboxTablePublisherService
{
    public interface ICatalogOutboxSingletonDatabase
    {
        IDbConnection Connection { get; }
        bool DataReaderState { get; }

        void DataReaderBusy();
        void DataReaderReady();
        Task<int> ExecuteAsync(string sql);
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
    }
}
