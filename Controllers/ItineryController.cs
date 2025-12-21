using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItineryController : ControllerBase
    {
        private readonly int COUNT_SKIPPED_PER_PAGES = 100;

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public ItineryController(ILogger<ItineryController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Itinerary>>> Get([FromQuery] int count, [FromQuery] int page)
        {
            List<Itinerary> itineraries = await _context.Itineraries.Skip(page * COUNT_SKIPPED_PER_PAGES).Take(count).Select(item => new Itinerary
            {
                ID = item.ID,
                PositionPlanID = item.PositionPlanID,
                AUDCode = item.AUDCode,
                AUDName = item.AUDName,
                Operations = item.Operations,
                NumberPositions = item.NumberPositions,
                KitIncreasingKit = item.KitIncreasingKit
            }).ToListAsync();

            if (itineraries.Count == 0)
            {
                return NotFound();
            }
            else
            {
               return Ok(itineraries);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Itinerary> Get(int id)
        {
            Itinerary? itinerary = _context.Itineraries.FirstOrDefault(item => item.ID == id);

            if (itinerary == null) 
            {
                return NotFound();
            }
            else 
            {
                return Ok(itinerary);
            } 
        }

        [HttpGet("{id}/OperationsOfItinerary")]
        public async Task<ActionResult<IEnumerable<OperationsOfItinerary>>> GetOperationsOfItinerary(int id)
        {
            Itinerary? itinerary = await _context.Itineraries.FirstOrDefaultAsync(item => item.ID == id);

            if (itinerary == null)
            {
                return NotFound();
            }
            else
            {
                IList<OperationsOfItinerary> operations = itinerary.Operations;
                return Ok(operations);
            }
        }
    }
}
