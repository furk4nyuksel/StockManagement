using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class CargoManagement
    {
        StockEntities db;
        public CargoManagement()
        {
            db = new StockEntities();

        }

        public List<Cargo> getAllCargo()
        {
            return (from i in db.Cargo select i).ToList();
        }
        public void deleteCargo(int id)
        {
            var isactivecontrol = (from i in db.Cargo where i.CargoID == id select i).SingleOrDefault();
            isactivecontrol.IsActive = false;
            db.SaveChanges();

        }
        public void addCargo(string cargoname)
        {
            Cargo cargoekle = new Cargo { CargoName = cargoname, IsActive = true };
            db.Cargo.Add(cargoekle);
            db.SaveChanges();
        }
    }
}
