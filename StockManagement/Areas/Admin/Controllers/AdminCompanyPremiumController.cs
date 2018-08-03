using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminCompanyPremiumController : Controller
    {
        AppUsers gelen;
        Company gelensirket;
        ProductManagement productManagement;
        public AdminCompanyPremiumController()
        {
            productManagement = new ProductManagement();

        }
        // GET: Admin/AdminCompanyPremium
        public ActionResult PremiumProduct()
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                gelensirket = (Company)Session["Company"];
                if (gelen.Status == 2)
                {
                    return View(productManagement.getAllProductifnotPremium(gelensirket.CompanyID));
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        public ActionResult SendProduct(int productid)
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                gelensirket = (Company)Session["Company"];
                if (gelen.Status == 2)
                {
                    productManagement.AddPremiumProduct(productid);
                    return RedirectToAction("PremiumProduct", "AdminCompanyPremium");
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }


    }
}