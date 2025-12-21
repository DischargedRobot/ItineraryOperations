using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
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
        public ActionResult<IEnumerable<Equipment>> Get()
        {
            return Ok(_context.Equipment.ToList());
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
