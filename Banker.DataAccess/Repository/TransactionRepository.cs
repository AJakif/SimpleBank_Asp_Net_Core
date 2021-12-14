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
    public class TransactionRepository: ITransactionRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(IConfiguration config, ILogger<TransactionRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public List<Transection> GetTransactionList(int id)
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
                                TransId = dataReader["TransId"].ToString(),
                                Name = dataReader["Name"].ToString(),
                                Date = Convert.ToDateTime(dataReader["Date"]),
                                Amount = Convert.ToDecimal(dataReader["Amount"]),
                                Source = dataReader["Source"].ToString(),
                                TransactionType = dataReader["TransactionType"].ToString(),
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

        public Transection GetTransaction(int id)
        {
            CollectData collect = new CollectData();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT [OId] ,[UserId] ,[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type] " +
                $"FROM [Transaction] WHERE [OId] = '{id}'";
                string sql = query;
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    try
                    {
                        while (dataReader.Read()) //make it single user
                        {
                            Transection t = new Transection
                            {
                                OId = Convert.ToInt32(dataReader["OId"]),
                                UserId = Convert.ToInt32(dataReader["UserId"]),
                                TransId = dataReader["TransId"].ToString(),
                                Name = dataReader["Name"].ToString(),
                                Date = Convert.ToDateTime(dataReader["Date"]),
                                Amount = Convert.ToDecimal(dataReader["Amount"]),
                                Source = dataReader["Source"].ToString(),
                                TransactionType = dataReader["TransactionType"].ToString(),
                                Type = dataReader["Type"].ToString()
                            };

                            collect.Transection = t;
                        }

                    }
                    catch (NullReferenceException e)
                    {
                        _logger.LogWarning($"'{e}' Exception");
                    }

                }
                connection.Close();
            }
            return (collect.Transection);
        }

        public int Transaction(CollectData collect)
        {
            string Query = $"UPDATE[dbo].[Transaction] SET [Source] = '{collect.Transection.Source}' ,[Type] = '{collect.Transection.Type}' ,[Updated_at] = GETDATE() ,[Updated_by] = '{collect.Transection.Name}' " +
            $"WHERE OId = '{collect.Transection.OId}'";
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

        public int DeleteTransaction(Transection transection)
        {
            string Query = $"DELETE FROM [dbo].[Transaction]  WHERE OId = '{transection.OId}' ";
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

        public int Withdraw(Transection wtvm, int id, string transId)
        {
            string Query ="Insert into [Transaction] (UserId,TransId,Name,Date,Amount,Source,TransactionType,Type,Created_at,Created_by)" +
                        $"values ('{id}','{transId}','{wtvm.Name}',GETDATE(),'{wtvm.Amount}','{wtvm.Source}','{"Withdraw"}','{wtvm.Type}',GETDATE(),'{wtvm.Name}')";
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

        public int Deposit(Transection dtvm, int id, string transId)
        {
            string Query = "Insert into [Transaction] (UserId,TransId,Name,Date,Amount,Source,TransactionType,Type,Created_at,Created_by)" +
                    $"values ('{id}','{transId}','{dtvm.Name}',GETDATE(),'{dtvm.Amount}','{dtvm.Source}','{"Deposit"}','{dtvm.Type}',GETDATE(),'{dtvm.Name}')";
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
