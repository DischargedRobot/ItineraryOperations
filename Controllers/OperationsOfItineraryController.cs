using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItineraryOperations.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationsOfItinerary>>> Get()
        {
            List<OperationsOfItinerary> operations = await _context.OperationsOfItinerary.Select(item => new OperationsOfItinerary
            {
                ID = item.ID,
                ItineraryID = item.ItineraryID,
                DivisionID = item.DivisionID,
                CategoryID = item.CategoryID,
                NormTime = item.NormTime,
                TypeOperation = item.TypeOperation,
                NumberPositions = item.NumberPositions,
                EquipmentID = item.EquipmentID,
                Status = item.Status,
                ExecutorID = item.ExecutorID,
                Name = item.Name,
                PaymentCoefficient = item.PaymentCoefficient,
                Reward = item.Reward,
                DateIssue = item.DateIssue,
                DateExecution = item.DateExecution,
                TotalWithSurcharge = item.TotalWithSurcharge,
                RewardAmount = item.RewardAmount
            })
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
        public async Task<ActionResult<OperationsOfItinerary>> Get(int id)
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
