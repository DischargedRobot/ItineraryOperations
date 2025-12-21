using ItineraryOperations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItineraryOperations.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ExecutorsController : ControllerBase
    {


        private readonly PostgresContext _context;

        private readonly ILogger<ExecutorsController> _logger;


        public ExecutorsController(ILogger<ExecutorsController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]       
        public async Task<ActionResult<IEnumerable<Executors>>> Get()
        {
            Executors.Felling(_context);
            return Ok(await _context.Executors.Select(item => new Executors
            {
                ID = item.ID,
                isBrigade = item.isBrigade,
                Members = item.Members,
                DivisionID = item.DivisionID,
            })
            .ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Executors>> GetById(int id)
        {
            Executors? executor = await _context.Executors.FirstOrDefaultAsync(item => item.ID == id);
            if (executor == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(executor);
            }
        }

        [HttpGet("by-division")]
        public async Task<ActionResult<IEnumerable<Executors>>> GetByDivisionID([FromQuery] int divisionID)
        {
            List<Executors> executors = await _context.Executors.Where(item => item.DivisionID == divisionID).ToListAsync();
            if (executors.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(executors);
            }
        }

    }
}
