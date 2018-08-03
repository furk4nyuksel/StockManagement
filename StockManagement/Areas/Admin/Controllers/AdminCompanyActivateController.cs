using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminCompanyActivateController : Controller
    {
        CompanyManagement companyManagement;
        AppUsers gelen;
        public AdminCompanyActivateController()
        {
            companyManagement = new CompanyManagement();
        }

        [HttpGet]
        public ActionResult CompanyActivate()
        {
            if (Session["Login"] != null)
            { gelen = (AppUsers)Session["Login"]; }
            else
            { return RedirectToAction("Index", "Home", new { area = "" }); }

            if (gelen.Status.Value == 3)
            {
                return View(companyManagement.getAllCompany());
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }


        }


        public ActionResult CompanyActivated(int cid = -1)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status.Value == 3)
                {
                    if (cid != -1)
                    {
                        companyManagement.ActivateCompany(cid);
                        return RedirectToAction("CompanyActivate", "AdminCompanyActivate");
                    }
                    else
                    {
                        return RedirectToAction("CompanyActivate", "AdminCompanyActivate");
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


        public ActionResult CompanyDeactivated(int cid)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status.Value == 3)
                {
                    companyManagement.DeactivateCompany(cid);
                    return RedirectToAction("CompanyActivate", "AdminCompanyActivate");
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

        public ActionResult CompanyDeleted(int cid)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                if (gelen.Status.Value == 3)
                {
                    companyManagement.DeleteCompany(cid);
                    return RedirectToAction("CompanyActivate", "AdminCompanyActivate");
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