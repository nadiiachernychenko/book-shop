using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab_infrastructure;
using lab_domain.Model;

namespace lab_infrastructure.Controllers;

[ApiController]
[Route("api/books")]
[Produces("application/json")]
public class BooksApiController : ControllerBase
{
    private readonly BooksShopWebContext _context;
    public BooksApiController(BooksShopWebContext context) => _context = context;

    // GET: /api/books?skip=0&limit=50
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int limit = 50)
    {
        if (limit <= 0) limit = 50;
        var items = await _context.Books
            .AsNoTracking()
            .Skip(skip).Take(limit)
            .ToListAsync();
        return Ok(items);
    }

    // GET: /api/books/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _context.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: /api/books
    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Create([FromBody] Book model)
    {
        try
        {
            _context.Books.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { status = "Ok", id = model.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "Error", message = ex.Message });
        }
    }

    // PUT: /api/books/5
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    public async Task<IActionResult> Update(int id, [FromBody] Book model)
    {
        if (id != model.Id)
            return BadRequest(new { status = "Error", message = "Route id != body id" });

        try
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { status = "Ok" });
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.Books.AnyAsync(e => e.Id == id);
            return exists
                ? StatusCode(500, new { status = "Error", message = "Concurrency error" })
                : NotFound(new { status = "Error", message = "Not found" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "Error", message = ex.Message });
        }
    }

    // DELETE: /api/books/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Books.FindAsync(id);
        if (entity is null)
            return NotFound(new { status = "Error", message = "Not found" });

        _context.Books.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok(new { status = "Ok" });
    }
}
