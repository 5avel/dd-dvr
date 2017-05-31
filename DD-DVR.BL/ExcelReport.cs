using ClosedXML.Excel;
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
        public void SaveExcelReport()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Отчет");
            // From a DataTable
            var dataTable = GetTable();
            worksheet.Cell(7, 1).Value = "From DataTable";
            worksheet.Range(7, 1, 7, 5).Merge().AddToNamed("Titles");
            var tableWithData = worksheet.Cell(8, 1).InsertTable(dataTable.AsEnumerable());

            // Prepare the style for the titles
            var titlesStyle = workbook.Style;
            titlesStyle.Font.Bold = true;
            titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titlesStyle.Fill.BackgroundColor = XLColor.LightBlue;

            // Format all titles in one shot
            workbook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs("InsertingTables.xlsx");
        }


        private DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Полукруг", typeof(int));
            table.Columns.Add("Начало", typeof(DateTime));
            table.Columns.Add("Конец", typeof(DateTime));
            table.Columns.Add("Количество", typeof(int));
            table.Columns.Add("Льготников", typeof(int));

            table.Rows.Add(1, DateTime.Now, DateTime.Now, 54, 5);
            table.Rows.Add(2, DateTime.Now, DateTime.Now, 23, 2);
            table.Rows.Add(3, DateTime.Now, DateTime.Now, 34, 6);
            table.Rows.Add(4, DateTime.Now, DateTime.Now, 33, 5);
            table.Rows.Add(5, DateTime.Now, DateTime.Now, 51, 2);
            table.Rows.Add(6, DateTime.Now, DateTime.Now, 5, 1);
            return table;
        }
    }


}
