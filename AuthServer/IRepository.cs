using System.Collections.Generic;
using System.Threading.Tasks;
using static AuthServer.RepositoryWithCache;

namespace AuthServer
{
    public interface IRepository
    {
        Task<string> CreateAsync(Product product);
        Task<Product> Get(int id);
        Task<List<Product>> GetAllAsync();
    }
}
