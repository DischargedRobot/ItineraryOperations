using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItineraryOperations.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ItineraryOperations.Models.CalculationTaskOrder;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : Controller
    {
        private readonly PostgresContext _context;

        private readonly ILogger<CalculationsController> _logger;

        //TODO: МБ убрать список с результатами?
        public static List<CalculationOperationResultDto> calculationsOperationsResults = new List<CalculationOperationResultDto>();
        public static List<CalculationOperation> calculationOperations = new List<CalculationOperation>();
        public static int idCalculationOperation = 0;

        public static List<CalculationTaskOrderResultDto> calculationTaskOrdersResults = new List<CalculationTaskOrderResultDto>();
        public static List<CalculationTaskOrder> calculationTaskOrders = new List<CalculationTaskOrder>();
        public static int idCalculationTaskOrder = 0;


        public CalculationsController(ILogger<CalculationsController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<int> Get()
        {
            Users.Felling(_context);
            Divisions.Felling(_context);
            Executors.Felling(_context);
            Products.Felling(_context);
            PlanPosition.Felling(_context);
            OperationCategories.Felling(_context);
            Itinerary.Felling(_context);
            OperationsOfItinerary.Felling(_context);
            TaskOrders.Felling(_context);

            return Ok(1);
            //return Ok(await _context.Divisions.Select(item => new Divisions
            //{
            //    ID = item.ID,
            //    Name = item.Name
            //})
            //.ToArrayAsync());
        }

        [HttpPost("operation")]
        public IActionResult PostOperation([FromBody] CalculationOperationDto calculationOperation)
        {
            //На случай ошибки при валидации
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Ошибка валидации",
                    Details = ModelState
                });
            }

            //На случай внезапной ошибки
            try
            {
                int newId = Interlocked.Increment(ref idCalculationOperation);
                CalculationOperation calc = new CalculationOperation(newId, calculationOperation);

                CalculationOperationResultDto result = new CalculationOperationResultDto(calc.Calculate());
                calculationsOperationsResults.Add(result);
                calculationOperations.Add(calc);

                return CreatedAtAction(nameof(GetOperation), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Ошибка на сервере при произведении расчётов",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("operation/{id}")]
        public IActionResult PostOperation(int id)
        {
            //На случай внезапной ошибки
            try
            {
                int newId = Interlocked.Increment(ref idCalculationOperation);
                CalculationOperation calc = new CalculationOperation(newId,
                    _context.OperationsOfItinerary
                    .Include(cat => cat.OperationCategory)
                    .First(i => i.ID == id));

                CalculationOperationResultDto result = new CalculationOperationResultDto(calc.Calculate());
                calculationsOperationsResults.Add(result);
                calculationOperations.Add(calc);

                return CreatedAtAction(nameof(GetOperation), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Ошибка на сервере при произведении расчётов",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("operation/{id}")]
        public IActionResult GetOperation(int id)
        {

            var operation = calculationOperations.FirstOrDefault(oper => oper.ID == id);
            if (operation == null)
            {
                return NotFound(new
                {
                    Message = $"Вычисление с ID = {id} не найдено",
                });
            }

            return Ok(new CalculationOperationResultDto(operation));
        }

        [HttpDelete("operation/{id}")]
        public IActionResult DeleteOperation(int id)
        {

            var operation = calculationOperations.FirstOrDefault(oper => oper.ID == id);
            if (operation == null)
            {
                return NotFound(new
                {
                    Message = $"Вычисление с ID = {id} не найдено",
                });
            }
            calculationOperations.Remove(operation);
            return NoContent();
        }

        [HttpPost("taskOrder")]
        public IActionResult PostTaskOrder([FromBody] List<OperationsOfItinerary> operations)
        {
            //На случай ошибки при валидации
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Ошибка валидации",
                    Details = ModelState
                });
            }

            //На случай внезапной ошибки
            try
            {
                int newId = Interlocked.Increment(ref idCalculationTaskOrder);
                CalculationTaskOrder calc = new CalculationTaskOrder(newId, operations);

                CalculationTaskOrderResultDto result = new CalculationTaskOrderResultDto(calc.Calculate());
                calculationTaskOrdersResults.Add(result);
                calculationTaskOrders.Add(calc);
                

                return CreatedAtAction(nameof(GetTaskOrder), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Ошибка на сервере при произведении расчётов",
                    Error = ex
                });
            }
        }

        [HttpPost("taskOrder/{id}")]
        public async Task<IActionResult> PostTaskOrder(int id)
        {
            //На случай внезапной ошибки
            try
            {
                TaskOrders? task = await _context.TaskOrders.Include(i => i.Operations).ThenInclude(cat => cat.OperationCategory).FirstOrDefaultAsync(i => i.ID == id);
                if (task == null)
                {
                    return NotFound(new
                    {
                        Message = $"Наряд задание с ID = {id} не найден",
                    });
                }

                int newId = Interlocked.Increment(ref idCalculationTaskOrder);
                CalculationTaskOrder calc = new CalculationTaskOrder(newId, task.Operations.ToList());

                CalculationTaskOrderResultDto result = new CalculationTaskOrderResultDto(calc.Calculate());
                calculationTaskOrdersResults.Add(result);
                calculationTaskOrders.Add(calc);

                return CreatedAtAction(nameof(GetTaskOrder), new { id = result.ID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Ошибка на сервере при произведении расчётов",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("taskOrder/{id}")]
        public IActionResult GetTaskOrder(int id)
        {
            
            var taskOrder = calculationTaskOrders.FirstOrDefault(task => task.ID == id);
            if (taskOrder == null)
            {
                return NotFound(new
                {
                    Message = $"Вычисление с ID = {id} не найдено",
                });
            }

            return Ok(new CalculationTaskOrderResultDto(taskOrder));
        }

        [HttpDelete("taskOrder/{id}")]
        public IActionResult DeleteTaskOrder(int id)
        {

            var taskOrder = calculationTaskOrders.FirstOrDefault(task => task.ID == id);
            if (taskOrder == null)
            {
                return NotFound(new
                {
                    Message = $"Вычисление с ID = {id} не найдено",
                });
            }
            calculationTaskOrders.Remove(taskOrder);
            return NoContent();
        }
    }
}
