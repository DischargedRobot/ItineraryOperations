using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class ProductDto
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AUDCode { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public ProductDto(Products product)
        {
            ID = product.ID;
            Name = product.Name;
            AUDCode = product.AUDCode;
            DepartmentId = product.DivisionID;
        }

        public ProductDto()
        {

        }
    }
}
