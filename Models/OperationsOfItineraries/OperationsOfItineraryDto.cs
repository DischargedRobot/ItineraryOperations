using ItineraryOperations.Models.Executor;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using System;

namespace ItineraryOperations.Models
{
    public class OperationsOfItineraryDto
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public int ItineraryID { get; set; }

        [Required]
        public int DivisionID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        public double NormTime { get; set; }

        public int TypeOperationID { get; set; }

        public int NumberPositions { get; set; }

        public int EquipmentID { get; set; }

        public int Status { get; set; }

        public int? ExecutorID { get; set; }

        public string Name { get; set; }

        [DefaultValue(1), Column(TypeName = "decimal(6,3)")]
        public float PaymentCoefficient { get; set; }

        [Column(TypeName = "decimal(7,3)")]
        public float Reward { get; set; }

        public DateOnly DateIssue { get; set; }

        public DateOnly DateExecution { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public float TotalWithSurcharge { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public float RewardAmount { get; set; }

        public OperationsOfItineraryDto(OperationsOfItinerary operationOfItinerary)
        {
            ItineraryID = operationOfItinerary.ItineraryID;
            DivisionID = operationOfItinerary.DivisionID;
            CategoryID = operationOfItinerary.CategoryID;
            NormTime = operationOfItinerary.NormTime;
            TypeOperationID = operationOfItinerary.TypeOperationID;
            NumberPositions = operationOfItinerary.NumberPositions;
            EquipmentID = operationOfItinerary.EquipmentID;
            Status = operationOfItinerary.Status;
            ExecutorID = operationOfItinerary.ExecutorID;
            Name = operationOfItinerary.Name;
            PaymentCoefficient = operationOfItinerary.PaymentCoefficient;
            Reward = operationOfItinerary.Reward;
            DateIssue = operationOfItinerary.DateIssue;
            DateExecution = operationOfItinerary.DateExecution;
        }

        public OperationsOfItineraryDto()
        {

        }
    }
}
