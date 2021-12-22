using Banker.BusinessLayer.BO;
using Banker.DataAccess.Repository.IRepository;
using Banker.Manager.IManager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Manager
{
    public class ReportGenarateManager : IReportGenarateManager
    {
        private readonly IReportGenarateRepository _reportGenarate;

        public ReportGenarateManager(IReportGenarateRepository reportGenarate)
        {
            _reportGenarate = reportGenarate;
        }

        public ReportDataBO ReportGet(int id)
        {
            return  _reportGenarate.GetReport(id);
        }
    }
}
