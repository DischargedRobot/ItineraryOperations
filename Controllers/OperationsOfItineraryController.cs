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
using ItineraryOperations.Lib;
using NuGet.Protocol.Plugins;
using System.Globalization;

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

        // TODO: в либу
        private async Task<ActionResult<OperationsOfItineraryDto[]>> GetOperationsForResponse(int itineraryID)
        {
            var itinerary = await _context.Itineraries
                    .FirstOrDefaultAsync(itiner => itiner.ID == itineraryID);

            if (itinerary == null)
            {
                return NotFound(new APIError { Message = "Маршрутный лист не найден" });
            }

            await _context.Entry(itinerary)
                .Reference(i => i.PlanPosition)
                .LoadAsync();

            if (itinerary.PlanPosition == null)
            {
                return NotFound(new APIError { Message = "Позиция плана не найдена для данного маршрутного листа" });
            }

            await _context.Entry(itinerary.PlanPosition)
                .Reference(pp => pp.Product)
                .LoadAsync();

            if (itinerary.PlanPosition.Product == null)
            {
                return NotFound(new APIError { Message = "Изделие не найдено для данной позиции плана" });
            }

            var productDto = new ProductDto(itinerary.PlanPosition.Product);

            var operations = await _context.OperationsOfItinerary
                .Where(op => op.ItineraryID == itineraryID)
                .ToArrayAsync();

            if (operations == null || operations.Length == 0)
            {
                return NotFound(new APIError { Message = "Операций нет" });
            }

            var opers = operations
                .Select(oper => new OperationsOfItineraryDto(oper, productDto))
                .ToArray();

            return this.CheckNotFoundArray(opers);
        }
        private async Task<ActionResult<OperationsOfItineraryDto>> GetOperationForResponse(int itineraryID)
        {
            var itinerary = await _context.Itineraries
                    .FirstOrDefaultAsync(itiner => itiner.ID == itineraryID);

            if (itinerary == null)
            {
                return NotFound(new APIError { Message = "Маршрутный лист не найден" });
            }

            await _context.Entry(itinerary)
                .Reference(i => i.PlanPosition)
                .LoadAsync();

            if (itinerary.PlanPosition == null)
            {
                return NotFound(new APIError { Message = "Позиция плана не найдена для данного маршрутного листа" });
            }

            await _context.Entry(itinerary.PlanPosition)
                .Reference(pp => pp.Product)
                .LoadAsync();

            if (itinerary.PlanPosition.Product == null)
            {
                return NotFound(new APIError { Message = "Изделие не найдено для данной позиции плана" });
            }

            var productDto = new ProductDto(itinerary.PlanPosition.Product);

            var operation = await _context.OperationsOfItinerary
                .FirstOrDefaultAsync(op => op.ItineraryID == itineraryID);

            if (operation == null)
            {
                return NotFound(new APIError { Message = "Операции нет" });
            }

            return this.CheckNotFoundObject(new OperationsOfItineraryDto(operation, productDto));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationsOfItineraryDto>>> Get()
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            Console.WriteLine("startsss");

            OperationsOfItinerary.Felling(_context);
            List<OperationsOfItineraryDto> operations = await _context.OperationsOfItinerary
                .Select(op => new OperationsOfItineraryDto
                (
                    op,
                    new ProductDto(op.Itinerary.PlanPosition.Product)
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
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(OperationsOfItineraryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError400Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        public async Task<ActionResult<OperationsOfItineraryDto[]>> GetByItinerary(int itineraryID)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            try
            {
                return await GetOperationsForResponse(itineraryID);
            }
            catch (Exception ex)
            {
                // Логируем полную ошибку для диагностики
                // _logger.LogError(ex, "Ошибка при получении операций для маршрутного листа {ItineraryID}", itineraryID);

                return StatusCode(500, new APIError { Message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        [HttpGet("by-executor/{executorID}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешно", typeof(OperationsOfItineraryDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Не найдено", typeof(APIError400Example))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIError404Example))]
        public async Task<ActionResult<IEnumerable<OperationsOfItineraryDto>>> GetByExecutor(int executorID)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            List<OperationsOfItineraryDto> operations = await _context.OperationsOfItinerary
                .Where(op => op.ExecutorID == executorID)
                .Select(op => new OperationsOfItineraryDto
                (
                    op,
                    new ProductDto(op.Itinerary.PlanPosition.Product)
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
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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

        [HttpPut("isFormed")]
        public async Task<ActionResult<OperationsOfItinerary>> SetIsFormed(
            [FromBody] int[] operationsIds
        )
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            var opertionsForChanges = await _context.OperationsOfItinerary
                .Where(op => operationsIds.Contains(op.ID))
                .ToListAsync();

            // проверка что все на месте
            var foundIds = opertionsForChanges.Select(o => o.ID).ToHashSet();
            var missingIds = operationsIds.Except(foundIds).ToList();

            if (missingIds.Any())
                return NotFound( new APIError { Message = $"Операции с ID {string.Join(", ", missingIds)} не найдены" });

            foreach ( var operation in opertionsForChanges )
            {
                operation.isFormed = true;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<OperationsOfItineraryDto>> Update(
     int id,
     [FromBody] OperationsOfItineraryDto dto)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            var entity = await _context.OperationsOfItinerary
                .FirstOrDefaultAsync(op => op.ID == id);

            if (entity == null)
            {
                return NotFound($"Операция с ID={id} не найдена");
            }

            // --- Обновляем поля явно ---

            entity.Name = dto.Name;
            entity.DivisionID = dto.DepartmentId;
            entity.CategoryID = dto.CategoryId;
            entity.NormTime = dto.NormTime;
            entity.TypeOperationID = dto.TypeId;
            entity.NumberPositions = dto.NumberPositions;

            entity.EquipmentID = dto.EquipmentId == 0 ? null : dto.EquipmentId;
            entity.ExecutorID = dto.ExecutorId == 0 ? null : dto.ExecutorId;

            entity.PaymentCoefficient = dto.PaymentCoefficient;
            entity.Reward = dto.Award;
            entity.isFormed = dto.IsFormed;
            // Конвертация строк в DateOnly
            if (!string.IsNullOrEmpty(dto.DateIssue))
                {
                entity.DateIssue = DateOnly.ParseExact(
                        dto.DateIssue,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(dto.DateExecution))
                {
                entity.DateExecution = DateOnly.ParseExact(
                        dto.DateExecution,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);
                }

            // Пересчитываем вычисляемые поля
            entity.TotalWithSurcharge = entity.CalculateTotalWithSurcharge(_context);
            entity.RewardAmount = entity.CalculateRewardAmount(_context);

                // Сохраняем изменения
                await _context.SaveChangesAsync();
            


            // ПОСЛЕ SaveChanges проверьте, существует ли запись
            var checkExists = await _context.OperationsOfItinerary
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ID == id);

                Console.WriteLine($"Запись существует после сохранения: {checkExists != null}");


                // Получаем обновлённый объект, чтобы удостовериться в изменения
                var updatedOperation = await _context.OperationsOfItinerary
                                                .FirstOrDefaultAsync(t => t.ID == id);
                if (updatedOperation == null)
                {
                    return StatusCode(500, new APIError { Message = $"Внутренняя ошибка сервера: Обновлённый объект не найден" });
                }
                else
                {
                    return await GetOperationForResponse(updatedOperation.ItineraryID);
                }


            }
        }
}
