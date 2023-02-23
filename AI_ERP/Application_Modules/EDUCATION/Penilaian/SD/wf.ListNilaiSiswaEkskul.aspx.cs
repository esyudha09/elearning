using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_ListNilaiSiswaEkskul : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATALISTNILAIEKSKUL_SD";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            DoChangePage,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            DoShowPengaturan
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/text-lines.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Nilai Ekstrakurikuler";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                cboPeriode.Items.Clear();
                foreach (var item in DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity())
                {
                    cboPeriode.Items.Add(new ListItem
                    {
                        Value = item.TahunAjaran + item.Semester,
                        Text = item.TahunAjaran + " semester " + item.Semester
                    });
                }
            }
            BindListView(!IsPostBack, Libs.GetQ());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboPeriode.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        protected string GetTahunAjaran()
        {
            string tahun_ajaran = "";

            if (cboPeriode.Items.Count > 0)
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriode.SelectedValue.Substring(0, 9);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    tahun_ajaran = periode.Substring(0, 9);
                }
            }

            return tahun_ajaran;
        }

        protected string GetSemester()
        {
            string semester = "";

            if (cboPeriode.Items.Count > 0)
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    semester = cboPeriode.SelectedValue.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    semester = periode.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            return semester;
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = GetTahunAjaran();
            string semester = GetSemester();

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    tahun_ajaran = periode.Substring(0, 9);
                    semester = periode.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            txtTahunAjaran.Value = tahun_ajaran;
            txtSemester.Value = semester;
            ltrPeriode.Text = tahun_ajaran + "&nbsp;<sup style=\"color: yellow;\">" + semester + "</sup>";

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds.SelectParameters.Clear();
            if (keyword.Trim() != "")
            {
                sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_NILAI_EKSKUL_FOR_SEARCH;
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("nama", keyword);
            }
            else
            {
                sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_NILAI_EKSKUL;
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
            }                
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        protected void DoRefresh(bool refresh_all = false)
        {
            if (refresh_all)
            {
                Response.Redirect(Libs.FILE_PAGE_URL);
            }
            else
            {
                string periode = GetPeriode();
                periode = periode.Replace("/", "-");

                Response.Redirect(Libs.FILE_PAGE_URL + (periode != "" ? "?p=" + periode : ""));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DoRefresh();
        }

        protected void InitFields()
        {
            txtID.Value = "";
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Rapor_AspekPenilaian.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected string GetPeriode()
        {
            string periode = "";
            if (cboPeriode.Items.Count > 0)
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    periode = cboPeriode.SelectedValue;
                }
            }
            return periode;
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            if (Libs.GetQueryString("p").Trim() != "")
            {
                string periode = GetPeriode();
                periode = periode.Replace("/", "-");

                Response.Redirect(
                        Libs.FILE_PAGE_URL +
                        (Libs.GetQueryString("q").Trim() != "" ? "?q=" + Libs.GetQueryString("q") : "") +
                        (periode != "" && (Libs.GetQueryString("q").Trim() != "" ? "?q=" + Libs.GetQueryString("q") : "") != "" ? "&p=" + periode : "") +
                        (periode != "" && (Libs.GetQueryString("q").Trim() != "" ? "?q=" + Libs.GetQueryString("q") : "") == "" ? "?p=" + periode : "")
                    );
            }
            else
            {
                BindListView(!IsPostBack, Libs.GetQ());
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            string periode = GetPeriode();
            periode = periode.Replace("/", "-");
            this.Session[SessionViewDataName] = 0;

            Response.Redirect(
                    Libs.FILE_PAGE_URL +
                    (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : "") +
                    (periode != "" ? "&p=" + periode : "")
                );
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_AspekPenilaian m = DAO_Rapor_AspekPenilaian.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.Nama) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnDoPengaturan_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowPengaturan.ToString();
        }
    }
}