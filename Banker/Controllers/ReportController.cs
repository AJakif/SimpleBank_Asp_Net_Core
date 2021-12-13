using Banker.Extensions;
using Banker.Models.ViewModels;
using Banker.Models.ViewModels.ReportModels;
using BankerLibrary.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Controllers
{
    public class ReportController : Controller
    {
        private readonly ITransactionRepository _transaction;
        private readonly IUserRepository _user;
        private readonly IReportRepository _report;

        public ReportController(ITransactionRepository transaction, IUserRepository user, IReportRepository report)
        {
            _transaction = transaction;
            _user = user;
            _report = report;
        }

        [Authorize]
        [Route("/Home/Report")]
        public IActionResult Report()
        {
            (int id, _) = HttpContext.GetUserInfo();
            CollectData Model = new CollectData
            {
                
                Transections = _transaction.GetTransactionList(id),
                User = _user.GetUserById(id)
            };

            return View(Model);
        }

        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyDeposit/{month}")]
        public JsonResult GetCurrentDeposit(int month)
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var json = JsonConvert.SerializeObject(_report.CurrentDeposit(month, id));
            return Json(json);
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyWithdraw/{month}")]
        public JsonResult GetCurrentWithdraw(int month)
        {
            
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var json = JsonConvert.SerializeObject(_report.CurrentWithdraw(month,id));
            return Json(json);
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyWithdraw")]
        public JsonResult GetYearlyWithdraw()
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            
            return Json(_report.YearlyWithdraw(id));
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyDeposit")]
        public JsonResult GetYearlyDeposit()
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            
            return Json(_report.YearlyDeposit(id));
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyCatDeposit")]
        public JsonResult GetYearlyCatDeposit()
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            return Json(_report.YearlyCatDeposit(id));
        }

        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyCatWithdraw")]
        public JsonResult GetYearlyCatWithdraw()
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            
            return Json(_report.YearlyCatWithdraw(id));
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyYearlyDeposit")]
        public JsonResult GetMYDeposit()
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            return Json(new { FirstList = _report.MyMDeposit(id), SecondList = _report.MyYDeposit(id) });
        }



        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyYearlyWithdraw")]
        public JsonResult GetMYWithdraw()
        {
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            return Json(new { FirstList = _report.MyMWithdraw(id), SecondList = _report.MyYWithdraw(id) });
        }

    }
}
