using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace ItineraryOperations.Models
{
    public class PlanPositionDto
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateOnly StartedDate { get; set; }
        
        [Required]
        public DateOnly FinishedDate { get; set; }
        [Required]
        public int ProductID { get; set; }

        public PlanPositionDto() { }
        public PlanPositionDto(PlanPosition planPosition)
        {
            ID = planPosition.ID;
            StartedDate = planPosition.StartedDate;
            FinishedDate = planPosition.FinishedDate;
            ProductID = planPosition.ProductID;
        }
    }
}
