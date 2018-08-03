using StockManagementOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminProductController : Controller
    {
        CategoryManagement categoryManagement;
        StorageManagement storageManagement;
        CargoManagement cargoManagement;
        ProductManagement productManagement;
        AppUsers gelen;
        Company companybilgi;

        string resimyolu;

        public AdminProductController()
        {

            categoryManagement = new CategoryManagement();
            storageManagement = new StorageManagement();
            cargoManagement = new CargoManagement();
            productManagement = new ProductManagement();

        }
        // GET: AdminProduct
        public ActionResult Product()
        {

            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];

                if (gelen.Status.Value == 2)
                {
                    if (storageManagement.getAllCompanyStorage(companybilgi.CompanyID) == null)
                    {
                        return RedirectToAction("NoStorage", "AdminProduct");
                    }
                    ViewBag.categori = categoryManagement.GetAllCategory();
                    new SelectList(categoryManagement.GetAllCategory(), "CategoryID", "CategoryName", 1);
                    ViewBag.storage = storageManagement.getAllCompanyStorage(companybilgi.CompanyID);
                    new SelectList(storageManagement.getAllCompanyStorage(companybilgi.CompanyID), "StorageID", "StorageName", 0);
                    ViewBag.cargo = cargoManagement.getAllCargo();

                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }

            }
            else
            {
                return RedirectToAction("Storage", "AdminStorage");
            }


        }
        [HttpPost]
        public ActionResult Product(HttpPostedFileBase myfile, string productname = "", int oldprice = -1, int newprice = -1, int ddlCategory = -1, int productinstock = -1, int criticalstock = -1, int ddlStorage = -1, int cargopay = -1, string productdetail = "", int ddlCargo = -1)
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
            ViewBag.categori = categoryManagement.GetAllCategory();
            ViewBag.storage = storageManagement.getAllCompanyStorage(companybilgi.CompanyID);
            ViewBag.cargo = cargoManagement.getAllCargo();
            if (ViewBag.storage == null)
            {

                return RedirectToAction("NoStorage", "AdminProductController");
            }
            else
            {
                if (productname == "" || oldprice < 0 || newprice < 0 || productinstock < 0 || ddlStorage < 0 || ddlCargo < 0 || myfile == null || ddlCategory < 0 || criticalstock < 0 || cargopay < 0 || productdetail == "")
                {

                    Session["hata"] = "Alanlar Boş Geçilemez";
                    return RedirectToAction("Product", "AdminProduct");
                }
                else
                {
                    if (myfile != null && !string.IsNullOrEmpty(myfile.FileName))
                    {
                        Guid key = Guid.NewGuid();
                        myfile.SaveAs(Server.MapPath("~/Files/ProductImage/" + key + myfile.FileName));
                        resimyolu = "/Files/ProductImage/" + key + myfile.FileName;
                    }
                    productManagement.addProduct(productname, oldprice, newprice, ddlCategory, productinstock, criticalstock, ddlStorage, companybilgi.CompanyID, cargopay, productdetail, ddlCargo, resimyolu);
                    Session["hata"] = "Urunler Basarı ile Eklendi";
                    return RedirectToAction("Product", "AdminProduct");
                }
            }


        }

        public ActionResult NoStorage()
        {
            return View();
        }
    }
}