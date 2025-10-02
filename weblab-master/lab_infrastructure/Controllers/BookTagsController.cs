using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab_domain.Model;
using lab_infrastructure;

namespace lab_infrastructure.Controllers
{
    public class BookTagsController : Controller
    {
        private readonly BooksShopWebContext _context;
        public BookTagsController(BooksShopWebContext context) => _context = context;

        // GET: /BookTags
        public async Task<IActionResult> Index()
        {
            var items = await _context.BookTags
                .Include(t => t.Book)
                .AsNoTracking()
                .ToListAsync();
            return View(items); // Views/BookTags/Index.cshtml
        }

        // GET: /BookTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var item = await _context.BookTags
                .Include(t => t.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return item is null ? NotFound() : View(item);
        }

        // GET: /BookTags/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name");
            return View();
        }

        // POST: /BookTags/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,Tag")] BookTag model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", model.BookId);
                return View(model);
            }

            _context.BookTags.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /BookTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();
            var item = await _context.BookTags.FindAsync(id);
            if (item is null) return NotFound();

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", item.BookId);
            return View(item);
        }

        // POST: /BookTags/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,Tag")] BookTag model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", model.BookId);
                return View(model);
            }

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.BookTags.AnyAsync(e => e.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /BookTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var item = await _context.BookTags
                .Include(t => t.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return item is null ? NotFound() : View(item);
        }

        // POST: /BookTags/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.BookTags.FindAsync(id);
            if (item != null)
            {
                _context.BookTags.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
