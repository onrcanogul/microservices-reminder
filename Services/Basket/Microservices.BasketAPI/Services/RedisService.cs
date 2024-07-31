using StackExchange.Redis;

namespace Microservices.BasketAPI.Services
{
    public class RedisService(string host, int port)
    {
        private ConnectionMultiplexer connectionMultiplexer;
        public void Connect() => connectionMultiplexer = ConnectionMultiplexer.Connect($"{host}:{port}");
        public IDatabase GetDatabase(int db = 1) => connectionMultiplexer.GetDatabase(db);
    }
}
