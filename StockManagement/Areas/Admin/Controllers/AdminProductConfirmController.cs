using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{

    public class AdminProductConfirmController : Controller
    {
        ProductManagement productManagement;
        AppUsers gelen;
        public AdminProductConfirmController()
        {
            productManagement = new ProductManagement();
        }

        public ActionResult ProductConfirm()
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status.Value == 3)
            {
                var allProductDeactivate = productManagement.GetAllDeactiveProduct();
                return View(allProductDeactivate);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }


        public ActionResult Confirmed(int id)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status.Value == 3)
            {
                var allProductDeactivate = productManagement.productConfirm(id);
                return RedirectToAction("ProductConfirm", "AdminProductConfirm");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
        public ActionResult Deleted(int id)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status.Value == 3)
            {
                productManagement.DeleteProductById(id);
                return RedirectToAction("ProductConfirm", "AdminProductConfirm");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
    }
}