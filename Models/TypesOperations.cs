using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ItineraryOperations.Models
{
    public class TypesOperations
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.TypesOperations.ExecuteDelete();

            List<TypesOperations> typesOperations = new List<TypesOperations> { 
                new() { Name = "адс"},
                new() { Name = "вальцовка"},
                new() { Name = "вырубка"},
                new() { Name = "Вырубная 2000"},
                new() { Name = "Вырубная 5000"},
                new() { Name = "Вырубная 6000"},
                new() { Name = "вязальная"},
                new() { Name = "гальваника"},
                new() { Name = "гибка"},
                new() { Name = "гидровырубка"},
                new() { Name = "гравировальная"},
                new() { Name = "Дефектировочная"},
                new() { Name = "заготовительная"},
                new() { Name = "заготовительная ст"}
            };

            context.TypesOperations.AddRange(typesOperations);
            context.SaveChanges();
        }
    }
}