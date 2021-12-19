using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.Models.ReportModels
{
    public class YearlyWithdrawViewModel
    {
        public decimal Total { get; set; }
        public string Year { get; set; }
        public string Remark { get; set; }
    }
    public class Years
    {
        public string Year { get; set; }
        public List<YearlyWithdrawViewModel> List { get; set; }

    }


}
