using Banker.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankerLibrary.Repository.IRepository
{
    public interface IReportRepository
    {
        List<CurrentMonthDepositViewModel> CurrentDeposit(int month, int id);

        List<CurrentMonthWithdrawViewModel> CurrentWithdraw(int month, int id);

        List<Years> YearlyWithdraw(int id);

        List<DYears> YearlyDeposit(int id);

        List<DRemarks> YearlyCatDeposit(int id);

        List<Remarks> YearlyCatWithdraw(int id);

        List<MYears> MyMDeposit(int id);

        List<YearlyMonthlyDepositViewModel> MyYDeposit(int id);

        List<WYears> MyMWithdraw(int id);

        List<YearlyMonthlyWithdrawViewModel> MyYWithdraw(int id);
    }
}
