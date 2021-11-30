using Banker.Extensions;
using Banker.Helpers;
using Banker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TransectionController> _logger;

        public TransectionController(IConfiguration config, ICommonHelper helper, ILogger<TransectionController> logger)
        {
            _config = config;
            _helper = helper;
            _logger = logger;
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
            _logger.LogInformation("Entered in Balance Dashboard");
            return View(Model);
        }

        [HttpGet]
        [Route("Home/Withdraw")]
        [Authorize]
        public IActionResult Withdraw()
        {
            _logger.LogInformation("Entered in withdraw form");
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
                    _logger.LogWarning("User wanted to withdraw more than user's balance!");
                    return View();
                }
                if(wtvm.Amount > uvm.Balance)
                {
                    if (uvm.Balance <= 0)
                    {
                        ViewBag.Error = "Sorry your balance is 0, you cannot withdraw now. please deposit first";
                        _logger.LogWarning("User's balance is 0 & wanted to withdraw!");
                    }
                    else
                    {
                        ViewBag.Error = "You cannot withdraw more than your balance!";
                        _logger.LogWarning("User wanted to withdraw more than user's balance!");
                    }
                    return View();
                }
                else
                {
                    string Query = "Insert into [Transaction] (UserId,Name,Date,Amount,Remark,Type,Created_at,Created_by)" +
                        $"values ('{id}','{wtvm.Name}',GETDATE(),'{wtvm.Amount}','{wtvm.Remark}','{"Withdraw"}',GETDATE(),'{wtvm.Name}')";
                    //If user doesn't exists it inserts data into database
                    int result = _helper.DMLTransaction(Query);
                    if (result > 0)
                    {
                        string Uquery = $"UPDATE [User] SET Updated_at = GETDATE(),Updated_by= '{wtvm.Name}' , Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') - '{wtvm.Amount}') WHERE OId = '{id}'";
                        int Uresult = _helper.DMLTransaction(Uquery);
                        if (Uresult > 0)
                        {
                            _logger.LogInformation("Withdraw completed, balance ammount updated!");
                            _logger.LogInformation("Redicted to Balance dashboard");
                            return RedirectToRoute("balance"); //Redirects to Home accounts index view
                        }
                    }
                }
            }
            catch(NullReferenceException e)
            {
                ViewBag.Error = "Error while withdraw, please try again!";
                _logger.LogError($"'{e}' exception..");
            } 
            return View();
        }

        [HttpGet]
        [Route("Home/Deposit")]
        [Authorize]
        public IActionResult Deposit()
        {
            _logger.LogInformation("Entered in Deposit form");
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
                    _logger.LogWarning("User wanted to deposit less than 10$");
                    return View();
                }
                var date = DateTime.Now;
                string Query = "Insert into [Transaction] (UserId,Name,Date,Amount,Remark,Type,Created_at,Created_by)" +
                    $"values ('{id}','{dtvm.Name}',GETDATE(),'{dtvm.Amount}','{dtvm.Remark}','{"Deposit"}',GETDATE(),'{dtvm.Name}')";
                //If user doesn't exists it inserts data into database
                int result = _helper.DMLTransaction(Query);
                if (result > 0)
                {
                    string Uquery = $"UPDATE [User] SET Updated_at = GETDATE(),Updated_by= '{dtvm.Name}' ,Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') + '{dtvm.Amount}') WHERE OId = '{id}'";
                    int Uresult = _helper.DMLTransaction(Uquery);
                    if (Uresult > 0)
                    {
                        _logger.LogInformation("Deposit completed, balance ammount updated!");
                        _logger.LogInformation("Redicted to Balance dashboard");
                        return RedirectToRoute("balance"); //Redirects to Home accounts index view
                    }
                }
            }
            catch(NullReferenceException e)
            {
                ViewBag.Error = "Error while depsit, please try again";
                _logger.LogInformation($"'{e}' Exception..");
            }
            
            
            return View();
        }
    }
}
