using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab_domain.Model;
using lab_infrastructure;

namespace lab_infrastructure.Controllers
{
    public class BookGenresController : Controller
    {
        private readonly BooksShopWebContext _context;
        public BookGenresController(BooksShopWebContext context) => _context = context;

        // GET: /BookGenres
        public async Task<IActionResult> Index()
        {
            var items = await _context.BookGenres.AsNoTracking().ToListAsync();
            return View(items); // Views/BookGenres/Index.cshtml
        }

        // GET: /BookGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();
            var item = await _context.BookGenres.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return item is null ? NotFound() : View(item);
        }

        // GET: /BookGenres/Create
        public IActionResult Create() => View();

        // POST: /BookGenres/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenreName")] BookGenre model)
        {
            if (!ModelState.IsValid) return View(model);
            _context.BookGenres.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /BookGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();
            var item = await _context.BookGenres.FindAsync(id);
            return item is null ? NotFound() : View(item);
        }

        // POST: /BookGenres/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenreName")] BookGenre model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.BookGenres.AnyAsync(e => e.Id == id);
                if (!exists) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /BookGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            var item = await _context.BookGenres.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return item is null ? NotFound() : View(item);
        }

        // POST: /BookGenres/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.BookGenres.FindAsync(id);
            if (item != null)
            {
                _context.BookGenres.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
