using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data.Model
{
    public class FareReport 
    {
        public FareReport()
        {
            Tours = new List<Tour>();
        }
        public List<Tour> Tours { set; get; }
        public bool IsClosed { get; set; }
    }

    public class Tour
    {
        public DateTime tourStart;
        public DateTime tourEnd;
        public List<Passenger> passengers = new List<Passenger>();
    }

    public class Passenger
    {
        public DateTime payTime;
        public decimal pay;
        public bool isExemption;
    }

}
