using Banker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface ILoginHistoryRepository
    {
        int LoginHistory(UserModel userDetails);

        HistoryModel GetHistory(int id);
    }
}
