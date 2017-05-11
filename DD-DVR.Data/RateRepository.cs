using DD_DVR.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class RateRepository
    {
        public IEnumerable<Rate> GetAllRates()
        {
            return new List<Rate>
            {
                new Rate() {Price = 3.00m },
                new Rate() {Price = 4.00m },
                new Rate() { Price = 5m },
                new Rate() {Price = 6m }
            };
        }
    }
}
