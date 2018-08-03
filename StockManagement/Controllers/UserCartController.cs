using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagement.Models;
using StockManagementOperation;
namespace StockManagement.Controllers
{
    public class UserCartController : Controller
    {
        UserCartManagement UsercartManagement;
        public UserCartController()
        {
            UsercartManagement = new UserCartManagement();
        }

        // GET: UserCart
        public ActionResult Index()
        {

            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];

                int gelenid = gelenuser.UserID;


                if (gelenuser.FirstName == null && gelenuser.LastName == null)
                {
                    return RedirectToAction("EditUserInformation", "Home");
                }
                else
                {
                    if (UsercartManagement.UserCartAnyControl(gelenid))
                    {
                        UsercartManagement.ProductActiveControl(gelenid);

                        ViewBag.Sepet = UsercartManagement.GetUserCart(gelenid);
                        UserCartModelList model = new UserCartModelList();

                        model.Count = UsercartManagement.GetUserCartCount(gelenid);
                        model.Pay = UsercartManagement.GetUserCartPay(gelenid);

                        return View(model);
                    }
                    else
                    {
                        ViewBag.Hatali = "Sepetenizde Hiç Ürün Bulunmuyor.";
                        return View();
                    }

                }



            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }


        }

        public ActionResult AddUserCart(int id, int count)
        {


            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];

                int gelenid = gelenuser.UserID;



                UsercartManagement.AddUserCart(id, count, gelenid);

                return RedirectToAction("ProductDetails/" + id, "Product");



            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }


        }


        public ActionResult DropUserCart(int id)
        {

            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];

                int gelenid = gelenuser.UserID;
                string kontrol = UsercartManagement.DropUserCart(id, gelenid);
                Session["Sepetkontrol"] = kontrol;
                return RedirectToAction("Index", "UserCart");


            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }

        }

        public ActionResult DecreaseUserCart(int id)
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;

                string kontrol = UsercartManagement.DecreaseUserCart(id, gelenid);
                Session["Sepetkontrol"] = kontrol;
                return RedirectToAction("Index", "UserCart");

            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }
        }


        public ActionResult IncreaseUserCart(int id)
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;

                string kontrol = UsercartManagement.IncreaseUserCart(id, gelenid);

                Session["Sepetkontrol"] = kontrol;



                return RedirectToAction("Index", "UserCart");

            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }
        }
    }
}