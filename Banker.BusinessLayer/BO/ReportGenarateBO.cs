using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.BusinessLayer.BO
{
    public class ReportGenarateBO
    {
        public List<MonthlyDepositReportBO> DepositList { get; set; }
        public List<MonthlyWithdrawReportBO> WithdrawList { get; set; }
    }
}
