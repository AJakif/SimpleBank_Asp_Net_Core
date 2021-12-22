using Banker.BusinessLayer.BO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banker.DataAccess.Repository.IRepository
{
    public interface IReportGenarateRepository
    {
        ReportDataBO GetReport(int id);
    }
}
