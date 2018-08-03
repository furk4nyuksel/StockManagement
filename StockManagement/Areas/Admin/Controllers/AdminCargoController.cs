using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminCargoController : Controller
    {
        // GET: AdminCargo
        CargoManagement cargoManagement;
        AppUsers gelen;
        public AdminCargoController()
        {
            cargoManagement = new CargoManagement();
        }
        public ActionResult Cargo()
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
                var allCargo = cargoManagement.getAllCargo();
                return View(allCargo);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddCargo(string cargoname = "")
        {
            //token
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
                if (cargoname != "")
                {
                    Session["hata"] = "Cargo Ekleme Basarılı";
                    cargoManagement.addCargo(cargoname);
                    return RedirectToAction("Cargo", "AdminCargo");
                }
                else
                {
                    Session["hata"] = "Cargo Ekleme Basarısız Boş Alanları Kontrol Ediniz";
                    return RedirectToAction("Cargo", "AdminCargo");
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }


        public ActionResult DeleteCargo(int id)
        {
            //token
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status == 3)
            {
                cargoManagement.deleteCargo(id);
                return RedirectToAction("Cargo", "AdminCargo");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
    }
}