using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using Tens.Models;

namespace Tens.Helpers
{
    public class MyHelpers
    {

        public static String ConvertMD5(String password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }

        public static String SupplierName(int item_id)
        {
            String name = null;
            DataModelDataContext context = new DataModelDataContext();
            item_supplier isup = context.item_suppliers.FirstOrDefault(x => x.item_id.Equals(item_id));
            name = isup.supplier.name;
            return name;
        }

        public static int getIdItem(String name)
        {
            int id = 0;
            DataModelDataContext context = new DataModelDataContext();
            id = context.inventrory_items.FirstOrDefault(x => x.item_description.Contains(name)).id_item;
            return id;
        }

        public static int getTotalShipped(int id, String name)
        {
            int result = 0;
            DataModelDataContext context = new DataModelDataContext();
            if (name == "price")
            {
                result = Convert.ToInt32(context.shipped_details.Where(x => x.shipped_id.Equals(id)).Sum(x => x.item_price));
            }
            else if (name == "item")
            {
                result = Convert.ToInt32(context.shipped_details.Where(x => x.shipped_id.Equals(id)).Sum(x => x.item_qty));
            }
            else 
            {
                result = 0;
            } 
            return result;
        }

        public static int getTotalSales(int id, String name)
        {
            int result = 0;
            DataModelDataContext context = new DataModelDataContext();
            if (name == "price")
            {
                result = Convert.ToInt32(context.sales_details.Where(x => x.sales_id.Equals(id)).Sum(x => x.item_price));
            }
            else if (name == "item")
            {
                result = Convert.ToInt32(context.sales_details.Where(x => x.sales_id.Equals(id)).Sum(x => x.item_qty));
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public static int GetReportBrand(int id,String choice)
        {
            int result = 0;
            DataModelDataContext context = new DataModelDataContext();
            List<inventrory_item> it = context.inventrory_items.Where(x => x.brand_id.Equals(id)).ToList();
            if (choice == "stock")
            {
                foreach (var row in it)
                {
                    int id_item = row.id_item;
                    int total = Convert.ToInt32(context.item_stock_levels.Where(x => x.item_id.Equals(id_item)).Sum(x => x.quantity_in_stock));
                    result = result + total;
                }
            }
            else if (choice == "shipped")
            {
                foreach (var row in it)
                {
                    String name_item = row.item_description;
                    int total = Convert.ToInt32(context.shipped_details.Where(x => x.item_name.Contains(name_item)).Sum(x => x.item_qty));
                    result = result + total;
                }
            }
            else if (choice == "sales")
            {
                foreach (var row in it)
                {
                    String name_item = row.item_description;
                    int total = Convert.ToInt32(context.sales_details.Where(x => x.item_name.Contains(name_item)).Sum(x => x.item_qty));
                    result = result + total;
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }


        public static int GetReportItem(int id, String choice)
        {
            int result = 0;
            DataModelDataContext context = new DataModelDataContext();
            inventrory_item it = context.inventrory_items.FirstOrDefault(x=>x.id_item.Equals(id));
            if (choice == "stock")
            {
                int temp = Convert.ToInt32(context.item_stock_levels.Where(x => x.item_id.Equals(id)).Sum(x => x.quantity_in_stock));
                result = temp;
            }
            else if (choice == "shipped")
            {
                int temp = Convert.ToInt32(context.shipped_details.Where(x => x.item_name.Contains(it.item_description)).Sum(x => x.item_qty));
                result = temp;
            }
            else if (choice == "sales")
            {
                int temp = Convert.ToInt32(context.sales_details.Where(x => x.item_name.Contains(it.item_description)).Sum(x => x.item_qty));
                result = temp;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public static int getSalesChart(int index)
        {
            DataModelDataContext context = new DataModelDataContext();
            int result = 0;
            int[] month = new int[12];
            String year = Convert.ToString(DateTime.Now.Year);
            month[0] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/01/"+year) && x.date_transaction <= Convert.ToDateTime("31/01/"+year)).Count();
            month[1] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/02/"+year) && x.date_transaction <= Convert.ToDateTime("29/02/"+year)).Count();
            month[2] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/03/"+year) && x.date_transaction <= Convert.ToDateTime("31/03/"+year)).Count();
            month[3] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/04/"+year) && x.date_transaction <= Convert.ToDateTime("30/04/"+year)).Count();
            month[4] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/05/"+year) && x.date_transaction <= Convert.ToDateTime("31/05/"+year)).Count();
            month[5] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/06/"+year) && x.date_transaction <= Convert.ToDateTime("30/06/"+year)).Count();
            month[6] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/07/" + year) && x.date_transaction <= Convert.ToDateTime("31/07/"+year)).Count();
            month[7] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/08/" + year) && x.date_transaction <= Convert.ToDateTime("31/08/"+year)).Count();
            month[8] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/09/" + year) && x.date_transaction <= Convert.ToDateTime("30/09/"+year)).Count();
            month[9] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/10/" + year) && x.date_transaction <= Convert.ToDateTime("31/10/"+year)).Count();
            month[10] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/11/" + year) && x.date_transaction <= Convert.ToDateTime("30/11/"+year)).Count();
            month[11] = context.sales.Where(x => x.date_transaction >= Convert.ToDateTime("01/12/" + year) && x.date_transaction <= Convert.ToDateTime("31/12/"+year)).Count();
            result = month[index];
            return result;
        }

    }
}