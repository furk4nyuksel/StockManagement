using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class StorageManagement
    {
        StockEntities db;
        public StorageManagement()
        {
            db = new StockEntities();
        }
        public List<Storage> getAllCompanyStorage(int companyid)
        {
            if ((from i in db.Storage where i.CompanyID == companyid && i.IsActive == true select i).Count() < 1)
            {
                return null;

            }
            else
            {
                return (from i in db.Storage where i.CompanyID == companyid && i.IsActive == true select i).ToList();
            }

        }
        public void addStorage(string storagename, int countyıd, string detay, int Companyıd)
        {
            Byte turdonusumu = Convert.ToByte(countyıd);
            Storage eklenecekstorage = new Storage { StorageName = storagename, CountyID = turdonusumu, AdressDetails = detay, IsActive = true, CompanyID = Companyıd };
            db.Storage.Add(eklenecekstorage);
            db.SaveChanges();
        }
        public void deleteStorage(int id)
        {
            var isactivecontrol = (from i in db.Storage where i.StorageID == id select i).SingleOrDefault();
            isactivecontrol.IsActive = false;
            db.SaveChanges();

        }
    }
}
