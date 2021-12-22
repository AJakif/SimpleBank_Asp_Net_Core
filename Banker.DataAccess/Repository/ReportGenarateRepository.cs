using Banker.BusinessLayer.BO;
using Banker.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.DataAccess.Repository
{
    public class ReportGenarateRepository : IReportGenarateRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ReportGenarateRepository> _logger;

        public ReportGenarateRepository(IConfiguration config, ILogger<ReportGenarateRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public ReportDataBO GetReport(int id)
        {
            UserBO user = new UserBO();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT [Name],[Balance] FROM[Bank].[dbo].[User] WHERE[OId] = '{id}'";
            string sql = query;
            SqlCommand command = new SqlCommand(sql, connection);
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read()) //make it single user
                {
                    try
                    {
                        if (dataReader != null)
                        {
                            user.Name = dataReader["Name"].ToString();
                            user.Amount = Convert.ToDecimal(dataReader["Balance"]);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogError($"'{e}' Exception");
                    }
                }
            }
            connection.Close();

            List<MonthlyDepositReportBO> dlist = new List<MonthlyDepositReportBO>();
            connection.Open();
            string Query = "SELECT CASE { fn MONTH([Date]) } when 1 then 'January' when 2 then 'February'"+

            "when 3 then 'March' when 4 then 'April' when 5 then 'May' when 6 then 'June' when 7 then 'July' "+
            " when 8 then 'August' when 9 then 'September' when 10 then 'October' when 11 then 'November' when 12 then 'December'"+
            $" END AS _Month, SUM([Amount])AS Total FROM[dbo].[Transaction] WHERE[TransactionType] = 'Deposit' AND[UserId] = '{id}' AND datepart(yy, [Date]) = year(GetDate())"+
            " GROUP BY { fn MONTH([Date]) } ORDER BY { fn MONTH([Date]) }";
            string Sql = Query;
            SqlCommand Command = new SqlCommand(Sql, connection);
            using (SqlDataReader dataReader = Command.ExecuteReader())
            {
                while (dataReader.Read()) //make it single user
                {
                    try
                    {
                        if (dataReader != null)
                        {
                            MonthlyDepositReportBO deposit = new MonthlyDepositReportBO
                            {
                                Month = dataReader["_Month"].ToString(),
                                Total = Convert.ToDecimal(dataReader["Total"])
                            };
                            dlist.Add(deposit);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogError($"'{e}' Exception");
                    }
                }
            }
            connection.Close();

            List<MonthlyWithdrawReportBO> wlist = new List<MonthlyWithdrawReportBO>();
            connection.Open();
            string wquery = "SELECT CASE { fn MONTH([Date]) } when 1 then 'January' when 2 then 'February'" +

            "when 3 then 'March' when 4 then 'April' when 5 then 'May' when 6 then 'June' when 7 then 'July' " +
            " when 8 then 'August' when 9 then 'September' when 10 then 'October' when 11 then 'November' when 12 then 'December'" +
            $" END AS _Month, SUM([Amount])AS Total FROM[dbo].[Transaction] WHERE[TransactionType] = 'Withdraw' AND[UserId] = '{id}' AND datepart(yy, [Date]) = year(GetDate())" +
            " GROUP BY { fn MONTH([Date]) } ORDER BY { fn MONTH([Date]) }";
            string wSql = wquery;
            SqlCommand wCommand = new SqlCommand(wSql, connection);
            using (SqlDataReader dataReader = wCommand.ExecuteReader())
            {
                while (dataReader.Read()) //make it single user
                {
                    try
                    {
                        if (dataReader != null)
                        {
                            MonthlyWithdrawReportBO Withdraw = new MonthlyWithdrawReportBO
                            {
                                Month = dataReader["_Month"].ToString(),
                                Total = Convert.ToDecimal(dataReader["Total"])
                            };
                            wlist.Add(Withdraw);
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogError($"'{e}' Exception");
                    }
                }
            }
            connection.Close();


            ReportGenarateBO rgbo = new ReportGenarateBO
            {
                DepositList = dlist,
                WithdrawList = wlist,
            };

            ReportDataBO data = new ReportDataBO
            {
                User = user,
                Data = rgbo
            };

            return data;
        }
    }
}
