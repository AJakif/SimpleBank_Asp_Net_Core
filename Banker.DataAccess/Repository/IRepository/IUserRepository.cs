using Banker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface IUserRepository
    {
        bool UserAlreadyExists(RegisterViewModel rvm);

        int Register(RegisterViewModel rvm);

        UserViewModel GetUserByEmail(LoginViewModel lvm);

        UserViewModel GetUserById(int id);

        int UpdateWithdrawBalance(Transection wtvm, int id);

        int UpdateDepositBalance(Transection wtvm, int id);
    }
}
