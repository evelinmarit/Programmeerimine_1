using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class BuyerService : IBuyerService
    {
        private readonly ApplicationDbContext _context;

        public BuyerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<Buyer>> List(int page, int pageSize)
        {
            var applicationDbContext = _context.Buyers.Include(b => b.Orders);
            return await _context.Buyers.GetPagedAsync(page, pageSize);
        }
        public async Task<Buyer> Get(int id)
        {
            return await _context.Buyers
                .Include(b => b.Orders)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task Save(Buyer buyer)
        {
            if (buyer.Id == 0)
            {
                _context.Buyers.Add(buyer);
            }
            else
            {
                _context.Buyers.Update(buyer);
            }
            await _context.SaveChangesAsync();

        }
        public async Task Delete(int id)
        {
            await _context.Buyers
                .Where(buyer => buyer.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
