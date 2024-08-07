using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Microservices.OrderOutboxTablePublisherService
{
    public class CatalogOutboxSingletonDatabase : ICatalogOutboxSingletonDatabase
    {
         IDbConnection _dbConnection;

        public CatalogOutboxSingletonDatabase(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("SQLServer"));
        }

        public IDbConnection Connection
        {
            get
            {
                if(_dbConnection.State == ConnectionState.Closed)
                    _dbConnection.Open();
                return _dbConnection;
            }
        }
        public bool dataReaderState = true;
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql) => await _dbConnection.QueryAsync<T>(sql);
        public async Task<int> ExecuteAsync(string sql) => await _dbConnection.ExecuteAsync(sql);
        public void DataReaderBusy() => dataReaderState = false; 
        public void DataReaderReady() => dataReaderState = true;
        public bool DataReaderState => dataReaderState;

    }
}
