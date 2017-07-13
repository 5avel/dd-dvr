using ClosedXML.Excel;
using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

            // Таблица Круги
            var dataTable = GetToursTable(report);
            
            var tableWithData = worksheet.Cell(2, 1).InsertTable(dataTable.AsEnumerable());
            tableWithData.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableWithData.ShowAutoFilter = false;
            tableWithData.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableWithData.Style.Border.OutsideBorderColor = XLColor.Black;
            tableWithData.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            tableWithData.Style.Border.BottomBorderColor = XLColor.Black;
            tableWithData.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            tableWithData.Style.Border.RightBorderColor = XLColor.Black;

            var dataTableCosts = GetResultTable(report);
            var tableWithData2 = worksheet.Cell(2+ dataTable.Rows.Count+2, 1).InsertTable(dataTableCosts.AsEnumerable());
            tableWithData2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableWithData2.ShowAutoFilter = false;
            tableWithData2.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            tableWithData2.Style.Border.OutsideBorderColor = XLColor.Black;
            tableWithData2.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            tableWithData2.Style.Border.BottomBorderColor = XLColor.Black;
            tableWithData2.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            tableWithData2.Style.Border.RightBorderColor = XLColor.Black;


            // Prepare the style for the titles
            //var titlesStyle = workbook.Style;
            //titlesStyle.Font.Bold = true;
            //titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //titlesStyle.Fill.BackgroundColor = XLColor.LightBlue;
            //titlesStyle.Border.RightBorder = titlesStyle.Border.LeftBorder 
            //    = titlesStyle.Border.TopBorder = XLBorderStyleValues.Thin;
            //titlesStyle.Border.RightBorderColor = titlesStyle.Border.LeftBorderColor 
            //    = titlesStyle.Border.TopBorderColor = XLColor.Black;

            // Format all titles in one shot
            //workbook.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs("InsertingTables.xlsx");
        }


        private DataTable GetToursTable(FareReport report)
        {
            DataTable table = new DataTable();
            table.Columns.Add("№ Рейса", typeof(int));
            table.Columns.Add("Пассажиров", typeof(int));
            table.Columns.Add("Льготных", typeof(int));
            table.Columns.Add("Простой", typeof(String));
            table.Columns.Add("Начало", typeof(String));
            table.Columns.Add("Длительность", typeof(String));
            table.Columns.Add("Окончание", typeof(String));
            
            

            for (int i = 0; i < report.Tours.Count; i++)
            {
                int tourNum = i + 1;
                int passengersCount = report.Tours[i].passengers.Count;
                int exemptionPassengers = report.Tours[i].passengers.Count<Passenger>(p => p.isExemption);
                DateTime expectation = 
                    i > 0 
                    ? new DateTime(report.Tours[i].tourStart.Ticks - report.Tours[i - 1].tourEnd.Ticks) 
                    : DateTime.MinValue;
                DateTime tourStart = report.Tours[i].tourStart;
                DateTime tourLenght = new DateTime(report.Tours[i].tourEnd.Ticks - report.Tours[i].tourStart.Ticks);
                DateTime tourEnd = report.Tours[i].tourEnd;

                // ToString("HH:mm:ss")

                table.Rows.Add(
                    tourNum,
                    passengersCount,
                    exemptionPassengers,
                    expectation.ToString("HH:mm:ss"),
                    tourStart.ToString("HH:mm:ss"),
                    tourLenght.ToString("HH:mm:ss"),
                    tourEnd.ToString("HH:mm:ss"));
            }
            return table;
        }

        private DataTable GetResultTable(FareReport report)
        {
            DataTable table = new DataTable();
            DateTime date = report.Tours[0].tourStart;
            table.Columns.Add("Дата:", typeof(String));
            table.Columns.Add(date.ToString("dd.MM.yyyy"), typeof(String));

            table.Rows.Add("День недели:", CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(date.DayOfWeek));
            table.Rows.Add("Маршрут:", report.Rout.Title);
            //table.Rows.Add("N выпуска:", "-");
            table.Rows.Add("N автобуса:", report.Bus.Title);
            table.Rows.Add("Водитель:", report.Driver.Title);
            DateTime onRout = new DateTime(report.Tours[report.Tours.Count - 1].tourEnd.Ticks - report.Tours[0].tourStart.Ticks);
            table.Rows.Add("На маршруте:", onRout.ToString("HH:mm:ss"));
            table.Rows.Add("Рейсов:", report.Tours.Count.ToString());
            table.Rows.Add("Пассажиров", "");
            table.Rows.Add("Всего:", report.Tours.Sum(x => x.passengers.Count).ToString());
            table.Rows.Add("Льготных:", report.Tours.Sum(x => x.passengers.Count(q => q.isExemption)));
            table.Rows.Add("Платили:", report.Tours.Sum(x => x.passengers.Count(q => q.isExemption == false)));
            //table.Rows.Add("Тариф:", "");
            table.Rows.Add("Касса:", report.Tours.Sum(x => x.passengers.Sum(q => q.pay)).ToString());
            return table;
        }

        private DataTable GetRatesTable(FareReport report)
        {
            DataTable table = new DataTable();
            DateTime date = report.Tours[0].tourStart;
            table.Columns.Add("Цена:", typeof(String));
            table.Columns.Add(date.ToString("Количество"), typeof(String));
            table.Columns.Add(date.ToString("Сумма"), typeof(String));

            // получить все ставки 
            // вывести количества и суммы по всем ставкам

            table.Rows.Add("День недели:", CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(date.DayOfWeek));
            
            return table;
        }
    }


}
