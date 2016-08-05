using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using PagedList;
using MvcRazorToPdf;


namespace Tens.Controllers
{
    public class ReportsController : Controller
    {


        private DataModelDataContext context;

        public ReportsController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Sales(int? page, string firstdate, string lastdate)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<sale> s = context.sales.OrderByDescending(x => x.id_sales).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(firstdate) && !String.IsNullOrEmpty(lastdate))
            {
                DateTime a = Convert.ToDateTime(firstdate);
                DateTime b = Convert.ToDateTime(lastdate);
                s = context.sales.Where(x => x.date_transaction >= a && x.date_transaction <= b).OrderBy(x => x.date_transaction).ToPagedList(pageIndex, pageSize);
            }
            return View(s);
        }

        public ActionResult Shipped(int? page, string firstdate, string lastdate)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<shipped> s = context.shippeds.OrderByDescending(x => x.id_shipped).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(firstdate) && !String.IsNullOrEmpty(lastdate))
            {
                DateTime a = Convert.ToDateTime(firstdate);
                DateTime b = Convert.ToDateTime(lastdate);
                s = context.shippeds.Where(x => x.date_transaction >= a && x.date_transaction <= b).OrderBy(x => x.date_transaction).ToPagedList(pageIndex, pageSize);
            }
            return View(s);
        }


        public ActionResult Items(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<inventrory_item> item = context.inventrory_items.OrderByDescending(x => x.id_item).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                item = context.inventrory_items.Where(x => x.brand.brand_name.Contains(searchString)
                || x.item_category.category_description.Contains(searchString)
                || x.item_description.Contains(searchString)
                || x.avarage_montly_usage.Equals(searchString)
                || x.recorder_level.Equals(searchString)
                || x.recorder_quantity.Equals(searchString)
                || x.other_item_details.Contains(searchString)
                ).ToPagedList(pageIndex, pageSize);
            }
            return View(item);
        }

        public ActionResult Brand(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<brand> br = context.brands.OrderBy(b => b.brand_name).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                br = context.brands.Where(b => b.brand_name.Contains(searchString)).ToPagedList(pageIndex, pageSize);
            }
            return View(br);
        }

        public ActionResult TestPdf()
        {
            List<brand> model = context.brands.ToList();
            return new PdfActionResult(model);
        }

    }
}
