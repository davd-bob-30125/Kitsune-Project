using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kitsune.Data;
using Kitsune.Models;
using MvcMovie.Models;

namespace Kitsune.Controllers
{
    public class ProductsController : Controller
    {
        private readonly LibraryContext _context;

        public ProductsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string productType, string searchString)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            IQueryable<string> genreQuery = from p in _context.Product
                                            orderby p.Type
                                            select p.Type;
            var products = from p in _context.Product
                          select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name!.Contains(searchString));
            }
           
            if (!string.IsNullOrEmpty(productType))
            {
                products = products.Where(x => x.Type == productType);
            }

            var productTypeVM = new ProductTypeViewModel
            {
                Types = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Products = await products.ToListAsync()
            };


            return View(productTypeVM);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity,Type")] Product product)
        {
            
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity,Type,OrderItems")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            
            
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Product == null)
            {
                return Problem("Entity set 'LibraryContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> createOrder([FromRoute] int id)
        {

            Order order = new Order()
            {
                UserId = 10,
                Status = "Created",
                CreatedAt = DateTime.Now,
                FinishedAt = DateTime.MinValue,
            };

            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Create", "OrderItems", new { ProductId = id , OrderId = order.Id});
        }
    }
}
