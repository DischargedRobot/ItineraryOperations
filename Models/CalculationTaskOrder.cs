namespace ItineraryOperations.Models
{
    public class CalculationTaskOrder
    {
        public int ID { get; private set; }
        public List<OperationsOfItinerary> OperationsOfItinerary { get; set; }

        public double TotalAmountReward { get; set; }
        public double TotalWithSurcharge { get; set; }

        public double TotalWithReward { get; set; }

        public CalculationTaskOrder(int id, List<OperationsOfItinerary> operations) 
        {
            ID = id;
            OperationsOfItinerary = operations;
        }


        public CalculationTaskOrder Calculate() 
        {
            var operations = Enumerable.Range(0, OperationsOfItinerary.Count)
                                                .Select(operation => new CalculationOperation(OperationsOfItinerary[operation]));
            var results = operations.Select(operation => new CalculationOperationResultDto(operation.Calculate()));
            TotalAmountReward = results.Select(result => result.RewardAmount).Sum();
            TotalWithSurcharge = results.Select(result => result.TotalWithSurcharge).Sum();
            TotalWithReward = TotalWithSurcharge + TotalAmountReward;

            return this;
        }

        public class CalculationTaskOrderResultDto
        {
            public int ID { get; private set; }
            public double TotalAmountReward { get; set; }
            public double TotalWithSurcharge { get; set; }
            public double TotalWithReward { get; set; }

            public CalculationTaskOrderResultDto(CalculationTaskOrder calc)
            {
                ID = calc.ID;
                TotalAmountReward = calc.TotalAmountReward;
                TotalWithSurcharge = calc.TotalWithSurcharge;
                TotalWithReward = calc.TotalWithReward;
            }
        }

    }
}
