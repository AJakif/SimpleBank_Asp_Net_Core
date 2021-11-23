using Banker.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Helpers
{
    public class CommonHelper
    {
        private readonly IConfiguration _config;

        public CommonHelper(IConfiguration config)
        {
            _config = config;
        }


        public UserViewModel GetUserByEmail (string query)
        {
            UserViewModel user = new UserViewModel();

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                using(SqlDataReader dataReader = command.ExecuteReader())
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
                        user.Balance = Convert.ToDouble(dataReader["Balance"]);

                    }
                }
                connection.Close();
            }
            return user;
        }

        internal UserViewModel GetUserById(int id)
        {
            UserViewModel user = new UserViewModel();

            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"Select Balance from [User] where OId = '{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read()) //make it single user
                    {
                        user.Balance = Convert.ToDouble(dataReader["Balance"]);

                    }
                }
                connection.Close();
            }
            return user;
        }

        public int DMLTransaction(string Query)
        {
            int Result;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = Query;
                SqlCommand command = new SqlCommand(sql, connection);
                Result = command.ExecuteNonQuery();
                connection.Close();
            }
            return Result;
        }
        
        public bool UserAlreadyExists(string query)
        {
            bool flag = false;
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader dr = command.ExecuteReader();
                if(dr.HasRows)
                {
                    flag = true;
                }
                connection.Close();

            }
            return flag;

        }

        public static int ConvertToInt(string str)
        {
            int result = 0;

            if (string.IsNullOrEmpty(str)) {
                return result;
            }
            
            try
            {
                result = int.Parse(str);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
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
                    while (dataReader.Read()) //make it single user
                    {
                        Transection trans = new Transection
                        {
                            OId = Convert.ToInt32(dataReader["OId"]),
                            UserId = Convert.ToInt32(dataReader["UserId"]),
                            Name = dataReader["Name"].ToString(),
                            Date = Convert.ToDateTime(dataReader["Date"]),
                            Amount = Convert.ToDouble(dataReader["Amount"]),
                            Remark = dataReader["Remark"].ToString(),
                            Type = dataReader["Type"].ToString()
                        };

                        TransactionList.Add(trans);

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
                        HistoryViewModel history = new HistoryViewModel
                        {
                            OId = Convert.ToInt32(dataReader["OId"]),
                            UserId = Convert.ToInt32(dataReader["UserId"]),
                            DateTime = Convert.ToDateTime(dataReader["DateTime"])
                        };

                        historyList.Add(history);

                    }
                }
                connection.Close();
            }

            HistoryViewModel obj = new HistoryViewModel
            {
                historyList = historyList
            };

            return obj;
        }
    }
}
