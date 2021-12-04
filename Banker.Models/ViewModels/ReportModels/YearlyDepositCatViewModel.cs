using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ViewModels.ReportModels
{
    public class YearlyDepositCatViewModel
    {
        public decimal Total { get; set; }
        public string Year { get; set; }
        public string Remark { get; set; }
    }
    public class DRemarks
    {
        public string Remark { get; set; }
        public List<YearlyDepositCatViewModel> List { get; set; }

    }


}
