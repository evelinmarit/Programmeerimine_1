using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;

namespace KooliProjekt.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Category>> List(int page, int pageSize)
        {
            return await _context.Categories.GetPagedAsync(page, pageSize);
        }

        public async Task<Category> Get(int id)
        {
            return await _context.Categories
                .Include(category => category.Products)
                .Where(category => category.Id == id)
                .FirstOrDefaultAsync();        
        }
        public async Task Save(Category category)
        {
            if (category.Id == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                _context.Categories.Update(category);
            }
            await _context.SaveChangesAsync();

        }
        public async Task Delete(int id)
        {
            await _context.Categories
                .Where(category => category.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
