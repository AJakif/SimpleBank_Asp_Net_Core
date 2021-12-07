using Banker.Extensions;
using Banker.Helpers;
using Banker.Models.ViewModels;
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
        private readonly ICommonHelper _helper;

        public ReportController(ILogger<ReportController> logger, IConfiguration config, ICommonHelper helper)
        {
            _logger = logger;
            _config = config;
            _helper = helper;
        }

        [Authorize]
        [Route("/Home/Report")]
        public IActionResult Report()
        {
            (int id, _) = HttpContext.GetUserInfo();
            CollectData Model = new CollectData
            {
                Transections = _helper.GetTransaction(id),
                User = _helper.GetUserById(id)
            };

            return View(Model);
        }

        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyDeposit/{month}")]
        public JsonResult GetCurrentDeposit(int month)
        {
            List<CurrentMonthDepositViewModel> cmvmlist = new List<CurrentMonthDepositViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' AND Month([Date]) = '{month}'  AND datepart(yy, [Date]) = year(GetDate()) GROUP BY [Remark] ORDER BY [Remark]";
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
        [Route("/Home/Report/monthlyWithdraw/{month}")]
        public JsonResult GetCurrentWithdraw(int month)
        {
            List<CurrentMonthWithdrawViewModel> cmvmlist = new List<CurrentMonthWithdrawViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Withdraw' AND[UserId] = '{id}' AND Month([Date]) = '{month}'  AND datepart(yy, [Date]) = year(GetDate()) GROUP BY [Remark] ORDER BY [Remark]";
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


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyCatDeposit")]
        public JsonResult GetYearlyCatDeposit()
        {
            List<YearlyDepositCatViewModel> cmvmlist = new List<YearlyDepositCatViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
                "[Remark], DATENAME(yy, [Date])  ORDER BY [Remark], DATENAME(yy, [Date])";
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
                            YearlyDepositCatViewModel ywvm = new YearlyDepositCatViewModel
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
            List<DRemarks> remarkList = new List<DRemarks>();

            var remarks = cmvmlist.Select(c => c.Remark).Distinct().ToList();

            foreach (var r in remarks)
            {
                DRemarks remark = new DRemarks
                {
                    Remark = r,
                    List = cmvmlist.FindAll(x => x.Remark == r)
                };
                remarkList.Add(remark);
            }
            return Json(remarkList);
        }

        [HttpGet]
        [Authorize]
        [Route("/Home/Report/yearlyCatWithdraw")]
        public JsonResult GetYearlyCatWithdraw()
        {
            List<YearlyWithdrawCatViewModel> cmvmlist = new List<YearlyWithdrawCatViewModel>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Remark] FROM[dbo].[Transaction] WHERE[Type] = 'Withdraw' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
                "[Remark], DATENAME(yy, [Date])  ORDER BY [Remark], DATENAME(yy, [Date])";
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
                            YearlyWithdrawCatViewModel ywvm = new YearlyWithdrawCatViewModel
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
            List<Remarks> remarkList = new List<Remarks>();

            var remarks = cmvmlist.Select(c => c.Remark).Distinct().ToList();

            foreach (var r in remarks)
            {
                Remarks remark = new Remarks
                {
                    Remark = r,
                    List = cmvmlist.FindAll(x => x.Remark == r)
                };
                remarkList.Add(remark);
            }
            return Json(remarkList);
        }


        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyYearlyDeposit")]
        public JsonResult GetMYDeposit()
        {
            List<YearlyMonthlyDepositViewModel> cmvmlist = new List<YearlyMonthlyDepositViewModel>();
            List<Monthly> mlist = new List<Monthly>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch

            var yquery = $"SELECT SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' GROUP BY DATENAME(yy, [Date]) ORDER BY " +
            "DATENAME(yy, [Date])";

            var mquery = "SELECT CASE { fn MONTH([Date]) } " +
            "when 1 then 'January' when 2 then 'February' when 3 then 'March' when 4 then 'April' when 5 then 'May'" +
            "when 6 then 'June'  when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'October'" +
            "when 11 then 'November' when 12 then 'December' END AS _Month,  SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year " +
            $"FROM[dbo].[Transaction] WHERE[Type] = 'Deposit' AND[UserId] = '{id}' GROUP BY  DATENAME(yy, [Date]), " +
            "{ fn MONTH([Date]) } ORDER BY  DATENAME(yy, [Date]), { fn MONTH([Date]) }";

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string ysql = yquery;
                
                SqlCommand command = new SqlCommand(ysql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            YearlyMonthlyDepositViewModel cmvm = new YearlyMonthlyDepositViewModel
                            {
                                YTotal = Convert.ToDecimal(dataReader["Total"]),
                                Year = dataReader["_Year"].ToString()
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


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string msql = mquery;

                SqlCommand command = new SqlCommand(msql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            Monthly cmvm = new Monthly
                            {
                                MTotal = Convert.ToDecimal(dataReader["Total"]),
                                Month = dataReader["_Month"].ToString(),
                                Year = dataReader["_Year"].ToString()
                            };

                            mlist.Add(cmvm);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }

            List<MYears> yearList = new List<MYears>();

            var years = mlist.Select(c => c.Year).Distinct().ToList();

            foreach (string y in years)
            {
                MYears year = new MYears
                {
                    Year = y,
                    List = mlist.FindAll(x => x.Year == y)
                };
                yearList.Add(year);
            }
            return Json(new { FirstList = yearList, SecondList = cmvmlist });
        }



        [HttpGet]
        [Authorize]
        [Route("/Home/Report/monthlyYearlyWithdraw")]
        public JsonResult GetMYWithdraw()
        {
            List<YearlyMonthlyWithdrawViewModel> cmvmlist = new List<YearlyMonthlyWithdrawViewModel>();
            List<WMonthly> mlist = new List<WMonthly>();
            (int id, _) = HttpContext.GetUserInfo(); //try-catch

            var yquery = $"SELECT SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year FROM[dbo].[Transaction] WHERE[Type] = 'Withdraw' AND[UserId] = '{id}' GROUP BY DATENAME(yy, [Date]) ORDER BY " +
            "DATENAME(yy, [Date])";

            var mquery = "SELECT CASE { fn MONTH([Date]) } "+ 
            "when 1 then 'January' when 2 then 'February' when 3 then 'March' when 4 then 'April' when 5 then 'May'"+
            "when 6 then 'June'  when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'October'"+
            "when 11 then 'November' when 12 then 'December' END AS _Month,  SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year " +
            $"FROM[dbo].[Transaction] WHERE[Type] = 'Withdraw' AND[UserId] = '{id}' GROUP BY  DATENAME(yy, [Date]), "+
            "{ fn MONTH([Date]) } ORDER BY  DATENAME(yy, [Date]), { fn MONTH([Date]) }";

            string connectionString = _config["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string ysql = yquery;

                SqlCommand command = new SqlCommand(ysql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            YearlyMonthlyWithdrawViewModel cmvm = new YearlyMonthlyWithdrawViewModel
                            {
                                YTotal = Convert.ToDecimal(dataReader["Total"]),
                                Year = dataReader["_Year"].ToString()
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


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string msql = mquery;

                SqlCommand command = new SqlCommand(msql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            WMonthly cmvm = new WMonthly
                            {
                                MTotal = Convert.ToDecimal(dataReader["Total"]),
                                Month = dataReader["_Month"].ToString(),
                                Year = dataReader["_Year"].ToString()
                            };

                            mlist.Add(cmvm);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }

            List<WYears> yearList = new List<WYears>();

            var years = mlist.Select(c => c.Year).Distinct().ToList();

            foreach (string y in years)
            {
                WYears year = new WYears
                {
                    Year = y,
                    List = mlist.FindAll(x => x.Year == y)
                };
                yearList.Add(year);
            }
            return Json(new { FirstList = yearList, SecondList = cmvmlist });
        }

    }
}
