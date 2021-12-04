using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ViewModels.ReportModels
{
    public class YearlyWithdrawCatViewModel
    {
        public decimal Total { get; set; }
        public string Year { get; set; }
        public string Remark { get; set; }
    }
    public class Remarks
    {
        public string Remark { get; set; }
        public List<YearlyWithdrawCatViewModel> List { get; set; }

    }


}
