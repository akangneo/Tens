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
    public class SalesController : Controller
    {
        //
        // GET: /Sales/

        private DataModelDataContext context;

        public SalesController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string firstdate, string lastdate)
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

        [HttpGet]
        public ActionResult Create()
        {
            DateTime today = DateTime.Today;
            sale s = new sale();

            String last_code = context.sales.Where(x => x.date_transaction <= DateTime.Now).Max(x => x.code_sales);
            String code_sales = null;
            if (last_code == null)
            {
                code_sales = "SL" + DateTime.Now.ToString("ddMM") + "0001";
            }
            else
            {
                String temp = last_code.Substring(6);
                int index = (Convert.ToInt32(temp)) + 1;
                String temp_index = Convert.ToString(index);
                int temp_length = Convert.ToString(index).Length;
                for (int i = 4; i > temp_length; i--)
                {
                    temp_index = "0" + temp_index;
                }
                code_sales = "SL" + DateTime.Now.ToString("ddMM") + "" + temp_index;
            }

            ViewData["sales_code"] = code_sales;
            ViewData["date_transaction"] = (DateTime.Now.Date).ToString("yyyy-MM-dd");
            return View(s);
        }

        [HttpPost]
        public ActionResult Create(sale s)
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


                s.code_sales = Request["code_sales"];
                s.date_transaction = DateTime.Now;
                s.user = context.users.FirstOrDefault(x => x.id_user.Equals(4));
                s.disc = Convert.ToDecimal(disc);
                s.sub_total = Convert.ToDecimal(sub_total);
                s.grand_total = Convert.ToDecimal(grand_total);
                context.sales.InsertOnSubmit(s);
                context.SubmitChanges();

                for (int i = 0; i < id_item.Length; i++)
                {
                    sales_detail sd = new sales_detail();
                    sd.sale = s;
                    sd.item_name = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id_item[i])).item_description;
                    sd.item_price = Convert.ToDecimal(price_item[i]);
                    sd.item_qty = Convert.ToInt32(qty[i]);
                    sd.total = Convert.ToDecimal(total[i]);
                    context.sales_details.InsertOnSubmit(sd);
                    context.SubmitChanges();

                    item_stock_level its = context.item_stock_levels.FirstOrDefault(x => x.item_id.Equals(id_item[i]));
                    its.quantity_in_stock = its.quantity_in_stock - Convert.ToInt32(qty[i]);
                    context.SubmitChanges();

                }


                TempData["cls"] = "success";
                TempData["message"] = " Insert data success !! ";
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = " " + e.ToString();
            }
            return RedirectToAction("Index", "Sales");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewData["sales"] = context.sales.Where(x => x.id_sales.Equals(id)).ToList();
            ViewData["sales_details"] = context.sales_details.Where(x => x.sales_id.Equals(id)).ToList();
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

                sale s = context.sales.FirstOrDefault(x => x.id_sales.Equals(Request["id_sales"]));
                s.code_sales = Request["code_sales"];
                s.date_transaction = DateTime.Now;
                s.user = context.users.FirstOrDefault(x => x.id_user.Equals(4));
                s.disc = Convert.ToDecimal(disc);
                s.sub_total = Convert.ToDecimal(sub_total);
                s.grand_total = Convert.ToDecimal(grand_total);
                context.SubmitChanges();

                List<sales_detail> ls = context.sales_details.Where(x => x.sales_id.Equals(Request["id_sales"])).ToList();
                context.sales_details.DeleteAllOnSubmit(ls);
                context.SubmitChanges();

                for (int i = 0; i < id_item.Length; i++)
                {
                    sales_detail sd = new sales_detail();
                    sd.sale = s;
                    sd.item_name = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id_item[i])).item_description;
                    sd.item_price = Convert.ToDecimal(price_item[i]);
                    sd.item_qty = Convert.ToInt32(qty[i]);
                    sd.total = Convert.ToDecimal(total[i]);
                    context.sales_details.InsertOnSubmit(sd);
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
            return RedirectToAction("Index", "Sales");
        }


        [HttpGet]
        public ActionResult View(int id)
        {
            ViewData["sales"] = context.sales.Where(x => x.id_sales.Equals(id)).ToList();
            ViewData["sales_details"] = context.sales_details.Where(x => x.sales_id.Equals(id)).ToList();
            return View();
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {

                sales_detail sd = context.sales_details.FirstOrDefault(x => x.sales_id.Equals(id));
                sale s = context.sales.FirstOrDefault(x => x.id_sales.Equals(id));

                List<sales_detail> ls = context.sales_details.Where(x => x.sales_id.Equals(id)).ToList();
                foreach (var row in ls)
                {
                    inventrory_item it = context.inventrory_items.FirstOrDefault(x=>x.item_description.Contains(row.item_name));
                    item_stock_level its = context.item_stock_levels.FirstOrDefault(x=>x.item_id.Equals(it.id_item));
                    its.quantity_in_stock = its.quantity_in_stock + Convert.ToInt32(row.item_qty);
                }


                context.sales_details.DeleteOnSubmit(sd);
                context.SubmitChanges();

                context.sales.DeleteOnSubmit(s);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Delete data success !! ";
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = " " + e.ToString();
            }
            return RedirectToAction("Index", "Sales");
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
            var stock_item = context.item_stock_levels.Where(x => x.item_id.Equals(id)).ToList();
            return Json(new { price = Convert.ToInt32(list[0].standard_price), stock = Convert.ToInt32(stock_item[0].quantity_in_stock) }, JsonRequestBehavior.AllowGet);
        }

        


    }
}
