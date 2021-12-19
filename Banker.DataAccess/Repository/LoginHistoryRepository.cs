using Banker.Models;
using BankerLibrary.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository
{
    public class LoginHistoryRepository : ILoginHistoryRepository
    {

        private readonly IConfiguration _config;
        private readonly ILogger<LoginHistoryRepository> _logger;

        public LoginHistoryRepository(IConfiguration config, ILogger<LoginHistoryRepository> logger)
        {
            _config = config;
            _logger = logger;
        }
        public int LoginHistory(UserModel userDetails)
        {
            string Query = "Insert into [LoginHistory] (UserId,DateTime,Created_at,Created_by)" + $"values ('{userDetails.OId}',GETDATE(),GETDATE(),(SELECT Name FROM[User] WHERE OId = '{userDetails.OId}'))";
            _logger.LogInformation("Entered in DMLTransaction..");
            int Result;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                string sql = Query;
                SqlCommand command = new SqlCommand(sql, connection);
                Result = command.ExecuteNonQuery();
                _logger.LogInformation("Data Inserted");
                connection.Close();
                return Result;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"'{e}' Exception..");
                connection.Close();
                return -1;
            }
        }

        public HistoryModel GetHistory(int id)
        {
            List<HistoryModel> historyList = new List<HistoryModel>();

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"Select TOP 20 * from [LoginHistory] where  UserId = '{id}'ORDER BY DateTime DESC";
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
                                HistoryModel history = new HistoryModel
                                {
                                    OId = Convert.ToInt32(dataReader["OId"]),
                                    UserId = Convert.ToInt32(dataReader["UserId"]),
                                    DateTime = Convert.ToDateTime(dataReader["DateTime"])
                                };

                                historyList.Add(history);
                            }
                        }
                        catch (NullReferenceException e)
                        {
                            _logger.LogError($"'{e}' Exception");
                        }
                    }
                }
                connection.Close();
            }

            HistoryModel obj = new HistoryModel
            {
                HistoryList = historyList
            };

            return obj;
        }
    }
}
