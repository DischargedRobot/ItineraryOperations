using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItineraryOperations.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsOfItineraryController : Controller
    {
        private readonly PostgresContext _context;

        private readonly ILogger<OperationsOfItineraryController> _logger;

        public OperationsOfItineraryController(ILogger<OperationsOfItineraryController> logger, PostgresContext context)
        {
            _context = context;
            _logger = logger;
        }

    //    .Select(item => new OperationsOfItinerary
    //        {
    //            ID = item.ID,
    //            ItineraryID = item.ItineraryID,
    //            DivisionID = item.DivisionID,
    //            CategoryID = item.CategoryID,
    //            NormTime = item.NormTime,
    //            TypeOperation = item.TypeOperation,
    //            NumberPositions = item.NumberPositions,
    //            EquipmentID = item.EquipmentID,
    //            Status = item.Status,
    //            ExecutorID = item.ExecutorID,
    //            Name = item.Name,
    //            PaymentCoefficient = item.PaymentCoefficient,
    //            Reward = item.Reward,
    //            DateIssue = item.DateIssue,
    //            DateExecution = item.DateExecution,
    //            TotalWithSurcharge = item.TotalWithSurcharge,
    //            RewardAmount = item.RewardAmount
    //})

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationsOfItineraryDto>>> Get()
        {
            Console.WriteLine("startsss");

            OperationsOfItinerary.Felling(_context);
            List<OperationsOfItineraryDto> operations = await _context.OperationsOfItinerary
                .Select(op => new OperationsOfItineraryDto
                (
                    op,
                    op.Itinerary.PlanPosition.Product
                ))
                .ToListAsync();

            if (operations.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(operations);
            }
        }

        [HttpGet("by-itinerary/{itineraryID}")]
        public async Task<ActionResult<IEnumerable<OperationsOfItinerary>>> GetByItinerary(int itineraryID)
        {
            List<OperationsOfItinerary> operations = await _context.OperationsOfItinerary.Where(op => op.ItineraryID == itineraryID).ToListAsync();

            if (operations.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(operations);
            }
        }

        [HttpGet("by-executor/{executorID}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(OperationsOfItineraryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError400Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        public async Task<ActionResult<IEnumerable<OperationsOfItineraryDto>>> GetByExecutor(int executorID)
        {

            List<OperationsOfItineraryDto> operations = await _context.OperationsOfItinerary
                .Where(op => op.ExecutorID == executorID)
                .Select(op => new OperationsOfItineraryDto
                (
                    op,
                    op.Itinerary.PlanPosition.Product
                ))
                .ToListAsync();
            //.Select(operation =>
            //    _context.Itineraries.FirstOrDefault(itinerary =>
            //        itinerary.ID == operation.ItineraryID))
            //    .Select(itiner =>
            //        _context.PlanPositions.FirstOrDefault(planPosition =>
            //            planPosition.ID == itiner.PositionPlanID))
            //                .Select(plan => plan.ProductID).ToListAsync();

            if (operations.Count == 0)
            {
                return NotFound(new APIError { Message = "Операций у этого исполнителя нет"});
            }
            else
            {
                return Ok(operations);
            }
        }

        [HttpGet("by-division/{divisionID}")]
        public async Task<ActionResult<IEnumerable<OperationsOfItinerary>>> GetByDivision(int divisionID)
        {
            List<OperationsOfItinerary> operations = await _context.OperationsOfItinerary.Where(op => op.DivisionID == divisionID).ToListAsync();

            if (operations.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(operations);
            }
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<OperationsOfItinerary>>> GetByDate([FromQuery] int count, [FromQuery] int page, [FromQuery] DateOnly dateStart, [FromQuery] DateOnly? dateEnd)
        {
            if (!dateEnd.HasValue)
            {
                dateEnd = new DateOnly(dateStart.Year, dateStart.Month, DateTime.DaysInMonth(dateStart.Year, dateStart.Month));
            }
            List<OperationsOfItinerary> operations = await _context.OperationsOfItinerary.Where(i => i.DateIssue >= dateStart && i.DateIssue <= dateEnd)
                                                                    .ToListAsync();

            if (operations.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(operations);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationsOfItinerary>> GetByID(int id)
        {
            OperationsOfItinerary? operations = await _context.OperationsOfItinerary.FirstOrDefaultAsync(item => item.ID == id);
            if (operations == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(operations);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OperationsOfItinerary>> Update(int id, [FromBody] OperationsOfItinerary newOperationOfItinerary)
        {
            var existingOperationOfItinerary = await _context.TaskOrders.FirstOrDefaultAsync(t => t.ID == id);
            if (existingOperationOfItinerary == null)
            {
                return NotFound($"TaskOrder с ID={id} не найден");
            }

            //Получаем все не навигационные свойства
            var properties = newOperationOfItinerary.GetType()
                            .GetProperties()
                            .Where(prop => prop.PropertyType == typeof(int) || 
                                            prop.PropertyType == typeof(string) || 
                                            prop.PropertyType == typeof(float) || 
                                            prop.PropertyType == typeof(double) || 
                                            prop.PropertyType == typeof(DateOnly));
            foreach (var property in properties)
            {
                var newValue = property.GetValue(newOperationOfItinerary);
                property.SetValue(existingOperationOfItinerary, newValue);
            }
            
            await _context.SaveChangesAsync();

            // Получаем обновлённый объект, чтобы удостовериться в изменения
            var updatedTask = await _context.TaskOrders
                                            .Include(t => t.Division)
                                            .Include(t => t.Executor)
                                            .FirstOrDefaultAsync(t => t.ID == id);

            return Ok(updatedTask);
        }
    }
}
