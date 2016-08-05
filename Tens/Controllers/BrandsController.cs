using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using PagedList;

namespace Tens.Controllers
{
    public class BrandsController : Controller
    {
        //
        // GET: /Brands/

        private DataModelDataContext context;


        public BrandsController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<brand> br = context.brands.OrderByDescending(b=>b.id_brand).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                br = context.brands.Where(b => b.brand_name.Contains(searchString)).ToPagedList(pageIndex, pageSize);
            }
            return View(br);
        }

        [HttpGet]
        public ActionResult Create()
        {
            brand b = new brand();
            return View(b);
        }


        [HttpPost]
        public ActionResult Create(brand b)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Insert data success !!";
                context.brands.InsertOnSubmit(b);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Brands");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            brand br = context.brands.FirstOrDefault(b => b.id_brand.Equals(id));
            return View(br);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            brand br = context.brands.FirstOrDefault(b => b.id_brand.Equals(id));
            return View(br);
        }


        [HttpPost]
        public ActionResult Edit(brand b)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Update data success !!";
                brand br = context.brands.FirstOrDefault(x => x.id_brand.Equals(b.id_brand));
                br.brand_name = b.brand_name;
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Brands");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                brand b = context.brands.FirstOrDefault(x => x.id_brand.Equals(id));
                TempData["cls"] = "success";
                TempData["message"] = "Delete data success !!";
                context.brands.DeleteOnSubmit(b);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Brands");
        }

    }
}
