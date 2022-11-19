using System.Collections.Generic;
using System.Threading.Tasks;
using static AuthServer.RepositoryWithCache;

namespace AuthServer
{
    public class Repository : IRepository
    {
        public async Task<string> CreateAsync(Product product)
        {
            return product.Name;
        }

        public async Task<Product> Get(int id)
        {
            Product dsa = new Product
            {
                Id = 1,
                Name = "dsadsa"
            };
            return dsa;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            List<Product> dsa = new List<Product>
            {
               new Product{Id=1,Name="dsadsa"},
               new Product{Id=2,Name="gfdgd"},
               new Product{Id=3,Name="cxzcz"},
               new Product{Id=4,Name="ewqeq"},
               new Product{Id=5,Name="jhgjg"},
            };
            return dsa;
        }
    }
}
