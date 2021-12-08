using Banker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Helpers
{
    public interface ICommonHelper
    {
        UserViewModel GetUserByEmail(string query);

        UserViewModel GetUserById(int id);
        
        int DMLTransaction(string Query);
        
        bool UserAlreadyExists(string query);
        
        List<Transection> GetTransactionList(int id);

        Transection GetTransaction(int id);

        AuditViewModel GetAudit( string query);


        HistoryViewModel GetHistory(int id);
    }
}
