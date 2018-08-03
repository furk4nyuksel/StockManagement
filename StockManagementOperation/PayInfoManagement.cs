using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockManagementOperation;
namespace StockManagementOperation
{
    public class PayInfoManagement
    {
        StockEntities db;
        public PayInfoManagement()
        {
            db = new StockEntities();
        }


        public void AddPayInfo(string cartname, string cartnumber, string cartexp, int cartsec, int userid)
        {
            PayInfo pay = new PayInfo
            {
                UserID = userid,
                CardNumber = cartnumber,
                NameOnCard = cartname,
                Expiration = cartexp,
                SecurityCode = cartsec,
            };

            db.PayInfo.Add(pay);
            db.SaveChanges();

        }

        public bool PayInfoUserCartControl(int userid)
        {
            return (from i in db.UserCart where i.UserID == userid && i.IsActive.Value && i.Quantity > 0 select i).Any();
        }
    }
}
