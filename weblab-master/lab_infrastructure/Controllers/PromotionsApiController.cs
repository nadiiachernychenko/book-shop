using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using lab_infrastructure;
using lab_domain.Model;

namespace lab_infrastructure.Controllers;

[ApiController]
[Route("api/promotions")]
[Produces("application/json")]
public class PromotionsApiController : ControllerBase
{
    private readonly BooksShopWebContext _context;
    public PromotionsApiController(BooksShopWebContext context) => _context = context;

    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int limit = 100)
    {
        if (limit <= 0) limit = 100;
        if (skip < 0) skip = 0;

        var total = await _context.Promotions.CountAsync();

        var items = await _context.Promotions
            .AsNoTracking()
            .Include(p => p.Book)
            .OrderByDescending(p => p.StartDate)
            .Skip(skip).Take(limit)
            .ToListAsync();

        return Ok(new { Items = items, TotalCount = total });
    }

    // GET: /api/promotions/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var item = await _context.Promotions
            .AsNoTracking()
            .Include(p => p.Book)
            .FirstOrDefaultAsync(x => x.Id == id);

        return item is null ? NotFound(new { status = "Error", message = "Not found" }) : Ok(item);
    }

    // POST: /api/promotions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Promotion model)
    {
        
        ModelState.Remove(nameof(Promotion.Book));
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        
        var bookExists = await _context.Books.AnyAsync(b => b.Id == model.BookId);
        if (!bookExists) return BadRequest(new { status = "Error", message = "BookId not found" });

        _context.Promotions.Add(model);
        await _context.SaveChangesAsync();

        
        return CreatedAtAction(nameof(GetOne), new { id = model.Id }, new
        {
            model.Id,
            model.BookId,
            model.PromoName,
            model.Description,
            model.StartDate,
            model.EndDate
        });
    }

    // PUT: /api/promotions/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Promotion dto)
    {
        if (id != dto.Id)
            return BadRequest(new { status = "Error", message = "Route id != body id" });

        ModelState.Remove(nameof(Promotion.Book));
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var entity = await _context.Promotions.FindAsync(id);
        if (entity is null) return NotFound(new { status = "Error", message = "Not found" });

       
        if (!await _context.Books.AnyAsync(b => b.Id == dto.BookId))
            return BadRequest(new { status = "Error", message = "BookId not found" });

        
        entity.BookId = dto.BookId;
        entity.PromoName = dto.PromoName;
        entity.Description = dto.Description;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;

        await _context.SaveChangesAsync();
        return Ok(new { status = "Ok" });
    }

    // DELETE: /api/promotions/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Promotions.FindAsync(id);
        if (entity is null)
            return NotFound(new { status = "Error", message = "Not found" });

        _context.Promotions.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
