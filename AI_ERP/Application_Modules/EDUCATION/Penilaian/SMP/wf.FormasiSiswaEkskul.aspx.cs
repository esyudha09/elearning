using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_FormasiSiswaEkskul : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAFORMASISISWASMPEKSKUL";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            ShowDataList,
            ShowDataFormasi,
            ShowDataFormasiGuruPembimbing,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowInputFormasiEkskul,
            DoShowInputFormasiGuruPembimbing,
            DoShowConfirmHapus,
            DoShowConfirmHapusMengajar,
            DoShowTampilanData,
            DoTampilkanData,
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
            public static string GetSemester()
            {
                return Libs.GetQueryString("s");
            }

            public static string GetMapel()
            {
                return Libs.GetQueryString("m");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitKeyEventClient();
                ListDropdown();
                InitMapelUnit();
                //txtSiswaEkskul.KodeUnit = GetUnitSekolah().Kode.ToString();
                //txtSiswaEkskul.TahunAjaran = GetTahunAjaran();
                //txtSiswaEkskul.Semester = GetSemester();
                txtSiswa.KodeUnit = GetUnitSekolah().Kode.ToString();
                txtSiswa.TahunAjaran = GetTahunAjaran();
                txtSiswa.Semester = GetSemester();

                txtGuruPembimbing.NamaControl.ValidationGroup = "vldNamaGuruPembimbing";
            }

            if (!(IsPostBack || this.Session[SessionViewDataName] == null))
            {
                dpData.SetPageProperties((int)this.Session[SessionViewDataName], dpData.MaximumRows, true);
            }

            BindListView(true, this.Master.txtCariData.Text);
            if (mvMain.ActiveViewIndex != 0) this.Master.ShowHeaderTools = false;
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void InitMapelUnit()
        {
            cboMapel.Items.Clear();
            cboMapel.Items.Add("");

            List<Mapel> lst_mapel = DAO_Mapel.GetAll_Entity().FindAll(
                    m => m.Rel_Sekolah.ToUpper() == GetUnitSekolah().Kode.ToString().ToUpper() &&
                         (
                            m.Jenis == Application_Libs.Libs.JENIS_MAPEL.EKSKUL ||
                            m.Jenis == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                         )
                );
            foreach (Mapel m in lst_mapel)
            {
                cboMapel.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            ListDropdownStrukturNilai();
        }

        protected Sekolah GetUnitSekolah()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
        }

        protected void ListDropdown()
        {
            cboPeriode.Items.Clear();
            foreach (var item in DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity())
            {
                cboPeriode.Items.Add(new ListItem
                {
                    Value = item.TahunAjaran + "|" + item.Semester,
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onchange", sKeyEnter + "document.getElementById('" + cboMapel.ClientID + "').focus(); return false; }");
            //cboMapel.Attributes.Add("onchange", sKeyEnter + "document.getElementById('" + txtNamaGuru.ClientID + "').focus(); return false; }");
            //txtNamaGuru.Attributes.Add("onkeydown", sKeyEnter + "return false; }");
            cboMapel.Attributes.Add("onchange", sKeyEnter + "document.getElementById('" + txtGuru.NamaClientID + "').focus(); return false; }");
            txtGuru.NamaControl.Attributes.Add("onkeydown", sKeyEnter + "return false; }");

            txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKFormasiSiswa.ClientID + "').click(); return false; }");
        }

        protected string GetTahunAjaran()
        {
            string tahun_ajaran = "";

            if (!IsPostBack)
            {
                tahun_ajaran = Libs.GetTahunAjaranByTanggal(DateTime.Now);
            }
            else
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriode.SelectedValue.Substring(0, 9);
                }
            }

            return tahun_ajaran;
        }

        public static string GetKelasEkskul(string rel_struktur_penilaian)
        {
            Rapor_StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(
                    rel_struktur_penilaian
                );
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    return m.Rel_Kelas.ToString() + ";" +
                           m.Rel_Kelas2.ToString() + ";" +
                           m.Rel_Kelas3.ToString() + ";";
                }
            }
            return "";
        }

        public static string GetGuruEkskul(string kode)
        {
            FormasiEkskulGuru m = DAO_FormasiEkskulGuru.GetByHeader_Entity(
                    kode
                ).FirstOrDefault();
            if (m != null)
            {
                if (m.Rel_Guru != null)
                {
                    return m.Rel_Guru;
                }
            }
            return "";
        }

        protected string GetSemester()
        {
            string semester = "";

            if (!IsPostBack)
            {
                semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            }
            else
            {
                if (cboPeriode.SelectedValue.Trim() != "")
                {
                    semester = cboPeriode.SelectedValue.Substring(cboPeriode.SelectedValue.Length - 1, 1);
                }
            }

            return semester;
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = GetTahunAjaran();
            string semester = GetSemester();

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_FormasiEkskul.SP_SELECT_ALL_BY_PERIODE_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectCommand = DAO_FormasiEkskul.SP_SELECT_ALL_BY_PERIODE;
            }
            if (isbind) lvData.DataBind();
        }

        private void BindListViewFormasiEkskul(string rel_formasiekskul, bool isbind = true)
        {
            sql_ds_formasi_siswa.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_formasi_siswa.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_formasi_siswa.SelectParameters.Clear();
            sql_ds_formasi_siswa.SelectParameters.Add("Rel_FormasiEkskul", rel_formasiekskul);
            sql_ds_formasi_siswa.SelectCommand = DAO_FormasiEkskulDet.SP_SELECT_BY_HEADER;
            if (isbind) lvListSiswaEkskul.DataBind();
        }

        private void BindListViewFormasiGuruPembimbing(string rel_formasiekskul, bool isbind = true)
        {
            sql_ds_guru.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds_guru.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_guru.SelectParameters.Clear();
            sql_ds_guru.SelectParameters.Add("Rel_FormasiEkskul", rel_formasiekskul);
            sql_ds_guru.SelectCommand = DAO_FormasiEkskulGuru.SP_SELECT_BY_HEADER;
            if (isbind) lvListGuruPembina.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_sekolah = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_sekolah");
            System.Web.UI.WebControls.Literal imgh_mapel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_mapel");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");

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
            imgh_kelas.Text = html_image;
            imgh_mapel.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_sekolah.Visible = false;
            imgh_kelas.Visible = false;
            imgh_mapel.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Sekolah":
                    imgh_sekolah.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "Mapel":
                    imgh_mapel.Visible = true;
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
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            this.Master.txtCariData.Text = "";
            BindListView(true, "");
        }

        protected void InitFields()
        {
            txtID.Value = "";
            InitMapelUnit();
            //txtNamaGuru.Text = "";
            txtGuru.Value = "";

            ListDropdownStrukturNilai();
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
                    DAO_FormasiEkskul.Delete(txtID.Value);
                    BindListView(true, this.Master.txtCariData.Text);
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
                if (cboMapelStrukturNilai.SelectedValue.Trim() != "")
                {
                    Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(
                            cboMapelStrukturNilai.SelectedValue
                        );
                    if (m_sn != null)
                    {
                        if (m_sn.TahunAjaran != null)
                        {
                            FormasiEkskul m = new FormasiEkskul();
                            m.Rel_Mapel = m_sn.Rel_Mapel.ToString();
                            m.TahunAjaran = m_sn.TahunAjaran;
                            m.Semester = m_sn.Semester;
                            m.Rel_Rapor_StrukturNilai = m_sn.Kode.ToString();
                            m.Rel_Guru = "";
                            if (txtID.Value.Trim() != "")
                            {
                                m.Kode = new Guid(txtID.Value);
                                DAO_FormasiEkskul.Update(m);
                                BindListView(true, this.Master.txtCariData.Text);
                                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                            }
                            else
                            {
                                DAO_FormasiEkskul.Insert(m);
                                BindListView(true, this.Master.txtCariData.Text);
                                InitFields();
                                txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                            }
                        }
                    }
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
                FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        //txtNamaGuru.Text = m.Rel_Guru;
                        txtGuru.Value = m.Rel_Guru;
                        cboMapel.SelectedValue = m.Rel_Mapel;
                        txtTahunPelajaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        BindListView(true, this.Master.txtCariData.Text);
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            BindListView(true, this.Master.txtCariData.Text);
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        string mapel = "";
                        string guru = "";

                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                mapel = m_mapel.Nama;
                            }
                        }

                        Pegawai m_guru = DAO_Pegawai.GetByID_Entity(m.Rel_Guru);
                        if (m_guru != null)
                        {
                            if (m_guru.Nama != null)
                            {
                                guru = m_guru.Nama;
                            }
                        }

                        ltrMsgConfirmHapus.Text = "Hapus<br />" +
                                                  (
                                                    mapel.Trim() != ""
                                                    ? "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                      "<span style=\"font-weight: bold;\">" +
                                                        mapel +
                                                      "</span><br />"
                                                    : ""
                                                  ) +
                                                  (
                                                    guru.Trim() != ""
                                                    ? "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                      "<span style=\"font-weight: bold;\">" +
                                                        guru +
                                                      "</span><br />"
                                                    : ""
                                                  ) +
                                                    "<i class=\"fa fa-hashtag\"></i>&nbsp;" +
                                                    "<span style=\"font-weight: bold;\">" +
                                                        Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                        " Semester " +
                                                        m.Semester +
                                                    "</span>";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnShowDataFormasi_Click(object sender, EventArgs e)
        {
            ltrCaptionFormasi.Text = "";
            FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                    Pegawai m_guru = DAO_Pegawai.GetByID_Entity(m.Rel_Guru.ToString());
                    string s_guru = "";
                    if (m_guru != null)
                    {
                        if (m_guru.Nama != null)
                        {
                            s_guru = m_guru.Nama;
                        }
                    }
                    if (s_guru.Trim() == "")
                    {
                        s_guru = m.Rel_Guru;
                    }

                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            ltrCaptionFormasi.Text =
                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m.TahunAjaran +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "Sm." + m.Semester +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             DAO_Rapor_StrukturNilai.GetNamaKelasEkskul(m.Rel_Rapor_StrukturNilai) +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m_mapel.Nama +
                                             "&nbsp;" +
                                             "</span>";

                            BindListViewFormasiEkskul(txtID.Value, true);
                            mvMain.ActiveViewIndex = 1;
                            this.Master.ShowHeaderTools = false;
                            txtKeyAction.Value = JenisAction.ShowDataFormasi.ToString();
                        }
                    }
                }
            }
        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, this.Master.txtCariData.Text);
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void lnkOKFormasiSiswa_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskulDet m = new FormasiEkskulDet();
                m.Kode = Guid.NewGuid();
                m.Rel_FormasiEkskul = txtID.Value;
                //m.Rel_Siswa = txtSiswaEkskul.Value;
                m.Rel_Siswa = txtSiswa.Value;
                m.Keterangan = txtKeterangan.Text;
                if (txtIDItem.Value.Trim() == "")
                {
                    DAO_FormasiEkskulDet.Insert(m);
                }
                else
                {
                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_FormasiEkskulDet.Update(m);
                }
            }
            BindListViewFormasiEkskul(txtID.Value, true);
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void InitInputFormasiEkskul()
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        //txtSiswaEkskul.Value = "";
                        txtSiswa.Value = "";
                        txtGuruPembimbing.Value = "";
                    }
                }
            }
        }

        protected void btnShowEditFormasi_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                FormasiEkskulDet m = DAO_FormasiEkskulDet.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Keterangan != null)
                    {
                        InitInputFormasiEkskul();
                        //txtSiswaEkskul.Value = m.Rel_Siswa;
                        txtSiswa.Value = m.Rel_Siswa;
                        txtKeterangan.Text = m.Keterangan;
                        BindListViewFormasiEkskul(txtID.Value, true);
                        txtKeyAction.Value = JenisAction.DoShowInputFormasiEkskul.ToString();
                    }
                }
            }
        }

        protected void btnDoShowTampilan_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowTampilanData.ToString();
        }

        protected void lnkOKTampilanData_Click(object sender, EventArgs e)
        {
            this.Session[SessionViewDataName] = 0;
            dpData.SetPageProperties(0, dpData.MaximumRows, true);
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoTampilkanData.ToString();
        }

        protected void btnShowInputSiswaEkskul_Click(object sender, EventArgs e)
        {
            //txtSiswaEkskul.Value = "";
            txtSiswa.Value = "";
            txtIDItem.Value = "";
            txtKeterangan.Text = "";
            InitInputFormasiEkskul();
            txtKeyAction.Value = JenisAction.DoShowInputFormasiEkskul.ToString();
        }

        protected void lvListSiswaEkskul_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvListSiswaEkskul_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void ListDropdownStrukturNilai()
        {
            cboMapelStrukturNilai.Items.Clear();
            cboMapelStrukturNilai.Items.Add("");
            List<Rapor_StrukturNilai> lst = DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(
                    GetTahunAjaran(), GetSemester()
                );
            foreach (var item in lst)
            {
                string kelas_1 = "";
                string kelas_2 = "";
                string kelas_3 = "";

                if (item.Rel_Kelas.ToString() != Constantas.GUID_NOL && item.Rel_Kelas.ToString() != "")
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(item.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            kelas_1 = m_kelas.Nama;
                        }
                    }
                }

                if (item.Rel_Kelas2.ToString() != Constantas.GUID_NOL && item.Rel_Kelas.ToString() != "")
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(item.Rel_Kelas2.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            kelas_2 = m_kelas.Nama;
                        }
                    }
                }

                if (item.Rel_Kelas3.ToString() != Constantas.GUID_NOL && item.Rel_Kelas.ToString() != "")
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(item.Rel_Kelas3.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            kelas_3 = m_kelas.Nama;
                        }
                    }
                }

                Mapel m_mapel = DAO_Mapel.GetByID_Entity(item.Rel_Mapel.ToString());
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        if (DAO_Mapel.GetJenisMapelByJenis(m_mapel.Jenis) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                        {
                            cboMapelStrukturNilai.Items.Add(
                                new ListItem
                                {
                                    Value = item.Kode.ToString(),
                                    Text = m_mapel.Nama + HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                           (
                                                kelas_1 +
                                                (
                                                    kelas_1.Trim() != "" &&
                                                    kelas_2.Trim() != ""
                                                    ? ", "
                                                    : ""
                                                ) +
                                                kelas_2 +
                                                (
                                                    (
                                                        kelas_2.Trim() != "" || kelas_1.Trim() != ""
                                                    ) && kelas_3.ToString().Trim() != ""
                                                    ? ", "
                                                    : ""
                                                ) +
                                                kelas_3
                                           )
                                }
                            );
                        }
                    }
                }
            }
        }
        
        protected void lnkOKHapusItemFormasi_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_FormasiEkskulDet.Delete(item);
                }
                txtSelItem.Value = "";
                BindListViewFormasiEkskul(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void lvListGuruPembina_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvListGuruPembina_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void lnkOKFormasiGuruPembimbing_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                FormasiEkskulGuru m = new FormasiEkskulGuru();
                m.Kode = Guid.NewGuid();
                m.Rel_FormasiEkskul = txtID.Value;
                m.Rel_Guru = txtGuruPembimbing.Value;
                if (txtIDItem.Value.Trim() == "")
                {
                    DAO_FormasiEkskulGuru.Insert(m);
                }
                else
                {
                    m.Kode = new Guid(txtIDItem.Value);
                    DAO_FormasiEkskulGuru.Update(m);
                }
            }
            BindListViewFormasiGuruPembimbing(txtID.Value, true);
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void btnShowDataFormasiGuruPembina_Click(object sender, EventArgs e)
        {
            ltrCaptionFormasiGuru.Text = "";
            FormasiEkskul m = DAO_FormasiEkskul.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel);
                    Pegawai m_guru = DAO_Pegawai.GetByID_Entity(m.Rel_Guru.ToString());
                    string s_guru = "";
                    if (m_guru != null)
                    {
                        if (m_guru.Nama != null)
                        {
                            s_guru = m_guru.Nama;
                        }
                    }
                    if (s_guru.Trim() == "")
                    {
                        s_guru = m.Rel_Guru;
                    }

                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            ltrCaptionFormasiGuru.Text =
                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m.TahunAjaran +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "Sm." + m.Semester +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             DAO_Rapor_StrukturNilai.GetNamaKelasEkskul(m.Rel_Rapor_StrukturNilai) +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m_mapel.Nama +
                                             "&nbsp;" +
                                             "</span>";

                            BindListViewFormasiGuruPembimbing(txtID.Value, true);
                            mvMain.ActiveViewIndex = 2;
                            this.Master.ShowHeaderTools = false;
                            txtKeyAction.Value = JenisAction.ShowDataFormasiGuruPembimbing.ToString();
                        }
                    }
                }
            }
        }

        protected void btnShowInputGuruPembimbing_Click(object sender, EventArgs e)
        {
            txtGuruPembimbing.Value = "";
            txtIDItem.Value = "";
            InitInputFormasiEkskul();
            txtKeyAction.Value = JenisAction.DoShowInputFormasiGuruPembimbing.ToString();
        }

        protected void btnShowEditGuruPembimbing_Click(object sender, EventArgs e)
        {
            if (txtIDItem.Value.Trim() != "")
            {
                FormasiEkskulGuru m = DAO_FormasiEkskulGuru.GetByID_Entity(txtIDItem.Value);
                if (m != null)
                {
                    if (m.Rel_Guru != null)
                    {
                        InitInputFormasiEkskul();
                        txtGuruPembimbing.Value = m.Rel_Guru;
                        BindListViewFormasiGuruPembimbing(txtID.Value, true);
                        txtKeyAction.Value = JenisAction.DoShowInputFormasiGuruPembimbing.ToString();
                    }
                }
            }
        }

        protected void lnkOKHapusItemGuruPembimbing_Click(object sender, EventArgs e)
        {
            if (txtSelItem.Value.Trim() != "")
            {
                string[] arr_item = txtSelItem.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_item)
                {
                    DAO_FormasiEkskulGuru.Delete(item);
                }
                txtSelItem.Value = "";
                BindListViewFormasiGuruPembimbing(txtID.Value, true);
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }
    }
}