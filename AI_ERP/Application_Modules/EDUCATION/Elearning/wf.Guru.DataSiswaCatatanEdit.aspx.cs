using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Guru_DataSiswaCatatanEdit : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATACATATANSISWA";
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
            DoShowConfirmHapus
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Catatan Saya terhadap Siswa";

            this.Master.ShowHeaderSubTitle = false;
            this.Master.ShowSubHeaderGuru = true;
            this.Master.SelectMenuGuru_Siswa();
            this.Master.HeaderCardVisible = false;
            InitURLOnMenu();

            if (!IsPostBack)
            {
                ListKategoriCatatan();
                InitDropdownList();
                InitKeyEventClient();                
                txtIDGuru.Value = Libs.LOGGED_USER_M.NoInduk;
            }
            ParseDropdownKelasTahunAjaran(cboKelasTahunAjaranSemester.SelectedValue);
            if (!IsPostBack)
            {
                ListSiswa();
            }
            BindListView(!IsPostBack, Libs.GetQ());
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        protected void ListKategoriCatatan()
        {
            cboKategoriCatatan.Items.Clear();
            cboKategoriCatatan.Items.Add("");
            cboKategoriCatatan.Items.Add(new ListItem { Value = KategoriCatatanSiswa.Prestasi.Kode, Text = KategoriCatatanSiswa.Prestasi.Nama });
            cboKategoriCatatan.Items.Add(new ListItem { Value = KategoriCatatanSiswa.Pelanggaran.Kode, Text = KategoriCatatanSiswa.Pelanggaran.Nama });
            cboKategoriCatatan.Items.Add(new ListItem { Value = KategoriCatatanSiswa.Lainnya.Kode, Text = KategoriCatatanSiswa.Lainnya.Nama });
        }

        protected void ParseDropdownKelasTahunAjaran(string value)
        {
            txtKelas.Value = "";
            txtTahunAjaran.Value = "";
            txtSemester.Value = "";

            string[] arr_str = value.Split(new string[] { "|" }, StringSplitOptions.None);
            if (arr_str.Length == 3)
            {
                txtKelas.Value = arr_str[0];
                txtTahunAjaran.Value = arr_str[1];
                txtSemester.Value = arr_str[2];
            }

            txtKelas.Value = QS.GetKelas();
            txtTahunAjaran.Value = QS.GetTahunAjaran();            
        }

        protected void InitDropdownList()
        {
            cboKelasTahunAjaranSemester.Items.Clear();
            List<CatatanSiswaKelasPeriode> lst_kelas_periode = DAO_CatatanSiswa.GetKelasPeriode_Entity(Libs.LOGGED_USER_M.NoInduk);
            foreach (CatatanSiswaKelasPeriode m in lst_kelas_periode)
            {
                cboKelasTahunAjaranSemester.Items.Add(new ListItem {
                    Value = m.Kode + "|" + m.TahunAjaran + "|" + m.Semester,
                    Text = m.Kelas + " " + m.TahunAjaran + " sm. " + m.Semester
                });
            }
        }

        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                        return m_kelas.Rel_Sekolah.ToString();
                    }
                }

                return "";
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetMapel()
            {
                string m = Libs.GetQueryString("m");
                return RandomLibs.GetParseTahunAjaran(m);
            }
        }

        protected void InitURLOnMenu()
        {
            string ft = Libs.Decryptdata(Libs.GetQueryString("ft"));
            string url_penilaian = Libs.GetURLPenilaian(Libs.GetQueryString("kd"));
            string m = Libs.GetQueryString("m");

            string[] arr_bg =
                { "#FD6933", "#0AC6AE", "#F7921E", "#4AA4A4", "#43B8C9", "#95D1C5", "#019ADD", "#31384B", "#18AEC7", "#5299CF", "#2D2C28", "#D5C5C6", "#262726", "#01ACAC", "#322D3A", "#3B4F5D", "#009E00", "#E90080", "#549092", "#00A9A9", "#9B993A" };
            string[] arr_bg_image =
                { "a.png", "b.png", "c.png", "d.png", "e.png", "f.png", "g.png", "h.png", "i.png", "j.png", "k.png", "l.png", "m.png", "n.png", "o.png", "p.png", "q.png", "r.png", "u.png", "s.png", "t.png" };
            string bg_image = "";

            if (ft.Trim() != "")
            {
                int _id = 0;
                for (int i = 0; i < arr_bg.Length; i++)
                {
                    if (arr_bg_image[i] == ft)
                    {
                        _id = i;
                        bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[i]);
                        break;
                    }
                }
                ltrBGHeader.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat;";
            }

            if (m.Trim() != "")
            {
                bool ada = false;
                Mapel mapel = DAO_Mapel.GetByID_Entity(m);
                if (mapel != null)
                {
                    if (mapel.Nama != null)
                    {
                        if (mapel.Jenis.Trim().ToLower() == Libs.JENIS_MAPEL.KHUSUS.Trim().ToLower())
                        {
                            this.Master.SetURLGuru_TimeLine(
                                ""
                            );
                        }
                        else
                        {
                            this.Master.SetURLGuru_TimeLine(
                                ResolveUrl(
                                        Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                                        "?t=" + Libs.GetQueryString("t") +
                                        "&ft=" + Libs.GetQueryString("ft") +
                                        "&kd=" + Libs.GetQueryString("kd") +
                                        (
                                            Libs.GetQueryString("m").Trim() != ""
                                            ? "&m=" + Libs.GetQueryString("m")
                                            : ""
                                        )
                                    )
                            );
                        }
                    }
                }
                if (!ada)
                {
                    this.Master.SetURLGuru_TimeLine(
                            ResolveUrl(
                                    Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                                    "?t=" + Libs.GetQueryString("t") +
                                    "&ft=" + Libs.GetQueryString("ft") +
                                    "&kd=" + Libs.GetQueryString("kd") +
                                    (
                                        Libs.GetQueryString("m").Trim() != ""
                                        ? "&m=" + Libs.GetQueryString("m")
                                        : ""
                                    )
                                )
                        );
                }
            }
            else
            {
                this.Master.SetURLGuru_TimeLine(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : ""
                            )
                        )
                );
            }

            this.Master.SetURLGuru_Siswa(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : ""
                            )
                        )
                );
            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            url_penilaian +
                            "?t=" + Libs.GetQueryString("t") +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : ""
                            )
                        )
                );
            lnkDataSiswa.NavigateUrl =
                ResolveUrl(
                    Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                    "?t=" + Libs.GetQueryString("t") +
                    "&ft=" + Libs.GetQueryString("ft") +
                    "&kd=" + Libs.GetQueryString("kd") +
                    (
                        Libs.GetQueryString("m").Trim() != ""
                        ? "&m=" + Libs.GetQueryString("m")
                        : ""
                    )
                );

            string s_html = "";
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(QS.GetKelas());

            string s_mapel = "";
            if (QS.GetMapel().Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());

                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_mapel = m_mapel.Nama;
                    }
                }
            }

            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    s_html = "<label style=\"margin-left: 45px; color: white; font-weight: bold;\">" +
                                "Catatan Siswa" +
                             "</label>" +
                             "<br />" +
                             "<label style=\"margin-left: 45px; color: white;\">" +
                                "Kelas" +
                                "&nbsp;" +
                                "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                "&nbsp;" +
                                tahun_ajaran +
                             "</label>";
                }
            }

            ltrCaption.Text = s_html;
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtCari.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKCari.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("Rel_Guru", txtIDGuru.Value);
                sql_ds.SelectParameters.Add("Rel_KelasDet", txtKelas.Value);
                sql_ds.SelectCommand = DAO_CatatanSiswa.SP_SELECT_BY_GURU_BY_KELAS_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("Rel_Guru", txtIDGuru.Value);
                sql_ds.SelectParameters.Add("Rel_KelasDet", txtKelas.Value);
                sql_ds.SelectCommand = DAO_CatatanSiswa.SP_SELECT_BY_GURU_BY_KELAS;
            }
            if (isbind) lvData.DataBind();
        }

        protected void ListSiswa()
        {
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(txtKelas.Value);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                            if (m_sekolah != null)
                            {
                                if (m_sekolah.Nama != null)
                                {

                                    cboSiswa.Items.Clear();
                                    List<Siswa> lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                            m_sekolah.Kode.ToString(),
                                            txtKelas.Value,
                                            txtTahunAjaran.Value,
                                            txtSemester.Value
                                        );
                                    cboSiswa.Items.Add("");
                                    foreach (Siswa m in lst_siswa)
                                    {
                                        cboSiswa.Items.Add(new ListItem
                                        {
                                            Value = m.Kode.ToString(),
                                            Text = m.Nama.Trim().ToUpper()
                                        });
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    Libs.FILE_PAGE_URL +
                    "?t=" + Libs.GetQueryString("t") +
                    "&ft=" + Libs.GetQueryString("ft") +
                    "&kd=" + Libs.GetQueryString("kd") +
                    (
                        Libs.GetQueryString("m").Trim() != ""
                        ? "&m=" + Libs.GetQueryString("m")
                        : ""
                    )
                );
        }

        protected void InitFields()
        {
            txtID.Value = "";
            txtIDGuru.Value = "";
            cboKategoriCatatan.SelectedValue = "";
            cboSiswa.SelectedValue = "";
            txtTanggalCatatan.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            txtCatatan.Text = "";
            txtCatatanVal.Value = "";
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
                    DAO_CatatanSiswa.Delete(txtID.Value, txtIDGuru.Value, Libs.LOGGED_USER_M.UserID);
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
                if (txtKelas.Value.Trim() != "")
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(txtKelas.Value);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                            if (m_kelas != null)
                            {
                                if (m_kelas.Nama != null)
                                {

                                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(
                                            m_kelas.Rel_Sekolah.ToString()
                                        );
                                    if (m_sekolah != null)
                                    {
                                        if (m_sekolah.Nama != null)
                                        {

                                            if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.KB)
                                            {
                                                Application_Entities.Elearning.KB.CatatanSiswa m =
                                                new Application_Entities.Elearning.KB.CatatanSiswa
                                                {
                                                    Rel_Siswa = cboSiswa.SelectedValue,
                                                    Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                                    Catatan = txtCatatanVal.Value,
                                                    TahunAjaran = QS.GetTahunAjaran(),
                                                    Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                                    Rel_KelasDet = QS.GetKelas(),
                                                    Rel_Mapel = QS.GetMapel(),
                                                    Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                                                };
                                                m.Kode = Guid.NewGuid();
                                                if (txtID.Value.Trim() != "")
                                                {
                                                    m.Kode = new Guid(txtID.Value);
                                                    Application_DAOs.Elearning.KB.DAO_CatatanSiswa.Update(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                else
                                                {
                                                    Application_DAOs.Elearning.KB.DAO_CatatanSiswa.Insert(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                txtIDGuru.Value = Libs.LOGGED_USER_M.NoInduk;
                                            }
                                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                                            {
                                                Application_Entities.Elearning.TK.CatatanSiswa m =
                                                new Application_Entities.Elearning.TK.CatatanSiswa
                                                {
                                                    Rel_Siswa = cboSiswa.SelectedValue,
                                                    Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                                    Catatan = txtCatatanVal.Value,
                                                    TahunAjaran = QS.GetTahunAjaran(),
                                                    Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                                    Rel_KelasDet = QS.GetKelas(),
                                                    Rel_Mapel = QS.GetMapel(),
                                                    Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                                                };
                                                m.Kode = Guid.NewGuid();
                                                if (txtID.Value.Trim() != "")
                                                {
                                                    m.Kode = new Guid(txtID.Value);
                                                    Application_DAOs.Elearning.TK.DAO_CatatanSiswa.Update(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                else
                                                {
                                                    Application_DAOs.Elearning.TK.DAO_CatatanSiswa.Insert(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                txtIDGuru.Value = Libs.LOGGED_USER_M.NoInduk;
                                            }
                                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                                            {

                                                Application_Entities.Elearning.SD.CatatanSiswa m =
                                                new Application_Entities.Elearning.SD.CatatanSiswa
                                                {
                                                    Rel_Siswa = cboSiswa.SelectedValue,
                                                    Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                                    Catatan = txtCatatanVal.Value,
                                                    TahunAjaran = QS.GetTahunAjaran(),
                                                    Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                                    Rel_KelasDet = QS.GetKelas(),
                                                    Rel_Mapel = QS.GetMapel(),
                                                    Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                                                };
                                                m.Kode = Guid.NewGuid();
                                                if (txtID.Value.Trim() != "")
                                                {
                                                    m.Kode = new Guid(txtID.Value);
                                                    Application_DAOs.Elearning.SD.DAO_CatatanSiswa.Update(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                else
                                                {
                                                    Application_DAOs.Elearning.SD.DAO_CatatanSiswa.Insert(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );                                                    
                                                }
                                                txtIDGuru.Value = Libs.LOGGED_USER_M.NoInduk;
                                            }
                                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                                            {
                                                Application_Entities.Elearning.SMP.CatatanSiswa m =
                                                new Application_Entities.Elearning.SMP.CatatanSiswa
                                                {
                                                    Rel_Siswa = cboSiswa.SelectedValue,
                                                    Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                                    Catatan = txtCatatanVal.Value,
                                                    TahunAjaran = QS.GetTahunAjaran(),
                                                    Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                                    Rel_KelasDet = QS.GetKelas(),
                                                    Rel_Mapel = QS.GetMapel(),
                                                    Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                                                };
                                                m.Kode = Guid.NewGuid();
                                                if (txtID.Value.Trim() != "")
                                                {
                                                    m.Kode = new Guid(txtID.Value);
                                                    Application_DAOs.Elearning.SMP.DAO_CatatanSiswa.Update(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                else
                                                {
                                                    Application_DAOs.Elearning.SMP.DAO_CatatanSiswa.Insert(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                txtIDGuru.Value = Libs.LOGGED_USER_M.NoInduk;
                                            }
                                            else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                                            {
                                                Application_Entities.Elearning.SMA.CatatanSiswa m =
                                                new Application_Entities.Elearning.SMA.CatatanSiswa
                                                {
                                                    Rel_Siswa = cboSiswa.SelectedValue,
                                                    Rel_Kategori = cboKategoriCatatan.SelectedValue,
                                                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text),
                                                    Catatan = txtCatatanVal.Value,
                                                    TahunAjaran = QS.GetTahunAjaran(),
                                                    Semester = Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalCatatan.Text)).ToString(),
                                                    Rel_KelasDet = QS.GetKelas(),
                                                    Rel_Mapel = QS.GetMapel(),
                                                    Rel_Guru = Libs.LOGGED_USER_M.NoInduk.ToString()
                                                };
                                                m.Kode = Guid.NewGuid();
                                                if (txtID.Value.Trim() != "")
                                                {
                                                    m.Kode = new Guid(txtID.Value);
                                                    Application_DAOs.Elearning.SMA.DAO_CatatanSiswa.Update(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                else
                                                {
                                                    Application_DAOs.Elearning.SMA.DAO_CatatanSiswa.Insert(
                                                            m, Libs.LOGGED_USER_M.UserID
                                                        );
                                                }
                                                txtIDGuru.Value = Libs.LOGGED_USER_M.NoInduk;
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                ParseDropdownKelasTahunAjaran(cboKelasTahunAjaranSemester.SelectedValue);
                BindListView(!IsPostBack, Libs.GetQ().Trim());
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
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
                CatatanSiswa m = DAO_CatatanSiswa.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        txtID.Value = m.Kode.ToString();
                        cboKategoriCatatan.SelectedValue = m.Rel_Kategori.ToString();
                        cboSiswa.SelectedValue = m.Rel_Siswa;
                        txtTanggalCatatan.Text = Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false);
                        txtCatatan.Text = m.Catatan;
                        txtCatatanVal.Value = m.Catatan;
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
                CatatanSiswa m = DAO_CatatanSiswa.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                            m.TahunAjaran,
                            m.Semester,
                            m.Rel_Siswa);
                        if (m_siswa != null)
                        {
                            if (m_siswa.Nama != null)
                            {
                                ltrMsgConfirmHapus.Text = "Hapus catatan siswa ?" +
                                                          "<div style=\"margin-top: 5px; padding-left: 10px; padding-right: 10px; background-color: white; font-weight: normal; width: 100%; border-style: solid; border-color: #c0dfd7; border-width: 1px; border-radius: 5px; background-color: #F1F9F7;\">" +
                                                            "<label style=\"font-weight: bold; color: black; margin-top: 15px;\">\"" +
                                                                Libs.GetHTMLSimpleText(m_siswa.Nama) +
                                                            "\"</label>" +
                                                            (
                                                                m.Catatan.ToString().Trim().Length > 500
                                                                ? m.Catatan.ToString().Trim().Substring(0, 500) +
                                                                    (
                                                                        m.Catatan.ToString().Trim().Length > 500
                                                                        ? "..."
                                                                        : ""
                                                                    )
                                                                : m.Catatan.ToString().Trim()
                                                            ) +
                                                          "</div>";
                                BindListView(true);
                                txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();                                
                            }
                        }
                    }
                }
            }
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.Search.ToString();
        }

        protected Libs.UnitSekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetByID_Entity(QS.GetUnit());
            if (m_unit != null)
            {
                if (m_unit.Nama != null)
                {
                    if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.KB)
                    {
                        return Libs.UnitSekolah.KB;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                    {
                        return Libs.UnitSekolah.TK;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                    {
                        return Libs.UnitSekolah.SD;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                    {
                        return Libs.UnitSekolah.SMP;
                    }
                    else if (m_unit.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                    {
                        return Libs.UnitSekolah.SMA;
                    }
                }
            }

            return Libs.UnitSekolah.SALAH;
        }

        protected void cboKelasTahunAjaranSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParseDropdownKelasTahunAjaran(cboKelasTahunAjaranSemester.SelectedValue);
            BindListView(true);
        }

        protected void lnkOKCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    Libs.FILE_PAGE_URL +
                    "?t=" + Libs.GetQueryString("t") +
                    "&ft=" + Libs.GetQueryString("ft") +
                    "&kd=" + Libs.GetQueryString("kd") +
                    (
                        Libs.GetQueryString("m").Trim() != ""
                        ? "&m=" + Libs.GetQueryString("m")
                        : ""
                    ) +
                    "&q=" + txtCari.Text
                );
        }
    }
}