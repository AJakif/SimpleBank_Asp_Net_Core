using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ViewModels.ReportModels
{
    public class YearlyDepositViewModel
    {
        public decimal Total { get; set; }
        public string Year { get; set; }
        public string Remark { get; set; }
    }
    public class DYears
    {
        public string Year { get; set; }
        public List<YearlyDepositViewModel> List { get; set; }

    }


}
