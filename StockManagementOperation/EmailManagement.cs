using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementOperation
{
    class EmailManagement
    {
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
