using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace ItineraryOperations.Models
{
    public class OperationCategories
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required, Column(TypeName = "decimal(8,4)")]
        public decimal Payment { get; set; }

        public int DivisionID { get; set; }
        [ForeignKey("DivisionID"), Required]
        public Divisions? Division { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.OperationCategories.ExecuteDelete();
            context.SaveChanges();

            var divisions = context.Divisions.ToList();

            var rand = new Random();
            //Генерируем 20*6 категорий с рандомной оплатой и привязкой к подразделениям 
            Enumerable.Range(65, 20)
                    .Select(i => Enumerable.Range(1, 6).Select(item => context.OperationCategories.Add(new OperationCategories
                    {
                        Name = $"{(char)i}{item}",
                        Payment = (decimal)rand.NextDouble() * (30.00M - 18.00M) + 18.00M,
                        DivisionID = divisions[rand.Next(divisions.Count)].ID,
                    })).ToList()).ToList();

            //List<OperationCategories> categories = new List<OperationCategories>
            //{
            //    new() {Name = "A3", Payment = 18.40M, DivisionID = 342},
            //    new() {Name = "A5", Payment = 19.90M, DivisionID = 115},
            //    new() {Name = "a6", Payment = 23.40M, DivisionID = 382},
            //    new() {Name = "b2", Payment = 15.30M, DivisionID = 144},
            //    new() {Name = "b3", Payment = 16.80M, DivisionID = 194},
            //    new() {Name = "B4", Payment = 18.40M, DivisionID = 175},
            //    new() {Name = "b5", Payment = 19.90M, DivisionID = 382},
            //    new() {Name = "b6", Payment = 20.00M, DivisionID = 381},
            //    new() {Name = "P4", Payment = 24.00M, DivisionID = 186},
            //    new() {Name = "P6", Payment = 24.00M, DivisionID = 381},
            //    new() {Name = "T4", Payment = 23.50M, DivisionID = 178},
            //    new() {Name = "T5", Payment = 19.10M, DivisionID = 144}
            //};

            context.SaveChanges();
        }

    }
}
