using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;

namespace MVCPractice.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            string role=Convert.ToString(Session["role"]);
            //string mgr = "Manager";
            if (role.Equals("Manager"))
                return RedirectToAction("Index", "Manager");
            else if (role.Equals("Customer"))
                return RedirectToAction("Index", "Customer");
            else
            return View();
        }

        [HttpPost]
        public ActionResult LoginCheck(string userId,string password)
        {
            LoginClass obj = new LoginClass();
            string result=obj.checkCredentials(userId, password);
            if (result == "Manager")
            {
                Session["role"] = "Manager";
                Session["userId"] = userId;

                return RedirectToAction("Index", "Manager");
            }
            else if (result == "Customer")
            {
                Session["role"] = "Customer";
                Session["userId"] = userId;
                return RedirectToAction("Index", "Customer");

            }
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }

    }
}
