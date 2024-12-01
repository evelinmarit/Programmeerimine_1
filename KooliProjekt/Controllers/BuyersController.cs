using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;
using KooliProjekt.Data.Migrations;

namespace KooliProjekt.Controllers
{
    public class BuyersController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IBuyerService _buyerService;
        public BuyersController(/*ApplicationDbContext context, */IBuyerService buyerService)
        {
            //_context = context;
            _buyerService = buyerService;
        }

        // GET: Buyers
        public async Task<IActionResult> Index(int page)
        {
            //return View(await _context.Buyers.ToListAsync());
            var data = await _buyerService.List(page, 10);
            return View(data);
        }

        // GET: Buyers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var buyer = await _buyerService.Get(id.Value);
            //var buyer = await _context.Buyers
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // GET: Buyers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buyers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShippingAddress,Email,PhoneNumber,RegisteredDate,LoyaltyPoints,Discount")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                await _buyerService.Save(buyer);
                //_context.Add(buyer);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buyer);
        }

        // GET: Buyers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var buyer = await _buyerService.Get(id.Value);
            //var buyer = await _context.Buyers.FindAsync(id);
            if (buyer == null)
            {
                return NotFound();
            }
            return View(buyer);
        }

        // POST: Buyers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShippingAddress,Email,PhoneNumber,RegisteredDate,LoyaltyPoints,Discount")] Buyer buyer)
        {
            if (id != buyer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                //    _context.Update(buyer);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!BuyerExists(buyer.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                await _buyerService.Save(buyer);
                return RedirectToAction(nameof(Index));
            }
            return View(buyer);
        }

        // GET: Buyers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var buyer = await _buyerService.Get(id.Value);
            //var buyer = await _context.Buyers
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (buyer == null)
            {
                return NotFound();
            }

            return View(buyer);
        }

        // POST: Buyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var buyer = await _context.Buyers.FindAsync(id);
            //if (buyer != null)
            //{
            //    _context.Buyers.Remove(buyer);
            //}

            //await _context.SaveChangesAsync();
            await _buyerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool BuyerExists(int id)
        //{
        //    return _context.Buyers.Any(e => e.Id == id);
        //}
    }
}
