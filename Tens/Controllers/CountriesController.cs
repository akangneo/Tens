using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tens.Models;
using PagedList;

namespace Tens.Controllers
{
    public class CountriesController : Controller
    {
        //
        // GET: /Countries/

        private DataModelDataContext context;

        public CountriesController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<country> c = context.countries.OrderByDescending(m => m.id_country).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                c = context.countries.Where(m => m.country_name.Contains(searchString)).ToPagedList(pageIndex, pageSize);
            }
            return View(c);  
        }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    if (requestContext.HttpContext.Session["user"] == null)
        //    {
        //        requestContext.HttpContext.Response.Clear();
        //        requestContext.HttpContext.Response.Redirect(Url.Action("Login", "Auth"));
        //        requestContext.HttpContext.Response.End();
        //    }
        //}

        [HttpGet]
        public ActionResult Create()
        {
            country c = new country();
            return View(c);
        }

        [HttpPost]
        public ActionResult Create(country c)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Insert data success !!";
                context.countries.InsertOnSubmit(c);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Countries");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                country con = context.countries.FirstOrDefault(c => c.id_country.Equals(id));
                TempData["cls"] = "success";
                TempData["message"] = "Delete data success !!";
                context.countries.DeleteOnSubmit(con);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Countries");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            country con = context.countries.FirstOrDefault(c => c.id_country.Equals(id));
            return View(con);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            country con = context.countries.FirstOrDefault(c => c.id_country.Equals(id));
            return View(con);
        }

        [HttpPost]
        public ActionResult Edit(country c)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Update data success !!";
                country con = context.countries.FirstOrDefault(x =>x.id_country.Equals(c.id_country));
                con.country_name = c.country_name;
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Countries");
        }


    }
}
