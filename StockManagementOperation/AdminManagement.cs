using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementOperation;

namespace StockManagementOperation
{
    public class AdminManagement
    {
        StockEntities db;

        public AdminManagement()
        {
            db = new StockEntities();
        }

        public List<AppUsers> getAllAdmin()
        {
            return (from i in db.AppUsers where i.Status == 3 select i).ToList();
        }
        public string addAdmin(string email)
        {
            var emailkontrol = (from i in db.AppUsers where i.Email == email && i.IsActive.Value select i).Any();
            if (emailkontrol)
            {
                var adminolacakkullanıcı = (from i in db.AppUsers where i.Email == email && i.IsActive.Value select i).SingleOrDefault();
                adminolacakkullanıcı.Status = 3;
                db.SaveChanges();
                return "Admin Olma İşlemi Basarılı.";
            }
            else
            {
                return "Admin Olma İşlemi Basarısız Lutfen Emaili Kontrol Ediniz";
            }


        }
        public void deleteAdmin(int id)
        {
            var adminolacakkullanıcı = (from i in db.AppUsers where i.UserID == id && i.IsActive.Value select i).SingleOrDefault();

            adminolacakkullanıcı.Status = 1;
            db.SaveChanges();

        }

    }
}
