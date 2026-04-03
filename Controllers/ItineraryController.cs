using ItineraryOperations.Lib;
using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly int COUNT_SKIPPED_PER_PAGES = 100;

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public ItineraryController(ILogger<ItineraryController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Itinerary>>> Get([FromQuery] int count, [FromQuery] int page)
        {
            List<Itinerary> itineraries = await _context.Itineraries.Skip(page * COUNT_SKIPPED_PER_PAGES)
                                                                    .OrderBy(i => i.ID)
                                                                    .Take(count)
                                                                    .ToListAsync();

            if (itineraries.Count == 0)
            {
                return NotFound();
            }
            else
            {
               return Ok(itineraries);
            }
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<Itinerary>>> GetByDate([FromQuery] int count, [FromQuery] int page, [FromQuery] DateOnly dateStart, [FromQuery] DateOnly? dateEnd)
        {
            if (!dateEnd.HasValue)
            {
                dateEnd = new DateOnly(dateStart.Year, dateStart.Month, DateTime.DaysInMonth(dateStart.Year, dateStart.Month));
            }
            List<Itinerary> itineraries = await _context.Itineraries.Where(i => i.Date >= dateStart && i.Date <= dateEnd)
                                                                    .OrderBy(i => i.ID)
                                                                    .Skip(page * COUNT_SKIPPED_PER_PAGES)
                                                                    .Take(count)
                                                                    .ToListAsync();

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
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(ExecutorDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        public async Task<ActionResult<OperationsOfItinerary[]>> GetOperationsOfItinerary(int id)
        {
            Itinerary? itinerary = await _context.Itineraries.FirstOrDefaultAsync(item => item.ID == id);

            //this.CheckNotFoundObject(itinerary, "У этой позиции плана маршрутных листов нет");

            if (itinerary != null)
            {
                Console.WriteLine(_context.Itineraries.ToArray() + "dsfdsf");

                var operations = itinerary.Operations.ToArray();
                return this.CheckNotFoundArray(operations, "У этого маршрутного листа операций нет"); ;
            }
            else
            {
                
                return NotFound(new APIError { Message = "Маршрутный лист не найден" });
            }
        }

        [HttpGet("by-plan-position/{planPositionId}")]
        public async Task<ActionResult<ItineraryDto[]>> GetItineraryByPlanPosition(int planPositionId)
        {
            ItineraryDto[] itinerary = await _context.Itineraries
                .Where(itiner => itiner.PositionPlanID == planPositionId)
                .Select(itiner => new ItineraryDto(itiner))
                .ToArrayAsync();

            return this.CheckNotFoundArray(itinerary, "У этой позиции плана маршрутных листов нет");

        }

        [HttpPost("by-plan-positions")]
        public async Task<ActionResult<ItineraryDto[]>> GetItineraryByPlanPositions(
            [FromBody] int[] planPositionId) 
        {
            ItineraryDto[] itinerary = await _context.Itineraries
                .Where(itiner => planPositionId.Contains(itiner.PositionPlanID)) 
                .Select(itiner => new ItineraryDto(itiner))
                .ToArrayAsync();

            return this.CheckNotFoundArray(itinerary, "У этой позиции плана маршрутных листов нет");
        }

    }
}
