using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItineraryOperations.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using ItineraryOperations.Lib;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : Controller
    {
        private readonly PostgresContext _context;

        private readonly ILogger<DivisionsController> _logger;

        public DivisionsController(ILogger<DivisionsController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(OperationsOfItineraryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError404Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Не Авторизован", typeof(APIError401Example))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(APIError401Example))]
        public async Task<ActionResult<IEnumerable<Divisions>>> Get()
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (sessionIsActive)
            {
                return Ok(await _context.Divisions.Select(item => new Divisions
                {
                    ID = item.ID,
                    Name = item.Name
                })
                .ToArrayAsync());
            }
            return Unauthorized(new APIError { Message = "Сессия недействительна" });

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Divisions>> Get(int id)
        {
            Divisions? executor = await _context.Divisions.FirstOrDefaultAsync(item => item.ID == id);
            if (executor == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(executor);
            }
        }
    }
}
