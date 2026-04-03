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
    public class EquipmentController : ControllerBase
    {
        private readonly ILogger<EquipmentController> _logger;

        private readonly PostgresContext _context;

        public EquipmentController(ILogger<EquipmentController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(OperationsOfItineraryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError404Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Не Авторизован", typeof(APIError401Example))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(APIError401Example))]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> Get()
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (sessionIsActive)
            {
                return Ok(await _context.Equipment
                .AsNoTracking()
                .Select(equipment => new EquipmentDto(equipment))
                .ToListAsync()
                );
            }
            return Unauthorized( new APIError { Message= "Сессия недействительна"});
            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> Get(int id)
        {
            Equipment? equipment = await _context.Equipment.FirstOrDefaultAsync(item => item.ID == id);

            if (equipment == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(equipment);
            }
        }
    }
}
