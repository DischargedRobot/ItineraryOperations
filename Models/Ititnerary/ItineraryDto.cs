using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ItineraryOperations.Models
{
    public class ItineraryDto
    {
        [Required]
        public int ID { get; set; }
        [Required]

        public int PositionPlanID { get; set; }
        
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
    }
}
