using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> List(int page, int pageSize, ProductSearch search = null);
        Task<List<Data.Category>> ListCategories();
        Task<Product> Get(int id);
        Task Save(Product product);
        Task Delete(int id);
    }
}
