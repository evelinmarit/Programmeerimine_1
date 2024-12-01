using KooliProjekt.Data;
using KooliProjekt.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<Product>> List(int page, int pageSize)
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            //await applicationDbContext.ToListAsync();
            return await _context.Products.GetPagedAsync(page, pageSize);
        }
        public async Task<Product> Get(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
            
        }
        public async Task<List<Data.Category>> ListCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task Save(Product product)
        {
            await _context.Products
                .Include(product => product.Category)
                .Where(product => product.Id == 0)
                .FirstOrDefaultAsync();
            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                _context.Products.Update(product);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await _context.Products
                .Where(product => product.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
