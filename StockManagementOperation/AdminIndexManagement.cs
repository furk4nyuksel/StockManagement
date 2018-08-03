using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class AdminIndexManagement
    {
        StockEntities db;
        public AdminIndexManagement()
        {
            db = new StockEntities();
        }
        public double ProductView()
        {
            if ((from i in db.ProductDetails select i).Count() < 1)
            {
                return 0;

            }
            else
            {
                var toplamgoruntulenme = (from i in db.ProductDetails select i).Sum(s => s.ProductView.Value);
                return toplamgoruntulenme;
            }

        }

        public double TotalUser()
        {
            if ((from i in db.AppUsers select i).Count() < 1)
            {
                return 0;

            }
            else
            {
                var toplamuser = (from i in db.AppUsers select i).Count();
                return toplamuser;
            }

        }

        public double TotalCompany()
        {
            if ((from i in db.Company select i).Count() < 1)
            {
                return 0;

            }
            else
            {
                var toplamcompany = (from i in db.Company select i).Count();
                return toplamcompany;
            }

        }

        public double TotalProduct()
        {
            if ((from i in db.Products select i).Count() < 1)
            {
                return 0;

            }
            else
            {
                var toplamproduct = (from i in db.Products select i).Count();
                return toplamproduct;
            }

        }

        public double getDailySales()
        {
            if ((from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Day == DateTime.Now.Day select i).Count() > 0)
            {
                var gunluksatıs = (from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Day == DateTime.Now.Day select i).Sum(s => s.Price.Value);

                return gunluksatıs;

            }
            else
            {
                return 0;

            }


        }

        public double getMonthlySales()
        {
            if ((from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Month == DateTime.Now.Month select i).Count() > 0)
            {
                var aylıksatıs = (from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Month == DateTime.Now.Month select i).Sum(s => s.Price.Value);

                return aylıksatıs;
            }
            else
            {
                return 0;
            }
        }

        public double getYearlySales()
        {
            if ((from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Year == DateTime.Now.Year select i).Count() > 0)
            {
                var yıllıksatıs = (from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Year == DateTime.Now.Year select i).Sum(s => s.Price.Value);

                return yıllıksatıs;
            }
            else
            {
                return 0;
            }
        }

        public double getTotalSales()
        {

            if ((from i in db.OrderDetails where ((DateTime)i.Order.CreDate).Year == DateTime.Now.Year select i).Count() > 0)
            {

                var totalsatıs = (from i in db.OrderDetails select i).Sum(s => s.Price.Value);
                return totalsatıs;
            }
            else
            {
                return 0;
            }
        }
    }
}
