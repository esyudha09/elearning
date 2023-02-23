using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Ortu_KalenderAkademik : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-calendar\"></i>&nbsp;&nbsp;" +
                                       "Kalender Akademik";
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            InitKalender();
        }

        protected void InitKalender()
        {
            ltrKalender.Text = Kalender.GetHTMLKalender(DateTime.Now.Month, DateTime.Now.Year);
        }
    }
}