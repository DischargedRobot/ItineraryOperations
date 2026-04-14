using System.ComponentModel.DataAnnotations;

namespace ItineraryOperations.Models
{
    public class Role
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Name { get; set; }

        // Предустановленные роли
        public static readonly int BrigadirId = 1;
        public static readonly int ExecutorId = 2;

        public static readonly string BrigadirName = "Бригадир";
        public static readonly string ExecutorName = "Исполнитель";
    }
}
