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
    public class ShippedController : Controller
    {
        //
        // GET: /Shipped/

        private DataModelDataContext context;

        public ShippedController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string firstdate,string lastdate)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<shipped> s = context.shippeds.OrderByDescending(x=>x.id_shipped).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(firstdate) && !String.IsNullOrEmpty(lastdate))
            {
                DateTime a = Convert.ToDateTime(firstdate);
                DateTime b = Convert.ToDateTime(lastdate);
                s = context.shippeds.Where(x => x.date_transaction >= a && x.date_transaction <= b).OrderBy(x => x.date_transaction).ToPagedList(pageIndex, pageSize);
            }
            return View(s);
        }


        [HttpGet]
        public ActionResult Create()
        {
            DateTime today = DateTime.Today;
            shipped s = new shipped();

            String last_code = context.shippeds.Where(x=>x.date_transaction <= DateTime.Now).Max(x => x.code_shipped);
            String code_shipped = null;
            if (last_code == null)
            {
                code_shipped = "SH" + DateTime.Now.ToString("ddMM") + "0001";
            }
            else
            {
                String temp = last_code.Substring(6);
                int index = (Convert.ToInt32(temp))+1;
                String temp_index = Convert.ToString(index);
                int temp_length = Convert.ToString(index).Length;
                for (int i = 4; i > temp_length; i--)
                {
                    temp_index = "0" + temp_index;
                }
                code_shipped = "SH" + DateTime.Now.ToString("ddMM") + "" + temp_index;
            }

            ViewData["shipped_code"] = code_shipped;
            ViewData["date_transaction"] = (DateTime.Now.Date).ToString("yyyy-MM-dd");
            ViewData["supplier"] = context.suppliers.ToList();
            return View(s);
        }

        [HttpPost]
        public ActionResult Create(shipped s)
        {
            try
            {
                string[] id_item = Request.Form.GetValues("id_item");
                string[] price_item = Request.Form.GetValues("price_item");
                string[] qty = Request.Form.GetValues("qty");
                string[] total = Request.Form.GetValues("total");
                
                double grand_total = 0;
                double sub_total = 0;
                int temp_total = 0;
                double disc = 0;
                if (id_item.Length > 10) 
                {
                    disc = 0.05;
                }
                for (int i = 0; i < total.Length; i++)
                {
                    temp_total = temp_total + Convert.ToInt32(total[i]);
                }
                sub_total = temp_total;
                grand_total = temp_total - (temp_total * disc);

               
                s.code_shipped = Request["code_shipped"];
                s.date_transaction = DateTime.Now;
                s.supplier = context.suppliers.FirstOrDefault(x => x.id_supplier.Equals(Request["supplier_id"]));
                s.user = context.users.FirstOrDefault(x => x.id_user.Equals(4));
                s.status = 1;
                s.disc = Convert.ToDecimal(disc);
                s.sub_total = Convert.ToDecimal(sub_total);
                s.grand_totall = Convert.ToDecimal(grand_total);
                context.shippeds.InsertOnSubmit(s);
                context.SubmitChanges();

                for (int i = 0; i < id_item.Length; i++)
                {
                    shipped_detail sd = new shipped_detail();
                    sd.shipped = s;
                    sd.item_name = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id_item[i])).item_description;
                    sd.item_price = Convert.ToDecimal(price_item[i]);
                    sd.item_qty = Convert.ToInt32(qty[i]);
                    sd.total = Convert.ToDecimal(total[i]);
                    context.shipped_details.InsertOnSubmit(sd);
                    context.SubmitChanges();

                    item_stock_level its = context.item_stock_levels.FirstOrDefault(x => x.item_id.Equals(id_item[i]));
                    its.quantity_in_stock = its.quantity_in_stock + Convert.ToInt32(qty[i]);
                    context.SubmitChanges();

                }


               TempData["cls"] = "success";
               TempData["message"] = " Insert data success !! ";
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = " "+e.ToString();
            }
            return RedirectToAction("Index", "Shipped");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewData["supplier"] = context.suppliers.ToList();
            ViewData["shipped"] = context.shippeds.Where(x => x.id_shipped.Equals(id)).ToList();
            ViewData["shipped_details"] = context.shipped_details.Where(x => x.shipped_id.Equals(id)).ToList();
            return View();
        }


        [HttpPost]
        public ActionResult Edit()
        {
            try
            {
                string[] id_item = Request.Form.GetValues("id_item");
                string[] price_item = Request.Form.GetValues("price_item");
                string[] qty = Request.Form.GetValues("qty");
                string[] total = Request.Form.GetValues("total");

                double grand_total = 0;
                double sub_total = 0;
                int temp_total = 0;
                double disc = 0;
                if (id_item.Length > 10)
                {
                    disc = 0.05;
                }
                for (int i = 0; i < total.Length; i++)
                {
                    temp_total = temp_total + Convert.ToInt32(total[i]);
                }
                sub_total = temp_total;
                grand_total = temp_total - (temp_total * disc);

                shipped s = context.shippeds.FirstOrDefault(x => x.id_shipped.Equals(Request["id_shipped"]));
                s.code_shipped = Request["code_shipped"];
                s.date_transaction = DateTime.Now;
                s.supplier = context.suppliers.FirstOrDefault(x => x.id_supplier.Equals(Request["supplier_id"]));
                s.user = context.users.FirstOrDefault(x => x.id_user.Equals(4));
                s.status = 1;
                s.disc = Convert.ToDecimal(disc);
                s.sub_total = Convert.ToDecimal(sub_total);
                s.grand_totall = Convert.ToDecimal(grand_total);
                context.SubmitChanges();

                List<shipped_detail> ls = context.shipped_details.Where(x => x.shipped_id.Equals(Request["id_shipped"])).ToList();
                context.shipped_details.DeleteAllOnSubmit(ls);
                context.SubmitChanges();

                for (int i = 0; i < id_item.Length; i++)
                {
                    shipped_detail sd = new shipped_detail();
                    sd.shipped = s;
                    sd.item_name = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id_item[i])).item_description;
                    sd.item_price = Convert.ToDecimal(price_item[i]);
                    sd.item_qty = Convert.ToInt32(qty[i]);
                    sd.total = Convert.ToDecimal(total[i]);
                    context.shipped_details.InsertOnSubmit(sd);
                    context.SubmitChanges();

                    item_stock_level its = context.item_stock_levels.FirstOrDefault(x => x.item_id.Equals(id_item[i]));
                    its.quantity_in_stock = its.quantity_in_stock + Convert.ToInt32(qty[i]);
                    context.SubmitChanges();

                }


                TempData["cls"] = "success";
                TempData["message"] = " Update data success !! ";
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = " " + e.ToString();
            }
            return RedirectToAction("Index", "Shipped");
        }


        [HttpGet]
        public ActionResult View(int id)
        {
            ViewData["shipped"] = context.shippeds.Where(x => x.id_shipped.Equals(id)).ToList();
            ViewData["shipped_details"] = context.shipped_details.Where(x=>x.shipped_id.Equals(id)).ToList();
            return View();
        }


       [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {

                shipped_detail sd = context.shipped_details.FirstOrDefault(x => x.shipped_id.Equals(id));
                shipped s = context.shippeds.FirstOrDefault(x => x.id_shipped.Equals(id));

                context.shipped_details.DeleteOnSubmit(sd);
                context.SubmitChanges();

                context.shippeds.DeleteOnSubmit(s);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Delete data success !! ";
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = " " + e.ToString();
            }
            return RedirectToAction("Index", "Shipped");
        }


       [HttpGet]
       public ActionResult GetItems()
       {
           List<inventrory_item> iv = new List<inventrory_item>();
           var list = context.inventrory_items.ToList();
           int x = 0;
           foreach (var row in list)
           {
               inventrory_item i = new inventrory_item();
               i.id_item = list[x].id_item;
               i.item_description = list[x].item_description;
               iv.Add(i);
               x++;
           }
           return Json(iv, JsonRequestBehavior.AllowGet);
       }

       [HttpGet]
       public ActionResult GetItemPrice(int id)
       {
           var list = context.item_suppliers.Where(x => x.item_id.Equals(id)).ToList();
           return Json(new { price = Convert.ToInt32(list[0].standard_price) }, JsonRequestBehavior.AllowGet);
       }

    }
}
