using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class ReportRepository : Repository<ReportRepository>
    {
        public FareReport FareReport { set; get; }
    }
}
