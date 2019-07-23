using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;

namespace MVCPractice.Controllers
{
    public class ManagerController : Controller
    {
        //
        // GET: /Manager/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Withdraw()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Withdraw(long acc,int amt)
        {
            try
            {

                ManagerClass obj = new ManagerClass();

                string res = obj.withdraw(acc, amt);
                CustomerClass cc = new CustomerClass();
                Session["Medal"] = cc.checkMedal(acc);
                ViewBag.result = res;

            }

            catch (Exception exp)
            {
                ViewBag.Error = "Exception " + exp;
            }
            return View();
       
        }
        public ActionResult Deposit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Deposit(long acc, int amt)
        {
            try
            {

                ManagerClass obj = new ManagerClass();

                string res = obj.deposit(acc, amt);
                CustomerClass cc = new CustomerClass();
                Session["Medal"] = cc.checkMedal(acc);
                ViewBag.result = res;

            }

            catch (Exception exp)
            {
                ViewBag.Error = "Exception " + exp;
            }
            return View();

        }
        
    }
}
