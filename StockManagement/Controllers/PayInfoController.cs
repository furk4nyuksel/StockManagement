using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagement.Models;
using StockManagementOperation;
namespace StockManagement.Controllers
{
    public class PayInfoController : Controller
    {
        // GET: PayInfo
        UserCartManagement UsercartManagement;
        PayInfoManagement payinfoManagement;
        OrderManagement OrderManagement;
        public PayInfoController()
        {
            UsercartManagement = new UserCartManagement();
            payinfoManagement = new PayInfoManagement();
            OrderManagement = new OrderManagement();
        }

        public ActionResult Index(int ddladress = 0)
        {
            if (Session["Login"] != null)
            {

                if (ddladress == 0)
                {
                    if (Session["addresid"] != null)
                    {
                        ddladress = (int)Session["addresid"]; //hatalı işlemde geri gitmesini engelliyor adrese geldigi için
                    }

                }




                if (ddladress > 0)
                {
                    AppUsers gelen = (AppUsers)Session["Login"];
                    int gelenid = gelen.UserID;

                    if (payinfoManagement.PayInfoUserCartControl(gelenid))
                    {

                        ViewBag.adressid = ddladress;
                        UserCartModelList model = new UserCartModelList();
                        model.Pay = UsercartManagement.GetUserCartPay(gelenid);
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Adress");
                    }

                }
                else
                {

                    string hata = "Hatalı Seçim Yaptınız";
                    Session.Add("hataadres", hata);
                    return RedirectToAction("Index", "Adress");
                }



            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult PayAndFinish(string cartname, string cartnumber, string cartexp, int cartsec = 0, int addresid = 0)
        {
            if (Session["Login"] != null)
            {

                AppUsers gelen = (AppUsers)Session["Login"];
                int gelenid = gelen.UserID;
                string hata;
                string cartsecstring = cartsec.ToString();


                if (addresid > 0 && !string.IsNullOrEmpty(cartname) && !string.IsNullOrEmpty(cartexp))
                {

                    if (cartsecstring.Length >= 4 && cartsecstring.Length <= 6)
                    {
                        if (cartnumber.Length >= 16 && cartnumber.Length <= 16)
                        {
                            if (cartexp.Length >= 3 && cartexp.Length <= 7)
                            {

                                OrderManagement.AddOrder(cartname, cartnumber, cartexp, cartsec, addresid, gelenid);
                                var orderid = OrderManagement.LastOrderID(gelenid);

                                Session.Add("OrderID", orderid);
                                return RedirectToAction("PayConfirmation", "PayInfo", new { @id = 1 });


                            }
                            else
                            {
                                hata = "Son Kullanma süreniz hatalı";
                                Session.Add("payeror", hata);
                                Session.Add("addresid", addresid);
                                return RedirectToAction("Index", "PayInfo");
                            }
                        }
                        else
                        {
                            hata = "Kart numaranız eksik veya hatalı";
                            Session.Add("payeror", hata);
                            Session.Add("addresid", addresid);
                            return RedirectToAction("Index", "PayInfo");
                        }

                    }
                    else
                    {
                        hata = "güvenlik kodunuz yanlış";
                        Session.Add("payeror", hata);
                        Session.Add("addresid", addresid);
                        return RedirectToAction("Index", "PayInfo");
                    }



                }
                else
                {
                    hata = "Boş Veya Eksik Kısımlar Bulunmakta";
                    Session.Add("payeror", hata);
                    Session.Add("addresid", addresid);
                    return RedirectToAction("Index", "PayInfo");
                }


            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }
        }


        public ActionResult PayConfirmation(int id = 0)
        {

            if (id == 1)
            {

                if (Session["Login"] != null)
                {

                    AppUsers gelenuser = (AppUsers)Session["Login"];
                    int gelenid = gelenuser.UserID;

                    if (Session["OrderID"] != null)
                    {
                        int orderid = (int)Session["OrderID"];

                        return View(OrderManagement.GetOrderNumberData(orderid, gelenid));
                    }
                    else
                    {
                        ViewBag.Hata = "Ödeme Kaydınız Bulunmamakta";
                        return View();
                    }

                }
                else
                {
                    return RedirectToAction("LoginUser", "Home");
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "Home");
            }

        }





    }
}