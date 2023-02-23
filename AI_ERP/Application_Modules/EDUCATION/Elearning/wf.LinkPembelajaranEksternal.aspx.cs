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

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_LinkPembelajaranEksternal : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGELINKPEMBELAJARANEKSTERNAL";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoShowFilter,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoChangePage,
            DoShowConfirmHapus,
            DoShowDownloadHistLinkPembelajaran
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

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/science-book.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Link Pembelajaran Eksternal";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitInput();
                InitKeyEventClient();
                RenderLevelJenjang_Filter();

                txtTanggalMulai_HistLinkPembelajaran.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                txtTanggalAkhir_HistLinkPembelajaran.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            }
            BindListView(!IsPostBack, this.Master.txtCariData.Text);
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            cboKategori.Attributes.Add("onchange", "CekKategori()");
            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtLinkTautanCredit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtLinkTautan.ClientID + "').focus(); return false; }");
            txtLinkTautan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNamaTentang.ClientID + "').focus(); return false; }");
            txtNamaTentang.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKategori.ClientID + "').focus(); return false; }");
            cboKategori.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').focus(); return false; }");
            txtKategori.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void InitInput()
        {
            cboKategori.Items.Clear();
            cboKategori.Items.Add("");
            List<string> lst_kategori = DAO_LinkPembelajaranEksternal.GetDistinctKategori_Entity();
            foreach (string m in lst_kategori)
            {
                cboKategori.Items.Add(new ListItem
                {
                    Value = m,
                    Text = m
                });
            }
            cboKategori.Items.Add(new ListItem {
                Value = "Lainnya",
                Text = "Tambah/Masukan Kategori Lainnya"
            });

            cboKategori_Filter.Items.Clear();
            cboKategori_Filter.Items.Add("");
            foreach (string m in lst_kategori)
            {
                cboKategori_Filter.Items.Add(new ListItem
                {
                    Value = m,
                    Text = m
                });
            }
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (txtUsingFilter.Value == "1")
            {
                string s_unit_filter = txtUnit_Filter.Value;
                string s_sql = "SELECT " +
                                    "DISTINCT " +
                                        "a.Kode, a.Nama, a.Kategori, a.Unit, " +
                                        "d.Nama AS LinkOwner," +
                                        "COUNT(b.Kode) AS JumlahAksesSemua, " +
                                        "(" +
                                            "SELECT COUNT(x.Kode) " +
                                            "FROM LinkPembelajaranEksternalBuka x " +
                                            "WHERE CONVERT(varchar(50), x.Rel_LinkPembelajaranEksternal) = a.Kode AND " +
                                                  "x.Rel_Pegawai = '" + Libs.LOGGED_USER_M.NoInduk.Replace("'", "''") + "' " +
                                        ")  AS JumlahAksesGuruYbs " +
                                "FROM LinkPembelajaranEksternal a " +
                                "LEFT JOIN LinkPembelajaranEksternalBuka b ON " +
                                    "CONVERT(varchar(50), b.Rel_LinkPembelajaranEksternal) = CONVERT(varchar(50), a.Kode) " +
                                "LEFT JOIN Pegawai d ON d.Kode = a.Rel_Pegawai " +
                                    "WHERE " +
                                          (
                                                txtNamaTentang_Filter.Text.Trim() != ""
                                                ? " a.Nama LIKE '%" + txtNamaTentang_Filter.Text.Replace("'", "''") + "%' "
                                                : ""
                                          ) +

                                          (
                                                txtNamaTentang_Filter.Text.Trim() != "" &&
                                                txtLinkTautan_Filter.Text.Trim() != ""
                                                ? " AND "
                                                : ""
                                          ) +
                                          (
                                                txtLinkTautan_Filter.Text.Trim() != ""
                                                ? " a.Link LIKE '%" + txtLinkTautan_Filter.Text.Replace("'", "''") + "%' "
                                                : ""
                                          ) +

                                          (
                                                (
                                                    txtNamaTentang_Filter.Text.Trim() != "" ||
                                                    txtLinkTautan_Filter.Text.Trim() != ""
                                                ) &&
                                                cboKategori_Filter.SelectedValue.Trim() != ""
                                                ? " AND "
                                                : ""
                                          ) +
                                          (
                                                cboKategori_Filter.SelectedValue.Trim() != ""
                                                ? " a.Kategori LIKE '%" + cboKategori_Filter.SelectedValue.Trim().Replace("'", "''") + "%' "
                                                : ""
                                          ) +

                                          (
                                                (
                                                    txtNamaTentang_Filter.Text.Trim() != "" ||
                                                    txtLinkTautan_Filter.Text.Trim() != "" ||
                                                    cboKategori_Filter.SelectedValue.Trim() != ""
                                                ) &&
                                                txtLinkTautan_FilterCredit_Filter.Text.Trim() != ""
                                                ? " AND "
                                                : ""
                                          ) +
                                          (
                                                txtLinkTautan_FilterCredit_Filter.Text.Trim() != ""
                                                ? " d.Nama LIKE '%" + txtLinkTautan_FilterCredit_Filter.Text.Replace("'", "''") + "%' "
                                                : ""
                                          ) +

                                          (
                                                (
                                                    txtNamaTentang_Filter.Text.Trim() != "" ||
                                                    txtLinkTautan_Filter.Text.Trim() != "" ||
                                                    cboKategori_Filter.SelectedValue.Trim() != "" ||
                                                    txtLinkTautan_FilterCredit_Filter.Text.Trim() != ""
                                                ) &&
                                                s_unit_filter.Trim() != ""
                                                ? " AND "
                                                : ""
                                          ) + (s_unit_filter.Trim() != "" ? "(" : "") + s_unit_filter + (s_unit_filter.Trim() != "" ? ")" : "") +

                                "GROUP BY a.Kode, a.Nama, a.Kategori, a.Unit, d.Nama";

                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommandType = SqlDataSourceCommandType.Text;
                sql_ds.SelectCommand = s_sql;
            }
            else
            {
                if (keyword.Trim() != "")
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("nama", keyword);
                    sql_ds.SelectParameters.Add("Rel_Pegawai", Libs.LOGGED_USER_M.NoInduk);
                    sql_ds.SelectCommand = DAO_LinkPembelajaranEksternalBuka.SP_SELECT_HIT_COUNTER_FOR_SEARCH;
                }
                else
                {
                    sql_ds.SelectParameters.Clear();
                    sql_ds.SelectParameters.Add("Rel_Pegawai", Libs.LOGGED_USER_M.NoInduk);
                    sql_ds.SelectCommand = DAO_LinkPembelajaranEksternalBuka.SP_SELECT_HIT_COUNTER;
                }
            }

            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_kategori = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kategori");
            System.Web.UI.WebControls.Literal imgh_nama = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_nama");
            System.Web.UI.WebControls.Literal imgh_JumlahAksesSemua = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_JumlahAksesSemua");
            System.Web.UI.WebControls.Literal imgh_JumlahAksesGuruYbs = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_JumlahAksesGuruYbs");

            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_kategori.Text = html_image;
            imgh_nama.Text = html_image;
            imgh_JumlahAksesSemua.Text = html_image;
            imgh_JumlahAksesGuruYbs.Text = html_image;

            imgh_kategori.Visible = false;
            imgh_nama.Visible = false;
            imgh_JumlahAksesSemua.Visible = false;
            imgh_JumlahAksesGuruYbs.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Kategori":
                    imgh_kategori.Visible = true;
                    break;
                case "Nama":
                    imgh_nama.Visible = true;
                    break;
                case "JumlahAksesSemua":
                    imgh_JumlahAksesSemua.Visible = true;
                    break;
                case "JumlahAksesGuruYbs":
                    imgh_JumlahAksesGuruYbs.Visible = true;
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
            txtUsingFilter.Value = "";
            this.Master.txtCariData.Text = "";
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, "");
        }

        protected void InitFields()
        {
            string s_nama = "";
            Pegawai m = DAO_Pegawai.GetByID_Entity(Libs.LOGGED_USER_M.NoInduk);
            if (m != null)
            {
                if (m.Nama != null) s_nama = m.Nama;
            }

            txtID.Value = "";
            txtLinkTautanCredit.Text = s_nama;
            txtLinkTautan.Text = "";
            txtNamaTentang.Text = "";
            cboKategori.SelectedValue = "";
            txtKategori.Text = "";
            RenderLevelJenjang("", true);
        }

        protected void RenderLevelJenjang(string kode, bool enabled)
        {
            bool ada = false;
            string html = "";
            LinkPembelajaranEksternal m = (
                    kode.Trim() != ""
                    ? DAO_LinkPembelajaranEksternal.GetByID_Entity(kode)
                    : null
                );

            if (m != null)
            {
                if (m.Rel_Pegawai != null)
                {
                    ada = true;
                }
            }

            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m0 => m0.UrutanJenjang).ToList();
            if (!ada)
            {
                foreach (Sekolah item in lst_sekolah)
                {
                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\" style=\"padding-top: 5px; padding-bottom: 5px;\">" +
                                    "<label for=\"chk_" + item.Kode.ToString().Replace("-", "_") + "\">" +
                                        "<input " + (enabled ? "" : " disabled ") + " onchange=\"UnCheckSemua(this.checked)\" name=\"arr_unit[]\" value=\"" + item.Nama + "\" class=\"access-hide\" id=\"chk_" + item.Kode.ToString().Replace("-", "_") + "\" type=\"checkbox\"><span class=\"switch-toggle\"></span>" +
                                        "<span style=\"font-weight: normal; font-size: medium; color: grey;\">" +
                                            item.Nama +
                                        "</span>" +
                                    "</label>" +
                                "</div>" +
                            "</div>";
                }
                html += "<div class=\"row\">" +
                            "<div class=\"col-xs-12\" style=\"padding-top: 5px; padding-bottom: 5px;\">" + 
                                "<label for=\"chk_semua\">" +
                                    "<input " + (enabled ? "" : " disabled ") + " onchange=\"UnCheckAllUnit(this.checked)\" value=\"Semua\" class=\"access-hide\" id=\"chk_semua" + "\" type=\"checkbox\"><span class=\"switch-toggle\"></span>" +
                                    "<span style=\"font-weight: normal; font-size: medium; color: grey;\">" +
                                        "Semua" +
                                    "</span>" +
                                "</label>" +
                            "</div>" +
                        "</div>";
            }
            else
            {
                string[] arr_level = m.Unit.Split(new string[] { "," }, StringSplitOptions.None);
                foreach (Sekolah item in lst_sekolah)
                {
                    string s_checked = "";
                    if (arr_level.Contains(item.Nama))
                    {
                        s_checked = " checked ";
                    }

                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\" style=\"padding-top: 5px; padding-bottom: 5px;\">" +
                                    "<label for=\"chk_" + item.Kode.ToString().Replace("-", "_") + "\">" +
                                        "<input " + (enabled ? "" : " disabled ") + " onchange=\"UnCheckSemua(this.checked)\" name=\"arr_unit[]\" value=\"" + item.Nama + "\" " + s_checked + " class=\"access-hide\" id=\"chk_" + item.Kode.ToString().Replace("-", "_") + "\" type=\"checkbox\"><span class=\"switch-toggle\"></span>" +
								        "<span style=\"font-weight: normal; font-size: medium; color: grey;\">" +
									        item.Nama +
								        "</span>" +
                                    "</label>" +
                                "</div>" +
                            "</div>";
                }
                html += "<div class=\"row\">" +
                            "<div class=\"col-xs-12\" style=\"padding-top: 5px; padding-bottom: 5px;\"> " +
                                "<label for=\"chk_semua\">" +
                                    "<input " + (enabled ? "" : " disabled ") + " onchange=\"UnCheckAllUnit(this.checked)\" value=\"Semua\" " + (m.Unit.Trim().ToUpper() == "SEMUA" ? "checked" : "") + " class=\"access-hide\" id=\"chk_semua" + "\" type=\"checkbox\"><span class=\"switch-toggle\"></span>" +
                                    "<span style=\"font-weight: normal; font-size: medium; color: grey;\">" +
                                        "Semua" +
                                    "</span>" +
                                "</label>" +
                            "</div>" +
                        "</div>"; 
            }
                    
            ltrLevelJenjang.Text = html;
        }


        protected void RenderLevelJenjang_Filter()
        {
            string html = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m0 => m0.UrutanJenjang).ToList();
            foreach (Sekolah item in lst_sekolah)
            {
                html += "<div class=\"row\">" +
                            "<div class=\"col-xs-12\" style=\"padding-top: 5px; padding-bottom: 5px;\">" +
                                "<label for=\"chk_" + item.Kode.ToString().Replace("-", "_") + "\">" +
                                    "<input onchange=\"UnCheckSemua_Filter(this.checked)\" name=\"arr_unit_filter[]\" value=\"" + item.Nama + "\" class=\"access-hide\" id=\"chk_" + item.Kode.ToString().Replace("-", "_") + "\" type=\"checkbox\"><span class=\"switch-toggle\"></span>" +
                                    "<span style=\"font-weight: normal; font-size: medium; color: grey;\">" +
                                        item.Nama +
                                    "</span>" +
                                "</label>" +
                            "</div>" +
                        "</div>";
            }
            html += "<div class=\"row\">" +
                        "<div class=\"col-xs-12\" style=\"padding-top: 5px; padding-bottom: 5px;\"> " +
                            "<label for=\"chk_semua\">" +
                                "<input onchange=\"UnCheckAllUnit_Filter(this.checked)\" value=\"Semua\" class=\"access-hide\" id=\"chk_semua_filter" + "\" type=\"checkbox\"><span class=\"switch-toggle\"></span>" +
                                "<span style=\"font-weight: normal; font-size: medium; color: grey;\">" +
                                    "Semua" +
                                "</span>" +
                            "</label>" +
                        "</div>" +
                    "</div>";

            ltrLevelJenjang_Filter.Text = html;
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            InitInput();
            EnabledInput(true);
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Mapel.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
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
                LinkPembelajaranEksternal m = new LinkPembelajaranEksternal();
                if (txtID.Value.Trim() != "")
                {
                    m = DAO_LinkPembelajaranEksternal.GetByID_Entity(txtID.Value.Trim());
                }
                
                m.Kode = Guid.NewGuid();
                m.Rel_Pegawai = Libs.LOGGED_USER_M.NoInduk;
                m.Nama = txtNamaTentang.Text;
                m.Link = txtLinkTautan.Text;
                m.Kategori = (
                        cboKategori.SelectedValue.Trim().ToUpper() != "" &&
                        cboKategori.SelectedValue.Trim().ToUpper() != "LAINNYA"
                        ? cboKategori.SelectedValue
                        : txtKategori.Text
                    );
                m.Unit = txtUnit.Value;

                m.RTG_UNIT = "";
                m.RTG_LEVEL = "";
                m.RTG_SEMESTER = "";
                m.RTG_KELAS = "";
                m.RTG_SUBKELAS = "";

                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_LinkPembelajaranEksternal.Update(m);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_LinkPembelajaranEksternal.Insert(m);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text.Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
                }

                InitInput();
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
                LinkPembelajaranEksternal m = DAO_LinkPembelajaranEksternal.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        InitInput();

                        txtID.Value = m.Kode.ToString();
                        txtLinkTautanCredit.Text = DAO_Pegawai.GetByID_Entity(m.Rel_Pegawai).Nama;
                        txtNamaTentang.Text = m.Nama;
                        txtLinkTautan.Text = m.Link;
                        cboKategori.SelectedValue = m.Kategori;
                        txtUnit.Value = m.Unit;
                        RenderLevelJenjang(m.Kode.ToString(), (m.Rel_Pegawai == Libs.LOGGED_USER_M.NoInduk ? true : false));
                        EnabledInput((m.Rel_Pegawai == Libs.LOGGED_USER_M.NoInduk ? true : false));

                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void EnabledInput(bool value)
        {
            txtLinkTautan.Enabled = value;
            txtNamaTentang.Enabled = value;
            cboKategori.Enabled = value;
            txtKategori.Enabled = value;
            lnkOKInput.Visible = value;
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
                Mapel m = DAO_Mapel.GetByID_Entity(txtID.Value.Trim());
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

        protected void lnkOKInput_Filter_Click(object sender, EventArgs e)
        {
            txtUsingFilter.Value = "1";
            BindListView(true);
        }

        protected void btnDoFilter_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowFilter.ToString();
        }

        protected void btnDoRefresh_Click(object sender, EventArgs e)
        {
            BindListView(true);
        }

        protected void btnDoShowDownloadHist_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowDownloadHistLinkPembelajaran.ToString();
        }
    }
}