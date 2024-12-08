using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOrderService
    {
        Task<PagedResult<Order>> List(int page, int pageSize, OrderSearch search = null);
        Task<Order> Get(int id);
        Task<List<Data.Buyer>> ListBuyers();
        Task<List<Data.OrderItem>> ListOrderItems();
        Task Save(Order order);
        Task Delete(int id);

    }
}
