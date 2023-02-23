using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules
{
    public partial class wf_LinkOpener : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            DoOpenURL();
        }

        protected void DoOpenURL()
        {
            string jenis = Libs.GetQueryString("j");
            string id = Libs.GetQueryString("id");

            if (jenis.Trim().ToUpper() == Routing.URL.APPLIACTION_MODULES.LINK_OPENER.JENIS_LO_LINK_PEMBELAJARAN_EKSTERNAL.ToUpper().Trim())
            {
                LinkPembelajaranEksternal m = DAO_LinkPembelajaranEksternal.GetByID_Entity(id);
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        LinkPembelajaranEksternalBuka m0 = new LinkPembelajaranEksternalBuka();
                        m0.Kode = Guid.NewGuid();
                        m0.Rel_LinkPembelajaranEksternal = id;
                        m0.Tanggal = DateTime.Now;
                        m0.Rel_Pegawai = Libs.LOGGED_USER_M.NoInduk;
                        m0.Link = m.Link;
                        m0.Nama = m.Nama;
                        m0.Kategori = m.Kategori;
                        m0.RTG_UNIT = "";
                        m0.RTG_LEVEL = "";
                        m0.RTG_SEMESTER = "";
                        m0.RTG_KELAS = "";
                        m0.RTG_SUBKELAS = "";
                        DAO_LinkPembelajaranEksternalBuka.Insert(m0);

                        Response.Redirect(m.Link);
                    }
                }
            }
        }
    }
}