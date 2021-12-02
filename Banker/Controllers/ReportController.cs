using Banker.Extensions;
using Banker.Models.ViewModels.ReportModels;
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
            List<CurrentMonthDepositViewModel> cmvmlist = new List<CurrentMonthDepositViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' AND datepart(mm, [Date]) = month(GetDate())  AND datepart(yy, [Date]) = year(GetDate()) GROUP BY [Remark] ORDER BY [Remark]";
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
                            CurrentMonthDepositViewModel cmvm = new CurrentMonthDepositViewModel
                            {
                                Total = Convert.ToDecimal(dataReader["Total"]),
                                Remark = dataReader["Remark"].ToString()
                            };

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


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyWithdraw")]
        public JsonResult GetCurrentWithdraw()
        {
            List<CurrentMonthWithdrawViewModel> cmvmlist = new List<CurrentMonthWithdrawViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Withdraw' AND[UserId] = '{id}' AND datepart(mm, [Date]) = month(GetDate()) AND datepart(yy, [Date]) = year(GetDate()) GROUP BY [Remark] ORDER BY [Remark]";
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
                            CurrentMonthWithdrawViewModel cmvm = new CurrentMonthWithdrawViewModel
                            {
                                Total = Convert.ToDecimal(dataReader["Total"]),
                                Remark = dataReader["Remark"].ToString()
                            };

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


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyWithdraw")]
        public JsonResult GetYearlyWithdraw()
        {
            List<YearlyWithdrawViewModel> cmvmlist = new List<YearlyWithdrawViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Withdraw' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY "+
                "DATENAME(yy, [Date]),[Remark]  ORDER BY DATENAME(yy, [Date]),[Remark]";
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
                            YearlyWithdrawViewModel ywvm = new YearlyWithdrawViewModel
                            {
                                Total = Convert.ToDecimal(dataReader["Total"]),
                                Year = dataReader["_Year"].ToString(),
                                Remark = dataReader["Remark"].ToString()
                            };

                            cmvmlist.Add(ywvm);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }
            List<Years> yearList = new List<Years>();

            var years = cmvmlist.Select(c => c.Year).Distinct().ToList();

            foreach(var y in years)
            {
                Years year = new Years
                {
                    Year = y,
                    List = cmvmlist.FindAll(x => x.Year == y)
                };
                yearList.Add(year);
            }
            return Json(yearList);
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyDeposit")]
        public JsonResult GetYearlyDeposit()
        {
            List<YearlyDepositViewModel> cmvmlist = new List<YearlyDepositViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
                "DATENAME(yy, [Date]),[Remark]  ORDER BY DATENAME(yy, [Date]),[Remark]";
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
                            YearlyDepositViewModel ywvm = new YearlyDepositViewModel
                            {
                                Total = Convert.ToDecimal(dataReader["Total"]),
                                Year = dataReader["_Year"].ToString(),
                                Remark = dataReader["Remark"].ToString()
                            };

                            cmvmlist.Add(ywvm);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }
            List<DYears> yearList = new List<DYears>();

            var years = cmvmlist.Select(c => c.Year).Distinct().ToList();

            foreach (var y in years)
            {
                DYears year = new DYears
                {
                    Year = y,
                    List = cmvmlist.FindAll(x => x.Year == y)
                };
                yearList.Add(year);
            }
            return Json(yearList);
        }
    }
}
