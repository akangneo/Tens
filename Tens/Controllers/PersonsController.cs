using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using System.IO;
using Tens.Helpers;
using PagedList;

namespace Tens.Controllers
{
    public class PersonsController : Controller
    {
        //
        // GET: /Persons/

        private DataModelDataContext context;


        public PersonsController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<person> p = context.persons.OrderByDescending(w => w.id_person).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                p = context.persons.Where(pr => pr.person_name.Contains(searchString) 
                || pr.phone.Contains(searchString)
                || pr.cellphone.Contains(searchString)
                || pr.email.Contains(searchString)
                ).ToPagedList(pageIndex, pageSize);
            }
            return View(p);
        }

        [HttpGet]
        public ActionResult Create()
        {
            person p = new person();
            ViewData["countries"] = context.countries.ToList();
            ViewData["roles"] = context.roles.Where(v => v.id_role != 1).ToList();
            return View(p);
        }

        [HttpPost]
        public ActionResult Create(person p)
        {
            try
            {
                
                String photo = null;
                // UPLOAD IMAGE IF EXISTS

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        Random rnd = new Random();
                        String new_files = Convert.ToString(rnd.Next(1000, 9999));
                        String extension = Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Upload/") + new_files + "" + extension);
                        photo = new_files + "" + extension;

                    }
                }

                // INSERT ADDRESS
                address ad = new address();
                ad.description = Request["address"];
                ad.city = Request["city"];
                ad.postal_code = Request["postal_code"];
                ad.country_id = Convert.ToInt32(Request["country_id"]);
                ad.other_address_details = Request["other_address"];
                context.addresses.InsertOnSubmit(ad);
                context.SubmitChanges();


                // INSERT PERSON
                p.address = ad;
                p.image = photo;
                context.persons.InsertOnSubmit(p);
                context.SubmitChanges();

                // INSERT USER
                DateTime tomorrow = DateTime.Now.AddDays(30);

                role r = context.roles.FirstOrDefault(v => v.id_role.Equals(Request["role_id"]));

                user u = new user();
                u.username = Convert.ToString(Request["person_name"]).Replace(" ", "");
                u.password = MyHelpers.ConvertMD5("123");
                u.role = r;
                u.person = p;
                u.status = 1;
                u.date_expired = tomorrow;
                context.users.InsertOnSubmit(u);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = "Insert data success !!";

            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Persons");
        }


        public ActionResult Delete(int id)
        {
            try
            {
                
                TempData["cls"] = "success";
                TempData["message"] = "Delete data success !!";

                person p = context.persons.FirstOrDefault(x => x.id_person.Equals(id));
                address ad = context.addresses.FirstOrDefault(a => a.id_address.Equals(p.address_id));

                string fullPath = Request.MapPath("~/Upload/" + p.image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                context.addresses.DeleteOnSubmit(ad);
                context.SubmitChanges();


            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        public ActionResult View(int id)
        {

            person p = context.persons.FirstOrDefault(x => x.id_person.Equals(id));
            ViewData["user"] = context.users.Where(x => x.person_id == id).ToList();
            ViewData["address"] = context.addresses.Where(x => x.id_address == p.address_id).ToList();
            ViewData["countries"] = context.countries.ToList();
            ViewData["roles"] = context.roles.Where(v => v.id_role != 1).ToList();

            return View(p);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            
            person p = context.persons.FirstOrDefault(x => x.id_person.Equals(id));
            ViewData["user"] = context.users.Where(x => x.person_id == id).ToList();
            ViewData["address"] = context.addresses.Where(x => x.id_address == p.address_id).ToList();
            ViewData["countries"] = context.countries.ToList();
            ViewData["roles"] = context.roles.Where(v => v.id_role != 1).ToList();
            
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(person p)
        {
            try
            {

                var temp_person = context.persons.FirstOrDefault(x => x.id_person.Equals(p.id_person));
                String photo = null;
                // UPLOAD IMAGE IF EXISTS

                if (Request.Files.Count > 0)
                {
                   
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        Random rnd = new Random();
                        String new_files = Convert.ToString(rnd.Next(1000, 9999));
                        String extension = Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("~/Upload/") + new_files + "" + extension);
                        photo = new_files + "" + extension;

                        //Delete foto if exist

                        string fullPath = Request.MapPath("~/Upload/" + temp_person.image);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                    }
                }

                // UPDATE ADDRESS
                address ad = context.addresses.FirstOrDefault(x=>x.id_address==temp_person.address_id);
                ad.description = Request["address"];
                ad.city = Request["city"];
                ad.postal_code = Request["postal_code"];
                ad.country_id = Convert.ToInt32(Request["country_id"]);
                ad.other_address_details = Request["other_address"];
                context.SubmitChanges();


                // UPDATE PERSON
                temp_person.person_name = p.person_name;
                temp_person.gender = p.gender;
                temp_person.phone = p.phone;
                temp_person.cellphone = p.cellphone;
                temp_person.email = p.email;
                temp_person.address = ad;
                temp_person.image = photo;
                context.SubmitChanges();

                // UPDATE USER
                role r = context.roles.FirstOrDefault(v => v.id_role.Equals(Request["role_id"]));
                user us = context.users.FirstOrDefault(x => x.person_id == temp_person.id_person);
                us.role = r;
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Update data success !!";

            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Persons");
        }

        

    }

}
