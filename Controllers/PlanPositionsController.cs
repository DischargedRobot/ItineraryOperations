using ItineraryOperations.Lib;
using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<PlanPosition>>> Get([FromQuery] int count = 100, [FromQuery] int page = 1)
        {
            List<PlanPosition> planPositions = await _context.PlanPositions.Skip((page-1) * COUNT_SKIPPED_PER_PAGES).Take(count).Select(item => new PlanPosition
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

            return this.CheckNotFoundObject(planPosition, "Объекта нет");
        }

        [HttpGet("by-product/{productID}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(PlanPositionDto[]))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError400Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        public async Task<ActionResult<PlanPositionDto[]>> GetPlanPositions(int productID)
        {

            PlanPositionDto[] planPositions = await _context.PlanPositions
                .Where(planPos => planPos.ProductID == productID)
                .Select(planPos => new PlanPositionDto(planPos))
                .ToArrayAsync();


            return this.CheckNotFoundArray(planPositions, "Позиций плана по этому издлию нет"); 

        }
    }
}
