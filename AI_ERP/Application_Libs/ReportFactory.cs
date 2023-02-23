using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace AI_ERP.Application_Libs
{
    public class ReportFactory
    {

        protected static Queue reportQueue = new Queue();
        protected static int iMaxCount = 75;

        protected static ReportDocument CreateReport(Type reportClass)
        {
            object report = Activator.CreateInstance(reportClass);
            reportQueue.Enqueue(report);
            return (ReportDocument)report;
        }

        public static ReportDocument GetReport(Type reportClass)
        {
            if (reportQueue.Count > iMaxCount)
            {
                ((ReportDocument)reportQueue.Dequeue()).Close();
                ((ReportDocument)reportQueue.Dequeue()).Dispose();
                GC.Collect();
            }
            return CreateReport(reportClass);

        }
    }
}