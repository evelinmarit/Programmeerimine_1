using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOrderItemService
    {
        Task<PagedResult<OrderItem>> List(int page, int pageSize, OrderItemSearch search = null);
        Task<List<Order>> ListOrders();
        Task<List<Product>> ListProducts();
        Task<OrderItem> Get(int id);
        Task Save(OrderItem orderItem);
        Task Delete(int id);
    }
}
