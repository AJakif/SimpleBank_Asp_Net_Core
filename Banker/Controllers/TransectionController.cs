using Banker.Extensions;
using Banker.Helpers;
using Banker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICommonHelper _helper;

        public TransectionController(IConfiguration config, ICommonHelper helper)
        {
            _config = config;
            _helper = helper;
        }

        [HttpGet]
        [Route("Home/Balance")]
        [Authorize]
        public IActionResult Balance()
        {
            (int id, _) = HttpContext.GetUserInfo();
            CollectData Model = new CollectData
                {
                    Transections = _helper.GetTransaction(id),
                    User = _helper.GetUserById(id)
                };
            (_, string name) = HttpContext.GetUserInfo();
            ViewBag.Name = name;
            return View(Model);
        }

        [HttpGet]
        [Route("Home/Withdraw")]
        [Authorize]
        public IActionResult Withdraw()
        {
            (_, string name) = HttpContext.GetUserInfo();
            ViewBag.Name = name;
            return View();
        }

        [HttpPost]
        public IActionResult Withdraw( Transection wtvm)
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
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
                    if (uvm.Balance <= 0)
                    {
                        ViewBag.Error = "Sorry your balance is 0, you cannot withdraw now. please deposit first";
                        ViewBag.email = HttpContext.Session.GetString("Email");
                    }
                    else
                    {
                        ViewBag.Error = "You cannot withdraw more than your balance!";
                        ViewBag.email = HttpContext.Session.GetString("Email");
                    }
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
                            return RedirectToRoute("balance"); //Redirects to Home accounts index view
                        }
                    }
                }
            }
            catch(NullReferenceException)
            {
                ViewBag.Error = "Error while withdraw, please try again!";
                ViewBag.email = HttpContext.Session.GetString("Email");
            } 
            return View();
        }

        [HttpGet]
        [Route("Home/Deposit")]
        [Authorize]
        public IActionResult Deposit()
        {
            (_, string name) = HttpContext.GetUserInfo();
            ViewBag.Name = name;
            return View();
        }

        [HttpPost]
        public IActionResult Deposit(Transection dtvm)
        {
            try
            {
                (int id, _) = HttpContext.GetUserInfo();
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
                        return RedirectToRoute("balance"); //Redirects to Home accounts index view
                    }
                }
            }
            catch(NullReferenceException)
            {
                ViewBag.Error = "Error while depsit, please try again";
                ViewBag.email = HttpContext.Session.GetString("Email");
            }
            
            
            return View();
        }
    }
}
