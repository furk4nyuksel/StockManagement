using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminStockController : Controller
    {
        AppUsers gelen;
        Company companybilgi;
        CriticalStockManagement criticalStockManagement;
        // GET: Admin/AdminStock

        public AdminStockController()
        {
            criticalStockManagement = new CriticalStockManagement();
        }
        [HttpGet]
        public ActionResult Stock()
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status.Value == 2)
            {
                return View(criticalStockManagement.getCompanyProducts(companybilgi.CompanyID));
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        [HttpPost]
        public ActionResult Stock(int stockcount = -1, int productid = -1)
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status.Value == 2)
            {
                if (stockcount > 0 && productid > 0)
                {
                    Session["hata"] = criticalStockManagement.stockUpdate(stockcount, productid);
                }
                else
                {
                    Session["hata"] = "Alanlar boş olamaz";
                }
                return RedirectToAction("Stock", "AdminStock");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
    }
}