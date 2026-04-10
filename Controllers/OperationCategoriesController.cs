using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;
using ItineraryOperations.Lib;

namespace ItineraryOperations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationCategoriesController : ControllerBase
    {

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public OperationCategoriesController(ILogger<OperationCategoriesController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationCategories>>> Get()
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            List<OperationCategories> mainSubjects =  await _context.OperationCategories.Select(item => new OperationCategories
            {
                ID = item.ID,
                Name = item.Name,
                Payment = item.Payment,
                DivisionID = item.DivisionID
            }).ToListAsync();

            if (mainSubjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(mainSubjects);
            }
            //return Ok(_context.OperationCategories.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationCategories>> Get(int id)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            OperationCategories? operationCategory = _context.OperationCategories.FirstOrDefault(item => item.ID == id);

            if (operationCategory == null) {
                return NotFound();
            }
            else {
                return Ok(operationCategory);
            } 
        }

        [HttpGet("/by-AUDCode")]
        public async Task<ActionResult<Products>> GetByAUDCode([FromQuery] string AUDCode)
        {
            bool sessionIsActive = await CheckSessionFunctions.CheckSession(Request, _context);
            if (!sessionIsActive)
            {
                return Unauthorized(new APIError { Message = "Сессия недействительна" });
            }

            Products? product = _context.Products.FirstOrDefault(item => item.AUDCode == AUDCode);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }
    }
}
