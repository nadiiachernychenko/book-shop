using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab_infrastructure;
using lab_domain.Model;

namespace lab_infrastructure.Controllers;

[ApiController]
[Route("api/publishers")]
[Produces("application/json")]
public class PublishersApiController : ControllerBase
{
    private readonly BooksShopWebContext _context;
    public PublishersApiController(BooksShopWebContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int limit = 100)
    {
        if (limit <= 0) limit = 100;
        var items = await _context.Publishers.AsNoTracking().Skip(skip).Take(limit).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _context.Publishers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Publisher model)
    {
        try
        {
            _context.Publishers.Add(model);
            await _context.SaveChangesAsync();
            return Ok(new { status = "Ok", id = model.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = "Error", message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Publisher model)
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
            var exists = await _context.Publishers.AnyAsync(e => e.Id == id);
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
        var entity = await _context.Publishers.FindAsync(id);
        if (entity is null)
            return NotFound(new { status = "Error", message = "Not found" });

        _context.Publishers.Remove(entity);
        await _context.SaveChangesAsync();
        return Ok(new { status = "Ok" });
    }
}
