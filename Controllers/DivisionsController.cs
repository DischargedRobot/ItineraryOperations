using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItineraryOperations.Models;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
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
        public async Task<ActionResult<IEnumerable<Divisions>>> Get()
        {

            //Divisions.Felling(_context);
            return Ok(await _context.Divisions.Select(item => new Divisions
            {
                ID = item.ID,
                Name = item.Name
            })
            .ToArrayAsync());
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
