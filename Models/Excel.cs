using ClosedXML.Excel;

namespace ItineraryOperations.Models
{
    // Модель для исполнителя (шапка)
    public class ExcelExecutorInfo
    {
        public string Name { get; set; }        // Алехина Г.
        public string Department { get; set; }  // Цех 387

        public void WriteToExcel(IXLWorksheet worksheet, int row)
        {
            worksheet.Cell(row, 1).Value = $"Исполнитель: \"{Name}\"";
            worksheet.Range(row, 1, row, 5).Merge();
            worksheet.Cell(row, 1).Style.Font.SetBold(true);
            worksheet.Cell(row, 1).Style.Font.SetFontSize(12);
            worksheet.Cell(row, 1).Style.Font.SetFontColor(XLColor.DarkBlue);

            worksheet.Cell(row, 6).Value = $"Подразделение: {Department}";
            worksheet.Range(row, 6, row, 15).Merge();
            worksheet.Cell(row, 6).Style.Font.SetBold(true);
            worksheet.Cell(row, 6).Style.Font.SetFontColor(XLColor.DarkBlue);

            for (int col = 1; col <= 15; col++)
                worksheet.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
        }
    }

    public class ExcelOperationRow
    {
        public string CPC { get; set; }
        public string OperationTypeName { get; set; }      
        public DateTime IssueDate { get; set; }  
        public DateTime ExecutionDate { get; set; }
        public decimal Tariff { get; set; } 
        public decimal Payment { get; set; }
        public decimal NT { get; set; }    
        public decimal NTonPayment { get; set; } 
        public int Kit { get; set; }         
        public decimal KitOnNTPayment { get; set; } 
        public decimal Coefficient { get; set; }
        public decimal TotalSum { get; set; } 
        public decimal PremiumPercent { get; set; }
        public decimal Premium { get; set; }
        public int ProductId { get; set; }

        public void WriteToExcel(IXLWorksheet worksheet, int row)
        {
            worksheet.Cell(row, 1).Value = CPC; 
            worksheet.Cell(row, 2).Value = OperationTypeName;
            worksheet.Cell(row, 3).Value = IssueDate.ToString("dd.MM.yyyy");
            worksheet.Cell(row, 4).Value = ExecutionDate.ToString("dd.MM.yyyy");
            worksheet.Cell(row, 5).Value = Tariff;
            worksheet.Cell(row, 6).Value = Payment;
            worksheet.Cell(row, 7).Value = NT;
            worksheet.Cell(row, 8).Value = NTonPayment;
            worksheet.Cell(row, 9).Value = Kit;
            worksheet.Cell(row, 10).Value = KitOnNTPayment;
            worksheet.Cell(row, 11).Value = Coefficient;
            worksheet.Cell(row, 12).Value = TotalSum;
            worksheet.Cell(row, 13).Value = PremiumPercent;
            worksheet.Cell(row, 14).Value = Premium;
        }
    }

    public class ExcelProductDetail
    {
        public string ProductName { get; set; }

        public void WriteToExcel(IXLWorksheet worksheet, int row)
        {
            // Стиль для строки с изделием 
            for (int col = 1; col <= 15; col++)
            {
                var cell = worksheet.Cell(row, col);
                cell.Style.Font.SetItalic(true);
                cell.Style.Font.SetFontColor(XLColor.Gray);
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            worksheet.Cell(row, 1).Value = $"Изделие:";
            worksheet.Cell(row, 1).Style.Font.SetBold(true) ;
            worksheet.Cell(row, 2).Value =  ProductName;
            worksheet.Range(row, 2, row, 15).Merge(); 

        }
    }

    public class ExcelItinerary
    {
        public string AUDName { get; set; }
        public string AUDCode { get; set; } 
        public string KitIncreasingKit { get; set; }


        public void WriteToExcel(IXLWorksheet worksheet, int row)
        {
            // Стиль для строки с изделием 
            for (int col = 1; col <= 15; col++)
            {
                var cell = worksheet.Cell(row, col);
                cell.Style.Font.SetItalic(true);
                cell.Style.Font.SetFontColor(XLColor.Gray);
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            worksheet.Cell(row, 1).Value = $"ДСЕ:";
            worksheet.Cell(row, 1).Style.Font.SetBold(true) ;
            worksheet.Cell(row, 2).Value = AUDName;
            worksheet.Range(row, 2, row, 5).Merge();

            worksheet.Cell(row, 6).Value = $"КОД ДСЕ:";
            worksheet.Cell(row, 6).Style.Font.SetBold(true) ;
            worksheet.Cell(row, 7).Value =  AUDName;
            worksheet.Range(row, 7, row, 11).Merge();

            worksheet.Cell(row, 12).Value = $"Комплект:";
            worksheet.Cell(row, 12).Value = KitIncreasingKit;
            worksheet.Cell(row, 12).Style.Font.SetBold(true) ;
            worksheet.Range(row, 12, row, 15).Merge(); 

        }
    }

    public class ExcelTaskResult
    {
        public decimal NT {  set; get; }
        
        public decimal NTonPayment { set; get; }
        
        public decimal KitOnNTPayment { get; set; }
        public decimal TotalSum { get; set; }
        
        public decimal Premium { set; get; }
        public decimal TotalSumWithPremium { set; get; }

        public void WriteToExcel(IXLWorksheet worksheet, int row)
        {
            worksheet.Cell(row, 1).Value = "Итого по начислению:";
            worksheet.Range(row, 1, row, 7).Merge();
            worksheet.Cell(row, 1).Style.Font.SetBold(true);

            worksheet.Cell(row, 8).Value = NT;
            worksheet.Cell(row, 8).Style.NumberFormat.Format = "0.0000";

            worksheet.Cell(row, 9).Value = NTonPayment;
            worksheet.Cell(row, 9).Style.NumberFormat.Format = "0.0000";

            worksheet.Cell(row, 10).Value = TotalSum;
            worksheet.Range(row, 10, row, 11).Merge();
            worksheet.Cell(row, 10).Style.NumberFormat.Format = "0.00";

            worksheet.Cell(row, 12).Value = TotalSumWithPremium;
            worksheet.Range(row, 12, row, 13).Merge();
            worksheet.Cell(row, 12).Style.NumberFormat.Format = "0.00";

            worksheet.Cell(row, 14).Value = Premium;
            worksheet.Range(row, 14, row, 15).Merge();
            worksheet.Cell(row, 14).Style.NumberFormat.Format = "0.00";

            for (int col = 1; col <= 15; col++)
            {
                worksheet.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightGreen;
                worksheet.Cell(row, col).Style.Font.SetItalic(true);
            }
        }
    }

    public class ExcelExecutorResult
    {
        public decimal NT { set; get; }

        public decimal NTonPayment { set; get; }

        public decimal TotalSum { get; set; }

        public decimal Premium { set; get; }
        public decimal TotalSumWithPremium { set; get; }

        public void WriteToExcel(IXLWorksheet worksheet, int row)
        {
            worksheet.Cell(row, 1).Value = "Итого по начислению для исполнителя:";
            worksheet.Range(row, 1, row, 7).Merge();
            worksheet.Cell(row, 1).Style.Font.SetBold(true);
            worksheet.Cell(row, 1).Style.Font.SetFontColor(XLColor.DarkOrange);

            worksheet.Cell(row, 8).Value = NT;
            worksheet.Cell(row, 8).Style.NumberFormat.Format = "0.0000";

            worksheet.Cell(row, 9).Value = NTonPayment;
            worksheet.Cell(row, 9).Style.NumberFormat.Format = "0.0000";

            worksheet.Cell(row, 10).Value = TotalSumWithPremium;
            worksheet.Range(row, 10, row, 13).Merge();
            worksheet.Cell(row, 10).Style.NumberFormat.Format = "0.00";

            worksheet.Cell(row, 14).Value = Premium;
            worksheet.Range(row, 14, row, 15).Merge();
            worksheet.Cell(row, 14).Style.NumberFormat.Format = "0.00";

            for (int col = 1; col <= 15; col++)
            {
                worksheet.Cell(row, col).Style.Fill.BackgroundColor = XLColor.LightSalmon;
                worksheet.Cell(row, col).Style.Font.SetBold(true);
            }
        }

        public void WriteFooterSumToExcel(IXLWorksheet worksheet, int row)
        {
            worksheet.Cell(row, 1).Value = "ОБЩИЙ ИТОГ ПО ВСЕМ ИСПОЛНИТЕЛЯМ";
            worksheet.Range(row, 1, row, 7).Merge();
            worksheet.Cell(row, 1).Style.Font.SetBold(true);
            worksheet.Cell(row, 1).Style.Font.SetFontSize(12);
            worksheet.Cell(row, 1).Style.Font.SetFontColor(XLColor.White);

            worksheet.Cell(row, 8).Value = NT;
            worksheet.Cell(row, 8).Style.NumberFormat.Format = "0.0000";

            worksheet.Cell(row, 9).Value = NTonPayment;
            worksheet.Cell(row, 9).Style.NumberFormat.Format = "0.0000";

            worksheet.Cell(row, 10).Value = TotalSum;
            worksheet.Range(row, 10, row, 11).Merge();
            worksheet.Cell(row, 10).Style.NumberFormat.Format = "0.00";

            worksheet.Cell(row, 12).Value = TotalSumWithPremium;
            worksheet.Range(row, 12, row, 13).Merge();
            worksheet.Cell(row, 12).Style.NumberFormat.Format = "0.00";

            worksheet.Cell(row, 14).Value = Premium;
            worksheet.Range(row, 14, row, 15).Merge();
            worksheet.Cell(row, 14).Style.NumberFormat.Format = "0.00";

            for (int col = 1; col <= 15; col++)
            {
                worksheet.Cell(row, col).Style.Fill.BackgroundColor = XLColor.DarkSlateBlue;
                worksheet.Cell(row, col).Style.Font.SetBold(true);
                worksheet.Cell(row, col).Style.Font.SetFontColor(XLColor.White);
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            }
        }
    }

    public class ExcelDivisionResult
    {
        public decimal NT { set; get; }

        public decimal NTonPayment { set; get; }

        public decimal TotalSum { get; set; }         // Итого

        public decimal Premium { set; get; }
    }

    // Extension methods for Excel writing operations
    public static class ExcelWriteExtensions
    {
        public static void WriteOperationsToExcel(this XLWorkbook workbook, int startRow, List<ExcelOperationRow> operations)
        {
            var worksheet = workbook.Worksheet(1);

            int currentRow = startRow;

            foreach (var op in operations)
            {
                op.WriteToExcel(worksheet, currentRow);
                currentRow++;
            }
        }

        public static void WriteTableHeaders(this IXLWorksheet worksheet, int row)
        {
            // Список заголовков как на скриншоте
            string[] headers = {
            "ШПЗ", "Операция", "Дата выдачи", "Дата исп.", 
            "Тариф", "Ц, руб.", "НВР, руб.", "ИЦ, руб.",
            "Кол-во", "Сумма",
            "К", "Итого", "% прем.", "Премия"
        };

            // Записываем каждый заголовок в свою колонку
            for (int col = 0; col < headers.Length; col++)
            {
                var cell = worksheet.Cell(row, col + 1);
                cell.Value = headers[col];

                // Стиль заголовков (жирный, по центру, с фоном)
                cell.Style.Font.SetBold(true);
                cell.Style.Font.SetFontName("Arial");
                cell.Style.Font.SetFontSize(10);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
        }

        public static void WriteOrderHeader(this IXLWorksheet worksheet, int row,
        string orderNumber, DateTime date)
        {
            // Строка 1: "Наряд: №12345"
            worksheet.Cell(row, 1).Value = $"Наряд: №{orderNumber}";
            worksheet.Range(row+1, 1, row+1, 4).Merge();
            worksheet.Cell(row, 1).Style.Font.SetBold(true);
            worksheet.Cell(row, 1).Style.Font.SetFontSize(12);

            // Строка 2: "Дата: 30.09.2015"
            worksheet.Cell(row, 5).Value = $"Дата: {date:dd.MM.yyyy}";
            worksheet.Range(row+1, 5, row+1, 15).Merge();
            worksheet.Cell(row, 5).Style.Font.SetBold(true);

            worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.LightYellow;
        }
    }
}
