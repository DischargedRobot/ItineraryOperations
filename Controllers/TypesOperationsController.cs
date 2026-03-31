using ItineraryOperations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItineraryOperations.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TypesOperationsController : ControllerBase
    {


        private readonly PostgresContext _context;

        private readonly ILogger<TypesOperationsController> _logger;


        public TypesOperationsController(ILogger<TypesOperationsController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]       
        public async Task<ActionResult<IEnumerable<TypesOperations>>> Get()
        {
            List<TypesOperations> typesOperations = await _context.TypesOperations.Select(index => new TypesOperations
            {
                ID = index.ID,
                Name = index.Name
            }).ToListAsync();

            if (typesOperations.Count == 0) 
            {
                return NotFound();
            }
            else
            {
                return Ok(typesOperations);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypesOperations>> GetById(int id)
        {
            TypesOperations? typesOperations = await _context.TypesOperations.FirstOrDefaultAsync(item => item.ID == id);
            if (typesOperations == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(typesOperations);
            }
        }
    }
}
