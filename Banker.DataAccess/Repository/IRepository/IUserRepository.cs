using Banker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface IUserRepository
    {
        bool UserAlreadyExists(RegisterModel rvm);

        int Register(RegisterModel rvm);

        UserModel GetUserByEmail(LoginModel lvm);

        UserModel GetUserById(int id);

        int UpdateWithdrawBalance(TransactionModel wtvm, int id);

        int UpdateDepositBalance(TransactionModel wtvm, int id);
    }
}
