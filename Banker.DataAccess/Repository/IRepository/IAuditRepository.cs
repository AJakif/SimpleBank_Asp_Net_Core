using Banker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface IAuditRepository
    {
        AuditModel GetAudit();

        AuditModel GetAuditType(string type);

        AuditModel GetLogType(string type);

        AuditModel GetDate(string date);
        int InsertEditAudit(CollectDataModel collect, int id);

        int InsertDeleteAudit(TransactionModel transection);

        int InsertAddAudit(TransactionModel wtvm, int id, string transId);
    }
}
