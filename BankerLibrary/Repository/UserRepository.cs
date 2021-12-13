using Banker.Models.ViewModels;
using BankerLibrary.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration config, ILogger<UserRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public bool UserAlreadyExists(RegisterViewModel rvm)
        {
            string query = $"Select * from [User] where Name='{rvm.Name}'" + $"OR Email = '{rvm.Email}'";
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

        public int Register(RegisterViewModel rvm)
        {
            //if user exists then returns to Account controller and redirects to register view
            string Query = "Insert into [User] (Name,Address,Gender,Role,Phone,Email,Password,Balance,Created_at,Created_by)" +
                $"values ('{rvm.Name}','{rvm.Address}','{rvm.Gender}','customer','{rvm.Phone}','{rvm.Email}','{rvm.Password}','{100}',GETDATE(),'{rvm.Name}')";
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

        public UserViewModel GetUserByEmail(LoginViewModel lvm)
        {
            string query = $"select * from [User] where Email='{lvm.Email}' and Password='{lvm.Password}'";
            _logger.LogInformation("Login query innitialized and GetUserByEmail class called in common helper class");

            _logger.LogInformation("Entered in GetUserByEmail..");
            UserViewModel user = new UserViewModel();

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
                            user.OId = Convert.ToInt32(dataReader["OId"]);
                            user.Name = dataReader["Name"].ToString();
                            user.Address = dataReader["Address"].ToString();
                            user.Gender = dataReader["Gender"].ToString();
                            user.Phone = dataReader["Phone"].ToString();
                            user.Email = dataReader["Email"].ToString();
                            user.Password = dataReader["Password"].ToString();
                            user.Balance = Convert.ToDecimal(dataReader["Balance"]);
                            user.Role = dataReader["Role"].ToString();

                        }
                    }
                    catch (NullReferenceException e)
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
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }
                }
                connection.Close();
            }
            return user;
        }

        public int UpdateWithdrawBalance(Transection wtvm, int id)
        {
            string Query = $"UPDATE [User] SET Updated_at = GETDATE(),Updated_by= '{wtvm.Name}' , Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') - '{wtvm.Amount}') WHERE OId = '{id}'";
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

        public int UpdateDepositBalance(Transection wtvm, int id)
        {
            string Query = $"UPDATE [User] SET Updated_at = GETDATE(),Updated_by= '{wtvm.Name}' , Balance = ((SELECT Balance FROM[User] WHERE OId = '{id}') + '{wtvm.Amount}') WHERE OId = '{id}'";
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

    }
}
