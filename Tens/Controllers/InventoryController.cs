using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tens.Models;
using PagedList;
using System.IO;

namespace Tens.Controllers
{
    public class InventoryController : Controller
    {
        //
        // GET: /Inventory/
        private DataModelDataContext context;

        public InventoryController()
        {
            context = new DataModelDataContext();
        }

        public ActionResult Index(int? page, string searchString)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.number = pageIndex;
            IPagedList<inventrory_item> item = context.inventrory_items.OrderByDescending(x=>x.id_item).ToPagedList(pageIndex, pageSize);
            if (!String.IsNullOrEmpty(searchString))
            {
                item = context.inventrory_items.Where(x => x.brand.brand_name.Contains(searchString)
                || x.item_category.category_description.Contains(searchString)
                || x.item_description.Contains(searchString)
                || x.avarage_montly_usage.Equals(searchString)
                || x.recorder_level.Equals(searchString)
                || x.recorder_quantity.Equals(searchString)
                || x.other_item_details.Contains(searchString)
                ).ToPagedList(pageIndex, pageSize);
            }
            return View(item);
        }

        [HttpGet]
        public ActionResult Create()
        {
            inventrory_item i = new inventrory_item();
            ViewData["brand"] = context.brands.ToList();
            ViewData["category"] = context.item_categories.ToList();
            ViewData["supplier"] = context.suppliers.ToList();
            return View(i);
        }


        [HttpPost]
        public ActionResult Create(inventrory_item i)
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
                        file.SaveAs(Server.MapPath("~/Items/") + new_files + "" + extension);
                        photo = new_files + "" + extension;
                    }
                }

                var brand = context.brands.FirstOrDefault(x => x.id_brand.Equals(Request["brand_id"]));
                var item_category = context.item_categories.FirstOrDefault(x => x.item_category_code.Equals(Request["item_category_code"]));

                //Save to inventory items
                i.brand = brand;
                i.item_category = item_category;
                i.photo = photo;
                context.inventrory_items.InsertOnSubmit(i);
                context.SubmitChanges();

                //Save to item stock level
                item_stock_level il = new item_stock_level();
                il.item_id = i.id_item;
                il.stocking_taking_date = Convert.ToInt32(Request["stocking_taking_date"]);
                il.quantity_in_stock = 0;
                context.item_stock_levels.InsertOnSubmit(il);
                context.SubmitChanges();


                //Save to item supplier
                item_supplier isup = new item_supplier();
                isup.item_id = i.id_item;
                isup.supplier_id = Convert.ToInt32(Request["supplier_id"]);
                isup.value_supplied_to_date = Convert.ToInt32(Request["value_supplied_to_date"]);
                isup.total_quantity_supplied_to_date = Convert.ToInt32(Request["total_quantity_supplied_to_date"]);
                isup.first_item_supplied_to_date = Convert.ToInt32(Request["first_item_supplied_to_date"]);
                isup.last_item_supplied_to_date = Convert.ToInt32(Request["last_item_supplied_to_date"]);
                isup.delivery_lead_time = TimeSpan.Parse(Request["delivery_lead_time"]);
                isup.standard_price = Convert.ToInt32(Request["standard_price"]);
                isup.percentage_discount = Convert.ToInt32(Request["percentage_discount"]);
                isup.minimum_order_quantity = Convert.ToInt32(Request["minimum_order_quantity"]);
                isup.maximum_order_quantity = Convert.ToInt32(Request["maximum_order_quantity"]);
                isup.other_item_supplier_details = Request["other_item_supplier_details"];
                context.item_suppliers.InsertOnSubmit(isup);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Insert data success !! ";
               
            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Inventory");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                inventrory_item inv = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id));
                item_supplier isup = context.item_suppliers.FirstOrDefault(x => x.item_id.Equals(id));
                item_stock_level isok = context.item_stock_levels.FirstOrDefault(x => x.item_id.Equals(id));

                string fullPath = Request.MapPath("~/Items/" + inv.photo);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                context.item_stock_levels.DeleteOnSubmit(isok);
                context.SubmitChanges();

                context.item_suppliers.DeleteOnSubmit(isup);
                context.SubmitChanges();

                context.inventrory_items.DeleteOnSubmit(inv);
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Delete data success !! ";
            
            }
            catch(Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = " "+e.ToString();
            }
            return RedirectToAction("Index", "Inventory");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            inventrory_item i = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id));
            ViewData["item_supplier"] = context.item_suppliers.Where(x => x.item_id.Equals(id)).ToList();
            ViewData["item_stock"] = context.item_stock_levels.Where(x => x.item_id.Equals(id)).ToList();
            ViewData["brand"] = context.brands.ToList();
            ViewData["category"] = context.item_categories.ToList();
            ViewData["supplier"] = context.suppliers.ToList();
            return View(i);
        }

        [HttpPost]
        public ActionResult Edit(inventrory_item i)
        {
            try
            {
                var item = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(i.id_item));
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
                        file.SaveAs(Server.MapPath("~/Items/") + new_files + "" + extension);
                        photo = new_files + "" + extension;

                        //Delete foto if exist

                        string fullPath = Request.MapPath("~/Items/" + item.photo);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                    }
                }

                var brand = context.brands.FirstOrDefault(x => x.id_brand.Equals(Request["brand_id"]));
                var item_category = context.item_categories.FirstOrDefault(x => x.item_category_code.Equals(Request["item_category_code"]));

                //Update to inventory items
                item.brand = brand;
                item.item_category = item_category;
                item.item_description = i.item_description;
                item.avarage_montly_usage = i.avarage_montly_usage;
                item.recorder_level = i.recorder_level;
                item.recorder_quantity = i.recorder_quantity;
                item.other_item_details = i.other_item_details;
                item.photo = photo;
                context.SubmitChanges();

                //Update to item stock level
                item_stock_level il = context.item_stock_levels.FirstOrDefault(x => x.item_id.Equals(i.id_item));
                il.item_id = i.id_item;
                il.stocking_taking_date = Convert.ToInt32(Request["stocking_taking_date"]);
                il.quantity_in_stock = 0;
                context.SubmitChanges();


                //Update to item supplier
                item_supplier isup = context.item_suppliers.FirstOrDefault(x => x.item_id.Equals(i.id_item));
                isup.item_id = i.id_item;
                isup.supplier_id = Convert.ToInt32(Request["supplier_id"]);
                isup.value_supplied_to_date = Convert.ToInt32(Request["value_supplied_to_date"]);
                isup.total_quantity_supplied_to_date = Convert.ToInt32(Request["total_quantity_supplied_to_date"]);
                isup.first_item_supplied_to_date = Convert.ToInt32(Request["first_item_supplied_to_date"]);
                isup.last_item_supplied_to_date = Convert.ToInt32(Request["last_item_supplied_to_date"]);
                isup.delivery_lead_time = TimeSpan.Parse(Request["delivery_lead_time"]);
                isup.standard_price = Convert.ToInt32(Request["standard_price"]);
                isup.percentage_discount = Convert.ToInt32(Request["percentage_discount"]);
                isup.minimum_order_quantity = Convert.ToInt32(Request["minimum_order_quantity"]);
                isup.maximum_order_quantity = Convert.ToInt32(Request["maximum_order_quantity"]);
                isup.other_item_supplier_details = Request["other_item_supplier_details"];
                context.SubmitChanges();

                TempData["cls"] = "success";
                TempData["message"] = " Update data success !! ";
                

            }
            catch (Exception e)
            {
                TempData["cls"] = "danger";
                TempData["message"] = e.ToString();
            }
            return RedirectToAction("Index", "Inventory");
        }


        [HttpGet]
        public ActionResult View(int id)
        {
            inventrory_item i = context.inventrory_items.FirstOrDefault(x => x.id_item.Equals(id));
            ViewData["item_supplier"] = context.item_suppliers.Where(x => x.item_id.Equals(id)).ToList();
            ViewData["item_stock"] = context.item_stock_levels.Where(x => x.item_id.Equals(id)).ToList();
            ViewData["brand"] = context.brands.ToList();
            ViewData["category"] = context.item_categories.ToList();
            ViewData["supplier"] = context.suppliers.ToList();
            return View(i);
        }

    }

 }

