using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab_domain.Model;
using lab_infrastructure;

namespace lab_infrastructure.Controllers
{
    public class PublishersController : Controller
    {
        private readonly BooksShopWebContext _context;

        public PublishersController(BooksShopWebContext context)
        {
            _context = context;
        }

        // GET: Publishers
        public async Task<IActionResult> Index()
        {
            var items = await _context.Publishers.AsNoTracking().ToListAsync();
            return View(items); // будет искать Views/Publishers/Index.cshtml
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Publishers.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) return NotFound();

            return View(item); // Views/Publishers/Details.cshtml
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View(); // Views/Publishers/Create.cshtml
        }

        // POST: Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,Description")] Publisher model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Publishers.FindAsync(id);
            if (item == null) return NotFound();

            return View(item); // Views/Publishers/Edit.cshtml
        }

        // POST: Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,Description")] Publisher model)
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
                if (!_context.Publishers.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.Publishers.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) return NotFound();

            return View(item); // Views/Publishers/Delete.cshtml
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Publishers.FindAsync(id);
            if (item != null)
            {
                _context.Publishers.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
