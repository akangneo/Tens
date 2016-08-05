using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using System.Text;
using System.Security.Cryptography;
using Tens.Helpers;
using System.Collections;
using System.Web.Routing;
using PagedList;

namespace Tens.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private DataModelDataContext context;

        public HomeController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page)
        {
            ViewData["total_sales"] = context.sales.Count();
            ViewData["total_shipped"] = context.shippeds.Count();
            ViewData["total_brand"] = context.brands.Count();
            ViewData["total_supplier"] = context.suppliers.Count();
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<sale> s = context.sales.OrderByDescending(x => x.id_sales).ToPagedList(pageIndex, pageSize);
            return View(s);
        }
        

    }
}
