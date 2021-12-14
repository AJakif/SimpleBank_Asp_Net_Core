using Banker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface ILoginHistoryRepository
    {
        int LoginHistory(UserViewModel userDetails);

        HistoryViewModel GetHistory(int id);
    }
}
