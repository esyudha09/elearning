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
    public partial class wf_PengaturanEkskul : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PENGATURANEKSKUL_KB";
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
            ShowDataList,
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
            DoUpdateItem,
            DoShowTampilanData,
            DoTampilkanData,
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

            if (!IsPostBack)
            {
                this.Master.ShowHeaderTools = true;
                this.Master.HeaderCardVisible = false;
                InitKeyEventClient();
                ListDropdown();
            }
            BindListView(true, this.Master.txtCariData.Text);
            if (mvMain.ActiveViewIndex == 1) BindListViewDesain(txtID.Value, true);
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

            cboPeriode.Items.Clear();
            foreach (var item in DAO_Rapor_DesignEkskul.GetAll_Entity().Select(
                m => new { m.TahunAjaran, m.Semester }).Distinct().OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).ToList())
            {
                cboPeriode.Items.Add(new ListItem
                {
                    Value = item.TahunAjaran + "|" + item.Semester,
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }

            Sekolah sekolah = DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.KB).FirstOrDefault();
            if (sekolah != null)
            {
                if (sekolah.Nama != null)
                {
                    List<Mapel> lst_mapel =
                        DAO_Mapel.GetAllBySekolah_Entity(sekolah.Kode.ToString()).FindAll(
                                m => m.Jenis == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                     m.Jenis == Application_Libs.Libs.JENIS_MAPEL.EKSKUL
                            ).OrderBy(m => m.Nama).ToList();
                    cboMapel.Items.Clear();
                    cboMapel.Items.Add("");
                    foreach (Mapel m in lst_mapel)
                    {
                        cboMapel.Items.Add(new ListItem
                        {
                            Value = m.Kode.ToString(),
                            Text = m.Nama
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
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboMapel.ClientID + "').focus(); return false; }");
            cboMapel.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            string tahun_ajaran = "";
            string semester = "";

            if (!IsPostBack)
            {
                tahun_ajaran = Libs.GetTahunAjaranByTanggal(DateTime.Now);
                semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            }
            else
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriode.SelectedValue.Substring(0, 9);
                    semester = cboPeriode.SelectedValue.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            txtTahunAjaran.Value = tahun_ajaran;
            txtSemester.Value = semester;

            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectCommand = DAO_Rapor_DesignEkskul.SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectCommand = DAO_Rapor_DesignEkskul.SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewDesain(string rel_desain_rapor, bool isbind = true)
        {
            sql_ds_desain.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_desain.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_desain.SelectParameters.Clear();
            sql_ds_desain.SelectParameters.Add("Rel_Rapor_DesignEkskul", rel_desain_rapor);
            sql_ds_desain.SelectCommand = DAO_Rapor_DesignEkskulDet.SP_SELECT_BY_HEADER;
            if (isbind) lvDesain.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Master.txtCariData.Text = "";
            BindListView(true, "");
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtTahunPelajaran.Text = "";
            cboSemester.SelectedValue = "";
            cboMapel.SelectedValue = "";
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
                    DAO_Rapor_DesignEkskul.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
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
                Rapor_DesignEkskul m = new Rapor_DesignEkskul();
                m.Kode = Guid.NewGuid();
                m.TahunAjaran = txtTahunPelajaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Kelas = cboKelas.SelectedValue;
                m.Rel_Mapel = cboMapel.SelectedValue;
                if (txtID.Value.Trim() != "")
                {
                    if (GetCurrentData(txtID.Value) != null)
                    {
                        m.Kode = new Guid(txtID.Value);
                        DAO_Rapor_DesignEkskul.Update(m, Libs.LOGGED_USER_M.UserID);
                        BindListView(true, this.Master.txtCariData.Text);
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
                else
                {
                    if (DAO_Rapor_DesignEkskul.GetAll_Entity().FindAll(m1 =>
                            m1.TahunAjaran == txtTahunPelajaran.Text &&
                            m1.Semester == cboSemester.SelectedValue &&
                            m1.Rel_Kelas == cboKelas.SelectedValue &&
                            m1.Rel_Mapel == cboMapel.SelectedValue).Count > 0)
                    {
                        txtKeyAction.Value = "Desain rapor tahun pelajaran : " +
                                             txtTahunPelajaran.Text +
                                             ", semester : " +
                                             cboSemester.SelectedValue + " " +
                                             ", kelas : " +
                                             cboSemester.SelectedItem.Text + " " +
                                             ", mata pelajaran : " +
                                             cboMapel.SelectedItem.Text + " " +
                                             "sudah ada";
                        return;
                    }

                    DAO_Rapor_DesignEkskul.Insert(m, Libs.LOGGED_USER_M.UserID);
                    ListDropdown();
                    BindListView(true, this.Master.txtCariData.Text);
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
                Rapor_DesignEkskul m = DAO_Rapor_DesignEkskul.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        cboMapel.SelectedValue = m.Rel_Mapel.ToString();
                        cboKelas.SelectedValue = m.Rel_Kelas.ToString();
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_DesignEkskul m = DAO_Rapor_DesignEkskul.GetByID_Entity(txtID.Value.Trim());
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
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void ShowDesain()
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_DesignEkskul m = DAO_Rapor_DesignEkskul.GetByID_Entity(txtID.Value);
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
                                             "<span style=\"font-weight: bold;\">" + DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString()).Nama + "</span>";

                        this.Session[SessionViewDataName] = 0;
                        this.Master.ShowHeaderTools = false;

                        //show kelas pertama
                        txtKelasDet.Value = "";
                        txtCountSiswa.Value = "";
                        txtIndexSiswa.Value = "";
                        txtTahunAjaran.Value = "";
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                KelasDet kelas_det = DAO_KelasDet.GetByKelas_Entity(m_kelas.Kode.ToString()).
                                                     OrderBy(m0 => m0.UrutanKelas).
                                                     ToList().
                                                     FindAll(m0 => m0.Nama.Trim().ToLower() != m_kelas.Nama.Trim().ToLower()).FirstOrDefault();

                                if (kelas_det != null)
                                {
                                    if (kelas_det.Nama != null)
                                    {

                                        txtKelasDet.Value = kelas_det.Kode.ToString();
                                        txtCountSiswa.Value = GetListSiswa().Count.ToString();
                                        txtIndexSiswa.Value = "0";
                                        txtTahunAjaran.Value = m.TahunAjaran;

                                    }
                                }
                            }
                        }
                        //end show kelas pertama
                        ShowListSiswa();
                        BindDataDesain();

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

            txtCountSiswa.Value = GetListSiswa().Count.ToString();
            txtIndexSiswa.Value = "0";
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
            BindListViewDesain(txtID.Value, true);
            Rapor_DesignEkskul m_rapor = DAO_Rapor_DesignEkskul.GetByID_Entity(txtID.Value);
            if (m_rapor != null)
            {
                if (m_rapor.TahunAjaran != null)
                {
                    ShowListKelas(m_rapor.Rel_Kelas.ToString());
                }
            }
            txtIDJenisInsert.Value = "";
        }

        protected void btnShowDesain_Click(object sender, EventArgs e)
        {
            bool is_locked = false;
            div_hapus_item_rapor.Visible = !is_locked;
            div_button_settings_rapor_design.Visible = !is_locked;
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
                DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                {
                    Kode = id_guid,
                    Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
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
                        DAO_Rapor_DesignEkskulDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                        id++;
                    }
                }
            }
            else
            {
                DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                {
                    Kode = Guid.NewGuid(),
                    Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
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

                if (txtIDItemPenilaian.Value.Trim() != "")
                {
                    DAO_Rapor_DesignEkskulDet.Update(new Rapor_DesignEkskulDet
                    {
                        Kode = new Guid(txtIDItemPenilaian.Value),
                        Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                        Poin = txtPoinItemKategoriPencapaian.Text,
                        Rel_KomponenRapor = kode_kategori_pencapaian,
                        JenisKomponen = JenisKomponenRapor.KategoriPencapaian
                    }, Libs.LOGGED_USER_M.UserID);
                    BindDataDesain();
                    txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                }
                else
                {
                    if (txtIDJenisInsert.Value.Trim() != "")
                    {
                        Guid id_guid = Guid.NewGuid();
                        DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                        {
                            Kode = id_guid,
                            Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
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
                                DAO_Rapor_DesignEkskulDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                id++;
                            }
                        }
                    }
                    else
                    {
                        DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                            Poin = txtPoinItemKategoriPencapaian.Text,
                            Rel_KomponenRapor = kode_kategori_pencapaian,
                            JenisKomponen = JenisKomponenRapor.KategoriPencapaian
                        }, Libs.LOGGED_USER_M.UserID);
                    }

                    BindDataDesain();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
                }
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

                if (txtIDItemPenilaian.Value.Trim() != "")
                {
                    DAO_Rapor_DesignEkskulDet.Update(new Rapor_DesignEkskulDet
                    {
                        Kode = new Guid(txtIDItemPenilaian.Value),
                        Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                        Poin = txtPoinItemSubKategoriPencapaian.Text,
                        Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                        JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian
                    }, Libs.LOGGED_USER_M.UserID);
                    BindDataDesain();
                    txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                }
                else
                {
                    if (txtIDJenisInsert.Value.Trim() != "")
                    {
                        Guid id_guid = Guid.NewGuid();
                        DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                        {
                            Kode = id_guid,
                            Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
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
                                DAO_Rapor_DesignEkskulDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                id++;
                            }
                        }
                    }
                    else
                    {
                        DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                            Poin = txtPoinItemSubKategoriPencapaian.Text,
                            Rel_KomponenRapor = kode_sub_kategori_pencapaian,
                            JenisKomponen = JenisKomponenRapor.SubKategoriPencapaian
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                    BindDataDesain();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
                }
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

                if (txtIDItemPenilaian.Value.Trim() != "")
                {
                    DAO_Rapor_DesignEkskulDet.Update(new Rapor_DesignEkskulDet
                    {
                        Kode = new Guid(txtIDItemPenilaian.Value),
                        Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                        Poin = txtPoinItemPoinKategoriPencapaian.Text,
                        Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                        JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian
                    }, Libs.LOGGED_USER_M.UserID);
                    BindDataDesain();
                    txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                }
                else
                {
                    if (txtIDJenisInsert.Value.Trim() != "")
                    {
                        Guid id_guid = Guid.NewGuid();
                        DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                        {
                            Kode = id_guid,
                            Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
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
                                DAO_Rapor_DesignEkskulDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                                id++;
                            }
                        }
                        BindDataDesain();
                        ShowInputPoinKategoriPencapaian(false);
                        txtKeyAction.Value = JenisAction.DoUpdateItem.ToString();
                    }
                    else
                    {
                        DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                            Poin = txtPoinItemPoinKategoriPencapaian.Text,
                            Rel_KomponenRapor = kode_poin_kategori_pencapaian,
                            JenisKomponen = JenisKomponenRapor.PoinKategoriPencapaian
                        }, Libs.LOGGED_USER_M.UserID);
                        BindDataDesain();
                        ShowInputPoinKategoriPencapaian(false);
                        txtIDJenisInput.Value = JenisInput.ItemReguler.ToString();
                        txtKeyAction.Value = JenisAction.DoUpdatePoinPenilaian.ToString();
                    }
                }
            }
        }

        protected void lnkOKKriteriaPencapaian_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
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

        protected void lnkOKHapusItemPenilaian_Click(object sender, EventArgs e)
        {
            if (txtIDItemPenilaian.Value.Trim() != "")
            {
                if (txtID.Value.Trim() != "" && GetCurrentData(txtID.Value) != null)
                {
                    string[] arr_id = txtIDItemPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in arr_id)
                    {
                        DAO_Rapor_DesignEkskulDet.Delete(id, Libs.LOGGED_USER_M.UserID);
                    }
                    txtIDItemPenilaian.Value = "";
                    BindDataDesain();
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
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
                    Rapor_DesignEkskulDet item_rapor = DAO_Rapor_DesignEkskulDet.GetByID_Entity(txtIDItemPenilaian.Value);
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
                                    txtKeyAction.Value = JenisAction.DoShowEditKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.SubKategoriPencapaian:
                                    var m1 = DAO_Rapor_SubKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m1 != null) if (m1.Nama != null) s_komponen_rapor = m1.Nama;

                                    txtPoinItemSubKategoriPencapaian.Text = item_rapor.Poin;
                                    txtSubKategoriPencapaian.Text = s_komponen_rapor;
                                    txtSubKategoriPencapaianVal.Value = s_komponen_rapor;
                                    txtKeyAction.Value = JenisAction.DoShowEditSubKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.PoinKategoriPencapaian:
                                    var m2 = DAO_Rapor_PoinKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m2 != null) if (m2.Nama != null) s_komponen_rapor = m2.Nama;

                                    txtPoinItemPoinKategoriPencapaian.Text = item_rapor.Poin;
                                    txtPoinKategoriPencapaian.Text = s_komponen_rapor;
                                    txtPoinKategoriPencapaianVal.Value = s_komponen_rapor;
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
                    Rapor_DesignEkskulDet item_rapor = DAO_Rapor_DesignEkskulDet.GetByID_Entity(txtIDItemPenilaian.Value);
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
                                    txtKeyAction.Value = JenisAction.DoShowEditKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.SubKategoriPencapaian:
                                    var m1 = DAO_Rapor_SubKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m1 != null) if (m1.Nama != null) s_komponen_rapor = m1.Nama;

                                    txtPoinItemSubKategoriPencapaian.Text = item_rapor.Poin;
                                    txtSubKategoriPencapaian.Text = s_komponen_rapor;
                                    txtSubKategoriPencapaianVal.Value = s_komponen_rapor;
                                    txtKeyAction.Value = JenisAction.DoShowEditSubKategoriPencapaian.ToString();
                                    break;
                                case JenisKomponenRapor.PoinKategoriPencapaian:
                                    var m2 = DAO_Rapor_PoinKategoriPencapaian.GetByID_Entity(item_rapor.Rel_KomponenRapor.ToString());
                                    s_komponen_rapor = "";
                                    if (m2 != null) if (m2.Nama != null) s_komponen_rapor = m2.Nama;

                                    txtPoinItemPoinKategoriPencapaian.Text = item_rapor.Poin;
                                    txtPoinKategoriPencapaian.Text = s_komponen_rapor;
                                    txtPoinKategoriPencapaianVal.Value = s_komponen_rapor;
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
                if (txtIDJenisInput.Value == JenisInput.ItemReguler.ToString() ||
                    txtIDJenisInput.Value == JenisInput.ItemEkskul.ToString())
                {
                    string[] arr_item = txtListIDItemPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int urut = 1;
                    foreach (string item in arr_item)
                    {
                        DAO_Rapor_DesignEkskulDet.UpdateUrut(item, urut * 100, Libs.LOGGED_USER_M.UserID);
                        urut++;
                    }
                }
                txtSelIDItemPenilaian.Value = "";
                BindDataDesain();
                txtKeyAction.Value = JenisAction.DoUpdateUrut.ToString();
            }
        }

        protected void lnkAddRekomendasiEkskul_Click(object sender, EventArgs e)
        {
            txtIDItemPenilaian.Value = "";
            if (txtIDJenisInsert.Value.Trim() != "")
            {
                Guid id_guid = Guid.NewGuid();
                DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                {
                    Kode = id_guid,
                    Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(Constantas.GUID_NOL),
                    JenisKomponen = JenisKomponenRapor.Rekomendasi,
                }, Libs.LOGGED_USER_M.UserID);
                if (txtListIDItemPenilaian.Value.Trim() != "")
                {
                    string[] arr_kode = txtListIDItemPenilaian.Value.Replace(C_ID, id_guid.ToString()).
                        Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    int id = 1;
                    foreach (string kode in arr_kode)
                    {
                        DAO_Rapor_DesignEkskulDet.UpdateUrut(kode, id * 100, Libs.LOGGED_USER_M.UserID);
                        id++;
                    }
                }
            }
            else
            {
                DAO_Rapor_DesignEkskulDet.Insert(new Rapor_DesignEkskulDet
                {
                    Kode = Guid.NewGuid(),
                    Rel_Rapor_DesignEkskul = new Guid(txtID.Value),
                    Poin = "",
                    Rel_KomponenRapor = new Guid(Constantas.GUID_NOL),
                    JenisKomponen = JenisKomponenRapor.Rekomendasi,
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
                    txtKeyAction.Value = JenisAction.DoShowKriteriaPencapaian.ToString();
                }
                else
                {
                    ShowHTMLListKriteria();
                    txtKeyAction.Value = JenisAction.DoShowKriteriaPencapaian.ToString();
                }
            }
            else
            {
                ShowHTMLListKriteria();
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
            BindDataDesain();
        }

        public static Rapor_DesignEkskul GetCurrentData(string kode)
        {
            if (kode.Trim() != "")
            {
                Rapor_DesignEkskul m = DAO_Rapor_DesignEkskul.GetByID_Entity(kode);
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

        protected void lnkOKPeriode_Click(object sender, EventArgs e)
        {
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void btnDoShowTampilan_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowTampilanData.ToString();
        }
    }
}