using DD_DVR.Data.Model;
using System.Collections.Generic;

namespace DD_DVR.Data
{
    public class RoutRepository
    {
        public List<Driver> Drivers { set; get; }
        public List<Bus> Buses { set; get; }
        public List<Rout> Routes { set; get; }
    }
}
