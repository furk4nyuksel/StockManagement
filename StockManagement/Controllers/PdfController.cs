using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using StockManagementOperation;
using StockManagement.Models;
namespace StockManagement.Controllers
{
    public class PdfController : Controller
    {
        OrderManagement orderManagemet;
        public PdfController()
        {
            orderManagemet = new OrderManagement();
        }
        // GET: Pdf
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Invoice(string randomkey)
        {
            if (orderManagemet.OrderControl(randomkey) != null)
            {

                TotalOrderModelList TotalOrderModel = new TotalOrderModelList();
                TotalOrderModel.Order = orderManagemet.Order(randomkey.ToString());
                TotalOrderModel.OrderDetails = orderManagemet.GetAllOrderDetails(randomkey.ToString());
                TotalOrderModel.GetQuantity = orderManagemet.LastOrderTotalQuantity(randomkey.ToString());
                TotalOrderModel.GetTotalAmount = orderManagemet.LastOrderTotalAmount(randomkey.ToString());
                return View(TotalOrderModel);
            }
            else
            {
                ViewBag.gelen = "Kayıt Bulunamadı.";

                return View();
            }
        }

        public ActionResult GetInvoice(string randomkeyy)
        {

            if (orderManagemet.OrderControl(randomkeyy) != null)
            {

                //var url = "Invoice?randomkey" + randomkey;

                return new ActionAsPdf("Invoice", new { @randomkey = randomkeyy })
                {
                    FileName = DateTime.Now + "Fatura.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Landscape
                };
            }
            else
            {
                return RedirectToAction("LoginUser", "Home");

            }







        }
    }
}