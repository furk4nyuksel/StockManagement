using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class PdfManagement
    {
        StockEntities db;
        public PdfManagement()
        {
            db = new StockEntities();
        }

        public void setCompanyOrderKey(string key, int companyid)
        {
            var keykontrol = (from i in db.CompanyOrderPdf where i.RndKey == key select i).Count();
            if (keykontrol == 0)
            {
                CompanyOrderPdf atılacakkey = new CompanyOrderPdf();
                atılacakkey.CompanyID = companyid;
                atılacakkey.RndKey = key;
                db.CompanyOrderPdf.Add(atılacakkey);
                db.SaveChanges();
            }
            else if (keykontrol == 1)//else if cunku keykontrol 1 den fazla olmamalı mantıken
            {
                var degisecekkey = (from i in db.CompanyOrderPdf where i.CompanyID == companyid select i).SingleOrDefault();
                degisecekkey.RndKey = key;
                db.SaveChanges();

            }
        }

        public string companyOrderPdfControl(string key, int companyid)
        {
            var deger = (from i in db.CompanyOrderPdf where i.RndKey == key && i.CompanyID == companyid select i).Count();
            if (deger > 0)
            {
                return "basarılı";
            }
            else
            {
                return "basarısız";
            }

        }



    }
}
