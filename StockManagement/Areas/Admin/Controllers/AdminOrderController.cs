using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rotativa;
using StockManagementOperation;

namespace StockManagement.Areas.Admin.Controllers
{
    public class AdminOrderController : Controller
    {
        OrderManagement orderManagement;
        PdfManagement pdfManagement;
        AppUsers gelen;
        Company companybilgi;
        public AdminOrderController()
        {
            orderManagement = new OrderManagement();
            pdfManagement = new PdfManagement();
        }

        public ActionResult Orders()
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
                if (gelen.Status.Value == 2)
                {
                    return View(orderManagement.getCompanyOrder(companybilgi.CompanyID));
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }

            }
            else
            {
                return RedirectToAction("Storage", "AdminStorage", new { @area = "" });
            }

        }

        public void ExportToExcel()
        {
            StockManagementOperation.StockEntities db = new StockManagementOperation.StockEntities();

            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
                if (gelen.Status.Value == 2)
                {

                    var grid = new GridView();

                    grid.DataSource = (from i in db.OrderDetails
                                       where i.Products.Company.CompanyID == companybilgi.CompanyID
                                       select new
                                       {
                                           SiparisNo = i.OrderID,
                                           SiparisDetayno = i.OrderDetailsID,
                                           UrunAdi = i.Products.ProductName,
                                           Adet = i.Quantity,
                                           Tutar = i.Quantity * i.Price

                                       }).ToList();

                    grid.DataBind();
                    Response.Charset = "utf8_turkish_ci";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=OrderList.xls");
                    Response.ContentType = "application/excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htmltextwriter = new HtmlTextWriter(sw);
                    grid.RenderControl(htmltextwriter);
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }


        }


        public ActionResult OrderPdf()
        {
            if (Session["Login"] != null && Session["Company"] != null)
            {
                gelen = (AppUsers)Session["Login"];
                companybilgi = (Company)Session["Company"];
                if (gelen.Status.Value == 2)
                {
                    Guid key = Guid.NewGuid();
                    pdfManagement.setCompanyOrderKey(key.ToString(), companybilgi.CompanyID);
                    return new ActionAsPdf("OrdersView", new { @parametre = key, companyid = companybilgi.CompanyID })
                    {
                        FileName = DateTime.Now + "Fatura.pdf",
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Landscape
                    };
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }

            }
            else
            {
                return RedirectToAction("Storage", "AdminStorage", new { @area = "" });
            }

        }


        public ActionResult OrdersView(string parametre, int companyid)
        {
            var a = pdfManagement.companyOrderPdfControl(parametre, companyid);
            if (a == "basarılı")
            {
                return View(orderManagement.getCompanyOrder(companyid));
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }

        }
    }
}