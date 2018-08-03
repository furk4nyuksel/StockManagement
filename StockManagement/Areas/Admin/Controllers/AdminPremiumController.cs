using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminPremiumController : Controller
    {
        ProductManagement productManagement;
        AppUsers gelen;
        public AdminPremiumController()
        {
            productManagement = new ProductManagement();
        }
        // GET: Admin/AdminPremium
        public ActionResult Premium()
        {
            if (Session["Login"] != null)
            { gelen = (AppUsers)Session["Login"]; }
            else
            { return RedirectToAction("Index", "Home", new { area = "" }); }

            if (gelen.Status.Value == 3)
            {
                return View(productManagement.getAllPremiumProduct());
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        public ActionResult ActivatePremium(int productid)
        {
            if (Session["Login"] != null)
            { gelen = (AppUsers)Session["Login"]; }
            else
            { return RedirectToAction("Index", "Home", new { area = "" }); }

            if (gelen.Status.Value == 3)
            {
                productManagement.ActiveProductPremium(productid);
                return RedirectToAction("Premium", "AdminPremium");
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        public ActionResult DeActivatePremium(int productid)
        {
            if (Session["Login"] != null)
            { gelen = (AppUsers)Session["Login"]; }
            else
            { return RedirectToAction("Index", "Home", new { area = "" }); }

            if (gelen.Status.Value == 3)
            {
                productManagement.DeleteProductPremium(productid);
                return RedirectToAction("Premium", "AdminPremium");
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
    }
}