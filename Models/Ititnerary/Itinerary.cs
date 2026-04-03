using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ItineraryOperations.Models
{
    public class Itinerary
    {
        [Key]
        public int ID { get; set; }

        public int PositionPlanID { get; set; }
        [ForeignKey("PositionPlanID"), Required]
        public PlanPosition PlanPosition { get; set; }
        
        [Required]
        public string AUDCode { get; set; }
        public MainSubject MainSubject { get; set; }

        [Required]
        public string AUDName { get; set; }

        public IList<OperationsOfItinerary> Operations { get; set; } = new List<OperationsOfItinerary>();

        [Required]
        public int NumberPositions { get; set; }

        [Required]
        public string KitIncreasingKit { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public string Route { get; set; }
        public static void Felling(PostgresContext context)
        {
            context.Itineraries.ExecuteDelete();
            context.SaveChanges();

            var planPositions = context.PlanPositions.ToList();
            var mainSubject = context.MainSubject.ToList();
            var random = new Random();
            var departments = context.Divisions.ToList();
            Enumerable.Range(0, mainSubject.Count-1).Select(i => 
            {
                var aud = mainSubject[random.Next(mainSubject.Count)];
                context.Itineraries.Add(new Itinerary
                {
                    PositionPlanID = planPositions[random.Next(planPositions.Count)].ID,
                    AUDCode = aud.AUDCode,
                    AUDName = aud.Name,
                    Operations = [],
                    NumberPositions = 30,
                    KitIncreasingKit = $"30/150",
                    Route = string.Join("-",departments.GetRange(random.Next(0, departments.Count - 4), 3).Select(dep => dep.Name))
                });
                return 1;
            }).ToList();

            context.SaveChanges();
        }
    }
}
