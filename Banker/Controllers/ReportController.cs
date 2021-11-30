using Banker.Extensions;
using Banker.Models.ViewModels;
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
        private readonly ILogger<ReportController> _logger;
        private readonly IConfiguration _config;

        public ReportController(ILogger<ReportController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [Authorize]
        [Route("/Home/Report")]
        public IActionResult Report()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyDeposit")]
        public JsonResult GetCurrentDeposit()
        {
            List<CurrentMonthViewModel>cmvmlist = new List<CurrentMonthViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' AND datepart(mm, [Date]) = month(GetDate()) GROUP BY [Remark] ORDER BY [Remark]";
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            CurrentMonthViewModel cmvm = new CurrentMonthViewModel();

                            cmvm.Total = Convert.ToDecimal(dataReader["Total"]);
                            cmvm.Remark = dataReader["Remark"].ToString();

                            cmvmlist.Add(cmvm);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }

            var json = JsonConvert.SerializeObject(cmvmlist);
            return Json(json);
        }
    }
}
