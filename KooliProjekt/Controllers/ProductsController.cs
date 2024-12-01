using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.Identity.Client;

namespace KooliProjekt.Controllers
{
    public class ProductsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public ProductsController(/*ApplicationDbContext context,*/IProductService productService)
        {
            //_context = context;
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page)
        {
            var data = await _productService.List(page, 10);
            return View(data);
            //var applicationDbContext = _context.Products.Include(p => p.Category);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productService.Get(id.Value);
            //var product = await _context.Products
            //    .Include(p => p.Category)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _productService.ListCategories();
            ViewData["CategoryId"] = categories;
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PhotoUrl,Price,CategoryId")] Product product)
        {
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                await _productService.Save(product);
                //_context.Add(product);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var categories = await _productService.ListCategories();
            ViewData["CategoryId"] = categories; // Kategooriate andmed ViewData-sse
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productService.Get(id.Value);
            //var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _productService.ListCategories();
            ViewData["CategoryId"] = categories; // Kategooriate andmed ViewData-sse
            return View(product);
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            //return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PhotoUrl,Price,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.Update(product);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!ProductExists(product.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                await _productService.Save(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _productService.ListCategories();
            ViewData["CategoryId"] = categories; // Kategooriate andmed ViewData-sse
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productService.Get(id.Value);
            //var product = await _context.Products
                //.Include(p => p.Category)
                //.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            //if (product != null)
            //{
            //    _context.Products.Remove(product);
            //}

            //await _context.SaveChangesAsync();
            await _productService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
