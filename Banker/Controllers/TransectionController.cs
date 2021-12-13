using Banker.Extensions;
using Banker.Models.ViewModels;
using BankerLibrary.Repository.IRepository;
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
        private readonly ILogger<TransectionController> _logger;
        private readonly ITransactionRepository _transaction;
        private readonly IAuditRepository _audit;
        private readonly IUserRepository _user;

        public TransectionController(IConfiguration config, ILogger<TransectionController> logger, ITransactionRepository transaction, IAuditRepository audit, IUserRepository user)
        {
            _config = config;
            _logger = logger;
            _transaction = transaction;
            _audit = audit;
            _user = user;
        }

        [HttpGet]
        [Route("Home/Balance")]
        [Authorize]
        public IActionResult Balance()
        {
            (int id, _) = HttpContext.GetUserInfo();
            CollectData Model = new CollectData
            {
                Transections = _transaction.GetTransactionList(id),
                User = _user.GetUserById(id)
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

            var Transections = _transaction.GetTransactionList(id);
            
            return Json(new { data = Transections });
        }

        [HttpPost]
        [Route("/Home/Transaction/Edit")]
        public JsonResult Edit(int id)
        {
            var transection = _transaction.GetTransaction(id);
            return Json (transection);

        }

        [HttpPost]
        public IActionResult Balance(CollectData collect)
        {
            try
            {
                int Uresult = _transaction.Transaction(collect);
                if (Uresult > 0)
                {
                    (int id, _) = HttpContext.GetUserInfo();
                    int Iresult = _audit.InsertEditAudit(collect,id);
                    if (Iresult > 0)
                    {
                        
                        CollectData Model = new CollectData
                        {
                            Transections = _transaction.GetTransactionList(id),
                            User = _user.GetUserById(id)
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
            var transection = _transaction.GetTransaction(id);
            try
            { 
                int Uresult = _audit.InsertDeleteAudit(transection);
                if (Uresult > 0)
                {
                    
                    int Iresult = _transaction.DeleteTransaction(transection);
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
                uvm = _user.GetUserById(id);
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
                    
                    //If user doesn't exists it inserts data into database
                    int result = _transaction.Withdraw(wtvm, id, transId);
                    if (result > 0)
                    {
                        int Uresult = _user.UpdateWithdrawBalance(wtvm,id);
                        if (Uresult > 0)
                        {
                            int Iresult = _audit.InsertAddAudit(wtvm,id,transId);
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
                uvm = _user.GetUserById(id);
                if (dtvm.Amount <= 9)
                {
                    ViewBag.Error = "You cannot deposit less than 10$!";
                    _logger.LogWarning("User wanted to deposit less than 10$");
                    return View();
                }
                var date = DateTime.Now;
                string transId = date.ToString("yyyyMMdd-HHmmssfff");
                //If user doesn't exists it inserts data into database
                int result = _transaction.Deposit(dtvm, id, transId);
                if (result > 0)
                {
                    int Uresult = _user.UpdateDepositBalance(dtvm, id);
                    if (Uresult > 0)
                    {
                        int Iresult = _audit.InsertAddAudit(dtvm, id, transId);
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
