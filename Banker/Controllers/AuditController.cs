using BankerLibrary.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Controllers
{
    public class AuditController : Controller
    {
        private readonly ILogger<AuditController> _logger;
        private readonly IAuditRepository _audit;

        public AuditController(ILogger<AuditController> logger, IAuditRepository audit)
        {
            _logger = logger;
            _audit = audit;
        }

        [Authorize(Roles = "admin")]
        [Route("Home/Audit")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("Home/Audit/GetAll")]
        public JsonResult GetAudit()
        {
            
            var audit = _audit.GetAudit();
            _logger.LogInformation("Entered in Audit Dashboard");
            return Json(new { data = audit.AuditList });
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("/Home/Audit/TType/{type}")]
        public JsonResult GetByTransType(string type)
        {
            var audit = _audit.GetAuditType(type);
            return Json(new { data = audit.AuditList });
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("/Home/Audit/LogType/{type}")]
        public JsonResult GetByLogType(string type)
        {
            var audit = _audit.GetLogType(type);
            return Json(new { data = audit.AuditList });
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("/Home/Audit/Date/{date}")]
        public JsonResult GetByDate(string date)
        {
            var audit = _audit.GetDate(date);
            return Json(new { data = audit.AuditList });
        }
    }
}
