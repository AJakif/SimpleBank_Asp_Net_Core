using Banker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface ITransactionRepository
    {
        List<TransactionModel> GetTransactionList(int id);

        TransactionModel GetTransaction(int id);

        int UpdateTransaction(CollectDataModel collect);

        int DeleteTransaction(TransactionModel transection);

        int Withdraw(TransactionModel wtvm, int id, string transId);

        int Deposit(TransactionModel dtvm, int id, string transId);
    }
}
