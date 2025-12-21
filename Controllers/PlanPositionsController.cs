using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlanPositionsController : ControllerBase
    {
        private readonly int COUNT_SKIPPED_PER_PAGES = 100;

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public PlanPositionsController(ILogger<PlanPositionsController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanPosition>>> Get([FromQuery] int count, [FromQuery] int page)
        {
            List<PlanPosition> planPositions = await _context.PlanPositions.Skip(page * COUNT_SKIPPED_PER_PAGES).Take(count).Select(item => new PlanPosition
            {
                ID = item.ID,
                ProductID = item.ProductID
            }).ToListAsync();

            if (planPositions.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(planPositions);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanPosition>> Get(int id)
        {
            PlanPosition? planPosition = await _context.PlanPositions.FirstOrDefaultAsync(item => item.ID == id);

            if (planPosition == null) {
                return NotFound();
            }
            else {
                return Ok(planPosition);
            } 
        }
    }
}
