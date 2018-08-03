using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminManagementController : Controller
    {
        AdminManagement adminManagement;
        AppUsers gelen;

        public AdminManagementController()
        {
            adminManagement = new AdminManagement();
        }
        public ActionResult AdminManage()
        {
            if (Session["Login"] != null)
            { gelen = (AppUsers)Session["Login"]; }
            else
            { return RedirectToAction("Index", "Home", new { area = "" }); }

            if (gelen.Status.Value == 3)
            {
                return View(adminManagement.getAllAdmin());
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }
        public ActionResult AddAdmin(string email = "")
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status.Value == 3)
                {
                    if (email != "")
                    {
                        string kontrol = adminManagement.addAdmin(email);
                        Session["hata"] = kontrol;
                        return RedirectToAction("AdminManage", "AdminManagement");
                    }
                    else
                    {
                        Session["hata"] = "Email boş girilimez";
                        return RedirectToAction("AdminManage", "AdminManagement");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }
        public ActionResult DeleteAdmin(int id)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status.Value == 3)
                {
                    adminManagement.deleteAdmin(id);
                    return RedirectToAction("AdminManage", "AdminManagement");
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }
    }
}