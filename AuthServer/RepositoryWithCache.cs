using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthServer
{
    public class RepositoryWithCache : IRepository
    {
        private const string createkey = "createkey";
        private const string getkey = "getkey";
        private const string key = "cacheKey";
        IRepository repository;
        RedisService redisService;
        IDatabase cacheRepo;
        public RepositoryWithCache(IRepository repository, RedisService redisService)
        {
            this.repository = repository;
            this.redisService = redisService;
            cacheRepo = redisService.GetDb(1);  
        }

        public async Task<string> CreateAsync(Product product)
        {
            await cacheRepo.HashSetAsync(createkey, product.Id, JsonSerializer.Serialize(product));
            return product.Name;
        }

        public async Task<Product> Get(int id)
        {
            if (cacheRepo.KeyExists(createkey))
            {
                var product = await cacheRepo.HashGetAsync(key,id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            };
            var products = await GetAllAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public async Task<List<Product>> GetAllAsync()
        {
            List<Product> list = new List<Product>()
            {
                new Product{Id=1,Name="Klavye"},
                new Product{Id=2,Name="Mouse"},
                new Product{Id=3,Name="Mönitör"},
                new Product{Id=4,Name="Masa"}
            };
            var cacheDatas = await cacheRepo.HashGetAllAsync(key);
            foreach (var item in cacheDatas)
            {
                var product = JsonSerializer.Serialize(item.Value);
            }
            return list;
        }
    }
}
