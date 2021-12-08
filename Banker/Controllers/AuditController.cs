using Banker.Helpers;
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

        private readonly IConfiguration _config;
        private readonly ICommonHelper _helper;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IConfiguration config, ICommonHelper helper, ILogger<AuditController> logger)
        {
            _config = config;
            _helper = helper;
            _logger = logger;
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
            string query = $"Select * from [TansactionAudit] ORDER BY Date DESC";
            var audit = _helper.GetAudit(query);
            _logger.LogInformation("Entered in Audit Dashboard");
            return Json(new { data = audit.AuditList });
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("/Home/Audit/TType/{type}")]
        public JsonResult GetByTransType(int type)
        {
            string query = $"SELECT[OId] ,[UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType] " +
            $"FROM[dbo].[TansactionAudit] WHERE[TransactionType] = '{type}'";
            var audit = _helper.GetAudit(query);
            return Json(new { data = audit.AuditList });
        }
    }
}
