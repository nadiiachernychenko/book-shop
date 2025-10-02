using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab_infrastructure;
using lab_domain.Model;

namespace lab_infrastructure.Controllers;

[ApiController]
[Route("api/book-tags")]
[Produces("application/json")]
public class BookTagsApiController : ControllerBase
{
    private readonly BooksShopWebContext _context;
    public BookTagsApiController(BooksShopWebContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int limit = 100)
    {
        if (limit <= 0) limit = 100;
        var items = await _context.BookTags
            .AsNoTracking()
            .Include(t => t.Book)
            .Skip(skip).Take(limit)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _context.BookTags
            .AsNoTracking()
            .Include(t => t.Book)
            .FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookTag model)
    {
        try
        {
            _context.BookTags.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { status = "Ok", id = model.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "Error", message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookTag model)
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
            var exists = await _context.BookTags.AnyAsync(e => e.Id == id);
            return exists
                ? StatusCode(500, new { status = "Error", message = "Concurrency error" })
                : NotFound(new { status = "Error", message = "Not found" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "Error", message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.BookTags.FindAsync(id);
        if (entity is null)
            return NotFound(new { status = "Error", message = "Not found" });

        _context.BookTags.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok(new { status = "Ok" });
    }
}
