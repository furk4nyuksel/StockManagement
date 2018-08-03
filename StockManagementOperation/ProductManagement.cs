using System;
using System.Collections.Generic;
using System.Linq;

namespace StockManagementOperation
{
    public class ProductManagement
    {

        StockEntities db;
        public ProductManagement()
        {

            db = new StockEntities();
        }


        public List<Products> GetAllProduct()
        {
            return (from i in db.Products where i.IsActive.Value && i.ProductStock > 0 && i.Company.CompanyID > 0 select i).ToList();

        }

        public ProductDetails GetProductDetails(int id)
        {
            return (from i in db.ProductDetails where i.ProductID == id && i.Products.IsActive.Value && i.Products.Company.IsActive.Value && i.Products.Company.CompanyID > 0 select i).SingleOrDefault();
        }
        public List<Products> GetCategoryProduct(int id)
        {
            return (from i in db.Products where i.IsActive.Value && i.ProductStock > 0 && i.CategoryID == id && i.Company.CompanyID > 0 select i).ToList();
        }
        public string ProductDetailViewIncrease(int id) ///view
        {
            var gelen = (from i in db.ProductDetails where i.Products.IsActive.Value && i.Products.ProductID == id select i).SingleOrDefault();
            if (gelen != null)
            {
                gelen.ProductView += 1;
                db.SaveChanges();
                return ("");
            }
            else
            {
                return ("Ürün Sayısı Arttırılamadı Stoğu Aşıyor");
            }

        }



        public void addProduct(string productname, int oldprice, int newprice, int ddlCategory, int productinstock, int criticalstock, int ddlStorage, int companyid, int cargopay, string productdetail, int ddlCargo, string myfile)
        {
            Products eklenecekproduct = new Products { ProductName = productname, OldPrice = oldprice, NewPrice = newprice, CategoryID = Convert.ToByte(ddlCategory), ProductStock = productinstock, CriticalStock = criticalstock, StorageID = ddlStorage, CompanyID = companyid, IsActive = false, CargoPay = cargopay };
            db.Products.Add(eklenecekproduct);
            db.SaveChanges();
            ProductDetails eklenecekdetay = new ProductDetails { ProductDetailText = productdetail, ProductID = eklenecekproduct.ProductID, CargoID = ddlCargo, ProductView = 1 };
            db.ProductDetails.Add(eklenecekdetay);
            db.SaveChanges();
            ProductPhotograph eklenecekphoto = new ProductPhotograph { ProductID = eklenecekproduct.ProductID, PhotoSource = myfile };
            db.ProductPhotograph.Add(eklenecekphoto);
            db.SaveChanges();
        }

        public string productConfirm(int id)
        {
            var kontrol = (from i in db.Products where i.ProductID == id && i.Company.CompanyID > 0 select i).Count();
            if (kontrol > 0)
            {
                var degisecekveri = (from i in db.Products where i.ProductID == id && i.Company.CompanyID > 0 select i).SingleOrDefault();
                degisecekveri.IsActive = true;
                db.SaveChanges();
                return "Onaylama Basarılı";
            }
            else
            {
                return "Onaylama Basarısız";
            }
        }

        public List<Products> GetAllDeactiveProduct()
        {
            return (from i in db.Products where i.IsActive.Value == false && i.Company.IsActive == true select i).ToList();

        }
        public List<Products> GetAllCompanyActiveProduct(int id)
        {
            return (from i in db.Products where i.IsActive.Value == true && i.CompanyID == id && i.Company.CompanyID > 0 select i).ToList();
        }



        public List<Products> GetCompanyProduct(int companyID)
        {
            return (from i in db.Products where i.IsActive.Value && i.Company.IsActive.Value && i.CompanyID == companyID && i.ProductStock > 0 && i.Company.CompanyID > 0 select i).ToList();
        }

        public bool GetCompanyProductAnyControl(int companyID)
        {
            var sorgu = (from i in db.Products where i.IsActive.Value && i.Company.IsActive.Value && i.CompanyID == companyID && i.ProductStock > 0 select i).Any();
            return sorgu;
        }
        public Products GetProductById(int id)
        {
            return (from i in db.Products where i.ProductID == id && i.Company.CompanyID > 0 select i).SingleOrDefault();
        }

        public void DeleteProductById(int id)
        {

            var silinecekproducts = (from i in db.Products where i.ProductID == id select i).ToList();
            foreach (var silinecekproduct in silinecekproducts)
            {
                db.Products.Remove(silinecekproduct);
                var silinecekphotos = (from i in db.ProductPhotograph where i.Products.ProductID == silinecekproduct.ProductID select i).ToList();
                foreach (var silinecekphoto in silinecekphotos)
                {
                    db.ProductPhotograph.Remove(silinecekphoto);
                }
                var silinecekproductdetails = (from i in db.ProductDetails where i.ProductID == silinecekproduct.ProductID select i).SingleOrDefault();
                db.ProductDetails.Remove(silinecekproductdetails);
            }
            db.SaveChanges();
        }
        public void DeleteProductByCompanyId(int companyid)
        {
            var silinecekproducts = (from i in db.Products where i.CompanyID == companyid select i).ToList();
            foreach (var silinecekproduct in silinecekproducts)
            {
                db.Products.Remove(silinecekproduct);
                var silinecekphotos = (from i in db.ProductPhotograph where i.Products.ProductID == silinecekproduct.ProductID select i).ToList();
                foreach (var silinecekphoto in silinecekphotos)
                {
                    db.ProductPhotograph.Remove(silinecekphoto);
                }
                var silinecekproductdetails = (from i in db.ProductDetails where i.ProductID == silinecekproduct.ProductID select i).SingleOrDefault();
                db.ProductDetails.Remove(silinecekproductdetails);
                var silinecekproductmainpage = (from i in db.ProductMainPage where i.ProductID == silinecekproduct.ProductID select i).SingleOrDefault();
                if (silinecekproductmainpage != null)
                {
                    db.ProductMainPage.Remove(silinecekproductmainpage);
                }
                var silineceksepeturunleri = (from i in db.UserCart where i.Products.CompanyID == companyid select i).ToList();
                foreach (var item in silineceksepeturunleri)
                {
                    if (silineceksepeturunleri != null)
                    {
                        db.UserCart.Remove(item);
                    }
                }
                var silinecekorderdetails = (from i in db.OrderDetails where i.ProductID == silinecekproduct.ProductID select i).ToList();
                foreach (var item in silinecekorderdetails)
                {
                    if (silinecekorderdetails != null)
                    {
                        db.OrderDetails.Remove(item);
                    }
                }

            }
            db.SaveChanges();

        }

        public string UpdateProductInfo(Products product, ProductDetails productDetails)
        {
            var degisecekproduct = (from i in db.Products where i.ProductID == product.ProductID select i).Single();
            degisecekproduct.ProductName = product.ProductName;
            degisecekproduct.OldPrice = product.OldPrice;
            degisecekproduct.NewPrice = product.NewPrice;
            degisecekproduct.ProductStock = product.ProductStock;
            degisecekproduct.CriticalStock = product.CriticalStock;
            degisecekproduct.CargoPay = product.CargoPay;
            degisecekproduct.StorageID = product.StorageID;
            db.SaveChanges();

            var degisecekdetails = (from i in db.ProductDetails where i.ProductID == degisecekproduct.ProductID select i).Single();
            degisecekdetails.ProductDetailText = productDetails.ProductDetailText;
            degisecekdetails.CargoID = productDetails.CargoID;
            db.SaveChanges();
            return "";

        }



        public string AddPic(int id, string myfile)
        {

            ProductPhotograph eklenecekphoto = new ProductPhotograph();
            eklenecekphoto.PhotoSource = myfile;
            eklenecekphoto.ProductID = id;
            db.ProductPhotograph.Add(eklenecekphoto);
            db.SaveChanges();
            return "basarılı";
        }

        public string DeletePic(int id, string myfile)
        {

            return "";
        }

        public List<OrderDetails> GetTopSellingProduct()
        {
            return (from i in db.OrderDetails orderby i.Quantity descending where i.Products.IsActive.Value && i.Products.Company.IsActive.Value && i.Products.ProductStock > 0 && i.Products.Company.CompanyID > 0 select i).Take(6).ToList();
        }

        public List<ProductDetails> GetTopViewProduct()
        {
            return (from i in db.ProductDetails orderby i.ProductView descending where i.Products.IsActive.Value && i.Products.Company.IsActive.Value && i.Products.ProductStock > 0 && i.Products.CompanyID > 0 select i).Take(6).ToList();
        }

        public List<Products> GetLastClickCategoryProduct(int categoryid)
        {
            return (from i in db.Products where i.IsActive.Value && i.Company.IsActive.Value && i.ProductStock > 0 && i.CategoryID == categoryid && i.Company.CompanyID > 0 select i).Take(6).ToList();
        }

        public List<ProductPhotograph> getAllProductPictures(int productid)
        {
            return (from i in db.ProductPhotograph where i.ProductID == productid && i.Products.Company.CompanyID > 0 select i).ToList();
        }

        public string getproductMap(int photoid)
        {
            return (from i in db.ProductPhotograph where i.PhotoID == photoid && i.Products.Company.CompanyID > 0 select i.PhotoSource).SingleOrDefault();
        }

        public void deleteProductPic(string psource)
        {
            var silinecekresim = (from i in db.ProductPhotograph where i.PhotoSource == psource select i).SingleOrDefault();
            db.ProductPhotograph.Remove(silinecekresim);
            db.SaveChanges();
        }

        public ProductDetails ProductDetailsActiveControl(int productid)
        {
            var sorgu = (from i in db.ProductDetails where i.ProductID == productid && i.Products.Company.IsActive.Value && i.Products.IsActive.Value && i.Products.Company.CompanyID > 0 select i).SingleOrDefault();
            return (sorgu);
        }

        public List<ProductMainPage> GetProductPremium()
        {
            return (from i in db.ProductMainPage where i.ProductID == i.Products.ProductID && i.Products.IsActive.Value && i.Products.Company.IsActive.Value && i.Products.ProductStock > 0 && i.Products.Company.CompanyID > 0 && i.IsActive.Value select i).ToList();
        }

        public List<ProductMainPage> getAllPremiumProduct()
        {
            return (from i in db.ProductMainPage where i.Products.Company.CompanyID > 0 select i).ToList();
        }


        public void AddPremiumProduct(int productid)
        {
            ProductMainPage productMainPage = new ProductMainPage();
            productMainPage.ProductID = productid;
            productMainPage.IsActive = false;
            db.ProductMainPage.Add(productMainPage);
            db.SaveChanges();
        }

        public void ActiveProductPremium(int productid)
        {
            var premiumolacakproduct = (from i in db.ProductMainPage where i.ProductID == productid select i).SingleOrDefault();
            premiumolacakproduct.IsActive = true;
            db.SaveChanges();
        }

        public void DeleteProductPremium(int productid)
        {
            var premiumiptalolacakproduct = (from i in db.ProductMainPage where i.ProductID == productid select i).SingleOrDefault();
            db.ProductMainPage.Remove(premiumiptalolacakproduct);
            db.SaveChanges();
        }

        public List<Products> getAllProductifnotPremium(int companyid)
        {
            return (from i in db.Products where i.ProductID != i.ProductMainPage.FirstOrDefault().ProductID && i.IsActive.Value && i.CompanyID == companyid && i.Company.CompanyID > 0 select i).ToList();
        }

    }
}
