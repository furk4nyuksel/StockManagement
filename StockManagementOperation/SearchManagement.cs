using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{

    public class SearchManagement
    {
        StockEntities db;

        public SearchManagement()
        {
            db = new StockEntities();
        }


        public List<Products> SearchProductMenu(string aranan)
        {
            List<Products> sorgu = (from i in db.Products where i.IsActive.Value && i.Company.IsActive.Value && i.ProductStock > 0 && i.ProductName.ToUpper().Contains(aranan.ToLower()) select i).ToList();

            return (sorgu);
        }

        public List<Products> SearchCompanyProductMenu(int companyID, string aranan)
        {
            List<Products> sorgu = (from i in db.Products where i.CompanyID == companyID && i.IsActive.Value && i.Company.IsActive.Value && i.ProductStock > 0 && i.ProductName.ToUpper().Contains(aranan.ToLower()) select i).ToList();

            return (sorgu);
        }


    }
}
