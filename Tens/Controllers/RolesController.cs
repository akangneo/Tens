using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using PagedList;

namespace Tens.Controllers
{
    public class RolesController : Controller
    {
        //
        // GET: /Roles/

        private DataModelDataContext context;

        public RolesController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<role> rl = context.roles.OrderByDescending(r => r.id_role).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                rl = context.roles.Where(r => r.role_name.Contains(searchString)).ToPagedList(pageIndex, pageSize);
            }
            return View(rl);  
        }

        [HttpGet]
        public ActionResult Create()
        {
            role rl = new role();
            return View(rl);
        }

        [HttpPost]
        public ActionResult Create(role rl)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Insert data success !!";
                context.roles.InsertOnSubmit(rl);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Roles");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                role con = context.roles.FirstOrDefault(r => r.id_role.Equals(id));
                TempData["cls"] = "success";
                TempData["message"] = "Delete data success !!";
                context.roles.DeleteOnSubmit(con);
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Roles");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            role con = context.roles.FirstOrDefault(r => r.id_role.Equals(id));
            return View(con);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            role con = context.roles.FirstOrDefault(r => r.id_role.Equals(id));
            return View(con);
        }

        [HttpPost]
        public ActionResult Edit(role c)
        {
            try
            {
                TempData["cls"] = "success";
                TempData["message"] = "Update data success !!";
                role con = context.roles.FirstOrDefault(x => x.id_role.Equals(c.id_role));
                con.role_name = c.role_name;
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Roles");
        }


    }
}
