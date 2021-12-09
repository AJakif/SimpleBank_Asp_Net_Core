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
                Transections = _helper.GetTransactionList(id),
                User = _helper.GetUserById(id)
            };
            _logger.LogInformation("Entered in Balance Dashboard");
            return View(Model);
        }


        [HttpGet]
        [Route("Home/Transaction/GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            (int id, _) = HttpContext.GetUserInfo();

            var Transections = _helper.GetTransactionList(id);
            
            return Json(new { data = Transections });
        }

        [HttpPost]
        [Route("/Home/Transaction/Edit")]
        public JsonResult Edit(int id)
        {
            var transection = _helper.GetTransaction(id);
            return Json (transection);

        }

        [HttpPost]
        public IActionResult Balance(CollectData collect)
        {
            try
            {
                string Query = $"UPDATE[dbo].[Transaction] SET [Source] = '{collect.Transection.Source}' ,[Type] = '{collect.Transection.Type}' ,[Updated_at] = GETDATE() ,[Updated_by] = '{collect.Transection.Name}' " +
            $"WHERE OId = '{collect.Transection.OId}'";
                int Uresult = _helper.DMLTransaction(Query);
                if (Uresult > 0)
                {
                    (int id, _) = HttpContext.GetUserInfo();
                    string Iquery = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                    $",[Created_at],[Created_by]) VALUES ('{id}','{collect.Transection.TransId}','{collect.Transection.Name}',GETDATE(),'{collect.Transection.Amount}','{collect.Transection.Source}','{collect.Transection.TransactionType}','{collect.Transection.Type}','{"Edited"}',GETDATE(),'{collect.Transection.Name}')";
                    int Iresult = _helper.DMLTransaction(Iquery);
                    if (Iresult > 0)
                    {
                        
                        CollectData Model = new CollectData
                        {
                            Transections = _helper.GetTransactionList(id),
                            User = _helper.GetUserById(id)
                        };
                        return View(Model);
                    }
                    else
                    {
                        ViewBag.Error = "Audit insertion error";
                        RedirectToRoute("balance");
                    }
                }
                else
                {
                    ViewBag.Error = "Error while Update transaction.";
                    RedirectToRoute("balance");
                }
            }
            catch (NullReferenceException e)
            {
                ViewBag.Error = "Error while Edit, please try again!";
                _logger.LogInformation($"'{e}' Exception..");
            }
            return View();
        }

        [HttpDelete]
        [Route("/Home/Transaction/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var transection = _helper.GetTransaction(id);
            try
            {
                string Query = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                    $",[Created_at],[Created_by]) VALUES ('{transection.UserId}','{transection.TransId}','{transection.Name}',GETDATE(),'{transection.Amount}','{transection.Source}','{transection.TransactionType}','{transection.Type}','{"Deleted"}',GETDATE(),'{transection.Name}')";
                
                int Uresult = _helper.DMLTransaction(Query);
                if (Uresult > 0)
                {
                    string Iquery = $"DELETE FROM [dbo].[Transaction]  WHERE OId = '{transection.OId}' ";
                    int Iresult = _helper.DMLTransaction(Iquery);
                    if (Iresult > 0)
                    {
                        return Json(new { success = true, message = "Transaction Deleted successful" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error while Deleting transaction." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Error while insert into audit table." });
                }
            }
            catch (NullReferenceException)
            {
                return Json(new { success = false, message = "Error while Delete, please try again!" });
            }
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
                    var date = DateTime.Now;
                    string transId = date.ToString("yyyyMMdd-HHmmssfff");
                    string Query = "Insert into [Transaction] (UserId,TransId,Name,Date,Amount,Source,TransactionType,Type,Created_at,Created_by)" +
                        $"values ('{id}','{transId}','{wtvm.Name}',GETDATE(),'{wtvm.Amount}','{wtvm.Source}','{"Withdraw"}','{wtvm.Type}',GETDATE(),'{wtvm.Name}')";
                    //If user doesn't exists it inserts data into database
                    int result = _helper.DMLTransaction(Query);
                    if (result > 0)
                    {
                        string Uquery = $"UPDATE [User] SET Updated_at = GETDATE(),Updated_by= '{wtvm.Name}' , Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') - '{wtvm.Amount}') WHERE OId = '{id}'";
                        int Uresult = _helper.DMLTransaction(Uquery);
                        if (Uresult > 0)
                        {
                            string Iquery = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                            $",[Created_at],[Created_by]) VALUES ('{id}','{transId}','{wtvm.Name}',GETDATE(),'{wtvm.Amount}','{wtvm.Source}','{"Withdraw"}','{wtvm.Type}','{"Added"}',GETDATE(),'{wtvm.Name}')";
                            int Iresult = _helper.DMLTransaction(Iquery);
                            if(Iresult > 0)
                            {
                                _logger.LogInformation("Withdraw completed, balance ammount updated!");
                                _logger.LogInformation("Redicted to Balance dashboard");
                                return RedirectToRoute("balance"); //Redirects to Home accounts index view
                            }
                            
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
                string transId = date.ToString("yyyyMMdd-HHmmssfff");
                string Query = "Insert into [Transaction] (UserId,TransId,Name,Date,Amount,Source,TransactionType,Type,Created_at,Created_by)" +
                    $"values ('{id}','{transId}','{dtvm.Name}',GETDATE(),'{dtvm.Amount}','{dtvm.Source}','{"Deposit"}','{dtvm.Type}',GETDATE(),'{dtvm.Name}')";
                //If user doesn't exists it inserts data into database
                int result = _helper.DMLTransaction(Query);
                if (result > 0)
                {
                    string Uquery = $"UPDATE [User] SET Updated_at = GETDATE(),Updated_by= '{dtvm.Name}' ,Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') + '{dtvm.Amount}') WHERE OId = '{id}'";
                    int Uresult = _helper.DMLTransaction(Uquery);
                    if (Uresult > 0)
                    {
                        string Iquery = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                           $",[Created_at],[Created_by]) VALUES ('{id}','{transId}','{dtvm.Name}',GETDATE(),'{dtvm.Amount}','{dtvm.Source}','{"Withdraw"}','{dtvm.Type}','{"Added"}',GETDATE(),'{dtvm.Name}')";
                        int Iresult = _helper.DMLTransaction(Iquery);
                        if (Iresult > 0)
                        {
                            _logger.LogInformation("Deposit completed, balance ammount updated!");
                            _logger.LogInformation("Redicted to Balance dashboard");
                            return RedirectToRoute("balance"); //Redirects to Home accounts index view
                        }
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
