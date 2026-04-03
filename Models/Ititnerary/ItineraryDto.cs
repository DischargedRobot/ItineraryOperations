using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

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

        public IList<int> OperationsIds { get; set; } = new List<int>();

        [Required]
        public int NumberPositions { get; set; }

        [Required]
        public string KitIncreasingKit { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public int[] Route { get; set; }

        public ItineraryDto(Itinerary itinerary)
        {
            ID = itinerary.ID;
            PositionPlanID = itinerary.PositionPlanID;
            AUDCode = itinerary.AUDCode;
            MainSubject = itinerary.MainSubject;
            AUDName = itinerary.AUDName;
            OperationsIds = itinerary.Operations.Select(oper => oper.ID).ToList();
            NumberPositions = itinerary.NumberPositions;
            KitIncreasingKit = itinerary.KitIncreasingKit;
            Date = itinerary.Date;
            Route = Array.ConvertAll(Regex.Replace(itinerary.Route, @"[^\d,]", "").Split(","), int.Parse);
        }
        public ItineraryDto() { }
    }
}
