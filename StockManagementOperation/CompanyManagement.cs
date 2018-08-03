using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace StockManagementOperation
{
    public class CompanyManagement
    {
        StockEntities db;
        UserManagement userManagemet;
        ProductManagement productManagement;
        public CompanyManagement()
        {
            db = new StockEntities();
            userManagemet = new UserManagement();
            productManagement = new ProductManagement();
        }

        public Company getCompany(int userıd)
        {
            var control = (from i in db.Company where i.UserID == userıd select i).Any();
            if (!control)
            {
                return null;
            }
            else
            {
                return (from i in db.Company where i.UserID == userıd select i).SingleOrDefault();
            }
        }
        public string addCompany(int userıd, string companyname, string bankno, string yol, HttpPostedFileBase myfile)
        {
            var companynamecontrol = (from i in db.Company where i.CompanyName == companyname select i).Any();
            if (!companynamecontrol)//veri yoksa
            {


                Company eklenecekcompany = new Company { UserID = userıd, CompanyName = companyname, BankAccountNo = bankno, IsActive = false };
                eklenecekcompany.CompanyPhoto = new byte[myfile.ContentLength];
                myfile.InputStream.Read(eklenecekcompany.CompanyPhoto, 0, myfile.ContentLength);

                db.Company.Add(eklenecekcompany);
                db.SaveChanges();
                return "Kayıt Tamamlandı";
            }
            else
            {
                return "Basarısız Kayıt";
            }
        }

        public List<Company> getAllCompany()
        {
            return (from i in db.Company select i).ToList();
        }

        public void ActivateCompany(int id)
        {
            var aktifedilecekcompany = (from i in db.Company where i.CompanyID == id select i).SingleOrDefault();
            aktifedilecekcompany.IsActive = true;
            aktifedilecekcompany.AppUsers.Status = 2;
            db.SaveChanges();
            var urun = (from i in db.Products where i.Company.CompanyID == id select i).ToList();
            if (urun != null)
            {
                foreach (var item in urun)
                {
                    item.IsActive = true;
                    var productmainpageısactive = (from i in db.ProductMainPage where i.ProductID == item.ProductID select i).SingleOrDefault();
                    if (productmainpageısactive != null)
                    {
                        productmainpageısactive.IsActive = true;
                    }
                    db.SaveChanges();
                }
            }
            if (aktifedilecekcompany != null)
            {
                userManagemet.Mail(aktifedilecekcompany.AppUsers.Email, "Şirketiniz Onaylandı", "Merhaba " + aktifedilecekcompany.CompanyName + " Adlı Şirketiniz Kabul Edilmiştir. Başarılar Dileriz");
            }
        }
        public void DeactivateCompany(int id)
        {
            var deaktifedilecekcompany = (from i in db.Company where i.CompanyID == id select i).SingleOrDefault();
            deaktifedilecekcompany.AppUsers.Status = 1;
            deaktifedilecekcompany.IsActive = false;
            var urun = (from i in db.Products where i.Company.CompanyID == id select i).ToList();
            if (urun != null)
            {
                foreach (var item in urun)
                {
                    item.IsActive = false;
                    var productmainpageısactive = (from i in db.ProductMainPage where i.ProductID == item.ProductID select i).SingleOrDefault();
                    if (productmainpageısactive != null)
                    {
                        productmainpageısactive.IsActive = false;
                    }
                }
            }
            db.SaveChanges();
        }

        public void DeleteCompany(int id)
        {
            var silinecekCompany = (from i in db.Company where i.CompanyID == id select i).SingleOrDefault();
            silinecekCompany.AppUsers.Status = 1;
            productManagement.DeleteProductByCompanyId(id);
            db.Company.Remove(silinecekCompany);
            var silinecekpdfler = (from i in db.CompanyOrderPdf where i.CompanyID == id select i).ToList();
            if (silinecekpdfler != null)
            {
                foreach (var pdf in silinecekpdfler)
                {
                    db.CompanyOrderPdf.Remove(pdf);
                }
            }
            var silinecekstoragelar = (from i in db.Storage where i.CompanyID == id select i).ToList();
            if (silinecekstoragelar != null)
            {
                foreach (var storage in silinecekstoragelar)
                {
                    db.Storage.Remove(storage);
                }
            }
            db.SaveChanges();

        }


    }
}
