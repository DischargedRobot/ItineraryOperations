using Microsoft.EntityFrameworkCore;

namespace ItineraryOperations.Models
{
    public class UsersDto
    {

        public  string Name { get; set; }
        public string SecondName {  get; set; }
        public  string MiddleName { get; set; }
        public  string Password { get; set; }
        public  string Login {  get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }



    public UsersDto(Users user) 
    {
        Name = user.Name;
        SecondName = user.SecondName;   
        MiddleName = user.MiddleName;
        Password = user.Password;
        Login = user.Login;
        PhoneNumber = user.PhoneNumber;
        Email = user.Email;
    }

    }
}
