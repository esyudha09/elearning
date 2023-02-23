using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_LTS_Proses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProsesLTS();
            }
        }

        protected void ProsesLTS()
        {
            string tahun_ajaran = Libs.GetQueryString("t");
            string semester = Libs.GetQueryString("s");
            string kelas_det = Libs.GetQueryString("kd");

            if (kelas_det.Trim() != "")
            {

            }
            else
            {

            }
        }
    }
}