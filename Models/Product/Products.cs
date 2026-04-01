using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItineraryOperations.Models
{
    public class Products
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string AUDCode { get; set; }
        public MainSubject MainSubject { get; set; }

        public string? CPC { get; set; }

        [Required]
        public int DivisionID { get; set; }
        [ForeignKey("DivisionID")]
        public Divisions Division { get; set; }
        public static void Felling(PostgresContext context)
        {
            context.Products.ExecuteDelete();
            context.SaveChanges();

            var products = new List<Products>
            {
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ С ВОЗМОЖНОСТЬЮ ВЫБОРА РЕЖИМОВ-01", AUDCode="КИУС94252711001-01", DivisionID = 381 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ С ВОЗМОЖНОСТЬЮ ВЫБОРА РЕЖИМОВ-02", AUDCode="КИУС94252711001-02", DivisionID = 186 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКА-120-ПЗ", AUDCode="КИУС94252711003", DivisionID = 342 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКА-120-ПЗ", AUDCode="КИУС94252711003-01", DivisionID = 342 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004", DivisionID = 178 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-01", DivisionID = 178 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа -25-ПЗ", AUDCode="КИУС94252711004-02", DivisionID = 178 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-03", DivisionID = 342 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-04", DivisionID = 382 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-05 ПЗ", AUDCode="КИУС94252711004-05", DivisionID = 381 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-05 ПЗ", AUDCode="КИУС94252711004-05.01", DivisionID = 342 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-ПЗ", AUDCode="КИУС94252711004-06", DivisionID = 342 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГКа-25-07 ПЗ", AUDCode="КИУС94252711004-07-ПЗ", DivisionID = 178 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ ДЛЯ СТЕРИЛИЗАЦИИ РАСТРОВОР ЛЕКАРСТВ", AUDCode="КИУС94252711005", DivisionID = 175 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ АВТОМАТИЧЕСКИЙ ДЛЯ СТЕРИЛИЗАЦИИ РАСТРОВОР ЛЕКАРСТВ", AUDCode="КИУС94252711005-01", DivisionID = 175 },
                new() { Name = "СТЕРИЛИЗАТОР ВКА-75-Р-ПЗ МОДЕРНИЗИРОВАННЫЙ", AUDCode="КИУС94252711005М", DivisionID = 194 },
                new() { Name = "СТЕРИЛИЗАТОР ВК-75-Р-ПЗ", AUDCode="КИУС942527110053", DivisionID = 194 },
                new() { Name = "СТЕРИЛИЗАТОР ПАРОВОЙ ГПа-10-ПЗ", AUDCode="КИУС94252711006", DivisionID = 115 },
                new() { Name = "СТЕРИЛИЗАТОР ВОЗДУШНЫЙ ГП-320 \"ПЗ\"", AUDCode = "КИУС94252712001", DivisionID = 115},
                new() { Name = "СТЕРИЛИЗАТОР ВОЗДУШНЫЙ АВОМАТИЧЕСКИЙ ГП-01", AUDCode = "КИУС94252712001-01", DivisionID = 178},
                new() { Name = "СТЕРИЛИЗАТОР ВОЗДУШНЫЙ АВОМАТИЧЕСКИЙ ГП-01-01", AUDCode = "КИУС94252712001-01-01", DivisionID = 178},
                new() { Name = "ШКАФ СУШИЛЬНЫЙ ШС-320-ПЗ", AUDCode = "КИУС94252712001-02", DivisionID = 144},
                new() { Name = "ШКАФ СУШИЛЬНЫЙ ШС-320-ПЗ", AUDCode = "КИУС94252712001-02", DivisionID = 144},
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
