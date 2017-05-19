using DD_DVR.Data.Model;

using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DD_DVR.Data
{
    public class RoutRepository : BaseRepository<RoutRepository>
    {
        public static new string savePath = "Configs\\RoutRepository.xml";
        private RoutRepository() { }
   
        public List<Driver> Drivers { set; get; }
        public List<Bus> Buses { set; get; }
        public List<Rout> Routes { set; get; }
    }
}
