using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using MvcRazorToPdf;
using iTextSharp.text;

namespace Tens.Controllers
{
    public class PdfController : Controller
    {

        private DataModelDataContext context;

        public PdfController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult BillShipped(int id)
        {
            ViewData["shipped"] = context.shippeds.Where(x => x.id_shipped.Equals(id)).ToList();
            ViewData["shipped_details"] = context.shipped_details.Where(x => x.shipped_id.Equals(id)).ToList();
            return new PdfActionResult(null, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(300f, 500f, 90));
                document.NewPage();
            });
        }


        public ActionResult BillSales(int id)
        {
            ViewData["sales"] = context.sales.Where(x => x.id_sales.Equals(id)).ToList();
            ViewData["sales_details"] = context.sales_details.Where(x => x.sales_id.Equals(id)).ToList();
            return new PdfActionResult(null, (writer, document) =>
            {
                document.SetPageSize(new Rectangle(300f, 500f, 90));
                document.NewPage();
            });
        }

        public ActionResult Sales(String firstdate, String lastdate)
        {
            List<sale> s = context.sales.OrderBy(x => x.code_sales).ToList();
            if (!String.IsNullOrEmpty(firstdate) && !String.IsNullOrEmpty(lastdate))
            {
                DateTime a = Convert.ToDateTime(firstdate);
                DateTime b = Convert.ToDateTime(lastdate);
                s = context.sales.Where(x => x.date_transaction >= a && x.date_transaction <= b).OrderBy(x => x.date_transaction).ToList();
            }
            return new PdfActionResult(s);
        }


        public ActionResult Shipped(String firstdate, String lastdate)
        {
            List<shipped> s = context.shippeds.OrderBy(x => x.code_shipped).ToList();
            if (!String.IsNullOrEmpty(firstdate) && !String.IsNullOrEmpty(lastdate))
            {
                DateTime a = Convert.ToDateTime(firstdate);
                DateTime b = Convert.ToDateTime(lastdate);
                s = context.shippeds.Where(x => x.date_transaction >= a && x.date_transaction <= b).OrderBy(x => x.date_transaction).ToList();
            }
            return new PdfActionResult(s);
        }

        public ActionResult Items()
        {
            List<inventrory_item> model = context.inventrory_items.ToList();
            return new PdfActionResult(model);
        }


        public ActionResult Brand()
        {
            List<brand> model = context.brands.ToList();
            return new PdfActionResult(model);
        }

    }
}
