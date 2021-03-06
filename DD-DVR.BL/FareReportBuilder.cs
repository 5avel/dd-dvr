﻿using DD_DVR.Data;
using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.BL
{
    public class FareReportBuilder
    {
        public FareReport Report;
        private string reportPath = "";

        public Tour CurentTour
        {
            get
            {
                if (Report.Tours.Count == 0) return null;
                return Report.Tours[Report.Tours.Count - 1];
            }
        }

        public void StartCalculation(string pathToVideo)
        {
            reportPath = pathToVideo + "\\Data\\";
            if (File.Exists(reportPath + "ReportRepository.xml"))
            {  // если отчет создан загружаем его
                Report = ReportRepository.LoadObjFromFile(reportPath).FareReport;
            }
            else
            { // Иначе создаем новый
                VideoDataRepository videoDataRepository = VideoDataRepository.LoadObjFromFile(reportPath);
                //TODO: Создаем новый отчет получая из папки Video/Data выбранные во время конвертации параметрs: Маршрут, Автобус, Водитель.
                Report = new Data.Model.FareReport()
                {
                    Rout = videoDataRepository.Rout,
                    Bus = videoDataRepository.Bus,
                    Driver = videoDataRepository.Driver
                };
                ReportRepository.SaveObjToFile(new ReportRepository() { FareReport = Report }, reportPath);


            }
            
        }

        public void EndCalculation()
        {
            Report.IsClosed = true;
        }

        public void SaveReport()
        {
            ReportRepository.SaveObjToFile(new ReportRepository() { FareReport = Report }, reportPath);
        }

        public void StartTour(DateTime dt)
        {
            Report.Tours.Add(new Tour() { tourStart = dt });
        }

        public void EndTour(DateTime dt)
        {
            CurentTour.tourEnd = dt;
        }

        public void SaveReportAsExecl()
        {
            new ExcelReport().SaveExcelReport(Report, reportPath);
        }
    }
}
