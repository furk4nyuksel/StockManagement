using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class UserCartManagement
    {
        StockEntities db;

        public UserCartManagement()
        {
            db = new StockEntities();

        }

        public List<UserCart> GetUserCart(int id)
        {

            return (from i in db.UserCart where i.UserID == id && i.IsActive.Value select i).ToList();

        }

        public int GetUserCartCount(int id)
        {
            int sonuc = 0;
            try
            {

                sonuc = (from i in db.UserCart where i.UserID == id && i.IsActive.Value select i.Quantity.Value).Sum(t => t);

            }
            catch
            {

                sonuc = 0;
            }

            return (sonuc);


        }
        public float GetUserCartPay(int id)
        {
            var sorgu = (from i in db.UserCart
                         where i.IsActive.Value && i.UserID == id
                         select new
                         {
                             TotalPrice = i.Quantity * i.Products.NewPrice


                         }).Sum(s => s.TotalPrice);


            if (sorgu == 0)
            {
                return (0);

            }

            else
            {
                if (sorgu == null)
                {
                    return (0);
                }
                return (float)sorgu.Value;
            }
        }

        public void AddUserCart(int productid, int count, int userid)
        {

            var sorgu = (from i in db.Products where count <= i.ProductStock && i.IsActive.Value && i.ProductID == productid select i).Any();

            var sorgu2 = (from i in db.UserCart where i.IsActive.Value && i.ProductID == productid && i.UserID == userid select i).Any();

            var sorgu3 = (from i in db.Products where i.Company.UserID != userid && i.ProductID == productid select i).SingleOrDefault();
            if (sorgu)
            {


                if (!sorgu2)
                {

                    if (sorgu3 != null)
                    {


                        //insert
                        UserCart Kaydet = new UserCart
                        {
                            UserID = userid,
                            CreDate = DateTime.Now,
                            ProductID = productid,
                            Quantity = count,
                            IsActive = true,

                        };

                        db.UserCart.Add(Kaydet);
                        db.SaveChanges();
                    }

                }
                else
                {
                    //update
                    var sepet = (from i in db.UserCart where i.UserID == userid && i.IsActive.Value && i.ProductID == productid select i).SingleOrDefault();

                    if (count < sepet.Products.ProductStock && sepet.Quantity < sepet.Products.ProductStock)
                    {
                        sepet.Quantity += count;

                        db.SaveChanges();
                    }

                }



            }


        }

        public string DropUserCart(int id, int userid)
        {
            var sorgu = (from i in db.UserCart where i.UserCartID == id && i.IsActive.Value && i.UserID == userid select i).SingleOrDefault();

            if (sorgu != null)
            {
                sorgu.IsActive = false;
                db.SaveChanges();
                return ("Ürün Silindi");
            }
            else
            {
                return ("Ürün Silinemedi");
            }


        }

        public string DecreaseUserCart(int id, int userid)
        {
            var sorgu = (from i in db.UserCart where i.UserCartID == id && i.IsActive.Value && i.UserID == userid select i).SingleOrDefault();

            if (sorgu.Quantity > 1)
            {
                sorgu.Quantity -= 1;
                db.SaveChanges();
                return ("Ürün Miktarı Bir Eksiltildi");
            }
            else
            {
                return ("Daha Fazla Ürün Miktarı Eksiltemezsiniz");
            }


        }
        public string IncreaseUserCart(int usercartid, int userid)
        {
            int count = 1;
            var sepetsorgu = (from i in db.UserCart where i.UserCartID == usercartid && i.IsActive.Value select i).SingleOrDefault();

            var kalanstock = (from i in db.Products where i.ProductID == sepetsorgu.ProductID select i).SingleOrDefault();

            if (kalanstock.ProductStock > sepetsorgu.Quantity)
            {

                sepetsorgu.Quantity += count;

                db.SaveChanges();
                return ("");
            }
            else
            {
                return "Miktar Arttırılamıyor Ürünün Stok Miktarını Aşıyor ";
            }

        }

        public void ProductActiveControl(int userid)
        {
            var sorgu = (from i in db.UserCart where i.UserID == userid where i.Products.IsActive == false select i).ToList();

            if (sorgu != null)
            {
                foreach (UserCart boskayit in sorgu)
                {
                    boskayit.IsActive = false;
                    db.SaveChanges();
                }
            }



        }


        public bool UserCartAnyControl(int userid)
        {
            return (from i in db.UserCart where i.UserID == userid && i.IsActive.Value select i).Any();
        }
    }
}
