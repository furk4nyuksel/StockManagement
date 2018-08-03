using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class CountyAndCityManagement
    {
        StockEntities db;
        public CountyAndCityManagement()
        {
            db = new StockEntities();
        }
        public List<City> getallCity()
        {
            return (from i in db.City select i).ToList();
        }
        public List<County> getAllCounty()
        {
            return (from i in db.County select i).ToList();
        }

        public List<County> getCompanyCounty(int id)
        {
            return (from i in db.County where i.CityID == id select i).ToList();
        }

        public List<County> getCounty(int id)
        {
            return (from i in db.County where i.CityID == id select i).ToList();
        }
    }
}
