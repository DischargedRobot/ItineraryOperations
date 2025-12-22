using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly int COUNT_SKIPPED_PER_PAGES = 100;

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public ProductsController(ILogger<ProductsController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Get([FromQuery] int count, [FromQuery] int page)
        {
            List<Products> products = await _context.Products.OrderBy(p => p.ID).Skip(page * COUNT_SKIPPED_PER_PAGES).Take(count).ToListAsync();

            if (products.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(products);
            }
        }

        // Получение списка продуктов с фильтрацией по подразделению
        [HttpGet("{divisionID}")]
        public async Task<ActionResult<IEnumerable<Products>>> GetAllByDivision([FromQuery] int count, [FromQuery] int page, int divisionID)
        {
            List<Products> products = await _context.Products.OrderBy(p => p.ID).Where(p => p.DivisionID == divisionID).Skip(page * COUNT_SKIPPED_PER_PAGES).Take(count).ToListAsync();

            if (products.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(products);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> Get(int id)
        {
            Products? product = await _context.Products.FirstOrDefaultAsync(item => item.ID == id);

            if (product == null) 
            {
                return NotFound();
            }
            else 
            {
                return Ok(product);
            } 
        }

        [HttpGet("by-AUDCode/{AUDCode}")]
        public ActionResult<Products> GetByAUDCode(string AUDCode)
        {
            Products? product = _context.Products.FirstOrDefault(item => item.AUDCode == AUDCode);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }
    }
}
