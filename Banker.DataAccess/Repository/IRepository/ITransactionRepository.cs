using Banker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface ITransactionRepository
    {
        List<Transection> GetTransactionList(int id);

        Transection GetTransaction(int id);

        int Transaction(CollectData collect);

        int DeleteTransaction(Transection transection);

        int Withdraw(Transection wtvm, int id, string transId);

        int Deposit(Transection dtvm, int id, string transId);
    }
}
