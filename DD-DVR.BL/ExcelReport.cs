using ClosedXML.Excel;
using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.BL
{
    class ExcelReport
    {
        public void SaveExcelReport(FareReport report)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Отчет");

            // Шапка
            worksheet.Cell(2, 1).Value = "Дата:";
            worksheet.Cell(2, 2).Value = report.Tours[0].tourStart.ToString("dd.MM.yyyy");

            worksheet.Cell(4, 1).Value = "Автобус №";
            worksheet.Cell(4, 2).Value = report.Bus.Title;

            worksheet.Cell(2, 4).Value = "Маршрут:";
            worksheet.Cell(2, 5).Value = report.Rout.Title;

            worksheet.Cell(4, 4).Value = "Водитель:";
            worksheet.Cell(4, 5).Value = report.Driver.Title;

            // Таблица Круги
            var dataTable = GetToursTable(report);
            worksheet.Cell(6, 1).Value = "Круги";
            worksheet.Range(6, 1, 6, 5).Merge().AddToNamed("Titles");
            var tableWithData = worksheet.Cell(7, 1).InsertTable(dataTable.AsEnumerable());
            tableWithData.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableWithData.ShowAutoFilter = false;
            tableWithData.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableWithData.Style.Border.OutsideBorderColor = XLColor.Black;
            tableWithData.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            tableWithData.Style.Border.BottomBorderColor = XLColor.Black;
            tableWithData.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            tableWithData.Style.Border.RightBorderColor = XLColor.Black;


            // Prepare the style for the titles
            var titlesStyle = workbook.Style;
            titlesStyle.Font.Bold = true;
            titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titlesStyle.Fill.BackgroundColor = XLColor.LightBlue;
            titlesStyle.Border.RightBorder = titlesStyle.Border.LeftBorder 
                = titlesStyle.Border.TopBorder = XLBorderStyleValues.Thin;
            titlesStyle.Border.RightBorderColor = titlesStyle.Border.LeftBorderColor 
                = titlesStyle.Border.TopBorderColor = XLColor.Black;

            // Format all titles in one shot
            workbook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs("InsertingTables.xlsx");
        }


        private DataTable GetToursTable(FareReport report)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Полукруг", typeof(int));
            table.Columns.Add("Начало", typeof(String));
            table.Columns.Add("Конец", typeof(String));
            table.Columns.Add("Количество", typeof(int));
            table.Columns.Add("Льготников", typeof(int));

            for (int i = 0; i < report.Tours.Count; i++)
            {
                table.Rows.Add(
                    i + 1,
                    report.Tours[i].tourStart.ToString("HH:mm:ss"),
                    report.Tours[i].tourEnd.ToString("HH:mm:ss"),
                    report.Tours[i].passengers.Count,
                    report.Tours[i].passengers.Count<Passenger>(p => p.isExemption)
                    );
            }
            return table;
        }
    }


}
