using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class UserSession
    {
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users Users { get; set; }

        [Key]        
        
        public int Id { get; set; }
        [Required]

        public DateTime Finished { get; set; }

        public UserSession(Users user) {
            UserId = user.ID;
            Finished = DateTime.UtcNow.AddHours(4);
        }

        public UserSession() { }
    }
}
