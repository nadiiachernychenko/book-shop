using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab_domain.Model;

namespace lab_infrastructure
{
    public class DiscountsController : Controller
    {
        private readonly BooksShopWebContext _context;

        public DiscountsController(BooksShopWebContext context)
        {
            _context = context;
        }

        // GET: Discounts
        public async Task<IActionResult> Index()
        {
            var discounts = await _context.Discounts
                .Include(d => d.Book)
                .AsNoTracking()
                .ToListAsync();

            return View(discounts);
        }

        // GET: Discounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var discount = await _context.Discounts
                .Include(d => d.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (discount == null) return NotFound();

            return View(discount);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books.AsNoTracking(), "Id", "Name");
            return View();
        }

        // POST: Discounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,DiscountPercentage,StartDate,EndDate")] Discount discount)
        {
            if (discount.StartDate >= discount.EndDate)
                ModelState.AddModelError("EndDate", "End date must be after start date");

            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books.AsNoTracking(), "Id", "Name", discount.BookId);
                return View(discount);
            }

            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Discounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) return NotFound();

            ViewData["BookId"] = new SelectList(_context.Books.AsNoTracking(), "Id", "Name", discount.BookId);
            return View(discount);
        }

        // POST: Discounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,DiscountPercentage,StartDate,EndDate")] Discount discount)
        {
            if (id != discount.Id) return NotFound();

            if (discount.StartDate >= discount.EndDate)
                ModelState.AddModelError("EndDate", "End date must be after start date");

            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books.AsNoTracking(), "Id", "Name", discount.BookId);
                return View(discount);
            }

            try
            {
                _context.Update(discount);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Discounts.AnyAsync(e => e.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var discount = await _context.Discounts
                .Include(d => d.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (discount == null) return NotFound();

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
                _context.Discounts.Remove(discount);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
