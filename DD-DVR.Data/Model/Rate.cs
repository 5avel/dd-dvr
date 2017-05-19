using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data.Model
{
    [Serializable]
    public class Rate
    {
        public decimal Price { set; get;}
        
        
        public string Text
        {
            get
            {
                return String.Format("{0:0.00}",Price)+"грн.";
            }
        }
    }
}
