using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA
{
    public partial class wf_NilaiSiswa : System.Web.UI.Page
    {
        private const string NKD = "n-KD";
        private const string C_NKD = "{nkd}";
        private const string LEBAR_COL_DEFAULT = "80";
        private const string LEBAR_COL_DEFAULT2 = "100";
        private const string LEBAR_COL_DEFAULT3 = "150";
        private const string LEBAR_COL_DEFAULT_SIKAP = "400";
        private const string LEBAR_COL_DEFAULT_NO = "0.1";

        public const string SEP_DES = "{[@_@]}";
        public const string SEP_KODE_DES = "([@_@])";

        private static class QS
        {
            public static string GetUnit()
            {
                KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelasdet != null)
                {
                    if (m_kelasdet.Nama != null)
                    {
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelasdet.Rel_Kelas.ToString());
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

            public static string GetTahunAjaranPure()
            {
                string t = Libs.GetQueryString("t");
                return t;
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                return s;
            }

            public static string GetMapel()
            {
                string m = Libs.GetQueryString("m");
                return m;
            }

            public static string GetGuru()
            {
                string guru = Libs.GetQueryString("g");
                if (guru.Trim() == "") return Libs.LOGGED_USER_M.NoInduk;
                return guru;
            }
        }

        protected List<Siswa> GetListSiswaSimulasi()
        {
            List<Siswa> lst_hasil = new List<Siswa>();

            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3111", Nama = "John Wick", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3112", Nama = "Tony Stark", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3113", Nama = "Carol Danvers", JenisKelamin = "P" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3114", Nama = "Steve Rogers", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3115", Nama = "Monica Hall", JenisKelamin = "P" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3116", Nama = "Richard Hendricks", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3117", Nama = "Erlich Bachman", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3118", Nama = "Bertram Gilfoyle", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3119", Nama = "Dinesh Chugtai", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3120", Nama = "Jian Yang", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3121", Nama = "Eddie Brock", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3122", Nama = "Peter Praker", JenisKelamin = "L" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3123", Nama = "Anne Weying", JenisKelamin = "P" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3124", Nama = "Hope Van Dyne", JenisKelamin = "P" });
            lst_hasil.Add(new Siswa { Kode = Guid.NewGuid(), NISSekolah = "3125", Nama = "Scott Lang", JenisKelamin = "L" });

            return lst_hasil.OrderBy(m => m.Nama).ToList();
        }

        public enum JenisAction
        {
            DoShowPilihSiswa,
            DoShowStatistik,
            DoShowDeskripsiPenilaian,
            DoUpdate
        }

        protected class NILAI_COL
        {
            public int IdKolom { get; set; }
            public string BluePrintFormula { get; set; }
        }

        protected class FORMULA_RAPOR_KTSP
        {
            public int IdKolom { get; set; }
            public string Kode_SN_KD { get; set; }
            public string Kode_SN_KP { get; set; }
            public string KolomHeadersFormula { get; set; }
        }

        public static class AtributPenilaian
        {
            public static string TahunAjaran { get { return RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")); } }
            public static string Semester {
                get {
                    return Libs.GetQueryString("s");
                }
            }
            public static string Kelas { get {
                    if (Libs.GetQueryString("kd").Trim() == "") return KelasLevel;

                    string rel_kelas_det = Libs.GetQueryString("kd");
                    string hasil = "";

                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            return m_kelas_det.Rel_Kelas.ToString();
                        }
                    }

                    return hasil;
                } }

            public static string KelasLevel { get { return Libs.GetQueryString("k"); } }
            public static string KelasDet { get { return Libs.GetQueryString("kd"); } }
            public static string Mapel { get { return Libs.GetQueryString("m"); } }
        }

        protected void RenderCenterMenu()
        {
            ltrCenter.Text = "<div data-toggle=\"modal\" href=\"#ui_modal_pilihan\" style=\"margin: 0 auto; display: table; cursor: pointer; color: grey; margin-top: 50px;\">" +
                                "<i class=\"fa fa-folder-open\" style=\"color: #b0b0b0; font-size: 40pt;\"></i>" +
                             "</div>" +
                             "<div data-toggle=\"modal\" href=\"#ui_modal_pilihan\" style=\"margin: 0 auto; display: table; cursor: pointer; color: grey;\">" +
                                "<span style=\"font-weight: bold;\">Buka Data Nilai</span>" +
                             "</div>" +
                             "<div data-toggle=\"modal\" href=\"#ui_modal_pilihan\" style=\"margin: 0 auto; display: table; cursor: pointer; color: grey;\">" +
                                "Tampilkan nilai siswa tiap semester" +
                             "</div>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowSubHeaderGuru = true;
            this.Master.ShowHeaderSubTitle = false;
            this.Master.SelectMenuGuru_Penilaian();
            
            InitURLOnMenu();
            a_pilih_data_nilai.Visible = true;

            if (!IsPostBack)
            {
                lnkSaveDeskripsiPenilaian.Visible = true;

                string rel_kelas_det = QS.GetKelas();
                if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && Libs.GetQueryString("kdgk").Trim() != "")
                {
                    rel_kelas_det = Libs.GetQueryString("kdgk").Trim();
                    lnkSaveDeskripsiPenilaian.Visible = false;
                }
                _UI.InitModalListNilai(
                    this.Page,
                    ltrListNilaiAkademik, 
                    ltrListSikap,
                    ltrListEkskul, 
                    ltrListNilaiRapor, 
                    QS.GetTahunAjaran(), 
                    QS.GetMapel(), 
                    rel_kelas_det, 
                    QS.GetGuru()
                );

                txtSemester.Value = QS.GetSemester();
                div_button_settings.Visible = false;
                if (QS.GetSemester().Trim() != "")
                {
                    div_button_settings.Visible = true;
                    LoadData(QS.GetSemester());
                }
                else
                {
                    RenderCenterMenu();
                }

                if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
                {
                    a_pilih_data_nilai.Visible = false;
                }
                else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
                {
                    a_pilih_data_nilai.Visible = false;
                }
            }
            else
            {
                if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
                {
                    a_pilih_data_nilai.Visible = false;
                    this.Master.ShowSubHeaderGuru = false;
                }
                else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
                {
                    a_pilih_data_nilai.Visible = false;
                    this.Master.ShowSubHeaderGuru = false;
                }
                else
                {
                    this.Master.ShowSubHeaderGuru = true;
                }
            }

            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
            {
                li_nilai_rapor.Visible = true;
                li_nilai_ekskul.Visible = true;
            }
            else
            {
                if (DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                   DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Application_Libs.Libs.JENIS_MAPEL.EKSKUL)
                {
                    li_nilai_ekskul.Visible = true;
                    li_nilai_rapor.Visible = false;
                }
                else
                {
                    li_nilai_ekskul.Visible = false;
                    li_nilai_rapor.Visible = false;
                }
            }
            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
                this.Master.SetURLGuru_Penilaian("");
                this.Master.SetURLGuru_Siswa("");
                this.Master.SetURLGuru_TimeLine("");
            }
        }

        protected void InitURLOnMenu()
        {
            string ft = Libs.Decryptdata(Libs.GetQueryString("ft"));
            string url_penilaian = Libs.GetURLPenilaian(Libs.GetQueryString("kd"));
            string m = Libs.GetQueryString("m");

            string[] arr_bg =
                { "#FD6933", "#0AC6AE", "#F7921E", "#4AA4A4", "#43B8C9", "#95D1C5", "#019ADD", "#31384B", "#18AEC7", "#5299CF", "#2D2C28", "#D5C5C6", "#262726", "#01ACAC", "#322D3A", "#3B4F5D", "#009E00", "#E90080", "#549092", "#00A9A9", "#9B993A" };
            string[] arr_bg_tab =
                { "#FF8255", "#19ceb7", "#FF982A", "#54B5B5", "#47C8D9", "#9BD8CC", "#00AAF3", "#3e465b", "#1EBDD9", "#5DADEA", "#3a3a39", "#E2CFD0", "#3a3a3a", "#00BCBC", "#524A5E", "#4B6476", "#00B300", "#FD1D94", "#64A8AA", "#00C0C0", "#B7B548" };
            string[] arr_bg_image =
                { "a.png", "b.png", "c.png", "d.png", "e.png", "f.png", "g.png", "h.png", "i.png", "j.png", "k.png", "l.png", "m.png", "n.png", "o.png", "p.png", "q.png", "r.png", "u.png", "s.png", "t.png" };
            string bg_image = "";
            int _id = 0;

            if (ft.Trim() != "")
            {
                for (int i = 0; i < arr_bg.Length; i++)
                {
                    if (arr_bg_image[i] == ft)
                    {
                        _id = i;
                        bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[_id]);
                        break;
                    }
                }
                ltrHeaderPilihan.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat; background-position-x: 5px;";
                ltrHeaderTab.Text = "background-color: " + arr_bg_tab[_id] + "; ";
            }
            else
            {
                _id = 1;
                bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[_id]);
                ltrHeaderPilihan.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat; background-position-x: 5px;";
                ltrHeaderTab.Text = "background-color: " + arr_bg_tab[_id] + "; ";
            }

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
                    if (s_mapel.Trim() != "")
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                 "</label>" +
                                 "<br />" +
                                 "<label style=\"margin-left: 45px; color: white; font-weight: bold;\">" +
                                    s_mapel +
                                 "</label>";

                        List<FormasiGuruKelas_ByGuru> lst_kelasguru =
                                DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, tahun_ajaran).FindAll(m0 => m0.Semester == QS.GetSemester()).ToList();

                        if (lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == QS.GetKelas().Trim().ToUpper() && m0.KodeMapel == "").Count > 0 &&
                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                        {
                            s_html += "<br />" +
                                      "<label style=\"margin-left: 45px; color: white;\">" +
                                        "<label style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">Guru Kelas atau Wali Kelas</label>" +
                                      "</label>";
                        }
                    }
                    else
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                    "<br />" +
                                    "<label style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">Guru Kelas atau Wali Kelas</label>" +
                                 "</label>";
                    }
                }
            }

            ltrCaption.Text = s_html;

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
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                            ? "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                            : (
                                                    Libs.GetQueryString("m").Trim() != ""
                                                    ? "&m=" + Libs.GetQueryString("m")
                                                    : ""
                                              )
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
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                        ? "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                        : (
                                                Libs.GetQueryString("m").Trim() != ""
                                                ? "&m=" + Libs.GetQueryString("m")
                                                : ""
                                          )
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
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                : (
                                        Libs.GetQueryString("m").Trim() != ""
                                        ? "&m=" + Libs.GetQueryString("m")
                                        : ""
                                  )
                            )
                        )
                );
            }

            this.Master.SetURLGuru_Siswa(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&k=" + Libs.GetQueryString("k") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                : (
                                        Libs.GetQueryString("m").Trim() != ""
                                        ? "&m=" + Libs.GetQueryString("m")
                                        : ""
                                  )
                            )
                        )
                );
            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&ft=" + Libs.GetQueryString("ft") +
                            "&k=" + Libs.GetQueryString("k") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                : (
                                        Libs.GetQueryString("m").Trim() != ""
                                        ? "&m=" + Libs.GetQueryString("m")
                                        : ""
                                  )
                            )
                        )
                );
        }

        public bool IsReadOnly(DAO_Rapor_StrukturNilai.StrukturNilai m_struktur_nilai)
        {
            bool is_readonly = _UI.IsReadonlyNilai(
                                        m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas(), m_struktur_nilai.Rel_Mapel.ToString(), m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester
                                    );
            if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI) is_readonly = false;
            return is_readonly;
        }

        protected void LoadData(string semester)
        {
            string tahun_ajaran = AtributPenilaian.TahunAjaran;
            string rel_kelas = AtributPenilaian.Kelas;
            string rel_kelas_det = AtributPenilaian.KelasDet;
            string rel_mapel = AtributPenilaian.Mapel;

            if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
            {
                rel_kelas = AtributPenilaian.KelasLevel;
                this.Master.ShowSubHeaderGuru = false;
            }
            else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
            {
                this.Master.ShowSubHeaderGuru = false;
            }
            else
            {
                this.Master.ShowSubHeaderGuru = true;
            }

            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas, rel_mapel
                        );

            if (lst_stuktur_nilai.Count == 1)
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m_struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                if (m_struktur_nilai != null)
                {
                    if (m_struktur_nilai.TahunAjaran != null)
                    {
                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_struktur_nilai.Rel_Mapel.ToString());
                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
                                {
                                    //LoadDataEkskul(IsReadOnly(m_struktur_nilai));
                                    LoadDataEkskul(false);
                                }
                                else
                                {
                                    switch (m_struktur_nilai.Kurikulum)
                                    {
                                        case Libs.JenisKurikulum.SMA.KTSP:
                                            LoadDataKTSP(semester, IsReadOnly(m_struktur_nilai));
                                            break;
                                        case Libs.JenisKurikulum.SMA.KURTILAS:
                                            LoadDataKURTILAS(semester, IsReadOnly(m_struktur_nilai));
                                            break;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        protected void LoadDataKURTILAS(string semester, bool is_readonly)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";

            string tahun_ajaran = AtributPenilaian.TahunAjaran;
            string rel_kelas = AtributPenilaian.Kelas;
            string rel_kelas_det = AtributPenilaian.KelasDet;
            string rel_mapel = AtributPenilaian.Mapel;

            List<NILAI_COL> lst_kolom_nilai_nkd = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_na_pengetahuan = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_na_keterampilan = new List<NILAI_COL>();

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                //status bar
                ltrStatusBar.Text = "";
                if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
                {
                    ltrStatusBar.Text += "&nbsp;" +
                                         "<span style=\"font-weight: bold; color: red;\">" +
                                            "<i class=\"fa fa-exclamation-triangle\"></i>" +
                                            "&nbsp;" +
                                            "PREVIEW INPUT" +
                                            "&nbsp;" +
                                         "</span>&nbsp;";
                }

                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        ltrStatusBar.Text += "<span style=\"font-weight: bold;\">" + m_mapel.Nama + "</span>";
                    }
                }

                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                             "&nbsp;" +
                                             "<span style=\"font-weight: normal;\">" + m_kelas_det.Nama + "</span>";
                    }
                }

                if (tahun_ajaran.Trim() != "")
                {
                    ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                         "&nbsp;" +
                                         "<span style=\"font-weight: normal;\">" + tahun_ajaran + "</span>";
                }

                if (semester.Trim() != "")
                {
                    ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;Sm." : "") +
                                         "&nbsp;" +
                                         "<span style=\"font-weight: normal;\">" + semester + "</span>";
                }
                //end status bar

                string s_kolom = "";
                string s_kolom_width = "";
                string s_kolom_style = "";
                string s_content = "";
                string s_merge_cells = "";
                string s_formula = "";
                string s_formula_rapor_pengetahuan_non_uas = "";
                string s_formula_rapor_pengetahuan_uas = "";
                string s_formula_rapor_keterampilan = "";

                string s_arr_js_siswa = "";
                string s_arr_js_ap = "";
                string s_arr_js_kd = "";
                string s_arr_js_kp = "";

                string css_bg = "#fff";
                string css_bg_nkd = "#fff";
                string css_bg_nap = "#fff";
                string css_bg_nilaiakhir = "#fff";

                string formula_blueprint_predikat = "";

                List<string> lst_ap = new List<string>();
                List<string> lst_kd = new List<string>();
                List<string> lst_kp = new List<string>();
                List<NILAI_COL> lst_nkd = new List<NILAI_COL>();
                List<FORMULA_RAPOR_KTSP> lst_col_rapor_ppk = new List<FORMULA_RAPOR_KTSP>();
                List<FORMULA_RAPOR_KTSP> lst_col_rapor_p = new List<FORMULA_RAPOR_KTSP>();

                List<int> lst_col_terakhir_nkd = new List<int>();
                List<int> lst_col_teraknir_ap_row0 = new List<int>();
                List<int> lst_col_teraknir_ap_row1 = new List<int>();
                List<int> lst_col_teraknir_ap_row2 = new List<int>();                

                int id_col_all = 0;
                int id_col_nilai_mulai = 4;
                int id_jml_fixed_row = 3;
                int id_col_rapor_pengetahuan_nilai = 0;
                int id_col_rapor_pengetahuan_predikat = 0;
                int id_col_rapor_keterampilan_nilai = 0;
                int id_col_rapor_keterampilan_predikat = 0;

                string js_statistik = "";

                //struktur nilai
                List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                        tahun_ajaran, semester, rel_kelas, rel_mapel
                    );

                if (lst_stuktur_nilai.Count == 1)
                {
                    string s_js_arr_kolom1 = "";
                    string s_js_arr_kolom2 = "";
                    string s_js_arr_kolom3 = "";
                    string s_js_arr_nilai = "";

                    lst_nkd.Clear();
                    lst_col_terakhir_nkd.Clear();

                    //parse struktur KURTILAS
                    DAO_Rapor_StrukturNilai.StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();

                    //kurikulum KURTILAS
                    if (struktur_nilai.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS)
                    {
                        formula_blueprint_predikat = GetFormulaPredikatKurtilas(struktur_nilai.Kode.ToString());

                        if (is_readonly)
                        {
                            ltrStatusBar.Text += "&nbsp;&nbsp;" +
                                                 "<span title=\" Data Terkunci/Tidak Bisa Di-edit \" style=\"font-weight: bold; yellow: red;\"><i class=\"fa fa-lock\"></i>&nbsp;Data Terkunci</span>";
                            ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: white;\"></i>" +
                                                "&nbsp;&nbsp;" +
                                                ltrStatusBar.Text;
                            div_statusbar.Attributes["style"] = "color: white; height: 40px; background-color: #ce3584; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
                        }
                        else
                        {
                            ltrStatusBar.Text += "&nbsp;&nbsp;" +
                                                 "<span title=\" Data Bisa Di-edit \" style=\"font-weight: bold; color: green;\"><i class=\"fa fa-unlock\"></i></span>";
                            ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: green;\"></i>" +
                                                "&nbsp;&nbsp;" +
                                                ltrStatusBar.Text;
                            div_statusbar.Attributes["style"] = "color: black; height: 40px; background-color: #eeeeee; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
                        }

                        if (struktur_nilai.KKM > 0)
                        {
                            ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;KKM :" : "") +
                                                 "&nbsp;" +
                                                 "<span style=\"font-weight: bold;\">" + Math.Round(struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) + "</span>";

                            txtKKM.Value = Math.Round(struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString();
                        }

                        //list siswa
                        List<Siswa> lst_siswa = new List<Siswa>();

                        bool b_mapel_pilihan = false;
                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                b_mapel_pilihan = (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() != "X" ? true : false);
                                if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() == "X")
                                {
                                    if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                                        Libs.LOGGED_USER_M.NoInduk,
                                                        QS.GetTahunAjaran(),
                                                        QS.GetSemester(),
                                                        QS.GetKelas(),
                                                        QS.GetMapel()
                                                    ))
                                    {
                                        b_mapel_pilihan = true;
                                    }
                                }
                            }
                        }

                        if (b_mapel_pilihan)
                        {
                            lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                    tahun_ajaran,
                                    semester,
                                    rel_mapel,
                                    rel_kelas_det
                                );
                        }
                        else
                        {
                            if (
                                m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT
                            )
                            {
                                lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                                        tahun_ajaran,
                                        semester,
                                        rel_mapel,
                                        rel_kelas,
                                        rel_kelas_det
                                    );
                            }
                            else
                            {
                                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                    m_kelas.Rel_Sekolah.ToString(),
                                    rel_kelas_det,
                                    QS.GetTahunAjaran(),
                                    QS.GetSemester()
                                );
                            }
                        }

                        if (Libs.GetQueryString("kdgk").Trim() != "")
                        {
                            lst_siswa = lst_siswa.FindAll(m0 => m0.Rel_KelasDet.ToString().ToUpper().Trim() == Libs.GetQueryString("kdgk").ToString().ToUpper().Trim());
                        }

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                        "{ row: 0, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true }, " +
                                        "{ row: 1, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true }, " +
                                        "{ row: 0, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true }, " +
                                        "{ row: 1, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true }, " +
                                        "{ row: 0, col: 2, className: \"htCenter htMiddle htFontBlack\", readOnly: true }, " +
                                        "{ row: 1, col: 2, className: \"htCenter htMiddle htFontBlack\", readOnly: true }";

                        //init ap arr kompetensi dasar/kd
                        s_arr_js_ap = "";
                        lst_ap.Clear();
                        for (int i = 0; i < id_col_nilai_mulai; i++)
                        {
                            s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                           "\"\"";
                            lst_ap.Add("");
                        }
                        //end init

                        //init js arr kompetensi dasar/kd
                        s_arr_js_kd = "";
                        lst_kd.Clear();
                        for (int i = 0; i < id_col_nilai_mulai; i++)
                        {
                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                           "\"\"";
                            lst_kd.Add("");
                        }
                        //end init

                        //init js arr komponen penilaian/kp
                        s_arr_js_kp = "";
                        lst_kp.Clear();
                        for (int i = 0; i < id_col_nilai_mulai; i++)
                        {
                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                           "\"\"";
                            lst_kp.Add("");
                        }
                        //end init

                        //load kurtilas ap
                        List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian =
                            DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                        id_col_all = id_col_nilai_mulai;
                        foreach (Rapor_StrukturNilai_KURTILAS_AP m_sn_ap in lst_aspek_penilaian)
                        {
                            int id_col_mulai_merge_ap = id_col_all;
                            int id_col_sampai_merge_ap = id_col_mulai_merge_ap;

                            if (m_sn_ap != null)
                            {
                                if (m_sn_ap.JenisPerhitungan != null)
                                {
                                    Rapor_AspekPenilaian m_ap =
                                        DAO_Rapor_AspekPenilaian.GetByID_Entity(m_sn_ap.Rel_Rapor_AspekPenilaian.ToString());

                                    if (m_ap != null)
                                    {
                                        if (m_ap.Nama != null)
                                        {
                                            //load kurtilas kd
                                            List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar =
                                                DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(m_sn_ap.Kode.ToString());

                                            int id_kd = 1;
                                            foreach (Rapor_StrukturNilai_KURTILAS_KD m_sn_kd in lst_kompetensi_dasar)
                                            {
                                                int id_col_mulai_merge_kd = id_col_all;
                                                int id_col_sampai_merge_kd = id_col_mulai_merge_kd;

                                                if (m_sn_kd != null)
                                                {
                                                    if (m_sn_kd.JenisPerhitungan != null)
                                                    {
                                                        Rapor_KompetensiDasar m_kd =
                                                            DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                        if (m_kd != null)
                                                        {
                                                            if (m_kd.Nama != null)
                                                            {

                                                                //load kurtilas kp
                                                                List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                                                    DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                                                                int id_kp = 1;
                                                                s_formula = "";                                                            
                                                                foreach (Rapor_StrukturNilai_KURTILAS_KP m_sn_kp in lst_komponen_penilaian)
                                                                {
                                                                    Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                    if (m_kp != null)
                                                                    {
                                                                        if (m_kp.Nama != null)
                                                                        {
                                                                            js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                            (id_col_all).ToString();

                                                                            //add ap to var arr js
                                                                            s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                                           "\"" + m_sn_ap.Kode.ToString() + "\"";
                                                                            lst_ap.Add(m_sn_ap.Kode.ToString());

                                                                            //add kd to var arr js
                                                                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                                           "\"" + m_sn_kd.Kode.ToString() + "\"";
                                                                            lst_kd.Add(m_sn_kd.Kode.ToString());

                                                                            //add kp to var arr js
                                                                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                                           "\"" + m_sn_kp.Kode.ToString() + "\"";
                                                                            lst_kp.Add(m_sn_kp.Kode.ToString());
                                                                            //end add ap, kd & kp

                                                                            if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                s_formula += (s_formula.Trim() != "" ? " + " : "") +
                                                                                             "(" +
                                                                                                    "IF(" +
                                                                                                        "OR(" + Libs.GetColHeader(id_col_all + 1) + "#" + " = \"\"," + Libs.GetColHeader(id_col_all + 1) + "#" + " = \"BL\"), 0, " +
                                                                                                        Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                    ")" +
                                                                                                    "*(" + m_sn_kp.BobotNK.ToString() + "%)" +
                                                                                             ")";
                                                                            }
                                                                            else if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                s_formula += (s_formula.Trim() != "" ? ", " : "") +
                                                                                             "IF(" +
                                                                                                 "OR(" + Libs.GetColHeader(id_col_all + 1) + "#" + " = \"\"," + Libs.GetColHeader(id_col_all + 1) + "#" + " = \"BL\"), 0, " +
                                                                                                 Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                             ")";
                                                                            }

                                                                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                                              "\"" +
                                                                                                    (
                                                                                                        m_sn_ap.Poin.Trim() != ""
                                                                                                        ? m_sn_ap.Poin + "<br />"
                                                                                                        : ""
                                                                                                    ) +
                                                                                                    Libs.GetHTMLSimpleText(m_ap.Nama).Replace("\"", "\\\"") + 
                                                                                              "\" ";
                                                                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                              "\"" + 
                                                                                                    (
                                                                                                        m_sn_kd.Poin.Trim() != ""
                                                                                                        ? m_sn_kd.Poin + "<br />"
                                                                                                        : ""
                                                                                                    ) +
                                                                                                    //Libs.GetHTMLSimpleText(m_ap.Nama).Replace("\"", "\\\"") + 
                                                                                                    Libs.GetHTMLSimpleText(m_kd.Nama).Replace("\"", "\\\"") +
                                                                                              "\" ";
                                                                            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                                                              "\"" +
                                                                                                    //Libs.GetHTMLSimpleText(m_ap.Nama).Replace("\"", "\\\"") +
                                                                                                    Libs.GetHTMLSimpleText(m_kp.Nama).Replace("\"", "\\\"") +
                                                                                                    (
                                                                                                        m_sn_kp.IsKomponenRapor
                                                                                                        ? "<sup title=' Komponen Rapor '>" +
                                                                                                            "<i class='fa fa-check-circle' style='color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;'></i>" +
                                                                                                          "</sup>"
                                                                                                        : ""
                                                                                                    ) +
                                                                                                    (
                                                                                                        m_sn_kp.BobotNK > 0
                                                                                                        ? "<br />" +
                                                                                                          "<sup class='badge' style='background-color: #B7770D; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                                            Math.Round(m_sn_kp.BobotNK, 0).ToString() + "%" +
                                                                                                          "</sup>"
                                                                                                        : ""
                                                                                                    ) +
                                                                                              "\" ";

                                                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                                             "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                                             "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";


                                                                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                                              LEBAR_COL_DEFAULT2;

                                                                            if (
                                                                                    Libs.GetHTMLSimpleText(m_ap.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.UAS &&
                                                                                    Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.UAS &&
                                                                                    Libs.GetHTMLSimpleText(m_kp.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.UAS
                                                                               )
                                                                            {
                                                                                //merge cell row kd
                                                                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                                                                "{" +
                                                                                                "  row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, " +
                                                                                                "  colspan: 1" +
                                                                                                "} ";
                                                                            }

                                                                            //formula rapor from ap
                                                                            if (Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN.ToUpper())
                                                                            {
                                                                                if (lst_komponen_penilaian.Count == 1)
                                                                                {
                                                                                    s_formula_rapor_pengetahuan_non_uas += (s_formula_rapor_pengetahuan_non_uas.Trim() != "" ? ", " : "") +
                                                                                                                   "IF(" +
                                                                                                                        "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\")" + ", 0 ," +
                                                                                                                        Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                                   ")";
                                                                                }
                                                                            }
                                                                            else if (Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.ToUpper())
                                                                            {
                                                                                s_formula_rapor_pengetahuan_uas += (s_formula_rapor_pengetahuan_uas.Trim() != "" ? ", " : "") +
                                                                                                                   "IF(" +
                                                                                                                        "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\")" + ", 0 ," +
                                                                                                                        Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                                   ")";
                                                                            }
                                                                            else if (Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN.ToUpper())
                                                                            {
                                                                                if (lst_komponen_penilaian.Count == 1)
                                                                                {
                                                                                    s_formula_rapor_keterampilan += (s_formula_rapor_keterampilan.Trim() != "" ? ", " : "") +
                                                                                                                "IF(" +
                                                                                                                    "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\")" + ", 0 ," +
                                                                                                                    Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                                ")";
                                                                                }
                                                                            }                                                                            
                                                                            //end formula rapor from ap

                                                                            id_col_all++;
                                                                            id_kp++;
                                                                            id_col_sampai_merge_ap++;
                                                                            id_col_sampai_merge_kd++;
                                                                        }
                                                                    }
                                                                }

                                                                //add nilai nkd
                                                                if (lst_komponen_penilaian.Count > 1)
                                                                {
                                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                    (id_col_all).ToString();

                                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                                       "\"" +
                                                                                            (
                                                                                                m_sn_ap.Poin.Trim() != ""
                                                                                                ? m_sn_ap.Poin + "<br />"
                                                                                                : ""
                                                                                            ) +
                                                                                            Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                                                       "\" ";
                                                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                       "\"" +
                                                                                            (
                                                                                                m_sn_kd.Poin.Trim() != ""
                                                                                                ? m_sn_kd.Poin + "<br />"
                                                                                                : ""
                                                                                            ) +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                                                       "\" ";
                                                                    s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                                                       "\"" + 
                                                                                            NKD +
                                                                                            (
                                                                                                m_sn_kd.IsKomponenRapor
                                                                                                ? "<sup title=' Komponen Rapor '>" +
                                                                                                    "<i class='fa fa-check-circle' style='color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;'></i>" +
                                                                                                  "</sup>"
                                                                                                : ""
                                                                                            ) +
                                                                                       "\" ";

                                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                                      LEBAR_COL_DEFAULT2;

                                                                    //add nilai nkd
                                                                    if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                    {
                                                                        s_formula = "IF(" +
                                                                                        "ROUND((" + s_formula + ")," + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA + ") <= 0, " +
                                                                                        "\"\"," +
                                                                                        "ROUND((" + s_formula + ")," + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA + ")" +
                                                                                    ")";
                                                                    }
                                                                    else if (m_sn_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                    {
                                                                        s_formula = "IF(" +
                                                                                        "ROUND(AVERAGE(" + s_formula + ")," + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA + ") <= 0, " +
                                                                                        "\"\"," +
                                                                                        "ROUND(AVERAGE(" + s_formula + ")," + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA + ")" +
                                                                                    ")";
                                                                    }

                                                                    //add content kolom nkd
                                                                    lst_kolom_nilai_nkd.Add(new NILAI_COL
                                                                    {
                                                                        BluePrintFormula = "=" + s_formula,
                                                                        IdKolom = id_col_all
                                                                    });
                                                                    //end content kolom nkd

                                                                    //formula rapor from nkd
                                                                    if (m_sn_kd.IsKomponenRapor)
                                                                    {
                                                                        if (Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN.ToUpper())
                                                                        {
                                                                            s_formula_rapor_pengetahuan_non_uas += (s_formula_rapor_pengetahuan_non_uas.Trim() != "" ? ", " : "") +
                                                                                                                   "ROUND(" +
                                                                                                                       "IF(" +
                                                                                                                            "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\")" + ", 0 ," +
                                                                                                                            Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                                       "), " +
                                                                                                                       Constantas.PEMBULATAN_DESIMAL_NILAI_SMA +
                                                                                                                    ")";
                                                                        }
                                                                        else if (Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.ToUpper())
                                                                        {
                                                                            s_formula_rapor_pengetahuan_uas += (s_formula_rapor_pengetahuan_uas.Trim() != "" ? ", " : "") +
                                                                                                               "ROUND(" +
                                                                                                                   "IF(" +
                                                                                                                        "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\")" + ", 0 ," +
                                                                                                                        Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                                   "), " +
                                                                                                                   Constantas.PEMBULATAN_DESIMAL_NILAI_SMA +
                                                                                                               ")";
                                                                        }
                                                                        else if (Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN.ToUpper())
                                                                        {
                                                                            s_formula_rapor_keterampilan += (s_formula_rapor_keterampilan.Trim() != "" ? ", " : "") +
                                                                                                            "ROUND(" +
                                                                                                                "IF(" +
                                                                                                                    "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\")" + ", 0 ," +
                                                                                                                    Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                                "), " +
                                                                                                                   Constantas.PEMBULATAN_DESIMAL_NILAI_SMA +
                                                                                                            ")";
                                                                        }
                                                                    }
                                                                    //end formula rapor from nkd

                                                                    //add ap to var arr js
                                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                                   "\"" + m_sn_ap.Kode.ToString() + "\"";
                                                                    lst_ap.Add(m_sn_ap.Kode.ToString());

                                                                    //add kd to var arr js
                                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                                   "\"" + m_sn_kd.Kode.ToString() + "\"";
                                                                    lst_kd.Add(m_sn_kd.Kode.ToString());

                                                                    //add kp to var arr js
                                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                                   "\"\"";
                                                                    lst_kp.Add("");
                                                                    //end add ap, kd & kp

                                                                    lst_nkd.Add(new NILAI_COL
                                                                    {
                                                                        IdKolom = id_col_all,
                                                                        BluePrintFormula = s_formula
                                                                    });

                                                                    id_col_all++;
                                                                    id_col_sampai_merge_ap++;
                                                                    id_col_sampai_merge_kd++;
                                                                    //end nilai nkd
                                                                }

                                                                //end load kurtilas kp

                                                            }
                                                        }

                                                        id_kd++;
                                                    }
                                                }

                                                //merge cell row kd
                                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                                "{" +
                                                                "  row: 1, col: " + (id_col_mulai_merge_kd) + ", rowspan: 1, " +
                                                                "  colspan: " + (id_col_sampai_merge_kd - id_col_mulai_merge_kd).ToString() +
                                                                "} ";

                                                if (id_kd > lst_kompetensi_dasar.Count)
                                                {
                                                    lst_col_teraknir_ap_row1.Add(id_col_mulai_merge_kd);
                                                }
                                            }
                                            //end load kurtilas kd

                                        }
                                    }

                                }
                            }

                            if (id_col_sampai_merge_ap - id_col_mulai_merge_ap > 1)
                            {
                                //merge cell row ap
                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                "{" +
                                                "  row: 0, col: " + (id_col_mulai_merge_ap) + ", rowspan: 1, " +
                                                "  colspan: " + (id_col_sampai_merge_ap - id_col_mulai_merge_ap).ToString() +
                                                "} ";
                            }

                            //col ap terakhir
                            lst_col_teraknir_ap_row0.Add(id_col_mulai_merge_ap);
                            lst_col_teraknir_ap_row2.Add(id_col_all - 1);
                            //end col ap terakhir
                        }
                        //end load kurtilas ap

                        //tambahkan kolom nilai rapor
                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"NILAI RAPOR\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"" + Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN + "\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"Nilai\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();
                        id_col_rapor_pengetahuan_nilai = id_col_all;
                        //------add content kolom na pengetahuan
                        lst_kolom_nilai_na_pengetahuan.Add(new NILAI_COL
                        {
                            BluePrintFormula = "=" + s_formula,
                            IdKolom = id_col_all
                        });
                        //------end content kolom na pengetahuan
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"NILAI RAPOR\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"" + Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN + "\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"Predikat\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        id_col_rapor_pengetahuan_predikat = id_col_all;
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 1, col: " + (id_col_all - 2) + ", rowspan: 1, " +
                                        "  colspan: 2" +
                                        "} ";

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"NILAI RAPOR\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"" + Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN + "\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"Nilai\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();
                        id_col_rapor_keterampilan_nilai = id_col_all;
                        //------add content kolom na keterampilan
                        lst_kolom_nilai_na_keterampilan.Add(new NILAI_COL
                        {
                            BluePrintFormula = "=" + s_formula,
                            IdKolom = id_col_all
                        });
                        //-----end content kolom na keterampilan
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"NILAI RAPOR\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"" + Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN + "\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"Predikat\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        id_col_rapor_keterampilan_predikat = id_col_all;
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 0, col: " + (id_col_all - 4) + ", rowspan: 1, " +
                                        "  colspan: 4" +
                                        "} ";
                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 1, col: " + (id_col_all - 2) + ", rowspan: 1, " +
                                        "  colspan: 2" +
                                        "} ";
                        //end tambahkan kolom nilai rapor

                        //tambahkan kolom kepribadian siswa
                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"PERILAKU BELAJAR\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"HD\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT_NO;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"PERILAKU BELAJAR\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"HD MAKS.\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT_NO;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"PERILAKU BELAJAR\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"LK\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT_NO;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"PERILAKU BELAJAR\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"RJ\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT_NO;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"PERILAKU BELAJAR\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"RPKB\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT_NO;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 0, col: " + (id_col_all - 5) + ", rowspan: 1, " +
                                        "  colspan: 5" +
                                        "} ";
                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 1, col: " + (id_col_all - 5) + ", rowspan: 1, " +
                                        "  colspan: 5" +
                                        "} ";
                        //end tambahkan kolom kepribadian siswa

                        //tambahkan capaian kedisiplinan siswa
                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"CAPAIAN KEDISIPLINAN\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"KEHADIRAN\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();

                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"CAPAIAN KEDISIPLINAN\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"KETEPATAN WAKTU\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"CAPAIAN KEDISIPLINAN\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"PENGGUNAAN SERAGAM\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                          "\"CAPAIAN KEDISIPLINAN\" ";
                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                          "\"PENGGUNAAN KAMERA\" ";

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                          LEBAR_COL_DEFAULT2;

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_ap.Add("");

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp
                        id_col_all++;

                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 0, col: " + (id_col_all - 4) + ", rowspan: 1, " +
                                        "  colspan: 4" +
                                        "} ";
                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 1, col: " + (id_col_all - 4) + ", rowspan: 1, " +
                                        "  colspan: 4" +
                                        "} ";
                        //end tambahkan kolom capaian kedisiplinan siswa

                        //tambahkan kolom nilai sikap
                        string js_nilai_sikap = "";
                        int id_col_all_without_sikap = id_col_all;
                        List<Rapor_StrukturNilai_KURTILAS_Sikap> lst_sikap =
                            DAO_Rapor_StrukturNilai_KURTILAS_Sikap.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());
                        Dictionary<string, string> lst_js_var_sikap = new Dictionary<string, string>();
                        foreach (Rapor_StrukturNilai_KURTILAS_Sikap m_sikap in lst_sikap)
                        {
                            Rapor_KompetensiDasar m_kd_sikap = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sikap.Rel_Rapor_KompetensiDasar.ToString());
                            if (m_kd_sikap != null)
                            {
                                if (m_kd_sikap.Nama != null)
                                {
                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                       "\"" + Libs.GetHTMLSimpleText(m_kd_sikap.Nama) +
                                                              "<div style='text-align: justify; font-weight: normal; font-size: 8pt; margin-top: 5px; border-radius: 3px; padding-top: 0px; padding-bottom: 0px; line-height: 1.3em; padding-left: 5px; padding-right: 5px;'>" +
                                                                    m_sikap.Deskripsi.Replace("\"", "&#34;").Replace("'", "&#39;").Replace("<p>", "").Replace("</p>", "") +                                                                    
                                                              "</div>" +
                                                       "\" ";
                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                       "\"\" ";
                                    s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                       "\"\" ";

                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                      LEBAR_COL_DEFAULT_SIKAP;

                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                    s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                     "{" +
                                                     "  row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, " +
                                                     "  colspan: 1" +
                                                     "} ";

                                    //add ap to var arr js
                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                   "\"\"";
                                    lst_ap.Add("");

                                    //add kd to var arr js
                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                   "\"" + m_kd_sikap.Kode.ToString() + "\"";
                                    lst_kd.Add(m_kd_sikap.Kode.ToString());

                                    //add kp to var arr js
                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                   "\"\"";
                                    lst_kp.Add("");
                                    //end add ap, kd & kp

                                    string js_var_nilai_sikap = "";
                                    foreach (string nilai_sikap in Libs.NILAI_SIKAP_KURTILAS.GetListNilaiSikap())
                                    {
                                        js_var_nilai_sikap += (js_var_nilai_sikap.Trim() != "" ? "," : "") +
                                                              "'" + nilai_sikap + "'";
                                    }
                                    if (js_var_nilai_sikap.Trim() != "")
                                    {
                                        string s_id = "sikap_" + Guid.NewGuid().ToString().Replace("-", "");
                                        js_var_nilai_sikap = "var " + s_id + " = " +
                                                             "[" + js_var_nilai_sikap + "];";
                                        lst_js_var_sikap.Add(s_id, js_var_nilai_sikap);
                                    }
                                    js_nilai_sikap += (js_nilai_sikap.Trim() != "" && js_var_nilai_sikap.Trim() != "" ? ", " : "") +
                                                      js_var_nilai_sikap;

                                    id_col_all++;
                                }
                            }
                        }
                        //end tambahkan kolom nilai sikap

                        if (s_js_arr_kolom1.Trim() != "") s_js_arr_kolom1 = ", " + s_js_arr_kolom1;
                        if (s_js_arr_kolom2.Trim() != "") s_js_arr_kolom2 = ", " + s_js_arr_kolom2;
                        if (s_js_arr_kolom3.Trim() != "") s_js_arr_kolom3 = ", " + s_js_arr_kolom3;
                        if (s_kolom_width.Trim() != "") s_kolom_width = ", " + s_kolom_width;

                        s_kolom = "[" +
                                      "\"#\", " +
                                      "\"NIS\", " +
                                      "\"NAMA SISWA\", " +
                                      "\"L/P\" " +
                                      s_js_arr_kolom1 +
                                  "], " +
                                  "[" +
                                      "\"#\", " +
                                      "\"NIS\", " +
                                      "\"NAMA SISWA\", " +
                                      "\"L/P\" " +
                                      s_js_arr_kolom2 +
                                  "], " +
                                  "[" +
                                      "\"#\", " +
                                      "\"NIS\", " +
                                      "\"NAMA SISWA\", " +
                                      "\"L/P\" " +
                                      s_js_arr_kolom3 +
                                  "]";

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }," +
                                         "{ row: 1, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }," +
                                         "{ row: 2, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }";

                        //list siswa dan nilainya
                        //get list nilai jika ada
                        Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                                tahun_ajaran, semester, rel_kelas_det, rel_mapel
                            ).FirstOrDefault();
                        
                        List<Rapor_NilaiSiswa_KURTILAS> lst_nilai_siswa = null;
                        List<Rapor_NilaiSiswa_KURTILAS_Det> lst_nilai_siswa_det = null;
                        if (m_nilai != null)
                        {
                            if (m_nilai.Kurikulum != null)
                            {
                                lst_nilai_siswa = DAO_Rapor_NilaiSiswa_KURTILAS.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                                lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                            }
                        }

                        //init js arr siswa
                        s_arr_js_siswa = "";
                        for (int i = 0; i < id_jml_fixed_row; i++)
                        {
                            s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                              "\"\"";
                        }

                        int id = 1;
                        foreach (Siswa m_siswa in lst_siswa)
                        {
                            string kelas_det = "";
                            m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                            if (m_kelas_det != null)
                            {
                                if (m_kelas_det.Nama != null)
                                {
                                    kelas_det = m_kelas_det.Nama;
                                }
                            }

                            Rapor_NilaiSiswa_KURTILAS m_nilai_siswa = new Rapor_NilaiSiswa_KURTILAS();
                            if (lst_nilai_siswa != null)
                            {
                                m_nilai_siswa = lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString() == m_siswa.Kode.ToString()).FirstOrDefault();
                            }

                            css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                            css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                            css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                            css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");

                            s_js_arr_nilai = "";
                            s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                              "\"" + m_siswa.Kode.ToString() + "\"";

                            if (id_col_all > id_col_nilai_mulai)
                            {
                                for (int i = id_col_nilai_mulai; i < id_col_all; i++)
                                {
                                    bool is_nkd = false;
                                    bool is_col_terakhir_ap_row2 = false;
                                    bool is_col_terakhir_ap_row1 = false;
                                    bool is_col_terakhir_ap_row0 = false;

                                    s_formula = "";
                                    foreach (NILAI_COL item_nkd in lst_nkd)
                                    {
                                        if (i == item_nkd.IdKolom)
                                        {
                                            is_nkd = true;
                                            //parse formula
                                            s_formula = (
                                                            item_nkd.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()).Trim() != ""
                                                            ? "=" : ""
                                                        ) +
                                                        item_nkd.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            break;
                                        }
                                    }

                                    foreach (int col in lst_col_teraknir_ap_row0)
                                    {
                                        if (i == col)
                                        {
                                            is_col_terakhir_ap_row0 = true;
                                            break;
                                        }
                                    }

                                    foreach (int col in lst_col_teraknir_ap_row1)
                                    {
                                        if (i == col)
                                        {
                                            is_col_terakhir_ap_row1 = true;
                                            break;
                                        }
                                    }

                                    foreach (int col in lst_col_teraknir_ap_row2)
                                    {
                                        if (i == col)
                                        {
                                            is_col_terakhir_ap_row2 = true;
                                            break;
                                        }
                                    }

                                    //kolom style header
                                    if (is_nkd || is_col_terakhir_ap_row0 || is_col_terakhir_ap_row1 || is_col_terakhir_ap_row2)
                                    {
                                        if (is_col_terakhir_ap_row0)
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: 0, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack htBorderRightNAPThin\", readOnly: true, renderer: \"html\" }";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                            "{ row: 0, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                        }

                                        if (is_col_terakhir_ap_row1)
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: 1, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack htBorderRightNAPThin\", readOnly: true, renderer: \"html\" }";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                            "{ row: 1, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                        }

                                        if (is_col_terakhir_ap_row2)
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: 2, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack htBorderRightNAPThin\", readOnly: true, renderer: \"html\" }";
                                            txtKTSPColAP.Value += ";" + i.ToString() + ";";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: 2, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                        }
                                    }
                                    else
                                    {
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: 0, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: 1, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: 2, col: " + i.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                    }
                                    //end kolom style header

                                    if (is_nkd)
                                    {
                                        //nkd per kompetensi dasar
                                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                          "'" + s_formula + "'";

                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nkd + " htFontBlack htBorderRightNKD\", readOnly: true }";
                                    }
                                    else if (is_col_terakhir_ap_row2)
                                    {
                                        //get nilainya disini
                                        Rapor_NilaiSiswa_KURTILAS_Det m_nilai_siswa_det_kurtilas =
                                            (
                                                lst_nilai_siswa_det != null
                                                ? lst_nilai_siswa_det.FindAll(
                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                             m.Rel_Rapor_StrukturNilai_KURTILAS_AP.Trim().ToUpper() == (i < lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                             m.Rel_Rapor_StrukturNilai_KURTILAS_KD.Trim().ToUpper() == (i < lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                             m.Rel_Rapor_StrukturNilai_KURTILAS_KP.Trim().ToUpper() == (i < lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                                    ).FirstOrDefault()
                                                : null
                                            );

                                        //get nilai siswa disini
                                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                          "'" +
                                                          (
                                                                m_nilai_siswa_det_kurtilas != null
                                                                ? (
                                                                    m_nilai_siswa_det_kurtilas.Nilai != null
                                                                    ? m_nilai_siswa_det_kurtilas.Nilai.Replace(@"""", "").Replace("'", "").Replace("\\", "").Trim().ToUpper()
                                                                    : ""
                                                                    )
                                                                : ""
                                                          ) +
                                                          "'";
                                        //end get nilai siswa

                                        if (lst_ap[i].Trim() == "" && lst_kd[i].Trim() != "" && lst_kp[i] == "") //biasanya nilai sikap
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack htFontBold htBorderRightNAPThin\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + "}";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack htBorderRightNAPThin\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + "}";
                                        }                                        
                                    }
                                    else
                                    {
                                        if (
                                                i != id_col_rapor_pengetahuan_nilai && i != id_col_rapor_pengetahuan_predikat &&
                                                i != id_col_rapor_keterampilan_nilai && i != id_col_rapor_keterampilan_predikat && 
                                                (
                                                    i != (id_col_rapor_keterampilan_predikat + 1) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 2) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 3) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 4) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 5) &&

                                                    i != (id_col_rapor_keterampilan_predikat + 6) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 7) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 8) &&
                                                    i != (id_col_rapor_keterampilan_predikat + 9)
                                                )
                                           )
                                        {
                                            //get nilainya disini
                                            Rapor_NilaiSiswa_KURTILAS_Det m_nilai_siswa_det_kurtilas =
                                                (
                                                    lst_nilai_siswa_det != null 
                                                    ?
                                                    lst_nilai_siswa_det.FindAll(
                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                             m.Rel_Rapor_StrukturNilai_KURTILAS_AP.Trim().ToUpper() == (i < lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                             m.Rel_Rapor_StrukturNilai_KURTILAS_KD.Trim().ToUpper() == (i < lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                             m.Rel_Rapor_StrukturNilai_KURTILAS_KP.Trim().ToUpper() == (i < lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                                    ).FirstOrDefault()
                                                    : null);

                                            //get nilai siswa disini
                                            s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                              "'" +
                                                              (
                                                                    m_nilai_siswa_det_kurtilas != null
                                                                    ? (
                                                                        m_nilai_siswa_det_kurtilas.Nilai != null
                                                                        ? m_nilai_siswa_det_kurtilas.Nilai.Replace(@"""", "").Replace("'", "").Replace("\\", "")
                                                                        : ""
                                                                        )
                                                                    : ""
                                                              ) +
                                                              "'";
                                            //end get nilai siswa


                                            if (lst_ap[i].Trim() == "" && lst_kd[i].Trim() != "" && lst_kp[i] == "") //biasanya nilai sikap
                                            {
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack htFontBold " +
                                                                 (lst_col_terakhir_nkd.FindAll(m => m == i).Count > 0 ? "htBorderRightNKD" : "") + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + "}";
                                            }
                                            else
                                            {
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " +
                                                                 (lst_col_terakhir_nkd.FindAll(m => m == i).Count > 0 ? "htBorderRightNKD" : "") + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + "}";
                                            }                                            
                                        }
                                        else
                                        {
                                            string s_nilai_rapor = "";
                                            Rapor_StrukturNilai_KURTILAS struktur_nilai_kurtilas = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(struktur_nilai.Kode.ToString());

                                            //kolom-kolom nilai rapor
                                            if (i == id_col_rapor_pengetahuan_nilai)
                                            {
                                                //parse formula rapor pengetahuan
                                                if (s_formula_rapor_pengetahuan_non_uas.Trim() != "")
                                                {
                                                    if (struktur_nilai_kurtilas != null)
                                                    {
                                                        if (struktur_nilai_kurtilas.TahunAjaran != null)
                                                        {
                                                            s_nilai_rapor
                                                                = "ROUND(" +
                                                                       "IF(" +
                                                                            "ROUND(" +
                                                                              "AVERAGE(" + s_formula_rapor_pengetahuan_non_uas + ")*" +
                                                                              struktur_nilai_kurtilas.BobotRaporPengetahuan.ToString() + "%" +
                                                                              ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA_2DES +
                                                                            ") + " +
                                                                            (
                                                                                s_formula_rapor_pengetahuan_uas.Trim() != ""
                                                                                ? "ROUND(" +
                                                                                      "AVERAGE(" + s_formula_rapor_pengetahuan_uas + ")*" +
                                                                                      struktur_nilai_kurtilas.BobotRaporUAS.ToString() + "%" +
                                                                                      ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA_2DES +
                                                                                  ") "
                                                                                : " 0 "
                                                                            ) +
                                                                            "= 0, \"\", " +
                                                                            "ROUND(" +
                                                                              "AVERAGE(" + s_formula_rapor_pengetahuan_non_uas + ")*" +
                                                                              struktur_nilai_kurtilas.BobotRaporPengetahuan.ToString() + "%" +
                                                                              ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA_2DES +
                                                                            ") + " +
                                                                            (
                                                                                s_formula_rapor_pengetahuan_uas.Trim() != ""
                                                                                ? "ROUND(" +
                                                                                      "AVERAGE(" + s_formula_rapor_pengetahuan_uas + ")*" +
                                                                                      struktur_nilai_kurtilas.BobotRaporUAS.ToString() + "%" +
                                                                                      ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA_2DES +
                                                                                  ") "
                                                                                : " 0 "
                                                                            ) +
                                                                      "), " +
                                                                      Constantas.PEMBULATAN_DESIMAL_NILAI_SMA +
                                                                  ")";

                                                            s_nilai_rapor
                                                                = (
                                                                    s_nilai_rapor.Replace("#", (id + id_jml_fixed_row).ToString()).Trim() != ""
                                                                    ? "=" : ""
                                                                  ) +
                                                                  s_nilai_rapor.Replace("#", (id + id_jml_fixed_row).ToString());
                                                        }
                                                    }
                                                }

                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack " + "\", readOnly: true }";
                                            }
                                            else if (i == id_col_rapor_keterampilan_nilai)
                                            {
                                                //parse formula rapor keterampilan
                                                if (s_formula_rapor_keterampilan.Trim() != "")
                                                {
                                                    if (struktur_nilai_kurtilas != null)
                                                    {
                                                        if (struktur_nilai_kurtilas.TahunAjaran != null)
                                                        {
                                                            s_nilai_rapor
                                                                = "ROUND(" +
                                                                    "IF(" +
                                                                        "ROUND(" +
                                                                          "AVERAGE(" + s_formula_rapor_keterampilan + ")" +
                                                                          ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA_2DES +
                                                                        ") = 0, \"\", " +
                                                                        "ROUND(" +
                                                                          "AVERAGE(" + s_formula_rapor_keterampilan + ")" +
                                                                          ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA_2DES +
                                                                        ")" +
                                                                    "), " +
                                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SMA +
                                                                   ")";

                                                            s_nilai_rapor
                                                                = (
                                                                    s_nilai_rapor.Replace("#", (id + id_jml_fixed_row).ToString()).Trim() != ""
                                                                    ? "=" : ""
                                                                  ) +
                                                                  s_nilai_rapor.Replace("#", (id + id_jml_fixed_row).ToString());
                                                        }
                                                    }
                                                }

                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack " + "\", readOnly: true }";
                                            }
                                            else if (i == id_col_rapor_pengetahuan_predikat)
                                            {
                                                s_nilai_rapor = formula_blueprint_predikat.Replace(
                                                                    "@", Libs.GetColHeader(i)
                                                                ).Replace(
                                                                    "#", (id + id_jml_fixed_row).ToString()
                                                                );

                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack " + "\", readOnly: true }";
                                            }
                                            else if (i == id_col_rapor_keterampilan_predikat)
                                            {
                                                s_nilai_rapor = formula_blueprint_predikat.Replace(
                                                                    "@", Libs.GetColHeader(i)
                                                                ).Replace(
                                                                    "#", (id + id_jml_fixed_row).ToString()
                                                                );

                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack " + "\", readOnly: true }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 1))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_HD != null ? m_nilai_siswa.LTS_HD.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 2))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_MAX_HD != null ? m_nilai_siswa.LTS_MAX_HD.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 3))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_LK != null ? m_nilai_siswa.LTS_LK.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 4))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_LK != null ? m_nilai_siswa.LTS_RJ.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 5))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_RPKB != null ? m_nilai_siswa.LTS_RPKB.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }

                                            //catatan kepribadian
                                            else if (i == (id_col_rapor_keterampilan_predikat + 6))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_CK_KEHADIRAN != null ? m_nilai_siswa.LTS_CK_KEHADIRAN.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 7))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_CK_KETEPATAN_WKT != null ? m_nilai_siswa.LTS_CK_KETEPATAN_WKT.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 8))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_CK_PENGGUNAAN_SRGM != null ? m_nilai_siswa.LTS_CK_PENGGUNAAN_SRGM.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            else if (i == (id_col_rapor_keterampilan_predikat + 9))
                                            {
                                                s_nilai_rapor = (m_nilai_siswa != null && m_nilai_siswa.LTS_CK_PENGGUNAAN_KMR != null ? m_nilai_siswa.LTS_CK_PENGGUNAAN_KMR.Replace(@"""", "").Replace("'", "") : "");
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack\", readOnly: false }";
                                            }
                                            //end catatan kepribadian

                                            else
                                            {
                                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                                  "'" + s_nilai_rapor + "'";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack " + "\", readOnly: true }";
                                            }
                                            //end kolom-kolom nilai rapor
                                        }
                                    }
                                }

                                s_content += (s_content.Trim() != "" ? ", " : "") +
                                             "[" +
                                                "\"" + id.ToString() + "\", " +
                                                "\"" + m_siswa.NISSekolah + "\", " +
                                                "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true), 3) + 
                                                    (
                                                        m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT 
                                                        ? "<label style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</label>"
                                                        : ""
                                                    ) +
                                                "\", " +
                                                "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                                (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                             "]";
                            }

                            //kolom style untuk fixed col header
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }," +
                                             "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }," +
                                             "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }," +
                                             "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }";

                            id++;
                        }

                        s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
                        s_arr_js_ap = "[" + s_arr_js_ap + "]";
                        s_arr_js_kd = "[" + s_arr_js_kd + "]";
                        s_arr_js_kp = "[" + s_arr_js_kp + "]";

                        if (s_content.Trim() != "") s_content = ", " + s_content;
                        if (s_merge_cells.Trim() != "") s_merge_cells = ", " + s_merge_cells;

                        string s_data = "var data_nilai = " +
                                        "[" +
                                            s_kolom +
                                            s_content +
                                        "];";

                        string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMA.NILAI_SISWA.ROUTE);
                        s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMA.NILAI_SISWA.DO_SAVE.FILE + "/Do");

                        string s_cols_nkd = "";
                        string s_cols_nap_pengetahuan = "";
                        string s_cols_nap_keterampilan = "";                        
                        
                        foreach (var item in lst_kolom_nilai_na_pengetahuan)
                        {
                            s_cols_nap_pengetahuan += (s_cols_nap_pengetahuan.Trim() != "" ? " || " : "") +
                                          "col === " + item.IdKolom;
                        }
                        foreach (var item in lst_kolom_nilai_na_keterampilan)
                        {
                            s_cols_nap_keterampilan += (s_cols_nap_keterampilan.Trim() != "" ? " || " : "") +
                                          "col === " + item.IdKolom;
                        }
                        foreach (var item in lst_kolom_nilai_nkd)
                        {
                            s_cols_nkd += (s_cols_nkd.Trim() != "" ? " || " : "") +
                                          "col === " + item.IdKolom;
                        }
                        
                        //init columns
                        string js_columns = "";
                        for (int i = 0; i < id_col_all_without_sikap; i++)
                        {
                            js_columns += (js_columns.Trim() != "" ? ", " : "") +
                                          "{}";
                                       
                        }
                        foreach (var js_var_sikap in lst_js_var_sikap)
                        {
                            js_columns += (js_columns.Trim() != "" ? ", " : "") +
                                          "{" +
                                            "type : 'dropdown', " +
                                            "source : " + js_var_sikap.Key +
                                          "}";
                        }
                        //end init columns

                        string js_no_kkm = " && (" +
                                               "parseInt(col) !== " + (id_col_rapor_keterampilan_predikat + 1).ToString() + " && " +
                                               "parseInt(col) !== " + (id_col_rapor_keterampilan_predikat + 2).ToString() + " && " +
                                               "parseInt(col) !== " + (id_col_rapor_keterampilan_predikat + 3).ToString() + " && " +
                                               "parseInt(col) !== " + (id_col_rapor_keterampilan_predikat + 4).ToString() + " && " +
                                               "parseInt(col) !== " + (id_col_rapor_keterampilan_predikat + 5).ToString() +
                                           " ) ";

                        string script = s_data +
                                        "var arr_s = " + s_arr_js_siswa + ";" +
                                        "var arr_ap = " + s_arr_js_ap + ";" +
                                        "var arr_kd = " + s_arr_js_kd + ";" +
                                        "var arr_kp = " + s_arr_js_kp + ";" +
                                        js_nilai_sikap +
                                        "var div_nilai = document.getElementById('div_nilai'), hot; " +
                                        "var container = $('#div_nilai');  " +
                                        "hot = new Handsontable(div_nilai, { " +
                                            "data: data_nilai, " +
                                            "colWidths: [35, 80, 300, 40 " + s_kolom_width + "], " +
                                            "rowHeaders: true, " +
                                            "colHeaders: true, " +
                                            "fixedColumnsLeft: 4, " +
                                            "fixedRowsTop: 3, " +
                                            "minSpareRows: 0, " +
                                            "startCols: 100, " +
                                            "startRows: 100, " +
                                            "colHeaders: false, " +
                                            "fontSize: 9, " +
                                            "contextMenu: false, " +
                                            "formulas: true, " +
                                            "fillHandle: false, " +
                                            "autoColumnSize : true," +
                                            "stretchH: 'all'," +
                                            "cells: function (row, col, prop) { " +
                                                "if(this.instance.getData().length != 0){ " +
                                                    "var cellProperties = {}; " +
                                                    "if(row >= " + id_jml_fixed_row + "){" +
                                                        "if((parseInt(col) > 3 " + js_no_kkm + " && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)) || this.instance.getData()[row][col] == 'BL'){ " +
                                                            "if((row + 2) % 2 === 0){ " +
                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + " +
                                                                                            "(" + txtKTSPColAP.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNAPThin ' : '') + " +
                                                                                            "(" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '') " +
                                                                                            "; " +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + " +
                                                                                            "(" + txtKTSPColAP.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNAPThin ' : '') + " +
                                                                                            "(" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '') " +
                                                                                            "; " +
                                                            "} " +
                                                        "} " +
                                                        "else if(parseInt(col) > 3 " + js_no_kkm + " && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){ " +
                                                            "if((row + 2) % 2 === 0){ " +
                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + " +
                                                                                            "(" + txtKTSPColAP.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNAPThin ' : '') + " +
                                                                                            "(" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '') " +
                                                                                            "; " +
                                                            "} else { " +
                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + " +
                                                                                            "(" + txtKTSPColAP.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNAPThin ' : '') + " +
                                                                                            "(" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '') " +
                                                                                            "; " +
                                                            "} " +
                                                        "} " +

                                                        ( //untuk nilai nk
                                                            s_cols_nkd.Trim() != ""
                                                            ? "if(" + s_cols_nkd + "){" +
                                                                    "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG3 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG4 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG3 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG4 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                              "}"
                                                            : ""
                                                        ) +

                                                        ( //untuk nilai na pengetahuan
                                                            s_cols_nap_pengetahuan.Trim() != ""
                                                            ? "if(" + s_cols_nap_pengetahuan + "){" +
                                                                    "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG5 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG6 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG5 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG6 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                              "}"
                                                            : ""
                                                        ) +

                                                        ( //untuk nilai na keterampilan
                                                            s_cols_nap_keterampilan.Trim() != ""
                                                            ? "if(" + s_cols_nap_keterampilan + "){" +
                                                                    "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG5 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG6 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG5 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG6 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                              "}"
                                                            : ""
                                                        ) +

                                                    "} " +
                                                    "return cellProperties; " +
                                                "} " +
                                            "}, " +
                                            "afterChange: function(changes, source) { " +
                                                "if (source === 'loadData') return; " +
                                                "var i_count = 0;" +
                                                "var i_index = 0;" +
                                                "var i_inc = 533;" +
                                                "SetProgressSaveDataValue(0); " +
                                                "$.each(changes, function(index, element) { " +
                                                    "i_count += i_inc;" +
                                                    "setTimeout(" +
                                                        "function() {" +
                                                            "i_index++;" +
                                                            "var i_proses = ((i_index / changes.length) * 100);" +
                                                            "SetProgressSaveDataValue(i_proses); " +
                                                            "if(i_count > i_inc) ShowProgressSaveData(Math.round(i_proses).toString() + '%');" +
                                                            "var row = element[0];" +
                                                            "var col = element[1];" +
                                                            "var oldVal = element[2];" +
                                                            "var newVal = element[3];" +

                                                            "var t = '" + Libs.GetQueryString("t") + "';" +
                                                            "var sm = '" + semester + "';" +
                                                            "var kdt = '" + rel_kelas_det.ToString() + "';" +
                                                            "var k = '" + rel_kelas.ToString() + "';" +
                                                            "var mp = '" + rel_mapel.ToString() + "';" +
                                                            "var s = arr_s[row];" +
                                                            "var ap = arr_ap[col];" +
                                                            "var kd = arr_kd[col];" +
                                                            "var kp = arr_kp[col];" +
                                                            "var n = data_nilai[row][col];" +

                                                            "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_rapor_pengetahuan_nilai.ToString() + "});" +
                                                            "var formula = hot.getDataAtCell(row, " + id_col_rapor_pengetahuan_nilai.ToString() + ");" +
                                                            "formula = formula.substr(1).toUpperCase();" +
                                                            "var newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_rapor_pengetahuan_nilai.ToString() + ", id: cellId});" +
                                                            "var nr_p = (newValue.result);" +

                                                            "cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_rapor_keterampilan_nilai.ToString() + "});" +
                                                            "formula = hot.getDataAtCell(row, " + id_col_rapor_keterampilan_nilai.ToString() + ");" +
                                                            "formula = formula.substr(1).toUpperCase();" +
                                                            "newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_rapor_keterampilan_nilai.ToString() + ", id: cellId});" +
                                                            "var nr_k = (newValue.result);" +

                                                            "cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_rapor_pengetahuan_predikat.ToString() + "});" +
                                                            "formula = hot.getDataAtCell(row, " + id_col_rapor_pengetahuan_predikat.ToString() + ");" +
                                                            "formula = formula.substr(1).toUpperCase();" +
                                                            "newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_rapor_pengetahuan_predikat.ToString() + ", id: cellId});" +
                                                            "var pnr_p = (newValue.result == null ? '' : newValue.result);" +

                                                            "cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_rapor_keterampilan_predikat.ToString() + "});" +
                                                            "formula = hot.getDataAtCell(row, " + id_col_rapor_keterampilan_predikat.ToString() + ");" +
                                                            "formula = formula.substr(1).toUpperCase();" +
                                                            "newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_rapor_keterampilan_predikat.ToString() + ", id: cellId});" +
                                                            "var pnr_k = (newValue.result == null ? '' : newValue.result);" +

                                                            "var lts_hd = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 1).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 1).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 1).ToString() + "]);" +
                                                            "var lts_hd_maks = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 2).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 2).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 2).ToString() + "]);" +
                                                            "var lts_lk = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 3).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 3).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 3).ToString() + "]);" +
                                                            "var lts_rj = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 4).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 4).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 4).ToString() + "]);" +
                                                            "var lts_rpkb = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 5).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 5).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 5).ToString() + "]);" +

                                                            "var lts_ck_hd = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 6).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 6).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 6).ToString() + "]);" +
                                                            "var lts_ck_kw = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 7).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 7).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 7).ToString() + "]);" +
                                                            "var lts_ck_ps = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 8).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 8).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 8).ToString() + "]);" +
                                                            "var lts_ck_pk = (data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 9).ToString() + "] === undefined || data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 9).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_rapor_keterampilan_predikat + 9).ToString() + "]);" +

                                                            (
                                                                "var s_url = '" + s_url_save + "' + " +
                                                                            "'?' + " +
                                                                            "'j=' + '" + Libs.Enkrip(Libs.JenisKurikulum.SMA.KURTILAS) + "' + " +
                                                                            "'&t=' + t + " +
                                                                            "'&sm=' + sm + " +
                                                                            "'&kdt=' + kdt + " +
                                                                            "'&s=' + s + " +
                                                                            "'&n=' + n + " +
                                                                            "'&ap=' + ap + " +
                                                                            "'&kd=' + kd + " +
                                                                            "'&kp=' + kp + " +
                                                                            "'&mp=' + mp + " +
                                                                            "'&k=' + k + " +
                                                                            "'&nr_ppk=' + " +
                                                                            "'&nr_p=' + nr_p + " +
                                                                            "'&nr_k=' + nr_k + " +
                                                                            "'&nr_prd=' + " +
                                                                            "'&ssid=" + Libs.Enkrip(Libs.LOGGED_USER_M.NoInduk) + "' + " +
                                                                            "'&pnr_p=' + pnr_p + " +
                                                                            "'&pnr_k=' + pnr_k + " +
                                                                            "'&lts_hd=' + lts_hd + " +
                                                                            "'&lts_maxhd=' + lts_hd_maks + " +
                                                                            "'&lts_lk=' + lts_lk + " +
                                                                            "'&lts_rj=' + lts_rj + " +
                                                                            "'&lts_rpkb=' + lts_rpkb + " +
                                                                            "'&lts_ck_hd=' + lts_ck_hd + " +
                                                                            "'&lts_ck_kw=' + lts_ck_kw + " +
                                                                            "'&lts_ck_ps=' + lts_ck_ps + " +
                                                                            "'&lts_ck_pk=' + lts_ck_pk " +
                                                                            ";" +

                                                                  "$.ajax({" +
                                                                        "url: s_url, " +
                                                                        "dataType: 'json', " +
                                                                        "type: 'GET', " +
                                                                        "contentType: 'application/json; charset=utf-8', " +
                                                                        "success: function(data) { " +
                                                                                "" +
                                                                            "}, " +
                                                                        "error: function(response) { " +
                                                                                "HideProgressSaveData(); " +
                                                                                "alert(response.responseText); " +
                                                                            "}, " +
                                                                        "failure: function(response) { " +
                                                                                "alert(response.responseText); " +
                                                                            "} " +
                                                                    "}); "
                                                            ) +
                                                        "}, i_count " +
                                                    "); " +
                                                "}); " +
                                                "this.render(); " +
                                            "}, " +
                                            "columns: [" + js_columns + "], " +
                                            "mergeCells: [ " +
                                                "{ row: 0, col: 0, rowspan: 3, colspan: 1 }, " +
                                                "{ row: 0, col: 1, rowspan: 3, colspan: 1 }, " +
                                                "{ row: 0, col: 2, rowspan: 3, colspan: 1 }, " +
                                                "{ row: 0, col: 3, rowspan: 3, colspan: 1 } " +
                                                s_merge_cells +
                                            "] " +
                                            (
                                                s_kolom_style.Trim() != ""
                                                ? ", cell: [" + s_kolom_style + "]"
                                                : ""
                                            ) +
                                        "}); " +
                                        "function calculateSize() {" +
                                            "var offset;" +

                                            "offset = Handsontable.dom.offset(div_nilai);" +
                                            "availableWidth = Handsontable.dom.innerWidth(document.body) - offset.left + window.scrollX;" +
                                            "availableHeight = Handsontable.dom.innerHeight(document.body) - offset.top + window.scrollY;" +

                                            "div_nilai.style.width = availableWidth + 'px';" +
                                            "div_nilai.style.height = availableHeight + 'px';" +
                                        "}" +
                                        "function Maximize() {" +
                                            "calculateSize();" +
                                            "hot.render();" +
                                        "}" +
                                        "container.find('table').addClass('zebraStyle2');" +
                                        "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_all.ToString() + "); " +
                                        "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_mulai.ToString() + "); ";

                        if (js_statistik.Trim() != "") {
                            js_statistik = "var arr_col = [" + js_statistik + "], ";
                            js_statistik += "id_fixed_row = " + (id_jml_fixed_row).ToString() + ", ";
                            js_statistik += "id_fixed_col = " + id_col_nilai_mulai.ToString() + ";";
                        }

                        ltrJSStatistik.Text = js_statistik;
                        ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";
                    }
                }
            }

        }

        protected void LoadDataEkskul(bool is_readonly)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";

            string tahun_ajaran = AtributPenilaian.TahunAjaran;
            string semester = AtributPenilaian.Semester;
            string rel_kelas = AtributPenilaian.Kelas;
            string rel_kelas_det = AtributPenilaian.KelasDet;
            string rel_mapel = AtributPenilaian.Mapel;

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            //status bar
            ltrStatusBar.Text = "";
            if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
            {
                ltrStatusBar.Text += "&nbsp;" +
                                     "<span style=\"font-weight: bold; color: red;\">" +
                                        "<i class=\"fa fa-exclamation-triangle\"></i>" +
                                        "&nbsp;" +
                                        "PREVIEW INPUT" +
                                        "&nbsp;" +
                                     "</span>&nbsp;";
            }

            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null)
            {
                if (m_mapel.Nama != null)
                {
                    ltrStatusBar.Text += "<span style=\"font-weight: bold;\">" + m_mapel.Nama + "</span>";
                }
            }

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                         "&nbsp;" +
                                         "<span style=\"font-weight: normal;\">" + m_kelas_det.Nama + "</span>";
                }
            }

            if (tahun_ajaran.Trim() != "")
            {
                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                     "&nbsp;" +
                                     "<span style=\"font-weight: normal;\">" + tahun_ajaran + "</span>";
            }

            if (semester.Trim() != "")
            {
                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;Sm." : "") +
                                     "&nbsp;" +
                                     "<span style=\"font-weight: normal;\">" + semester + "</span>";
            }
            //end status bar

            string s_kolom = "";
            string s_kolom_width = "";
            string s_kolom_style = "";
            string s_content = "";
            string s_merge_cells = "";
            string s_formula = "";
            string s_formula_rapor_ppk = "";
            string s_formula_rapor_p = "";

            int i_col_nilai_rapor_ppk = 0;
            int i_col_nilai_rapor_p = 0;
            int i_col_nilai_rapor_predikat = 0;

            string s_arr_js_siswa = "";
            string s_arr_js_kd = "";
            string s_arr_js_kp = "";

            string css_bg = "#fff";
            string css_bg_nkd = "#fff";
            string css_bg_nilaiakhir = "#fff";

            bool is_nkd = false;
            bool ada_praktik = false;

            List<NILAI_COL> lst_nkd = new List<NILAI_COL>();
            List<string> lst_kd = new List<string>();
            List<string> lst_kp = new List<string>();
            List<FORMULA_RAPOR_KTSP> lst_col_rapor_ppk = new List<FORMULA_RAPOR_KTSP>();
            List<FORMULA_RAPOR_KTSP> lst_col_rapor_p = new List<FORMULA_RAPOR_KTSP>();

            List<int> lst_col_terakhir_nkd = new List<int>();

            int id_col = 0;
            int id_col_all = 0;
            int id_col_akhir = 0;
            int id_col_nkd = 0;
            int id_col_nilai_mulai = 4;
            int id_jml_fixed_row = 2;

            string js_statistik = "";

            //struktur nilai
            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, rel_kelas, rel_mapel
                );

            if (lst_stuktur_nilai.Count == 1)
            {
                string s_js_arr_kolom1 = "";
                string s_js_arr_kolom2 = "";
                string s_js_arr_nilai = "";

                lst_nkd.Clear();
                lst_col_terakhir_nkd.Clear();

                //parse struktur KTSP
                DAO_Rapor_StrukturNilai.StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();

                //kurikulum KTSP
                if (struktur_nilai.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                {
                    if (is_readonly)
                    {
                        ltrStatusBar.Text += "&nbsp;&nbsp;" +
                                             "<span title=\" Data Terkunci/Tidak Bisa Di-edit \" style=\"font-weight: bold; yellow: red;\"><i class=\"fa fa-lock\"></i>&nbsp;Data Terkunci</span>";
                        ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: white;\"></i>" +
                                            "&nbsp;&nbsp;" +
                                            ltrStatusBar.Text;
                        div_statusbar.Attributes["style"] = "color: white; height: 40px; background-color: #ce3584; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
                    }
                    else
                    {
                        ltrStatusBar.Text += "&nbsp;&nbsp;" +
                                             "<span title=\" Data Bisa Di-edit \" style=\"font-weight: bold; color: green;\"><i class=\"fa fa-unlock\"></i></span>";
                        ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: green;\"></i>" +
                                            "&nbsp;&nbsp;" +
                                            ltrStatusBar.Text;
                        div_statusbar.Attributes["style"] = "color: black; height: 40px; background-color: #eeeeee; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
                    }

                    if (struktur_nilai.KKM > 0)
                    {
                        ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;KKM :" : "") +
                                             "&nbsp;" +
                                             "<span style=\"font-weight: bold;\">" + Math.Round(struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) + "</span>";

                        txtKKM.Value = Math.Round(struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString();
                    }

                    id_col = id_col_nilai_mulai;
                    id_col_all = id_col_nilai_mulai;

                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();
                    if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
                    {
                        lst_siswa = DAO_FormasiEkskulDet.GetListSiswaEkskul(
                                struktur_nilai.TahunAjaran,
                                struktur_nilai.Semester,
                                struktur_nilai.Rel_Mapel.ToString()
                            );
                    }
                    else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
                    {
                        lst_siswa = GetListSiswaSimulasi();
                    }
                    else
                    {
                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                            m_kelas.Rel_Sekolah.ToString(),
                            rel_kelas_det,
                            QS.GetTahunAjaran(),
                            QS.GetSemester()
                        );
                        if (lst_siswa.Count == 0)
                        {
                            lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            QS.GetTahunAjaran(),
                                            QS.GetSemester()
                                        );
                        }
                    }

                    //kompetensi dasar
                    List<Rapor_StrukturNilai_KTSP_KD> lst_kompetensi_dasar =
                        DAO_Rapor_StrukturNilai_KTSP_KD.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: 0, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                     "{ row: 1, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                     "{ row: 0, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                     "{ row: 1, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                     "{ row: 0, col: 2, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                     "{ row: 1, col: 2, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                    //init js arr kompetensi dasar/kd
                    s_arr_js_kd = "";
                    lst_kd.Clear();
                    for (int i = 0; i < id_col_nilai_mulai; i++)
                    {
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");
                    }
                    //end init

                    //init js arr komponen penilaian/kp
                    s_arr_js_kp = "";
                    lst_kp.Clear();
                    for (int i = 0; i < id_col_nilai_mulai; i++)
                    {
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                    }
                    //end init

                    foreach (var m_sn_kd in lst_kompetensi_dasar)
                    {
                        //komponen penilaian
                        List<Rapor_StrukturNilai_KTSP_KP> lst_komponen_penilaian =
                            DAO_Rapor_StrukturNilai_KTSP_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                        Rapor_KompetensiDasar m_kd =
                                DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());

                        if (m_kd != null)
                        {
                            if (m_kd.Nama != null)
                            {
                                if (lst_komponen_penilaian.Count > 0)
                                {
                                    id_col_nkd = id_col_all;

                                    foreach (var m_sn_kp in lst_komponen_penilaian)
                                    {
                                        Rapor_KomponenPenilaian m_kp =
                                            DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());

                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + m_sn_kd.Kode.ToString() + "\"";
                                                lst_kd.Add(m_sn_kd.Kode.ToString());

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + m_sn_kp.Kode.ToString() + "\"";
                                                lst_kp.Add(m_sn_kp.Kode.ToString());
                                                //end add kd & kp

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                  "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                  "\"" + 
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama) + 
                                                                        (
                                                                            m_sn_kp.BobotNKD > 0
                                                                            ? "<br />" +
                                                                              "<sup class='badge' style='background-color: #B7770D; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_sn_kp.BobotNKD, 0).ToString() + "%" +
                                                                              "</sup>"
                                                                            : ""
                                                                        ) +
                                                                  "\" ";

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                  LEBAR_COL_DEFAULT;

                                                if (m_sn_kp.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PRAKTIK)
                                                {
                                                    lst_col_rapor_p.Add(new FORMULA_RAPOR_KTSP
                                                    {
                                                        IdKolom = (id_col_all + 1),
                                                        Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                                        Kode_SN_KP = m_sn_kp.Kode.ToString(),
                                                        KolomHeadersFormula = "IF(" + Libs.GetColHeader((id_col_all + 1)) + "# = \"\", 0, " +
                                                                                      Libs.GetColHeader((id_col_all + 1)) + "#" +
                                                                                ")"
                                                    });

                                                    ada_praktik = true;
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontRed\", readOnly: true, renderer: \"html\" }";
                                                }
                                                else
                                                {
                                                    if (lst_komponen_penilaian.FindAll(m => m.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK).Count == 1)
                                                    {
                                                        lst_col_rapor_ppk.Add(new FORMULA_RAPOR_KTSP
                                                        {
                                                            IdKolom = id_col_all,
                                                            Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                                            Kode_SN_KP = m_sn_kp.Kode.ToString(),
                                                            KolomHeadersFormula = Libs.GetColHeader(id_col_all) + "#"
                                                        });
                                                    }

                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                }

                                                id_col_all++;
                                            }
                                        }
                                    }

                                    //cek n-kd jika lebih dari 1 buat kolom tambahan n-KD
                                    //ini kolom n-kd nya
                                    if (lst_komponen_penilaian.FindAll(m => m.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK).Count > 1)
                                    {
                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                        (id_col_all).ToString();

                                        //add kd to var arr js
                                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                       "\"\"";
                                        lst_kd.Add("");

                                        //add kp to var arr js
                                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                       "\"\"";
                                        lst_kp.Add("");
                                        //end add kd & kp

                                        //pasang formula
                                        //generate formula
                                        s_formula = "";
                                        foreach (var m_kp_ktsp in lst_komponen_penilaian)
                                        {
                                            if (m_kp_ktsp.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK)
                                            {
                                                if (m_kp_ktsp.BobotNKD > 0)
                                                {
                                                    s_formula += (s_formula.Trim() != "" ? "+" : "") +
                                                                 "(" +
                                                                    "IF(" +
                                                                        Libs.GetColHeader(id_col_nkd + 1) + "# = \"\"" +
                                                                        ", 0, " + Libs.GetColHeader(id_col_nkd + 1) + "#" +
                                                                    ")" +
                                                                    "*(" + (m_kp_ktsp.BobotNKD.ToString()) + "%)" +
                                                                 ")";
                                                }
                                            }

                                            id_col_nkd++;
                                        }
                                        if (s_formula.Trim() != "")
                                        {
                                            s_formula = "IF(" +
                                                            "ROUND(" +
                                                                "(" +
                                                                    s_formula +
                                                                "), " +
                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() +
                                                            ") = 0 " +
                                                        ", \"\", " +
                                                            "ROUND(" +
                                                                "(" +
                                                                    s_formula +
                                                                "), " +
                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() +
                                                            ")" +
                                                        ")";
                                        }
                                        //end pasang formula

                                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                    "{" +
                                                    "  row: 0, col: " + id_col + ", rowspan: 1, " +
                                                    "  colspan: " + (lst_komponen_penilaian.Count + 1).ToString() +
                                                    "} ";

                                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                           "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                          "\"" + NKD + "\" ";

                                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                          LEBAR_COL_DEFAULT;

                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                        //add nkd
                                        lst_nkd.Add(
                                                new NILAI_COL
                                                {
                                                    IdKolom = id_col + lst_komponen_penilaian.Count,
                                                    BluePrintFormula = s_formula
                                                }
                                            );

                                        lst_col_rapor_ppk.Add(new FORMULA_RAPOR_KTSP
                                        {
                                            IdKolom = id_col_all,
                                            Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                            Kode_SN_KP = "",
                                            KolomHeadersFormula = Libs.GetColHeader(id_col_all) + "#"
                                        });

                                        id_col_all++;
                                        id_col += (lst_komponen_penilaian.Count + 1);
                                    }
                                    else
                                    {
                                        s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                    "{" +
                                                    "  row: 0, col: " + id_col + ", rowspan: 1, " +
                                                    "  colspan: " + lst_komponen_penilaian.Count.ToString() +
                                                    "} ";

                                        id_col += lst_komponen_penilaian.Count;

                                        //index colom terakhir nkd
                                        lst_col_terakhir_nkd.Add(id_col_all - 1);
                                        txtKTSPColKD.Value += ";" + (id_col_all - 1) + ";";
                                    }
                                    //end cek n-kd
                                }
                                else
                                {
                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                    (id_col_all).ToString();

                                    //add kd to var arr js
                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                   "\"" + m_sn_kd.Kode.ToString() + "\"";
                                    lst_kd.Add(m_sn_kd.Kode.ToString());

                                    //add kp to var arr js
                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                   "\"\"";
                                    lst_kp.Add("");
                                    //end add kd & kp

                                    s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                 "{" +
                                                 "  row: 0, col: " + id_col + ", rowspan: 2, " +
                                                 "  colspan: 1 " +
                                                 "} ";

                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                              "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                              "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";

                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                      LEBAR_COL_DEFAULT;

                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                    //index colom terakhir nkd
                                    lst_col_terakhir_nkd.Add(id_col_all);
                                    txtKTSPColKD.Value += ";" + (id_col_all) + ";";

                                    lst_col_rapor_ppk.Add(new FORMULA_RAPOR_KTSP
                                    {
                                        IdKolom = id_col_all,
                                        Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                        Kode_SN_KP = "",
                                        KolomHeadersFormula = Libs.GetColHeader(id_col_all) + "#"
                                    });

                                    id_col_all++;
                                    id_col++;
                                }
                            }
                        }
                    }

                    //generate formula rapor
                    s_formula_rapor_ppk = "";
                    s_formula_rapor_p = "";
                    int jml_pembagi = 0;

                    //parse formula rapor ppk
                    foreach (FORMULA_RAPOR_KTSP formula_ppk in lst_col_rapor_ppk)
                    {
                        Rapor_StrukturNilai_KTSP_KD r_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(formula_ppk.Kode_SN_KD);
                        if (r_kd != null)
                        {
                            if (r_kd.Poin != null)
                            {
                                jml_pembagi++;
                                s_formula_rapor_ppk += (s_formula_rapor_ppk.Trim() != "" ? "+" : "") +
                                                       "(" +
                                                            "IF(" +
                                                                Libs.GetColHeader(formula_ppk.IdKolom + 1) + "# " +
                                                                "= \"\", 0, " +
                                                                Libs.GetColHeader(formula_ppk.IdKolom + 1) + "# " +
                                                            ")" +
                                                            "*" +
                                                            Math.Round(r_kd.BobotRaporPPK, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) +
                                                            "%" +
                                                        ")";
                            }
                        }
                    }
                    if (s_formula_rapor_ppk.Trim() != "")
                        s_formula_rapor_ppk = "=IF(ROUND((" + s_formula_rapor_ppk + ")/" + jml_pembagi.ToString() + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")<=0, \"\", " +
                                                  "ROUND((" + s_formula_rapor_ppk + ")/" + jml_pembagi.ToString() + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")" + ")";

                    //parse formula rapor praktik
                    jml_pembagi = 0;
                    foreach (string s_kd_p in lst_col_rapor_p.Select(m => m.Kode_SN_KD).Distinct().ToList())
                    {
                        string s_item_formula_rapor_p = "";
                        foreach (var s_kp_p in lst_col_rapor_p.FindAll(m => m.Kode_SN_KD == s_kd_p).OrderBy(m => m.IdKolom).ToList())
                        {
                            s_item_formula_rapor_p += (s_item_formula_rapor_p.Trim() != "" ? "+" : "") +
                                                      s_kp_p.KolomHeadersFormula;
                        }
                        s_item_formula_rapor_p = "(" + s_item_formula_rapor_p + ")/" +
                                                 lst_col_rapor_p.FindAll(m => m.Kode_SN_KD == s_kd_p).OrderBy(m => m.IdKolom).ToList().Count.ToString();

                        Rapor_StrukturNilai_KTSP_KD r_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(s_kd_p);
                        if (r_kd != null)
                        {
                            if (r_kd.Poin != null)
                            {
                                jml_pembagi++;
                                s_formula_rapor_p += (s_formula_rapor_p.Trim() != "" ? "+" : "") +
                                                     "(" +
                                                         "(" + s_item_formula_rapor_p + ")" +
                                                         "*" +
                                                         Math.Round(r_kd.BobotRaporP, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) +
                                                         "%" +
                                                     ")";
                            }
                        }
                    }

                    if (s_formula_rapor_p.Trim() != "")
                        s_formula_rapor_p = "=IF(ROUND(" + s_formula_rapor_p + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")<=0, \"\", " +
                                                "ROUND(" + s_formula_rapor_p + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")" + ")";

                    //end generate formula rapor
                    string s_js_nilai_rapor_ktsp_ppk = "";
                    string s_js_nilai_rapor_ktsp_p = "";
                    string s_js_nilai_rapor_ktsp_predikat = "";

                    //kolom header akhir (kolom nilai rapor)
                    if (!ada_praktik) //tidak ada nilai praktik
                    {
                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"NILAI RAPOR\", " +
                                           "\"NILAI RAPOR\"";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                           "\"TOTAL\", " +
                                           "\"PREDIKAT\"";

                        if (s_merge_cells.Trim() != "") s_merge_cells += ", " +
                                                    "{ row: 0, col: " + id_col_all + ", rowspan: 1, colspan: 2 } ";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                         "{ row: 1, col: " + (id_col_all + 1).ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true } ";

                        //col width untuk 2 kolom
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT3;

                        s_js_nilai_rapor_ktsp_ppk = "'" + s_formula_rapor_ppk + "'";
                        s_js_nilai_rapor_ktsp_predikat = "'" + GetFormulaPredikatDeskripsiKTSP(struktur_nilai.Kode.ToString()) + "'";

                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                        (id_col_all).ToString();

                        //col width untuk 2 kolom
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"LTS\" ";
                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                           "\"HD\" ";

                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + (id_col_all + 2).ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                         "{ row: 1, col: " + (id_col_all + 2).ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true } ";
                    }
                    //end kolom header akhir

                    if (s_js_arr_kolom1.Trim() != "") s_js_arr_kolom1 = ", " + s_js_arr_kolom1;
                    if (s_js_arr_kolom2.Trim() != "") s_js_arr_kolom2 = ", " + s_js_arr_kolom2;
                    if (s_kolom_width.Trim() != "") s_kolom_width = ", " + s_kolom_width;

                    s_kolom = "[" +
                                  "\"#\", " +
                                  "\"NIS\", " +
                                  "\"NAMA SISWA\", " +
                                  "\"L/P\" " +
                                  s_js_arr_kolom1 +
                              "], " +
                              "[" +
                                  "\"#\", " +
                                  "\"NIS\", " +
                                  "\"NAMA SISWA\", " +
                                  "\"L/P\" " +
                                  s_js_arr_kolom2 +
                              "]";

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: 0, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }," +
                                     "{ row: 1, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }";

                    //list siswa dan nilainya
                    //get list nilai jika ada
                    Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas_det, rel_mapel
                        ).FirstOrDefault();
                    List<Rapor_NilaiSiswa_KTSP> lst_nilai_siswa = null;
                    List<Rapor_NilaiSiswa_KTSP_Det> lst_nilai_siswa_det = null;
                    if (m_nilai != null)
                    {
                        if (m_nilai.Kurikulum != null)
                        {
                            lst_nilai_siswa = DAO_Rapor_NilaiSiswa_KTSP.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                            lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_KTSP_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                        }
                    }

                    //init js arr siswa
                    s_arr_js_siswa = "";
                    for (int i = 0; i < id_jml_fixed_row; i++)
                    {
                        s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                          "\"\"";
                    }

                    int id = 1;
                    foreach (Siswa m_siswa in lst_siswa)
                    {
                        string kelas_det = "";
                        m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                kelas_det = m_kelas_det.Nama;
                            }
                        }

                        css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                        css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                        css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                        s_js_arr_nilai = "";

                        s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                          "\"" + m_siswa.Kode.ToString() + "\"";

                        if (id_col_all > id_col_nilai_mulai)
                        {
                            for (int i = id_col_nilai_mulai; i < id_col_all; i++)
                            {
                                is_nkd = false;
                                s_formula = "";
                                foreach (NILAI_COL item_nkd in lst_nkd)
                                {
                                    if (i == item_nkd.IdKolom)
                                    {
                                        is_nkd = true;
                                        //parse formula
                                        s_formula = (
                                                        item_nkd.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()).Trim() != ""
                                                        ? "=" : ""
                                                    ) +
                                                    item_nkd.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                        break;
                                    }
                                }

                                if (is_nkd)
                                {
                                    //nkd per kompetensi dasar
                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_formula + "'";

                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + 2) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nkd + " htFontBlack htBorderRightNKD\", readOnly: true }";
                                }
                                else
                                {
                                    //---get nilainya disini
                                    Rapor_NilaiSiswa_KTSP_Det m_nilai_siswa_det_ktsp =
                                        (
                                            lst_nilai_siswa_det != null
                                            ?
                                            lst_nilai_siswa_det.FindAll(
                                                m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                     m.Rel_Rapor_StrukturNilai_KTSP_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KTSP_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                            ).FirstOrDefault()
                                            : null
                                        );

                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" +
                                                        (
                                                            m_nilai_siswa_det_ktsp != null
                                                            ? (
                                                                m_nilai_siswa_det_ktsp.Nilai != null
                                                                ? m_nilai_siswa_det_ktsp.Nilai.Replace(@"""", "").Replace("'", "").Replace("\\", "")
                                                                : ""
                                                              )
                                                            : ""
                                                        ) +
                                                      "'";
                                    //---end get nilai

                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " +
                                                     (lst_col_terakhir_nkd.FindAll(m => m == i).Count > 0 ? "htBorderRightNKD" : "") + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + "}";
                                }
                            }
                        }

                        //cell untuk nilai rapor & predikatnya
                        s_js_arr_nilai += ((s_js_arr_nilai.Trim() != "" && s_js_nilai_rapor_ktsp_ppk.Trim() != "" ? ", " : "") + s_js_nilai_rapor_ktsp_ppk.Replace("#", (id + id_jml_fixed_row).ToString())) +
                                          ((s_js_arr_nilai.Trim() != "" && s_js_nilai_rapor_ktsp_p.Trim() != "" ? ", " : "") + s_js_nilai_rapor_ktsp_p.Replace("#", (id + id_jml_fixed_row).ToString())) +
                                          (
                                            (s_js_arr_nilai.Trim() != "" && s_js_nilai_rapor_ktsp_predikat.Trim() != "" ? ", " : "") +
                                             s_js_nilai_rapor_ktsp_predikat.Replace("@", Libs.GetColHeader(id_col_all + 1)).
                                                                            Replace("#", (id + id_jml_fixed_row).ToString())
                                          );
                        //end cell nilai rapor

                        //cell untuk nilai perilaku belajar HD
                        if (lst_nilai_siswa != null)
                        {
                            if (lst_nilai_siswa.Count > 0)
                            {
                                if (lst_nilai_siswa.FindAll(m => m.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper()).Count > 0)
                                {
                                    Rapor_NilaiSiswa_KTSP m_ns = lst_nilai_siswa.FindAll(m => m.Rel_Siswa.ToString().ToUpper() == m_siswa.Kode.ToString().ToUpper()).FirstOrDefault();
                                    if (m_ns != null)
                                    {
                                        if (m_ns.Rel_Siswa != null)
                                        {
                                            s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") + "'" + m_ns.LTS_HD.Replace(@"""", "").Replace("'", "").Replace("'", "\\'") + "'";
                                        }
                                        else
                                        {
                                            s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") + "''";
                                        }
                                    }
                                    else
                                    {
                                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") + "''";
                                    }
                                }
                                else
                                {
                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") + "''";
                                }
                            }
                            else
                            {
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") + "''";
                            }
                        }
                        else
                        {
                            s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") + "''";
                        }
                        //end cell untuk nilai perilaku belajar HD

                        //style col akhir
                        id_col_akhir = id_col_all;
                        if (s_js_nilai_rapor_ktsp_ppk.Trim() != "") //rapor ppk
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_akhir.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBold htFontBlack\", readOnly: true }";

                            i_col_nilai_rapor_ppk = id_col_akhir;
                            id_col_akhir++;
                        }
                        if (s_js_nilai_rapor_ktsp_p.Trim() != "") //rapor p
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_akhir.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBold htFontBlack\", readOnly: true }";

                            i_col_nilai_rapor_p = id_col_akhir;
                            id_col_akhir++;
                        }
                        if (s_js_nilai_rapor_ktsp_predikat.Trim() != "") //rapor predikat
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_akhir.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBold htFontBlack\", readOnly: true }";

                            i_col_nilai_rapor_predikat = id_col_akhir;
                            id_col_akhir++;
                        }
                        //end cell untuk nilai rapor & predikatnya

                        //untuk rapor kepribadian
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                        "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_akhir).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack htCenter " +
                                        "\", readOnly: false }";
                        id_col_akhir++;
                        //end untuk rapor kepribadian

                        s_content += (s_content.Trim() != "" ? ", " : "") +
                                     "[" +
                                        "\"" + id.ToString() + "\", " +
                                        "\"" + m_siswa.NISSekolah + "\", " +
                                        "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true), 3) + "<label style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</label>\", " +
                                        "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                        (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                     "]";

                        //kolom style untuk fixed col header
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }," +
                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                        id++;
                    }
                    //end list siswa dan nilainya

                    s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
                    s_arr_js_kd = "[" + s_arr_js_kd + "]";
                    s_arr_js_kp = "[" + s_arr_js_kp + "]";

                    if (s_content.Trim() != "") s_content = ", " + s_content;
                    if (s_merge_cells.Trim() != "") s_merge_cells = ", " + s_merge_cells;

                    string s_data = "var data_nilai = " +
                                        "[" +
                                            s_kolom +
                                            s_content +
                                        "];";

                    string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMA.NILAI_SISWA.ROUTE);
                    s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMA.NILAI_SISWA.DO_SAVE.FILE + "/Do");

                    string script = s_data +
                                    "var arr_s = " + s_arr_js_siswa + ";" +
                                    "var arr_kd = " + s_arr_js_kd + ";" +
                                    "var arr_kp = " + s_arr_js_kp + ";" +
                                    "var div_nilai = document.getElementById('div_nilai'), hot; " +
                                    "var container = $('#div_nilai');  " +
                                    "hot = new Handsontable(div_nilai, { " +
                                        "data: data_nilai, " +
                                        "colWidths: [35, 80, 300, 40 " + s_kolom_width + "], " +
                                        "rowHeaders: true, " +
                                        "colHeaders: true, " +
                                        "fixedColumnsLeft: 4, " +
                                        "fixedRowsTop: 2, " +
                                        "minSpareRows: 0, " +
                                        "colHeaders: false, " +
                                        "fontSize: 9, " +
                                        "contextMenu: false, " +
                                        "formulas: true," +
                                        "fillHandle: false," +
                                        "cells: function (row, col, prop) {" +
                                          "if(this.instance.getData().length != 0){" +
                                            "var cellProperties = {};" +
                                            "if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                "if((row + 1) % 2 === 0){" +
                                                    "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                "} else {" +
                                                    "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                "} " +
                                            "} " +
                                            "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                "if((row + 1) % 2 === 0){" +
                                                    "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                "} else {" +
                                                    "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                "} " +
                                            "} " +
                                            "return cellProperties;" +
                                          "}" +
                                        "}," +
                                        "afterChange: function(changes, source) {" +
                                            "if (source === 'loadData') return;" +
                                            "$.each(changes, function(index, element) {" +
                                                "var row = element[0];" +
                                                "var col = element[1];" +
                                                "var oldVal = element[2];" +
                                                "var newVal = element[3];" +

                                                "var t = '" + Libs.GetQueryString("t") + "';" +
                                                "var sm = '" + semester + "';" +
                                                "var kdt = '" + rel_kelas_det.ToString() + "';" +
                                                "var k = '" + rel_kelas.ToString() + "';" +
                                                "var mp = '" + rel_mapel.ToString() + "';" +
                                                "var s = arr_s[row];" +
                                                "var kd = arr_kd[col];" +
                                                "var kp = arr_kp[col];" +
                                                "var n = data_nilai[row][col];" +
                                                
                                                "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + i_col_nilai_rapor_ppk.ToString() + "});" +
                                                "var formula = hot.getDataAtCell(row, " + i_col_nilai_rapor_ppk.ToString() + ");" +
                                                "formula = formula.substr(1).toUpperCase();" +
                                                "var newValue = hot.plugin.parse(formula, {row: row, col: " + i_col_nilai_rapor_ppk.ToString() + ", id: cellId});" +
                                                "var nr_ppk = (newValue.result);" +

                                                "cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + i_col_nilai_rapor_predikat.ToString() + "});" +
                                                "formula = hot.getDataAtCell(row, " + i_col_nilai_rapor_predikat.ToString() + ");" +
                                                "formula = formula.substr(1).toUpperCase();" +
                                                "newValue = hot.plugin.parse(formula, {row: row, col: " + i_col_nilai_rapor_predikat.ToString() + ", id: cellId});" +
                                                "var nr_prd = (newValue.result);" +

                                                "var lts_hd = (data_nilai[row][" + (i_col_nilai_rapor_predikat + 1).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_predikat + 1).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_predikat + 1).ToString() + "]);" +
                                                "var lts_hd_maks = (data_nilai[row][" + (i_col_nilai_rapor_predikat + 2).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_predikat + 2).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_predikat + 2).ToString() + "]);" +
                                                "var lts_lk = (data_nilai[row][" + (i_col_nilai_rapor_predikat + 3).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_predikat + 3).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_predikat + 3).ToString() + "]);" +
                                                "var lts_rj = (data_nilai[row][" + (i_col_nilai_rapor_predikat + 4).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_predikat + 4).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_predikat + 4).ToString() + "]);" +
                                                "var lts_rpkb = (data_nilai[row][" + (i_col_nilai_rapor_predikat + 5).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_predikat + 5).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_predikat + 5).ToString() + "]);" +

                                                (
                                                    ada_praktik
                                                    ? "cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + i_col_nilai_rapor_p.ToString() + "});" +
                                                      "formula = hot.getDataAtCell(row, " + i_col_nilai_rapor_p.ToString() + ");" +
                                                      "formula = formula.substr(1).toUpperCase();" +
                                                      "newValue = hot.plugin.parse(formula, {row: row, col: " + i_col_nilai_rapor_p.ToString() + ", id: cellId});" +
                                                      "nr_p = (newValue.result);"
                                                    : "nr_p = 0;"
                                                ) +

                                                (
                                                    Libs.GetQueryString("token") != Constantas.SMA.TOKEN_PREVIEW_NILAI
                                                    ? "var s_url = '" + s_url_save + "' + " +
                                                                   "'?' + " +
                                                                   "'j=' + '" + Libs.Enkrip(Libs.JenisKurikulum.SMA.KTSP) + "' + " +
                                                                   "'&t=' + t + " +
                                                                   "'&sm=' + sm + " +
                                                                   "'&kdt=' + kdt + " +
                                                                   "'&s=' + s + " +
                                                                   "'&n=' + n + " +
                                                                   "'&ap=&kd=' + kd + " +
                                                                   "'&kp=' + kp + " +
                                                                   "'&mp=' + mp + " +
                                                                   "'&k=' + k + " +
                                                                   "'&nr_ppk=' + nr_ppk + " +
                                                                   "'&nr_p=' + nr_p + " +
                                                                   "'&nr_prd=' + nr_prd + " +
                                                                   "'&lts_hd=' + lts_hd + " +
                                                                   "'&lts_maxhd=' + lts_hd_maks + " +
                                                                   "'&lts_lk=' + lts_lk + " +
                                                                   "'&lts_rj=' + lts_rj + " +
                                                                   "'&lts_rpkb=' + lts_rpkb + " +
                                                                   "'&ssid=" + Libs.Enkrip(Libs.LOGGED_USER_M.NoInduk) + "'" +
                                                                   ";" +

                                                      "$.ajax({" +
                                                        "url: s_url, " +
                                                        "dataType: 'json', " +
                                                        "type: 'GET', " +
                                                        "contentType: 'application/json; charset=utf-8', " +
                                                        "success: function(data) { " +
                                                            "}, " +
                                                        "error: function(response) { " +
                                                                "alert(response.responseText); " +
                                                            "}, " +
                                                        "failure: function(response) { " +
                                                                "alert(response.responseText); " +
                                                            "} " +
                                                      "}); "
                                                    : ""
                                                ) +

                                            "});" +
                                            "this.render();" +
                                        "}, " +
                                        "mergeCells: [ " +
                                            "{ row: 0, col: 0, rowspan: 2, colspan: 1 }, " +
                                            "{ row: 0, col: 1, rowspan: 2, colspan: 1 }, " +
                                            "{ row: 0, col: 2, rowspan: 2, colspan: 1 }, " +
                                            "{ row: 0, col: 3, rowspan: 2, colspan: 1 } " +
                                            s_merge_cells +
                                        "] " +
                                        (
                                            s_kolom_style.Trim() != ""
                                            ? ", cell: [" + s_kolom_style + "]"
                                            : ""
                                        ) +
                                    "}); " +
                                    "function calculateSize() {" +
                                        "var offset;" +

                                        "offset = Handsontable.dom.offset(div_nilai);" +
                                        "availableWidth = Handsontable.dom.innerWidth(document.body) - offset.left + window.scrollX;" +
                                        "availableHeight = Handsontable.dom.innerHeight(document.body) - offset.top + window.scrollY;" +

                                        "div_nilai.style.width = availableWidth + 'px';" +
                                        "div_nilai.style.height = availableHeight + 'px';" +
                                    "}" +
                                    "function Maximize() {" +
                                        "calculateSize();" +
                                        "hot.render();" +
                                    "}" +
                                    "container.find('table').addClass('zebraStyle');" +
                                    "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_all.ToString() + "); " +
                                    "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_mulai.ToString() + "); ";

                    if (js_statistik.Trim() != "")
                    {
                        js_statistik = "var arr_col = [" + js_statistik + "], ";
                        js_statistik += "id_fixed_row = " + (id_jml_fixed_row).ToString() + ", ";
                        js_statistik += "id_fixed_col = " + id_col_nilai_mulai.ToString() + ";";
                    }

                    ltrJSStatistik.Text = js_statistik;

                    ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";
                }

            }
        }

        protected string GetFormulaPredikatKurtilas(string kode_struktur)
        {
            string formula = "";
            string s_penutup = "";
            int id = 1;
            List<Rapor_StrukturNilai_KURTILAS_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_KURTILAS_Predikat.GetAllByHeader_Entity(kode_struktur);
            foreach (var m in lst_predikat)
            {                
                formula += (formula.Trim() != "" ? "," : "") +
                           "IF(AND(@#>=" + m.Minimal.ToString() + ",@#<" + (m.Maksimal + Convert.ToDecimal(0.1)).ToString() + "),\"" + m.Predikat + "\"";
                if (id == lst_predikat.Count)
                {
                    formula += (formula.Trim() != "" ? "," : "") + "\"\"";
                }
                s_penutup += ")";
                id++;
            }

            if (formula.Trim() != "") formula = "=" + formula + s_penutup;

            return formula;
        }

        protected string GetFormulaPredikatKTSP(string kode_struktur)
        {
            string formula = "";
            string s_penutup = "";
            int id = 1;
            List<Rapor_StrukturNilai_KTSP_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(kode_struktur);
            foreach (var m in lst_predikat)
            {
                formula += (formula.Trim() != "" ? "," : "") +
                           "IF(AND(@#>=" + m.Minimal.ToString() + ",@#<" + (m.Maksimal + Convert.ToDecimal(0.1)).ToString() + "),\"" + m.Predikat + "\"";
                if (id == lst_predikat.Count)
                {
                    formula += (formula.Trim() != "" ? "," : "") + "\"\"";
                }
                s_penutup += ")";
                id++;
            }

            if (formula.Trim() != "") formula = "=" + formula + s_penutup;

            return formula;
        }

        protected string GetFormulaPredikatDeskripsiKTSP(string kode_struktur)
        {
            string formula = "";
            string s_penutup = "";
            int id = 1;
            List<Rapor_StrukturNilai_KTSP_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(kode_struktur);
            foreach (var m in lst_predikat)
            {
                formula += (formula.Trim() != "" ? "," : "") +
                           "IF(AND(@#>=" + m.Minimal.ToString() + ",@#<" + (m.Maksimal + Convert.ToDecimal(0.1)).ToString() + "),\"" + m.Deskripsi + "\"";
                if (id == lst_predikat.Count)
                {
                    formula += (formula.Trim() != "" ? "," : "") + "\"\"";
                }
                s_penutup += ")";
                id++;
            }

            if (formula.Trim() != "") formula = "=" + formula + s_penutup;

            return formula;
        }

        protected void LoadDataKTSP(string semester, bool is_readonly)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";

            string tahun_ajaran = AtributPenilaian.TahunAjaran;
            string rel_kelas = AtributPenilaian.Kelas;
            string rel_kelas_det = AtributPenilaian.KelasDet;
            string rel_mapel = AtributPenilaian.Mapel;

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //status bar
                    ltrStatusBar.Text = "";
                    if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
                    {
                        ltrStatusBar.Text += "&nbsp;" +
                                             "<span style=\"font-weight: bold; color: red;\">" +
                                                "<i class=\"fa fa-exclamation-triangle\"></i>" +
                                                "&nbsp;" +
                                                "PREVIEW INPUT" +
                                                "&nbsp;" +
                                             "</span>&nbsp;";
                    }

                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            ltrStatusBar.Text += "<span style=\"font-weight: bold;\">" + m_mapel.Nama + "</span>";
                        }
                    }

                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                                 "&nbsp;" +
                                                 "<span style=\"font-weight: normal;\">" + m_kelas_det.Nama + "</span>";
                        }
                    }

                    if (tahun_ajaran.Trim() != "")
                    {
                        ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                             "&nbsp;" +
                                             "<span style=\"font-weight: normal;\">" + tahun_ajaran + "</span>";
                    }

                    if (semester.Trim() != "")
                    {
                        ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;Sm." : "") +
                                             "&nbsp;" +
                                             "<span style=\"font-weight: normal;\">" + semester + "</span>";
                    }
                    //end status bar

                    string s_kolom = "";
                    string s_kolom_width = "";
                    string s_kolom_style = "";
                    string s_content = "";
                    string s_merge_cells = "";
                    string s_formula = "";
                    string s_formula_rapor_ppk = "";
                    string s_formula_rapor_p = "";

                    int i_col_nilai_rapor_ppk = 0;
                    int i_col_nilai_rapor_p = 0;
                    int i_col_nilai_rapor_predikat = 0;

                    string s_arr_js_siswa = "";
                    string s_arr_js_kd = "";
                    string s_arr_js_kp = "";

                    string css_bg = "#fff";
                    string css_bg_nkd = "#fff";
                    string css_bg_nilaiakhir = "#fff";

                    bool is_nkd = false;
                    bool ada_praktik = false;

                    List<NILAI_COL> lst_nkd = new List<NILAI_COL>();
                    List<string> lst_kd = new List<string>();
                    List<string> lst_kp = new List<string>();
                    List<FORMULA_RAPOR_KTSP> lst_col_rapor_ppk = new List<FORMULA_RAPOR_KTSP>();
                    List<FORMULA_RAPOR_KTSP> lst_col_rapor_p = new List<FORMULA_RAPOR_KTSP>();

                    List<int> lst_col_terakhir_nkd = new List<int>();                    

                    int id_col = 0;
                    int id_col_all = 0;
                    int id_col_akhir = 0;
                    int id_col_nkd = 0;
                    int id_col_nilai_mulai = 4;
                    int id_jml_fixed_row = 2;

                    string js_statistik = "";

                    //struktur nilai
                    List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas, rel_mapel                
                        );

                    if (lst_stuktur_nilai.Count == 1)
                    {
                        string s_js_arr_kolom1 = "";
                        string s_js_arr_kolom2 = "";
                        string s_js_arr_nilai = "";

                        lst_nkd.Clear();
                        lst_col_terakhir_nkd.Clear();

                        //parse struktur KTSP
                        DAO_Rapor_StrukturNilai.StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                        
                        //kurikulum KTSP
                        if (struktur_nilai.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                        {
                            if (is_readonly)
                            {
                                ltrStatusBar.Text += "&nbsp;&nbsp;" +
                                                     "<span title=\" Data Terkunci/Tidak Bisa Di-edit \" style=\"font-weight: bold; yellow: red;\"><i class=\"fa fa-lock\"></i>&nbsp;Data Terkunci</span>";
                                ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: white;\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    ltrStatusBar.Text;
                                div_statusbar.Attributes["style"] = "color: white; height: 40px; background-color: #ce3584; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
                            }
                            else
                            {
                                ltrStatusBar.Text += "&nbsp;&nbsp;" +
                                                     "<span title=\" Data Bisa Di-edit \" style=\"font-weight: bold; color: green;\"><i class=\"fa fa-unlock\"></i></span>";
                                ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: green;\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    ltrStatusBar.Text;
                                div_statusbar.Attributes["style"] = "color: black; height: 40px; background-color: #eeeeee; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
                            }

                            if (struktur_nilai.KKM > 0)
                            {
                                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;KKM :" : "") +
                                                     "&nbsp;" +
                                                     "<span style=\"font-weight: bold;\">" + Math.Round(struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) + "</span>";

                                txtKKM.Value = Math.Round(struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString();
                            }

                            id_col = id_col_nilai_mulai;
                            id_col_all = id_col_nilai_mulai;

                            //list siswa
                            List<Siswa> lst_siswa = new List<Siswa>();

                            bool b_mapel_pilihan = false;
                            if (m_mapel != null)
                            {
                                if (m_mapel.Nama != null)
                                {
                                    b_mapel_pilihan = (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN  && m_kelas.Nama.Trim().ToUpper() != "X" ? true : false);
                                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN && m_kelas.Nama.Trim().ToUpper() == "X")
                                    {
                                        if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                                            Libs.LOGGED_USER_M.NoInduk,
                                                            QS.GetTahunAjaran(),
                                                            QS.GetSemester(),
                                                            QS.GetKelas(),
                                                            QS.GetMapel()
                                                        ))
                                        {
                                            b_mapel_pilihan = true;
                                        }
                                    }
                                }
                            }

                            if (b_mapel_pilihan)
                            {
                                lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                    tahun_ajaran,
                                    semester,
                                    rel_mapel,
                                    rel_kelas
                                );
                            }
                            else
                            {
                                if (
                                    m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT
                                )
                                {
                                    lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                                            tahun_ajaran,
                                            semester,
                                            rel_mapel,
                                            m_kelas.Kode.ToString(),
                                            rel_kelas_det
                                        );
                                }
                                else
                                {
                                    if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
                                    {
                                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            QS.GetTahunAjaran(),
                                            QS.GetSemester()
                                        );
                                        if (lst_siswa.Count == 0)
                                        {
                                            lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                                            m_kelas.Rel_Sekolah.ToString(),
                                                            rel_kelas_det,
                                                            QS.GetTahunAjaran(),
                                                            QS.GetSemester()
                                                        );
                                        }
                                    }
                                    else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
                                    {
                                        lst_siswa = GetListSiswaSimulasi();
                                    }
                                    else
                                    {
                                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            QS.GetTahunAjaran(),
                                            QS.GetSemester()
                                        );
                                        if (lst_siswa.Count == 0)
                                        {
                                            lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                                            m_kelas.Rel_Sekolah.ToString(),
                                                            rel_kelas_det,
                                                            QS.GetTahunAjaran(),
                                                            QS.GetSemester()
                                                        );
                                        }
                                    }
                                }
                            }

                            //kompetensi dasar
                            List <Rapor_StrukturNilai_KTSP_KD> lst_kompetensi_dasar = 
                                DAO_Rapor_StrukturNilai_KTSP_KD.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 0, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                             "{ row: 1, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                             "{ row: 0, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                             "{ row: 1, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                             "{ row: 0, col: 2, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                             "{ row: 1, col: 2, className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                            //init js arr kompetensi dasar/kd
                            s_arr_js_kd = "";
                            lst_kd.Clear();
                            for (int i = 0; i < id_col_nilai_mulai; i++)
                            {
                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                               "\"\"";
                                lst_kd.Add("");
                            }
                            //end init

                            //init js arr komponen penilaian/kp
                            s_arr_js_kp = "";
                            lst_kp.Clear();
                            for (int i = 0; i < id_col_nilai_mulai; i++)
                            {
                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                               "\"\"";
                                lst_kp.Add("");
                            }
                            //end init

                            foreach (var m_sn_kd in lst_kompetensi_dasar)
                            {
                                //komponen penilaian
                                List<Rapor_StrukturNilai_KTSP_KP> lst_komponen_penilaian =
                                    DAO_Rapor_StrukturNilai_KTSP_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                                Rapor_KompetensiDasar m_kd =
                                        DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());

                                if (m_kd != null)
                                {
                                    if (m_kd.Nama != null)
                                    {
                                        if (lst_komponen_penilaian.Count > 0)
                                        {
                                            id_col_nkd = id_col_all;

                                            foreach (var m_sn_kp in lst_komponen_penilaian)
                                            {
                                                Rapor_KomponenPenilaian m_kp =
                                                    DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());

                                                if (m_kp != null)
                                                {
                                                    if (m_kp.Nama != null)
                                                    {
                                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                        (id_col_all).ToString();

                                                        //add kd to var arr js
                                                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                       "\"" + m_sn_kd.Kode.ToString() + "\"";
                                                        lst_kd.Add(m_sn_kd.Kode.ToString());

                                                        //add kp to var arr js
                                                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                       "\"" + m_sn_kp.Kode.ToString() + "\"";
                                                        lst_kp.Add(m_sn_kp.Kode.ToString());
                                                        //end add kd & kp

                                                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                          "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                          "\"" + Libs.GetHTMLSimpleText(m_kp.Nama) + "\" ";
                                                        
                                                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                          LEBAR_COL_DEFAULT;

                                                        if (m_sn_kp.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PRAKTIK)
                                                        {
                                                            lst_col_rapor_p.Add(new FORMULA_RAPOR_KTSP {
                                                                IdKolom = (id_col_all + 1),
                                                                Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                                                Kode_SN_KP = m_sn_kp.Kode.ToString(),
                                                                KolomHeadersFormula = "IF(" + Libs.GetColHeader((id_col_all + 1)) + "# = \"\", 0, " +
                                                                                              Libs.GetColHeader((id_col_all + 1)) + "#" +
                                                                                        ")"
                                                            });

                                                            ada_praktik = true;
                                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                             "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontRed\", readOnly: true, renderer: \"html\" }";
                                                        }
                                                        else
                                                        {
                                                            if (lst_komponen_penilaian.FindAll(m => m.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK).Count == 1)
                                                            {
                                                                lst_col_rapor_ppk.Add(new FORMULA_RAPOR_KTSP {
                                                                    IdKolom = id_col_all,
                                                                    Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                                                    Kode_SN_KP = m_sn_kp.Kode.ToString(),
                                                                    KolomHeadersFormula = Libs.GetColHeader(id_col_all) + "#"
                                                                });
                                                            }

                                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                             "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                        }

                                                        id_col_all++;
                                                    }
                                                }
                                            }

                                            //cek n-kd jika lebih dari 1 buat kolom tambahan n-KD
                                            //ini kolom n-kd nya
                                            if (lst_komponen_penilaian.FindAll(m => m.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK).Count > 1)
                                            {
                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"\"";
                                                lst_kd.Add("");

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"\"";
                                                lst_kp.Add("");
                                                //end add kd & kp

                                                //pasang formula
                                                //generate formula
                                                s_formula = "";
                                                foreach (var m_kp_ktsp in lst_komponen_penilaian)
                                                {
                                                    if (m_kp_ktsp.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK)
                                                    {
                                                        if (m_kp_ktsp.BobotNKD > 0)
                                                        {
                                                            s_formula += (s_formula.Trim() != "" ? "+" : "") +
                                                                         "(" +
                                                                            "IF(" +
                                                                                Libs.GetColHeader(id_col_nkd + 1) + "# = \"\"" +
                                                                                ", 0, " + Libs.GetColHeader(id_col_nkd + 1) + "#" + 
                                                                            ")" +
                                                                            "*(" + (m_kp_ktsp.BobotNKD.ToString()) + "%)" +
                                                                         ")";
                                                        }
                                                    }

                                                    id_col_nkd++;
                                                }
                                                if (s_formula.Trim() != "")
                                                {
                                                    s_formula = "IF(" +
                                                                    "ROUND(" +
                                                                        "(" +
                                                                            s_formula +
                                                                        "), " +
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() +
                                                                    ") = 0 "+
                                                                ", \"\", " +
                                                                    "ROUND(" +
                                                                        "(" +
                                                                            s_formula +
                                                                        "), " +
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() +
                                                                    ")" +
                                                                ")";
                                                }
                                                //end pasang formula
                                                
                                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                            "{" +
                                                            "  row: 0, col: " + id_col + ", rowspan: 1, " +
                                                            "  colspan: " + (lst_komponen_penilaian.Count + 1).ToString() +
                                                            "} ";

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                  "\"" + NKD + "\" ";
                                                
                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                  LEBAR_COL_DEFAULT;

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                //add nkd
                                                lst_nkd.Add(
                                                        new NILAI_COL {
                                                            IdKolom = id_col + lst_komponen_penilaian.Count,
                                                            BluePrintFormula = s_formula
                                                        }
                                                    );

                                                lst_col_rapor_ppk.Add(new FORMULA_RAPOR_KTSP {
                                                    IdKolom = id_col_all,
                                                    Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                                    Kode_SN_KP = "",
                                                    KolomHeadersFormula = Libs.GetColHeader(id_col_all) + "#"
                                                });

                                                id_col_all++;
                                                id_col += (lst_komponen_penilaian.Count + 1);
                                            }
                                            else
                                            {
                                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                            "{" +
                                                            "  row: 0, col: " + id_col + ", rowspan: 1, " +
                                                            "  colspan: " + lst_komponen_penilaian.Count.ToString() +
                                                            "} ";

                                                id_col += lst_komponen_penilaian.Count;

                                                //index colom terakhir nkd
                                                lst_col_terakhir_nkd.Add(id_col_all - 1);
                                                txtKTSPColKD.Value += ";" + (id_col_all - 1) + ";";
                                            }
                                            //end cek n-kd
                                        }
                                        else
                                        {
                                            js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                            (id_col_all).ToString();

                                            //add kd to var arr js
                                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                           "\"" + m_sn_kd.Kode.ToString() + "\"";
                                            lst_kd.Add(m_sn_kd.Kode.ToString());

                                            //add kp to var arr js
                                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                           "\"\"";
                                            lst_kp.Add("");
                                            //end add kd & kp

                                            s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                         "{" +
                                                         "  row: 0, col: " + id_col + ", rowspan: 2, " +
                                                         "  colspan: 1 " +
                                                         "} ";

                                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                      "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                      "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\" ";
                                            
                                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                              LEBAR_COL_DEFAULT;

                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }, " +
                                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                            //index colom terakhir nkd
                                            lst_col_terakhir_nkd.Add(id_col_all);
                                            txtKTSPColKD.Value += ";" + (id_col_all) + ";";

                                            lst_col_rapor_ppk.Add(new FORMULA_RAPOR_KTSP
                                            {
                                                IdKolom = id_col_all,
                                                Kode_SN_KD = m_sn_kd.Kode.ToString(),
                                                Kode_SN_KP = "",
                                                KolomHeadersFormula = Libs.GetColHeader(id_col_all) + "#"
                                            });

                                            id_col_all++;
                                            id_col++;
                                        }
                                    }
                                }
                            }

                            //generate formula rapor
                            s_formula_rapor_ppk = "";
                            s_formula_rapor_p = "";

                            //parse formula rapor ppk
                            foreach (FORMULA_RAPOR_KTSP formula_ppk in lst_col_rapor_ppk)
                            {
                                Rapor_StrukturNilai_KTSP_KD r_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(formula_ppk.Kode_SN_KD);
                                if (r_kd != null)
                                {
                                    if (r_kd.Poin != null)
                                    {
                                        s_formula_rapor_ppk += (s_formula_rapor_ppk.Trim() != "" ? "+" : "") +
                                                               "(" +
                                                                    "IF(" +
                                                                        Libs.GetColHeader(formula_ppk.IdKolom + 1) + "# " +
                                                                        "= \"\", 0, " +
                                                                        Libs.GetColHeader(formula_ppk.IdKolom + 1) + "# " +
                                                                    ")" +
                                                                    "*" +
                                                                    Math.Round(r_kd.BobotRaporPPK, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) +
                                                                    "%" +
                                                                ")";
                                    }
                                }
                            }
                            if (s_formula_rapor_ppk.Trim() != "")
                                s_formula_rapor_ppk = "=IF(ROUND(" + s_formula_rapor_ppk + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")<=0, \"\", " +
                                                          "ROUND(" + s_formula_rapor_ppk + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")" + ")";

                            //parse formula rapor praktik
                            foreach (string s_kd_p in lst_col_rapor_p.Select(m => m.Kode_SN_KD).Distinct().ToList())
                            {
                                string s_item_formula_rapor_p = "";
                                foreach (var s_kp_p in lst_col_rapor_p.FindAll(m => m.Kode_SN_KD == s_kd_p).OrderBy(m => m.IdKolom).ToList())
                                {
                                    s_item_formula_rapor_p += (s_item_formula_rapor_p.Trim() != "" ? "+" : "") +
                                                              s_kp_p.KolomHeadersFormula;
                                }
                                s_item_formula_rapor_p = "(" + s_item_formula_rapor_p + ")/" +
                                                         lst_col_rapor_p.FindAll(m => m.Kode_SN_KD == s_kd_p).OrderBy(m => m.IdKolom).ToList().Count.ToString();

                                Rapor_StrukturNilai_KTSP_KD r_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(s_kd_p);
                                if (r_kd != null)
                                {
                                    if (r_kd.Poin != null)
                                    {
                                        s_formula_rapor_p += (s_formula_rapor_p.Trim() != "" ? "+" : "") +
                                                             "(" +
                                                                 "(" + s_item_formula_rapor_p + ")" +
                                                                 "*" +
                                                                 Math.Round(r_kd.BobotRaporP, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA) +
                                                                 "%" +
                                                             ")";
                                    }
                                }
                            }

                            if (s_formula_rapor_p.Trim() != "")
                                s_formula_rapor_p = "=IF(ROUND(" + s_formula_rapor_p + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")<=0, \"\", " +
                                                        "ROUND(" + s_formula_rapor_p + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() + ")" + ")";

                            //end generate formula rapor
                            string s_js_nilai_rapor_ktsp_ppk = "";
                            string s_js_nilai_rapor_ktsp_p = "";
                            string s_js_nilai_rapor_ktsp_predikat = "";

                            //kolom header akhir (kolom nilai rapor)
                            if (ada_praktik) //jika ada nilai praktik
                            {
                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                (id_col_all).ToString();
                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                (id_col_all + 1).ToString();

                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                   "\"NILAI RAPOR\", " +
                                                   "\"NILAI RAPOR\", " +
                                                   "\"NILAI RAPOR\"";
                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") + 
                                                   "\"PPK\", " +
                                                   "\"P\", " +
                                                   "\"SIKAP\"";
                                if (s_merge_cells.Trim() != "") s_merge_cells += ", " +
                                                            "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 1, colspan: 3 } ";

                                //kolom style paling akhir
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                                 "{ row: 1, col: " + (id_col_all + 1).ToString() + ", className: \"htCenter htMiddle htFontBold htFontRed\", readOnly: true }, " +
                                                 "{ row: 1, col: " + (id_col_all + 2).ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true } ";

                                //col width untuk 3 kolom
                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;

                                s_js_nilai_rapor_ktsp_ppk = "'" + s_formula_rapor_ppk + "'";
                                s_js_nilai_rapor_ktsp_p = "'" + s_formula_rapor_p + "'";
                                s_js_nilai_rapor_ktsp_predikat = "\"\"";
                            }
                            else //tidak ada nilai praktik
                            {
                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                (id_col_all).ToString();

                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") + 
                                                   "\"NILAI RAPOR\", " +
                                                   "\"NILAI RAPOR\"";
                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") + 
                                                   "\"PPK\", " +
                                                   "\"SIKAP\"";

                                if (s_merge_cells.Trim() != "") s_merge_cells += ", " +
                                                            "{ row: 0, col: " + id_col_all + ", rowspan: 1, colspan: 2 } ";
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }, " +
                                                 "{ row: 1, col: " + (id_col_all + 1).ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true }";

                                //col width untuk 2 kolom
                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;
                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") + LEBAR_COL_DEFAULT;

                                s_js_nilai_rapor_ktsp_ppk = "\"" + s_formula_rapor_ppk + "\"";
                                s_js_nilai_rapor_ktsp_p = "\"" + s_formula_rapor_p + "\"";
                            }
                            //end kolom header akhir

                            if (s_js_arr_kolom1.Trim() != "") s_js_arr_kolom1 = ", " + s_js_arr_kolom1;
                            if (s_js_arr_kolom2.Trim() != "") s_js_arr_kolom2 = ", " + s_js_arr_kolom2;
                            if (s_kolom_width.Trim() != "") s_kolom_width = ", " + s_kolom_width;

                            s_kolom = "[" +
                                          "\"#\", " +
                                          "\"NIS\", " +
                                          "\"NAMA SISWA\", " +
                                          "\"L/P\" " +
                                          s_js_arr_kolom1 +
                                      "], " +
                                      "[" +
                                          "\"#\", " +
                                          "\"NIS\", " +
                                          "\"NAMA SISWA\", " +
                                          "\"L/P\" " +
                                          s_js_arr_kolom2 +
                                      "]";

                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 0, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }," +
                                             "{ row: 1, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true, renderer: \"html\" }";

                            //list siswa dan nilainya
                            //get list nilai jika ada
                            Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, rel_mapel
                                ).FirstOrDefault();
                            List<Rapor_NilaiSiswa_KTSP> lst_nilai_siswa = null;
                            List<Rapor_NilaiSiswa_KTSP_Det> lst_nilai_siswa_det = null;
                            if (m_nilai != null)
                            {
                                if (m_nilai.Kurikulum != null)
                                {
                                    lst_nilai_siswa = DAO_Rapor_NilaiSiswa_KTSP.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                                    lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_KTSP_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());                                    
                                }
                            }
                            
                            //init js arr siswa
                            s_arr_js_siswa = "";
                            for (int i = 0; i < id_jml_fixed_row; i++)
                            {
                                s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                                  "\"\"";
                            }

                            int id = 1;
                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                string kelas_det = "";
                                m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                                if (m_kelas_det != null)
                                {
                                    if (m_kelas_det.Nama != null)
                                    {
                                        kelas_det = m_kelas_det.Nama;
                                    }
                                }

                                css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                                css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                                css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                                s_js_arr_nilai = "";

                                s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                                  "\"" + m_siswa.Kode.ToString() + "\"";

                                if (id_col_all > id_col_nilai_mulai)
                                {
                                    for (int i = id_col_nilai_mulai; i < id_col_all; i++)
                                    {
                                        is_nkd = false;
                                        s_formula = "";
                                        foreach (NILAI_COL item_nkd in lst_nkd)
                                        {
                                            if (i == item_nkd.IdKolom)
                                            {
                                                is_nkd = true;
                                                //parse formula
                                                s_formula = (
                                                                item_nkd.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()).Trim() != ""
                                                                ? "=" : ""
                                                            ) +
                                                            item_nkd.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                break;
                                            }
                                        }

                                        if (is_nkd)
                                        {
                                            //nkd per kompetensi dasar
                                            s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                              "'" + s_formula + "'";

                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + 2) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nkd + " htFontBlack htBorderRightNKD\", readOnly: true }";
                                        }
                                        else
                                        {
                                            //---get nilainya disini
                                            Rapor_NilaiSiswa_KTSP_Det m_nilai_siswa_det_ktsp =
                                                (
                                                    lst_nilai_siswa_det != null
                                                    ?
                                                    lst_nilai_siswa_det.FindAll(
                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                             m.Rel_Rapor_StrukturNilai_KTSP_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                             m.Rel_Rapor_StrukturNilai_KTSP_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                                    ).FirstOrDefault()
                                                    : null
                                                );

                                            s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                              "'" +
                                                                (
                                                                    m_nilai_siswa_det_ktsp != null
                                                                    ? (
                                                                        m_nilai_siswa_det_ktsp.Nilai != null 
                                                                        ? m_nilai_siswa_det_ktsp.Nilai.Replace(@"""", "").Replace("'", "").Replace("\\", "")
                                                                        : ""
                                                                      )
                                                                    : ""
                                                                ) +
                                                              "'";
                                            //---end get nilai

                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + 
                                                             (lst_col_terakhir_nkd.FindAll(m => m == i).Count > 0 ? "htBorderRightNKD" : "") + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + "}";
                                        }                                        
                                    }
                                }

                                //get nilai predikat sikap
                                s_js_nilai_rapor_ktsp_predikat = "\"\"";
                                Rapor_NilaiSiswa_KTSP m_nilai_siswa_ktsp = null;
                                if (lst_nilai_siswa != null)
                                {
                                    if (lst_nilai_siswa.FirstOrDefault() != null)
                                    {
                                        if (lst_nilai_siswa.FirstOrDefault().Rel_Siswa != null)
                                        {
                                            m_nilai_siswa_ktsp = lst_nilai_siswa.FindAll(
                                                    m => m.Rel_Siswa == m_siswa.Kode.ToString()
                                                ).FirstOrDefault();
                                            if (m_nilai_siswa_ktsp != null)
                                            {
                                                if (m_nilai_siswa_ktsp.Rapor_Sikap != null)
                                                {
                                                    s_js_nilai_rapor_ktsp_predikat = "\"" + m_nilai_siswa_ktsp.Rapor_Sikap + "\"";
                                                }
                                            }
                                        }
                                    }
                                }
                                //end get nilai predikat sikap

                                //cell untuk nilai rapor & predikatnya
                                s_js_arr_nilai += ((s_js_arr_nilai.Trim() != "" && s_js_nilai_rapor_ktsp_ppk.Trim() != "" ? ", " : "") + s_js_nilai_rapor_ktsp_ppk.Replace("#", (id + 2).ToString())) +
                                                  ((s_js_arr_nilai.Trim() != "" && s_js_nilai_rapor_ktsp_p.Trim() != "" ? ", " : "") + s_js_nilai_rapor_ktsp_p.Replace("#", (id + 2).ToString())) +
                                                  ((s_js_arr_nilai.Trim() != "" && s_js_nilai_rapor_ktsp_predikat.Trim() != "" ? ", " : "") + s_js_nilai_rapor_ktsp_predikat);
                                //end cell nilai rapor

                                //style col akhir
                                id_col_akhir = id_col_all;
                                if (s_js_nilai_rapor_ktsp_ppk.Trim() != "") //rapor ppk
                                {
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_akhir.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBold htFontBlack\", readOnly: true }";

                                    i_col_nilai_rapor_ppk = id_col_akhir;
                                    id_col_akhir++;
                                }
                                if (s_js_nilai_rapor_ktsp_p.Trim() != "") //rapor p
                                {
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_akhir.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBold htFontBlack\", readOnly: true }";

                                    i_col_nilai_rapor_p = id_col_akhir;
                                    id_col_akhir++;
                                }
                                if (s_js_nilai_rapor_ktsp_predikat.Trim() != "") //rapor predikat
                                {
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_akhir.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBold htFontBlack\", readOnly: false }";

                                    i_col_nilai_rapor_predikat = id_col_akhir;
                                    id_col_akhir++;
                                }
                                //end cell untuk nilai rapor & predikatnya

                                s_content += (s_content.Trim() != "" ? ", " : "") +
                                             "[" +
                                                "\"" + id.ToString() + "\", " +
                                                "\"" + m_siswa.NISSekolah + "\", " +
                                                "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true), 3) +
                                                    (
                                                        m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT
                                                        ? "<label style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</label>"
                                                        : ""
                                                    ) +
                                                "\", " +
                                                "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                                (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                             "]";

                                //kolom style untuk fixed col header
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                                id++;
                            }
                            //end list siswa dan nilainya

                            s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
                            s_arr_js_kd = "[" + s_arr_js_kd + "]";
                            s_arr_js_kp = "[" + s_arr_js_kp + "]";

                            if (s_content.Trim() != "") s_content = ", " + s_content;
                            if (s_merge_cells.Trim() != "") s_merge_cells = ", " + s_merge_cells;

                            string s_data = "var data_nilai = " +
                                                "[" +
                                                    s_kolom +
                                                    s_content +
                                                "];";

                            string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMA.NILAI_SISWA.ROUTE);
                            s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMA.NILAI_SISWA.DO_SAVE.FILE + "/Do");

                            string script = s_data +
                                            "var arr_s = " + s_arr_js_siswa + ";" +
                                            "var arr_kd = " + s_arr_js_kd + ";" +
                                            "var arr_kp = " + s_arr_js_kp + ";" +
                                            "var div_nilai = document.getElementById('div_nilai'), hot; " +
                                            "var container = $('#div_nilai');  " +
                                            "hot = new Handsontable(div_nilai, { " +
                                                "data: data_nilai, " +
                                                "colWidths: [35, 80, 300, 40 " + s_kolom_width + "], " +
                                                "rowHeaders: true, " +
                                                "colHeaders: true, " +
                                                "fixedColumnsLeft: 4, " +
                                                "fixedRowsTop: 2, " +
                                                "minSpareRows: 0, " +
                                                "colHeaders: false, " +
                                                "fontSize: 9, " +
                                                "contextMenu: false, " +
                                                "formulas: true," +
                                                "fillHandle: false," +
                                                "cells: function (row, col, prop) {" +
                                                  "if(this.instance.getData().length != 0){" +
                                                    "var cellProperties = {};" +
                                                    "if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                        "if((row + 1) % 2 === 0){" +
                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                        "} else {" +
                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                        "} " +
                                                    "} " +
                                                    "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                        "if((row + 1) % 2 === 0){" +
                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                        "} else {" +
                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                        "} " +
                                                    "} " +
                                                    "return cellProperties;" +
                                                  "}" +
                                                "}," +
                                                "afterChange: function(changes, source) {" +
                                                    "if (source === 'loadData') return;" +
                                                    "$.each(changes, function(index, element) {" +
                                                        "var row = element[0];" +
                                                        "var col = element[1];" +
                                                        "var oldVal = element[2];" +
                                                        "var newVal = element[3];" +

                                                        "var t = '" + Libs.GetQueryString("t") + "';" +
                                                        "var sm = '" + semester + "';" +
                                                        "var kdt = '" + rel_kelas_det.ToString() + "';" +
                                                        "var k = '" + rel_kelas.ToString() + "';" +
                                                        "var mp = '" + rel_mapel.ToString() + "';" +
                                                        "var s = arr_s[row];" +
                                                        "var kd = arr_kd[col];" +
                                                        "var kp = arr_kp[col];" +
                                                        "var n = data_nilai[row][col];" +
                                                        "var nr_prd = data_nilai[row][" + i_col_nilai_rapor_predikat.ToString() + "];" +

                                                        "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + i_col_nilai_rapor_ppk.ToString() + "});" +
                                                        "var formula = hot.getDataAtCell(row, " + i_col_nilai_rapor_ppk.ToString() + ");" +
                                                        "formula = formula.substr(1).toUpperCase();" +
                                                        "var newValue = hot.plugin.parse(formula, {row: row, col: " + i_col_nilai_rapor_ppk.ToString() + ", id: cellId});" +
                                                        "var nr_ppk = (newValue.result);" +

                                                        "cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + i_col_nilai_rapor_p.ToString() + "});" +
                                                        "formula = hot.getDataAtCell(row, " + i_col_nilai_rapor_p.ToString() + ");" +
                                                        "formula = formula.substr(1).toUpperCase();" +
                                                        "newValue = hot.plugin.parse(formula, {row: row, col: " + i_col_nilai_rapor_p.ToString() + ", id: cellId});" +
                                                        "nr_p = (newValue.result);" +

                                                        "var lts_hd = (data_nilai[row][" + (i_col_nilai_rapor_p + 1).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_p + 1).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_p + 1).ToString() + "]);" +
                                                        "var lts_hd_maks = (data_nilai[row][" + (i_col_nilai_rapor_p + 2).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_p + 2).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_p + 2).ToString() + "]);" +
                                                        "var lts_lk = (data_nilai[row][" + (i_col_nilai_rapor_p + 3).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_p + 3).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_p + 3).ToString() + "]);" +
                                                        "var lts_rj = (data_nilai[row][" + (i_col_nilai_rapor_p + 4).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_p + 4).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_p + 4).ToString() + "]);" +
                                                        "var lts_rpkb = (data_nilai[row][" + (i_col_nilai_rapor_p + 5).ToString() + "] === undefined || data_nilai[row][" + (i_col_nilai_rapor_p + 5).ToString() + "] === null ? '' : data_nilai[row][" + (i_col_nilai_rapor_p + 5).ToString() + "]);" +

                                                        (
                                                            "var s_url = '" + s_url_save + "' + " +
                                                                           "'?' + " +
                                                                           "'j=' + '" + Libs.Enkrip(Libs.JenisKurikulum.SMA.KTSP) + "' + '&' + " +
                                                                           "'t=' + t + " +
                                                                           "'&sm=' + sm + " +
                                                                           "'&kdt=' + kdt + " +
                                                                           "'&s=' + s + " +
                                                                           "'&n=' + n + " +
                                                                           "'&ap=&kd=' + kd + " +
                                                                           "'&kp=' + kp + " +
                                                                           "'&mp=' + mp + " +
                                                                           "'&k=' + k + " +
                                                                           "'&nr_ppk=' + nr_ppk + " +
                                                                           "'&nr_p=' + nr_p + " +
                                                                           "'&nr_prd=' + nr_prd + " +
                                                                           "'&nr_p=' + nr_p + " +
                                                                           "'&lts_hd=' + lts_hd + " +
                                                                           "'&lts_maxhd=' + lts_hd_maks + " +
                                                                           "'&lts_lk=' + lts_lk + " +
                                                                           "'&lts_rj=' + lts_rj + " +
                                                                           "'&lts_rpkb=' + lts_rpkb + " +
                                                                           "'&ssid=" + Libs.Enkrip(Libs.LOGGED_USER_M.NoInduk) + "'" +
                                                                           ";" +

                                                            "$.ajax({" +
                                                                "url: s_url, " +
                                                                "dataType: 'json', " +
                                                                "type: 'GET', " +
                                                                "contentType: 'application/json; charset=utf-8', " +
                                                                "success: function(data) { " +
                                                                    "}, " +
                                                                "error: function(response) { " +
                                                                        "alert(response.responseText); " +
                                                                    "}, " +
                                                                "failure: function(response) { " +
                                                                        "alert(response.responseText); " +
                                                                    "} " +
                                                            "}); "
                                                        ) +

                                                    "});" +
                                                    "this.render();" +
                                                "}, " +
                                                "mergeCells: [ " +
                                                    "{ row: 0, col: 0, rowspan: 2, colspan: 1 }, " +
                                                    "{ row: 0, col: 1, rowspan: 2, colspan: 1 }, " +
                                                    "{ row: 0, col: 2, rowspan: 2, colspan: 1 }, " +
                                                    "{ row: 0, col: 3, rowspan: 2, colspan: 1 } " +
                                                    s_merge_cells +
                                                "] " +
                                                (
                                                    s_kolom_style.Trim() != ""
                                                    ? ", cell: [" + s_kolom_style + "]"
                                                    : ""
                                                ) +
                                            "}); " +
                                            "function calculateSize() {" +
                                                "var offset;" +

                                                "offset = Handsontable.dom.offset(div_nilai);" +
                                                "availableWidth = Handsontable.dom.innerWidth(document.body) - offset.left + window.scrollX;" +
                                                "availableHeight = Handsontable.dom.innerHeight(document.body) - offset.top + window.scrollY;" +

                                                "div_nilai.style.width = availableWidth + 'px';" +
                                                "div_nilai.style.height = availableHeight + 'px';" +
                                            "}" +
                                            "function Maximize() {" +
                                                "calculateSize();" +
                                                "hot.render();" +
                                            "}" +
                                            "container.find('table').addClass('zebraStyle');" +
                                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_all.ToString() + "); " +
                                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_mulai.ToString() + "); ";

                            if (js_statistik.Trim() != "")
                            {
                                js_statistik = "var arr_col = [" + js_statistik + "], ";
                                js_statistik += "id_fixed_row = " + (id_jml_fixed_row).ToString() + ", ";
                                js_statistik += "id_fixed_col = " + id_col_nilai_mulai.ToString() + ";";
                            }

                            ltrJSStatistik.Text = js_statistik;

                            ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";
                        }

                    }

                }
            }
        }

        protected void lnkShowStatistics_Click(object sender, EventArgs e)
        {
            ShowStatistikPenilaian();
            txtKeyAction.Value = JenisAction.DoShowStatistik.ToString();
        }

        protected void ShowStatistikPenilaian()
        {
            string tahun_ajaran = AtributPenilaian.TahunAjaran;
            string semester = AtributPenilaian.Semester;
            string rel_kelas = AtributPenilaian.Kelas;
            string rel_kelas_det = AtributPenilaian.KelasDet;
            string rel_mapel = AtributPenilaian.Mapel;

            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas, rel_mapel
                        );

            if (lst_stuktur_nilai.Count == 1)
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m = lst_stuktur_nilai.FirstOrDefault();
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        switch (m.Kurikulum)
                        {
                            case Libs.JenisKurikulum.SMA.KTSP:
                                ShowStatistikKTSP();
                                break;
                            case Libs.JenisKurikulum.SMA.KURTILAS:
                                ShowStatistikKURTILAS();
                                break;
                        }
                    }
                }
            }
        }

        protected void ShowStatistikKTSP()
        {
            string html = "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                            "<table class=\"table\">" +
                                "<tr>" +
                                    "<td colspan=\"2\" style=\"font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser-2.svg") + "\" style=\"height: 25px; width: 25px;\" />" +
                                        "&nbsp;&nbsp;" +
                                        "&nbsp;&nbsp;" +
                                        "Struktur Penilaian" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Nilai Rata-Rata" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Nilai Tertinggi" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Nilai Terrendah" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<i class=\"fa fa-user-circle\"></i> > KKM" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "% > KKM" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<i class=\"fa fa-user-circle\"></i> < KKM" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "% < KKM" +
                                    "</td>" +
                                "</tr>";

            html += "<tr>" +
                        "<td colspan=\"9\" style=\"font-size: smaller; color: grey; padding: 2px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 0px; border-color: #2485a9; background-color: #2485a9;\">" +
                        "</td>" +
                    "</tr>";

            html += "<tr>" +
                        "<td colspan=\"9\" style=\"font-size: smaller; color: grey; padding: 2px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 0px; border-color: white; background-color: white;\">" +
                        "</td>" +
                    "</tr>";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(AtributPenilaian.Kelas);
            if (m_kelas != null)
            {
                //struktur nilai
                List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                        AtributPenilaian.TahunAjaran, AtributPenilaian.Semester, AtributPenilaian.Kelas, AtributPenilaian.Mapel
                    );

                if (lst_stuktur_nilai.Count == 1)
                {
                    DAO_Rapor_StrukturNilai.StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                    if (struktur_nilai.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                    {
                        bool ada_rowspan_kd = false;
                        bool ada_praktik = false;

                        int jumlah_rowspan_kd = 0;

                        string bg1 = "#FFFFFF";
                        string bg2 = "#fbfbfb";

                        int id_all = 0;
                        int id_fixed_col = 3;

                        //load kurtilas kd
                        List<Rapor_StrukturNilai_KTSP_KD> lst_kompetensi_dasar =
                            DAO_Rapor_StrukturNilai_KTSP_KD.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                        foreach (Rapor_StrukturNilai_KTSP_KD m_sn_kd in lst_kompetensi_dasar)
                        {

                            if (m_sn_kd != null)
                            {
                                if (m_sn_kd.Poin != null)
                                {

                                    Rapor_KompetensiDasar m_kd =
                                        DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());

                                    if (m_kd != null)
                                    {
                                        if (m_kd.Nama != null)
                                        {
                                            jumlah_rowspan_kd = 0;
                                            ada_rowspan_kd = true;

                                            //load ktsp kp
                                            List<Rapor_StrukturNilai_KTSP_KP> lst_komponen_penilaian =
                                                DAO_Rapor_StrukturNilai_KTSP_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                                            foreach (Rapor_StrukturNilai_KTSP_KP m_sn_kp in lst_komponen_penilaian)
                                            {
                                                if (m_sn_kp.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PRAKTIK)
                                                {
                                                    ada_praktik = true;
                                                }

                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                if (m_kp != null)
                                                {
                                                    if (m_kp.Nama != null)
                                                    {
                                                        jumlah_rowspan_kd++;
                                                        id_all++;

                                                        string bg = (id_all % 2 == 0 ? bg1 : bg2);

                                                        html += "<tr>";

                                                        if (ada_rowspan_kd)
                                                        {
                                                            html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; @border_nkd\">" +
                                                                        (
                                                                            m_sn_kd.Poin.Trim() != ""
                                                                            ? m_sn_kd.Poin.Trim() + "&nbsp;"
                                                                            : ""
                                                                        ) +
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama).Trim() +
                                                                    "</td>";
                                                            ada_rowspan_kd = false;
                                                        }

                                                        html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; \">" +
                                                                    Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                "</td>";

                                                        html += "</tr>";

                                                    }
                                                }

                                            }

                                            bool ada_nkd = false;
                                            string css_border_nkd = " border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px;";

                                            //add nkd
                                            if (lst_komponen_penilaian.FindAll(m => m.Jenis == Libs.JenisKomponenNilaiKTSP.SMA.PPK).Count > 1)
                                            {
                                                ada_nkd = true;
                                                jumlah_rowspan_kd++;
                                                id_all++;

                                                html += "<tr>";

                                                if (ada_rowspan_kd)
                                                {
                                                    html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px; @border_nkd\">" +
                                                                (
                                                                    m_sn_kd.Poin.Trim() != ""
                                                                    ? m_sn_kd.Poin + "&nbsp;"
                                                                    : ""
                                                                ) +
                                                                Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                            "</td>";
                                                    ada_rowspan_kd = false;
                                                }

                                                string bgnkd1 = "#EFECDC";
                                                string bgnkd2 = "#EFECDC";

                                                html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;" + css_border_nkd + "\">" +
                                                            NKD +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "</tr>";
                                            }
                                            else if (lst_komponen_penilaian.Count == 0) //biasanya jika uas
                                            {
                                                id_all++;
                                                jumlah_rowspan_kd++;
                                                string bg = (id_all % 2 == 0 ? bg1 : bg2);

                                                html += "<tr>";

                                                if (ada_rowspan_kd)
                                                {
                                                    html += "<td colspan=\"2\" colspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;" + css_border_nkd + "\">" +
                                                                (
                                                                    m_sn_kd.Poin.Trim() != ""
                                                                    ? m_sn_kd.Poin.Trim() + "&nbsp;"
                                                                    : ""
                                                                ) +
                                                                Libs.GetHTMLSimpleText(m_kd.Nama).Trim() +
                                                            "</td>";
                                                    ada_rowspan_kd = false;
                                                }

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                        "</td>";

                                                html += "</tr>";
                                            }

                                            if (ada_nkd)
                                            {
                                                html = html.Replace("@border_nkd", css_border_nkd);
                                            }
                                            else
                                            {
                                                html = html.Replace("@border_nkd", "");
                                            }

                                            html = html.Replace("@rowspan_kd", jumlah_rowspan_kd.ToString());

                                        }
                                    }

                                }
                            }

                        }

                        //nilai rapor                        
                        string bgrapor1 = "#B3E2ED";
                        string bgrapor2 = "#CEEFF6";

                        id_all++;

                        html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; @border_nkd\">" +
                                    "NILAI RAPOR" +
                                "</td>";

                        html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; \">" +
                                    "PPK" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "</tr>";

                        if (ada_praktik) //jika ada nilai praktik
                        {
                            id_all++;

                            html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; \">" +
                                        "P" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "</td>";

                            html += "</tr>";

                            html = html.Replace("@rowspan_kd", "2");
                        }
                        else
                        {
                            html = html.Replace("@rowspan_kd", "1");
                        }

                    }

                }
            }

            html += "</table>" +
                    "</div>";

            ltrStatistikPenilaian.Text = html;
        }

        protected void ShowStatistikKURTILAS()
        {
            string html = "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                            "<table class=\"table\">" +
                                "<tr>" +
                                    "<td colspan=\"3\" style=\"font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser-2.svg") + "\" style=\"height: 25px; width: 25px;\" />" +
                                        "&nbsp;&nbsp;" +
                                        "&nbsp;&nbsp;" +
                                        "Struktur Penilaian" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Nilai Rata-Rata" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Nilai Tertinggi" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Nilai Terrendah" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<i class=\"fa fa-user-circle\"></i> > KKM" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "% > KKM" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<i class=\"fa fa-user-circle\"></i> < KKM" +
                                    "</td>" +
                                    "<td style=\"width: 100px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "% < KKM" +
                                    "</td>" +
                                "</tr>";

            html += "<tr>" +
                        "<td colspan=\"10\" style=\"font-size: smaller; color: grey; padding: 2px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 0px; border-color: #2485a9; background-color: #2485a9;\">" +
                        "</td>" +
                    "</tr>";

            html += "<tr>" +
                        "<td colspan=\"10\" style=\"font-size: smaller; color: grey; padding: 2px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 0px; border-color: white; background-color: white;\">" +
                        "</td>" +
                    "</tr>";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(AtributPenilaian.Kelas);
            if (m_kelas != null)
            {
                //struktur nilai
                List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                        AtributPenilaian.TahunAjaran, AtributPenilaian.Semester, AtributPenilaian.Kelas, AtributPenilaian.Mapel
                    );

                if (lst_stuktur_nilai.Count == 1)
                {
                    DAO_Rapor_StrukturNilai.StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                    if (struktur_nilai.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS)
                    {
                        //load kurtilas ap
                        List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian =
                            DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                        bool ada_rowspan_ap = false;
                        bool ada_rowspan_kd = false;

                        int jumlah_rowspan_ap = 0;
                        int jumlah_rowspan_kd = 0;

                        string bg1 = "#FFFFFF";
                        string bg2 = "#fbfbfb";                        

                        int id_all = 0;
                        int id_fixed_col = 3;
                        foreach (Rapor_StrukturNilai_KURTILAS_AP m_sn_ap in lst_aspek_penilaian)
                        {

                            if (m_sn_ap != null)
                            {
                                if (m_sn_ap.JenisPerhitungan != null)
                                {
                                    Rapor_AspekPenilaian m_ap =
                                        DAO_Rapor_AspekPenilaian.GetByID_Entity(m_sn_ap.Rel_Rapor_AspekPenilaian.ToString());

                                    if (m_ap != null)
                                    {
                                        if (m_ap.Nama != null)
                                        {
                                            jumlah_rowspan_ap = 0;
                                            ada_rowspan_ap = true;

                                            //load kurtilas kd
                                            List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar =
                                                DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(m_sn_ap.Kode.ToString());

                                            foreach (Rapor_StrukturNilai_KURTILAS_KD m_sn_kd in lst_kompetensi_dasar)
                                            {

                                                if (m_sn_kd != null)
                                                {
                                                    if (m_sn_kd.JenisPerhitungan != null)
                                                    {

                                                        Rapor_KompetensiDasar m_kd =
                                                            DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                        if (m_kd != null)
                                                        {
                                                            if (m_kd.Nama != null)
                                                            {
                                                                jumlah_rowspan_kd = 0;
                                                                ada_rowspan_kd = true;

                                                                //load kurtilas kp
                                                                List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                                                    DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                                                                foreach (Rapor_StrukturNilai_KURTILAS_KP m_sn_kp in lst_komponen_penilaian)
                                                                {

                                                                    Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                    if (m_kp != null)
                                                                    {
                                                                        if (m_kp.Nama != null)
                                                                        {
                                                                            jumlah_rowspan_ap++;
                                                                            jumlah_rowspan_kd++;
                                                                            id_all++;
                                                                            
                                                                            string bg = (id_all % 2 == 0 ? bg1 : bg2);

                                                                            html += "<tr>";

                                                                            if (ada_rowspan_ap)
                                                                            {
                                                                                html += "<td rowspan=\"@rowspan_ap\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                            (
                                                                                                m_sn_ap.Poin.Trim() != ""
                                                                                                ? m_sn_ap.Poin.Trim() + "&nbsp;"
                                                                                                : ""
                                                                                            ) +
                                                                                            Libs.GetHTMLSimpleText(m_ap.Nama).Trim() +
                                                                                        "</td>";
                                                                                ada_rowspan_ap = false;
                                                                            }

                                                                            if (ada_rowspan_kd)
                                                                            {
                                                                                html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; @border_nkd\">" +
                                                                                            (
                                                                                                m_sn_kd.Poin.Trim() != ""
                                                                                                ? m_sn_kd.Poin.Trim() + "&nbsp;"
                                                                                                : ""
                                                                                            ) +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama).Trim() +
                                                                                        "</td>";
                                                                                ada_rowspan_kd = false;
                                                                            }

                                                                            html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; \">" +
                                                                                        Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                                                        (
                                                                                            m_sn_kp.IsKomponenRapor
                                                                                            ? "<sup title=' Komponen Rapor '>" +
                                                                                                "<i class='fa fa-check-circle' style='color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;'></i>" +
                                                                                              "</sup>"
                                                                                            : ""
                                                                                        ) +
                                                                                        (
                                                                                            m_sn_kp.BobotNK > 0
                                                                                            ? "&nbsp;" +
                                                                                              "<sup class='badge' style='background-color: #B7770D; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                                Math.Round(m_sn_kp.BobotNK, 0).ToString() + "%" +
                                                                                              "</sup>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                    "</td>";

                                                                            html += "</tr>";

                                                                        }
                                                                    }

                                                                }

                                                                bool ada_nkd = false;
                                                                string css_border_nkd = " border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px;";

                                                                //add nkd
                                                                if (lst_komponen_penilaian.Count > 1)
                                                                {
                                                                    ada_nkd = true;
                                                                    jumlah_rowspan_ap++;
                                                                    jumlah_rowspan_kd++;
                                                                    id_all++;

                                                                    html += "<tr>";

                                                                    if (ada_rowspan_ap)
                                                                    {
                                                                        html += "<td rowspan=\"@rowspan_ap\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px;\">" +
                                                                                    (
                                                                                        m_sn_ap.Poin.Trim() != ""
                                                                                        ? m_sn_ap.Poin + "&nbsp;"
                                                                                        : ""
                                                                                    ) +
                                                                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                                                "</td>";
                                                                        ada_rowspan_ap = false;
                                                                    }

                                                                    if (ada_rowspan_kd)
                                                                    {
                                                                        html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px; @border_nkd\">" +
                                                                                    (
                                                                                        m_sn_kd.Poin.Trim() != ""
                                                                                        ? m_sn_kd.Poin + "&nbsp;"
                                                                                        : ""
                                                                                    ) +
                                                                                    Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                                                "</td>";
                                                                        ada_rowspan_kd = false;
                                                                    }

                                                                    string bgnkd1 = "#EFECDC";
                                                                    string bgnkd2 = "#EFECDC";                                                                    

                                                                    html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;" + css_border_nkd + "\">" +
                                                                                NKD +
                                                                                (
                                                                                    m_sn_kd.IsKomponenRapor
                                                                                    ? "<sup title=' Komponen Rapor '>" +
                                                                                        "<i class='fa fa-check-circle' style='color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;'></i>" +
                                                                                        "</sup>"
                                                                                    : ""
                                                                                ) +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";

                                                                    html += "</tr>";
                                                                }

                                                                if (ada_nkd)
                                                                {
                                                                    html = html.Replace("@border_nkd", css_border_nkd);
                                                                }
                                                                else
                                                                {
                                                                    html = html.Replace("@border_nkd", "");
                                                                }

                                                                html = html.Replace("@rowspan_kd", jumlah_rowspan_kd.ToString());

                                                            }
                                                        }

                                                    }
                                                }

                                            }

                                            html += "<tr>" +
                                                        "<td colspan=\"10\" style=\"font-size: smaller; color: grey; padding: 1px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 1px; border-color: #0F9D58; background-color: #0F9D58;\">" +
                                                        "</td>" +
                                                    "</tr>";

                                            html = html.Replace("@rowspan_ap", jumlah_rowspan_ap.ToString());

                                        }
                                    }

                                }
                            }

                        }

                        //nilai rapor
                        id_all++;

                        //nilai rapor pengetahuan
                        string bgrapor1 = "#B3E2ED";
                        string bgrapor2 = "#CEEFF6";

                        html += "<tr>";

                        html += "<td rowspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "NILAI RAPOR" +
                                "</td>";

                        html += "<td colspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;\">" +
                                    Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "</tr>";

                        id_all++;
                        id_all++;

                        //nilai rapor keterampilan
                        html += "<tr>";

                        html += "<td colspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;\">" +
                                    Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_2\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_3\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_4\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_5\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_6\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_7\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "</tr>";

                    }

                }
            }

            html +=     "</table>" +
                    "</div>";

            ltrStatistikPenilaian.Text = html;
        }

        protected void ShowInputDeskripsiPenilaian()
        {
            string html = "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                            "<table class=\"table\">" +
                                "<tr>" +
                                    "<td colspan=\"3\" style=\"font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser-2.svg") + "\" style=\"height: 25px; width: 25px;\" />" +
                                        "&nbsp;&nbsp;" +
                                        "&nbsp;&nbsp;" +
                                        "Jenis Tagihan" +
                                    "</td>" +
                                    "<td style=\"width: 650px; text-align: center; font-size: smaller; vertical-align: middle; background-color: #4B4B4B; font-weight: bold; color: white; border-style: none; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: none; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);\">" +
                                        "Deskripsi" +
                                    "</td>" +
                                "</tr>";

            html += "<tr>" +
                        "<td colspan=\"4\" style=\"font-size: smaller; color: grey; padding: 2px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 0px; border-color: #2485a9; background-color: #2485a9;\">" +
                        "</td>" +
                    "</tr>";

            html += "<tr>" +
                        "<td colspan=\"4\" style=\"font-size: smaller; color: grey; padding: 2px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 0px; border-color: white; background-color: white;\">" +
                        "</td>" +
                    "</tr>";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(AtributPenilaian.Kelas);
            if (m_kelas != null)
            {
                //struktur nilai
                List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                        AtributPenilaian.TahunAjaran, AtributPenilaian.Semester, AtributPenilaian.Kelas, AtributPenilaian.Mapel
                    );

                if (lst_stuktur_nilai.Count == 1)
                {
                    DAO_Rapor_StrukturNilai.StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                    List<Rapor_StrukturNilai_Deskripsi> lst_deskripsi = DAO_Rapor_StrukturNilai_Deskripsi.GetAllByTABySMByKelasByMapel_Entity(
                            struktur_nilai.TahunAjaran,
                            struktur_nilai.Semester,
                            struktur_nilai.Rel_Kelas,
                            struktur_nilai.Rel_Mapel.ToString()
                        );
                    //lnkSaveDeskripsiPenilaian.Visible = !IsReadOnly(struktur_nilai);
                    if (
                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && Libs.GetQueryString("kdgk").Trim() != "" &&
                            DAO_FormasiGuruMapel.GetByGuru_Entity(Libs.LOGGED_USER_M.NoInduk).FindAll(
                                m0 => m0.TahunAjaran.ToString().ToUpper() == struktur_nilai.TahunAjaran.ToString().ToUpper() &&
                                      m0.Semester.ToString().ToUpper() == struktur_nilai.Semester.ToString().ToUpper() &&
                                      m0.Rel_Kelas.ToString().ToUpper() == struktur_nilai.Rel_Kelas.ToString().ToUpper() &&
                                      m0.Rel_Mapel.ToString().ToUpper() == struktur_nilai.Rel_Mapel.ToString().ToUpper()
                            ).Count == 0
                        )
                    {
                        lnkSaveDeskripsiPenilaian.Visible = false;
                    }
                    else
                    {
                        lnkSaveDeskripsiPenilaian.Visible = true;
                    }

                    if (struktur_nilai.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS)
                    {
                        //load kurtilas ap
                        List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian =
                            DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                        bool ada_rowspan_ap = false;
                        bool ada_rowspan_kd = false;

                        int jumlah_rowspan_ap = 0;
                        int jumlah_rowspan_kd = 0;

                        string bg1 = "#FFFFFF";
                        string bg2 = "#fbfbfb";

                        int id_all = 0;
                        int id_fixed_col = 3;
                        int id_kp = 0;
                        foreach (Rapor_StrukturNilai_KURTILAS_AP m_sn_ap in lst_aspek_penilaian)
                        {

                            if (m_sn_ap != null)
                            {
                                if (m_sn_ap.JenisPerhitungan != null)
                                {
                                    Rapor_AspekPenilaian m_ap =
                                        DAO_Rapor_AspekPenilaian.GetByID_Entity(m_sn_ap.Rel_Rapor_AspekPenilaian.ToString());

                                    if (m_ap != null)
                                    {
                                        if (m_ap.Nama != null)
                                        {
                                            jumlah_rowspan_ap = 0;
                                            ada_rowspan_ap = true;

                                            //load kurtilas kd
                                            List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar =
                                                DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(m_sn_ap.Kode.ToString());

                                            foreach (Rapor_StrukturNilai_KURTILAS_KD m_sn_kd in lst_kompetensi_dasar)
                                            {

                                                if (m_sn_kd != null)
                                                {
                                                    if (m_sn_kd.JenisPerhitungan != null)
                                                    {

                                                        Rapor_KompetensiDasar m_kd =
                                                            DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                        if (m_kd != null)
                                                        {
                                                            if (m_kd.Nama != null)
                                                            {
                                                                jumlah_rowspan_kd = 0;
                                                                ada_rowspan_kd = true;

                                                                //load kurtilas kp
                                                                List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                                                    DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                                                                foreach (Rapor_StrukturNilai_KURTILAS_KP m_sn_kp in lst_komponen_penilaian)
                                                                {

                                                                    Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                    if (m_kp != null)
                                                                    {
                                                                        if (m_kp.Nama != null)
                                                                        {
                                                                            Rapor_StrukturNilai_Deskripsi m_deskripsi = lst_deskripsi.FindAll(
                                                                                    m0 => m0.Rel_KelasDet.ToString().Trim().ToUpper() == AtributPenilaian.KelasDet.Trim().ToUpper() &&
                                                                                          m0.Rel_StrukturNilai.ToString().Trim().ToUpper() == struktur_nilai.Kode.ToString().Trim().ToUpper() &&
                                                                                          m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == m_sn_ap.Kode.ToString().Trim().ToUpper() &&
                                                                                          m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == m_sn_kd.Kode.ToString().Trim().ToUpper() &&
                                                                                          m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == m_sn_kp.Kode.ToString().Trim().ToUpper()
                                                                                ).FirstOrDefault();

                                                                            string deskripsi = "";
                                                                            if (m_deskripsi != null)
                                                                            {
                                                                                if (m_deskripsi.TahunAjaran != null)
                                                                                {
                                                                                    deskripsi = Libs.GetHTMLNoParagraphDiAwal(m_deskripsi.Deskripsi);
                                                                                }
                                                                            }
                                                                            //else
                                                                            //{
                                                                            //    m_deskripsi = lst_deskripsi.FindAll(
                                                                            //        m0 => m0.Rel_StrukturNilai.ToString().Trim().ToUpper() == struktur_nilai.Kode.ToString().Trim().ToUpper() &&
                                                                            //              m0.Rel_StrukturNilai_AP.ToString().Trim().ToUpper() == m_sn_ap.Kode.ToString().Trim().ToUpper() &&
                                                                            //              m0.Rel_StrukturNilai_KD.ToString().Trim().ToUpper() == m_sn_kd.Kode.ToString().Trim().ToUpper() &&
                                                                            //              m0.Rel_StrukturNilai_KP.ToString().Trim().ToUpper() == m_sn_kp.Kode.ToString().Trim().ToUpper()
                                                                            //    ).FirstOrDefault();

                                                                            //    if (m_deskripsi != null)
                                                                            //    {
                                                                            //        if (m_deskripsi.TahunAjaran != null)
                                                                            //        {
                                                                            //            deskripsi = Libs.GetHTMLNoParagraphDiAwal(m_deskripsi.Deskripsi);
                                                                            //        }
                                                                            //    }
                                                                            //}

                                                                            string id_unique_kp = m_sn_kp.Kode.ToString().Replace("-", "_");
                                                                            string s_ctl_textbox = "";
                                                                            //if (IsReadOnly(struktur_nilai))
                                                                            //{
                                                                            //    s_ctl_textbox = "<div style=\"width: 100%; padding: 10px; text-align: justify;\">" + deskripsi + "</div>";
                                                                            //}
                                                                            //else
                                                                            //{
                                                                                s_ctl_textbox = "<input value=\"" + deskripsi + "\" class=\"cls_" + id_kp.ToString() + "\" name=\"txt_kp[]\" id=\"txt_" + id_unique_kp + "\" type=\"textbox\" style=\"min-width: 300px;\" />" +
                                                                                                "<input value=\"" + deskripsi + "\" " +
                                                                                                        "lang=\"" +
                                                                                                            struktur_nilai.Kode.ToString().ToString() + "|" +
                                                                                                            m_sn_ap.Kode.ToString().ToString() + "|" +
                                                                                                            m_sn_kd.Kode.ToString().ToString() + "|" +
                                                                                                            m_sn_kp.Kode.ToString() +
                                                                                                        "\" type=\"hidden\" name=\"hfl_kp[]\" id=\"hfl_" + id_unique_kp + "\" />";
                                                                            //}

                                                                            jumlah_rowspan_ap++;
                                                                            jumlah_rowspan_kd++;
                                                                            id_all++;

                                                                            string bg = (id_all % 2 == 0 ? bg1 : bg2);

                                                                            html += "<tr>";

                                                                            if (ada_rowspan_ap)
                                                                            {
                                                                                html += "<td rowspan=\"@rowspan_ap\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                            (
                                                                                                m_sn_ap.Poin.Trim() != ""
                                                                                                ? m_sn_ap.Poin.Trim() + "&nbsp;"
                                                                                                : ""
                                                                                            ) +
                                                                                            Libs.GetHTMLSimpleText(m_ap.Nama).Trim() +
                                                                                        "</td>";
                                                                                ada_rowspan_ap = false;
                                                                            }

                                                                            if (ada_rowspan_kd)
                                                                            {
                                                                                html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; @border_nkd\">" +
                                                                                            (
                                                                                                m_sn_kd.Poin.Trim() != ""
                                                                                                ? m_sn_kd.Poin.Trim() + "&nbsp;"
                                                                                                : ""
                                                                                            ) +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama).Trim() +
                                                                                        "</td>";
                                                                                ada_rowspan_kd = false;
                                                                            }

                                                                            html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2; \">" +
                                                                                        Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                                                        (
                                                                                            m_sn_kp.IsKomponenRapor
                                                                                            ? "<sup title=' Komponen Rapor '>" +
                                                                                                "<i class='fa fa-check-circle' style='color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;'></i>" +
                                                                                              "</sup>"
                                                                                            : ""
                                                                                        ) +
                                                                                        (
                                                                                            m_sn_kp.BobotNK > 0
                                                                                            ? "&nbsp;" +
                                                                                              "<sup class='badge' style='background-color: #B7770D; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                                Math.Round(m_sn_kp.BobotNK, 0).ToString() + "%" +
                                                                                              "</sup>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</td>";

                                                                            html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bg + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                                                                        s_ctl_textbox +
                                                                                    "</td>";

                                                                            html += "</tr>";

                                                                            id_kp++;
                                                                        }
                                                                    }

                                                                }

                                                                bool ada_nkd = false;
                                                                string css_border_nkd = " border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px;";

                                                                //add nkd
                                                                if (lst_komponen_penilaian.Count > 1)
                                                                {
                                                                    ada_nkd = true;
                                                                    jumlah_rowspan_ap++;
                                                                    jumlah_rowspan_kd++;
                                                                    id_all++;

                                                                    html += "<tr>";

                                                                    if (ada_rowspan_ap)
                                                                    {
                                                                        html += "<td rowspan=\"@rowspan_ap\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px;\">" +
                                                                                    (
                                                                                        m_sn_ap.Poin.Trim() != ""
                                                                                        ? m_sn_ap.Poin + "&nbsp;"
                                                                                        : ""
                                                                                    ) +
                                                                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                                                "</td>";
                                                                        ada_rowspan_ap = false;
                                                                    }

                                                                    if (ada_rowspan_kd)
                                                                    {
                                                                        html += "<td rowspan=\"@rowspan_kd\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-bottom-style: solid; border-bottom-color: #B7770D; border-bottom-width: 3px; @border_nkd\">" +
                                                                                    (
                                                                                        m_sn_kd.Poin.Trim() != ""
                                                                                        ? m_sn_kd.Poin + "&nbsp;"
                                                                                        : ""
                                                                                    ) +
                                                                                    Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                                                "</td>";
                                                                        ada_rowspan_kd = false;
                                                                    }

                                                                    string bgnkd1 = "#EFECDC";
                                                                    string bgnkd2 = "#EFECDC";

                                                                    html += "<td style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: normal; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;" + css_border_nkd + "\">" +
                                                                                NKD +
                                                                                (
                                                                                    m_sn_kd.IsKomponenRapor
                                                                                    ? "<sup title=' Komponen Rapor '>" +
                                                                                        "<i class='fa fa-check-circle' style='color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;'></i>" +
                                                                                        "</sup>"
                                                                                    : ""
                                                                                ) +
                                                                            "</td>";

                                                                    html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgnkd1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;" + css_border_nkd + "\">" +
                                                                            "</td>";
                                                                }

                                                                if (ada_nkd)
                                                                {
                                                                    html = html.Replace("@border_nkd", css_border_nkd);
                                                                }
                                                                else
                                                                {
                                                                    html = html.Replace("@border_nkd", "");
                                                                }

                                                                html = html.Replace("@rowspan_kd", jumlah_rowspan_kd.ToString());

                                                            }
                                                        }

                                                    }
                                                }

                                            }

                                            html += "<tr>" +
                                                        "<td colspan=\"10\" style=\"font-size: smaller; color: grey; padding: 1px; vertical-align: middle; font-weight: bold; border-style: solid; border-width: 1px; border-color: #0F9D58; background-color: #0F9D58;\">" +
                                                        "</td>" +
                                                    "</tr>";

                                            html = html.Replace("@rowspan_ap", jumlah_rowspan_ap.ToString());

                                        }
                                    }

                                }
                            }

                        }

                        //nilai rapor
                        id_all++;

                        //nilai rapor pengetahuan
                        string bgrapor1 = "#B3E2ED";
                        string bgrapor2 = "#CEEFF6";

                        html += "<tr>";

                        html += "<td rowspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                    "NILAI RAPOR" +
                                "</td>";

                        html += "<td colspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;\">" +
                                    Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor1 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "</tr>";

                        id_all++;
                        id_all++;

                        //nilai rapor keterampilan
                        html += "<tr>";

                        html += "<td colspan=\"2\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC; border-right-style: solid; border-right-width: 3px; border-right-color: #40B3D2;\">" +
                                    Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN +
                                "</td>";

                        html += "<td id=\"td_" + (id_all + id_fixed_col).ToString() + "_1\" style=\"text-align: center; font-size: small; color: black; padding: 2px; vertical-align: middle; background-color: " + bgrapor2 + "; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "</td>";

                        html += "</tr>";

                    }

                }
            }

            html += "</table>" +
                    "</div>";

            ltrDeskripsiPenilaian.Text = html;
        }

        protected void lnkDeskripsiPenilaian_Click(object sender, EventArgs e)
        {
            ShowInputDeskripsiPenilaian();
            txtKeyAction.Value = JenisAction.DoShowDeskripsiPenilaian.ToString();
        }

        protected void SaveDeskripsiKP()
        {
            string[] arr_deskripsi = txtParseDeskripsi_KP.Value.Split(new string[] { SEP_DES }, StringSplitOptions.None);
            foreach (string item_deskripsi in arr_deskripsi)
            {
                string[] arr_item_deskripsi = item_deskripsi.Split(new string[] { SEP_KODE_DES }, StringSplitOptions.None);
                if (arr_item_deskripsi.Length == 2)
                {
                    string[] arr_sn_item_deskripsi = arr_item_deskripsi[0].Split(new string[] { "|" }, StringSplitOptions.None);
                    if (arr_sn_item_deskripsi.Length == 4)
                    {
                        Rapor_StrukturNilai_Deskripsi m = new Rapor_StrukturNilai_Deskripsi
                        {
                            Kode = Guid.NewGuid(),
                            TahunAjaran = QS.GetTahunAjaran(),
                            Semester = QS.GetSemester(),
                            Rel_KelasDet = QS.GetKelas(),
                            Rel_StrukturNilai = arr_sn_item_deskripsi[0],
                            Rel_StrukturNilai_AP = arr_sn_item_deskripsi[1],
                            Rel_StrukturNilai_KD = arr_sn_item_deskripsi[2],
                            Rel_StrukturNilai_KP = arr_sn_item_deskripsi[3],
                            Deskripsi = arr_item_deskripsi[1]
                        };
                        DAO_Rapor_StrukturNilai_Deskripsi.Save(m);
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
            }
        }

        protected void lnkSaveDeskripsiPenilaian_Click(object sender, EventArgs e)
        {
            SaveDeskripsiKP();
        }
    }
}