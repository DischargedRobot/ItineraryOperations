using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ItineraryOperations.Models
{
    public class MainSubject
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AUDCode { get; set; }

        public static void Felling(PostgresContext context)
        {
            context.MainSubject.ExecuteDelete();
            context.SaveChanges();

            List<MainSubject> subjects = new List<MainSubject> {
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ С ВОЗМОЖНОСТЬЮ ВЫБОРА РЕЖИМОВ", AUDCode="КИУС94252711001-01" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ С ВОЗМОЖНОСТЬЮ ВЫБОРА РЕЖИМОВ", AUDCode="КИУС94252711001-02" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКА-120-ПЗ", AUDCode="КИУС94252711003" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКА-120-ПЗ", AUDCode="КИУС94252711003-01" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-01" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа -25-ПЗ", AUDCode="КИУС94252711004-02" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-03" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-04" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-05 ПЗ", AUDCode="КИУС94252711004-05" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-05 ПЗ", AUDCode="КИУС94252711004-05.01" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-06" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-07 ПЗ", AUDCode="КИУС94252711004-07-ПЗ" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ ДЛЯ СТЕРИЛИЗАЦИИ РАСТРОВОР ЛЕКАРСТВ", AUDCode="КИУС94252711005" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ ДЛЯ СТЕРИЛИЗАЦИИ РАСТРОВОР ЛЕКАРСТВ", AUDCode="КИУС94252711005-01" },
                    new() { Name = "СТЕРИЛИЗАТОР ВКА-75-Р-ПЗ МОДЕРНИЗИРОВАННЫЙ", AUDCode="КИУС94252711005М" },
                    new() { Name = "СТЕРИЛИЗАТОР ВК-75-Р-ПЗ", AUDCode="КИУС942527110053" },
                    new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГПа-10-ПЗ", AUDCode="КИУС94252711006" },
                    new() { Name = "СТЕРИЛИЗАТОР ВОЗДУШНЫЙ ГП-320 \"ПЗ\"", AUDCode = "КИУС94252712001" },
                    new() { Name = "СТЕРИЛИЗАТОР ВОЗДУШНЫЙ АВОМАТИЧЕСКИЙ ГП-01", AUDCode = "КИУС94252712001-01" },
                    new() { Name = "СТЕРИЛИЗАТОР ВОЗДУШНЫЙ АВОМАТИЧЕСКИЙ ГП-01-01", AUDCode = "КИУС94252712001-01-01" },
                    new() { Name = "ШКАФ СУШИЛЬНЫЙ ШС-320-ПЗ", AUDCode = "КИУС94252712001-02" },
                };
                context.AddRange(subjects);
                context.SaveChanges();
        }
    }
}
