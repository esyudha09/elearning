using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using System.Web.SessionState;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_AttemptStart : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPEL";

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
            DoShowConfirmHapus
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
                s_url_var += (QS.GetUnit().Trim() != "" ? "unit=" + QS.GetUnit().Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();

                return (
                            QS.GetUnit().Trim() != "" || QS.GetToken().Trim() != ""
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
            

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/document.svg") + "\">" +
                                   "&nbsp;&nbsp;" +
                                   "";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {


            }
            GetData();
        }



        protected void GetData()
        {

            CBT_RumahSoal m = DAO_CBT_RumahSoal_Input.GetByID_Entity(Libs.GetQueryString("rs"));

            if (m != null)
            {

                txtHeader.Text = "<img src=\"../../Application_Templates/sd header.png\" width=\"100%\">";


                txtNamaKP.Text = m.Nama.ToString();

                txtDeskripsi.Text = m.Deskripsi.ToString();
                txtKelas.Text = m.NamaKelas.ToString();
                txtMapel.Text = m.NamaMapel.ToString();
                txtTahunAjaran.Text = m.TahunAjaran.ToString();
                txtSemester.Text = m.Semester.ToString();
                txtStart.Text = Libs.GetTanggalIndonesiaFromDate(m.StartDatetime, true);
                txtEnd.Text = Libs.GetTanggalIndonesiaFromDate(m.EndDatetime, true);
                txtLimit.Text = m.LimitTime.ToString() + " " + m.LimitSatuan.ToString();

                if (m.LimitTime != 0)
                {
                    divStart.Attributes.Add("style", "display:none");
                    divEnd.Attributes.Add("style", "display:none");
                    divLimit.Attributes.Add("style", "display:block");
                }
                else
                {
                    divStart.Attributes.Add("style", "display:block");
                    divEnd.Attributes.Add("style", "display:block");
                    divLimit.Attributes.Add("style", "display:none");
                }

                if (m.LimitTime == 0 && m.EndDatetime < DateTime.Now)
                {
                    btnStart.Visible = false;
                }

            }


        }

        protected void btnAttempt_Click(object sender, EventArgs e)
        {
            StatusSiswaInsert();

            var rs = Libs.GetQueryString("rs");

            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.ATTEMPT.ROUTE + "?rs=" + rs
                        )
                );
        }

        protected void StatusSiswaInsert()
        {
            try
            {
                CBT_StatusSiswa m = new CBT_StatusSiswa();
                m.Rel_RumahSoal = Libs.GetQueryString("rs");
                m.Rel_Siswa = Libs.LOGGED_USER_M.NoInduk;


                DAO_CBT_RumahSoal_Input.InsertStatus(m);


            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

    }
}