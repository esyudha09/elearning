using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_FormasiGuruKelas : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAFORMASIGURUKELAS";

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
            DoShowConfirmHapus,
            DoChangePage
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
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            {
                Libs.RedirectToBeranda(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/boss-1.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Formasi Guru Kelas/Wali Kelas";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();
                ListDropdown();
                InitKelasUnit();
                txtGuruKelas.KodeUnit = QS.GetUnit();
                txtGuruPendamping.KodeUnit = QS.GetUnit();
            }

            if (!(IsPostBack || this.Session[SessionViewDataName] == null))
            {
                dpData.SetPageProperties((int)this.Session[SessionViewDataName], dpData.MaximumRows, true);
            }

            BindListView(!IsPostBack, Libs.GetQ());
        }

        protected void InitKelasUnit()
        {
            txtParseKelasUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                txtParseKelasUnit.Value += "|;";
                List<KelasDet> lst_kelas = DAO_KelasDet.GetBySekolah_Entity(m_sekolah.Kode.ToString());
                foreach (KelasDet m in lst_kelas.FindAll(m0 => m0.IsKelasJurusan == false && m0.IsKelasSosialisasi == false).ToList())
                {
                    if (m.IsAktif)
                    {
                        string jenis_kelas = "Kelas Perwalian";
                        if (m.IsKelasJurusan)
                        {
                            jenis_kelas = "Kelas Jurusan";
                        }
                        else if (m.IsKelasSosialisasi)
                        {
                            jenis_kelas = "Kelas Sosialisasi";
                        }
                        txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                        txtParseKelasUnit.Value += m.Kode.ToString() +
                                                   "|" +
                                                   jenis_kelas +
                                                   HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                                   m.Nama +
                                                   ";";
                    }
                }
            }
        }

        protected void ListDropdown()
        {
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            cboUnitSekolah.Items.Clear();
            cboUnitSekolah.Items.Add("");
            //div_input_filter_unit.Visible = true;
            if (IsByAdminUnit())
            {
                cboUnitSekolah.Items.Clear();
                lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
                //div_input_filter_unit.Visible = false;
            }            
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnitSekolah.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboUnitSekolah.ClientID + "').focus(); return false; }");
            cboUnitSekolah.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtGuruKelas.NamaClientID + "').focus(); return false; }");
            txtGuruKelas.NamaControl.Attributes.Add("onkeydown", sKeyEnter + "return false; }");
            txtGuruPendamping.NamaControl.Attributes.Add("onkeydown", sKeyEnter + "return false; }");

            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
            cboKelas.Attributes.Add("onchange", txtKelasUnit.ClientID + ".value = this.value; return false;"); 
            cboUnitSekolah.Attributes["onchange"] = "ShowKelasByUnit(this.value);";
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (IsByAdminUnit())
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_FormasiGuruKelas.SP_SELECT_ALL_BY_UNIT_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Sekolah", QS.GetUnit());
                    sql_ds.SelectCommand = DAO_FormasiGuruKelas.SP_SELECT_ALL_BY_UNIT;
                }
            }
            else
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectCommand = DAO_FormasiGuruKelas.SP_SELECT_ALL_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectCommand = DAO_FormasiGuruKelas.SP_SELECT_ALL;
                }
            }

            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_sekolah = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_sekolah");
            System.Web.UI.WebControls.Literal imgh_kelasdet = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelasdet");
            System.Web.UI.WebControls.Literal imgh_gurukelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_gurukelas");
            System.Web.UI.WebControls.Literal imgh_gurupendamping = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_gurupendamping");

            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_tahunajaran.Text = html_image;
            imgh_sekolah.Text = html_image;
            imgh_kelasdet.Text = html_image;
            imgh_gurukelas.Text = html_image;
            imgh_gurupendamping.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_sekolah.Visible = false;
            imgh_kelasdet.Visible = false;
            imgh_gurukelas.Visible = false;
            imgh_gurupendamping.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Sekolah":
                    imgh_sekolah.Visible = true;
                    break;
                case "KelasDet":
                    imgh_kelasdet.Visible = true;
                    break;
                case "GuruKelas":
                    imgh_gurukelas.Visible = true;
                    break;
                case "GuruPendamping":
                    imgh_gurupendamping.Visible = true;
                    break;
            }

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 20)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtGuruKelas.Value = "";
            txtGuruPendamping.Value = "";
            txtKelasUnit.Value = "";
            if (cboUnitSekolah.Items.Count > 0) cboUnitSekolah.SelectedIndex = 0;
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_FormasiGuruKelas.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                FormasiGuruKelas m = new FormasiGuruKelas();
                m.Rel_Sekolah = new Guid(cboUnitSekolah.SelectedValue);
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_GuruKelas = txtGuruKelas.Value;
                m.Rel_GuruPendamping = (txtGuruPendamping.Value.Trim() == "" ? txtGuruKelas.Value : txtGuruPendamping.Value);
                m.Rel_KelasDet = txtKelasUnit.Value;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_FormasiGuruKelas.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_FormasiGuruKelas.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiGuruKelas m = DAO_FormasiGuruKelas.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        cboUnitSekolah.SelectedValue = m.Rel_Sekolah.ToString();
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;                        
                        txtGuruKelas.Value = m.Rel_GuruKelas;
                        txtGuruPendamping.Value = (m.Rel_GuruPendamping == m.Rel_GuruKelas ? "" : m.Rel_GuruPendamping);
                        txtKelasUnit.Value = m.Rel_KelasDet;
                        cboKelas.SelectedValue = m.Rel_KelasDet;
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiGuruKelas m = DAO_FormasiGuruKelas.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                            " Semester " +
                                                            m.Semester +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }
    }
}