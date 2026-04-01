using ItineraryOperations.Models.Executor;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ItineraryOperations.Models
{
    public class OperationsOfItineraryDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ItineraryId { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public double NormTime { get; set; }

        public int TypeId { get; set; }

        public int NumberPositions { get; set; }

        public int EquipmentId { get; set; }

        public int? ExecutorId { get; set; }

        [Required]
        public string Name { get; set; }

        [DefaultValue(1), Column(TypeName = "decimal(6,3)")]
        public float PaymentCoefficient { get; set; }

        [Column(TypeName = "decimal(7,3)")]
        public float Award { get; set; }

        public DateOnly DateIssue { get; set; }

        public DateOnly DateExecution { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public float TotalWithSurcharge { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public float RewardAmount { get; set; }

        public bool IsFormed { get; set; }

        [Required]
        public Products Product { get; set; }

        public OperationsOfItineraryDto(OperationsOfItinerary operationOfItinerary, Products product)
        {
            Product = product;
            Id = operationOfItinerary.ID;
            ItineraryId = operationOfItinerary.ItineraryID;
            DepartmentId = operationOfItinerary.DivisionID;
            CategoryId = operationOfItinerary.CategoryID;
            NormTime = operationOfItinerary.NormTime;
            TypeId = operationOfItinerary.TypeOperationID;
            NumberPositions = operationOfItinerary.NumberPositions;
            EquipmentId = operationOfItinerary.EquipmentID;
            ExecutorId = operationOfItinerary.ExecutorID;
            Name = operationOfItinerary.Name;
            PaymentCoefficient = operationOfItinerary.PaymentCoefficient;
            Award = operationOfItinerary.Reward;
            DateIssue = operationOfItinerary.DateIssue;
            DateExecution = operationOfItinerary.DateExecution;
            IsFormed = operationOfItinerary.isFormed;
        }

        public OperationsOfItineraryDto()
        {

        }
    }
}
