using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace ItineraryOperations.Models
{
    public class TaskOrders
    {
        [Key]
        public int ID { get; set; }

        public int DivisionID { get; set; }
        [ForeignKey("DivisionID")]
        public Divisions? Division { get; set; }

        public int ExecutorID { get; set; }
        [ForeignKey("ExecutorID")]
        public Executors? Executor { get; set; }

        public IList<OperationsOfItinerary> Operations { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.TaskOrders.ExecuteDelete();
            context.SaveChanges();

            Random random = new Random();
            var operations = context.OperationsOfItinerary.ToList();
            //5 случайных нарядов
            Enumerable.Range(1, 5).Select(i => 
            {
                int element = random.Next(operations.Count);
                var operation = operations[element];
                if (operation?.ExecutorID is int executorId)
                {
                    context.TaskOrders.Add(new TaskOrders
                    {
                        DivisionID = operation.DivisionID,
                        ExecutorID = executorId,
                        Operations = new List<OperationsOfItinerary>() { operation }
                    });
                } else
                {
                    return 1;
                }
                
                operations.RemoveAt(element);
                return 1;
            }).ToList();

            context.SaveChanges();
        }
        
    }
}
