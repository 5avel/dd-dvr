using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class ConfigurationRepository
    {
        public ConfigurationRepository()
        {
            Currency = "грн.";
        }
        public string Currency { set; get; }
        public string GetOutputVodeoDir()
        {
            return @"D:\DD-DVR\Video\";
        }
    }
}
