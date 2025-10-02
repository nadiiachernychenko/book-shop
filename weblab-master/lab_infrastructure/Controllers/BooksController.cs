using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab_domain.Model;
using lab_infrastructure;

namespace lab_infrastructure.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksShopWebContext _context;

        public BooksController(BooksShopWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<JsonResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term) || term.Length < 3)
                return Json(new List<object>());

            var searchResults = await _context.Books
                .Include(c => c.Publisher)
                .Include(c => c.BookTags)
                .Where(c => c.Name.Contains(term) ||
                            c.Publisher.Name.Contains(term) ||
                            c.BookTags.Any(t => t.Tag.Contains(term)))
                .Take(10)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    brand = c.Publisher.Name,
                    price = c.Price,
                    imageUrl = c.ImageUrl,
                    stock = c.Stock,
                    label = c.Name, 
                    value = c.Name  
                })
                .ToListAsync();

            return Json(searchResults);
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var bookShopWebContext = _context.Books
                .Include(c => c.Publisher)
                .Include(c => c.Genre)
                .Include(c => c.BookTags)      
                .Include(c => c.Promotions)    
                .Include(c => c.Discounts);
            return View(await bookShopWebContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(c => c.Publisher)
                .Include(c => c.Genre)
                .Include(c => c.BookTags)
                .Include(c => c.Promotions)
                .Include(c => c.Discounts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.BookGenres, "Id", "GenreName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PublisherId,GenreId,ReleaseDate,Price,Stock,Description,Rating,ImageUrl")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["GenreId"] = new SelectList(_context.BookGenres, "Id", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["GenreId"] = new SelectList(_context.BookGenres, "Id", "GenreName", book.GenreId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PublisherId,GenreId,ReleaseDate,Price,Stock,Description,Rating,ImageUrl")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["GenreId"] = new SelectList(_context.BookGenres, "Id", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(c => c.Publisher)
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
