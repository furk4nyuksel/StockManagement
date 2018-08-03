using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockManagementOperation
{
    public class OrderManagement
    {
        StockEntities db;
        EmailManagement emailmanagement;
        CriticalStockManagement criticalStockmanagemet;
        public OrderManagement()
        {
            db = new StockEntities();
            emailmanagement = new EmailManagement();
            criticalStockmanagemet = new CriticalStockManagement();


        }


        public void AddOrder(string cartname, string cartnumber, string cartexp, int cartsec, int Adressid, int UserID)
        {

            PayInfo pay = new PayInfo
            {
                UserID = UserID,
                CardNumber = cartnumber,
                NameOnCard = cartname,
                Expiration = cartexp,
                SecurityCode = cartsec,
            };

            db.PayInfo.Add(pay);
            db.SaveChanges();

            var payid = pay.PayID;


            var sepet = (from i in db.UserCart where i.IsActive.Value && i.UserID == UserID select i).ToList();

            var gelen = (double)sepet.Sum(s => s.Quantity * s.Products.NewPrice);


            Order order = new Order
            {
                AddressID = Adressid,
                CreDate = DateTime.Now,
                UserID = UserID,
                IsActive = true,
                TotalPrice = gelen,
                PayInfoID = payid
            };
            db.Order.Add(order);
            db.SaveChanges();

            var gelenorderid = order.OrderID;
            foreach (var sepettekiurunler in sepet)
            {
                var gelenproduct = (from i in db.Products where i.ProductID == sepettekiurunler.ProductID select i).SingleOrDefault();

                gelenproduct.ProductStock -= sepettekiurunler.Quantity;

                db.SaveChanges();
                OrderDetails OrderDetail = new OrderDetails
                {
                    OrderID = gelenorderid,
                    ProductID = sepettekiurunler.ProductID,
                    IsActive = false,
                    Quantity = sepettekiurunler.Quantity,
                    Price = sepettekiurunler.Quantity * sepettekiurunler.Products.NewPrice,

                };
                db.OrderDetails.Add(OrderDetail);
                sepettekiurunler.IsActive = false;
                db.SaveChanges();
                var odenenfiyat = OrderDetail.Price;

                Guid randomkey = Guid.NewGuid();


                UpdateInvoiceRandomKey(gelenorderid, randomkey.ToString());



                emailmanagement.Mail(sepettekiurunler.AppUsers.Email, "Sipariş Verildi", "Merhabalar Siparişleriniz Alındı Yakın Zamanda Kargoya Verilip Size Gönderilecek.   </br> </br> </br>  " + sepettekiurunler.Quantity + " Tane  Ürün Olarak " + sepettekiurunler.Products.ProductName + " </br> </br> Toplam Ödeyeceğiniz Tutar  " + odenenfiyat + "₺  <a href='http://localhost:58166/Pdf/GetInvoice/?randomkeyy=" + randomkey + "'>Faturayı Pdf Indir</a>");
                emailmanagement.Mail(sepettekiurunler.Products.Company.AppUsers.Email, "Ürün Satıldı", "Merhaba Ürünlerinizden " + sepettekiurunler.Products.ProductName + "  " + sepettekiurunler.Quantity + " Adet Satıldı Lütfen Yakın Bir Zaman da Kargoya Veriniz");

                criticalStockmanagemet.GetCriticalProduct(gelenproduct.ProductID);




            }


        }


        public List<Order> GetOrderUser(int userid)
        {

            var Ordertablosu = (from i in db.Order where i.UserID == userid && i.IsActive == true select i).ToList();

            return (Ordertablosu);


        }



        public int LastOrderID(int userid)
        {
            return (from i in db.Order orderby i.CreDate descending where i.UserID == userid && i.IsActive.Value select i.OrderID).Take(1).Max(s => s);
        }


        public Order Order(string guidkey)
        {
            return (from i in db.Order orderby i.CreDate descending where i.OrderInvoice == guidkey && i.IsActive == true select i).SingleOrDefault();
        }

        public List<OrderDetails> GetAllOrderDetails(string guidkey)
        {
            return (from i in db.OrderDetails where i.Order.OrderInvoice.Equals(guidkey) && i.IsActive == false select i).ToList();
        }

        public double LastOrderTotalAmount(string guidkey)
        {
            return (from i in db.Order where i.OrderInvoice.Equals(guidkey) select i.TotalPrice.Value).SingleOrDefault();
        }
        public int LastOrderTotalQuantity(string guidkey)
        {
            return (from i in db.OrderDetails where i.Order.OrderInvoice.Equals(guidkey) select i.Quantity.Value).Sum();
        }
        public Order OrderControl(string guidkey)
        {

            return (from i in db.Order where i.IsActive.Value && i.OrderInvoice.Equals(guidkey) select i).SingleOrDefault();
        }

        public bool UserOrderDownloadControl(int orderid, int userid)
        {
            return (from i in db.Order where i.OrderID == orderid && i.UserID == userid select i).Any();
        }


        public void UpdateInvoiceRandomKey(int orderID, string guiddatarandom)
        {
            var gelenorderID = (from i in db.Order where i.OrderID == orderID select i).SingleOrDefault();


            gelenorderID.OrderInvoice = guiddatarandom;

            db.SaveChanges();


        }



        //public List<OrderDetails> GetAllOrderDetails(int orderID)
        //{
        //    return (from i in db.OrderDetails where i.OrderID == orderID && i.IsActive == false select i).ToList();
        //}

        //public double LastOrderTotalAmount(int orderID)
        //{
        //    return (from i in db.Order where i.OrderID == orderID select i.TotalPrice.Value).SingleOrDefault();
        //}
        //public int LastOrderTotalQuantity(int orderID)
        //{
        //    return (from i in db.OrderDetails where i.Order.OrderID == 9 select i.Quantity.Value).Sum();
        //}
        //public int OrderControl(int userid)
        //{

        //    return (from i in db.Order where i.UserID == userid && i.IsActive.Value select i.OrderID).Take(1).SingleOrDefault();
        //}


        public List<OrderDetails> getCompanyOrder(int Companyid)
        {

            return (from i in db.OrderDetails where i.Products.CompanyID == Companyid select i).ToList();


        }

        public List<OrderDetails> GetOrderDetailsMain(int orderid, int userid)
        {
            return (from i in db.OrderDetails where i.OrderID == orderid && i.Order.UserID == userid select i).ToList();
        }
        public Order GetOrderNumberData(int orderid, int userid)
        {
            return (from i in db.Order where i.OrderID == orderid select i).SingleOrDefault();
        }
        public bool GetAllOrderAnycontrol(int userid)
        {
            return (from i in db.Order where i.UserID == userid select i).Any();
        }


    }
}
