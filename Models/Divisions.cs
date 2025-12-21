using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ItineraryOperations.Models
{
    public class Divisions
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Name { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.Divisions.ExecuteDelete();
            context.SaveChanges();

            List<Divisions> newDivisions = new List<Divisions>
            {
                new Divisions {ID = 342, Name = $"Цех 342" },
                new Divisions {ID = 382, Name = $"Цех 382" },
                new Divisions {ID = 178, Name = $"178" },
                new Divisions {ID = 186, Name = $"186" },
                new Divisions {ID = 115, Name = $"115" },
                new Divisions {ID = 144, Name = $"144" },
                new Divisions {ID = 175, Name = $"175" },
                new Divisions {ID = 194, Name = $"194" },
                new Divisions {ID = 381, Name = $"381" }
            };

            context.Divisions.AddRange(newDivisions);

            context.SaveChanges();
        }
    }
}
