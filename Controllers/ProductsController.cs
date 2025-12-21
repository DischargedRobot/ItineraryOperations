using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public ProductsController(ILogger<ProductsController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Get()
        {
            return Ok(await _context.Products.Select( item => new Products
            {
                ID = item.ID,
                Name = item.Name,
            }).ToArrayAsync());
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

        [HttpGet("by-AUDCode")]
        public ActionResult<Products> GetByAUDCode([FromQuery] string AUDCode)
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
