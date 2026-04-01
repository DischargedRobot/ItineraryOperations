using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
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
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(ProductDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError400Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]

        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] int count = 100, [FromQuery] int page = 1)
        {
            List<ProductDto> products = await _context.Products
                .OrderBy(p => p.ID)
                .Skip((page-1) * COUNT_SKIPPED_PER_PAGES)
                .Take(count)
                .Select(product => new ProductDto(product))
                .ToListAsync();

            Console.WriteLine(products);
            if (products.Count == 0)
            {
                return NotFound(new APIError { Message = "Изделий нет" });
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
