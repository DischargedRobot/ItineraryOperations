
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
    [Route("api/[controller]")]
    [ApiController]
    public class TaskOrdersController : Controller
    {
        private readonly int COUNT_SKIPPED_PER_PAGES = 100;

        private readonly PostgresContext _context;

        private readonly ILogger<TaskOrdersController> _logger;

        public TaskOrdersController(ILogger<TaskOrdersController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskOrdersController>>> Get([FromQuery] int count, [FromQuery] int page)
        {
            List<TaskOrders> taskOrders = await _context.TaskOrders
                                                        .Skip(page * COUNT_SKIPPED_PER_PAGES)
                                                        .Take(count)
                                                        .Select(item => new TaskOrders
                                                        {
                                                            ID = item.ID,
                                                            DivisionID = item.DivisionID,
                                                            ExecutorID = item.ExecutorID,
                                                            Operations = item.Operations
                                                        }).ToListAsync();

            if (taskOrders.Count == 0)
            {
                return NotFound();
            }
            {
                return Ok();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskOrders>> GetById(int id)
        {

            TaskOrders? executor = await _context.TaskOrders.FirstOrDefaultAsync(item => item.ID == id);
            if (executor == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(executor);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskOrders>> Post([FromBody] TaskOrders taskOrder)
        {
            var divisionExists = await _context.Divisions.AnyAsync(d => d.ID == taskOrder.DivisionID);
            var executorExists = await _context.Executors.AnyAsync(e => e.ID == taskOrder.ExecutorID);

            if (!divisionExists || !executorExists)
            {
                return BadRequest("Связанные объекты не найдены");
            }
            else
            {
                _context.TaskOrders.Add(taskOrder);
                int result = await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = taskOrder.ID }, taskOrder);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskOrders>> Update(int id, [FromBody] TaskOrders newTaskOrder)
        {
            var existingTaskOrder = await _context.TaskOrders.FirstOrDefaultAsync(t => t.ID == id);
            if (existingTaskOrder == null)
            {
                return NotFound($"TaskOrder с ID={id} не найден");
            }

            existingTaskOrder.DivisionID = newTaskOrder.DivisionID;
            existingTaskOrder.ExecutorID = newTaskOrder.ExecutorID;
            existingTaskOrder.Operations = newTaskOrder.Operations;
            await _context.SaveChangesAsync();
            
            var updatedTask = await _context.TaskOrders
                                            .Include(t => t.Division)
                                            .Include(t => t.Executor)
                                            .FirstOrDefaultAsync(t => t.ID == id);

            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            TaskOrders? taskOrder = await _context.TaskOrders.FindAsync(id);
            if (taskOrder == null)
            {
                return NotFound();
            }
            else
            {
                _context.TaskOrders.Remove(taskOrder);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpGet("{id}/operations")]
        public async Task<ActionResult<TaskOrders>> GetOperationsById(int id)
        {
            TaskOrders? taskOrder = await _context.TaskOrders.FindAsync(id);
            if (taskOrder == null)
            {
                return NotFound();
            }
            else
            {
                IList<OperationsOfItinerary> operations = taskOrder.Operations;
                return Ok(operations);
            }
        }
    }
}
