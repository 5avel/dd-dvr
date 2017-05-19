using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DD_DVR.Data
{
    public class RateRepository : BaseRepository<RateRepository>
    {
        public static new string savePath = "Configs\\RateRepository.xml";

        private RateRepository() {}
       
        public List<Rate> Rates { set; get; }
        public int SelectedRateNum { set; get; }
    }
}
