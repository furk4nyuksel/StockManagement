using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{

    public class AdminCategoryController : Controller
    {
        CategoryManagement categoryManagement;
        AppUsers gelen;

        public AdminCategoryController()
        {
            categoryManagement = new CategoryManagement();
        }

        public ActionResult Category()
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
                return View(categoryManagement.GetAllCategory());
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
        public ActionResult addCategory(string categoryname = "", int ordernumber = -1)
        {
            if (Session["Login"] != null)
            {
                if (categoryname != "" && ordernumber > 0)
                {
                    gelen = (AppUsers)Session["Login"];
                    Session["hata"] = categoryManagement.addCategory(categoryname, ordernumber);
                    return RedirectToAction("Category", "AdminCategory");
                }
                else
                {
                    Session["hata"] = "Alanlar boş geçilemez";
                    return RedirectToAction("Category", "AdminCategory");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }

        [HttpGet]
        public ActionResult updateCategory(int categoryıd = -1)
        {
            if (Session["Login"] != null)
            {
                if (categoryıd > 0)
                {
                    ViewBag.deisecekkategori = categoryıd;
                    ViewBag.categoriad = categoryManagement.getCategoriNameById(categoryıd);
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
            return View();
        }
        [HttpPost]
        public ActionResult updateCategory(int categoryıd = -1, string categoriname = "", int order = -1)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status == 3)
                {
                    if (categoryıd < 0 && categoriname == "" && order < 0)
                    {
                        Session["hata"] = "Alanlar boş geçilemez";
                        return View();
                    }
                    else
                    {
                        string categoriadıkontrol = categoryManagement.UpdateCategory(categoryıd, categoriname, order);
                        Session["hata"] = categoriadıkontrol;
                        return RedirectToAction("Category", "AdminCategory");
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

    }
}