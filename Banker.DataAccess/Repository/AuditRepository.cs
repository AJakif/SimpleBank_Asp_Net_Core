using Banker.Models.ViewModels;
using BankerLibrary.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository
{
    public class AuditRepository: IAuditRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuditRepository> _logger;

        public AuditRepository(IConfiguration config, ILogger<AuditRepository> logger)
        {
            _config = config;
            _logger = logger;
        }

        public int InsertEditAudit(CollectData collect,int id)
        {
            string Query = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                    $",[Created_at],[Created_by]) VALUES ('{id}','{collect.Transection.TransId}','{collect.Transection.Name}',GETDATE(),'{collect.Transection.Amount}','{collect.Transection.Source}','{collect.Transection.TransactionType}','{collect.Transection.Type}','{"Edited"}',GETDATE(),'{collect.Transection.Name}')";
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

        public int InsertDeleteAudit(Transection transection)
        {
            string Query = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                     $",[Created_at],[Created_by]) VALUES ('{transection.UserId}','{transection.TransId}','{transection.Name}',GETDATE(),'{transection.Amount}','{transection.Source}','{transection.TransactionType}','{transection.Type}','{"Deleted"}',GETDATE(),'{transection.Name}')";
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

        public int InsertAddAudit(Transection wtvm, int id, string transId)
        {
            string Query = $"INSERT INTO[dbo].[TansactionAudit] ([UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType]" +
                                             $",[Created_at],[Created_by]) VALUES ('{id}','{transId}','{wtvm.Name}',GETDATE(),'{wtvm.Amount}','{wtvm.Source}','{"Withdraw"}','{wtvm.Type}','{"Added"}',GETDATE(),'{wtvm.Name}')";
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

        public AuditViewModel GetAudit()
        {
            string query = $"Select * from [TansactionAudit] ORDER BY Date DESC";
            List<AuditViewModel> avml = new List<AuditViewModel>();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                                AuditViewModel avm = new AuditViewModel()
                                {
                                    OId = Convert.ToInt32(dataReader["OId"]),
                                    UserId = Convert.ToInt32(dataReader["UserId"]),
                                    TransId = dataReader["TransId"].ToString(),
                                    Name = dataReader["Name"].ToString(),
                                    Date = Convert.ToString(dataReader["Date"]),
                                    Amount = Convert.ToDecimal(dataReader["Amount"]),
                                    Source = dataReader["Source"].ToString(),
                                    TransactionType = dataReader["TransactionType"].ToString(),
                                    Type = dataReader["Type"].ToString(),
                                    LogType = dataReader["LogType"].ToString()
                                };

                                avml.Add(avm);
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

            AuditViewModel obj = new AuditViewModel
            {
                AuditList = avml
            };

            return obj;
        }

        public AuditViewModel GetAuditType(string type)
        {
            string query = $"SELECT[OId] ,[UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType] " +
           $"FROM[dbo].[TansactionAudit] WHERE[TransactionType] = '{type}'";
            List<AuditViewModel> avml = new List<AuditViewModel>();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                                AuditViewModel avm = new AuditViewModel()
                                {
                                    OId = Convert.ToInt32(dataReader["OId"]),
                                    UserId = Convert.ToInt32(dataReader["UserId"]),
                                    TransId = dataReader["TransId"].ToString(),
                                    Name = dataReader["Name"].ToString(),
                                    Date = Convert.ToString(dataReader["Date"]),
                                    Amount = Convert.ToDecimal(dataReader["Amount"]),
                                    Source = dataReader["Source"].ToString(),
                                    TransactionType = dataReader["TransactionType"].ToString(),
                                    Type = dataReader["Type"].ToString(),
                                    LogType = dataReader["LogType"].ToString()
                                };

                                avml.Add(avm);
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

            AuditViewModel obj = new AuditViewModel
            {
                AuditList = avml
            };

            return obj;
        }

        public AuditViewModel GetLogType(string type)
        {
            string query = $"SELECT[OId] ,[UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType] " +
            $"FROM[dbo].[TansactionAudit] WHERE[LogType] = '{type}'";
            List<AuditViewModel> avml = new List<AuditViewModel>();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                                AuditViewModel avm = new AuditViewModel()
                                {
                                    OId = Convert.ToInt32(dataReader["OId"]),
                                    UserId = Convert.ToInt32(dataReader["UserId"]),
                                    TransId = dataReader["TransId"].ToString(),
                                    Name = dataReader["Name"].ToString(),
                                    Date = Convert.ToString(dataReader["Date"]),
                                    Amount = Convert.ToDecimal(dataReader["Amount"]),
                                    Source = dataReader["Source"].ToString(),
                                    TransactionType = dataReader["TransactionType"].ToString(),
                                    Type = dataReader["Type"].ToString(),
                                    LogType = dataReader["LogType"].ToString()
                                };

                                avml.Add(avm);
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

            AuditViewModel obj = new AuditViewModel
            {
                AuditList = avml
            };

            return obj;
        }

        public AuditViewModel GetDate(string date)
        {
            string query = $"SELECT[OId] ,[UserId],[TransId],[Name],[Date],[Amount],[Source],[TransactionType],[Type],[LogType] " +
            $"FROM[dbo].[TansactionAudit] WHERE CONVERT(VARCHAR(10), [Date], 23) = '{date}'";
            List<AuditViewModel> avml = new List<AuditViewModel>();
            string connectionString = _config["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                                AuditViewModel avm = new AuditViewModel()
                                {
                                    OId = Convert.ToInt32(dataReader["OId"]),
                                    UserId = Convert.ToInt32(dataReader["UserId"]),
                                    TransId = dataReader["TransId"].ToString(),
                                    Name = dataReader["Name"].ToString(),
                                    Date = Convert.ToString(dataReader["Date"]),
                                    Amount = Convert.ToDecimal(dataReader["Amount"]),
                                    Source = dataReader["Source"].ToString(),
                                    TransactionType = dataReader["TransactionType"].ToString(),
                                    Type = dataReader["Type"].ToString(),
                                    LogType = dataReader["LogType"].ToString()
                                };

                                avml.Add(avm);
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

            AuditViewModel obj = new AuditViewModel
            {
                AuditList = avml
            };

            return obj;
        }
    }
}
