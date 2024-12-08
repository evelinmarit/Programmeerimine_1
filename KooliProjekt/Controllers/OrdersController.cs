using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = await _orderService.Get(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var buyers = _orderService.ListBuyers().Result;
            ViewBag.BuyerId = new SelectList(buyers, "Id", "Email");
            var orderItems = _orderService.ListOrderItems().Result;
            ViewBag.OrderItemId = new SelectList(orderItems, "Id", "Product");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,Status,BuyerId,OrderItems")] Order order)
        {
            ModelState.Remove("Buyer");
            if (ModelState.IsValid)
            {
                await _orderService.Save(order);
                return RedirectToAction(nameof(Index));
            }

            var buyers = await _orderService.ListBuyers();
            ViewBag.BuyerId = new SelectList(buyers, "Id", "Email", order.BuyerId);
            var orderItems = await _orderService.ListOrderItems();
            ViewBag.OrderItemId = new SelectList(orderItems, "Id", "Product", order.OrderItems);
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
            if (order == null)
            {
                return NotFound();
            }
            var buyers = await _orderService.ListBuyers();
            ViewBag.BuyerId = new SelectList(buyers, "Id", "Email", order.BuyerId);
            var orderItems = await _orderService.ListOrderItems();
            ViewBag.OrderItemId = new SelectList(orderItems, "Id", "Product", order.OrderItems);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,Status,BuyerId,OrderItems")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Buyer");
            if (ModelState.IsValid)
            {
                await _orderService.Save(order);
                return RedirectToAction(nameof(Index));
            }
            var buyers = await _orderService.ListBuyers();
            ViewBag.BuyerId = new SelectList(buyers, "Id", "Email", order.BuyerId);
            var orderItems = await _orderService.ListOrderItems();
            ViewBag.orderItemId = new SelectList(orderItems, "Id", "Product", order.OrderItems);
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
            await _orderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
