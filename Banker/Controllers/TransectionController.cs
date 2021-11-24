using Banker.Helpers;
using Banker.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Controllers
{

    public class TransectionController : Controller
    {
        private readonly IConfiguration _config;
        readonly CommonHelper _helper;

        public TransectionController(IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        [HttpGet]
        [Route("Home/Balance")]
        public IActionResult Balance()
        {
            ViewBag.email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(ViewBag.Email))
            {
                var idStr = HttpContext.Session.GetString("OId");
                int id = CommonHelper.ConvertToInt(idStr); //try-catch
                CollectData Model = new CollectData
                {
                    Transections = _helper.GetTransaction(id),
                    User = _helper.GetUserById(id)
                };
                return View(Model);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
            
        }

        [HttpGet]
        [Route("Home/Withdraw")]
        public IActionResult Withdraw()
        {
            ViewBag.email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(ViewBag.Email))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult Withdraw( Transection wtvm)
        {
            var idStr = HttpContext.Session.GetString("OId");
            int id = CommonHelper.ConvertToInt(idStr); //try-catch
            try
            {
                UserViewModel uvm = new UserViewModel();
                uvm = _helper.GetUserById(id);
                if(wtvm.Amount <= 9)
                {
                    ViewBag.Error = "You cannot withdraw less than 10$!";
                    ViewBag.email = HttpContext.Session.GetString("Email");
                    return View();
                }
                if(wtvm.Amount > uvm.Balance)
                {
                    ViewBag.Error = "You cannot withdraw more than your balance!";
                    ViewBag.email = HttpContext.Session.GetString("Email");
                    return View();
                }
                if (uvm.Balance <= 0)
                {
                    ViewBag.Error = "Sorry your balance is 0, you cannot withdraw now. please deposit first";
                    ViewBag.email = HttpContext.Session.GetString("Email");
                    return View();
                }
                else
                {
                    var date = DateTime.Now;
                    string Query = "Insert into [Transaction] (UserId,Name,Date,Amount,Remark,Type)" +
                        $"values ('{id}','{wtvm.Name}','{date}','{wtvm.Amount}','{wtvm.Remark}','{"Withdraw"}')";
                    //If user doesn't exists it inserts data into database
                    int result = _helper.DMLTransaction(Query);
                    if (result > 0)
                    {
                        string Uquery = $"UPDATE [User] SET Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') - '{wtvm.Amount}') WHERE OId = '{id}'";
                        int Uresult = _helper.DMLTransaction(Uquery);
                        if (Uresult > 0)
                        {
                            return RedirectToAction("Balance", "Transection"); //Redirects to Home accounts index view
                        }
                    }
                }
            }
            catch
            {
                ViewBag.Error = "Error while withdraw, please try again!";
                ViewBag.email = HttpContext.Session.GetString("Email");
            } 
            return View();
        }

        [HttpGet]
        [Route("Home/Deposit")]
        public IActionResult Deposit()
        {
            ViewBag.email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(ViewBag.Email))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult Deposit(Transection dtvm)
        {
            try
            {
                var idStr = HttpContext.Session.GetString("OId");
                int id = CommonHelper.ConvertToInt(idStr); //try-catch
                UserViewModel uvm = new UserViewModel();
                uvm = _helper.GetUserById(id);
                if (dtvm.Amount <= 9)
                {
                    ViewBag.Error = "You cannot deposit less than 10$!";
                    ViewBag.email = HttpContext.Session.GetString("Email");
                    return View();
                }
                var date = DateTime.Now;
                string Query = "Insert into [Transaction] (UserId,Name,Date,Amount,Remark,Type)" +
                    $"values ('{id}','{dtvm.Name}','{date}','{dtvm.Amount}','{dtvm.Remark}','{"Diposit"}')";
                //If user doesn't exists it inserts data into database
                int result = _helper.DMLTransaction(Query);
                if (result > 0)
                {
                    string Uquery = $"UPDATE [User] SET Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') + '{dtvm.Amount}') WHERE OId = '{id}'";
                    int Uresult = _helper.DMLTransaction(Uquery);
                    if (Uresult > 0)
                    {
                        return RedirectToAction("Balance", "Transection"); //Redirects to Home accounts index view
                    }
                }
            }
            catch
            {
                ViewBag.Error = "Error while depsit, please try again";
                ViewBag.email = HttpContext.Session.GetString("Email");
            }
            
            
            return View();
        }
    }
}
