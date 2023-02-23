using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs.Elearning.KB;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.KB
{
    public partial class wf_DesainRapor : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATADESAINRAPOR_KB";
        public const string C_ID = "{{id}}";

        public enum JenisItem
        {
            KategoriPencapaian,
            SubKategoriPencapaian,
            PoinKategoriPencapaian,
            Rekomendasi
        }

        public enum JenisInput
        {
            ItemKriteria,
            ItemReguler,
            ItemEkskul
        }

        public class KriteriaPenilaian
        {
            public string Kode { get; set; }
            public int Urut { get; set; }
        }

        public class ItemPenilaian
        {
            public string Kode { get; set; }
            public JenisItem JenisItemPenilaian { get; set; }
            public int Urut { get; set; }
        }

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
            DoUpdatePoinPenilaian,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            DoShowConfirmPosting,
            DoShowInputDesainKategoriPencapaian,
            DoShowInputDesainSubKategoriPencapaian,
            DoShowInputDesainPoinKategoriPencapaian,
            DoShowKriteriaPencapaian,
            DoUpdateUrut,
            DoShowEditKategoriPencapaian,
            DoShowEditSubKategoriPencapaian,
            DoShowEditPoinKategoriPencapaian,
            DoShowPengaturanItemPenilaian,
            DoShowAddEkskul,
            DoUpdateItem,
            DoPosting
        }

        public static List<KriteriaPenilaian> lst_kriteria_penilaian = new List<KriteriaPenilaian>();
        public static List<ItemPenilaian> lst_item_penilaian = new List<ItemPenilaian>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" " +
                                            "src=\"" + ResolveUrl("~/Application_CLibs/images/svg/ebook-2.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Desain Rapor";
            if (GetTR() == TipeRapor.LTS)
            {
                this.Master.HeaderTittle += "<span style=\"font-weight: bold;\">LTS</span>";
            }
            else if (GetTR() == TipeRapor.SEMESTER)
            {
                this.Master.HeaderTittle += "<span style=\"font-weight: bold;\">Semester</span>";
            }

            if (!IsPostBack)
            {
                this.Master.ShowHeaderTools = true;
                this.Master.HeaderCardVisible = false;
                InitKeyEventClient();
                ListDropdown();
                ShowDataSiswa();
                if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
            }
            if (mvMain.ActiveViewIndex == 0)
            {
                BindListView(!IsPostBack, Libs.GetQ());
            }
        }

        protected void ListDropdown()
        {
            List<Sekolah> lst_unit = DAO_Sekolah.GetAll_Entity();
            Sekolah unit = lst_unit.FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.KB).FirstOrDefault();
            cboKelas.Items.Clear();
            if (unit != null)
            {
                if (unit.Nama != null)
                {
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(unit.Kode.ToString()).OrderBy(m => m.UrutanLevel).ToList();
                    cboKelas.Items.Add("");
                    foreach (var item in lst_kelas)
                    {
                        cboKelas.Items.Add(new ListItem
                        {
                            Value = item.Kode.ToString(),
                            Text = item.Nama
                        });
                    }
                }
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboJenisRapor.ClientID + "').focus(); return false; }");
            cboJenisRapor.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("TipeRapor", GetTR());
                sql_ds.SelectCommand = DAO_Rapor_Design.SP_SELECT_ALL_BY_TIPE_RAPOR_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TipeRapor", GetTR());
                sql_ds.SelectCommand = DAO_Rapor_Design.SP_SELECT_ALL_BY_TIPE_RAPOR;
            }

            if (GetTR() == TipeRapor.LTS)
            {
                ltrTipeRapor.Text = "<span style=\"font-weight: bold;\">LTS</span>";
            }
            else if (GetTR() == TipeRapor.SEMESTER)
            {
                ltrTipeRapor.Text = "<span style=\"font-weight: bold;\">Semester</span>";
            }

            if (isbind) lvData.DataBind();
        }

        private void BindListViewKriteria(string rel_desain_rapor, bool isbind = true)
        {
            sql_ds_kriteria.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_kriteria.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_kriteria.SelectParameters.Clear();
            sql_ds_kriteria.SelectParameters.Add("Rel_Rapor_Design", rel_desain_rapor);
            sql_ds_kriteria.SelectCommand = DAO_Rapor_DesignKriteria.SP_SELECT_BY_HEADER;
            if (isbind) lvKriteria.DataBind();
        }

        private void BindListViewDesain(string rel_desain_rapor, bool isbind = true)
        {
            sql_ds_desain.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_desain.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_desain.SelectParameters.Clear();
            sql_ds_desain.SelectParameters.Add("Rel_Rapor_Design", rel_desain_rapor);
            sql_ds_desain.SelectCommand = DAO_Rapor_DesignDet.SP_SELECT_BY_HEADER;
            if (isbind) lvDesain.DataBind();
        }

        private void BindListViewEkskul(string rel_desain_rapor, bool isbind = true)
        {
            sql_ds_ekskul.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_ekskul.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_ekskul.SelectParameters.Clear();
            sql_ds_ekskul.SelectParameters.Add("Rel_Rapor_Design", rel_desain_rapor);
            sql_ds_ekskul.SelectCommand = DAO_Rapor_DesignDetEkskul.SP_SELECT_BY_HEADER;
            if (isbind) lvEkskul.DataBind();
        }

        private void BindListViewEkskulBySiswa(string rel_desain_rapor, string rel_siswa, bool isbind = true)
        {
            sql_ds_ekskul.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_ekskul.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_ekskul.SelectParameters.Clear();
            sql_ds_ekskul.SelectParameters.Add("Rel_Rapor_Design", rel_desain_rapor);
            sql_ds_ekskul.SelectParameters.Add("Rel_Siswa", rel_siswa);
            sql_ds_ekskul.SelectCommand = DAO_Rapor_DesignDetEkskul.SP_SELECT_BY_HEADER_BY_SISWA_FOR_DESIGN;
            if (isbind) lvEkskul.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtTahunPelajaran.Text = "";
            cboSemester.SelectedValue = "";
            cboJenisRapor.SelectedValue = "";
            txtKeterangan.Text = "";
            cboKelas.SelectedValue = "";
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
                if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
                {
                    if (!GetCurrentData(txtID.Value).IsLocked)
                    {
                        DAO_Rapor_Design.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                        BindListView(!IsPostBack, Libs.GetQ().Trim());
                        txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected string GetTR() { return Libs.GetQueryString("tr").ToUpper(); }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                Rapor_Design m = new Rapor_Design();
                m.Kode = Guid.NewGuid();
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Kelas = new Guid(cboKelas.SelectedValue);
                m.TipeRapor = GetTR();
                m.Keterangan = txtKeterangan.Text;
                m.JenisRapor = cboJenisRapor.SelectedValue;
                if (txtID.Value.Trim() != "")
                {
                    if (GetCurrentData(txtID.Value) != null)
                    {
                        if (!GetCurrentData(txtID.Value).IsLocked)
                        {
                            
                        }
                        m.Kode = new Guid(txtID.Value);
                        DAO_Rapor_Design.Update(m, Libs.LOGGED_USER_M.UserID);
                        BindListView(!IsPostBack, Libs.GetQ().Trim());
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
                else
                {
                    if (DAO_Rapor_Design.GetAll_Entity().FindAll(m1 =>
                            m1.TahunAjaran == txtTahunPelajaran.Text &&
                            m1.Semester == cboSemester.SelectedValue &&
                            m1.TipeRapor == GetTR() &&
                            m1.Rel_Kelas == new Guid(cboKelas.SelectedValue)).Count > 0)
                    {
                        txtKeyAction.Value = "Desain rapor tahun pelajaran : " +
                                             txtTahunPelajaran.Text +
                                             ", semester : " +
                                             cboSemester.SelectedValue + " " +
                                             ", kelas : " +
                                             cboSemester.SelectedItem.Text + " " +
                                             ", jenis rapor : " +
                                             cboJenisRapor.SelectedItem.Text + " " +
                                             "sudah ada";
                        return;
                    }

                    DAO_Rapor_Design.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
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
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        txtKeterangan.Text = m.Keterangan;
                        cboJenisRapor.SelectedValue = m.JenisRapor;
                        cboSemester.SelectedValue = m.Semester;
                        cboKelas.SelectedValue = m.Rel_Kelas.ToString();
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL + (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : ""));
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void ShowDataList()
        {
            this.Master.ShowHeaderTools = true;
            mvMain.ActiveViewIndex = 0;
        }

        protected void ShowDesain(bool just_desain_ekskul = false)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrInfoDesain.Text = "<span style=\"font-weight: bold;\">" + m.TahunAjaran + "</span>" +
                                             "&nbsp;&nbsp;" +
                                             "<i class=\"fa fa-arrow-right\" style=\"font-weight: normal;\"></i>" +
                                             "&nbsp;&nbsp;" +
                                             "Semester " +
                                             "<span style=\"font-weight: bold;\">" + m.Semester + "</span>" +
                                             "&nbsp;&nbsp;" +
                                             "<i class=\"fa fa-arrow-right\" style=\"font-weight: normal;\"></i>" +
                                             "&nbsp;&nbsp;" +
                                             "Kelas " +
                                             "<span style=\"font-weight: bold;\">" + DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString()).Nama + "</span>" +
                                             "&nbsp;&nbsp;" +
                                             "<i class=\"fa fa-arrow-right\" style=\"font-weight: normal;\"></i>" +
                                             "&nbsp;&nbsp;" +
                                             "<span style=\"font-weight: bold;\">" + m.JenisRapor + "</span>";

                        this.Session[SessionViewDataName] = 0;
                        this.Master.ShowHeaderTools = false;

                        //show kelas pertama
                        txtKelasDet.Value = "";
                        txtCountSiswa.Value = "";
                        txtIndexSiswa.Value = "";
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                foreach (KelasDet kelas_det in DAO_KelasDet.GetByKelas_Entity(m_kelas.Kode.ToString()).
                                        OrderBy(m0 => m0.UrutanKelas).
                                        ToList().
                                        FindAll(m0 => m0.Nama.Trim().ToLower() != m_kelas.Nama.Trim().ToLower())
                                ){
                                    if (kelas_det != null)
                                    {
                                        if (kelas_det.Nama != null)
                                        {
                                            if (
                                                kelas_det.IsAktif && DAO_Siswa.GetByRombel_Entity(
                                                        m_kelas.Rel_Sekolah.ToString(),
                                                        kelas_det.Kode.ToString(),
                                                        txtTahunAjaran.Value,
                                                        txtSemester.Value
                                                    ).Count > 0
                                            )
                                            {
                                                txtKelasDet.Value = kelas_det.Kode.ToString();
                                                txtCountSiswa.Value = GetListSiswa().Count.ToString();
                                                txtIndexSiswa.Value = "0";
                                                txtTahunAjaran.Value = m.TahunAjaran;
                                                txtSemester.Value = m.Semester;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //end show kelas pertama
                        ShowListSiswa();
                        div_pencapaian_perkembangan.Visible = true;
                        div_kriteria_penilaian.Visible = true;
                        lnkAddKategoriPencapaian.Visible = true;
                        lnkAddKategoriPencapaianEkskul.Visible = true;
                        lnkAddSubKategoriPencapaian.Visible = true;
                        lnkAddSubKategoriPencapaianEkskul.Visible = true;
                        lnkAddPoinKategoriPencapaian.Visible = true;
                        lnkAddPoinKategoriPencapaianEkskul.Visible = true;
                        lnkAddRekomendasi.Visible = true;
                        lnkAddRekomendasiEkskul.Visible = true;
                        div_button_settings_rapor_design.Visible = true; //!IsLocked(txtID.Value);
                        div_pop_up_siswa.Visible = false;
                        div_nav_siswa_dp_desain.Visible = false;
                        div_list_desain_ekskul.Visible = false;

                        if (just_desain_ekskul)
                        {
                            div_pop_up_siswa.Visible = true;
                            div_nav_siswa_dp_desain.Visible = true;
                            div_list_desain_ekskul.Visible = true;

                            BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            div_pencapaian_perkembangan.Visible = false;
                            div_kriteria_penilaian.Visible = false;
                            lnkAddEkskul.Visible = true;
                            div_hapus_item_rapor.Visible = true;
                            div_button_settings_rapor_design.Visible = false;
                        }
                        else
                        {
                            BindDataDesain();
                        }

                        mvMain.ActiveViewIndex = 1;
                    }
                }
            }
        }

        protected void ShowListSiswa()
        {
            ltrListSiswa.Text = "";
            int id = 1;
            foreach (Siswa m_siswa in GetListSiswa())
            {
                string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                ltrListSiswa.Text += "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"width: 100%;\">" +
                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                "<tr>" +
                                                    "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                        id.ToString() +
                                                        "." +
                                                    "</td>" +
                                                    "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                        "<input name=\"txt_siswa[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                        "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                            "style=\"height: 32px; width: 32px; border-radius: 100%;\">" +
                                                    "</td>" +
                                                    "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                        "<span style=\"color: grey; font-weight: bold;\">" +
                                                            Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 3) +
                                                            (
                                                                m_siswa.Panggilan.Trim() != ""
                                                                ? "<br />" +
                                                                  "<span style=\"color: #bfbfbf; font-weight: normal\">" +
                                                                    Libs.GetNamaPanggilan(m_siswa.Panggilan) +
                                                                  "</span>"
                                                                : ""
                                                            ) +
                                                        "</span>" +
                                                    "</td>" +
                                                    "<td style=\"width: 50px; text-align: right; vertical-align: middle; padding-right: 0px;\">" +
                                                        "<label id=\"img_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                               "style=\"display: none; font-size: small; color: grey; font-weight: bold;\">" +

                                                            "<img src=\"" + ResolveUrl("~/Application_CLibs/images/giphy.gif") + "\" " +
                                                                    "style=\"height: 16px; width: 20px;\" />" +

                                                        "</label>" +
                                                        "<a id=\"lbl_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                            "onclick=\"ShowProsesPilihSiswa('" + m_siswa.Kode.ToString() + "', true); " + txtIndexSiswa.ClientID + ".value = '" + (id - 1) + "'; " + btnShowDesainEkskulSiswa.ClientID + ".click(); \"" +
                                                            "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #1DA1F2; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                            "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                            "title=\" Buka \">" +
                                                                "<i class=\"fa fa-folder-open\"></i>&nbsp;&nbsp;Buka" +
                                                        "</a>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; margin-top: 5px; margin-bottom: 5px; border-color: #E9EFF5;\" />" +
                                        "</div>" +
                                    "</div>";

                id++;
            }

            Rapor_Design m_rapor = DAO_Rapor_Design.GetByID_Entity(txtID.Value);
            if (m_rapor != null)
            {
                if (m_rapor.TahunAjaran != null)
                {
                    ShowListKelas(m_rapor.Rel_Kelas.ToString());
                }
            }
            txtCountSiswa.Value = GetListSiswa().Count.ToString();
            txtIndexSiswa.Value = "0";
            ShowDataSiswa();
        }

        protected List<Siswa> GetListSiswa()
        {
            Sekolah unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.KB).FirstOrDefault();
            return DAO_Siswa.GetByRombel_Entity(
                        unit.Kode.ToString(),
                        txtKelasDet.Value,
                        txtTahunAjaran.Value,
                        txtSemester.Value
                    );
        }

        protected void ShowDataSiswa()
        {
            if (Libs.GetStringToInteger(txtIndexSiswa.Value) >= 0 &&
                Libs.GetStringToInteger(txtCountSiswa.Value) > 0)
            {
                txtIDSiswa.Value = "";
                Siswa m_siswa = GetListSiswa()[Libs.GetStringToInteger(txtIndexSiswa.Value)];
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {

                        txtIDSiswa.Value = m_siswa.Kode.ToString();
                        lblNamaSiswa.Text = Libs.GetPersingkatNama(Libs.GetPerbaikiEjaanNama(m_siswa.Nama)) +
                                            (
                                                m_siswa.Panggilan.Trim() != ""
                                                ? " (" + Libs.GetPerbaikiEjaanNama(m_siswa.Panggilan.Trim().ToLower()) + ")"
                                                : ""
                                            );
                        lblKelasSiswa.Text = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet).Nama;
                        lblNamaSiswaInfo.Text = lblNamaSiswa.Text;
                        lblKelasSiswaInfo.Text = lblKelasSiswa.Text;
                        lblInfoPeriode.Text = "<span style=\"font-weight: bold;\">" +
                                                txtTahunAjaran.Value +
                                              "</span>" +
                                              (
                                                txtSemester.Value != ""
                                                ? "&nbsp;Semester&nbsp;" +
                                                  "<span style=\"font-weight: bold;\">" +
                                                    txtSemester.Value +
                                                  "</span>"
                                                : ""
                                              );

                    }
                }
            }
        }

        protected void ShowListKelas(string rel_kelas)
        {
            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {

                    List<KelasDet> lst_kelas_det = DAO_KelasDet.GetByKelas_Entity(rel_kelas).OrderBy(m => m.UrutanKelas).ToList();
                    ltrListKelas.Text = "";
                    int id = 1;
                    foreach (KelasDet kelas in lst_kelas_det)
                    {
                        if (kelas.Nama.Trim().ToLower() != m_kelas.Nama.Trim().ToLower())
                        {
                            if (kelas.IsAktif && DAO_Siswa.GetByRombel_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    kelas.Kode.ToString(),
                                    txtTahunAjaran.Value,
                                    txtSemester.Value
                                ).Count > 0
                            )
                            {

                                ltrListKelas.Text += "<div class=\"row\">" +
                                                        "<div class=\"col-xs-12\" style=\"width: 100%;\">" +
                                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                                "<tr>" +
                                                                    "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                        id.ToString() +
                                                                        "." +
                                                                    "</td>" +
                                                                    "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                        "<span style=\"color: grey; font-weight: bold;\">" +
                                                                            kelas.Nama +
                                                                        "</span>" +
                                                                    "</td>" +
                                                                    "<td style=\"width: 50px; text-align: right; vertical-align: middle; padding-right: 0px;\">" +
                                                                        "<label id=\"img_" + kelas.Kode.ToString().Replace("-", "_") + "\" " +
                                                                               "style=\"display: none; font-size: small; color: grey; font-weight: bold;\">" +

                                                                            "<img src=\"" + ResolveUrl("~/Application_CLibs/images/giphy.gif") + "\" " +
                                                                                    "style=\"height: 16px; width: 20px;\" />" +

                                                                        "</label>" +
                                                                        "<a id=\"lbl_" + kelas.Kode.ToString().Replace("-", "_") + "\" " +
                                                                            "onclick=\"" + txtKelasDet.ClientID + ".value = '" + kelas.Kode.ToString() + "'; " +
                                                                                          "ShowProsesPilihKelas('" + kelas.Kode.ToString() + "', true); " +
                                                                                           btnShowKelas.ClientID + ".click(); \"" +
                                                                            "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #1DA1F2; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                                            "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                            "title=\" Buka \">" +
                                                                                "<i class=\"fa fa-folder-open\"></i>&nbsp;&nbsp;Buka" +
                                                                        "</a>" +
                                                                    "</td>" +
                                                                "</tr>" +
                                                            "</table>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class=\"row\">" +
                                                        "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px; margin-top: 5px; margin-bottom: 5px; border-color: #E9EFF5;\" />" +
                                                        "</div>" +
                                                    "</div>";
                                id++;
                            }
                        }

                    }

                }
            }
        }

        protected void BindDataDesain()
        {
            BindListViewKriteria(txtID.Value, true);
            BindListViewDesain(txtID.Value, true);
            BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);

            txtIDJenisInsert.Value = "";
        }

        protected void btnShowDesain_Click(object sender, EventArgs e)
        {
            txtShowInputPencapaian.Value = "1";
            bool is_locked = IsLocked(txtID.Value);
            lnkAddKriteria.Visible = !is_locked;
            lnkAddEkskul.Visible = !is_locked;
            div_hapus_item_rapor.Visible = !is_locked;
            div_hapus_item_rapor.Visible = true;
            ShowDataSiswa();
            ShowDesain();
        }

        protected void ShowKategoriPencapaian()
        {
            txtIDItemPenilaian.Value = "";
            txtPoinItemKategoriPencapaian.Text = "";
            txtKategoriPencapaian.Text = "";
            txtKategoriPencapaianVal.Value = "";
            txtSubKategoriPencapaianVal.Value = "";
            txtPoinKategoriPencapaianVal.Value = "";
            txtIDJenisInput.Value = JenisInput.ItemReguler.ToString();
            txtKeyAction.Value = JenisAction.DoShowInputDesainKategoriPencapaian.ToString();
        }

        protected void lnkAddKategoriPencapaian_Click(object sender, EventArgs e)
        {
            ShowKategoriPencapaian();
        }

        protected void ShowSubKategoriPencapaian()
        {
            txtIDItemPenilaian.Value = "";
            txtPoinItemSubKategoriPencapaian.Text = "";
            txtKategoriPencapaian.Text = "";
            txtKategoriPencapaianVal.Value = "";
            txtSubKategoriPencapaian.Text = "";
            txtSubKategoriPencapaianVal.Value = "";
            txtPoinKategoriPencapaianVal.Value = "";
            txtIDJenisInput.Value = JenisInput.ItemReguler.ToString();
            txtKeyAction.Value = JenisAction.DoShowInputDesainSubKategoriPencapaian.ToString();
        }

        protected void lnkAddSubKategoriPencapaian_Click(object sender, EventArgs e)
        {
            ShowSubKategoriPencapaian();
        }

        protected void ShowInputPoinKategoriPencapaian(bool do_show = true)
        {
            txtIDItemPenilaian.Value = "";
            txtPoinItemPoinKategoriPencapaian.Text = "";
            txtKategoriPencapaianVal.Value = "";
            txtSubKategoriPencapaianVal.Value = "";
            txtPoinKategoriPencapaian.Text = "";
            txtPoinKategoriPencapaianVal.Value = "";
            txtIDJenisInput.Value = JenisInput.ItemReguler.ToString();
            if (do_show) txtKeyAction.Value = JenisAction.DoShowInputDesainPoinKategoriPencapaian.ToString();
        }

        protected void lnkAddPoinKategoriPencapaian_Click(object sender, EventArgs e)
        {
            ShowInputPoinKategoriPencapaian();
        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            txtIDItemPenilaian.Value = "";
            txtIDKriteriaEdit.Value = "";
            ShowDataList();
        }

        protected void ShowHTMLListKriteria(string selected_kode = "")
        {
            ltrKriteriaPenilaian.Text = "";
            string html = "";
            List<Rapor_Kriteria> lst_kriteria = DAO_Rapor_Kriteria.GetAll_Entity().OrderBy(m => m.Nama).ToList();
            foreach (Rapor_Kriteria item in lst_kriteria)
            {
                string s_id = "chk_" + item.Kode.ToString().Replace("-", "_");
                html += "<div class=\"row\">" +
                            "<div class=\"col-xs-12\">" +
                                "<div class=\"form-group form-group-label\" style=\"margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px; margin-bottom: 0px;\">" +
                                    "<div class=\"radiobtn radiobtn-adv\">" +
                                        "<label for=\"" + s_id + "\" style=\"font-weight: bold; color: grey;\">" +
                                            "<input " + (item.Kode.ToString().ToLower().Trim() == selected_kode.ToLower().Trim() ? " checked=\"checked\" " : "") + " value=\"" + item.Kode.ToString() + "\" name=\"rdo_kriteria[]\" type=\"radio\" id=\"" + s_id + "\" class=\"access-hide\" />" +
                                            "<span class=\"radiobtn-circle\"></span>" +
                                            "<span class=\"radiobtn-circle-check\"></span>" +
                                            "<span style=\"font-weight: bold; color: black;\">" +
                                                item.Alias +
                                            "</span>" +
                                            "<br />" +
                                            "<span style=\"font-weight: normal;\">" +
                                                Libs.GetHTMLSimpleText(item.Nama) +
                                            "</span>" +
                                        "</label>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                        "<div class=\"row\">" +
                            "<div class=\"col-xs-12\" style=\"margin: 0px;\">" +
                                "<hr style=\"margin: 0px;\" />" +
                            "</div>" +
                        "</div>";
            }
            ltrKriteriaPenilaian.Text = html;
        }

        protected void lnkAddKriteria_Click(object sender, EventArgs e)
        {
            txtIDKriteriaEdit.Value = "";
            ShowHTMLListKriteria();
            txtKeyAction.Value = JenisAction.DoShowKriteriaPencapaian.ToString();
        }

        protected void lnkAddRekomendasi_Click(object sender, EventArgs e)
        {
            txtIDItemPenilaian.Value = "";
            if (txtIDJenisInsert.Value.Trim() != "")
            {
                Guid id_guid = Guid.NewGuid();
                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                {
                    Kode = id_guid,
                    Rel_Rapor_Design = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(Constantas.GUID_NOL),
                    JenisKomponen = JenisKomponenRapor.Rekomendasi
                }, Libs.LOGGED_USER_M.UserID);
                if (txtListIDItemPenilaian.Value.Trim() != "")
                {
                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int id = 1;
                    foreach (string kode in arr_kode)
                    {
                        DAO_Rapor_DesignDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                        id++;
                    }
                }
            }
            else
            {
                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                {
                    Kode = Guid.NewGuid(),
                    Rel_Rapor_Design = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(Constantas.GUID_NOL),
                    JenisKomponen = JenisKomponenRapor.Rekomendasi
                }, Libs.LOGGED_USER_M.UserID);
            }
            BindDataDesain();
        }

        protected void lnkOKKategoriPencapaian_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
            {
                Guid kode_kategori_pencapaian = Guid.NewGuid();
                if (DAO_Rapor_KategoriPencapaian.GetAll_Entity().FindAll(m => m.Nama == txtKategoriPencapaianVal.Value).Count > 0)
                {
                    kode_kategori_pencapaian = DAO_Rapor_KategoriPencapaian.GetAll_Entity().FindAll(m => m.Nama == txtKategoriPencapaianVal.Value).FirstOrDefault().Kode;
                }
                else
                {
                    DAO_Rapor_KategoriPencapaian.Insert(new Rapor_KategoriPencapaian
                    {
                        Kode = kode_kategori_pencapaian,
                        Nama = txtKategoriPencapaianVal.Value,
                        Keterangan = ""
                    }, Libs.LOGGED_USER_M.UserID);
                }

                //if (!GetCurrentData(txtID.Value).IsLocked || txtShowInputPencapaian.Value == "0")
                //{
                    if (txtIDJenisInput.Value == JenisInput.ItemReguler.ToString())
                    {
                        if (txtIDItemPenilaian.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignDet.Update(new Rapor_DesignDet
                            {
                                Kode = new Guid(txtIDItemPenilaian.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Poin = txtPoinItemKategoriPencapaian.Text,
                                Rel_KomponenRapor = kode_kategori_pencapaian,
                                JenisKomponen = JenisKomponenRapor.KategoriPencapaian
                            }, Libs.LOGGED_USER_M.UserID);
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                        }
                        else
                        {
                            if (txtIDJenisInsert.Value.Trim() != "")
                            {
                                Guid id_guid = Guid.NewGuid();
                                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                                {
                                    Kode = id_guid,
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.KategoriPencapaian
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtListIDItemPenilaian.Value.Trim() != "")
                                {
                                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    int id = 1;
                                    foreach (string kode in arr_kode)
                                    {
                                        DAO_Rapor_DesignDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                        id++;
                                    }
                                }
                            }
                            else
                            {
                                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.KategoriPencapaian
                                }, Libs.LOGGED_USER_M.UserID);
                            }

                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoAdd.ToString();
                        }
                    }
                    else if (txtIDJenisInput.Value == JenisInput.ItemEkskul.ToString())
                    {
                        if (txtIDItemPenilaian.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignDetEkskul.Update(new Rapor_DesignDetEkskul
                            {
                                Kode = new Guid(txtIDItemPenilaian.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Poin = txtPoinItemKategoriPencapaian.Text,
                                Rel_KomponenRapor = kode_kategori_pencapaian,
                                JenisKomponen = JenisKomponenRapor.KategoriPencapaian,
                                Rel_Siswa = txtIDSiswa.Value,
                                Rel_KelasDet = txtKelasDet.Value
                            }, Libs.LOGGED_USER_M.UserID);
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                        }
                        else
                        {
                            if (txtIDJenisInsert.Value.Trim() != "")
                            {
                                Guid id_guid = Guid.NewGuid();
                                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                                {
                                    Kode = id_guid,
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.KategoriPencapaian,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_KelasDet = txtKelasDet.Value
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtListIDItemPenilaian.Value.Trim() != "")
                                {
                                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    int id = 1;
                                    foreach (string kode in arr_kode)
                                    {
                                        DAO_Rapor_DesignDetEkskul.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                        id++;
                                    }
                                }
                            }
                            else
                            {
                                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.KategoriPencapaian,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_KelasDet = txtKelasDet.Value
                                }, Libs.LOGGED_USER_M.UserID);
                            }

                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoAdd.ToString();
                        }
                    }
                //}
            }
        }

        protected void lnkOKSubKategoriPencapaian_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
            {
                Guid kode_sub_kategori_pencapaian = Guid.NewGuid();
                if (DAO_Rapor_SubKategoriPencapaian.GetAll_Entity().FindAll(m => m.Nama == txtSubKategoriPencapaianVal.Value).Count > 0)
                {
                    kode_sub_kategori_pencapaian = DAO_Rapor_SubKategoriPencapaian.GetAll_Entity().FindAll(m => m.Nama == txtSubKategoriPencapaianVal.Value).FirstOrDefault().Kode;
                }
                else
                {
                    DAO_Rapor_SubKategoriPencapaian.Insert(new Rapor_SubKategoriPencapaian
                    {
                        Kode = kode_sub_kategori_pencapaian,
                        Nama = txtSubKategoriPencapaianVal.Value,
                        Keterangan = ""
                    }, Libs.LOGGED_USER_M.UserID);
                }

                //if (!GetCurrentData(txtID.Value).IsLocked || txtShowInputPencapaian.Value == "0")
                //{
                    if (txtIDJenisInput.Value == JenisInput.ItemReguler.ToString())
                    {
                        if (txtIDItemPenilaian.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignDet.Update(new Rapor_DesignDet
                            {
                                Kode = new Guid(txtIDItemPenilaian.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Poin = txtPoinItemSubKategoriPencapaian.Text,
                                Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                                JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian
                            }, Libs.LOGGED_USER_M.UserID);
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                        }
                        else
                        {
                            if (txtIDJenisInsert.Value.Trim() != "")
                            {
                                Guid id_guid = Guid.NewGuid();
                                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                                {
                                    Kode = id_guid,
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemSubKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtListIDItemPenilaian.Value.Trim() != "")
                                {
                                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    int id = 1;
                                    foreach (string kode in arr_kode)
                                    {
                                        DAO_Rapor_DesignDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                        id++;
                                    }
                                }
                            }
                            else
                            {
                                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemSubKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian
                                }, Libs.LOGGED_USER_M.UserID);
                            }
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoAdd.ToString();
                        }
                    }
                    else if (txtIDJenisInput.Value == JenisInput.ItemEkskul.ToString())
                    {
                        if (txtIDItemPenilaian.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignDetEkskul.Update(new Rapor_DesignDetEkskul
                            {
                                Kode = new Guid(txtIDItemPenilaian.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Poin = txtPoinItemSubKategoriPencapaian.Text,
                                Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                                JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian,
                                Rel_Siswa = txtIDSiswa.Value,
                                Rel_KelasDet = txtKelasDet.Value
                            }, Libs.LOGGED_USER_M.UserID);
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                        }
                        else
                        {
                            if (txtIDJenisInsert.Value.Trim() != "")
                            {
                                Guid id_guid = Guid.NewGuid();
                                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                                {
                                    Kode = id_guid,
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemSubKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_KelasDet = txtKelasDet.Value
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtListIDItemPenilaian.Value.Trim() != "")
                                {
                                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    int id = 1;
                                    foreach (string kode in arr_kode)
                                    {
                                        DAO_Rapor_DesignDetEkskul.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                        id++;
                                    }
                                }
                            }
                            else
                            {
                                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemSubKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_KelasDet = txtKelasDet.Value
                                }, Libs.LOGGED_USER_M.UserID);
                            }
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoAdd.ToString();
                        }
                    }
                //}
            }
        }

        protected void lnkOKPoinPencapaian_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
            {
                Guid kode_poin_kategori_pencapaian = Guid.NewGuid();
                if (DAO_Rapor_PoinKategoriPencapaian.GetAll_Entity().FindAll(m => m.Nama == txtPoinKategoriPencapaianVal.Value).Count > 0)
                {
                    kode_poin_kategori_pencapaian = DAO_Rapor_PoinKategoriPencapaian.GetAll_Entity().FindAll(m => m.Nama == txtPoinKategoriPencapaianVal.Value).FirstOrDefault().Kode;
                }
                else
                {
                    DAO_Rapor_PoinKategoriPencapaian.Insert(new Rapor_PoinKategoriPencapaian
                    {
                        Kode = kode_poin_kategori_pencapaian,
                        Nama = txtPoinKategoriPencapaianVal.Value,
                        Keterangan = ""
                    }, Libs.LOGGED_USER_M.UserID);
                }

                //if (!GetCurrentData(txtID.Value).IsLocked || txtShowInputPencapaian.Value == "0")
                //{
                    if (txtIDJenisInput.Value == JenisInput.ItemReguler.ToString())
                    {
                        if (txtIDItemPenilaian.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignDet.Update(new Rapor_DesignDet
                            {
                                Kode = new Guid(txtIDItemPenilaian.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Poin = txtPoinItemPoinKategoriPencapaian.Text,
                                Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                                JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian
                            }, Libs.LOGGED_USER_M.UserID);
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                        }
                        else
                        {
                            if (txtIDJenisInsert.Value.Trim() != "")
                            {
                                Guid id_guid = Guid.NewGuid();
                                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                                {
                                    Kode = id_guid,
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemPoinKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtListIDItemPenilaian.Value.Trim() != "")
                                {
                                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    int id = 1;
                                    foreach (string kode in arr_kode)
                                    {
                                        DAO_Rapor_DesignDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                        id++;
                                    }
                                }
                                if (txtShowInputPencapaian.Value == "1")
                                {
                                    BindDataDesain();
                                }
                                else
                                {
                                    BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                                }
                                ShowInputPoinKategoriPencapaian(false);
                                txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                            }
                            else
                            {
                                DAO_Rapor_DesignDet.Insert(new Rapor_DesignDet
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemPoinKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtShowInputPencapaian.Value == "1")
                                {
                                    BindDataDesain();
                                }
                                else
                                {
                                    BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                                }
                                ShowInputPoinKategoriPencapaian(false);
                                txtIDJenisInput.Value = JenisInput.ItemReguler.ToString();
                                txtKeyAction.Value = JenisAction.DoUpdatePoinPenilaian.ToString();
                            }
                        }
                    }
                    else if (txtIDJenisInput.Value == JenisInput.ItemEkskul.ToString())
                    {
                        if (txtIDItemPenilaian.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignDetEkskul.Update(new Rapor_DesignDetEkskul
                            {
                                Kode = new Guid(txtIDItemPenilaian.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Poin = txtPoinItemPoinKategoriPencapaian.Text,
                                Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                                JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian,
                                Rel_Siswa = txtIDSiswa.Value,
                                Rel_KelasDet = txtKelasDet.Value
                            }, Libs.LOGGED_USER_M.UserID);
                            if (txtShowInputPencapaian.Value == "1")
                            {
                                BindDataDesain();
                            }
                            else
                            {
                                BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                            }
                            txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                        }
                        else
                        {
                            if (txtIDJenisInsert.Value.Trim() != "")
                            {
                                Guid id_guid = Guid.NewGuid();
                                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                                {
                                    Kode = id_guid,
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemPoinKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_KelasDet = txtKelasDet.Value
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtListIDItemPenilaian.Value.Trim() != "")
                                {
                                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    int id = 1;
                                    foreach (string kode in arr_kode)
                                    {
                                        DAO_Rapor_DesignDetEkskul.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                        id++;
                                    }
                                }
                                if (txtShowInputPencapaian.Value == "1")
                                {
                                    BindDataDesain();
                                }
                                else
                                {
                                    BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                                }
                                ShowInputPoinKategoriPencapaian(false);
                                txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                            }
                            else
                            {
                                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(txtID.Value),
                                    Poin = txtPoinItemPoinKategoriPencapaian.Text,
                                    Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                                    JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian,
                                    Rel_Siswa = txtIDSiswa.Value,
                                    Rel_KelasDet = txtKelasDet.Value
                                }, Libs.LOGGED_USER_M.UserID);
                                if (txtShowInputPencapaian.Value == "1")
                                {
                                    BindDataDesain();
                                }
                                else
                                {
                                    BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                                }
                                ShowInputPoinKategoriPencapaian(false);
                                txtIDJenisInput.Value = JenisInput.ItemEkskul.ToString();
                                txtKeyAction.Value = JenisAction.DoUpdatePoinPenilaian.ToString();
                            }
                        }
                    }
                //}
            }
        }

        protected void lnkOKKriteriaPencapaian_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
            {
                if (!GetCurrentData(txtID.Value).IsLocked)
                {
                    if (txtIDJenisInsert.Value.Trim() != "")
                    {
                        Guid id_guid = Guid.NewGuid();
                        DAO_Rapor_DesignKriteria.Insert(new Rapor_DesignKriteria
                        {
                            Kode = id_guid,
                            Rel_Rapor_Design = new Guid(txtID.Value),
                            Rel_Rapor_Kriteria = new Guid(txtIDKriteria.Value)
                        }, Libs.LOGGED_USER_M.UserID);
                        if (txtListIDItemPenilaian.Value.Trim() != "")
                        {
                            string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                                Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            int id = 1;
                            foreach (string kode in arr_kode)
                            {
                                DAO_Rapor_DesignKriteria.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                id++;
                            }
                        }
                        BindDataDesain();
                        txtKeyAction.Value = JenisAction.DoAdd.ToString();
                    }
                    else
                    {
                        if (txtIDKriteria.Value.Trim() != "" && txtIDKriteriaEdit.Value.Trim() == "")
                        {
                            DAO_Rapor_DesignKriteria.Insert(new Rapor_DesignKriteria
                            {
                                Kode = Guid.NewGuid(),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Rel_Rapor_Kriteria = new Guid(txtIDKriteria.Value)
                            }, Libs.LOGGED_USER_M.UserID);
                            BindDataDesain();
                            txtKeyAction.Value = JenisAction.DoAdd.ToString();
                        }
                        else if (txtIDKriteriaEdit.Value.Trim() != "")
                        {
                            DAO_Rapor_DesignKriteria.Update(new Rapor_DesignKriteria
                            {
                                Kode = new Guid(txtIDKriteriaEdit.Value),
                                Rel_Rapor_Design = new Guid(txtID.Value),
                                Rel_Rapor_Kriteria = new Guid(txtIDKriteria.Value)
                            }, Libs.LOGGED_USER_M.UserID);
                            BindDataDesain();
                            txtKeyAction.Value = JenisAction.DoAdd.ToString();
                        }
                    }
                    txtIDJenisInsert.Value = "";
                }
            }
        }

        protected void lnkOKHapusItemPenilaian_Click(object sender, EventArgs e)
        {
            if (txtIDItemPenilaian.Value.Trim() != "")
            {
                if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
                {
                    //if (!GetCurrentData(txtID.Value).IsLocked || txtShowInputPencapaian.Value == "0")
                    //{
                        string[] arr_id = txtIDItemPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string id in arr_id)
                        {
                            DAO_Rapor_DesignKriteria.Delete(id, Libs.LOGGED_USER_M.UserID);
                            DAO_Rapor_DesignDet.Delete(id, Libs.LOGGED_USER_M.UserID);
                            DAO_Rapor_DesignDetEkskul.Delete(id, Libs.LOGGED_USER_M.UserID);
                        }
                        txtIDItemPenilaian.Value = "";
                        if (txtShowInputPencapaian.Value == "1")
                        {
                            BindDataDesain();
                        }
                        else
                        {
                            BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                        }
                        txtKeyAction.Value = JenisAction.DoDelete.ToString();
                    //}
                }
            }
        }

        protected void btnShowEditItemPenilaian_Click(object sender, EventArgs e)
        {
            string s_komponen_rapor = "";

            if (txtIDJenisInput.Value == JenisInput.ItemReguler.ToString())
            {
                if (txtIDItemPenilaian.Value.Trim() != "")
                {
                    Rapor_DesignDet item_rapor = DAO_Rapor_DesignDet.GetByID_Entity(txtIDItemPenilaian.Value);
                    if (item_rapor != null)
                    {
                        if (item_rapor.Poin != null)
                        {
                            switch (item_rapor.JenisKomponen)
                            {
                                case JenisKomponenRapor.KategoriPencapaian:
                                    var m0 = DAO_Rapor_KategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m0 != null) if (m0.Nama != null) s_komponen_rapor = m0.Nama;

                                    txtPoinItemKategoriPencapaian.Text = item_rapor.Poin;
                                    txtKategoriPencapaian.Text = s_komponen_rapor;
                                    txtKategoriPencapaianVal.Value = s_komponen_rapor;
                                    BindListViewDesain(txtID.Value, true);
                                    txtKeyAction.Value = JenisAction.DoShowEditKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.SubKategoriPencapaian:
                                    var m1 = DAO_Rapor_SubKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m1 != null) if (m1.Nama != null) s_komponen_rapor = m1.Nama;

                                    txtPoinItemSubKategoriPencapaian.Text = item_rapor.Poin;
                                    txtSubKategoriPencapaian.Text = s_komponen_rapor;
                                    txtSubKategoriPencapaianVal.Value = s_komponen_rapor;
                                    BindListViewDesain(txtID.Value, true);
                                    txtKeyAction.Value = JenisAction.DoShowEditSubKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.PoinKategoriPencapaian:
                                    var m2 = DAO_Rapor_PoinKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m2 != null) if (m2.Nama != null) s_komponen_rapor = m2.Nama;

                                    txtPoinItemPoinKategoriPencapaian.Text = item_rapor.Poin;
                                    txtPoinKategoriPencapaian.Text = s_komponen_rapor;
                                    txtPoinKategoriPencapaianVal.Value = s_komponen_rapor;
                                    BindListViewDesain(txtID.Value, true);
                                    txtKeyAction.Value = JenisAction.DoShowEditPoinKategoriPencapaian.ToString();
                                    break;
                            }
                        }
                    }
                }
            }
            else if (txtIDJenisInput.Value == JenisInput.ItemEkskul.ToString())
            {
                if (txtIDItemPenilaian.Value.Trim() != "")
                {
                    Rapor_DesignDetEkskul item_rapor = DAO_Rapor_DesignDetEkskul.GetByID_Entity(txtIDItemPenilaian.Value);
                    if (item_rapor != null)
                    {
                        if (item_rapor.Poin != null)
                        {
                            switch (item_rapor.JenisKomponen)
                            {
                                case JenisKomponenRapor.KategoriPencapaian:
                                    var m0 = DAO_Rapor_KategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m0 != null) if (m0.Nama != null) s_komponen_rapor = m0.Nama;

                                    txtPoinItemKategoriPencapaian.Text = item_rapor.Poin;
                                    txtKategoriPencapaian.Text = s_komponen_rapor;
                                    txtKategoriPencapaianVal.Value = s_komponen_rapor;
                                    BindListViewDesain(txtID.Value, true);
                                    txtKeyAction.Value = JenisAction.DoShowEditKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.SubKategoriPencapaian:
                                    var m1 = DAO_Rapor_SubKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m1 != null) if (m1.Nama != null) s_komponen_rapor = m1.Nama;

                                    txtPoinItemSubKategoriPencapaian.Text = item_rapor.Poin;
                                    txtSubKategoriPencapaian.Text = s_komponen_rapor;
                                    txtSubKategoriPencapaianVal.Value = s_komponen_rapor;
                                    BindListViewDesain(txtID.Value, true);
                                    txtKeyAction.Value = JenisAction.DoShowEditSubKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.PoinKategoriPencapaian:
                                    var m2 = DAO_Rapor_PoinKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m2 != null) if (m2.Nama != null) s_komponen_rapor = m2.Nama;

                                    txtPoinItemPoinKategoriPencapaian.Text = item_rapor.Poin;
                                    txtPoinKategoriPencapaian.Text = s_komponen_rapor;
                                    txtPoinKategoriPencapaianVal.Value = s_komponen_rapor;
                                    BindListViewDesain(txtID.Value, true);
                                    txtKeyAction.Value = JenisAction.DoShowEditPoinKategoriPencapaian.ToString();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void btnUpdateUrut_Click(object sender, EventArgs e)
        {
            if (txtListIDItemPenilaian.Value.Trim() != "")
            {
                if (txtIDJenisInput.Value == JenisInput.ItemKriteria.ToString())
                {
                    string[] arr_item = txtListIDItemPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int urut = 1;
                    foreach (string item in arr_item)
                    {
                        DAO_Rapor_DesignKriteria.UpdateUrut(item, urut * 100, Libs.LOGGED_USER_M.UserID);
                        urut++;
                    }
                    txtIDItemPenilaian.Value = txtSelIDItemPenilaian.Value;
                    txtSelIDItemPenilaian.Value = "";
                    BindListViewKriteria(txtID.Value, true);
                }
                else if (txtIDJenisInput.Value == JenisInput.ItemReguler.ToString())
                {
                    string[] arr_item = txtListIDItemPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int urut = 1;
                    foreach (string item in arr_item)
                    {
                        DAO_Rapor_DesignDet.UpdateUrut(item, urut * 100, Libs.LOGGED_USER_M.UserID);
                        urut++;
                    }
                    txtIDItemPenilaian.Value = txtSelIDItemPenilaian.Value;
                    txtSelIDItemPenilaian.Value = "";
                    BindListViewDesain(txtID.Value, true);
                }
                else if (txtIDJenisInput.Value == JenisInput.ItemEkskul.ToString())
                {
                    string[] arr_item = txtListIDItemPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int urut = 1;
                    foreach (string item in arr_item)
                    {
                        DAO_Rapor_DesignDetEkskul.UpdateUrut(item, urut * 100, Libs.LOGGED_USER_M.UserID);
                        urut++;
                    }
                    txtIDItemPenilaian.Value = txtSelIDItemPenilaian.Value;
                    txtSelIDItemPenilaian.Value = "";
                    BindListViewEkskulBySiswa(txtID.Value, txtIDSiswa.Value, true);
                }
                txtKeyAction.Value = JenisAction.DoUpdateUrut.ToString();
            }
        }

        protected void lnkAddRekomendasiEkskul_Click(object sender, EventArgs e)
        {
            txtIDItemPenilaian.Value = "";
            if (txtIDJenisInsert.Value.Trim() != "")
            {
                Guid id_guid = Guid.NewGuid();
                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                {
                    Kode = id_guid,
                    Rel_Rapor_Design = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(Constantas.GUID_NOL),
                    JenisKomponen = JenisKomponenRapor.Rekomendasi,
                    Rel_Siswa = txtIDSiswa.Value,
                    Rel_KelasDet = txtKelasDet.Value
                }, Libs.LOGGED_USER_M.UserID);
                if (txtListIDItemPenilaian.Value.Trim() != "")
                {
                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int id = 1;
                    foreach (string kode in arr_kode)
                    {
                        DAO_Rapor_DesignDetEkskul.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                        id++;
                    }
                }
            }
            else
            {
                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                {
                    Kode = Guid.NewGuid(),
                    Rel_Rapor_Design = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(Constantas.GUID_NOL),
                    JenisKomponen = JenisKomponenRapor.Rekomendasi,
                    Rel_Siswa = txtIDSiswa.Value,
                    Rel_KelasDet = txtKelasDet.Value
                }, Libs.LOGGED_USER_M.UserID);
            }
            BindDataDesain();
        }

        protected void ShowKategoriPencapaianEkskul()
        {
            txtIDItemPenilaian.Value = "";
            txtPoinItemKategoriPencapaian.Text = "";
            txtKategoriPencapaian.Text = "";
            txtKategoriPencapaianVal.Value = "";
            txtIDJenisInput.Value = JenisInput.ItemEkskul.ToString();
            txtKeyAction.Value = JenisAction.DoShowInputDesainKategoriPencapaian.ToString();
        }

        protected void lnkAddKategoriPencapaianEkskul_Click(object sender, EventArgs e)
        {
            ShowKategoriPencapaianEkskul();
        }

        protected void ShowSubKategoriPencapaianEKskul()
        {
            txtIDItemPenilaian.Value = "";
            txtPoinItemSubKategoriPencapaian.Text = "";
            txtSubKategoriPencapaian.Text = "";
            txtSubKategoriPencapaianVal.Value = "";
            txtIDJenisInput.Value = JenisInput.ItemEkskul.ToString();
            txtKeyAction.Value = JenisAction.DoShowInputDesainSubKategoriPencapaian.ToString();
        }

        protected void lnkAddSubKategoriPencapaianEkskul_Click(object sender, EventArgs e)
        {
            ShowSubKategoriPencapaianEKskul();
        }

        protected void ShowInputPoinKategoriPencapaianEkskul(bool do_show = true)
        {
            txtIDItemPenilaian.Value = "";
            txtPoinItemPoinKategoriPencapaian.Text = "";
            txtKategoriPencapaianVal.Value = "";
            txtSubKategoriPencapaianVal.Value = "";
            txtPoinKategoriPencapaian.Text = "";
            txtPoinKategoriPencapaianVal.Value = "";
            txtIDJenisInput.Value = JenisInput.ItemEkskul.ToString();
            if (do_show) txtKeyAction.Value = JenisAction.DoShowInputDesainPoinKategoriPencapaian.ToString();
        }

        protected void lnkAddPoinKategoriPencapaianEkskul_Click(object sender, EventArgs e)
        {
            ShowInputPoinKategoriPencapaianEkskul();
        }

        protected void btnShowEditItemKriteria_Click(object sender, EventArgs e)
        {
            Rapor_DesignKriteria m = DAO_Rapor_DesignKriteria.GetByID_Entity(txtIDKriteriaEdit.Value);
            if (m != null)
            {
                if (m.Kode != null)
                {
                    ShowHTMLListKriteria(m.Rel_Rapor_Kriteria.ToString());
                    BindListViewKriteria(txtID.Value, true);
                    txtKeyAction.Value = JenisAction.DoShowKriteriaPencapaian.ToString();
                }
                else
                {
                    ShowHTMLListKriteria();
                    BindListViewKriteria(txtID.Value, true);
                    txtKeyAction.Value = JenisAction.DoShowKriteriaPencapaian.ToString();
                }
            }
            else
            {
                ShowHTMLListKriteria();
                BindListViewKriteria(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoShowKriteriaPencapaian.ToString();
            }
        }

        protected void lvDesain_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
        }

        protected void btnShowKelas_Click(object sender, EventArgs e)
        {
            ShowListSiswa();
            BindDataDesain();
        }

        protected void btnShowDesainEkskulSiswa_Click(object sender, EventArgs e)
        {
            ShowDataSiswa();
            BindDataDesain();
        }

        protected void lnkOKPosting_Click(object sender, EventArgs e)
        {
            try
            {
                if (GetCurrentData(txtID.Value) != null)
                {
                    if (!GetCurrentData(txtID.Value).IsLocked)
                    {
                        DAO_Rapor_Design.UpdatePosting(txtID.Value, true);
                        BindListView(true);
                        txtKeyAction.Value = JenisAction.DoPosting.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        public static bool IsLocked(string kode)
        {
            Rapor_Design m = GetCurrentData(kode);
            if (m != null)
            {
                return m.IsLocked;
            }

            return false;
        }

        public static Rapor_Design GetCurrentData(string kode)
        {
            if (kode.Trim() != "")
            {
                Rapor_Design m = DAO_Rapor_Design.GetByID_Entity(kode);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        protected void btnShowConfirmPosting_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowConfirmPosting.ToString();
        }

        protected void btnShowPengaturanItemPenilaian_Click(object sender, EventArgs e)
        {
            BindListViewDesain(txtID.Value, true);

            chkIsNewPage.Checked = false;
            chkKunciSelainGuruKhusus.Checked = false;
            txtGuru1.Value = "";
            txtGuru2.Value = "";
            txtGuru3.Value = "";

            Rapor_DesignDet m = DAO_Rapor_DesignDet.GetByID_Entity(txtIDItemPenilaian.Value);
            if (m != null)
            {
                if (m.Poin != null)
                {
                    chkIsNewPage.Checked = m.IsNewPage;
                    chkKunciSelainGuruKhusus.Checked = m.IsLockGuruKelas;
                }
            }

            var lst_guru_khusus = DAO_Rapor_DesignDet_GuruKhusus.GetByHeader_Entity(txtIDItemPenilaian.Value);
            if (lst_guru_khusus.Count > 0)
            {
                foreach (var guru_khusus in lst_guru_khusus)
                {
                    switch (guru_khusus.Urut)
                    {
                        case 1:
                            txtGuru1.Value = guru_khusus.Rel_Guru;
                            break;
                        case 2:
                            txtGuru2.Value = guru_khusus.Rel_Guru;
                            break;
                        case 3:
                            txtGuru3.Value = guru_khusus.Rel_Guru;
                            break;
                        default:
                            break;
                    }
                }
            }

            txtKeyAction.Value = JenisAction.DoShowPengaturanItemPenilaian.ToString();
        }

        protected void lnkOKItemPengaturan_Click(object sender, EventArgs e)
        {
            try
            {
                DAO_Rapor_DesignDet_GuruKhusus.DeleteByHeader(txtIDItemPenilaian.Value);
                DAO_Rapor_DesignDet.UpdatePengaturan(txtIDItemPenilaian.Value, chkIsNewPage.Checked, chkKunciSelainGuruKhusus.Checked);

                if (txtGuru1.Value.Trim() != "" && txtGuru1.Text.Trim() != "")
                {
                    DAO_Rapor_DesignDet_GuruKhusus.Insert(new Rapor_DesignDet_GuruKhusus
                    {
                        Kode = Guid.NewGuid(),
                        Rel_Rapor_DesignDet = new Guid(txtIDItemPenilaian.Value),
                        Rel_Guru = txtGuru1.Value,
                        Urut = 1
                    });
                }

                if (txtGuru2.Value.Trim() != "" && txtGuru2.Text.Trim() != "")
                {
                    DAO_Rapor_DesignDet_GuruKhusus.Insert(new Rapor_DesignDet_GuruKhusus
                    {
                        Kode = Guid.NewGuid(),
                        Rel_Rapor_DesignDet = new Guid(txtIDItemPenilaian.Value),
                        Rel_Guru = txtGuru2.Value,
                        Urut = 2
                    });
                }

                if (txtGuru3.Value.Trim() != "" && txtGuru3.Text.Trim() != "")
                {
                    DAO_Rapor_DesignDet_GuruKhusus.Insert(new Rapor_DesignDet_GuruKhusus
                    {
                        Kode = Guid.NewGuid(),
                        Rel_Rapor_DesignDet = new Guid(txtIDItemPenilaian.Value),
                        Rel_Guru = txtGuru3.Value,
                        Urut = 3
                    });
                }

                BindListViewDesain(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void ShowListEkskul()
        {
            string s_html = "";

            var lst_ekskul = DAO_Rapor_DesignEkskul.GetByTABySM_Entity(txtTahunAjaran.Value, txtSemester.Value);
            var lst_ekskul_design = DAO_Rapor_DesignDetEkskul.GetByHeaderBySiswaForDesign_Entity(txtID.Value, txtIDSiswa.Value);
            int id = 1;
            foreach (var item_ekskul in lst_ekskul.FindAll(m => m.Rel_Kelas == GetCurrentData(txtID.Value).Rel_Kelas.ToString()))
            {
                if (lst_ekskul_design.FindAll(m => m.Rel_KomponenRapor == item_ekskul.Kode).Count == 0)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(item_ekskul.Rel_Mapel);
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            s_html += "<div class=\"row " + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                            "<div class=\"col-xs-12\" style=\"width: 100%;\">" +
                                                "<table style=\"margin: 0px; width: 100%;\">" +
                                                    "<tr>" +
                                                        "<td style=\"width: 30px; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                            id.ToString() +
                                                            "." +
                                                        "</td>" +
                                                        "<td style=\"padding: 0px; font-size: small;\">" +
                                                            "<span style=\"color: grey; font-weight: bold;\">" +
                                                                m_mapel.Nama +
                                                            "</span>" +
                                                        "</td>" +
                                                        "<td style=\"width: 50px; text-align: right; vertical-align: middle; padding-right: 0px;\">" +
                                                            "<a id=\"lbl_" + m_mapel.Kode.ToString().Replace("-", "_") + "\" " +
                                                                "onclick=\"" + txtIDMapelEkskul.ClientID + ".value = '" + item_ekskul.Kode.ToString() + "' ;ShowProgress(true); " + btnOKAddMapelEkskul.ClientID + ".click(); \"" +
                                                                "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #1DA1F2; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                                "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                "title=\" Pilih \">" +
                                                                    "<i class=\"fa fa-folder-open\"></i>&nbsp;&nbsp;Pilih" +
                                                            "</a>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</div>" +
                                      "</div>" +
                                      "<div class=\"row " + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                            "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                "<hr style=\"margin: 0px; border-color: #E9EFF5;\" />" +
                                            "</div>" +
                                      "</div>";

                            id++;
                        }
                    }
                }
            }

            if ((id - 1) == 0)
            {
                s_html = "<div class=\"row\">" +
                            "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px; margin-top: 20px;\">" +
                                "<label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label>" +
                            "</div>" +
                         "</div>";
            }

            ltrListEkskul.Text = s_html;
        }

        protected void lnkAddEkskul_Click(object sender, EventArgs e)
        {
            ShowListEkskul();
            BindDataDesain();
            txtKeyAction.Value = JenisAction.DoShowAddEkskul.ToString();
        }

        protected void btnShowDesainEkskul_Click(object sender, EventArgs e)
        {
            txtShowInputPencapaian.Value = "0";
            bool is_locked = IsLocked(txtID.Value);
            lnkAddKriteria.Visible = !is_locked;
            div_hapus_item_rapor.Visible = !is_locked;
            div_hapus_item_rapor.Visible = true; //!is_locked;
            ShowDataSiswa();
            ShowDesain(true);
        }

        protected void btnOKAddMapelEkskul_Click(object sender, EventArgs e)
        {
            txtIDItemPenilaian.Value = "";
            if (txtIDJenisInsert.Value.Trim() != "")
            {
                Guid id_guid = Guid.NewGuid();
                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                {
                    Kode = id_guid,
                    Rel_Rapor_Design = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(txtIDMapelEkskul.Value),
                    JenisKomponen = JenisKomponenRapor.MapelEkskul,
                    Rel_Siswa = txtIDSiswa.Value,
                    Rel_KelasDet = txtKelasDet.Value
                }, Libs.LOGGED_USER_M.UserID);
                if (txtListIDItemPenilaian.Value.Trim() != "")
                {
                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int id = 1;
                    foreach (string kode in arr_kode)
                    {
                        DAO_Rapor_DesignDetEkskul.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                        id++;
                    }
                }
            }
            else
            {
                DAO_Rapor_DesignDetEkskul.Insert(new Rapor_DesignDetEkskul
                {
                    Kode = Guid.NewGuid(),
                    Rel_Rapor_Design = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(txtIDMapelEkskul.Value),
                    JenisKomponen = JenisKomponenRapor.MapelEkskul,
                    Rel_Siswa = txtIDSiswa.Value,
                    Rel_KelasDet = txtKelasDet.Value
                }, Libs.LOGGED_USER_M.UserID);
            }
            txtIDMapelEkskul.Value = "";
            BindDataDesain();
        }

        public static string GetHTMLMapelEkskul(string kode)
        {
            string html = "";

            var m_desain_ekskul = DAO_Rapor_DesignEkskul.GetByID_Entity(kode);
            if (m_desain_ekskul != null)
            {
                if (m_desain_ekskul.TahunAjaran != null)
                {
                    var lst_desain_ekskul = DAO_Rapor_DesignEkskulDet.GetByHeader_Entity(kode);
                    int id = 0;
                    foreach (var item_desain_ekskul in lst_desain_ekskul)
                    {
                        if ((JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) != JenisKomponenRapor.Rekomendasi)
                        {
                            html += "<tr>" +
                                        "<td style=\"width: 30px; padding: 5px; background: " + (id % 2 == 0 ? "rgb(255, 235, 207)" : "transparent") + "; color: grey; padding-top: 10px; padding-bottom: 10px;\">" +
                                            "<label style=\"" +
                                                "text-transform: none; " +
                                                "text-decoration: none; " +
                                                (
                                                (JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) ==
                                                    JenisKomponenRapor.KategoriPencapaian
                                                ? "font-weight: bold; margin-right: 15px;"
                                                : (
                                                    (JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) ==
                                                        JenisKomponenRapor.SubKategoriPencapaian
                                                    ? "font-weight: bold; margin-left: 30px; margin-right: 15px;"
                                                    : (
                                                        (JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) ==
                                                            JenisKomponenRapor.PoinKategoriPencapaian
                                                        ? "font-weight: normal; margin-left: 45px; margin-right: 15px;"
                                                        : ""
                                                        )
                                                    )
                                                ) +
                                            "\">" +
                                                Libs.GetHTMLNoParagraphDiAwal(item_desain_ekskul.Poin) +
                                            "</label>" +
                                        "</td>" +
                                        "<td style=\"padding: 5px; background: " + (id % 2 == 0 ? "rgb(255, 235, 207)" : "transparent") + "; color: grey; padding-top: 10px; padding-bottom: 10px;\">" +
                                            "<label style=\"" +
                                                "text-transform: none; " +
                                                "text-decoration: none; " +
                                                (
                                                (JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) ==
                                                    JenisKomponenRapor.KategoriPencapaian
                                                ? "font-weight: bold; margin-right: 15px;"
                                                : (
                                                    (JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) ==
                                                        JenisKomponenRapor.SubKategoriPencapaian
                                                    ? "font-weight: bold; margin-left: 30px; margin-right: 15px;"
                                                    : (
                                                        (JenisKomponenRapor)Convert.ToInt16(item_desain_ekskul.JenisKomponen) ==
                                                            JenisKomponenRapor.PoinKategoriPencapaian
                                                        ? "font-weight: normal; margin-left: 45px; margin-right: 15px;"
                                                        : ""
                                                        )
                                                    )
                                                ) +
                                            "\">" +
                                                Libs.GetHTMLNoParagraphDiAwal(item_desain_ekskul.NamaKomponen) +
                                            "</label>" +
                                        "</td>" +
                                    "</tr>";
                        }
                        else
                        {
                            html += "<tr>" +
                                        "<td colspan=\"2\" style=\"padding: 10px; background: " + (id % 2 == 0 ? "rgb(255, 235, 207)" : "transparent") + "; color: grey; padding-top: 10px; padding-bottom: 10px;\">" +
                                            "<label style=\"font-weight: bold; margin-bottom: 5px; padding-left: 3px;\">" +
                                                Libs.GetHTMLNoParagraphDiAwal(item_desain_ekskul.NamaKomponen) +
                                            "</label>" +
                                            "<div style=\"background-color: #fffeef; border-style: solid; border-width: 1px; border-color: #E3E3E3; width: 100%; height: 100px;\"></div>" +
                                        "</td>" +
                                    "</tr>";
                        }

                        id++;
                    }
                }
            }

            if (html.Trim() != "")
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                        html +
                       "</table>";
            }

            return html;
        }
    }
}