using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using PagedList;

namespace Tens.Controllers
{
    public class SuppliersController : Controller
    {
        //
        // GET: /Suppliers/

        private DataModelDataContext context;

        public SuppliersController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<supplier> s = context.suppliers.OrderByDescending(x=>x.id_supplier).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                s = context.suppliers.Where(x => x.name.Contains(searchString)
                || x.phone.Contains(searchString)
                || x.cellphone.Contains(searchString)
                || x.email.Contains(searchString)
                ).ToPagedList(pageIndex, pageSize);
            }
            return View(s);
        }

        public ActionResult Create()
        {
            supplier s = new supplier();
            ViewData["countries"] = context.countries.ToList();
            ViewData["address_type"] = context.addresse_types.ToList();
            return View(s);
        }

        [HttpPost]
        public ActionResult Create(supplier s)
        {
            try
            {
                // Save to Address
                address ad = new address();
                ad.description = Request["address"];
                ad.city = Request["city"];
                ad.postal_code = Request["postal_code"];
                ad.country_id = Convert.ToInt32(Request["country_id"]);
                ad.other_address_details = Request["other_address"];
                context.addresses.InsertOnSubmit(ad);
                context.SubmitChanges();

                // Save to Supplier
                context.suppliers.InsertOnSubmit(s);
                context.SubmitChanges();

                // Save to Supplier Address
                suppliers_address sa = new suppliers_address();
                sa.address = ad;
                sa.supplier = s;
                sa.address_type_code = Request["address_type_code"];
                sa.date_address_from = DateTime.Parse(Request["date_address_from"]);
                sa.date_address_to = DateTime.Parse(Request["date_address_to"]);
                context.suppliers_addresses.InsertOnSubmit(sa);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Insert data success !!";

            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Suppliers");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            supplier sp = context.suppliers.FirstOrDefault(x => x.id_supplier.Equals(id));
            var sa = context.suppliers_addresses.Where(x => x.supplier_id == id).ToList();
            ViewData["countries"] = context.countries.ToList();
            ViewData["address_type"] = context.addresse_types.ToList();
            ViewData["supplier_address"] = sa;
            ViewData["address"] = context.addresses.Where(x => x.id_address==sa[0].address_id).ToList(); 
            return View(sp);
        }

        [HttpPost]
        public ActionResult Edit(supplier s)
        {
            try
            {
                int id = s.id_supplier;
                supplier sp = context.suppliers.FirstOrDefault(x => x.id_supplier.Equals(id));
                suppliers_address sa = context.suppliers_addresses.FirstOrDefault(x => x.supplier_id.Equals(id));

                // Save to Address
                address ad = context.addresses.FirstOrDefault(x => x.id_address.Equals(sa.address_id)); 
                ad.description = Request["address"];
                ad.city = Request["city"];
                ad.postal_code = Request["postal_code"];
                ad.country_id = Convert.ToInt32(Request["country_id"]);
                ad.other_address_details = Request["other_address"];
                context.SubmitChanges();

                // Save to Supplier
                sp.name = s.name;
                sp.email = s.email;
                sp.phone = s.phone;
                sp.cellphone = s.cellphone;
                sp.other_detils = s.other_detils;
                context.SubmitChanges();

                // Save to Supplier Address
                sa.address_type_code = Request["address_type_code"];
                sa.date_address_from = DateTime.Parse(Request["date_address_from"]);
                sa.date_address_to = DateTime.Parse(Request["date_address_to"]);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Update data success !!";

            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Suppliers");
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            supplier sp = context.suppliers.FirstOrDefault(x => x.id_supplier.Equals(id));
            var sa = context.suppliers_addresses.Where(x => x.supplier_id == id).ToList();
            ViewData["address_type"] = context.addresse_types.Where(x=>x.address_type_code==sa[0].address_type_code).ToList();
            ViewData["supplier_address"] = sa;
            ViewData["address"] = context.addresses.Where(x => x.id_address == sa[0].address_id).ToList();
            return View(sp);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                supplier sp = context.suppliers.FirstOrDefault(x => x.id_supplier.Equals(id));
                suppliers_address sa = context.suppliers_addresses.FirstOrDefault(x => x.supplier_id.Equals(id));
                address ad = context.addresses.FirstOrDefault(x => x.id_address.Equals(sa.address_id));

                context.addresses.DeleteOnSubmit(ad);
                context.SubmitChanges();

                context.suppliers.DeleteOnSubmit(sp);
                context.SubmitChanges();

                context.suppliers_addresses.DeleteOnSubmit(sa);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Delete data success !!";

            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Suppliers");
        }

    }
}
