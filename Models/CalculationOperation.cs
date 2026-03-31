using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace ItineraryOperations.Models
{
    public class CalculationOperation
    {

        public int ID { get; private set; }
        [Required(ErrorMessage = "Тариф обязателен")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Тариф должен быть больше нуля")]
        public double Tariff { get; set; }

        [Required(ErrorMessage = "Нормативное время обязательно")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Нормативное время должно быть больше нуля")]
        public double NormTime { get; set; }
        [Required(ErrorMessage = "Количество позиций обязательно")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество позиций должно быть больше 0")]
        public int NumberPositions { get; set; }
        [Required(ErrorMessage = "Коэффициент оплаты обязателен")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Коэффициент оплаты должен быть больше нуля")]
        public double PaymentCoefficient { get; set; }
        [Required(ErrorMessage = "Процент вознаграждения обязателен")]
        [Range(0, 100, ErrorMessage = "Процент вознаграждения должен быть от 0 до 100")]
        public float PercentageReward { get; set; }
        public double RewardAmount { get; private set; }
        public double TotalWithSurcharge { get; private set; }

        // Если по операции передали инфомрацию
        public CalculationOperation(int id, OperationsOfItinerary dto)
        {
            ID = id;
            Tariff = (double)dto.OperationCategory.Payment;
            NormTime = dto.NormTime;
            PaymentCoefficient = dto.PaymentCoefficient;
            PercentageReward = dto.Reward;
            NumberPositions = dto.NumberPositions;
        }

        // Если по дто передали данные об операции
        public CalculationOperation(int id, CalculationOperationDto dto)
        {
            ID = id;
            Tariff = dto.Tariff;
            NormTime = dto.NormTime;
            PaymentCoefficient = dto.PaymentCoefficient;
            PercentageReward = dto.PercentageReward;
            NumberPositions = dto.NumberPositions;
        }
        public CalculationOperation(OperationsOfItinerary dto)
        {
            Tariff = (double)dto.OperationCategory.Payment;
            NormTime = dto.NormTime;
            PaymentCoefficient = dto.PaymentCoefficient;
            PercentageReward = dto.Reward;
            NumberPositions = dto.NumberPositions;
        }

        public CalculationOperation Calculate()
        {
            var payment = Tariff * NormTime * NumberPositions;
            TotalWithSurcharge = payment * PaymentCoefficient;
            RewardAmount = payment * PercentageReward;
            return this;
        }

    }

    public class CalculationOperationDto
    {
        [Required(ErrorMessage = "Тариф обязателен")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Тариф должен быть больше нуля")]
        public double Tariff { get; set; }

        [Required(ErrorMessage = "Нормативное время обязательно")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Нормативное время должно быть больше нуля")]
        public double NormTime { get; set; }
        [Required(ErrorMessage = "Количество позиций обязательно")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество позиций должно быть больше 0")]
        public int NumberPositions { get; set; }
        [Required(ErrorMessage = "Коэффициент оплаты обязателен")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Коэффициент оплаты должен быть больше нуля")]
        public double PaymentCoefficient { get; set; }
        [Required(ErrorMessage = "Процент вознаграждения обязателен")]
        [Range(0, 100, ErrorMessage = "Процент вознаграждения должен быть от 0 до 100")]
        public float PercentageReward { get; set; }
    }

    public class CalculationOperationResultDto
    {
        public int ID { get; private set; }
        public double RewardAmount { get; set; }
        public double TotalWithSurcharge { get; set; }

        public double TotalWithReward { get; set; }

        public CalculationOperationResultDto(CalculationOperation calc) 
        { 
            ID = calc.ID;
            RewardAmount = calc.RewardAmount;
            TotalWithSurcharge = calc.TotalWithSurcharge;
            TotalWithReward = RewardAmount + TotalWithSurcharge;
        }
    }
}
