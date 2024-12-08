using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using KooliProjekt.Models;

namespace KooliProjekt.Controllers
{
    public class OrdersController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        public OrdersController(/*ApplicationDbContext context, */IOrderService orderService)
        {
            //_context = context;
            _orderService = orderService;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int page = 1, OrderIndexModel model = null)
        {
            model = model ?? new OrderIndexModel();
            model.Data = await _orderService.List(page, 10, model.Search);
            return View(model);
            //var applicationDbContext = _context.Orders.Include(o => o.Buyer);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _orderService.Get(id.Value);
            //var order = await _context.Orders
            //    .Include(o => o.Buyer)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var buyers = _orderService.ListBuyers();
            var orderItems = _orderService.ListOrderItems();
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,Status,BuyerId")] Order order)
        {
            ModelState.Remove("Buyer");
            if (ModelState.IsValid)
            {
                await _orderService.Save(order);
                //_context.Add(order);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //var buyers = _orderService.ListBuyers();
            //var orderItems = _orderService.ListOrderItems();
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "Email", order.BuyerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _orderService.Get(id.Value);
            //var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //var buyers = _orderService.ListBuyers();
            //var orderItems = _orderService.ListOrderItems();
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "Email", order.BuyerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,Status,BuyerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Buyer");
            if (ModelState.IsValid)
            {
                //try
                //{

                //    _context.Update(order);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!OrderExists(order.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                await _orderService.Save(order);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["BuyerId"] = new SelectList(_context.Buyers, "Id", "Email", order.BuyerId);
            var buyers = _orderService.ListBuyers();
            var orderItems = _orderService.ListOrderItems();
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _orderService.Get(id.Value);
            //var order = await _context.Orders
            //    .Include(o => o.Buyer)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var order = await _context.Orders.FindAsync(id);
            //if (order != null)
            //{
            //    _context.Orders.Remove(order);
            //}

            //await _context.SaveChangesAsync();
            await _orderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.Id == id);
        //}
    }
}
