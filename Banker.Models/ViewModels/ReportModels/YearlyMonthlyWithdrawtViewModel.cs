using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ViewModels.ReportModels
{
    public class YearlyMonthlyWithdrawViewModel
    {
       
        public decimal YTotal { get; set; }
        public string Year { get; set; }
    }
    public class WMonthly
    {
        public decimal MTotal { get; set; }
        public string Month { get; set; }

        public string Year { get; set; }
    }
    public class WYears
    {
        public string Year { get; set; }
        public List<WMonthly> List { get; set; }

    }
}
