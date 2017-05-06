using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class FareReport
    {
        List<Tour> tours = new List<Tour>();
    }

    public class Tour
    {
        DateTime tourStart;
        DateTime tourEnd;
        List<Passenger> passengers = new List<Passenger>();
    }

    public class Passenger
    {
        DateTime payTime;
        decimal pay;
        bool isExemption;
    }

}
