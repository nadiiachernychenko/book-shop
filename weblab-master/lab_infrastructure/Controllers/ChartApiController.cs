using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace lab_infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartApiController : ControllerBase
    {
        private readonly BooksShopWebContext _context;

        public ChartApiController(BooksShopWebContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStockData()
        {
            var stockData = await _context.Books
                .Select(deck => new { deck.Name, deck.Stock })
                .ToListAsync();

            return Ok(stockData);  
        }
    }
}

