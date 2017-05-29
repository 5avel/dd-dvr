using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class VideoDataRepository : Repository<VideoDataRepository>
    {
        public Rout Rout { set; get; }
        public Bus Bus { set; get; }
        public Driver Driver { set; get; }
    }
}
