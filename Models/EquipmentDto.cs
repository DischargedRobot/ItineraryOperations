using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace ItineraryOperations.Models
{
    public class EquipmentDto
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int OperationTypeId { get; set; }

        public EquipmentDto(Equipment equipment)
        {
            ID = equipment.ID;
            Name = equipment.Name;
            OperationTypeId = equipment.TypeOperationID;
            //
            // TODO: Add constructor logic here
            //
        }
    }
}

