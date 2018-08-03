using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;
namespace StockManagement.Controllers
{
    public class OrderListController : Controller
    {
        OrderManagement orderManagement;
        public OrderListController()
        {
            orderManagement = new OrderManagement();
        }
        // GET: OrderList
        public ActionResult Index()
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;

                if (orderManagement.GetAllOrderAnycontrol(gelenid))
                {
                    return View(orderManagement.GetOrderUser(gelenid));
                }
                else
                {
                    ViewBag.bos = "Siparişiniz Bulunamadı";
                    return View();
                }


            }
            else
            {
                return RedirectToAction("LoginUser", "Home");

            }



        }

        public ActionResult OrderDetails(int orderid = 0)
        {
            if (orderid > 0)
            {


                if (Session["Login"] != null)
                {
                    AppUsers gelenuser = (AppUsers)Session["Login"];
                    int gelenid = gelenuser.UserID;

                    return View(orderManagement.GetOrderDetailsMain(orderid, gelenid));
                }
                else
                {
                    return RedirectToAction("LoginUser", "Home");

                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

    }
}