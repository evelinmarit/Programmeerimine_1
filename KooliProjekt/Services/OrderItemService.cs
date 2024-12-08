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
        public async Task<PagedResult<OrderItem>> List(int page, int pageSize)
        {
            var applicationDbContext = _context.OrderItems.Include(oi => oi.Order).Include(oi => oi.Product);
            return await _context.OrderItems.GetPagedAsync(page, pageSize);            
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
                .Include(oi => oi.Product).Include(oi => oi.Order)
                .Where(oi => oi.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task Save(OrderItem orderItem)
        {
            await _context.OrderItems
                .Include(oi => oi.Product).Include(oi => oi.Order)
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
            await _context.OrderItems
                .Where(oi => oi.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
