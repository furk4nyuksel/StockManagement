using StockManagementOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace StockManagement.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        UserManagement userManagement;
        UserCartManagement UserCartManagement;
        AdressManagement AdressManagement;
        CountyAndCityManagement CountandCityManagement;
        public HomeController()
        {
            userManagement = new UserManagement();
            UserCartManagement = new UserCartManagement();
            AdressManagement = new AdressManagement();
            CountandCityManagement = new CountyAndCityManagement();
        }
        public ActionResult Index()
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;
                UserCartManagement.ProductActiveControl(gelenid);//sepet hata kontrolü 0 gelirse sil diye
                return View();
            }



            if (Request.Cookies["kullanici"] != null)
            {
                HttpCookie gelen = Request.Cookies["kullanici"];
                string gelenemail = gelen["email"];
                string gelensifre = gelen["sifre"];
                var user = userManagement.LoginUser(gelenemail, gelensifre);
                Session.Add("Login", user);

                return View();
            }
            else
            {
                return View();
            }



        }
        [HttpGet]
        public ActionResult RegisterUser()
        {

            if (Session["Login"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return View();
            }



        }

        [HttpPost]
        public ActionResult RegisterUser(string UserName, string Email, string sifre1, string sifre2)
        {

            if (Session["Login"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {



                if (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(sifre1) && string.IsNullOrEmpty(sifre2))
                {
                    ViewBag.sifrehata = "Eksik bilgiler var";
                    return View();
                }
                else
                {
                    if (userManagement.AddUserEmailControl(Email) == null)

                    {

                        if (userManagement.AddUserUserNameControl(UserName) == null)
                        {



                            if (sifre1 == sifre2)
                            {
                                if (sifre1.Length < 8)
                                {
                                    ViewBag.sifrehata = "Şifreniz 8 Karakterden Kısa Olamaz";
                                    return View();
                                }
                                else
                                {
                                    userManagement.AddUser(UserName, sifre1, Email);
                                    userManagement.EmailWelcomeSend(Email, UserName);
                                }


                            }

                            else
                            {
                                ViewBag.sifrehata = "Şifreniz Hatalı";
                                return View();
                            }

                        }

                        else
                        {
                            ViewBag.sifrehata = "Kullanıcı Adı Kullanımda";
                            return View();
                        }


                    }
                    else
                    {
                        ViewBag.sifrehata = "Mail Adresi Kullanımda";
                        return View();
                    }

                    return RedirectToAction("LoginUser", "Home");
                }
            }
        }
        [HttpGet]
        public ActionResult LoginUser()
        {
            if (Session["Login"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult LoginUser(string Email, string sifre1, string sifre2, int kaydet = 0)
        {





            if (Session["Login"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (sifre1 == sifre2)
                {

                    var user = userManagement.LoginUser(Email, sifre1);
                    if (user != null)
                    {



                        if (kaydet > 0)
                        {
                            HttpCookie kullanici = new HttpCookie("kullanici");
                            kullanici["email"] = Email;
                            kullanici["sifre"] = sifre1;

                            kullanici.Expires = DateTime.Now.AddMonths(6);
                            Response.Cookies.Add(kullanici);
                            Session.Add("Login", user);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            Session.Add("Login", user);
                            return RedirectToAction("Index", "Home");
                        }








                    }
                    else
                    {
                        ViewBag.sifrehata = "Kullanıcı Adı Veya Şifre Yanlış";
                        return View();
                    }


                }
                else
                {
                    ViewBag.sifrehata = "Şifreniz Hatalı";
                    return View();
                }

            }


        }

        public ActionResult LogoutUser()
        {




            if (Request.Cookies["kullanici"] != null)
            {
                Session.Abandon();
                Response.Cookies["kullanici"].Expires = DateTime.Now.AddDays(-1);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }


        }




        public ActionResult EditUserPanel()
        {
            if (Session["Login"] != null)
            {
                return View();


            }

            else
            {
                return RedirectToAction("LoginUser", "Home");

            }
        }


        [HttpGet]
        public ActionResult EditUserInformation()
        {
            if (Session["Login"] != null)
            {
                AppUsers gelen = (AppUsers)Session["Login"];

                if (gelen.FirstName == null)
                {
                    ViewBag.Hatali = "Rica Edersek Bir Kereliğine Bilgileri Doldururmusunuz :) Bir Daha Karşınıza Çıkmayacak";
                    return View();
                }
                else
                {
                    return View();
                }



            }
            else
            {

                return RedirectToAction("LoginUser", "Home");
            }
        }
        [HttpPost]
        public ActionResult EditUserInformation(string ad, string soyad, string birthdate, string phonenumber)
        {
            if (Session["Login"] != null)
            {
                AppUsers gelen = (AppUsers)Session["Login"];
                int gelenid = gelen.UserID;



                Session.Add("Login", userManagement.GetUser(gelenid));


                DateTime tarih;
                if (ad == "")
                {
                    ViewBag.Hatali = "Boş Kısımlar Mevcut";
                    return View();
                }
                else
                {
                    if (soyad == "")
                    {
                        ViewBag.Hatali = "Boş Kısımlar Mevcut";
                        return View();
                    }
                    else
                    {
                        if (birthdate == "")
                        {
                            ViewBag.Hatali = "Boş Tarih Veya Alanlar Mevcut";
                            return View();
                        }
                        else
                        {
                            if (phonenumber.Length <= 9)
                            {
                                ViewBag.Hatali = "Eksik Telefon Numarası Mevcut";
                                return View();
                            }
                            else
                            {
                                tarih = Convert.ToDateTime(birthdate);
                                userManagement.EditUserInformation(gelenid, ad, soyad, tarih, phonenumber);
                                ViewBag.Hatali = "Kayıt Başarılı";
                                return RedirectToAction("Index", "UserCart");
                            }
                        }
                    }
                }




            }

            else
            {
                return RedirectToAction("LoginUser", "Home");

            }
        }
        [HttpGet]
        public ActionResult EditPassword()
        {
            if (Session["Login"] != null)
            {


                return View();

            }

            else
            {
                return RedirectToAction("LoginUser", "Home");

            }
        }

        [HttpPost]
        public ActionResult EditPassword(string email, string suansifre, string degis1, string degis2)
        {
            if (Session["Login"] != null)
            {
                AppUsers gelen = (AppUsers)Session["Login"];
                int gelenid = gelen.UserID;

                string gelenkontrol = userManagement.HashUserPwd(suansifre);

                if (!gelen.Password.Equals(gelenkontrol))
                {
                    ViewBag.hatali = " Şuanki Şifreniz Uymuyor";
                }

                else
                {

                    if (degis1 == degis2)
                    {
                        if (degis1.Length < 8)
                        {
                            ViewBag.hatali = "Şifreniz 8 Karakterden Kısa Olamaz";
                            return View();
                        }
                        else
                        {
                            userManagement.EditUserPassword(gelenid, email, suansifre, degis1);
                            userManagement.EmailPasswordChange(gelen.Email, gelen.UserName);
                            ViewBag.basarili = "Şifre Başarı İle Degiştirildi";
                        }

                    }
                    else
                    {
                        ViewBag.hatali = " Şifreler Eşleşmiyor";
                    }
                }



                return View();


            }

            else
            {
                return RedirectToAction("LoginUser", "Home");

            }
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            if (Session["Login"] != null)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Random = userManagement.ForgetPasswordRandomKey();

                return View();
            }





        }

        [HttpPost]
        public ActionResult ForgetPassword(string Email, string dogrulama)
        {
            if (Session["Login"] != null)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["forgetpwkey"] != null)
                {
                    string gelen = Session["forgetpwkey"].ToString();
                    if (Session["forgetpwkey"].Equals(dogrulama))
                    {
                        var sorgu = userManagement.ForgetPassword(Email, dogrulama);

                        TempData.Add("Forgetpw", sorgu);
                        return View();
                    }
                    else
                    {
                        TempData.Add("Forgetpw", "Yanlış Doğrulama Kelimesi Girildi.");
                        return View();
                    }

                }
                else
                {
                    return View();
                }

            }




        }
        [HttpGet]
        public ActionResult ForgetPasswordChange(string key)
        {
            if (Session["Login"] != null)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.key = key;
                Session.Add("key", key);
                return View();
            }

        }

        [HttpPost]
        public ActionResult ForgetPasswordChange(string key, string Email, string degis1, string degis2)
        {
            if (Session["Login"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (degis1.Equals(degis2))
                {

                    if (Session["key"] != null)
                    {
                        if (key.Equals(Session["key"].ToString()))
                        {
                            var sorgu = userManagement.ForgetChangePassword(key, Email, degis1);

                            if (sorgu != null)
                            {
                                ViewBag.basarili = sorgu;
                            }
                            else
                            {
                                ViewBag.hatali = sorgu;
                            }
                            //Todo:Burada Viewbaglari kaldırıp
                        }
                        else
                        {
                            ViewBag.hatali = "Doğrulama Kodunda Hata Var";
                        }


                    }

                    return View();
                }
                else
                {
                    ViewBag.hatali = "Şifreler Eşleşmiyor";
                    return View();
                }


            }


        }
        [HttpGet]
        public ActionResult UpdateAddressSelect()
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;

                if (AdressManagement.GetUserAdressAnyControl(gelenid))
                {
                    ViewBag.Address = AdressManagement.GetUserAdress(gelenid);
                }
                else
                {
                    ViewBag.hata = "Herhangi bir kayıt bulunamadı Eklemek İsterseniz Şipariş Ödeme Kısmında Yönlendirileceksiniz.";
                }


                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult UpdateAdressRemove(int id)
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;
                var sorgu = AdressManagement.RemoveAdress(gelenid, id);
                TempData.Add("updateadress", sorgu);
                return RedirectToAction("UpdateAddressSelect", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult AddressUpdate(int adressid = 0)
        {
            // id kontroll
            if (adressid > 0)
            {
                if (Session["Login"] != null)
                {
                    AppUsers gelenuser = (AppUsers)Session["Login"];
                    int gelenid = gelenuser.UserID;

                    if (AdressManagement.AdressControlUpdate(adressid, gelenid) != null)
                    {
                        ViewBag.adressveri = AdressManagement.AdressControlUpdate(adressid, gelenid);


                        int gelenil = (int)AdressManagement.AdressControlUpdate(adressid, gelenid).County.City.CityID;
                        var adres = CountandCityManagement.getallCity();
                        SelectList list = new SelectList(adres, "CityID", "CityName", gelenil);
                        ViewBag.city = list;




                        int gelenilce = (int)AdressManagement.AdressControlUpdate(adressid, gelenid).County.CountyID;
                        var ilce = CountandCityManagement.getCompanyCounty(gelenil);
                        SelectList list2 = new SelectList(ilce, "CountyID", "CountyName", gelenilce);

                        ViewBag.County = list2;

                        return View();
                    }
                    else
                    {
                        Session.Add("AdressController", "Adress Hatalı Farklı Bir Kişinin Adresine Erişemezsiniz");
                        return View();
                    }


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
        [HttpPost]
        public ActionResult AddressUpdate(string AdressTitle, string AdresUzun, int adressid = 0, byte ddlcountry = 0)
        {
            if (Session["Login"] != null)
            {
                AppUsers gelenuser = (AppUsers)Session["Login"];
                int gelenid = gelenuser.UserID;

                var adres = CountandCityManagement.getallCity();
                ViewBag.city = adres;

                Session.Add("UpdateAdress", AdressManagement.UpdateUserAdress(gelenid, adressid, AdressTitle, AdresUzun, ddlcountry));

                return View(AdressManagement.GetUserAdress(gelenid));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


    }
}