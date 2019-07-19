﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;


namespace MVCPractice.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        
        public ActionResult Index()
        {
            string userId=Convert.ToString(Session["userId"]);
            BankEntities1 dbContext = new BankEntities1();
            Customer customer = dbContext.Customers.Single(x => x.userId == userId);
            List<Account> accounts = (dbContext.Accounts.Where(x=>x.customerId==customer.customerId)).ToList();
                
            return View(accounts);
        }

        [HttpPost]
        public ActionResult Index(string selectedAccount)
        {
            if (string.IsNullOrEmpty(selectedAccount))
            {
                return RedirectToAction("Index");
            }
            else
            {
                Session["accountNumber"] = selectedAccount;
                return RedirectToAction("Menu");
            }

        }

        public ActionResult Menu()
        {
            if(Session["accountNumber"]!=null)
                 return View();
            return View("Index");
            
        }

        public ActionResult FundTransfer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FundTransfer(long accountNo,long destinationAccountNo,int amount,string comment)
        {
            
            try
            {

                CustomerClass obj = new CustomerClass();
                TransactionClass obj1 = new TransactionClass();
                if (accountNo == destinationAccountNo)
                {
                    ViewBag.Error = "Source and destination account cant be same";
                    return View();
                }
                bool res = obj.checkAccount(destinationAccountNo);

                if (res)
                {
                    int amt = obj.getAmount(accountNo);
                    if (amt > amount)
                    {
                        obj.transferAdd(amount, destinationAccountNo);
                        obj.transferSub(amount, accountNo);

                        ViewBag.Success = "transferred " + amount + " successfully";

                        obj1.insTrans(accountNo, destinationAccountNo, amount, "FundTransfer", comment);
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Error= "Insufficient Amount";
                    }
                }
                else
                {
                    ViewBag.Error="Destination Account not found";
                }
            }

            catch (Exception exp)
            {
                ViewBag.Error = "Exception "+exp;
            }
            return View();
        }

        public ActionResult MiniStatement()
        {
            long accountNo =  long.Parse((Session["accountNumber"]).ToString());
            BankEntities1 dbContext = new BankEntities1();

            List<Transaction> transactions = (List<Transaction>)(dbContext.Transactions.Where(x => x.fromAccountNo == accountNo || x.toAccountNo == accountNo).OrderByDescending(x => x.transactionId).ToList());

            return View(transactions);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword1, string newPassword2)
        {
            CustomerClass obj = new CustomerClass();
            bool success;
            ViewBag.message = obj.changePassword(oldPassword, newPassword1, newPassword2, Session["userid"].ToString(), out success);
            if (success)
                ModelState.Clear();
            return View();
        }

        public ActionResult BalanceEnquiry()
        {
            long accountNo = long.Parse((Session["accountNumber"]).ToString());
            BankEntities1 dbContext = new BankEntities1();
            Account account = (Account)(dbContext.Accounts.Single(x => x.accountNo == accountNo));
            return View(account);
        }

        public ActionResult CustomStatement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomStatement(DateTime fromDate,DateTime toDate)
        {
            CustomerClass obj = new CustomerClass();
            int accountNo=Int32.Parse(( Session["accountNumber"].ToString()));
            IList<Transaction> transactions=obj.customstatement(accountNo,fromDate,toDate);

            
            return View("customStatementTable",transactions);

        }
    }
}
