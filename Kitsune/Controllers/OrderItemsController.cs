using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kitsune.Data;
using Kitsune.Models;

namespace Kitsune.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly LibraryContext _context;

        public OrderItemsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.OrderItem.Include(o => o.Order).Include(o => o.Product);
            return View(await libraryContext.ToListAsync());
        }

        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Where(m => m.OrderId == id)
                .ToListAsync();
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }



        // GET: OrderItems/Create
        public IActionResult Create(int ProductId, int OrderId)
        {
            ViewData["OrderId"] = _context.Order.Where(o => o.Id == OrderId);
            ViewData["ProductId"] = _context.Product.Where(p => p.Id == ProductId);
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,OrderId,Quantity")] OrderItem orderItem)
        {
            
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Orders");
            }
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", orderItem.ProductId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,OrderId,Quantity")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Set<Order>(), "Id", "Id", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderItem == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.OrderItem == null)
            {
                return Problem("Entity set 'LibraryContext.OrderItem'  is null.");
            }
            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItem.Remove(orderItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
          return (_context.OrderItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
