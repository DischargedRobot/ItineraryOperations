using ItineraryOperations.Models.Executor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class OperationsOfItinerary
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ItineraryID { get; set; }
        [ForeignKey("ItineraryID"), Required]
        public Itinerary? Itinerary { get; set; }

        [Required]
        public int DivisionID { get; set; }
        [ForeignKey("DivisionID"), Required]
        public Divisions? Division { get; set; }

        [Required]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID"), Required]
        public OperationCategories? OperationCategory { get; set; }

        [Required]
        public double NormTime { get; set; }

        public int TypeOperationID { get; set; }
        [ForeignKey("TypeOperationID"), Required]
        public TypesOperations? TypeOperation { get; set; }

        public int NumberPositions { get; set; }

        public int EquipmentID { get; set; }
        [ForeignKey("EquipmentID"), Required]
        public Equipment? Equipment { get; set; }

        public int Status { get; set; }

        public int? ExecutorID { get; set; }
        [ForeignKey("ExecutorID"), Required]
        public Executors? Executor { get; set; }
        [Required]
        public bool isFormed { get; set; } = false;

        public string Name { get; set; }

        [DefaultValue(1), Column(TypeName = "decimal(6,3)")]
        public float PaymentCoefficient { get; set; }

        [Column(TypeName = "decimal(7,3)")]
        public float Reward { get; set; }

        public DateOnly DateIssue { get; set; }

        public DateOnly DateExecution { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public float TotalWithSurcharge { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public float RewardAmount { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.OperationsOfItinerary.ExecuteDelete();
            context.SaveChanges();

            var itineraries = context.Itineraries
                                        .Include(i => i.PlanPosition)
                                            .ThenInclude(pp => pp.Product)
                                                .ThenInclude(p => p.Division)
                                        .ToList(); ;
            var categories = context.OperationCategories.ToList();
            var executors = context.Executors.ToList();
            var equipment = context.Equipment.Include(i => i.TypeOperations)
                                                .ToList();
            Random random = new Random();
            Console.WriteLine("start");
            // создаём 20 случайных операций
            Console.WriteLine(Enumerable.Range(1, 50).Select(i =>
            {
                int element = random.Next(itineraries.Count);
                int divisionID = itineraries[element].PlanPosition.Product.DivisionID;
                var equip = equipment[random.Next(equipment.Count)];
                var filteredExecutors = executors
                    .Where(e => e.DivisionID == divisionID)
                    .ToList();

                var randomExecutor = filteredExecutors.Any()
                    ? filteredExecutors[random.Next(filteredExecutors.Count)]
                    : null;


                var operation = new OperationsOfItinerary
                {
                    ItineraryID = itineraries[element].ID,
                    DivisionID = divisionID,
                    CategoryID = context.OperationCategories.Where(category => category.DivisionID == divisionID).First().ID,
                    NormTime = random.NextDouble(),
                    TypeOperationID = equip.TypeOperationID,
                    NumberPositions = random.Next(60),
                    EquipmentID = equip.ID,
                    Status = 0,
                    ExecutorID = randomExecutor?.ID,
                    Name = equip.TypeOperations?.Name == null ? "Имя по умолчанию" : equip.TypeOperations.Name,
                    PaymentCoefficient = (float)random.NextDouble() + 1,
                    Reward = (float)random.NextDouble(),
                    DateIssue = DateOnly.FromDateTime(DateTime.Now),
                    DateExecution = DateOnly.FromDateTime(DateTime.Now),
                    isFormed = random.Next(60) >= 30,
                };
                operation.TotalWithSurcharge = operation.CalculateTotalWithSurcharge(context);
                operation.RewardAmount = operation.CalculateRewardAmount(context);

                context.OperationsOfItinerary.Add(operation);
                return 1;
            }).ToList());
            context.SaveChanges();

            // Заполняем маршрутные листы только что добавленными операциями
            foreach (var itinerary in itineraries)
            {
                itinerary.Operations = context.OperationsOfItinerary.Where(oper => oper.ItineraryID == itinerary.ID).ToList();
            }
            context.SaveChanges();
        }

        public float CalculateRewardAmount(PostgresContext context)
        {

            return Reward * (float)NormTime * NumberPositions * (float)context.OperationCategories.First(category => category.ID == CategoryID).Payment;
        }

        public float CalculateTotalWithSurcharge(PostgresContext context)
        {
            return (float)NormTime * NumberPositions * (float)context.OperationCategories.First(category => category.ID == CategoryID).Payment * PaymentCoefficient;
        }
    }
}
