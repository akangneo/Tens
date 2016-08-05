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
    public class CategoriesController : Controller
    {
        //
        // GET: /Categories/

        private DataModelDataContext context;

        public CategoriesController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<item_category> c = context.item_categories.OrderByDescending(m => m.item_category_code).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                c = context.item_categories.Where(m => m.category_description.Contains(searchString) || m.item_category_code.Contains(searchString) ).ToPagedList(pageIndex, pageSize);
            }
            return View(c);  
        }

        [HttpGet]
        public ActionResult Create()
        {
            item_category c = new item_category();
            return View(c);
        }

        [HttpPost]
        public ActionResult Create(item_category c)
        {
            try
            {

                item_category con = context.item_categories.FirstOrDefault(cv => cv.item_category_code.Equals(c.item_category_code));

                if (con != null)
                {
                    TempData["cls"] = "danger";
                    TempData["message"] = String.Format("{0} was exists !! ",c.item_category_code);
                }
                else
                {
                    TempData["cls"] = "success";
                    TempData["message"] = "Insert data success !!";
                    context.item_categories.InsertOnSubmit(c);
                    context.SubmitChanges();
                }

               
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = String.Format("Error System !!");
            }
            return RedirectToAction("Index", "Categories");
        }

        
        [HttpGet]
        public ActionResult Delete(String code)
        {
            try
            {
                item_category con = context.item_categories.FirstOrDefault(cv => cv.item_category_code.Equals(code));
                TempData["cls"] = "success";
                TempData["message"] = "Delete data success !!";
                context.item_categories.DeleteOnSubmit(con);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Categories");
        }

        [HttpGet]
        public ActionResult View(String code)
        {
            item_category con = context.item_categories.FirstOrDefault(c => c.item_category_code.Equals(code));
            return View(con);
        }

        [HttpGet]
        public ActionResult Edit(String code)
        {
            item_category con = context.item_categories.FirstOrDefault(c => c.item_category_code.Equals(code));
            return View(con);
        }

        [HttpPost]
        public ActionResult Edit(item_category c)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Update data success !!";
                item_category con = context.item_categories.FirstOrDefault(cx => cx.item_category_code.Equals(c.item_category_code));
                con.category_description = c.category_description;
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Categories");
        }

    }
}
