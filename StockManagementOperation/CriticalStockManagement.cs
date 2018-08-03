using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class CriticalStockManagement
    {
        StockEntities db;
        EmailManagement emailmanagement;
        public CriticalStockManagement()
        {
            db = new StockEntities();
            emailmanagement = new EmailManagement();
        }


        public Products GetCriticalProduct(int productid)
        {
            var sorgu = (from i in db.Products where i.ProductID == productid && i.ProductStock < i.CriticalStock select i).SingleOrDefault();
            if (sorgu != null)
            {
                emailmanagement.Mail(sorgu.Company.AppUsers.Email, "Kritik Stok Denetimi", "Merhaba " + sorgu.Company.CompanyName + " Ürünlerinizden " + sorgu.ProductName + " İsimli Ürünün Stoğ'u Kritik Stok Miktarını Aşmıştır Kalan Ürün  Sayınız " + sorgu.ProductStock + " Lütfen Stok Sayınızı Arttırınız");
            }

            return (sorgu);
        }

        public List<Products> getCompanyProducts(int companyid)
        {
            var CompanyProducts = (from i in db.Products where i.Company.CompanyID == companyid select i).ToList();
            return CompanyProducts;
        }

        public List<Products> getCompanyCriticStockProduct(int companyid)
        {
            var CompanyCriticStock = (from i in db.Products where i.Company.CompanyID == companyid && i.ProductStock < i.CriticalStock select i).ToList();
            return CompanyCriticStock;
        }

        public string stockUpdate(int stockcount, int productid)
        {
            var degisecekproduct = (from i in db.Products where i.ProductID == productid select i).SingleOrDefault();
            degisecekproduct.ProductStock = degisecekproduct.ProductStock + stockcount;
            db.SaveChanges();
            return "Stok ekleme basarılı";
        }





    }
}
