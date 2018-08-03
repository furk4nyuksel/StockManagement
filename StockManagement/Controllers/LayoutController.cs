using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;
namespace StockManagement.Controllers
{
    public class LayoutController : Controller
    {
        CategoryManagement catoperation;
        SliderManagement slideroperation;
        UserCartManagement usercartoperation;
        public LayoutController()
        {
            catoperation = new CategoryManagement();
            slideroperation = new SliderManagement();
            usercartoperation = new UserCartManagement();
        }

        // GET: Layout
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult Category()
        {
            ViewBag.Category = catoperation.GetAllCategory();

            return PartialView();
        }

        public ActionResult Slider()
        {
            ViewBag.Slider = slideroperation.GetAllSlider();
            return PartialView();

            //TODO:Slider dönmüyor bi ara bak buraya
        }

        public ActionResult UserCartCount()
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;

                ViewBag.UserCartCount = usercartoperation.GetUserCartCount(gelenid);


                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }



        }

        public ActionResult SearchProduct()
        {
            return View();
        }




    }
}