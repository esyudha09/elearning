using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using System.Windows.Input;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_Attempt : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPEL";

        public static List<CBT_DesignSoal> lstDesignSoalJwb = null;
        public static List<CBT_Jwb> lstJwb = null;

        public static int lstIdx = 0;
        public static int mxIdx = 0;

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoChangePage,
            DoShowConfirmHapus,
            Clear
        }

        private static class QS
        {
            public static string GetUnit()
            {
                if (Libs.GetQueryString("u").Trim() != "")
                {
                    return Libs.GetQueryString("u");
                }
                else
                {
                    return Libs.GetQueryString("unit");
                }
            }

            public static string GetToken()
            {
                return Libs.GetQueryString("token");
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("k");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (Libs.GetQueryString("m").Trim() != "" ? "m=" + Libs.GetQueryString("m").Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();

                return (
                            Libs.GetQueryString("m").Trim() != "" || QS.GetToken().Trim() != ""
                        ? "?" : "") +
                        s_url_var;
            }
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            //{
            //    Libs.RedirectToLogin(this.Page);
            //}
            //if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            //{
            //    Libs.RedirectToBeranda(this.Page);
            //}
            if (!IsPostBack)
            {
                btnPrev.Enabled = false;

                lstIdx = 0;
                lstDesignSoalJwb = null;
                getListDs();
                getListJwb();
                getListLInk();
                getData(lstDesignSoalJwb[lstIdx].Rel_BankSoal.ToString());

            }

            var IdSoal = Libs.GetQueryString("id");
            if (!string.IsNullOrEmpty(IdSoal))
            {
                getData(IdSoal);
            }


        }

        protected void getListDs()
        {


            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;
            conn.Open();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = DAO_CBT_DesignSoal.SP_SELECT_BY_RS;
            comm.Parameters.Clear();
            //comm.Parameters.AddWithValue("@Rel_Siswa", Libs.LOGGED_USER_M.NoInduk);
            comm.Parameters.AddWithValue("@Rel_RumahSoal", Libs.GetQueryString("rs"));
            DataTable dtResult2 = new DataTable();
            sqlDA = new SqlDataAdapter(comm);
            sqlDA.Fill(dtResult2);

            lstDesignSoalJwb = new List<CBT_DesignSoal>();

            foreach (DataRow row in dtResult2.Rows)
            {
                CBT_DesignSoal j = new CBT_DesignSoal();
                j = DAO_CBT_DesignSoal.GetEntityFromDataRow(row);
                lstDesignSoalJwb.Add(j);
            }
            mxIdx = lstDesignSoalJwb.Count;
        }

        protected void getListJwb()
        {


            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;
            conn.Open();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = DAO_CBT_DesignSoal.SP_SELECT_JWB_BY_RS_SISWA;
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue("@Rel_Siswa", Libs.LOGGED_USER_M.NoInduk);
            comm.Parameters.AddWithValue("@Rel_RumahSoal", Libs.GetQueryString("rs"));
            DataTable dtResult2 = new DataTable();
            sqlDA = new SqlDataAdapter(comm);
            sqlDA.Fill(dtResult2);
            lstJwb = new List<CBT_Jwb>();

            foreach (DataRow row in dtResult2.Rows)
            {
                CBT_Jwb j = new CBT_Jwb();
                j = DAO_CBT_DesignSoal.GetEntityJwbFromDataRow(row);
                lstJwb.Add(j);
            }
        }

        protected void checkButton()
        {
            if (lstIdx > 0)
            {
                btnPrev.Enabled = true;
            }
            else
            {
                btnPrev.Enabled = false;
            }

            if (lstIdx < mxIdx - 1)
            {
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            lstIdx = lstIdx + 1;

            getData(lstDesignSoalJwb[lstIdx].Rel_BankSoal.ToString());

        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            lstIdx = lstIdx - 1;

            getData(lstDesignSoalJwb[lstIdx].Rel_BankSoal.ToString());

        }

        protected void btnLink_Click(object sender, EventArgs e)
        {
            lstIdx = Convert.ToInt32(hdIdx.Value) - 1;


            getData(lstDesignSoalJwb[lstIdx].Rel_BankSoal.ToString());

        }

        protected void getData(string idsoal)
        {

            CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(idsoal.Trim());

            if (m != null)
            {
                if (m.Soal != null)
                {

                    txtSoal.Text = m.Soal.ToString();
                    //txtJwbEssay.Text = m.JwbEssay.ToString();



                    if (m.Jenis == "essay")
                    {
                        EssayDiv.Attributes.Add("style", "display:block");
                        GandaDiv.Attributes.Add("style", "display:none");
                    }
                    else if (m.Jenis == "ganda")
                    {
                        EssayDiv.Attributes.Add("style", "display:none");
                        GandaDiv.Attributes.Add("style", "display:block");
                    }



                    txtJwbGanda1.Text = m.ListJwbGanda[0].Jawaban.ToString();
                    txtJwbGanda2.Text = m.ListJwbGanda[1].Jawaban.ToString();
                    txtJwbGanda3.Text = m.ListJwbGanda[2].Jawaban.ToString();
                    txtJwbGanda4.Text = m.ListJwbGanda[3].Jawaban.ToString();
                    txtJwbGanda5.Text = m.ListJwbGanda[4].Jawaban.ToString();

                }

            }
            checkButton();
            getListLInk();
            if (m.Jenis == "essay")
            {
                txtKeyAction.Value = JenisAction.DoShowData.ToString();
            }
            else
            {
                txtKeyAction.Value = JenisAction.Clear.ToString();
            }
        }

        protected void getListLInk()
        {
            string link = "";
            int i = 1;
            bool exist = false;
            foreach (CBT_DesignSoal row in lstDesignSoalJwb)
            {

                if (lstJwb != null)
                    exist = lstJwb.Any(m=> m.Rel_DesignSoal == row.Kode.ToString().ToUpper());
                if (exist)
                {
                    link += "<div class=\"col-md-2\">    <button class=\"btn btn-green\"  onclick=\"ContentPlaceHolder1_hdIdx.value=\' " + i + "  \';ContentPlaceHolder1_btnLinkClick.click();\" style=\"padding:15px\"> " + i + " </button> </div>";

                }
                else
                {
                    link += "<div class=\"col-md-2\">    <button class=\"btn btn-orange\"  onclick=\"ContentPlaceHolder1_hdIdx.value=\' " + i + "  \';ContentPlaceHolder1_btnLinkClick.click();\" style=\"padding:15px\"> " + i + " </button> </div>";

                }
                i++;
            }

            txtLink.Text = link;
        }


        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE +
                            QS.GetURLVariable()
                        )
                );
        }
    }
}