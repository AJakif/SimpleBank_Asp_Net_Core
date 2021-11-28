using Banker.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Helpers
{
    public class CommonHelper : ICommonHelper
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CommonHelper> _logger;

        public CommonHelper(IConfiguration config, ILogger<CommonHelper> logger)
        {
            _config = config;
            _logger = logger;
        }

        public UserViewModel GetUserByEmail (string query)
        {
            _logger.LogInformation("Entered in GetUserByEmail..");
            UserViewModel user = new UserViewModel();

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                using(SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            user.OId = Convert.ToInt32(dataReader["OId"]);
                            user.Name = dataReader["Name"].ToString();
                            user.Address = dataReader["Address"].ToString();
                            user.Gender = dataReader["Gender"].ToString();
                            user.Phone = dataReader["Phone"].ToString();
                            user.Email = dataReader["Email"].ToString();
                            user.Password = dataReader["Password"].ToString();
                            user.Balance = Convert.ToDecimal(dataReader["Balance"]);

                        }
                    }
                    catch(NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }
            return user;
        }

        public UserViewModel GetUserById(int id)
        {
            _logger.LogInformation("Entered in GetUserById..");
            UserViewModel user = new UserViewModel();

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"Select Balance from [User] where OId = '{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            user.Balance = Convert.ToDecimal(dataReader["Balance"]);

                        }
                    }
                    catch(NullReferenceException e )
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }
            return user;
        }

        public int DMLTransaction(string Query)
        {
            _logger.LogInformation("Entered in DMLTransaction..");
            int Result;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = Query;
                SqlCommand command = new SqlCommand(sql, connection);
                Result = command.ExecuteNonQuery();
                _logger.LogInformation("Data Inserted");
                connection.Close();
            }
            return Result;
        }
        
        public bool UserAlreadyExists(string query)
        {
            _logger.LogInformation("Entered in UserAlreadyExists..");
            bool flag = false;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader dr = command.ExecuteReader();
                try
                {
                    if (dr.HasRows)
                    {
                        flag = true;
                        _logger.LogInformation("User Already Exist..");
                    }
                }
                catch (NullReferenceException e)
                {
                    _logger.LogWarning($"'{e}' Exception");
                }

                connection.Close();

            }
            return flag;

        }

        public List<Transection> GetTransaction(int id)
        {
            List<Transection> TransactionList = new List<Transection>();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"Select TOP 20 * from [Transaction] where  UserId = '{id}'ORDER BY Date DESC";
                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            Transection trans = new Transection
                            {
                                OId = Convert.ToInt32(dataReader["OId"]),
                                UserId = Convert.ToInt32(dataReader["UserId"]),
                                Name = dataReader["Name"].ToString(),
                                Date = Convert.ToDateTime(dataReader["Date"]),
                                Amount = Convert.ToDecimal(dataReader["Amount"]),
                                Remark = dataReader["Remark"].ToString(),
                                Type = dataReader["Type"].ToString()
                            };

                            TransactionList.Add(trans);

                        }
                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }

                }
                connection.Close();
            }
            return (TransactionList);
        }

        public HistoryViewModel GetHistory(int id)
        {
            List<HistoryViewModel> historyList = new List<HistoryViewModel>();
            
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
                            if(dataReader != null)
                            {
                                HistoryViewModel history = new HistoryViewModel
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

            HistoryViewModel obj = new HistoryViewModel
            {
                HistoryList = historyList
            };

            return obj;
        }
    }
}
