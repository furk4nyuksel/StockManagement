using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementOperation;

namespace StockManagementOperation
{
    public class CompanyIndexManagement
    {
        StockEntities db;
        public CompanyIndexManagement()
        {
            db = new StockEntities();
        }

        public double CompanyProductView(int companyid)
        {
            if ((from i in db.ProductDetails where i.Products.CompanyID == companyid select i).Count() < 1)
            {
                return 0;

            }
            else
            {
                var toplamgoruntulenme = (from i in db.ProductDetails where i.Products.CompanyID == companyid select i).Sum(s => s.ProductView.Value);
                return toplamgoruntulenme;
            }

        }

        public double getDailySales(int companyid)
        {
            if ((from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Day == DateTime.Now.Day select i).Count() > 0)
            {
                var gunluksatıs = (from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Day == DateTime.Now.Day select i).Sum(s => s.Price.Value);

                return gunluksatıs;

            }
            else
            {
                return 0;

            }


        }

        public double getMonthlySales(int companyid)
        {
            if ((from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Month == DateTime.Now.Month select i).Count() > 0)
            {
                var aylıksatıs = (from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Month == DateTime.Now.Month select i).Sum(s => s.Price.Value);

                return aylıksatıs;
            }
            else
            {
                return 0;
            }
        }

        public double getYearlySales(int companyid)
        {
            if ((from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Year == DateTime.Now.Year select i).Count() > 0)
            {
                var yıllıksatıs = (from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Year == DateTime.Now.Year select i).Sum(s => s.Price.Value);

                return yıllıksatıs;
            }
            else
            {
                return 0;
            }
        }

        public double getTotalSales(int companyid)
        {

            if ((from i in db.OrderDetails where i.Products.CompanyID == companyid && ((DateTime)i.Order.CreDate).Year == DateTime.Now.Year select i).Count() > 0)
            {

                var totalsatıs = (from i in db.OrderDetails where i.Products.CompanyID == companyid select i).Sum(s => s.Price.Value);
                return totalsatıs;
            }
            else
            {
                return 0;
            }
        }

        public List<Storage> getStorage(int companyid)
        {
            if ((from i in db.Storage where i.CompanyID == companyid && i.IsActive == true select i).Count() > 0)
            {
                var depolar = (from i in db.Storage where i.CompanyID == companyid && i.IsActive == true select i).ToList();


                return depolar;
            }
            return null;
        }

    }
}
