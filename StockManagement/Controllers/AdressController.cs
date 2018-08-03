using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;
namespace StockManagement.Controllers
{
    public class AdressController : Controller
    {
        UserManagement usermanagement;
        AdressManagement Adressmanagement;
        CountyAndCityManagement CountyandCityManagement;
        PayInfoManagement payinfoManagement;
        public AdressController()
        {
            usermanagement = new UserManagement();
            Adressmanagement = new AdressManagement();
            CountyandCityManagement = new CountyAndCityManagement();
            payinfoManagement = new PayInfoManagement();
        }
        // GET: Adress

        public ActionResult Index()
        {

            if (Session["Login"] != null)
            {
                AppUsers gelensession = (AppUsers)Session["Login"];
                int id = gelensession.UserID;

                var data = Adressmanagement.GetUserAdress(id);
                if (payinfoManagement.PayInfoUserCartControl(id))
                {
                    if (data != null)
                    {

                        ViewBag.AdressListesi = data;

                        SelectList list = new SelectList(ViewBag.AdressListesi, "AdressID", "AdressTitle");

                    }
                    else
                    {
                        return RedirectToAction("AddAdress", "Adress");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }




                return View();
            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }

        }





        [HttpGet]
        public ActionResult AddAdress()
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int id = gelenuser.UserID;
                if (payinfoManagement.PayInfoUserCartControl(id))
                {
                    var adres = CountyandCityManagement.getallCity();
                    ViewBag.city = adres;

                    SelectList list = new SelectList(adres, "CityID", "CityName");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }



            }
            else
            {
                return RedirectToAction("LoginUser", "Home");

            }
            return View();
        }
        [HttpPost]
        public ActionResult AddAdress(string AdressTitle, string AdresUzun, byte ddlcountry = 0)
        {

            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int id = gelenuser.UserID;


                if (!string.IsNullOrEmpty(AdressTitle) && !string.IsNullOrEmpty(AdresUzun) && ddlcountry > 0)
                {
                    if (Adressmanagement.AdressControl(id, AdressTitle) == null)
                    {

                        Adressmanagement.AddAdress(id, AdressTitle, AdresUzun, ddlcountry);

                        return RedirectToAction("Index", "Adress");
                    }
                    else
                    {
                        var adres = CountyandCityManagement.getallCity();
                        ViewBag.city = adres;
                        ViewBag.Hatali = "Adress İsmi Kullanımda";
                        return View();
                    }

                }
                else
                {
                    var adres = CountyandCityManagement.getallCity();
                    ViewBag.city = adres;
                    ViewBag.Hatali = "Boş Veya Seçilmemiş Kısımlar Bulunuyor Kaydetme İşlemi Yapılmadı";
                    return View();
                }


            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }

        }

        public JsonResult GetCountry(int id)
        {
            if (Session["Login"] != null)
            {

                var counties = CountyandCityManagement.getCounty(id);
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