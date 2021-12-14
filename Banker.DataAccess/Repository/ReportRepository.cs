using Banker.Models.ViewModels.ReportModels;
using BankerLibrary.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankerLibrary.Repository
{
    public class ReportRepository:IReportRepository
    {

        private readonly IConfiguration _config;
        private readonly ILogger<ReportRepository> _logger;

        public ReportRepository(IConfiguration config, ILogger<ReportRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public List<CurrentMonthDepositViewModel> CurrentDeposit(int month, int id)
        {
            List<CurrentMonthDepositViewModel> cmvmlist = new List<CurrentMonthDepositViewModel>();
            var query = $"SELECT SUM([Amount])AS Total,[Source] FROM[dbo].[Transaction] WHERE[TransactionType] = 'Deposit' AND[UserId] = '{id}' AND Month([Date]) = '{month}'  AND datepart(yy, [Date]) = year(GetDate()) GROUP BY [Source] ORDER BY [Source]";
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
                                Remark = dataReader["Source"].ToString()
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
            return cmvmlist;
        }


        public List<CurrentMonthWithdrawViewModel> CurrentWithdraw(int month, int id)
        {
            List<CurrentMonthWithdrawViewModel> cmvmlist = new List<CurrentMonthWithdrawViewModel>();
            var query = $"SELECT SUM([Amount])AS Total,[Source] FROM[dbo].[Transaction] WHERE[TransactionType] = 'Withdraw' AND[UserId] = '{id}' AND Month([Date]) = '{month}'  AND datepart(yy, [Date]) = year(GetDate()) GROUP BY [Source] ORDER BY [Source]";
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
                                Remark = dataReader["Source"].ToString()
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
            return cmvmlist;

        }

        public List<Years> YearlyWithdraw(int id)
        {
            List<YearlyWithdrawViewModel> cmvmlist = new List<YearlyWithdrawViewModel>();
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Source] FROM[dbo].[Transaction] WHERE[TransactionType] = 'Withdraw' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
                "DATENAME(yy, [Date]),[Source]  ORDER BY DATENAME(yy, [Date]),[Source]";
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
                                Remark = dataReader["Source"].ToString()
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

            foreach (var y in years)
            {
                Years year = new Years
                {
                    Year = y,
                    List = cmvmlist.FindAll(x => x.Year == y)
                };
                yearList.Add(year);
            }

            return yearList;
        }

        public List<DYears> YearlyDeposit(int id)
        {
            List<YearlyDepositViewModel> cmvmlist = new List<YearlyDepositViewModel>();
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Source] FROM[dbo].[Transaction] WHERE[TransactionType] = 'Deposit' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
                "DATENAME(yy, [Date]),[Source]  ORDER BY DATENAME(yy, [Date]),[Source]";
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
                                Remark = dataReader["Source"].ToString()
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
            return yearList;
        }

        public List<DRemarks> YearlyCatDeposit(int id)
        {
            List<YearlyDepositCatViewModel> cmvmlist = new List<YearlyDepositCatViewModel>();
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Source] FROM[dbo].[Transaction] WHERE[TransactionType] = 'Deposit' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
               "[Source], DATENAME(yy, [Date])  ORDER BY [Source], DATENAME(yy, [Date])";
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
                                Remark = dataReader["Source"].ToString()
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
            return remarkList;
        }

        public List<Remarks> YearlyCatWithdraw(int id)
        {
            List<YearlyWithdrawCatViewModel> cmvmlist = new List<YearlyWithdrawCatViewModel>();
            var query = $"SELECT SUM([Amount])AS Total , DATENAME(yy, [Date]) AS _Year ,[Source] FROM[dbo].[Transaction] WHERE[TransactionType] = 'Withdraw' AND[UserId] = '{id}' AND[Date] >= DATEADD(year, -5, GETDATE()) GROUP BY " +
                "[Source], DATENAME(yy, [Date])  ORDER BY [Source], DATENAME(yy, [Date])";
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
                                Remark = dataReader["Source"].ToString()
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
            return remarkList;
        }


        public List<MYears> MyMDeposit(int id)
        {
            List<Monthly> mlist = new List<Monthly>();
            var mquery = "SELECT CASE { fn MONTH([Date]) } " +
            "when 1 then 'January' when 2 then 'February' when 3 then 'March' when 4 then 'April' when 5 then 'May'" +
            "when 6 then 'June'  when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'October'" +
            "when 11 then 'November' when 12 then 'December' END AS _Month,  SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year " +
            $"FROM[dbo].[Transaction] WHERE[TransactionType] = 'Deposit' AND[UserId] = '{id}' GROUP BY  DATENAME(yy, [Date]), " +
            "{ fn MONTH([Date]) } ORDER BY  DATENAME(yy, [Date]), { fn MONTH([Date]) }";

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
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
            return yearList;
        }

        public List<YearlyMonthlyDepositViewModel> MyYDeposit(int id)
        {
            List<YearlyMonthlyDepositViewModel> cmvmlist = new List<YearlyMonthlyDepositViewModel>();
            var yquery = $"SELECT SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year FROM[dbo].[Transaction] WHERE[TransactionType] = 'Deposit' AND[UserId] = '{id}' GROUP BY DATENAME(yy, [Date]) ORDER BY " +
            "DATENAME(yy, [Date])";

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
            return cmvmlist;
        }

        public List<WYears> MyMWithdraw(int id)
        {

            List<WMonthly> mlist = new List<WMonthly>();
            var mquery = "SELECT CASE { fn MONTH([Date]) } " +
               "when 1 then 'January' when 2 then 'February' when 3 then 'March' when 4 then 'April' when 5 then 'May'" +
               "when 6 then 'June'  when 7 then 'July' when 8 then 'August' when 9 then 'September' when 10 then 'October'" +
               "when 11 then 'November' when 12 then 'December' END AS _Month,  SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year " +
               $"FROM[dbo].[Transaction] WHERE[TransactionType] = 'Withdraw' AND[UserId] = '{id}' GROUP BY  DATENAME(yy, [Date]), " +
               "{ fn MONTH([Date]) } ORDER BY  DATENAME(yy, [Date]), { fn MONTH([Date]) }";

            string connectionString = _config["ConnectionStrings:DefaultConnection"];

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
            return yearList;
        }

        public List<YearlyMonthlyWithdrawViewModel> MyYWithdraw(int id)
        {
            List<YearlyMonthlyWithdrawViewModel> cmvmlist = new List<YearlyMonthlyWithdrawViewModel>();
            var yquery = $"SELECT SUM([Amount])AS Total, DATENAME(yy, [Date]) AS _Year FROM[dbo].[Transaction] WHERE[TransactionType] = 'Withdraw' AND[UserId] = '{id}' GROUP BY DATENAME(yy, [Date]) ORDER BY " +
            "DATENAME(yy, [Date])";

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
            return cmvmlist;
        }

    }
}
