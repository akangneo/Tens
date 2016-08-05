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
    public class AddressController : Controller
    {
        //
        // GET: /Address/

        private DataModelDataContext context;


        public AddressController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<addresse_type> a = context.addresse_types.OrderByDescending(ad => ad.address_type_code).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                a = context.addresse_types.Where(ad=>ad.address_type_code.Contains(searchString) || ad.address_type_description.Contains(searchString)).ToPagedList(pageIndex, pageSize);
            }
            return View(a);  
        }

        [HttpGet]
        public ActionResult Create()
        {
            addresse_type a = new addresse_type();
            return View(a);
        }

        [HttpPost]
        public ActionResult Create(addresse_type a)
        {
            try
            {

                addresse_type con = context.addresse_types.FirstOrDefault(ad=>ad.address_type_code.Contains(a.address_type_code));

                if (con != null)
                {
                    TempData["cls"] = "danger";
                    TempData["message"] = String.Format("{0} was exists !! ", a.address_type_code);
                }
                else
                {
                    TempData["cls"] = "success";
                    TempData["message"] = "Insert data success !!";
                    context.addresse_types.InsertOnSubmit(a);
                    context.SubmitChanges();
                }


            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = String.Format("Error System !!");
            }
            return RedirectToAction("Index", "Address");
        }

        [HttpGet]
        public ActionResult Delete(String code)
        {
            try
            {
                addresse_type con = context.addresse_types.FirstOrDefault(ad => ad.address_type_code.Contains(code));
                TempData["cls"] = "success";
                TempData["message"] = "Delete data success !!";
                context.addresse_types.DeleteOnSubmit(con);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Address");
        }

        [HttpGet]
        public ActionResult View(String code)
        {
            addresse_type con = context.addresse_types.FirstOrDefault(ad => ad.address_type_code.Contains(code));
            return View(con);
        }

        [HttpGet]
        public ActionResult Edit(String code)
        {
            addresse_type con = context.addresse_types.FirstOrDefault(ad => ad.address_type_code.Contains(code));
            return View(con);
        }

        [HttpPost]
        public ActionResult Edit(addresse_type a)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Update data success !!";
                addresse_type con = context.addresse_types.FirstOrDefault(ad => ad.address_type_code.Contains(a.address_type_code));
                con.address_type_description = a.address_type_description;
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Address");
        }

    }
}
