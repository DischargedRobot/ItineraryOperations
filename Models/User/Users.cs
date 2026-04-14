using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class Users
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string SecondName {  get; set; }

        [Required]
        public required string MiddleName { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Login {  get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        public static void Felling(PostgresContext context, int count = 10)
        {
            context.Users.ExecuteDelete();

            var random = new Random();
            var users = new List<Users>();

            var firstNames = new[] { "Иван", "Петр", "Алексей", "Дмитрий", "Сергей", "Андрей", "Максим", "Владимир", "Николай", "Александр" };
            var lastNames = new[] { "Иванов", "Петров", "Сидоров", "Смирнов", "Кузнецов", "Попов", "Васильев", "Соколов", "Михайлов", "Новиков" };
            var middleNames = new[] { "Иванович", "Петрович", "Алексеевич", "Дмитриевич", "Сергеевич", "Андреевич", "Максимович", "Владимирович", "Николаевич", "Александрович" };

            for (int i = 0; i < count; i++)
            {
                var user = new Users
                {
                    Name = firstNames[random.Next(firstNames.Length)],
                    SecondName = lastNames[random.Next(lastNames.Length)],
                    MiddleName = middleNames[random.Next(middleNames.Length)],
                    Login = $"user_{random.Next(1000, 9999)}",
                    Password = $"Pass{random.Next(1000, 9999)}!",
                    PhoneNumber = $"+7{random.Next(900, 999)}{random.Next(1000000, 9999999)}",
                    Email = $"user{random.Next(1000, 9999)}@example.com",   
                    Role = random.Next(2) == 0 ? new Role { ID = Role.BrigadirId, Name = Role.BrigadirName } : new Role { ID = Role.ExecutorId, Name = Role.ExecutorName }
                };

                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

    }
}
