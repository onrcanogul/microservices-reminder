using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderOutboxTablePublisherService
{
    public interface IOrderOutboxSingletonDatabase
    {
        IDbConnection Connection { get; }
        bool DataReaderState { get; }

        void DataReaderBusy();
        void DataReaderReady();
        Task<int> ExecuteAsync(string sql);
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
    }
}
