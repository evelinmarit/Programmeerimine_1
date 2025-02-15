using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly ApplicationDbContext _context;

        public OrderItemService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<OrderItem>> List(int page, int pageSize, OrderItemSearch search = null)
        {
            var query = _context.OrderItems.AsQueryable();
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.OrderSearch))
                {
                    search.OrderSearch = search.OrderSearch.Trim();
                    if (int.TryParse(search.OrderSearch, out int orderId))
                    {
                        query = query.Where(orderItem => orderItem.OrderId == orderId);
                    }
                }
            }
            return await query
                .OrderBy(orderItem => orderItem.OrderId)
                .Include(orderItem => orderItem.Product)
                .Include(orderItem => orderItem.Order)
                .GetPagedAsync(page, pageSize);
        }
            public async Task<List<Order>> ListOrders()
            {
                return await _context.Orders.ToListAsync();
            }
            public async Task<List<Product>> ListProducts()
            {
                return await _context.Products.ToListAsync();
            }
            public async Task<OrderItem> Get(int id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task Save(OrderItem orderItem)
        {
            await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Id == 0)
                .FirstOrDefaultAsync();
            if (orderItem.Id == 0)
            {
                _context.OrderItems.Add(orderItem);
            }
            else
            {
                _context.OrderItems.Update(orderItem);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return;
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            //await _context.OrderItems
            //    .Where(oi => oi.Id == id)
            //    .ExecuteDeleteAsync();
        }
    }
}
