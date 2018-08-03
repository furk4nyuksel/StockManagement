using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{

    public class AdminController : Controller
    {
        CompanyManagement companyManagement;
        UserManagement userManagement;
        public AdminController()
        {
            companyManagement = new CompanyManagement();
            userManagement = new UserManagement();

        }
        AppUsers gelen;


        public ActionResult Index()
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                int gelenid = gelen.UserID;

                Session.Add("Login", userManagement.GetUser(gelenid));

                var companysorgu = companyManagement.getCompany(gelenid);
                if (gelen.Status.Value == 3)
                {
                    return View();
                }
                else
                {
                    if (companysorgu != null)
                    {
                        if (companysorgu.IsActive.Value)
                        {
                            Session.Add("Company", companysorgu);
                            TempData["Hata"] = "";
                            Session.Add("hata", "");
                            return View();
                        }
                        else
                        {
                            return RedirectToAction("AccountInfo", "Admin");
                        }
                    }
                    else
                    {
                        return RedirectToAction("CompanyRegister", "Admin");
                    }
                }


            }
            else
            {
                return RedirectToAction("LoginUser", "Home", new { area = "" });
            }
        }


        [HttpGet]
        public ActionResult CompanyRegister()
        {

            if (Session["Login"] != null)
            {
                gelen = (StockManagementOperation.AppUsers)Session["Login"];
                var companycontrol = companyManagement.getCompany(gelen.UserID);
                if (companycontrol != null)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            if (Session["Login"] == null)
            {
                return RedirectToAction("LoginUser", "Home", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult CompanyRegister(HttpPostedFileBase myfile, string companyname = "", string bankno = "")
        {
            if (Session["Login"] != null)
            {
                gelen = (StockManagementOperation.AppUsers)Session["Login"];
            }
            if (Session["Login"] == null)
            {
                return RedirectToAction("LoginUser", "Home", new { area = "" });
            }
            var companycontrol = companyManagement.getCompany(gelen.UserID);
            if (companycontrol != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            if (companyname != "" && bankno != "" && (myfile != null && !string.IsNullOrEmpty(myfile.FileName)))
            {
                Guid key = Guid.NewGuid();
                myfile.SaveAs(Server.MapPath("~/Files/CompanyPhoto/" + key + myfile.FileName));
                var yol = Server.MapPath("~/Files/CompanyPhoto/" + key + myfile.FileName);

                string kayıt = companyManagement.addCompany(gelen.UserID, companyname, bankno, yol, myfile);//TODO RESİM GONDERİLMEDİ****************************************
                if (kayıt == "Kayıt Tamamlandı")
                {
                    return RedirectToAction("AccountInfo", "Admin");
                }
                else
                {
                    ViewBag.sonuc = kayıt;
                    return View();
                }
            }
            else
            {
                Session["hata"] = "Alanlar Bos Olamaz";
                return View();
            }


        }
        public ActionResult AccountInfo()
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                int gelenid = gelen.UserID;
                var companysorgu = companyManagement.getCompany(gelenid);
                var guncellenecekuser = userManagement.GetUser(gelenid);
                if (companysorgu != null)
                {
                    if (guncellenecekuser.Status == 2)
                    {
                        Session.Add("Company", companysorgu);
                        Session.Add("Login", guncellenecekuser);
                        return RedirectToAction("Index", "Admin", new { area = "" });
                    }

                }
                return View();
            }
            else
            {
                return RedirectToAction("LoginUser", "Home", new { area = "" });

            }

        }
    }
}