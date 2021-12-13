using Banker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface IAuditRepository
    {
        AuditViewModel GetAudit();

        AuditViewModel GetAuditType(string type);

        AuditViewModel GetLogType(string type);

        AuditViewModel GetDate(string date);
        int InsertEditAudit(CollectData collect, int id);

        int InsertDeleteAudit(Transection transection);

        int InsertAddAudit(Transection wtvm, int id, string transId);
    }
}
