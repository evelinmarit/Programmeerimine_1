using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<Order>> List(int page, int pageSize, OrderSearch search = null)
        {
            var query = _context.Orders.AsQueryable();
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Keyword))
                {
                    search.Keyword = search.Keyword.Trim();
                    if(int.TryParse(search.Keyword, out int orderId))
                    {
                        query = query.Where(order => order.Id == orderId);
                    }
                }
                if (!string.IsNullOrWhiteSpace(search.Status))
                {
                    search.Status = search.Status.Trim();
                    query = query.Where(order => order.Status == search.Status);
                }
            }
           
            return await query
                .OrderBy(order => order.OrderDate)
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                .GetPagedAsync(page, pageSize);
            
        }
        public async Task<Order> Get(int id)
        {
            return await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                .Where(o => o.Id == id)
                .FirstOrDefaultAsync();

        }
        public async Task<List<Data.Buyer>> ListBuyers()
        {
            return await _context.Buyers.ToListAsync();
        }
        public async Task<List<Data.OrderItem>> ListOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }
        public async Task Save(Order order)
        {
            await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                .Where(order => order.Id == 0)
                .FirstOrDefaultAsync();
            if (order.Id == 0)
            {
                _context.Orders.Add(order);
            }
            else
            {
                _context.Orders.Update(order);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _context.Orders
                .Where(order => order.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
