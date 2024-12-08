using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using Microsoft.Win32;
using KooliProjekt.Services;
using KooliProjekt.Models;
using KooliProjekt.Data.Migrations;

namespace KooliProjekt.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItem)
        {
            _orderItemService = orderItem;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index(int page = 1, OrderItemIndexModel model = null)
        {
            model = model ?? new OrderItemIndexModel();
            model.Data = await _orderItemService.List(page, 10, model.Search);
            return View(model);
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderItem = await _orderItemService.Get(id.Value);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            var orders = _orderItemService.ListOrders().Result;
            ViewBag.OrderId = new SelectList(orders, "Id", "Id");
            var products = _orderItemService.ListProducts().Result;
            ViewBag.ProductId = new SelectList(products, "Id", "Name");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,PriceAtOrderTime,Quantity,OrderId")] OrderItem orderItem)
        {
            ModelState.Remove("Product");
            ModelState.Remove("Order");
            if (ModelState.IsValid)
            {
                await _orderItemService.Save(orderItem);
                return RedirectToAction(nameof(Index));
            }
            var orders = await _orderItemService.ListOrders();
            ViewBag.OrderId = new SelectList(orders, "Id", "OrderId", orderItem.OrderId);
            var products = await _orderItemService.ListProducts();
            ViewBag.ProductId = new SelectList(products, "Id", "Name", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderItem = await _orderItemService.Get(id.Value);
            if (orderItem == null)
            {
                return NotFound();
            }
            var orders = await _orderItemService.ListOrders();
            ViewBag.OrderId = new SelectList(orders, "Id", "Id", orderItem.OrderId);
            var products = await _orderItemService.ListProducts();
            ViewBag.ProductId = new SelectList(products, "Id", "Name", orderItem.ProductId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,PriceAtOrderTime,Quantity,OrderId")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Product");
            ModelState.Remove("Order");
            if (ModelState.IsValid)
            {
                await _orderItemService.Save(orderItem);
                return RedirectToAction(nameof(Index));
            }
            var orders = await _orderItemService.ListOrders();
            ViewBag.OrderId = new SelectList(orders, "Id", "Id", orderItem.OrderId);
            var products = await _orderItemService.ListProducts();
            ViewBag.ProductId = new SelectList(products, "Id", "Name", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderItem = await _orderItemService.Get(id.Value);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderItemService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
