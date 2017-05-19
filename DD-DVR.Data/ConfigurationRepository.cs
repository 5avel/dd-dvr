using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class ConfigurationRepository : BaseRepository<ConfigurationRepository>
    {
        private ConfigurationRepository(){ }

        public static new string savePath = "Configs\\ConfigurationRepository.xml";

        public string OutputVodeoDir { set; get; }

    }
}
