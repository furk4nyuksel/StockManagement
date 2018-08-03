using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminSliderController : Controller
    {
        SliderManagement sliderManagement;
        AppUsers gelen;

        // GET: Admin/AdminSlider

        public AdminSliderController()
        {
            sliderManagement = new SliderManagement();
        }
        public ActionResult Slider()
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
                var allSlider = sliderManagement.GetAllSlider();
                return View(allSlider);
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
        public ActionResult AddSlider(HttpPostedFileBase myFile, string slidername = "", string photourl = "", int ordernum = -1)
        {
            string resimyolu;
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
                if (slidername != "" || photourl != "" || ordernum > 0)
                {
                    if (myFile != null && !string.IsNullOrEmpty(myFile.FileName))
                    {
                        Guid key = Guid.NewGuid();
                        myFile.SaveAs(Server.MapPath("~/Files/SliderImage/" + key + myFile.FileName));
                        resimyolu = "/Files/SliderImage/" + key + myFile.FileName;
                        Session["hata"] = sliderManagement.addSlider(slidername, resimyolu, photourl, ordernum);
                        return RedirectToAction("Slider", "AdminSlider");
                    }
                    Session["hata"] = "Alanlar boş geçilemez";
                    return RedirectToAction("Slider", "AdminSlider");
                }
                else
                {
                    Session["hata"] = "Alanlar boş geçilemez";
                    return RedirectToAction("Slider", "AdminSlider");
                }

            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        public ActionResult DeleteSlider(int id)
        {
            if (Session["Login"] != null)
            {
                gelen = (AppUsers)Session["Login"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            sliderManagement.deleteSlider(id);
            Session["hata"] = "Basarıyla Silindi";
            return RedirectToAction("Slider", "AdminSlider");
        }
    }
}