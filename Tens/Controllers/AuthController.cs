using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tens.Models;
using Tens.Helpers;

namespace Tens.Controllers
{
    public class AuthController : Controller
    {

        private DataModelDataContext context;

        public AuthController()
        {
            context = new DataModelDataContext();
        }


        [HttpGet]
        public ActionResult Login()
        {
            user us = new user();
            return View(us);
        }

        [HttpPost]
        public ActionResult Login(user us)
        {

            user user_login = (from u in context.users where u.username == us.username && u.password == (MyHelpers.ConvertMD5(us.password)) select u).FirstOrDefault();

            //var valid = from u in context.users where u.username == us.username && u.password == GetPassword(us.password) select u;
            if (user_login != null)
            {
                Dictionary<string, string> MyList = new Dictionary<String, String>();
                MyList.Add("username", user_login.username);
                MyList.Add("role_id", Convert.ToString(user_login.role_id));
                Session["user"] = MyList;
                TempData["login"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["failed"] = true;
                return View(us);
            }

        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Auth");
        }
        

    }
}
