using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.BL
{
    public class FareReportBuilder
    {
        public Data.Model.FareReport Report;

        public bool isClosedTour = true;

        public Tour CurentTour
        {
            get
            {
                if (Report.Tours.Count == 0) return null;
                return Report.Tours[Report.Tours.Count - 1];
            }
        }

        public void StartCalculation()
        {
            Report = new Data.Model.FareReport();
        }

        public void EndCalculation()
        {
            Report.IsClosed = true;
        }

        public void StartTour()
        {
            Report.Tours.Add(new Tour() { tourStart = DateTime.Now });
            isClosedTour = false;
        }

        public void EndTour()
        {
            CurentTour.tourEnd = DateTime.Now;
            isClosedTour = true;
        }


    }
}
