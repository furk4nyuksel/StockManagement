using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminStorageController : Controller
    {
        StorageManagement storageManagement;
        CountyAndCityManagement countyAndCityManagement;
        AppUsers gelen;
        Company companybilgi;
        public AdminStorageController()
        {
            storageManagement = new StorageManagement();
            countyAndCityManagement = new CountyAndCityManagement();
        }
        // GET: AdminStorage
        public ActionResult Storage()
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
                var allStorage = storageManagement.getAllCompanyStorage(companybilgi.CompanyID);
                ViewBag.storage = allStorage;
                var cities = countyAndCityManagement.getallCity();
                return View(cities);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
        [HttpPost]
        public ActionResult AddStorage(string storagename, string detay, int ddlCity = -1, int ddlcounty = -1)
        {
            //token
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
                if (!string.IsNullOrEmpty(storagename) && ddlcounty >= 0 && ddlCity >= 0 && !string.IsNullOrEmpty(detay))
                {
                    storageManagement.addStorage(storagename, ddlcounty, detay, companybilgi.CompanyID);
                    return RedirectToAction("Storage", "AdminStorage");
                }
                else
                {
                    Session["hata"] = "Alanlar boş geçilemez";
                    return RedirectToAction("Storage", "AdminStorage");
                }

            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }


        public ActionResult DeleteStorage(int id)
        {
            //token
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
                storageManagement.deleteStorage(id);
                return RedirectToAction("Storage", "AdminStorage");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        public JsonResult GetCounty(int id)
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
                var counties = countyAndCityManagement.getCounty(id);
                SelectList list = new SelectList(counties, "CountyID", "CountyName");
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }



        }

    }
}