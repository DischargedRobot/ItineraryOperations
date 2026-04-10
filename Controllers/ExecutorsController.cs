using ItineraryOperations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using ItineraryOperations.Lib;

namespace ItineraryOperations.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<ExecutorDto>>> Get()
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            //Executors.Felling(_context);
            var executors = await _context.Executors
                .Select(executor => new ExecutorDto
                {
                    ID = executor.ID,
                    IsBrigade = executor.isBrigade,
                    Members = executor.Members,
                    DivisionID = executor.DivisionID,
                    OperationsIDs = _context.OperationsOfItinerary
                        .Where(operation => operation.ExecutorID == executor.ID)
                        .Select(oper => oper.ID)
                        .ToArray()
                })
                .ToArrayAsync();

            return Ok(executors);
        }

        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(ExecutorDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неверный запрос", typeof(APIError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIError400Example))]
        public async Task<ActionResult<ExecutorDto>> GetById(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            Executors? executor = await _context.Executors
                .FirstOrDefaultAsync(executor => executor.ID == id);

            if (executor == null)
            {
                return NotFound(new APIError { Message = "Объект не найден"});
            }
            else
            {
                return Ok(new ExecutorDto(executor));
            }
        }

        [HttpGet("by-division")]
        public async Task<ActionResult<IEnumerable<ExecutorDto>>> GetByDivisionID([FromQuery] int divisionID)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            ExecutorDto[] executors = await _context.Executors
                .Where(item => item.DivisionID == divisionID)
                .Select(executor => new ExecutorDto
                {
                    ID = executor.ID,
                    IsBrigade = executor.isBrigade,
                    Members = executor.Members,
                    DivisionID = executor.DivisionID,
                    OperationsIDs = _context.OperationsOfItinerary
                        .Where(operation => operation.ExecutorID == executor.ID)
                        .Select(oper => oper.ID)
                        .ToArray()
                })
                .ToArrayAsync();

            if (executors.Length == 0)
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
