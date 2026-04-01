using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class Executors
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public bool isBrigade { get; set; }

        [Required]
        public required string[] Members { get; set; } = Array.Empty<string>();

        public int DivisionID { get; set; }
        [ForeignKey("DivisionID")]
        public Divisions? Divisions { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.Executors.ExecuteDelete();

            List<Executors> executors = new List<Executors>
            {
                new Executors { ID = 1, isBrigade = true, Members = new string[] { "Иванов И.В.", "Петров П.П.", "Сидоров С.С." }, DivisionID = 342},
                new Executors { ID = 2, isBrigade = false, Members = new string[] {"Морозов М.М." }, DivisionID = 382},
                new Executors { ID = 3, isBrigade = true, Members = new string[] { "Васильев А.В.", "Николаев Н.Н.", "Алексеев З.А.", "Григорьев Г.Г." }, DivisionID = 178 },
                new Executors { ID = 4, isBrigade = false, Members = new string[] { "Федоров Ф.Ф." }, DivisionID = 186},
                new Executors { ID = 5, isBrigade = true, Members = new string[] { "Попов П.П.", "Андреев А.А.", "Михайлов Ж.М." }, DivisionID = 115},
                new Executors { ID = 6, isBrigade = false, Members = new string[] { "Волков В.В." }, DivisionID = 144 },
                new Executors { ID = 7, isBrigade = true, Members = new string[] { "Зайцев З.З.", "Юрьев Ю.Ю.", "Романов Р.Р." }, DivisionID = 175 },
                new Executors { ID = 8, isBrigade = false, Members = new string[] { "Семенов К.С."}, DivisionID = 194 },
                new Executors { ID = 9, isBrigade = true, Members = new string[] { "Борисов Б.Б.", "Гусев Г.Г.", "Поляков Ч.П.", "Титов Т.Т." },  DivisionID = 186  },
                new Executors { ID = 10, isBrigade = false, Members = new string[] { "Смирнов С.Д." }, DivisionID = 342 },
                new Executors { ID = 11, isBrigade = true, Members = new string[] { "Новиков Р.Н.", "Орлов О.О.", "Степанов Ф.С." }, DivisionID = 382 },
                new Executors { ID = 12, isBrigade = false, Members = new string[] { "Устинов Л.У." }, DivisionID = 178 },
                new Executors { ID = 13, isBrigade = true, Members = new string[] { "Филиппов Ф.Т.", "Хромцов Х.Х.", "Цыпкин Ц.Ц." }, DivisionID = 186 },
                new Executors { ID = 14, isBrigade = false, Members = new string[] { "Щукин Щ.Щ." }, DivisionID = 115 },
                new Executors { ID = 15, isBrigade = true, Members = new string[] { "Эмиров М.Э.", "Юрьев Ю.Ю.", "Якимов Я.Я." }, DivisionID = 144 },
                new Executors { ID = 16, isBrigade = false, Members = new string[] { "Белов Б.Б." }, DivisionID = 175 },
                new Executors { ID = 17, isBrigade = true, Members = new string[] { "Горюнов Г.Г.", "Дроздов В.Д.", "Евсеев Е.Е.", "Журавлев И.Ж." }, DivisionID = 194 },
                new Executors { ID = 18, isBrigade = false, Members = new string[] { "Козлов К.К." }, DivisionID = 381 },
                new Executors { ID = 19, isBrigade = true, Members = new string[] { "Лебедев Л.Л.", "Макаров М.М.", "Новиков Н.Н." }, DivisionID = 342},
                new Executors { ID = 20, isBrigade = false, Members = new string[] { "Романов Н.Р." }, DivisionID = 382 }
            };

            context.AddRange(executors);
            context.SaveChanges();
        }
    }
}
