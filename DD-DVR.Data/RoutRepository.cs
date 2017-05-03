using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_DVR.Data
{
    public class RoutRepository
    {
        public IEnumerable<Driver> GetAllDrivers()
        {
            return new List<Driver>
            {
                new Driver() {Title = "Иванов И.И."},
                new Driver(){Title = "Петров П.П."},
                new Driver(){ Title = "Максимов М.М." },
                new Driver() {Title = "Павлов П.П." }
            };
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return new List<Bus>
            {
                new Bus() {Title = "1234"},
                new Bus(){Title = "2345"},
                new Bus(){ Title = "5662" },
                new Bus() {Title = "1111" }
            };
        }
        public IEnumerable<Rout> GetAllRoutes()
        {
            return new List<Rout>
            {
                new Rout() {Title = "Орловщина"},
                new Rout(){Title = "Маршрут №5"},
                new Rout(){ Title = "Маршрут №6" }
            };
        }
    }
}
