using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class UserManagement
    {
        public string HashUserPwd(string userPwd)//FURKAN
        {
            byte[] myChar = Encoding.ASCII.GetBytes(userPwd);///encoding ediyoruz burda girdigimiz karakter kadar char var
            SHA256Managed hasher = new SHA256Managed();
            byte[] enChar = hasher.ComputeHash(myChar);

            string crytoData = string.Empty;///boş null anlamında yaptık

            foreach (var item in enChar)
            {
                crytoData += string.Format("{0:X2}", item); ///x2 yapmamız 16 bitlik abcd123 içlerine serpmek için

            }
            return crytoData;
        }


        //celal 
        StockEntities db;
        public UserManagement()
        {
            db = new StockEntities();
        }

        public List<AppUsers> getAllUsers()
        {
            return (from i in db.AppUsers select i).ToList();
        }
        public AppUsers GetUser(int userid)
        {
            return (from i in db.AppUsers where i.UserID == userid select i).SingleOrDefault();
        }
        public AppUsers LoginUser(string email, string sifre)
        {
            string sifreliveri = HashUserPwd(sifre);
            return (from i in db.AppUsers where i.Email == email && i.Password == sifreliveri select i).SingleOrDefault();
        }
        public void AddUser(string username, string password, string email)
        {
            string sifreliveri = HashUserPwd(password);
            AppUsers user = new AppUsers { UserName = username, Password = sifreliveri, Email = email, Status = 1, IsActive = true, CreaDate = DateTime.Now };
            db.AppUsers.Add(user);
            db.SaveChanges();
        }

        public AppUsers AddUserEmailControl(string email)
        {
            var kontrol = (from i in db.AppUsers where i.Email.Equals(email) select i).SingleOrDefault();
            return (kontrol);

        }

        public AppUsers AddUserUserNameControl(string username)
        {
            var kontrol = (from i in db.AppUsers where i.UserName.Equals(username) select i).SingleOrDefault();
            return (kontrol);

        }
        public void UptadeUser(AppUsers guncellenecekuser)
        {
            var degisecekUser = (from i in db.AppUsers where i.UserID == guncellenecekuser.UserID select i).Single();
            db.Entry(degisecekUser).OriginalValues.SetValues(guncellenecekuser); //celal
            db.SaveChanges();

        }

        public void UpdateUserStatus(int userid, byte status)
        {
            var degisecekstatus = (from i in db.AppUsers where i.UserID == userid select i).SingleOrDefault();
            degisecekstatus.Status = status;
            db.SaveChanges();
        }

        public void EditUserInformation(int id, string ad, string soyad, DateTime birthdate, string phonenumber)
        {
            var sorgu = (from i in db.AppUsers where i.UserID == id select i).SingleOrDefault();

            sorgu.FirstName = ad;
            sorgu.LastName = soyad;
            sorgu.BirtDate = birthdate;
            sorgu.PhoneNumber = phonenumber;

            db.SaveChanges();

        }
        public void EditUserPassword(int id, string email, string suansifre, string degisen)
        {
            string gelensifre = HashUserPwd(suansifre);

            var gelenuser = (from i in db.AppUsers where i.Email.Equals(email) && i.Password.Equals(gelensifre) && i.UserID == id select i).SingleOrDefault();

            string degisensifre = HashUserPwd(degisen);

            gelenuser.Password = degisensifre;

            db.SaveChanges();

        }

        public List<RandomKeyResetPassword> ForgetPasswordRandomKey()
        {

            //   var sorgu=db.AppUsers.OrderBy(s => Guid.NewGuid()).Take(1).ToList();

            var RandomUser = (from i in db.RandomKeyResetPassword orderby Guid.NewGuid() select i).Take(1).ToList();
            return (RandomUser);

        }

        public string ForgetPassword(string Email, string dogrulama)
        {
            var gelendogrulama = (from i in db.RandomKeyResetPassword where i.RandomText.Equals(dogrulama) select i).SingleOrDefault();

            var emaildogrulama = (from i in db.AppUsers where i.Email.Equals(Email) select i).SingleOrDefault();

            if (gelendogrulama != null)
            {
                if (emaildogrulama != null)
                {
                    Guid sifre = Guid.NewGuid();
                    emaildogrulama.ForgetPassword = sifre.ToString();
                    db.SaveChanges();
                    string domainadi = "http://localhost:58166/Home/ForgetPasswordChange/?key=";
                    Mail(Email, "Şifre Değişiklik İsteği", "Merhaba " + emaildogrulama.UserName + " Şifrenizi Sıfırlama Talebi Aldık <a href='" + domainadi + sifre + "'>ŞİFRE SIFIRLAMAK İÇİN ÜZERİME TIKLA</a>  Urlden Yeni Şifrenizi Alabilirsiniz");
                    return ("Email Adresinize Sıfırlama Maili Gönderildi");
                }
                else
                {
                    return ("Sunucumuzda Böyle Bir Mail Bulunmadı");
                }

            }
            else
            {
                return ("Doğrulama Kodu Hatalı");
            }


        }
        public string ForgetChangePassword(string key, string Email, string newpassword)
        {
            var gelensorgu = (from i in db.AppUsers where i.ForgetPassword.Equals(key) && i.IsActive.Value && i.Email.Equals(Email) select i).SingleOrDefault();

            if (gelensorgu != null)
            {
                var degisensifre = HashUserPwd(newpassword);
                gelensorgu.Password = degisensifre;
                gelensorgu.ForgetPassword = null;

                db.SaveChanges();

                Mail(Email, "Şifre Değişikligi", "Merhaba " + gelensorgu.UserName + " Şifreniz Güncellendi Yeni Şifrenizde Yeniden Giriş Yapabilirsiniz.");

                return ("Şifreniz Güncellendi Yeni Şifreniz İle Giriş Yapabilirsiniz.");


            }

            else
            {
                return ("Hatalı Email veya Şifre Girdiniz.");
            }
        }

        public void EmailWelcomeSend(string gidenmail, string Username)
        {
            SmtpClient client = new SmtpClient();
            MailAddress from = new MailAddress("furkanyaziyor@gmail.com");
            MailAddress to = new MailAddress(gidenmail);
            MailMessage msg = new MailMessage(from, to);
            msg.IsBodyHtml = true;
            msg.Subject = "Sitemize Hoşgeldin";
            msg.Body += "<h1>  Merhaba  " + Username + "  Sitemize Hoş Geldin Keyifli Alışverişler Dileriz  </h1>";
            msg.CC.Add(gidenmail);//herkes görür
            NetworkCredential info = new NetworkCredential("furkanyaziyor@gmail.com", "ibanez756q7");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = info;
            client.Send(msg);


        }

        public void EmailPasswordChange(string gidenmail, string Username)
        {
            SmtpClient client = new SmtpClient();
            MailAddress from = new MailAddress("furkanyaziyor@gmail.com");
            MailAddress to = new MailAddress(gidenmail);
            MailMessage msg = new MailMessage(from, to);
            msg.IsBodyHtml = true;
            msg.Subject = "Şifreniz Değiştirildi";
            msg.Body += "<h1>  Merhaba  " + Username + "  şifreniz degiştirildi. Eğer siz değiştirmediyseniz Admin ile iletişime geçiniz. </h1>";
            msg.CC.Add(gidenmail);//herkes görür
            NetworkCredential info = new NetworkCredential("furkanyaziyor@gmail.com", "ibanez756q7");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = info;
            client.Send(msg);
        }

        public void Mail(string gidenmail, string subject, string body)
        {
            SmtpClient client = new SmtpClient();
            MailAddress from = new MailAddress("furkanyaziyor@gmail.com");
            MailAddress to = new MailAddress(gidenmail);
            MailMessage msg = new MailMessage(from, to);
            msg.IsBodyHtml = true;
            msg.Subject = subject;
            msg.Body += "<h1>  " + body + "  </h1>";
            msg.CC.Add(gidenmail);//herkes görür
            NetworkCredential info = new NetworkCredential("furkanyaziyor@gmail.com", "ibanez756q7");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Credentials = info;
            client.Send(msg);


        }

    }
}
