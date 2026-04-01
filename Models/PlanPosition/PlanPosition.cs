using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace ItineraryOperations.Models
{
    public class PlanPosition
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateOnly StartedDate { get; set; }
        
        [Required]
        public DateOnly FinishedDate { get; set; }

        public int ProductID { get; set; }
        [ForeignKey("ProductID"), Required]
        public Products Product { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.PlanPositions.ExecuteDelete();
            context.SaveChanges();

            List<Products> products = context.Products.ToList();
            Enumerable.Range(0, products.Count).Select(number => context.PlanPositions.Add(new PlanPosition { ProductID = products[number].ID , StartedDate = DateOnly.FromDateTime(DateTime.Now), FinishedDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)) })).ToList();

            context.SaveChanges();
        }
    }
}
