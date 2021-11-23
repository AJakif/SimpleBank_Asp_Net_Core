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
                ViewBag.Error = "Please Login First";
                return RedirectToAction("Login","Account");
            }
            
        }

        [HttpGet]
        public IActionResult Withdraw()
        {
            ViewBag.email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(ViewBag.Email))
            {
                return View();
            }
            else
            {
                ViewBag.Error = "Please Login First";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult Withdraw( Transection wtvm)
        {
            var idStr = HttpContext.Session.GetString("OId");
            int id = CommonHelper.ConvertToInt(idStr); //try-catch
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
                    ViewBag.Success = "Withdraw done successfully";
                    return RedirectToAction("Balance", "Transection"); //Redirects to Home accounts index view
                }
            }
            ViewBag.Error = "Error while withdraw, please try again!";
            return View();
        }

        [HttpGet]
        public IActionResult Deposit()
        {
            ViewBag.email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(ViewBag.Email))
            {
                return View();
            }
            else
            {
                ViewBag.Error = "Please Login First";
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult Deposit(Transection dtvm)
        {
            var idStr = HttpContext.Session.GetString("OId");
            int id = CommonHelper.ConvertToInt(idStr); //try-catch
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
                    ViewBag.Success = "Diposit done successfully";
                    return RedirectToAction("Balance", "Transection"); //Redirects to Home accounts index view
                }
            }
            ViewBag.Error = "Error while depsit, please try again";
            return View();
        }
    }
}
