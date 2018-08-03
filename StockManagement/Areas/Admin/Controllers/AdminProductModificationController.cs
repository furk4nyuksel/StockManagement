using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminProductModificationController : Controller
    {
        AppUsers gelen;
        Company companybilgi;
        ProductManagement productManagement;
        StorageManagement storageManagement;
        CargoManagement cargoManagement;
        CategoryManagement categoryManagement;

        public AdminProductModificationController()
        {
            categoryManagement = new CategoryManagement();
            storageManagement = new StorageManagement();
            cargoManagement = new CargoManagement();
            productManagement = new ProductManagement();
        }

        public ActionResult ProductSelect()
        {

            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (gelen.Status.Value == 2)
            {
                return View(productManagement.GetAllCompanyActiveProduct(companybilgi.CompanyID));
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }

        [HttpGet]
        public ActionResult ProductModification(int id)
        {

            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (productManagement.GetProductById(id) != null && companybilgi.Products.Where(s => s.ProductID == id).Count() > 0)
            {
                if (gelen.Status.Value == 2)
                {
                    StockManagement.Areas.Admin.Models.ProductModel productModel = new Models.ProductModel();
                    productModel.cargo = cargoManagement.getAllCargo();
                    productModel.categories = categoryManagement.GetAllCategory();
                    productModel.storage = storageManagement.getAllCompanyStorage(companybilgi.CompanyID);
                    productModel.product = productManagement.GetProductById(id);
                    Session["hata"] = productModel.product.ProductID;
                    return View(productModel);

                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }


        }

        [HttpPost]
        public ActionResult ProductModification(string productname = "", int oldprice = -1, int newprice = -1, int productinstock = -1, int criticalstock = -1, int ddlStorage = -1, int cargopay = -1, string productdetail = "", int ddlCargo = -1)
        {

            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (productManagement.GetProductById((int)Session["hata"]) != null && companybilgi.Products.Where(s => s.ProductID == (int)Session["hata"]).Count() > 0)
            {
                if (gelen.Status.Value == 2)
                {
                    if (productname == "" || oldprice < 0 || newprice < 0 || productinstock < 0 || ddlStorage < 0 || ddlCargo < 0 || criticalstock < 0 || cargopay < 0 || productdetail == "")
                    {

                        Session["hata"] = "Alanlar Boş Geçilemez";
                        return View();
                    }
                    else
                    {
                        Products degisecekproduct = new Products();
                        degisecekproduct.ProductID = (int)Session["hata"];
                        degisecekproduct.ProductName = productname;
                        degisecekproduct.OldPrice = oldprice;
                        degisecekproduct.NewPrice = newprice;
                        degisecekproduct.ProductStock = productinstock;
                        degisecekproduct.CriticalStock = criticalstock;
                        degisecekproduct.CargoPay = cargopay;
                        degisecekproduct.StorageID = ddlStorage;

                        ProductDetails degisecekdetay = new ProductDetails();
                        degisecekdetay.ProductDetailText = productdetail;
                        degisecekdetay.CargoID = ddlCargo;

                        productManagement.UpdateProductInfo(degisecekproduct, degisecekdetay);
                        Session["hata"] = "Urun Basarıyla Değiştirildi";
                        return RedirectToAction("ProductSelect", "AdminProductModification");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }


        public ActionResult ProductDelete(int id = -1)
        {
            /* if (Session["Login"] != null && Session["Company"] != null)
             {
                 gelen = (AppUsers)Session["Login"];
                 companybilgi = (Company)Session["Company"];
             }
             else
             {
                 return RedirectToAction("Index", "Home", new { area = "" });
             }
             if (gelen.Status.Value == 2)
             {
                 if (id>0)
                 {
                     productManagement.DeleteProductById(id);
                     Session["hata"] = "Silme İşlemi Tamamlandı";
                     return RedirectToAction("ProductSelect", "AdminModification");

                 }
                 else
                 {
                     return RedirectToAction("Index", "Admin");
                 }
             }
             else
             {
                 return RedirectToAction("Index", "Admin");
             }*/
            return View();
        }


        [HttpGet]
        public ActionResult AddPicture(int id = -1)
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if (id < 0)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                if (productManagement.GetProductById(id) != null && companybilgi.Products.Where(s => s.ProductID == id).Count() > 0)
                {
                    if (gelen.Status.Value == 2)
                    {
                        Session["hata"] = id;
                        return View(productManagement.getAllProductPictures(id));
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Admin");

                }
            }

        }

        [HttpPost]
        public ActionResult AddPicture(HttpPostedFileBase myFile)
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            if ((int)Session["hata"] > 0)
            {
                if (productManagement.GetProductById((int)Session["hata"]) != null && companybilgi.Products.Where(s => s.ProductID == (int)Session["hata"]).Count() > 0)
                {
                    string resimyolu = "";
                    if (gelen.Status.Value == 2)
                    {
                        if (myFile != null && !string.IsNullOrEmpty(myFile.FileName))
                        {
                            Guid key = Guid.NewGuid();
                            myFile.SaveAs(Server.MapPath("~/Files/ProductImage/" + key + myFile.FileName));
                            resimyolu = "/Files/ProductImage/" + key + myFile.FileName;
                            productManagement.AddPic((int)Session["hata"], resimyolu);
                            return View(productManagement.getAllProductPictures((int)Session["hata"]));
                        }
                        else
                        {
                            return RedirectToAction("AddPicture", "AdminProductModification");
                        }


                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }



        public ActionResult DeletePic(string photosource)
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (System.IO.File.Exists(Server.MapPath(photosource)))
            {
                System.IO.File.Delete(Server.MapPath(photosource));
                productManagement.deleteProductPic(photosource);
            }
            return RedirectToAction("ProductSelect", "AdminProductModification");

        }




    }
}