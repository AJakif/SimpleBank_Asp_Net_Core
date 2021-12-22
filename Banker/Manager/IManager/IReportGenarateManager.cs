using Banker.BusinessLayer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banker.Manager.IManager
{
    public interface IReportGenarateManager
    {
        ReportDataBO ReportGet(int id);
    }
}
