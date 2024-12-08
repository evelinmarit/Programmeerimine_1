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
using KooliProjekt.Search;
using KooliProjekt.Models;

namespace KooliProjekt.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page = 1, ProductIndexModel model = null)
        {
            model = model ?? new ProductIndexModel();
            model.Data = await _productService.List(page, 10, model.Search);
            return View(model);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productService.Get(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _productService.ListCategories().Result;
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name"); // Või kasuta ViewData
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PhotoUrl,Price,CategoryId,AtStock")] Product product)
        {
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                await _productService.Save(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _productService.ListCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryId);
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
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _productService.ListCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PhotoUrl,Price,CategoryId,AtStock")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                await _productService.Save(product);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _productService.ListCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", product.CategoryId);
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
            await _productService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
