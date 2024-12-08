using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IOrderItemService
    {
        Task<PagedResult<OrderItem>> List(int page, int pageSize);
        Task<List<Order>> ListOrders();
        Task<List<Product>> ListProducts();
        Task<OrderItem> Get(int id);
        Task Save(OrderItem orderItem);
        Task Delete(int id);
    }
}
