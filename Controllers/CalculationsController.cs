using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ItineraryOperations.Models;
using ItineraryOperations.Lib;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ItineraryOperations.Models.CalculationTaskOrder;
using ClosedXML.Excel;
using Swashbuckle.AspNetCore.Annotations;

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
        public async Task<ActionResult<int>> Get()
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            _logger.LogInformation("Тестовый лог - метод Get вызван в {Time}", DateTime.Now);
            
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
        }

        [HttpPost("operation")]
        public async Task<IActionResult> PostOperation([FromBody] CalculationOperationDto calculationOperation)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
        public async Task<IActionResult> PostOperation(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
        public async Task<IActionResult> GetOperation(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
        public async Task<IActionResult> DeleteOperation(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
        public async Task<IActionResult> PostTaskOrder([FromBody] List<OperationsOfItinerary> operations)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
        public async Task<IActionResult> GetTaskOrder(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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
        public async Task<IActionResult> DeleteTaskOrder(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

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

        /// <summary>
        /// Генерирует Excel файл с расчетами по исполнителям, изделиям и операциям
        /// </summary>
        /// <param name="request">Запрос с данными исполнителей, изделий и операций</param>
        /// <returns>Excel файл для скачивания</returns>
        /// <response code="200">Возвращает Excel файл</response>
        /// <response code="400">Ошибка валидации входных данных</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("excel")]
        [SwaggerResponse(StatusCodes.Status200OK, "Excel файл успешно сгенерирован", typeof(FileResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка валидации входных данных", typeof(APIError400Example))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера при генерации файла", typeof(APIError500Example))]
        public async Task<IActionResult> GenerateExcel([FromBody] ExcelGenerationRequest request)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            _logger.LogInformation("Начало генерации Excel файла. Количество исполнителей: {ExecutorCount}", 
                request.Executors?.Count ?? 0);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Ошибка валидации при генерации Excel: {ValidationErrors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                
                return BadRequest(new
                {
                    Message = "Ошибка валидации",
                    Details = ModelState
                });
            }

            try
            {
                _logger.LogInformation("Создание Excel workbook");
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Расчеты");

                int currentRow = 1;

                // Заголовок документа
                var orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                _logger.LogInformation("Генерация номера заказа: {OrderNumber}", orderNumber);
                
                worksheet.WriteOrderHeader(currentRow, orderNumber, DateTime.Now);
                currentRow += 2;

                int executorIndex = 0;
                foreach (var executorRequest in request.Executors)
                {
                    executorIndex++;
                    _logger.LogInformation("Обработка исполнителя {ExecutorIndex}/{TotalExecutors}, ID: {ExecutorId}", 
                        executorIndex, request.Executors.Count, executorRequest.Id);

                    // Получаем данные исполнителя
                    var executor = await _context.Executors
                        .Include(e => e.Divisions)
                        .FirstOrDefaultAsync(e => e.ID == executorRequest.Id);

                    if (executor == null) 
                    {
                        _logger.LogWarning("Исполнитель с ID {ExecutorId} не найден, пропускаем", executorRequest.Id);
                        continue;
                    }

                    _logger.LogInformation("Найден исполнитель: {ExecutorName}, Подразделение: {Department}", 
                        string.Join(", ", executor.Members), executor.Divisions?.Name ?? "Не указано");

                    // Заголовок исполнителя
                    var executorInfo = new ExcelExecutorInfo
                    {
                        Name = string.Join(", ", executor.Members),
                        Department = executor.Divisions?.Name ?? "Не указано"
                    };
                    executorInfo.WriteToExcel(worksheet, currentRow);
                    currentRow += 2;

                    // Заголовки таблицы
                    worksheet.WriteTableHeaders(currentRow);
                    currentRow++;

                    var executorResult = new ExcelExecutorResult();
                    int productIndex = 0;

                    foreach (var productRequest in executorRequest.Products)
                    {
                        productIndex++;
                        _logger.LogInformation("Обработка изделия {ProductIndex}/{TotalProducts} для исполнителя {ExecutorId}, ID изделия: {ProductId}", 
                            productIndex, executorRequest.Products.Count, executorRequest.Id, productRequest.Id);

                        // Получаем данные изделия
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == productRequest.Id);
                        if (product == null) 
                        {
                            _logger.LogWarning("Изделие с ID {ProductId} не найдено, пропускаем", productRequest.Id);
                            continue;
                        }

                        _logger.LogInformation("Найдено изделие: {ProductName}, AUD: {AUDCode}", 
                            product.Name, product.AUDCode);

                        // Заголовок изделия
                        var productDetail = new ExcelProductDetail
                        {
                            ProductName = product.Name
                        };
                        productDetail.WriteToExcel(worksheet, currentRow);
                        currentRow++;

                        var productResult = new ExcelTaskResult();
                        int itineraryIndex = 0;

                        foreach (var itineraryRequest in productRequest.Itineraries)
                        {
                            itineraryIndex++;
                            _logger.LogInformation("Обработка маршрута {ItineraryIndex}/{TotalItineraries} для изделия {ProductId}, ID маршрута: {ItineraryId}", 
                                itineraryIndex, productRequest.Itineraries.Count, productRequest.Id, itineraryRequest.Id);

                        // Получаем данные маршрута
                        var itinerary = await _context.Itineraries
                            .Include(i => i.PlanPosition)
                                .ThenInclude(pp => pp.Product)
                            .FirstOrDefaultAsync(i => i.ID == itineraryRequest.Id);

                        if (itinerary?.PlanPosition?.Product == null) 
                        {
                            _logger.LogWarning("Маршрут с ID {ItineraryId} не найден или не имеет изделия, пропускаем", itineraryRequest.Id);
                            continue;
                        }

                        _logger.LogInformation("Найден маршрут: {ItineraryName}, Изделие: {ProductName}", 
                            itinerary.AUDName, itinerary.PlanPosition.Product);

                        // Заголовок маршрута (ДСЕ)
                        var excelItinerary = new ExcelItinerary
                        {
                            AUDName = itinerary.AUDName,
                            AUDCode = itinerary.AUDCode ?? "",
                            KitIncreasingKit = itinerary.KitIncreasingKit ?? ""
                        };
                        excelItinerary.WriteToExcel(worksheet, currentRow);
                        currentRow++;

                        var itineraryResult = new ExcelTaskResult();
                        int operationIndex = 0;

                        foreach (var operationRequest in itineraryRequest.Operations)
                        {
                            operationIndex++;
                            _logger.LogInformation("Обработка операции {OperationIndex}/{TotalOperations} для маршрута {ItineraryId}, ID операции: {OperationId}", 
                                operationIndex, itineraryRequest.Operations.Count, itineraryRequest.Id, operationRequest.Id);

                            // Получаем данные операции
                            var operation = await _context.OperationsOfItinerary
                                .Include(o => o.OperationCategory)
                                .Include(o => o.TypeOperation)
                                .Include(o => o.Itinerary)
                                    .ThenInclude(i => i.PlanPosition)
                                        .ThenInclude(pp => pp.Product)
                                .FirstOrDefaultAsync(o => o.ID == operationRequest.Id);

                            if (operation?.OperationCategory == null) 
                            {
                                _logger.LogWarning("Операция с ID {OperationId} не найдена или не имеет категории, пропускаем", operationRequest.Id);
                                continue;
                            }

                            _logger.LogInformation("Найдена операция: {OperationName}, Тип: {OperationType}, Норма времени: {NormTime}", 
                                operation.Name, operation.TypeOperation?.Name, operation.NormTime);

                            // Создаем расчет операции
                            int calcId = Interlocked.Increment(ref idCalculationOperation);
                            var calc = new CalculationOperation(calcId, operation!);
                            var calcResult = calc.Calculate();

                            _logger.LogInformation("Расчет операции {OperationId} завершен. Итого с доплатой: {TotalWithSurcharge}, Премия: {RewardAmount}", 
                                operationRequest.Id, calcResult.TotalWithSurcharge, calcResult.RewardAmount);

                            // Создаем строку для Excel
                            var excelRow = new ExcelOperationRow
                            {
                                CPC = product.CPC ?? "",
                                OperationTypeName = operation.TypeOperation?.Name ?? "",
                                IssueDate = operation.DateIssue.ToDateTime(TimeOnly.MinValue),
                                ExecutionDate = operation.DateExecution.ToDateTime(TimeOnly.MinValue),
                                Tariff = (decimal)calcResult.Tariff,
                                Payment = (decimal)(calcResult.Tariff * calcResult.NormTime * calcResult.NumberPositions),
                                NT = (decimal)calcResult.NormTime,
                                NTonPayment = (decimal)(calcResult.Tariff * calcResult.NormTime),
                                Kit = calcResult.NumberPositions,
                                KitOnNTPayment = (decimal)(calcResult.Tariff * calcResult.NormTime * calcResult.NumberPositions),
                                Coefficient = (decimal)calcResult.PaymentCoefficient,
                                TotalSum = (decimal)calcResult.TotalWithSurcharge,
                                PremiumPercent = (decimal)calcResult.PercentageReward,
                                Premium = (decimal)calcResult.RewardAmount,
                                ProductId = product.ID
                            };

                            excelRow.WriteToExcel(worksheet, currentRow);
                            currentRow++;

                            // Накапливаем результаты
                            itineraryResult.NT += (decimal)calcResult.NormTime;
                            itineraryResult.NTonPayment += (decimal)(calcResult.Tariff * calcResult.NormTime);
                            itineraryResult.KitOnNTPayment += (decimal)(calcResult.Tariff * calcResult.NormTime * calcResult.NumberPositions);
                            itineraryResult.TotalSum += (decimal)calcResult.TotalWithSurcharge;
                            itineraryResult.Premium += (decimal)calcResult.RewardAmount;
                            itineraryResult.TotalSumWithPremium += (decimal)(calcResult.TotalWithSurcharge + calcResult.RewardAmount);
                        }

                        // Итоги по маршруту
                        if (itineraryRequest.Operations.Any())
                        {
                            _logger.LogInformation("Записываем итоги по маршруту {ItineraryId}. Общая сумма: {TotalSum}, Премия: {Premium}", 
                                itineraryRequest.Id, itineraryResult.TotalSum, itineraryResult.Premium);
                            
                            itineraryResult.WriteToExcel(worksheet, currentRow);
                            currentRow += 2;

                            // Накапливаем в результат изделия
                            productResult.NT += itineraryResult.NT;
                            productResult.NTonPayment += itineraryResult.NTonPayment;
                            productResult.KitOnNTPayment += itineraryResult.KitOnNTPayment;
                            productResult.TotalSum += itineraryResult.TotalSum;
                            productResult.Premium += itineraryResult.Premium;
                            productResult.TotalSumWithPremium += itineraryResult.TotalSumWithPremium;
                        }
                    }

                    // Итоги по изделию
                    if (productRequest.Itineraries.Any())
                    {
                        _logger.LogInformation("Записываем итоги по изделию {ProductId}. Общая сумма: {TotalSum}, Премия: {Premium}", 
                            productRequest.Id, productResult.TotalSum, productResult.Premium);
                        
                        productResult.WriteToExcel(worksheet, currentRow);
                        currentRow += 2;

                        // Накапливаем в результат исполнителя
                        executorResult.NT += productResult.NT;
                        executorResult.NTonPayment += productResult.NTonPayment;
                        executorResult.TotalSum += productResult.TotalSum;
                        executorResult.Premium += productResult.Premium;
                        executorResult.TotalSumWithPremium += productResult.TotalSumWithPremium;
                    }
                }

                // Итоги по исполнителю
                if (executorRequest.Products.Any())
                    {
                        _logger.LogInformation("Записываем итоги по исполнителю {ExecutorId}. Общая сумма: {TotalSum}, Премия: {Premium}", 
                            executorRequest.Id, executorResult.TotalSum, executorResult.Premium);
                        
                        executorResult.WriteToExcel(worksheet, currentRow);
                        currentRow += 3;
                    }
                }

                _logger.LogInformation("Настройка форматирования Excel файла");
                // Настройка ширины колонок
                worksheet.Columns().AdjustToContents();

                // Создаем поток для файла
                _logger.LogInformation("Сохранение Excel файла в поток");
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"Расчеты_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                _logger.LogInformation("Excel файл успешно сгенерирован. Имя файла: {FileName}, Размер: {FileSize} байт", 
                    fileName, stream.Length);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при генерации Excel файла: {ErrorMessage}", ex.Message);
                
                return StatusCode(500, new
                {
                    Message = "Ошибка при генерации Excel файла",
                    Error = ex.Message
                });
            }
        }
    }

    /// <summary>
    /// Запрос для генерации Excel файла с расчетами
    /// </summary>
    public class ExcelGenerationRequest
    {
        /// <summary>
        /// Список исполнителей с их изделиями и операциями
        /// </summary>
        public List<ExecutorRequest> Executors { get; set; } = new();
    }

    /// <summary>
    /// Данные исполнителя для Excel отчета
    /// </summary>
    public class ExecutorRequest
    {
        /// <summary>
        /// ID исполнителя из базы данных
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <summary>
        /// Список изделий исполнителя
        /// </summary>
        public List<ProductRequest> Products { get; set; } = new();
    }

    /// <summary>
    /// Данные изделия для Excel отчета
    /// </summary>
    public class ProductRequest
    {
        /// <summary>
        /// ID изделия из базы данных
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <summary>
        /// Список маршрутов (ДСЕ) для изделия
        /// </summary>
        public List<ItineraryRequest> Itineraries { get; set; } = new();
    }

    /// <summary>
    /// Данные маршрута (ДСЕ) для Excel отчета
    /// </summary>
    public class ItineraryRequest
    {
        /// <summary>
        /// ID маршрута из базы данных
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <summary>
        /// Список операций по маршруту
        /// </summary>
        public List<OperationRequest> Operations { get; set; } = new();
    }

    /// <summary>
    /// Данные операции для Excel отчета
    /// </summary>
    public class OperationRequest
    {
        /// <summary>
        /// ID операции из базы данных
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
    }
}
