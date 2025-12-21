using ItineraryOperations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;

namespace ItineraryOperations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainSubjectController : ControllerBase
    {

        private readonly PostgresContext _context;

        private readonly ILogger _logger;

        public MainSubjectController(ILogger<MainSubjectController> logger, PostgresContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MainSubject>>> Get()
        {
            //void FillingInOperationListTable()
            //{
            //    MainSubject m = new MainSubject
            //    {
            //        Name = "d",
            //        AUDCode = 2
            //    };

            //    _context.MainSubject.Add(m);
            //    _context.SaveChanges();
            //}
            //FillingInOperationListTable();
            var mainSubjects = await _context.MainSubject.Select(item => new 
            {
                item.AUDCode,
                item.Name
            }).ToListAsync();

            if (mainSubjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
               return Ok(mainSubjects);
            }
        }


        [HttpGet("{id}")]
        public ActionResult<MainSubject> Get(int id)
        {
            MainSubject? mainSubject = _context.MainSubject.FirstOrDefault(item => item.ID == id);

            if (mainSubject == null) {
                return NotFound();
            }
            else {
                return Ok(mainSubject);
            } 
        }
    }
}
