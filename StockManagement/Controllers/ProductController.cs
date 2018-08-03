using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;
namespace StockManagement.Controllers
{
    public class ProductController : Controller
    {
        ProductManagement productmanagement;
        SearchManagement searchmanagement;
        public ProductController()
        {
            productmanagement = new ProductManagement();
            searchmanagement = new SearchManagement();
        }

        public ActionResult Index()
        {
            return View(productmanagement.GetAllProduct());
        }

        public ActionResult ProductDetails(int id = 0)
        {

            if (id > 0)
            {
                if (productmanagement.ProductDetailsActiveControl(id) != null)
                {
                    string kontrol = productmanagement.ProductDetailViewIncrease(id);
                    Session["Sepetkontrol"] = kontrol; //Todo:Hata Kontrolü Çalışmıyor Bak
                    return View(productmanagement.GetProductDetails(id));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {

                return RedirectToAction("Index", "Home");
            }


        }

        public ActionResult CategoryProduct(int id = 0)
        {
            //devam edicez

            if (id > 0)
            {
                HttpCookie cookie = new HttpCookie("Category");
                cookie["CategoryID"] = id.ToString();
                cookie.Expires = DateTime.Now.AddDays(15);
                Response.Cookies.Add(cookie);


                return View(productmanagement.GetCategoryProduct(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult SearchProductMenu(string arama)
        {
            if (string.IsNullOrEmpty(arama))
            {
                ViewBag.Hata = "Hatalı Arama Yapıyorsunuz";
                return View();
            }
            else
            {
                var gelen = searchmanagement.SearchProductMenu(arama);
                if (gelen.Count == 0)
                {
                    ViewBag.Hata = "hiç bir sonuç bulunamadı";
                    return View();

                }
                else
                {
                    return View(searchmanagement.SearchProductMenu(arama));
                }



            }


        }

        [HttpPost]
        public ActionResult SearchCompanyProductMenu(int CompanyID, string arama)
        {
            if (string.IsNullOrEmpty(arama))
            {
                ViewBag.Hata = "Hatalı Arama Yapıyorsunuz";
                return View();
            }
            else
            {
                var gelen = searchmanagement.SearchCompanyProductMenu(CompanyID, arama);
                if (gelen.Count == 0)
                {
                    ViewBag.Hata = "hiç bir sonuç bulunamadı";
                    return View();

                }
                else
                {
                    return View(searchmanagement.SearchCompanyProductMenu(CompanyID, arama));
                }



            }


        }

        public ActionResult CompanyProduct(int id = 0)
        {

            if (id > 0)
            {
                if (productmanagement.GetCompanyProductAnyControl(id))
                {
                    productmanagement.GetCompanyProduct(id);

                    return View(productmanagement.GetCompanyProduct(id));
                }
                else
                {
                    return RedirectToAction("Index", "Product");
                }

            }
            else
            {
                return RedirectToAction("Index", "Product");
            }

        }

        //partitialview
        public ActionResult TopSellingProducts()
        {
            return PartialView(productmanagement.GetTopSellingProduct());
        }

        public ActionResult TopViewProducts()
        {
            return PartialView(productmanagement.GetTopViewProduct());
        }

        public ActionResult GetPremiumProduct()
        {
            return PartialView(productmanagement.GetProductPremium());
        }

        public ActionResult GetCategoryCookieProduct()
        {



            if (Request.Cookies["Category"] != null)
            {
                HttpCookie gelen = Request.Cookies["Category"];
                string id = gelen["CategoryID"];

                int gelenid = Convert.ToInt32(id);

                if (gelenid != 0)
                {
                    return PartialView(productmanagement.GetLastClickCategoryProduct(gelenid));
                }
                else
                {
                    return View();
                }
            }

            else
            {
                return View();
            }
        }

    }



}
