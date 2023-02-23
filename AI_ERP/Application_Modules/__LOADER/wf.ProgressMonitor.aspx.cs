using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Threading;
using System.ComponentModel;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.__LOADER
{
    public partial class wf_ProgressMonitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            _bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _bw.DoWork += bw_DoWork;
            _bw.ProgressChanged += bw_ProgressChanged;
            _bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            _bw.RunWorkerAsync("");
        }

        static BackgroundWorker _bw;

        public static int Percent { get; set; }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Application_Libs.Libs.LOGGED_USER_M == null)
                {
                    _bw.ReportProgress(0);
                }
                else
                {
                    _bw.ReportProgress(Application_Libs.Libs.LOGGED_USER_M.ProgressPercent);
                }
            }
            catch (Exception)
            {
                _bw.ReportProgress(0);
            }            
        }

        void bw_RunWorkerCompleted(object sender,
                                   RunWorkerCompletedEventArgs e)
        {
            Percent = 100;
        }

        void bw_ProgressChanged(object sender,
                                ProgressChangedEventArgs e)
        {
            Percent = e.ProgressPercentage;
        }

        [System.Web.Services.WebMethod]
        public static int GetProgress()
        {
            return Application_Libs.Libs.LOGGED_USER_M.ProgressPercent;
        }
    }
}