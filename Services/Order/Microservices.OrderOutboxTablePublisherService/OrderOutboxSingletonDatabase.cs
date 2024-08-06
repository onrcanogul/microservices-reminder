using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderOutboxTablePublisherService
{
    public class OrderOutboxSingletonDatabase : IOrderOutboxSingletonDatabase
    {
         IDbConnection _dbConnection;

        public OrderOutboxSingletonDatabase(IConfiguration configuration)
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
        public void DataReaderBusy() => dataReaderState = false; //singleton eklediğimiz için statesine bakmamız lazim
        public void DataReaderReady() => dataReaderState = true;
        public bool DataReaderState => dataReaderState;

    }
}
