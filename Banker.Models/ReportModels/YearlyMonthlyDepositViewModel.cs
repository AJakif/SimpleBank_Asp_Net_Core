using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ReportModels
{
    public class YearlyMonthlyDepositViewModel
    {
       
        public decimal YTotal { get; set; }
        public string Year { get; set; }
    }
    public class Monthly
    {
        public decimal MTotal { get; set; }
        public string Month { get; set; }

        public string Year { get; set; }
    }
    public class MYears
    {
        public string Year { get; set; }
        public List<Monthly> List { get; set; }

    }
}
