using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;
using StockManagement.Areas.Admin.Models;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminLayoutController : Controller
    {
        // GET: Admin/AdminLayout
        CompanyManagement companyManagement;
        CompanyIndexManagement companyIndexManagemet;
        AdminIndexManagement adminIndexManagement;
        AppUsers gelen;
        Company companybilgi;
        public AdminLayoutController()
        {
            companyManagement = new CompanyManagement();
            companyIndexManagemet = new CompanyIndexManagement();
            adminIndexManagement = new AdminIndexManagement();
        }
        public ActionResult Photo()
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status.Value == 2)
                {
                    return View(companyManagement.getCompany(gelen.UserID));
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

        }

        public ActionResult Admin()
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                AdminIndexModel adminIndexVeri = new AdminIndexModel();
                adminIndexVeri.toplamgoruntulenme = adminIndexManagement.ProductView();
                adminIndexVeri.toplamuye = adminIndexManagement.TotalUser();
                adminIndexVeri.toplamsirket = adminIndexManagement.TotalCompany();
                adminIndexVeri.toplamurun = adminIndexManagement.TotalProduct();
                adminIndexVeri.gunluksatıs = adminIndexManagement.getDailySales();
                adminIndexVeri.aylıksatıs = adminIndexManagement.getMonthlySales();
                adminIndexVeri.yıllıksatıs = adminIndexManagement.getYearlySales();
                adminIndexVeri.totalsatıs = adminIndexManagement.getTotalSales();
                return View(adminIndexVeri);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        public ActionResult Company()
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
                CompanyIndexModel companyındexveri = new CompanyIndexModel();
                companyındexveri.toplamgoruntulenme = companyIndexManagemet.CompanyProductView(gelen.Company.First().CompanyID);
                companyındexveri.gunluksatıs = companyIndexManagemet.getDailySales(gelen.Company.First().CompanyID);
                companyındexveri.aylıksatıs = companyIndexManagemet.getMonthlySales(gelen.Company.First().CompanyID);
                companyındexveri.yıllıksatıs = companyIndexManagemet.getYearlySales(gelen.Company.First().CompanyID);
                companyındexveri.totalsatıs = companyIndexManagemet.getTotalSales(gelen.Company.First().CompanyID);
                companyındexveri.depolar = companyIndexManagemet.getStorage(gelen.Company.First().CompanyID);
                return View(companyındexveri);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { @area = "" });
            }
        }



    }
}