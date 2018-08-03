using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    public class AdressManagement
    {
        StockEntities db;
        public AdressManagement()
        {
            db = new StockEntities();
        }

        public List<Address> GetUserAdress(int userid)
        {
            return (from i in db.Address where i.UserID == userid && i.IsActive.Value select i).ToList();


        }

        public bool GetUserAdressAnyControl(int userid)
        {
            return (from i in db.Address where i.UserID == userid && i.IsActive.Value select i).Any();
        }
        public void AddAdress(int userid, string AdressTitle, string AdressDetails, byte ddlcountry)
        {
            Address adres = new Address
            {
                UserID = userid,
                AddressTitle = AdressTitle,
                AdresDetails = AdressDetails,
                CountyID = ddlcountry,
                IsActive = true

            };

            db.Address.Add(adres);
            db.SaveChanges();
        }


        public Address AdressControl(int userid, string Adresstitle) //aynı isim kontrolü
        {
            return (from i in db.Address where i.UserID == userid && i.AddressTitle.Equals(Adresstitle) && i.IsActive.Value select i).SingleOrDefault();
        }

        public string UpdateUserAdress(int userid, int adressID, string AdressTitle, string AdressDetails, byte ddlcountry)
        {
            var updateuser = (from i in db.Address where i.UserID == userid && i.AddressID == adressID && i.IsActive.Value select i).SingleOrDefault();

            if (updateuser != null)
            {
                if (!string.IsNullOrEmpty(AdressTitle) && !string.IsNullOrEmpty(AdressDetails))
                {
                    if (ddlcountry > 0)
                    {
                        updateuser.AddressTitle = AdressTitle;
                        updateuser.AdresDetails = AdressDetails;
                        updateuser.CountyID = ddlcountry;
                        db.SaveChanges();
                        return ("Adress Güncellendi");
                    }
                    else
                    {
                        return ("İlçe Seçilmemiş");
                    }

                }
                else
                {
                    return ("Eksik Veriler Var");
                }

            }
            else
            {
                return ("Adress Güncellenemedi Hatalı Bişeyler Var");
            }
        }
        public Address AdressControlUpdate(int id, int userid)
        {
            return (from i in db.Address where i.AddressID == id && i.UserID == userid select i).SingleOrDefault();
        }


        public string RemoveAdress(int userid, int addressid)
        {
            var sorgu = (from i in db.Address where i.UserID == userid && i.IsActive.Value && i.AddressID == addressid select i).SingleOrDefault();

            if (sorgu != null)
            {
                sorgu.IsActive = false;
                db.SaveChanges();
                return ("Adres Başarı İle Silindi");
            }
            else
            {
                return ("Adres Silinemedi");
            }
        }
    }
}
