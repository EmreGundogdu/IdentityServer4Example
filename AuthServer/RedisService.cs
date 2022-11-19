using StackExchange.Redis;

namespace AuthServer
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _connectionMultiplier;
        public RedisService(string Url)
        {
            _connectionMultiplier = ConnectionMultiplexer.Connect("localhost:6379");
        }
        public IDatabase GetDb(int dbIndex)
        {
            return _connectionMultiplier.GetDatabase(dbIndex);
        }
    }
}
