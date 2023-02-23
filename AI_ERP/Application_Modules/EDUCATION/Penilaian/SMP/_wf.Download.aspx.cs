using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class _wf_Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //reporting
            if (Downloads.GetQS_JenisDownload() ==
                Downloads.JenisDownload.RAPOR_DAPODIK_SMP)
            {
                string tahun_ajaran = Libs.GetQueryString("t");
                string semester = Libs.GetQueryString("s");
                string rel_kelasdet = Libs.GetQueryString("kd");
                string rel_mapel = Libs.GetQueryString("m");

                ltrDownload.Text += Reports_SMP.GetHTMLRaporDapodikPerMapel(
                        this.Response,
                        tahun_ajaran,
                        semester,
                        rel_kelasdet,
                        rel_mapel
                    );

                Response.ContentType = "application/x-msexcel";
                Response.AddHeader("Content-Disposition", "attachment; filename = Format Nilai Rapor " + DAO_Mapel.GetByID_Entity(rel_mapel).Nama + " " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentEncoding = Encoding.UTF8;
                StringWriter tw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(tw);
                ltrDownload.RenderControl(hw);
                Response.Write(tw.ToString());
                Response.End();

                return;
            }
        }
    }
}