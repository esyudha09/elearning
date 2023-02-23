using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_NilaiSiswa : System.Web.UI.Page
    {
        private const string LEBAR_COL_DEFAULT = "100";
        private const string LEBAR_COL_DEFAULT2 = "100";
        private const string LEBAR_COL_DEFAULT_NA = "100";

        public enum JenisAction
        {
            ShowPilihanSemester,
            ShowPengaturanLTS,
            DoUpdate,
            DoUpdateNilaiRapor
        }

        protected class NILAI_COL
        {
            public int IdKolom { get; set; }
            public decimal Bobot { get; set; }
            public string BluePrintFormula { get; set; }
        }

        protected class COL_KP
        {
            public Guid KodeAP { get; set; }
            public decimal BobotAP { get; set; }
            public Guid KodeKP { get; set; }
            public int IdKolom { get; set; }
        }

        protected class FORMULA_KP
        {
            public int IdKolom { get; set; }
            public string BluePrintFormula { get; set; }
        }

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

            public static string GetMapelSikap()
            {
                string ms = Libs.GetQueryString("ms");
                return ms;
            }

            public static string GetGuru()
            {
                string guru = Libs.GetQueryString("g");
                if (guru.Trim() == "") return Libs.LOGGED_USER_M.NoInduk;
                return guru;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.ShowSubHeaderGuru = true;
            this.Master.ShowHeaderSubTitle = false;
            this.Master.SelectMenuGuru_Penilaian();

            InitURLOnMenu();

            if (!IsPostBack)
            {
                txtSemester.Value = QS.GetSemester();
                div_button_settings.Visible = false;    
                if (QS.GetSemester().Trim() != "" && (QS.GetMapel().Trim() != "" || Libs.GetQueryString("ms").Trim() != ""))
                {
                    div_button_settings.Visible = true;
                    LoadData();
                }
                else
                {
                    RenderCenterMenu();
                }
            }

            _UI.InitModalListNilai(
                    this.Page,
                    ltrListNilaiAkademik, ltrListNilaiEkskul, ltrListNilaiSikap, ltrListNilaiVolunteer, ltrListNilaiRapor, 
                    QS.GetTahunAjaran(), QS.GetMapel(), QS.GetKelas(), QS.GetGuru()
                );
            ShowListPilihKDUntukLTS();

            lnkPengaturanLTS.Visible = true;
            if (QS.GetMapelSikap().Trim() != "")
            {
                lnkPengaturanLTS.Visible = false;
            }

            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
            {
                li_nilai_ekskul.Visible = true;
                li_nilai_rapor.Visible = true;
                li_nilai_volunteer.Visible = true;
            }
            else
            {
                li_nilai_ekskul.Visible = false;
                //li_nilai_rapor.Visible = false;
                li_nilai_volunteer.Visible = false;
            }
            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
                this.Master.SetURLGuru_Penilaian("");
                this.Master.SetURLGuru_Siswa("");
                this.Master.SetURLGuru_TimeLine("");
            }
        }

        protected void LoadData()
        {
            LoadDataNilai(QS.GetSemester());
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

        protected void ShowBelumAdaPengaturanNilai()
        {
            ltrCenter.Text = "<div style=\"margin: 0 auto; display: table; color: grey; margin-top: 50px;\">" +
                                "<i class=\"fa fa-exclamation-triangle\" style=\"color: #b0b0b0; font-size: 40pt;\"></i>" +
                             "</div>" +
                             "<br />" +
                             "<div style=\"margin: 0 auto; display: table; color: grey;\">" +
                                "Struktur nilai belum dilakukan" +
                             "</div>" +
                             "<div style=\"margin: 0 auto; display: table; color: grey;\">" +
                                "pengaturan oleh admin" +
                             "</div>";
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
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                            ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                            : ""
                                        ) +
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
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                        ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                        : ""
                                    ) +
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
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
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
                            "&s=" + Libs.GetQueryString("s") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
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
            this.Master.SetURLGuru_Penilaian("-");
        }

        protected void LoadDataNilai(string semester)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";

            string tahun_ajaran = QS.GetTahunAjaran();
            string rel_kelas = "";
            string rel_kelas_det = QS.GetKelas();
            string rel_mapel = QS.GetMapel();
            string rel_mapel_sikap = QS.GetMapelSikap();
            string rel_mapel_sikap_info = QS.GetMapel();
            if (rel_mapel_sikap.Trim() != "") rel_mapel = rel_mapel_sikap;

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                }
            }

            string nama_mapel_sikap = "";
            if (rel_mapel_sikap_info.Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel_sikap_info);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        nama_mapel_sikap = m_mapel.Nama;
                    }
                }
            }

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //status bar
                    ltrStatusBar.Text = "";
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            ltrStatusBar.Text += "<span style=\"font-weight: bold;\">" + 
                                                    m_mapel.Nama +
                                                    (
                                                        m_mapel.Kode.ToString().ToUpper().Trim() == DAO_Rapor_Semester.KODE_SIKAP.ToUpper().Trim() ||
                                                        m_mapel.Kode.ToString().ToUpper().Trim() == DAO_Rapor_Semester.KODE_SIKAP_SOSIAL.ToUpper().Trim() ||
                                                        m_mapel.Kode.ToString().ToUpper().Trim() == DAO_Rapor_Semester.KODE_SIKAP_SPIRITUAL.ToUpper().Trim()
                                                        ? " (LTS) "
                                                        : ""
                                                    ) +
                                                    (
                                                        nama_mapel_sikap.Trim().ToLower() != m_mapel.Nama.Trim().ToLower() && nama_mapel_sikap.Trim() != ""
                                                        ? ",&nbsp;" + nama_mapel_sikap
                                                        : ""
                                                    ) +
                                                 "</span>";
                        }
                    }

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

                    //struktur nilai
                    List<Rapor_StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas, rel_mapel
                        );

                    if (lst_stuktur_nilai.Count == 1)
                    {
                        Rapor_StrukturNilai m_struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                        if (m_struktur_nilai != null)
                        {
                            if (m_struktur_nilai.TahunAjaran != null)
                            {
                                bool is_readonly = _UI.IsReadonlyNilai(
                                        m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas(), rel_mapel, rel_mapel_sikap, rel_mapel_sikap_info,
                                        m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester
                                    );
                                
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

                                string s_kurikulum = DAO_Rapor_StrukturNilai.GetKurikulumByKelas(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());
                                //format 1 nilai gabung tanpa kelompokan KP
                                //--------------------------------------------------
                                if (!(m_struktur_nilai.IsKelompokanKP && m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()))
                                {
                                    LoadFormat1(semester, m_struktur_nilai, m_kelas, is_readonly, s_kurikulum);
                                }
                                //end format 1

                                //format 1 nilai kelompokan KP, contohnya mapel Al-quran
                                //--------------------------------------------------
                                else
                                {
                                    LoadFormat2(semester, m_struktur_nilai, m_kelas, is_readonly, s_kurikulum);
                                }
                                //end format 2

                            }
                            //end if struktur tahun ajaran not null
                        }
                    }

                }//end if nama not null
            }

            mvMain.ActiveViewIndex = 0;
        }

        protected void LoadFormat1(string semester, Rapor_StrukturNilai m_struktur_nilai, Kelas m_kelas, bool is_readonly, string kurikulum)
        {
            string tahun_ajaran = QS.GetTahunAjaran();
            string rel_kelas = "";
            string rel_kelas_det = QS.GetKelas();
            string rel_mapel = QS.GetMapel();
            string rel_mapel_sikap = QS.GetMapelSikap();

            List<NILAI_COL> lst_kolom_nilai_nkd = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap = new List<NILAI_COL>();

            List<NILAI_COL> lst_kolom_nilai_nap_forrapor = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap_forrapor_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap_ukk_forrapor = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap_ukk_forrapor_bobot = new List<NILAI_COL>();

            List<NILAI_COL> lst_kolom_nilai_nap_forrapor_all = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap_forrapor_bobot_all = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap_ukk_forrapor_all = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap_ukk_forrapor_bobot_all = new List<NILAI_COL>();

            string s_kolom = "";
            string s_content = "";
            string s_kolom_width = "";

            string s_merge_cells = "";
            string s_kolom_style = "";

            string s_js_arr_kolom1 = "";
            string s_js_arr_kolom2 = "";
            string s_js_arr_kolom3 = "";
            string s_formula = "";

            string css_bg = "#fff";
            string css_bg_nkd = "#fff";
            string css_bg_nap = "#fff";
            string css_bg_nilaiakhir = "#fff";
            string css_bg_kd_for_rapor = "#fff";

            int id_jml_fixed_row = 3;
            int id_col_mulai_content = 4;
            int id_col_all = id_col_mulai_content;
            int id_col_nilai_rapor = 0;

            List<string> lst_ap = new List<string>();
            List<string> lst_kd = new List<string>();
            List<string> lst_kp = new List<string>();

            string s_arr_js_siswa = "";
            string s_arr_js_ap = "";
            string s_arr_js_kd = "";
            string s_arr_js_kp = "";
            string s_arr_js_formula = "";

            string s_arr_kolom_nk = "";
            string s_arr_kolom_na = "";

            bool is_nilai_manual = false;

            lst_kolom_nilai_nkd.Clear();
            txtKKM.Value = Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

            //init js arr aspek penilaian/ap
            s_arr_js_ap = "";
            lst_ap.Clear();
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                               "\"\"";
                lst_ap.Add("");
            }
            //end init

            //init js arr kompetensi dasar/kd
            s_arr_js_kd = "";
            lst_kd.Clear();
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                               "\"\"";
                lst_kd.Add("");
            }
            //end init

            //init js arr komponen penilaian/kp
            s_arr_js_kp = "";
            lst_kp.Clear();
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                               "\"\"";
                lst_kp.Add("");
            }
            //end init

            //load ap
            int id_na = 1;
            List<Rapor_StrukturNilai_AP> lst_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(m_struktur_nilai.Kode.ToString());
            foreach (Rapor_StrukturNilai_AP m_struktur_ap in lst_struktur_ap)
            {
                int jml_merge_ap = 0;

                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        int jml_merge_kd = 0;

                        string s_formula_kd = "";
                        string s_formula_kd_gabung = "";

                        lst_kolom_nilai_nap_forrapor.Clear();
                        lst_kolom_nilai_nap_forrapor_bobot.Clear();
                        lst_kolom_nilai_nap_ukk_forrapor.Clear();
                        lst_kolom_nilai_nap_ukk_forrapor_bobot.Clear();

                        //load kd
                        int id_nk = 1;
                        int id_kd = 0;
                        List<Rapor_StrukturNilai_KD> lst_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KD m_struktur_kd in lst_struktur_kd)
                        {
                            id_kd++;
                            jml_merge_kd = 0;

                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (m_kd != null)
                            {
                                if (m_kd.Nama != null)
                                {
                                    s_formula = "";

                                    string s_formula_kp = "";

                                    //load kp
                                    List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(m_struktur_kd.Kode.ToString());
                                    foreach (Rapor_StrukturNilai_KP m_struktur_kp in lst_struktur_kp)
                                    {
                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_struktur_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                //add ap, kd, kp
                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + m_struktur_ap.Kode.ToString() + "\"";
                                                lst_ap.Add(m_struktur_ap.Kode.ToString());

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + m_struktur_kd.Kode.ToString() + "\"";
                                                lst_kd.Add(m_struktur_kd.Kode.ToString());

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + m_struktur_kp.Kode.ToString() + "\"";
                                                lst_kp.Add(m_struktur_kp.Kode.ToString());
                                                //end add ap, kd, kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"" + Libs.GetHTMLSimpleText(m_ap.Nama, true) + "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                   "\"" +
                                                                        (
                                                                            m_struktur_kd.Poin.Trim() != ""
                                                                            ? m_struktur_kd.Poin.Trim() +
                                                                              "<br />"
                                                                            : ""
                                                                        ) +
                                                                        Libs.GetPersingkatKalimat(Libs.GetHTMLSimpleText(m_kd.Nama, true)) +
                                                                        (
                                                                            m_struktur_kd.BobotAP > 0
                                                                            ? "&nbsp;" +
                                                                              "<sup class='badge' style='background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px; border-top-left-radius: 0px; border-top-right-radius: 0px;'>" +
                                                                                Math.Round(m_struktur_kd.BobotAP, 0).ToString() + "%" +
                                                                              "</sup>"
                                                                            : ""
                                                                        ) +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                                   "\"" +
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama, true) +
                                                                        (
                                                                            m_struktur_kp.BobotNK > 0
                                                                            ? "&nbsp;" +
                                                                              "<sup class='badge' style='background-color: #B7770D; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px; border-top-left-radius: 0px; border-top-right-radius: 0px;'>" +
                                                                                Math.Round(m_struktur_kp.BobotNK, 0).ToString() + "%" +
                                                                              "</sup>"
                                                                            : ""
                                                                        ) +
                                                                   "\"";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                //formula item kp
                                                s_formula_kp += (
                                                                    m_struktur_kd.JenisPerhitungan ==
                                                                        ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                                                    ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                      "(" +
                                                                            "IF(" +
                                                                                Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                                "= \"\", 0 ," +
                                                                                Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                            ")" +
                                                                            "*(" + (m_struktur_kp.BobotNK.ToString()) + "%)" +
                                                                      ")"
                                                                    : (
                                                                            m_struktur_kd.JenisPerhitungan ==
                                                                                ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                                            ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                              "IF(" +
                                                                                    Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                                    "= \"\", 0 ," +
                                                                                    Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                              ")"
                                                                            : ""
                                                                      )
                                                                );
                                                //end item formula kp

                                                id_col_all++;
                                                jml_merge_kd++;
                                                jml_merge_ap++;
                                            }
                                        }

                                    }
                                    //end load kp

                                    //generate formula untuk kp
                                    if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        s_formula = "IF(" +
                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() + ") <= 0, \"\", " +
                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() + ") " +
                                                     ")";
                                    }
                                    else if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        s_formula = "IF(" +
                                                        "ROUND((" + s_formula_kp + ")/" + jml_merge_kd.ToString() + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() + ") <= 0, \"\", " +
                                                        "ROUND((" + s_formula_kp + ")/" + jml_merge_kd.ToString() + ", " +
                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                             ")" +
                                                     ")";
                                    }
                                    //end generate

                                    //tambahkan ke formula kd
                                    s_formula_kd_gabung += (
                                            m_struktur_ap.JenisPerhitungan ==
                                                ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                            ? (s_formula_kd_gabung.Trim() != "" ? "+" : "") +
                                                "(" +
                                                    "(" +
                                                        "IF(" +
                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", 0, " +
                                                            Libs.GetColHeader(id_col_all + 1) + "#" +
                                                        ")" +
                                                    ")" +
                                                    "*(" + (m_struktur_kd.BobotAP.ToString()) + "%)" +
                                                ")"
                                            : (
                                                m_struktur_ap.JenisPerhitungan ==
                                                    ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                ? (s_formula_kd_gabung.Trim() != "" ? "+" : "") +
                                                    "(" +
                                                        "IF(" +
                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", 0, " +
                                                            Libs.GetColHeader(id_col_all + 1) + "#" +
                                                        ")" +
                                                    ")"
                                                : ""
                                                )
                                        );
                                    //end tambahkan ke formula kd

                                    //add content kolom nkd
                                    lst_kolom_nilai_nkd.Add(new NILAI_COL
                                    {
                                        BluePrintFormula = "=" + s_formula,
                                        IdKolom = id_col_all
                                    });
                                    //end content kolom nkd

                                    //tambahkan nk setelah kp
                                    //add ap to var arr js
                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                   "\"" + m_struktur_ap.Kode.ToString() + "\"";
                                    lst_ap.Add(m_struktur_ap.Kode.ToString());

                                    //add kd to var arr js
                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                   "\"" + m_struktur_kd.Kode.ToString() + "\"";
                                    lst_kd.Add(m_struktur_kd.Kode.ToString());

                                    //add kp to var arr js
                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                   "\"\"";
                                    lst_kp.Add("");
                                    //end add ap, kd & kp

                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                     LEBAR_COL_DEFAULT;

                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama, true) + "\"";

                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                       "\"" + Libs.GetPersingkatKalimat(Libs.GetHTMLSimpleText(m_kd.Nama, true)) + "\"";

                                    s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                       "\"NK" + id_nk.ToString() + "\"";

                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                    id_col_all++;
                                    jml_merge_kd++;
                                    jml_merge_ap++;
                                    id_nk++;
                                    //end tambahkan nk

                                    if (jml_merge_kd > 0)
                                    {
                                        s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                        "{ row: 1, col: " + (id_col_all - jml_merge_kd).ToString() + ", rowspan: 1, colspan: " + jml_merge_kd.ToString() + " }";
                                    }
                                    //end load kp
                                }
                            }
                        }
                        //end load kd
                        
                        if (m_struktur_ap.IsAdaPAT_UKK)
                        {
                            s_formula_kd = "";
                            //generate formula nap
                            //formula item kd dari nk
                            if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                            {
                                if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")" +
                                                            ", " +
                                                            "2" + //Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                        ") " +
                                                   ")";
                                }
                                else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() + " <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() +
                                                            ", " +
                                                            "2" + //Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                        ")" +
                                                   ")";
                                }
                            }
                            else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                            {
                                if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")" +
                                                            ", " +
                                                            "0" +
                                                        ") " +
                                                   ")";
                                }
                                else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() + " <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() +
                                                            ", " +
                                                            "0" +
                                                        ")" +
                                                   ")";
                                }
                            }
                            //end formula item kd dari nk
                            //end generate formula nap

                            //tambahkan KD For NA
                            //add ap to var arr js
                            s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_ap.Add(Constantas.SD.GetKode_NK_KD(m_struktur_ap.Kode.ToString()));

                            //add kd to var arr js
                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kd.Add(Constantas.SD.GetKode_NK_KD(m_struktur_ap.Kode.ToString()));

                            //add kp to var arr js
                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kp.Add(Constantas.SD.GetKode_NK_KD(m_struktur_ap.Kode.ToString()));
                            //end add ap, kd & kp

                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                    "NK<br />" +
                                                    Libs.GetHTMLSimpleText(m_ap.Nama, true) +
                                               "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                    "NK<br />" +
                                                    Libs.GetHTMLSimpleText(m_ap.Nama, true) +
                                               "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                    "NK<br />" +
                                                    Libs.GetHTMLSimpleText(m_ap.Nama, true) +
                                               "\"";

                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", rowspan: 2, colspan: 1 }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                            
                            lst_kolom_nilai_nap_forrapor.Add(new NILAI_COL
                            {
                                BluePrintFormula = "=" + s_formula_kd,
                                Bobot = 100,
                                IdKolom = id_col_all
                            });
                            lst_kolom_nilai_nap_forrapor_all.Add(new NILAI_COL
                            {
                                BluePrintFormula = "=" + s_formula_kd,
                                Bobot = 100,
                                IdKolom = id_col_all
                            });

                            id_col_all++;
                            jml_merge_kd++;
                            jml_merge_ap++;
                            id_nk++;
                            //end tambahkan KD For NA

                            //tambahkan KD For NA (bobot)
                            //add ap to var arr js
                            s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_BOBOT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_ap.Add(Constantas.SD.GetKode_NK_KD_BOBOT(m_struktur_ap.Kode.ToString()));

                            //add kd to var arr js
                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_BOBOT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kd.Add(Constantas.SD.GetKode_NK_KD_BOBOT(m_struktur_ap.Kode.ToString()));

                            //add kp to var arr js
                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_BOBOT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kp.Add(Constantas.SD.GetKode_NK_KD_BOBOT(m_struktur_ap.Kode.ToString()));
                            //end add ap, kd & kp

                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                "\"" +
                                                    "NK<br />" +
                                                    Libs.GetHTMLSimpleText(m_ap.Nama, true) +
                                                    "<br />" +
                                                    "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                        Math.Round(m_struktur_ap.Bobot_Non_PAT_UKK, 0).ToString() + "%" +
                                                    "</sup>" +
                                                "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                "\"" +
                                                    "NK<br />" +
                                                    Libs.GetHTMLSimpleText(m_ap.Nama, true) +
                                                    "<br />" +
                                                    "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                        Math.Round(m_struktur_ap.Bobot_Non_PAT_UKK, 0).ToString() + "%" +
                                                    "</sup>" +
                                                "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                "\"" +
                                                    "NK<br />" +
                                                    Libs.GetHTMLSimpleText(m_ap.Nama, true) +
                                                    "<br />" +
                                                    "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                        Math.Round(m_struktur_ap.Bobot_Non_PAT_UKK, 0).ToString() + "%" +
                                                    "</sup>" +
                                                "\"";

                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", rowspan: 2, colspan: 1 }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                            lst_kolom_nilai_nap_forrapor_bobot.Add(new NILAI_COL
                            {
                                BluePrintFormula = "=" +
                                    "ROUND(" +
                                        "(" +
                                            "IF(" +
                                                Libs.GetColHeader(id_col_all) + "# = \"\", 0, " +
                                                Libs.GetColHeader(id_col_all) + "#" +
                                            ")" +
                                        ")" +
                                        "*(" + (m_struktur_ap.Bobot_Non_PAT_UKK.ToString()) + "%)" +
                                    ",2)",
                                Bobot = m_struktur_ap.Bobot_Non_PAT_UKK,
                                IdKolom = id_col_all
                            });
                            lst_kolom_nilai_nap_forrapor_bobot_all.Add(new NILAI_COL
                            {
                                BluePrintFormula = "=" +
                                    "ROUND(" +
                                        "(" +
                                            "IF(" +
                                                Libs.GetColHeader(id_col_all) + "# = \"\", 0, " +
                                                Libs.GetColHeader(id_col_all) + "#" +
                                            ")" +
                                        ")" +
                                        "*(" + (m_struktur_ap.Bobot_Non_PAT_UKK.ToString()) + "%)" +
                                    ",2)",
                                Bobot = m_struktur_ap.Bobot_Non_PAT_UKK,
                                IdKolom = id_col_all
                            });

                            id_col_all++;
                            jml_merge_kd++;
                            jml_merge_ap++;
                            id_nk++;
                            //end tambahkan KD For NA (bobot)

                            //tambahkan UKK or PAT setelah kp
                            //add ap to var arr js
                            s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_UKKPAT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_ap.Add(Constantas.SD.GetKode_NK_KD_UKKPAT(m_struktur_ap.Kode.ToString()));

                            //add kd to var arr js
                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_UKKPAT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kd.Add(Constantas.SD.GetKode_NK_KD_UKKPAT(m_struktur_ap.Kode.ToString()));

                            //add kp to var arr js
                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_UKKPAT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kp.Add(Constantas.SD.GetKode_NK_KD_UKKPAT(m_struktur_ap.Kode.ToString()));
                            //end add ap, kd & kp

                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                    "PAT<br /><span style='font-size: xx-small; font-weight: normal;'>atau</span><br />UKK" +
                                               "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                    "PAT<br /><span style='font-size: xx-small; font-weight: normal;'>atau</span><br />UKK" +
                                               "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                    "PAT<br /><span style='font-size: xx-small; font-weight: normal;'>atau</span><br />UKK" +
                                               "\"";

                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", rowspan: 2, colspan: 1 }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                            lst_kolom_nilai_nap_ukk_forrapor.Add(new NILAI_COL
                            {
                                BluePrintFormula = "",
                                Bobot = 100,
                                IdKolom = id_col_all
                            });
                            lst_kolom_nilai_nap_ukk_forrapor_all.Add(new NILAI_COL
                            {
                                BluePrintFormula = "",
                                Bobot = 100,
                                IdKolom = id_col_all
                            });

                            id_col_all++;
                            jml_merge_kd++;
                            jml_merge_ap++;
                            id_nk++;
                            //end tambahkan nk  

                            //tambahkan UKK or PAT setelah kp
                            //add ap to var arr js
                            s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_UKKPAT_BOBOT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_ap.Add(Constantas.SD.GetKode_NK_KD_UKKPAT_BOBOT(m_struktur_ap.Kode.ToString()));

                            //add kd to var arr js
                            s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_UKKPAT_BOBOT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kd.Add(Constantas.SD.GetKode_NK_KD_UKKPAT_BOBOT(m_struktur_ap.Kode.ToString()));

                            //add kp to var arr js
                            s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                           "\"" + Constantas.SD.GetKode_NK_KD_UKKPAT_BOBOT(m_struktur_ap.Kode.ToString()) + "\"";
                            lst_kp.Add(Constantas.SD.GetKode_NK_KD_UKKPAT_BOBOT(m_struktur_ap.Kode.ToString()));
                            //end add ap, kd & kp

                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                "\"" +
                                                    "PAT" +
                                                    "<br />" +
                                                    "<span style='font-size: xx-small; font-weight: normal;'>atau</span>" +
                                                    "<br />" +
                                                    "UKK" +
                                                    "<br />" +
                                                    "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                        Math.Round(m_struktur_ap.Bobot_PAT_UKK, 0).ToString() + "%" +
                                                    "</sup>" +
                                                "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                "\"" +
                                                    "PAT" +
                                                    "<br />" +
                                                    "<span style='font-size: xx-small; font-weight: normal;'>atau</span>" +
                                                    "<br />" +
                                                    "UKK" +
                                                    "<br />" +
                                                    "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                        Math.Round(m_struktur_ap.Bobot_PAT_UKK, 0).ToString() + "%" +
                                                    "</sup>" +
                                                "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                "\"" +
                                                    "PAT" +
                                                    "<br />" +
                                                    "<span style='font-size: xx-small; font-weight: normal;'>atau</span>" +
                                                    "<br />" +
                                                    "UKK" +
                                                    "<br />" +
                                                    "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                        Math.Round(m_struktur_ap.Bobot_PAT_UKK, 0).ToString() + "%" +
                                                    "</sup>" +
                                                "\"";

                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", rowspan: 2, colspan: 1 }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                            lst_kolom_nilai_nap_ukk_forrapor_bobot.Add(new NILAI_COL
                            {
                                BluePrintFormula = "=" +
                                    "ROUND(" +
                                        "(" +
                                            "IF(" +
                                                Libs.GetColHeader(id_col_all) + "# = \"\", 0, " +
                                                Libs.GetColHeader(id_col_all) + "#" +
                                            ")" +
                                        ")" +
                                        "*(" + (m_struktur_ap.Bobot_PAT_UKK.ToString()) + "%)" +
                                    ",2)",
                                Bobot = m_struktur_ap.Bobot_PAT_UKK,
                                IdKolom = id_col_all
                            });
                            lst_kolom_nilai_nap_ukk_forrapor_bobot_all.Add(new NILAI_COL
                            {
                                BluePrintFormula = "=" +
                                    "ROUND(" +
                                        "(" +
                                            "IF(" +
                                                Libs.GetColHeader(id_col_all) + "# = \"\", 0, " +
                                                Libs.GetColHeader(id_col_all) + "#" +
                                            ")" +
                                        ")" +
                                        "*(" + (m_struktur_ap.Bobot_PAT_UKK.ToString()) + "%)" +
                                    ",2)",
                                Bobot = m_struktur_ap.Bobot_PAT_UKK,
                                IdKolom = id_col_all
                            });

                            id_col_all++;
                            jml_merge_kd++;
                            jml_merge_ap++;
                            id_nk++;
                            //end tambahkan UKK or PAT setelah kp
                        }

                        //tambahkan na setelah kp
                        //add ap to var arr js
                        s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                       "\"" + m_struktur_ap.Kode.ToString() + "\"";
                        lst_ap.Add(m_struktur_ap.Kode.ToString());

                        //add kd to var arr js
                        s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kd.Add("");

                        //add kp to var arr js
                        s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                       "\"\"";
                        lst_kp.Add("");
                        //end add ap, kd & kp

                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                         LEBAR_COL_DEFAULT;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"NA" +
                                                id_na.ToString() +
                                                (
                                                    m_struktur_ap.BobotRapor > 0
                                                    ? "&nbsp;" +
                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                        "</sup>"
                                                    : ""
                                                ) +
                                           "\"";

                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                           "\"NA" +
                                                id_na.ToString() +
                                                (
                                                    m_struktur_ap.BobotRapor > 0
                                                    ? "&nbsp;" +
                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                        "</sup>"
                                                    : ""
                                                ) +
                                           "\"";

                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                           "\"NA" +
                                                id_na.ToString() +
                                                (
                                                    m_struktur_ap.BobotRapor > 0
                                                    ? "&nbsp;" +
                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                        "</sup>"
                                                    : ""
                                                ) +
                                           "\"";
                        //end tambahkan na setelah kp

                        s_formula_kd = "";
                        if (m_struktur_ap.IsAdaPAT_UKK)
                        {
                            foreach (var item_kolom_nilai_nap_forrapor_bobot in lst_kolom_nilai_nap_forrapor_bobot)
                            {
                                s_formula_kd += "ROUND(" +
                                                    "IF(" +
                                                        Libs.GetColHeader(item_kolom_nilai_nap_forrapor_bobot.IdKolom + 1) + "# = \"\", 0, " +
                                                        Libs.GetColHeader(item_kolom_nilai_nap_forrapor_bobot.IdKolom + 1) + "#" +
                                                    ")" +
                                                ", 2)";
                            }

                            foreach (var item_kolom_nilai_nap_ukk_forrapor_bobot in lst_kolom_nilai_nap_ukk_forrapor_bobot)
                            {
                                s_formula_kd += (s_formula_kd.Trim() != "" ? "+" : "") +
                                                "ROUND(" +
                                                    "IF(" +
                                                        Libs.GetColHeader(item_kolom_nilai_nap_ukk_forrapor_bobot.IdKolom + 1) + "# = \"\", 0, " +
                                                        Libs.GetColHeader(item_kolom_nilai_nap_ukk_forrapor_bobot.IdKolom + 1) + "#" +
                                                    ")" +
                                                ", 2)";
                            }

                            if (s_formula_kd.Trim() != "")
                            {
                                s_formula_kd = "ROUND((" + s_formula_kd + "), 0)";
                            }
                        }
                        else if (!m_struktur_ap.IsAdaPAT_UKK)
                        {
                            //generate formula nap
                            //formula item kd dari nk
                            if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                            {
                                if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")" +
                                                            ", " +
                                                            "2" + //Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                        ") " +
                                                   ")";
                                }
                                else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() + " <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() +
                                                            ", " +
                                                            "2" + //Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                        ")" +
                                                   ")";
                                }
                            }
                            else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                            {
                                if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")" +
                                                            ", " +
                                                            "0" +
                                                        ") " +
                                                   ")";
                                }
                                else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                {
                                    s_formula_kd = "IF(" +
                                                        "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() + " <= 0, \"\", " +
                                                        "ROUND(" +
                                                            "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString() +
                                                            ", " +
                                                            "0" +
                                                        ")" +
                                                   ")";
                                }
                            }
                            //end formula item kd dari nk
                            //end generate formula nap
                        }

                        //add content kolom nap
                        lst_kolom_nilai_nap.Add(new NILAI_COL
                        {
                            BluePrintFormula = "=" + s_formula_kd,
                            IdKolom = id_col_all,
                            Bobot = m_struktur_ap.BobotRapor
                        });
                        s_formula_kd = "";
                        //end content kolom nap

                        //merge na
                        s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                         "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                        id_col_all++;
                        jml_merge_kd++;
                        jml_merge_ap++;
                        id_na++;
                        //end tambahkan na

                        if (jml_merge_ap > 0)
                        {
                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                            "{ row: 0, col: " + (id_col_all - jml_merge_ap).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                        }

                    }
                }
            }
            //end load ap

            //tambahkan kolom nilai akhir
            id_col_nilai_rapor = id_col_all;
            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                             LEBAR_COL_DEFAULT;

            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"" +
                                "NILAI AKHIR" +
                               "\"";

            s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"" +
                                "NILAI AKHIR" +
                               "\"";

            s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"" +
                                "NILAI AKHIR" +
                               "\"";
            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                             "{ row: 0, col: " + (id_col_all).ToString() + ", rowspan: 3, colspan: 1 }";
            //end tambahkan kolom nilai akhir

            s_merge_cells = (s_merge_cells.Trim() != "" ? ", " : "") +
                             s_merge_cells;

            s_kolom_width = (s_kolom_width.Trim() != "" ? ", " : "") +
                             s_kolom_width;

            s_js_arr_kolom1 = (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom1;

            s_js_arr_kolom2 = (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom2;

            s_js_arr_kolom3 = (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom3;

            //tambahkan capaian kedisiplinan siswa
            id_col_all++;
            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"LTS\" ";
            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                              "\"CAPAIAN KEDISIPLINAN\" ";
            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                              "\"KEHADIRAN\" ";

            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                             (rel_mapel_sikap.Trim() != "" ? "0" : LEBAR_COL_DEFAULT2);

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
                              "\"KETEPATAN WAKTU\" ";

            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                              (rel_mapel_sikap.Trim() != "" ? "0" : LEBAR_COL_DEFAULT2);

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
                              (rel_mapel_sikap.Trim() != "" ? "0" : LEBAR_COL_DEFAULT2);

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
                              "\"PENGGUNAAN KAMERA\" ";

            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                              (rel_mapel_sikap.Trim() != "" ? "0" : LEBAR_COL_DEFAULT2);

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
                      "]," +
                      "[" +
                            "\"#\", " +
                            "\"NIS\", " +
                            "\"NAMA SISWA\", " +
                            "\"L/P\" " +
                            s_js_arr_kolom3 +
                      "]";

            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                             "{ row: 0, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }," +
                             "{ row: 1, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }," +
                             "{ row: 2, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }";

            //load data siswa
            //list siswa

            int id_col_nilai_mulai = 4;
            //get list nilai jika ada
            if (rel_mapel_sikap.Trim() != "")
            {
                //nilai sikap
                Rapor_Sikap m_nilai = new Rapor_Sikap();
                m_nilai = DAO_Rapor_Sikap.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran, 
                        semester, 
                        rel_kelas_det,
                        (
                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel_sikap.Trim() != ""
                            ? (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel_sikap.Trim() != "" && rel_mapel.Trim() != ""
                                ? rel_mapel.ToString()
                                : ""
                              )
                            : rel_mapel.ToString()
                        ), 
                        rel_mapel_sikap
                    ).FirstOrDefault();

                List<Rapor_SikapSiswa> lst_nilai_siswa = null;
                List<Rapor_SikapSiswa_Det> lst_nilai_siswa_det = null;
                if (m_nilai != null)
                {
                    if (m_nilai.Kurikulum != null)
                    {
                        lst_nilai_siswa = DAO_Rapor_SikapSiswa.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                        lst_nilai_siswa_det = DAO_Rapor_SikapSiswa_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                    }
                }

                //init js arr siswa
                s_arr_js_siswa = "";
                for (int i = 0; i < id_jml_fixed_row; i++)
                {
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "''";
                }

                int id = 1;
                id_col_nilai_mulai = 4;
                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        m_kelas.Rel_Sekolah.ToString(),
                        rel_kelas_det,
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
                string s_js_arr_nilai = "";
                foreach (Siswa m_siswa in lst_siswa)
                {
                    css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                    css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                    css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                    css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                    css_bg_kd_for_rapor = (id % 2 == 0 ? " htBG19" : " htBG20");

                    s_js_arr_nilai = "";
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "'" + m_siswa.Kode.ToString() + "'";

                    for (int i = id_col_nilai_mulai; i < id_col_all - 4; i++)
                    {
                        string s_nilai = "";
                        bool is_kolom_nk = false;
                        bool is_kolom_na = false;
                        is_nilai_manual = false;

                        Rapor_SikapSiswa_Det m_nilai_det = new Rapor_SikapSiswa_Det();

                        if (m_nilai != null)
                        {
                            if (m_nilai.TahunAjaran != null)
                            {
                                m_nilai_det =
                                    lst_nilai_siswa_det.FindAll(
                                            m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                 m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i <= lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                 m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                 m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                        ).FirstOrDefault();
                            }
                        }

                        //get formula nilai nk
                        NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk != null)
                        {
                            if (m_nilai_col_nk.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_kd[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_nk = true;
                            }
                        }
                        //end get formula nilai nk

                        //get formula nilai nk for rapor
                        NILAI_COL m_nilai_col_nk_for_rapor = lst_kolom_nilai_nap_forrapor_all.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk_for_rapor != null)
                        {
                            if (m_nilai_col_nk_for_rapor.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk_for_rapor.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";
                            }
                        }
                        //end get formula nilai nk for rapor

                        //get formula nilai nk for rapor (by bobot)
                        NILAI_COL m_nilai_col_nk_for_rapor_by_bobot = lst_kolom_nilai_nap_forrapor_bobot_all.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk_for_rapor_by_bobot != null)
                        {
                            if (m_nilai_col_nk_for_rapor_by_bobot.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk_for_rapor_by_bobot.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";
                            }
                        }
                        //end get formula nilai nk for rapor (by bobot)

                        //get formula nilai nk UKK or PAT for rapor (by bobot)
                        NILAI_COL m_nilai_col_nk_ukk_pat_for_rapor_by_bobot = lst_kolom_nilai_nap_ukk_forrapor_bobot_all.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk_ukk_pat_for_rapor_by_bobot != null)
                        {
                            if (m_nilai_col_nk_ukk_pat_for_rapor_by_bobot.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk_ukk_pat_for_rapor_by_bobot.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";
                            }
                        }
                        //end get formula nilai nk UKK or PAT for rapor (by bobot)

                        //get formula nilai na
                        NILAI_COL m_nilai_col_na = lst_kolom_nilai_nap.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_na != null)
                        {
                            if (m_nilai_col_na.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_na.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_ap[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_na = true;
                            }
                        }
                        //end get formula nilai na

                        //---get nilainya disini
                        if (lst_nilai_siswa_det != null)
                        {
                            if (m_nilai_det != null)
                            {
                                if (m_nilai_det.Nilai != null)
                                {
                                    s_nilai = m_nilai_det.Nilai;
                                }
                            }
                        }

                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                          "'" + s_nilai + "'";
                        //---end get nilai

                        if (is_kolom_nk) //styling kolom nk
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + 
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nkd) + " htFontBlack " +
                                             "htBorderRightNKD" + "\", readOnly: true }";
                        }
                        else if (
                            lst_kolom_nilai_nap_forrapor_all.FindAll(m0 => m0.IdKolom == i).Count > 0
                        )
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + css_bg_kd_for_rapor + " htFontBlack " + "\", readOnly: true }";
                        }
                        else if (
                            lst_kolom_nilai_nap_forrapor_bobot_all.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                            lst_kolom_nilai_nap_ukk_forrapor_bobot_all.FindAll(m0 => m0.IdKolom == i).Count > 0
                        )
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + css_bg_kd_for_rapor + " htFontBlack " +
                                             "htBorderRightNKDUKK" + "\", readOnly: true }";
                        }
                        else if (is_kolom_na) //styling kolom na
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + 
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nap) + " htFontBlack " +
                                             "htBorderRightNAP" + "\", readOnly: true }";
                        }
                        else //styling kolom nilai
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                        }
                    }

                    string s_formula_rapor = "";
                    //get nilai akhir by formula
                    if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                    {
                        foreach (var item in lst_kolom_nilai_nap)
                        {
                            s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                               "(" +
                                                   "(" +
                                                        "IF(" +
                                                            Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                            Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                                        ")" +
                                                   ")*" + item.Bobot.ToString() + "%" +
                                               ")";
                        }
                    }
                    else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                    {
                        foreach (var item in lst_kolom_nilai_nap)
                        {
                            s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                               "IF(" +
                                                   Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                   Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                               ")";


                        }
                        if (s_formula_rapor.Trim() != "")
                        {
                            s_formula_rapor = "(" +
                                                    s_formula_rapor +
                                              ")/" + lst_kolom_nilai_nap.Count.ToString();
                        }
                    }
                    if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                    {
                        s_formula_rapor = "'" +
                                            (s_formula_rapor.Trim() != "" ? "=" : "") +
                                            "IF(" +
                                                "ROUND(" +
                                                    s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                    ", " +
                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                ") <= 0, \"\", " +
                                                "ROUND(" +
                                                    s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                    ", " +
                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                ")" +
                                            ")" +
                                          "'";
                    }
                    else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        s_formula_rapor = "'" +
                                            (s_formula_rapor.Trim() != "" ? "=" : "") +
                                            "IF(" +
                                                "ROUND(" +
                                                    s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                    ", " +
                                                    "0" +
                                                ") <= 0, \"\", " +
                                                "ROUND(" +
                                                    s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                    ", " +
                                                    "0" +
                                                ")" +
                                            ")" +
                                          "'";
                    }

                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      s_formula_rapor;

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 5).ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                    //end get nilai akhir by formula

                    s_content += (s_content.Trim() != "" ? ", " : "") +
                                 "[" +
                                    "\"" + id.ToString() + "\", " +
                                    "\"" + m_siswa.NISSekolah + "\", " +
                                    "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true), 3) + "\", " +
                                    "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                    (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                 "]";

                    //kolom style untuk fixed col header
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                    id++;
                }
                //end load data siswa

            }
            else
            {

                //nilai akademik
                Rapor_Nilai m_nilai = new Rapor_Nilai();
                m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran, semester, rel_kelas_det, rel_mapel
                    ).FirstOrDefault();

                List<Rapor_NilaiSiswa> lst_nilai_siswa = null;
                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = null;
                if (m_nilai != null)
                {
                    if (m_nilai.Kurikulum != null)
                    {
                        lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                        lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                    }
                }

                //init js arr siswa
                s_arr_js_siswa = "";
                for (int i = 0; i < id_jml_fixed_row; i++)
                {
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "''";
                }

                int id = 1;
                id_col_nilai_mulai = 4;
                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        m_kelas.Rel_Sekolah.ToString(),
                        rel_kelas_det,
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
                string s_js_arr_nilai = "";
                foreach (Siswa m_siswa in lst_siswa)
                {
                    css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                    css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                    css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                    css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                    css_bg_kd_for_rapor = (id % 2 == 0 ? " htBG19" : " htBG20");

                    s_js_arr_nilai = "";
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "'" + m_siswa.Kode.ToString() + "'";

                    for (int i = id_col_nilai_mulai; i < id_col_all - 5; i++)
                    {
                        string s_nilai = "";
                        bool is_kolom_nk = false;
                        bool is_kolom_na = false;
                        is_nilai_manual = false;

                        Rapor_NilaiSiswa_Det m_nilai_det = new Rapor_NilaiSiswa_Det();

                        if (m_nilai != null)
                        {
                            if (m_nilai.TahunAjaran != null)
                            {
                                m_nilai_det =
                                    lst_nilai_siswa_det.FindAll(
                                                m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                     m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i <= lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                            ).FirstOrDefault();
                            }
                        }

                        //get formula nilai nk
                        NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk != null)
                        {
                            if (m_nilai_col_nk.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_kd[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_nk = true;
                            }
                        }
                        //end get formula nilai nk

                        //get formula nilai nk for rapor
                        NILAI_COL m_nilai_col_nk_for_rapor = lst_kolom_nilai_nap_forrapor_all.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk_for_rapor != null)
                        {
                            if (m_nilai_col_nk_for_rapor.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk_for_rapor.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";
                            }
                        }
                        //end get formula nilai nk for rapor

                        //get formula nilai nk for rapor (by bobot)
                        NILAI_COL m_nilai_col_nk_for_rapor_by_bobot = lst_kolom_nilai_nap_forrapor_bobot_all.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk_for_rapor_by_bobot != null)
                        {
                            if (m_nilai_col_nk_for_rapor_by_bobot.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk_for_rapor_by_bobot.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";
                            }
                        }
                        //end get formula nilai nk for rapor (by bobot)

                        //get formula nilai nk UKK or PAT for rapor (by bobot)
                        NILAI_COL m_nilai_col_nk_ukk_pat_for_rapor_by_bobot = lst_kolom_nilai_nap_ukk_forrapor_bobot_all.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk_ukk_pat_for_rapor_by_bobot != null)
                        {
                            if (m_nilai_col_nk_ukk_pat_for_rapor_by_bobot.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk_ukk_pat_for_rapor_by_bobot.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                s_arr_js_formula += (s_arr_js_formula.Trim() != "" ? ", " : "") +
                                                    "{ " +
                                                    " 'row' : '" + ((id + id_jml_fixed_row) - 1).ToString() + "', " +
                                                    " 'col' : '" + (i).ToString() + "', " +
                                                    " 'formula' : '" + s_nilai + "' " +
                                                    "} ";
                            }
                        }
                        //end get formula nilai nk UKK or PAT for rapor (by bobot)

                        //get formula nilai na
                        NILAI_COL m_nilai_col_na = lst_kolom_nilai_nap.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_na != null)
                        {
                            if (m_nilai_col_na.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_na.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_ap[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_na = true;
                            }
                        }
                        //end get formula nilai na

                        //---get nilainya disini
                        if (lst_nilai_siswa_det != null)
                        {                            
                            if (m_nilai_det != null)
                            {
                                if (m_nilai_det.Nilai != null)
                                {
                                    s_nilai = m_nilai_det.Nilai;
                                }
                            }
                        }

                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                          "'" + s_nilai + "'";
                        //---end get nilai

                        if (is_kolom_nk) //styling kolom nk
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nkd) + " htFontBlack " +
                                             "htBorderRightNKD" + "\", readOnly: true }";
                        }
                        else if (
                            lst_kolom_nilai_nap_forrapor_all.FindAll(m0 => m0.IdKolom == i).Count > 0
                        ) {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + css_bg_kd_for_rapor + " htFontBlack " + "\", readOnly: true }";
                        }
                        else if (
                            lst_kolom_nilai_nap_forrapor_bobot_all.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                            lst_kolom_nilai_nap_ukk_forrapor_bobot_all.FindAll(m0 => m0.IdKolom == i).Count > 0
                        )
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + css_bg_kd_for_rapor + " htFontBlack " +
                                             "htBorderRightNKDUKK" + "\", readOnly: true }";
                        }
                        else if (is_kolom_na) //styling kolom na
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nap) + " htFontBlack " +
                                             "htBorderRightNAP" + "\", readOnly: true }";
                        }
                        else //styling kolom nilai
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                        }
                    }

                    string s_formula_rapor = "";
                    //get nilai akhir by formula
                    if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                    {
                        foreach (var item in lst_kolom_nilai_nap)
                        {
                            s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                               "(" +
                                                   "(" +
                                                        "IF(" +
                                                            Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                            Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                                        ")" +
                                                   ")*" + item.Bobot.ToString() + "%" +
                                               ")";
                        }
                    }
                    else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                    {
                        foreach (var item in lst_kolom_nilai_nap)
                        {
                            s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                               "IF(" +
                                                   Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                   Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                               ")";


                        }
                        if (s_formula_rapor.Trim() != "")
                        {
                            s_formula_rapor = "(" +
                                                    s_formula_rapor +
                                              ")/" + lst_kolom_nilai_nap.Count.ToString();
                        }
                    }
                    if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                    {
                        s_formula_rapor = "'" +
                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                        "IF(" +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                            ") <= 0, \"\", " +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                            ")" +
                                        ")" +
                                      "'";
                    }
                    else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        s_formula_rapor = "'" +
                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                        "IF(" +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                "0" +
                                            ") <= 0, \"\", " +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                "0" +
                                            ")" +
                                        ")" +
                                      "'";
                    }

                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      s_formula_rapor;

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 5).ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                    //end get nilai akhir by formula

                    //nilai capaian kedisiplinan
                    string s_lts_ck_kehadiran = "";
                    string s_lts_ck_ketepatan_waktu = "";
                    string s_lts_ck_penggunaan_seragam = "";
                    string s_lts_ck_penggunaan_kamera = "";
                    if (lst_nilai_siswa != null)
                    {
                        if (lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).Count == 1)
                        {
                            var m_nilai_siswa = lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                            s_lts_ck_kehadiran = m_nilai_siswa.LTS_CK_KEHADIRAN.Replace("\"", "").Replace("'", "");
                            s_lts_ck_ketepatan_waktu = m_nilai_siswa.LTS_CK_KETEPATAN_WKT.Replace("\"", "").Replace("'", "");
                            s_lts_ck_penggunaan_seragam = m_nilai_siswa.LTS_CK_PENGGUNAAN_SRGM.Replace("\"", "").Replace("'", "");
                            s_lts_ck_penggunaan_kamera = m_nilai_siswa.LTS_CK_PENGGUNAAN_KMR.Replace("\"", "").Replace("'", "");
                        }
                    }
                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      "'" + s_lts_ck_kehadiran + "'"; //lts ck kehadiran
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 4).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      "'" + s_lts_ck_ketepatan_waktu + "'"; //lts ck ketepatan waktu
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 3).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      "'" + s_lts_ck_penggunaan_seragam + "'"; //lts ck kehadiran
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 2).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      "'" + s_lts_ck_penggunaan_kamera + "'"; //lts ck ketepatan waktu
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 1).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                    //end nilai capaian kedisiplinan

                    s_content += (s_content.Trim() != "" ? ", " : "") +
                                 "[" +
                                    "\"" + id.ToString() + "\", " +
                                    "\"" + m_siswa.NISSekolah + "\", " +
                                    "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true),3) + "\", " +
                                    "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                    (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                 "]";

                    //kolom style untuk fixed col header
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                    id++;
                }
                //end load data siswa

            }

            //buat variable array untuk nkd & nap
            s_arr_kolom_na = "";
            s_arr_kolom_nk = "";
            foreach (var item_nkd in lst_kolom_nilai_nkd)
            {
                s_arr_kolom_nk += (s_arr_kolom_nk.Trim() != "" ? ", " : "") +
                                  item_nkd.IdKolom.ToString();
            }
            foreach (var item_nap in lst_kolom_nilai_nap)
            {
                s_arr_kolom_na += (s_arr_kolom_na.Trim() != "" ? ", " : "") +
                                  item_nap.IdKolom.ToString();
            }
            s_arr_kolom_na = "[" + s_arr_kolom_na + "]";
            s_arr_kolom_nk = "[" + s_arr_kolom_nk + "]";
            s_arr_js_formula = "[" + s_arr_js_formula + "]";
            //end buat variabel

            s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
            s_arr_js_ap = "[" + s_arr_js_ap + "]";
            s_arr_js_kd = "[" + s_arr_js_kd + "]";
            s_arr_js_kp = "[" + s_arr_js_kp + "]";

            s_content = (s_content.Trim() != "" ? "," : "") +
                        s_content;

            string s_data = "var data_nilai = " +
                            "[" +
                                s_kolom +
                                s_content +
                            "];";

            string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SD.NILAI_SISWA.ROUTE);
            s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SD.NILAI_SISWA.DO_SAVE.FILE + "/Do");

            string script = s_data +
                            "var arr_s = " + s_arr_js_siswa + ";" +
                            "var arr_ap = " + s_arr_js_ap + ";" +
                            "var arr_kd = " + s_arr_js_kd + ";" +
                            "var arr_kp = " + s_arr_js_kp + ";" +
                            "var arr_kolom_nkd = " + s_arr_kolom_nk + ";" +
                            "var arr_kolom_nap = " + s_arr_kolom_na + ";" +
                            "var arr_formula = " + s_arr_js_formula + ";" +
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
                                "formulas: true," +
                                "fillHandle: false," +
                                (
                                    is_readonly
                                    ? "readOnly: true, " +
                                      "contextMenu: false, " +
                                      "manualColumnResize: false, " +
                                      "manualRowResize: false, " +
                                      "comments: false, "
                                    : ""
                                ) +
                                "cells: function (row, col, prop) {" +
                                  "if(this.instance.getData().length != 0){" +
                                    "var cellProperties = {};" +
                                    "if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                        "if((row + 1) % 2 !== 0){" +
                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                        "} else {" +
                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                        "} " +
                                    "} " +
                                    "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                        "if((row + 1) % 2 !== 0){" +
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
                                        (
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel_sikap.Trim() != ""
                                            ? "var mp = '';"
                                            : "var mp = '" + rel_mapel.ToString() + "';"
                                        ) +
                                        "var ms = '" + rel_mapel_sikap.ToString() + "';" +
                                        "var s = arr_s[row];" +
                                        "var ap = arr_ap[col];" +
                                        "var kd = arr_kd[col];" +
                                        "var kp = arr_kp[col];" +
                                        "var n = data_nilai[row][col];" +

                                        "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_nilai_rapor.ToString() + "});" +
                                        "var formula = hot.getDataAtCell(row, " + id_col_nilai_rapor.ToString() + ");" +
                                        "formula = formula.substr(1).toUpperCase();" +
                                        "var newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_nilai_rapor.ToString() + ", id: cellId});" +
                                        "var nr = (newValue.result);" +

                                        "var lts_ck_hd = (data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "]);" +
                                        "var lts_ck_kw = (data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "]);" +
                                        "var lts_ck_ps = (data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "]);" +
                                        "var lts_ck_pk = (data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "]);" +
                                        
                                        "var s_url = '" + s_url_save + "' + " +
                                                    "'?' + " +
                                                    "'j=' + '' + '&' + " +
                                                    "'t=' + t + '&sm=' + sm + '&kdt=' + kdt + '&' + " +
                                                    "'s=' + s + '&n=' + n + '&ap=' + ap + '&kd=' + kd + '&kp=' + kp + '&' + " +
                                                    "'mp=' + mp + '&ms=' + ms + '&k=' + k + '&' + " +
                                                    "'nr=' + nr + " +
                                                    "'&lts_ck_hd=' + lts_ck_hd + " +
                                                    "'&lts_ck_kw=' + lts_ck_kw + " +
                                                    "'&lts_ck_ps=' + lts_ck_ps + " +
                                                    "'&lts_ck_pk=' + lts_ck_pk + " +
                                                    "'&ssid=" + Libs.Enkrip(Libs.LOGGED_USER_M.NoInduk) + "'" +
                                                    ";" +

                                        "$.ajax({" +
                                            "url: s_url, " +
                                            "dataType: 'json', " +
                                            "type: 'GET', " +
                                            "processData: false, " +
                                            "contentType: 'application/json; charset=utf-8', " +
                                            "success: function(data) { " +
                                                "}, " +
                                            "error: function(response) { " +
                                                    "alert(response.responseText); " +
                                                "}, " +
                                            "failure: function(response) { " +
                                                    "alert(response.responseText); " +
                                                "} " +
                                        "}); " +

                                    "});" +
                                    "this.render();" +
                                "}, " +
                                (
                                    !is_readonly
                                    ? "afterSelection: function(r,c){ " +
                                        "var find_nkd = arr_kolom_nkd.find(function(col){ return col === c; }); " +
                                        "if(find_nkd !== undefined){ " +
                                            "var cellId = hot.plugin.utils.translateCellCoords({row: r, col: c});" +
                                            "var formula = hot.getDataAtCell(r, c);" +
                                            "formula = (formula.substr(0, 1) === '=' ? formula.substr(1).toUpperCase() : formula.toUpperCase());" +
                                            "var newValue = hot.plugin.parse(formula, {row: r, col: c, id: cellId});" +
                                            "var n_update = (newValue.result);" +
                                            "if(n_update === null) n_update = data_nilai[r][c];" +

                                            "ShowUpdateNilai(n_update, r, c, arr_s[r], data_nilai[r][1], data_nilai[r][2], '', arr_kd[c]); " +
                                        "} " +
                                    //"var find_nap = arr_kolom_nap.find(function(col){ return col === c; }); " +
                                    //"if(find_nap !== undefined){ " +
                                    //    "var cellId = hot.plugin.utils.translateCellCoords({row: r, col: c});" +
                                    //    "var formula = hot.getDataAtCell(r, c);" +
                                    //    "formula = (formula.substr(0, 1) === '=' ? formula.substr(1).toUpperCase() : formula.toUpperCase());" +
                                    //    "var newValue = hot.plugin.parse(formula, {row: r, col: c, id: cellId});" +
                                    //    "var n_update = (newValue.result);" +
                                    //    "if(n_update === null) n_update = data_nilai[r][c];" +

                                    //    "ShowUpdateNilai(n_update, r, c, arr_s[r], data_nilai[r][1], data_nilai[r][2], arr_ap[c], ''); " +
                                    //"} " +
                                    "}, "
                                    : ""
                                ) +                                
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
                            "container.find('table').addClass('zebraStyle'); " +
                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_rapor.ToString() + "); " +
                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_mulai.ToString() + "); ";

            ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";
            txtIDColRapor.Value = id_col_nilai_rapor.ToString();
        }

        protected void LoadFormat2(string semester, Rapor_StrukturNilai m_struktur_nilai, Kelas m_kelas, bool is_readonly, string kurikulum)
        {
            string tahun_ajaran = QS.GetTahunAjaran();
            string rel_kelas = "";
            string rel_kelas_det = QS.GetKelas();
            string rel_mapel = QS.GetMapel();
            string rel_mapel_sikap = QS.GetMapelSikap();

            List<NILAI_COL> lst_kolom_nilai_nkd = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap = new List<NILAI_COL>();

            string s_kolom = "";
            string s_content = "";
            string s_kolom_width = "";

            string s_merge_cells = "";
            string s_kolom_style = "";

            string s_js_arr_kolom1 = "";
            string s_js_arr_kolom2 = "";
            string s_js_arr_kolom3 = "";
            string s_formula = "";

            string css_bg = "#fff";
            string css_bg_nkd = "#fff";
            string css_bg_nap = "#fff";
            string css_bg_nilaiakhir = "#fff";

            int id_jml_fixed_row = 3;
            int id_col_mulai_content = 4;
            int id_col_all = id_col_mulai_content;
            int id_col_nilai_rapor = 0;

            List<string> lst_ap = new List<string>();
            List<string> lst_kd = new List<string>();
            List<string> lst_kp = new List<string>();

            string s_arr_js_siswa = "";
            string s_arr_js_ap = "";
            string s_arr_js_kd = "";
            string s_arr_js_kp = "";

            string s_arr_kolom_nk = "";
            string s_arr_kolom_na = "";

            bool is_nilai_manual = false;

            lst_kolom_nilai_nkd.Clear();
            txtKKM.Value = Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

            //init js arr aspek penilaian/ap
            s_arr_js_ap = "";
            lst_ap.Clear();
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                               "\"\"";
                lst_ap.Add("");
            }
            //end init

            //init js arr kompetensi dasar/kd
            s_arr_js_kd = "";
            lst_kd.Clear();
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                               "\"\"";
                lst_kd.Add("");
            }
            //end init

            //init js arr komponen penilaian/kp
            s_arr_js_kp = "";
            lst_kp.Clear();
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                               "\"\"";
                lst_kp.Add("");
            }
            //end init

            List<COL_KP> lst_kolom_kp = new List<COL_KP>();

            //load ap
            List<Rapor_StrukturNilai_AP> lst_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(m_struktur_nilai.Kode.ToString());
            foreach (Rapor_StrukturNilai_AP m_struktur_ap in lst_struktur_ap)
            {
                int jml_merge_ap = 0;

                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        int jml_merge_kd = 0;

                        string s_formula_kd_gabung = "";

                        //load kd
                        int id_nk = 1;
                        List<Rapor_StrukturNilai_KD> lst_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KD m_struktur_kd in lst_struktur_kd)
                        {
                            jml_merge_kd = 0;

                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (m_kd != null)
                            {
                                if (m_kd.Nama != null)
                                {
                                    s_formula = "";

                                    string s_formula_kp = "";

                                    //load kp
                                    List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(m_struktur_kd.Kode.ToString());
                                    foreach (Rapor_StrukturNilai_KP m_struktur_kp in lst_struktur_kp)
                                    {
                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_struktur_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                lst_kolom_kp.Add(new COL_KP {
                                                    KodeAP = m_struktur_ap.Kode,
                                                    BobotAP = m_struktur_ap.BobotRapor,
                                                    KodeKP = m_struktur_kp.Rel_Rapor_KomponenPenilaian,
                                                    IdKolom = id_col_all
                                                });

                                                //add ap, kd, kp
                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + m_struktur_ap.Kode.ToString() + "\"";
                                                lst_ap.Add(m_struktur_ap.Kode.ToString());

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + m_struktur_kd.Kode.ToString() + "\"";
                                                lst_kd.Add(m_struktur_kd.Kode.ToString());

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + m_struktur_kp.Kode.ToString() + "\"";
                                                lst_kp.Add(m_struktur_kp.Kode.ToString());
                                                //end add ap, kd, kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"" + Libs.GetHTMLSimpleText(m_ap.Nama, true) + "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                   "\"" +
                                                                        (
                                                                            m_struktur_kd.Poin.Trim() != ""
                                                                            ? m_struktur_kd.Poin.Trim() +
                                                                              "<br />"
                                                                            : ""
                                                                        ) +
                                                                        Libs.GetPersingkatKalimat(Libs.GetHTMLSimpleText(m_kd.Nama, true)) +
                                                                        (
                                                                            m_struktur_kd.BobotAP > 0
                                                                            ? "&nbsp;" +
                                                                              "<sup class='badge' style='background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px; border-top-left-radius: 0px; border-top-right-radius: 0px;'>" +
                                                                                Math.Round(m_struktur_kd.BobotAP, 0).ToString() + "%" +
                                                                              "</sup>"
                                                                            : ""
                                                                        ) +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                                   "\"" +
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama, true) +
                                                                        (
                                                                            m_struktur_kp.BobotNK > 0
                                                                            ? "&nbsp;" +
                                                                              "<sup class='badge' style='background-color: #B7770D; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px; border-top-left-radius: 0px; border-top-right-radius: 0px;'>" +
                                                                                Math.Round(m_struktur_kp.BobotNK, 0).ToString() + "%" +
                                                                              "</sup>"
                                                                            : ""
                                                                        ) +
                                                                   "\"";

                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                //formula item kp
                                                s_formula_kp += (
                                                                    m_struktur_kd.JenisPerhitungan ==
                                                                        ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                                                    ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                      "(" +
                                                                            "IF(" +
                                                                                Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                                "= \"\", 0 ," +
                                                                                Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                            ")" +
                                                                            "*(" + (m_struktur_kp.BobotNK.ToString()) + "%)" +
                                                                      ")"
                                                                    : (
                                                                            m_struktur_kd.JenisPerhitungan ==
                                                                                ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                                            ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                              "IF(" +
                                                                                    Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                                    "= \"\", 0 ," +
                                                                                    Libs.GetColHeader(id_col_all + 1) + "# " +
                                                                              ")"
                                                                            : ""
                                                                      )
                                                                );
                                                //end item formula kp

                                                id_col_all++;
                                                jml_merge_kd++;
                                                jml_merge_ap++;
                                            }
                                        }

                                    }
                                    //end load kp

                                    //generate formula untuk kp
                                    if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        s_formula = "IF(" +
                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() + ") <= 0, \"\", " +
                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() + ") " +
                                                     ")";
                                    }
                                    else if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        s_formula = "IF(" +
                                                        "ROUND((" + s_formula_kp + ")/" + jml_merge_kd.ToString() + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() + ") <= 0, \"\", " +
                                                        "ROUND((" + s_formula_kp + ")/" + jml_merge_kd.ToString() + ", " +
                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                                             ")" +
                                                     ")";
                                    }
                                    //end generate

                                    //tambahkan ke formula kd
                                    s_formula_kd_gabung += (
                                            m_struktur_ap.JenisPerhitungan ==
                                                ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                            ? (s_formula_kd_gabung.Trim() != "" ? "+" : "") +
                                                "(" +
                                                    "(" +
                                                        "IF(" +
                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", 0, " +
                                                            Libs.GetColHeader(id_col_all + 1) + "#" +
                                                        ")" +
                                                    ")" +
                                                    "*(" + (m_struktur_kd.BobotAP.ToString()) + "%)" +
                                                ")"
                                            : (
                                                m_struktur_ap.JenisPerhitungan ==
                                                    ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                ? (s_formula_kd_gabung.Trim() != "" ? "+" : "") +
                                                    "(" +
                                                        "IF(" +
                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", 0, " +
                                                            Libs.GetColHeader(id_col_all + 1) + "#" +
                                                        ")" +
                                                    ")"
                                                : ""
                                                )
                                        );
                                    //end tambahkan ke formula kd

                                    //add content kolom nkd
                                    lst_kolom_nilai_nkd.Add(new NILAI_COL
                                    {
                                        BluePrintFormula = "=" + s_formula,
                                        IdKolom = id_col_all
                                    });
                                    //end content kolom nkd

                                    //tambahkan nk setelah kp
                                    //add ap to var arr js
                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                   "\"" + m_struktur_ap.Kode.ToString() + "\"";
                                    lst_ap.Add(m_struktur_ap.Kode.ToString());

                                    //add kd to var arr js
                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                   "\"" + m_struktur_kd.Kode.ToString() + "\"";
                                    lst_kd.Add(m_struktur_kd.Kode.ToString());

                                    //add kp to var arr js
                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                   "\"\"";
                                    lst_kp.Add("");
                                    //end add ap, kd & kp

                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                     LEBAR_COL_DEFAULT;

                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama, true) + "\"";

                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                       "\"" + Libs.GetPersingkatKalimat(Libs.GetHTMLSimpleText(m_kd.Nama, true)) + "\"";

                                    s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                       "\"NK" + id_nk.ToString() + "\"";

                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBlack\", readOnly: true, renderer: \"html\" }";

                                    id_col_all++;
                                    jml_merge_kd++;
                                    jml_merge_ap++;
                                    id_nk++;
                                    //end tambahkan nk

                                    //end load kp

                                    if (jml_merge_kd > 0)
                                    {
                                        s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                        "{ row: 1, col: " + (id_col_all - jml_merge_kd).ToString() + ", rowspan: 1, colspan: " + jml_merge_kd.ToString() + " }";
                                    }

                                }
                            }
                        }
                        //end load kd
                        
                        if (jml_merge_ap > 0)
                        {
                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                            "{ row: 0, col: " + (id_col_all - jml_merge_ap).ToString() + ", rowspan: 1, colspan: " + jml_merge_ap.ToString() + " }";
                        }

                    }
                }
            }
            //end load ap

            //tambahkan kolom kompetensi by kp
            List<FORMULA_KP> lst_formula_kp = new List<FORMULA_KP>();
            string s_blueprint_formula_rapor = "";
            string s_formula_q = "";
            string s_formula_non_q = "";
            int id_col_kp = id_col_all;
            int id_col_kp_count = 0;
            List<DAO_Rapor_StrukturNilai.DistinctKP> lst_distinctkp = DAO_Rapor_StrukturNilai.GetDistinctKP_Entity(m_struktur_nilai.Kode.ToString());
            List<Rapor_StrukturNilai_KPKelompok> lst_kpkelompok = DAO_Rapor_StrukturNilai_KPKelompok.GetAllByHeader_Entity(
                    m_struktur_nilai.Kode.ToString()
                );

            foreach (var m_distinctkp in lst_distinctkp.FindAll(
                m => m.Kode.ToUpper().Trim() != DAO_Rapor_StrukturNilai.Arr_KP_Quran[0] &&
                     m.Kode.ToUpper().Trim() != DAO_Rapor_StrukturNilai.Arr_KP_Quran[1] &&
                     m.Kode.ToUpper().Trim() != DAO_Rapor_StrukturNilai.Arr_KP_Quran[2]))
            {
                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_distinctkp.Kode.ToString());
                if (m_kp != null)
                {
                    if (m_kp.Nama != null)
                    {
                        s_formula_non_q += (s_formula_non_q.Trim() != "" ? "," : "") +
                                            "(" +
                                                (
                                                    "IF(" +
                                                        Libs.GetColHeader(id_col_kp + 1) + "#" + " = \"\", 0, " +
                                                        Libs.GetColHeader(id_col_kp + 1) + "#" +
                                                    ")"
                                                ) +
                                            ")";
                        id_col_kp++;
                    }
                }
            }

            foreach (var m_distinctkp in lst_distinctkp.FindAll(
                m => m.Kode.ToUpper().Trim() == DAO_Rapor_StrukturNilai.Arr_KP_Quran[0] ||
                     m.Kode.ToUpper().Trim() == DAO_Rapor_StrukturNilai.Arr_KP_Quran[1] ||
                     m.Kode.ToUpper().Trim() == DAO_Rapor_StrukturNilai.Arr_KP_Quran[2]))
            {
                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_distinctkp.Kode.ToString());
                if (m_kp != null)
                {
                    if (m_kp.Nama != null)
                    {
                        string s_bobot = "";
                        Rapor_StrukturNilai_KPKelompok m_kpkelompok = lst_kpkelompok.FindAll(
                                m => m.Rel_Rapor_KomponenPenilaian == m_kp.Kode
                            ).FirstOrDefault();
                        if (m_kpkelompok != null)
                        {
                            if (m_kpkelompok.Rel_Rapor_StrukturNilai != null)
                            {
                                s_bobot = m_kpkelompok.Bobot.ToString();
                            }
                        }
                        s_formula_q += (s_formula_q.Trim() != "" ? "+" : "") +
                                    "(" +
                                        (
                                            s_bobot + "% * " +
                                            "IF(" +
                                                Libs.GetColHeader(id_col_kp + 1) + "#" + " = \"\", 0, " +
                                                Libs.GetColHeader(id_col_kp + 1) + "#" +
                                            ")"
                                        ) +
                                    ")";
                        id_col_kp++;
                    }
                }
            }

            if (s_formula_q.Trim() != "" && s_formula_non_q.Trim() != "")
            {
                if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                {
                    s_formula_q = "IF(" +
                                        "ROUND(" +
                                            s_formula_q +
                                            ", " +
                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                        ") <= 0, \"\", " +
                                        "ROUND(" +
                                            s_formula_q +
                                            ", " +
                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                        ")" +
                                  ")";
                }
                else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                {
                    s_formula_q = "IF(" +
                                        "ROUND(" +
                                            s_formula_q.ToString() +
                                            ", " +
                                            "2" +
                                        ") <= 0, \"\", " +
                                        "ROUND(" +
                                            s_formula_q.ToString() +
                                            ", " +
                                            "2" +
                                        ")" +
                                   ")";
                }

                s_blueprint_formula_rapor = "AVERAGE(" +
                                                s_formula_q +
                                                "," +
                                                s_formula_non_q +
                                            ")";
            }
            else
            {
                s_blueprint_formula_rapor = s_formula_q;
            }

            id_col_kp = id_col_all;
            foreach (var m_distinctkp in lst_distinctkp)
            {
                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_distinctkp.Kode.ToString());
                if (m_kp != null)
                {
                    if (m_kp.Nama != null)
                    {
                        string s_bobot = "";
                        Rapor_StrukturNilai_KPKelompok m_kpkelompok = lst_kpkelompok.FindAll(
                                m => m.Rel_Rapor_KomponenPenilaian == m_kp.Kode
                            ).FirstOrDefault();
                        if (m_kpkelompok != null)
                        {
                            if (m_kpkelompok.Rel_Rapor_StrukturNilai != null)
                            {
                                s_bobot = m_kpkelompok.Bobot.ToString();
                            }
                        }

                        //tambahkan kolom nilai kp
                        s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                         LEBAR_COL_DEFAULT;

                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"" +
                                                Libs.GetHTMLSimpleText(m_kp.Nama, true) +
                                           "\"";

                        s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"" +
                                                Libs.GetHTMLSimpleText(m_kp.Nama, true) +
                                           "\"";

                        s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                           "\"" +
                                                Libs.GetHTMLSimpleText(m_kp.Nama, true) +
                                           "\"";
                        s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                         "{ row: 0, col: " + (id_col_kp).ToString() + ", rowspan: 3, colspan: 1 }";
                        //s_blueprint_formula_rapor += (s_blueprint_formula_rapor.Trim() != "" ? "+" : "") +
                        //                             "(" + 
                        //                                (
                        //                                    s_bobot + "% * " +
                        //                                    "IF(" +
                        //                                       Libs.GetColHeader(id_col_kp + 1) + "#" + " = \"\", 0, " +
                        //                                       Libs.GetColHeader(id_col_kp + 1) + "#" +
                        //                                    ")"
                        //                                ) +
                        //                             ")";

                        //get formula kp
                        string s_formula_kp = "";
                        string s_formula_item_kp = "";
                        foreach (Rapor_StrukturNilai_AP m_struktur_ap in lst_struktur_ap)
                        {
                            s_formula_item_kp = "";
                            List<COL_KP> lst_kolom_kp_ap = lst_kolom_kp.FindAll(m => m.KodeAP == m_struktur_ap.Kode && m.KodeKP == new Guid(m_distinctkp.Kode));
                            foreach (var m_kolom_kp_ap in lst_kolom_kp_ap)
                            {
                                s_formula_item_kp += (s_formula_item_kp.Trim() != "" ? "+" : "") +
                                                     "IF(" +
                                                        Libs.GetColHeader(m_kolom_kp_ap.IdKolom + 1) + "#" +
                                                        "= \"\", 0 ," +
                                                        Libs.GetColHeader(m_kolom_kp_ap.IdKolom + 1) + "#" +
                                                     ")";
                            }
                            if (lst_kolom_kp_ap.Count > 0)
                            {
                                s_formula_kp += (s_formula_kp.Trim() != "" ? "+" : "") +
                                                "(" +
                                                    m_struktur_ap.BobotRapor.ToString() + "%*" +
                                                    "(" + s_formula_item_kp + ")/" + lst_kolom_kp_ap.Count.ToString() +
                                                ")";
                            }
                        }
                        lst_formula_kp.Add(new FORMULA_KP
                        {
                            IdKolom = id_col_kp,
                            BluePrintFormula = (s_formula_kp.Trim() != "" ? "(" + s_formula_kp + ")" : "")
                        });
                        //end get formula kp

                        id_col_kp++;
                        id_col_kp_count++;
                        //end tambahkan kolom nilai kp
                    }
                }
            }

            //tambahkan kolom nilai akhir
            id_col_all = id_col_kp;
            id_col_nilai_rapor = id_col_all;
            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                             LEBAR_COL_DEFAULT;

            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"" +
                                "NILAI AKHIR" +
                               "\"";

            s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"" +
                                "NILAI AKHIR" +
                               "\"";

            s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               "\"" +
                                "NILAI AKHIR" +
                               "\"";
            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                             "{ row: 0, col: " + (id_col_all).ToString() + ", rowspan: 3, colspan: 1 }";
            //end tambahkan kolom nilai akhir

            s_merge_cells = (s_merge_cells.Trim() != "" ? ", " : "") +
                             s_merge_cells;

            s_kolom_width = (s_kolom_width.Trim() != "" ? ", " : "") +
                             s_kolom_width;

            s_js_arr_kolom1 = (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom1;

            s_js_arr_kolom2 = (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom2;

            s_js_arr_kolom3 = (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom3;

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
                      "]," +
                      "[" +
                            "\"#\", " +
                            "\"NIS\", " +
                            "\"NAMA SISWA\", " +
                            "\"L/P\" " +
                            s_js_arr_kolom3 +
                      "]";

            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                             "{ row: 0, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }," +
                             "{ row: 1, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }," +
                             "{ row: 2, col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }";

            //load data siswa
            //list siswa

            int id_col_nilai_mulai = 4;
            //get list nilai jika ada
            if (rel_mapel_sikap.Trim() != "")
            {

                //nilai sikap
                Rapor_Sikap m_nilai = new Rapor_Sikap();
                m_nilai = DAO_Rapor_Sikap.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran,
                        semester,
                        rel_kelas_det,
                        (
                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel_sikap.Trim() != ""
                            ? (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel_sikap.Trim() != "" && rel_mapel.Trim() != ""
                                ? rel_mapel.ToString()
                                : ""
                              )
                            : rel_mapel.ToString()
                        ),
                        rel_mapel_sikap
                    ).FirstOrDefault();

                List<Rapor_SikapSiswa> lst_nilai_siswa = null;
                List<Rapor_SikapSiswa_Det> lst_nilai_siswa_det = null;
                if (m_nilai != null)
                {
                    if (m_nilai.Kurikulum != null)
                    {
                        lst_nilai_siswa = DAO_Rapor_SikapSiswa.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                        lst_nilai_siswa_det = DAO_Rapor_SikapSiswa_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                    }
                }

                //init js arr siswa
                s_arr_js_siswa = "";
                for (int i = 0; i < id_jml_fixed_row; i++)
                {
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "''";
                }

                int id = 1;
                id_col_nilai_mulai = 4;
                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        m_kelas.Rel_Sekolah.ToString(),
                        rel_kelas_det,
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
                string s_js_arr_nilai = "";
                foreach (Siswa m_siswa in lst_siswa)
                {
                    css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                    css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                    css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                    css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");

                    s_js_arr_nilai = "";
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "'" + m_siswa.Kode.ToString() + "'";

                    for (int i = id_col_nilai_mulai; i < (id_col_kp - id_col_kp_count); i++)
                    {
                        string s_nilai = "";
                        bool is_kolom_nk = false;
                        bool is_kolom_na = false;
                        is_nilai_manual = false;

                        Rapor_SikapSiswa_Det m_nilai_det = new Rapor_SikapSiswa_Det();

                        if (m_nilai != null)
                        {
                            if (m_nilai.TahunAjaran != null)
                            {
                                m_nilai_det =
                                lst_nilai_siswa_det.FindAll(
                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i < lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i < lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i < lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                    ).FirstOrDefault();
                            }
                        }

                        //get formula nilai nk
                        NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk != null)
                        {
                            if (m_nilai_col_nk.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_kd[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_nk = true;
                            }
                        }
                        //end get formula nilai nk

                        //get formula nilai na
                        NILAI_COL m_nilai_col_na = lst_kolom_nilai_nap.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_na != null)
                        {
                            if (m_nilai_col_na.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_na.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_ap[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_na = true;
                            }
                        }
                        //end get formula nilai na

                        //---get nilainya disini
                        if (lst_nilai_siswa_det != null)
                        {                            
                            if (m_nilai_det != null)
                            {
                                if (m_nilai_det.Nilai != null)
                                {
                                    s_nilai = m_nilai_det.Nilai;
                                }
                            }
                        }

                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                          "'" + s_nilai + "'";
                        //---end get nilai

                        if (is_kolom_nk) //styling kolom nk
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nkd) + " htFontBlack " +
                                             "htBorderRightNKD" + "\", readOnly: true }";
                        }
                        else if (is_kolom_na) //styling kolom na
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nap) + " htFontBlack " +
                                             "htBorderRightNAP" + "\", readOnly: true }";
                        }
                        else //styling kolom nilai
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                        }
                    }

                    //nilai col kp
                    for (int i = (id_col_kp - id_col_kp_count); i < id_col_all - 4; i++)
                    {
                        string s_nilai = "";
                        FORMULA_KP f_kp = lst_formula_kp.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (f_kp != null)
                        {
                            s_nilai = (f_kp.BluePrintFormula.Trim() != "" ? "=" : "") +
                                      "IF(" +
                                          "ROUND(" +
                                            f_kp.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                            ", " +
                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                          ") <= 0, \"\", " +
                                          "ROUND(" +
                                            f_kp.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                            ", " +
                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                          ")" +
                                      ")";
                        }
                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                          "'" + s_nilai + "'";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                        "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nap + " htFontBlack " + "\", readOnly: true }";
                    }
                    //end nilai col kp

                    string s_formula_rapor = s_blueprint_formula_rapor;
                    if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                    {
                        s_formula_rapor = "'" +
                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                        "IF(" +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                            ") <= 0, \"\", " +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                            ")" +
                                        ")" +
                                      "'";
                    }
                    else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        s_formula_rapor = "'" +
                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                        "IF(" +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                "0" +
                                            ") <= 0, \"\", " +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                "0" +
                                            ")" +
                                        ")" +
                                      "'";
                    }

                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      s_formula_rapor;

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 5).ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                    //end get nilai akhir by formula

                    s_content += (s_content.Trim() != "" ? ", " : "") +
                                 "[" +
                                    "\"" + id.ToString() + "\", " +
                                    "\"" + m_siswa.NISSekolah + "\", " +
                                    "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true), 3) + "\", " +
                                    "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                    (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                 "]";

                    //kolom style untuk fixed col header
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                    id++;
                }
                //end load data siswa

            }
            else
            {

                Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran, semester, rel_kelas_det, rel_mapel
                    ).FirstOrDefault();
                List<Rapor_NilaiSiswa> lst_nilai_siswa = null;
                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = null;
                if (m_nilai != null)
                {
                    if (m_nilai.Kurikulum != null)
                    {
                        lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                        lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByHeader_Entity(m_nilai.Kode.ToString());
                    }
                }

                //init js arr siswa
                s_arr_js_siswa = "";
                for (int i = 0; i < id_jml_fixed_row; i++)
                {
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "''";
                }

                int id = 1;
                id_col_nilai_mulai = 4;
                List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        m_kelas.Rel_Sekolah.ToString(),
                        rel_kelas_det,
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
                string s_js_arr_nilai = "";
                foreach (Siswa m_siswa in lst_siswa)
                {
                    css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                    css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                    css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                    css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");

                    s_js_arr_nilai = "";
                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                      "'" + m_siswa.Kode.ToString() + "'";

                    for (int i = id_col_nilai_mulai; i < (id_col_kp - id_col_kp_count); i++)
                    {
                        string s_nilai = "";
                        bool is_kolom_nk = false;
                        bool is_kolom_na = false;
                        is_nilai_manual = false;

                        Rapor_NilaiSiswa_Det m_nilai_det = new Rapor_NilaiSiswa_Det();

                        if (m_nilai != null)
                        {
                            if (m_nilai.TahunAjaran != null)
                            {
                                m_nilai_det =
                                    lst_nilai_siswa_det.FindAll(
                                                m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                     m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i < lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i < lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i < lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                            ).FirstOrDefault();
                            }
                        }

                        //get formula nilai nk
                        NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_nk != null)
                        {
                            if (m_nilai_col_nk.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_kd[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_nk = true;
                            }
                        }
                        //end get formula nilai nk

                        //get formula nilai na
                        NILAI_COL m_nilai_col_na = lst_kolom_nilai_nap.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (m_nilai_col_na != null)
                        {
                            if (m_nilai_col_na.BluePrintFormula != null)
                            {
                                s_nilai = m_nilai_col_na.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());

                                //load nilai manual
                                Rapor_NilaiSiswa_AP_or_KD m_nilai_manual = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString(), QS.GetMapel(),
                                        lst_ap[i].Trim().ToUpper()

                                    ).FirstOrDefault();
                                if (m_nilai_manual != null)
                                {
                                    if (m_nilai_manual.Nilai != null)
                                    {
                                        s_nilai = "=" +
                                                  m_nilai_manual.Nilai;
                                        is_nilai_manual = true;
                                    }
                                }
                                //end load nilai manual

                                is_kolom_na = true;
                            }
                        }
                        //end get formula nilai na

                        //---get nilainya disini
                        if (lst_nilai_siswa_det != null)
                        {                            
                            if (m_nilai_det != null)
                            {
                                if (m_nilai_det.Nilai != null)
                                {
                                    s_nilai = m_nilai_det.Nilai;
                                }
                            }
                        }

                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                          "'" + s_nilai + "'";
                        //---end get nilai

                        if (is_kolom_nk) //styling kolom nk
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nkd) + " htFontBlack " +
                                             "htBorderRightNKD" + "\", readOnly: true }";
                        }
                        else if (is_kolom_na) //styling kolom na
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                             ", className: \"htCenter htMiddle " + (is_nilai_manual ? "bgManual" : css_bg_nap) + " htFontBlack " +
                                             "htBorderRightNAP" + "\", readOnly: true }";
                        }
                        else //styling kolom nilai
                        {
                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                        }
                    }

                    //nilai col kp
                    for (int i = (id_col_kp - id_col_kp_count); i < id_col_all - 4; i++)
                    {
                        string s_nilai = "";
                        FORMULA_KP f_kp = lst_formula_kp.FindAll(m => m.IdKolom == i).FirstOrDefault();
                        if (f_kp != null)
                        {
                            s_nilai = (f_kp.BluePrintFormula.Trim() != "" ? "=" : "") +
                                      "IF(" +
                                          "ROUND(" +
                                            f_kp.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                            ", " +
                                            "2 " + //Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                          ") <= 0, \"\", " +
                                          "ROUND(" +
                                            f_kp.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                            ", " +
                                            "2 " + //Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                          ")" +
                                      ")";
                        }
                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                          "'" + s_nilai + "'";
                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                        "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nap + " htFontBlack " + "\", readOnly: true }";
                    }
                    //end nilai col kp

                    string s_formula_rapor = s_blueprint_formula_rapor;
                    if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KTSP)
                    {
                        s_formula_rapor = "'" +
                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                        "IF(" +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                            ") <= 0, \"\", " +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SD.ToString() +
                                            ")" +
                                        ")" +
                                      "'";
                    }
                    else if (DAO_Rapor_StrukturNilai.GetKurikulumByKelas(m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester, rel_kelas_det) == Libs.JenisKurikulum.SD.KURTILAS)
                    {
                        s_formula_rapor = "'" +
                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                        "IF(" +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                "0" +
                                            ") <= 0, \"\", " +
                                            "ROUND(" +
                                                s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                ", " +
                                                "0" +
                                            ")" +
                                        ")" +
                                      "'";
                    }

                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                      s_formula_rapor;

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 5).ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                    //end get nilai akhir by formula

                    s_content += (s_content.Trim() != "" ? ", " : "") +
                                 "[" +
                                    "\"" + id.ToString() + "\", " +
                                    "\"" + m_siswa.NISSekolah + "\", " +
                                    "\"" + Libs.GetPersingkatNama(Libs.GetHTMLSimpleText(m_siswa.Nama.ToUpper(), true), 3) + "\", " +
                                    "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                    (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                 "]";

                    //kolom style untuk fixed col header
                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true }," +
                                     "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                    id++;
                }
                //end load data siswa

            }

            //buat variable array untuk nkd & nap
            s_arr_kolom_na = "";
            s_arr_kolom_nk = "";
            foreach (var item_nkd in lst_kolom_nilai_nkd)
            {
                s_arr_kolom_nk += (s_arr_kolom_nk.Trim() != "" ? ", " : "") +
                                  item_nkd.IdKolom.ToString();
            }
            foreach (var item_nap in lst_kolom_nilai_nap)
            {
                s_arr_kolom_na += (s_arr_kolom_na.Trim() != "" ? ", " : "") +
                                  item_nap.IdKolom.ToString();
            }
            s_arr_kolom_na = "[" + s_arr_kolom_na + "]";
            s_arr_kolom_nk = "[" + s_arr_kolom_nk + "]";
            //end buat variabel

            s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
            s_arr_js_ap = "[" + s_arr_js_ap + "]";
            s_arr_js_kd = "[" + s_arr_js_kd + "]";
            s_arr_js_kp = "[" + s_arr_js_kp + "]";

            s_content = (s_content.Trim() != "" ? "," : "") +
                        s_content;

            string s_data = "var data_nilai = " +
                            "[" +
                                s_kolom +
                                s_content +
                            "];";

            string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SD.NILAI_SISWA.ROUTE);
            s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SD.NILAI_SISWA.DO_SAVE.FILE + "/Do");

            string script = s_data +
                            "var arr_s = " + s_arr_js_siswa + ";" +
                            "var arr_ap = " + s_arr_js_ap + ";" +
                            "var arr_kd = " + s_arr_js_kd + ";" +
                            "var arr_kp = " + s_arr_js_kp + ";" +
                            "var arr_kolom_nkd = " + s_arr_kolom_nk + ";" +
                            "var arr_kolom_nap = " + s_arr_kolom_na + ";" +
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
                                "formulas: true," +
                                "fillHandle: false," +
                                (
                                    is_readonly
                                    ? "readOnly: true, " +
                                      "contextMenu: false, " +                                      
                                      "manualColumnResize: false, " +
                                      "manualRowResize: false, " +
                                      "comments: false, "
                                    : ""
                                ) +
                                "cells: function (row, col, prop) {" +
                                  "if(this.instance.getData().length != 0){" +
                                    "var cellProperties = {};" +
                                    "if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                        "if((row + 1) % 2 !== 0){" +
                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                        "} else {" +
                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                        "} " +
                                    "} " +
                                    "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                        "if((row + 1) % 2 !== 0){" +
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
                                        (
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel_sikap.Trim() != ""
                                            ? "var mp = '';"
                                            : "var mp = '" + rel_mapel.ToString() + "';"
                                        ) +                                        
                                        "var ms = '" + rel_mapel_sikap.ToString() + "';" +
                                        "var s = arr_s[row];" +
                                        "var ap = arr_ap[col];" +
                                        "var kd = arr_kd[col];" +
                                        "var kp = arr_kp[col];" +
                                        "var n = data_nilai[row][col];" +

                                        "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_nilai_rapor.ToString() + "});" +
                                        "var formula = hot.getDataAtCell(row, " + id_col_nilai_rapor.ToString() + ");" +
                                        "formula = formula.substr(1).toUpperCase();" +
                                        "var newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_nilai_rapor.ToString() + ", id: cellId});" +
                                        "var nr = (newValue.result);" +

                                        //"var lts_ck_hd = (data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "]);" +
                                        //"var lts_ck_kw = (data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "]);" +
                                        //"var lts_ck_ps = (data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "]);" +
                                        //"var lts_ck_pk = (data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "]);" +

                                        "var lts_ck_hd = '';" +
                                        "var lts_ck_kw = '';" +
                                        "var lts_ck_ps = '';" +
                                        "var lts_ck_pk = '';" +
                                        
                                        "var s_url = '" + s_url_save + "' + " +
                                                    "'?' + " +
                                                    "'j=' + '' + '&' + " +
                                                    "'t=' + t + '&sm=' + sm + '&kdt=' + kdt + '&' + " +
                                                    "'s=' + s + '&n=' + n + '&ap=' + ap + '&kd=' + kd + '&kp=' + kp + '&' + " +
                                                    "'mp=' + mp + '&ms=' + ms + '&k=' + k + '&' + " +
                                                    "'nr=' + nr + " +
                                                    "'&lts_ck_hd=' + lts_ck_hd + " +
                                                    "'&lts_ck_kw=' + lts_ck_kw + " +
                                                    "'&lts_ck_ps=' + lts_ck_ps + " +
                                                    "'&lts_ck_pk=' + lts_ck_pk + " +
                                                    "'&ssid=" + Libs.Enkrip(Libs.LOGGED_USER_M.NoInduk) + "'" +
                                                    ";" +

                                        "$.ajax({" +
                                            "url: s_url, " +
                                            "dataType: 'json', " +
                                            "type: 'GET', " +
                                            "processData: false, " +
                                            "contentType: 'application/json; charset=utf-8', " +
                                            "success: function(data) { " +
                                                "}, " +
                                            "error: function(response) { " +
                                                    "alert(response.responseText); " +
                                                "}, " +
                                            "failure: function(response) { " +
                                                    "alert(response.responseText); " +
                                                "} " +
                                        "}); " +

                                    "});" +
                                    "this.render();" +
                                "}, " +
                                (
                                    !is_readonly
                                    ? "afterSelection: function(r,c){ " +
                                            "var find_nkd = arr_kolom_nkd.find(function(col){ return col === c; }); " +
                                            "if(find_nkd !== undefined){ " +
                                                "var cellId = hot.plugin.utils.translateCellCoords({row: r, col: c});" +
                                                "var formula = hot.getDataAtCell(r, c);" +
                                                "formula = (formula.substr(0, 1) === '=' ? formula.substr(1).toUpperCase() : formula.toUpperCase());" +
                                                "var newValue = hot.plugin.parse(formula, {row: r, col: c, id: cellId});" +
                                                "var n_update = (newValue.result);" +
                                                "if(n_update === null) n_update = data_nilai[r][c];" +

                                                "ShowUpdateNilai(n_update, r, c, arr_s[r], data_nilai[r][1], data_nilai[r][2]); " +
                                            "} " +
                                        //"var find_nap = arr_kolom_nap.find(function(col){ return col === c; }); " +
                                        //"if(find_nap !== undefined){ " +
                                        //    "var cellId = hot.plugin.utils.translateCellCoords({row: r, col: c});" +
                                        //    "var formula = hot.getDataAtCell(r, c);" +
                                        //    "formula = (formula.substr(0, 1) === '=' ? formula.substr(1).toUpperCase() : formula.toUpperCase());" +
                                        //    "var newValue = hot.plugin.parse(formula, {row: r, col: c, id: cellId});" +
                                        //    "var n_update = (newValue.result);" +
                                        //    "if(n_update === null) n_update = data_nilai[r][c];" +

                                        //    "ShowUpdateNilai(n_update, r, c, arr_s[r], data_nilai[r][1], data_nilai[r][2]); " +
                                        //"} " +
                                        "}, "
                                    : ""
                                ) +                                
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
                            "container.find('table').addClass('zebraStyle'); " +
                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_rapor.ToString() + "); " +
                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_nilai_mulai.ToString() + "); ";

            ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";
            txtIDColRapor.Value = id_col_nilai_rapor.ToString();
        }

        protected void lnkPilihKelas_Click(object sender, EventArgs e)
        {

        }

        protected void lnkShowStatistics_Click(object sender, EventArgs e)
        {

        }

        protected void btnOKShowBySemester_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    Libs.FILE_PAGE_URL +
                    "?t=" + QS.GetTahunAjaranPure() + "&" +
                    "s=" + txtSemester.Value + "&" +
                    "kd=" + QS.GetKelas() + "&" +
                    (
                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                        : ""
                    ) +
                    "m=" + txtMapel.Value
                );
        }

        protected void ShowListPilihKDUntukLTS()
        {
            string html = "";
            string tahun_ajaran = QS.GetTahunAjaran();
            string rel_kelas = "";
            string rel_kelas_det = QS.GetKelas();
            string rel_mapel = QS.GetMapel();
            string rel_mapel_sikap = QS.GetMapelSikap();
            int id = 1;
            if (rel_mapel_sikap.Trim() != "") rel_mapel = rel_mapel_sikap;

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                }
            }

            if (tahun_ajaran.Trim() != "" && QS.GetSemester().Trim() != "" && rel_kelas.Trim() != "" && rel_mapel.Trim() != "")
            {

                //struktur nilai
                List<Rapor_StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                        tahun_ajaran, QS.GetSemester(), rel_kelas, rel_mapel
                    );

                if (lst_stuktur_nilai.Count == 1)
                {
                    Rapor_StrukturNilai m_struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                    if (m_struktur_nilai != null)
                    {
                        if (m_struktur_nilai.TahunAjaran != null)
                        {

                            bool is_readonly = !DAO_Rapor_StrukturNilai.IsNilaiCanEdit(m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas());
                            List<Rapor_StrukturNilai_AP> lst_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(m_struktur_nilai.Kode.ToString());
                            is_readonly = false;

                            foreach (Rapor_StrukturNilai_AP m_struktur_ap in lst_struktur_ap)
                            {
                                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                                if (m_ap != null)
                                {
                                    if (m_ap.Nama != null)
                                    {
                                        html += "<tr class=\"" + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                                    "<td style=\"padding: 10px; font-weight: bold; padding-top: 5px; padding-bottom: 5px;\">" +
                                                        Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                    "</td>" +
                                                "</tr>";
                                        id++;

                                        List<Rapor_StrukturNilai_KD> lst_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_struktur_ap.Kode.ToString());
                                        foreach (Rapor_StrukturNilai_KD m_struktur_kd in lst_struktur_kd)
                                        {
                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                                            if (m_kd != null)
                                            {
                                                if (m_kd.Nama != null)
                                                {
                                                    string s_checked = "";
                                                    List<Rapor_LTS_Mapel> lst_lts = DAO_Rapor_LTS_Mapel.GetByTABySMByMapelByKelasDet(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetMapel(), QS.GetKelas());
                                                    if (lst_lts.FindAll(m => m.Rel_Rapor_StrukturNilai_KD.Trim().ToLower() == m_struktur_kd.Kode.ToString().Trim().ToLower()).Count > 0)
                                                    {
                                                        s_checked = " checked=\"checked\" ";
                                                    }

                                                    html += "<tr class=\"" + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                                                "<td style=\"vertical-align: middle; padding: 10px; padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-align: justify;\">" +
                                                                    "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto; padding-left: 10px;\">" +
                                                                        "<label for=\"chk_kd_" + m_struktur_kd.Kode.ToString().Replace("-", "_") + "\">" +
                                                                            "<input value=\"" + m_struktur_kd.Kode.ToString() + "\" " +
                                                                                    "class=\"access-hide\" " +
                                                                                    "id=\"chk_kd_" + m_struktur_kd.Kode.ToString().Replace("-", "_") + "\" " +
                                                                                    "name=\"chk_kd[]\" " +
                                                                                    (is_readonly ? "disabled=\"disabled\" " : "") +
                                                                                    s_checked +
                                                                                    "type=\"checkbox\">" +
                                                                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                                            (
                                                                                m_struktur_kd.Deskripsi.Trim() != ""
                                                                                ? "<hr style=\"margin: 0px; border-color: orange;\" />" +
                                                                                  Libs.GetHTMLSimpleText(m_struktur_kd.Deskripsi.Trim()) 
                                                                                : ""
                                                                            ) +
                                                                        "</label>" +
                                                                    "</div>" +
                                                                "</td>" +
                                                            "</tr>";
                                                    id++;

                                                }
                                            }
                                        }

                                    }
                                }
                            }

                            lnkOKPilihKD.Visible = !is_readonly;

                        }
                    }
                }
            }

            ltrPilihKD.Text = "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                  "<table class=\"table\" style=\"margin: 0px;\">" +
                                    html +
                                  "</table>" +
                              "</div>";
        }

        protected void lnkPengaturanLTS_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.ShowPengaturanLTS.ToString();
        }

        protected void lnkOKPilihKD_Click(object sender, EventArgs e)
        {
            DAO_Rapor_LTS_Mapel.DeleteByTABySMByMapelByKelasDet(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetMapel(), QS.GetKelas());
            string s_kd = txtParseKD.Value;
            if (s_kd.Trim() != "")
            {
                string[] arr_kd = s_kd.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);                
                foreach (string item_arr_kd in arr_kd)
                {
                    string[] arr_item_kd = item_arr_kd.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr_item_kd.Length == 2) {
                        DAO_Rapor_LTS_Mapel.Insert(new Rapor_LTS_Mapel {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_StrukturNilai_KD = arr_item_kd[0],
                            Urutan = Libs.GetStringToInteger(arr_item_kd[1]),
                            Rel_KelasDet = QS.GetKelas(),
                            Rel_Mapel = QS.GetMapel(),
                            Semester = QS.GetSemester(),
                            TahunAjaran = QS.GetTahunAjaran()
                        });
                    }
                }
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void lnkOKUpdateNilai_Click(object sender, EventArgs e)
        {
            List<Rapor_NilaiSiswa_AP_or_KD> lst_rapor_nilai_siswa = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                    QS.GetTahunAjaran(),
                    QS.GetSemester(),
                    QS.GetKelas(),
                    txtRelSiswa.Value,
                    QS.GetMapel(),
                    (
                        txtRelRaporStrukturNilai_AP.Value.Trim() != ""
                        ? txtRelRaporStrukturNilai_AP.Value 
                        : txtRelRaporStrukturNilai_KD.Value
                    )
                );

            string rel_rapor_nilai = "";
            Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                    QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetMapel()
                ).FirstOrDefault();
            if (m_nilai != null)
            {
                if (m_nilai.TahunAjaran != null)
                {
                    rel_rapor_nilai = m_nilai.Kode.ToString();
                }
            }

            if (rel_rapor_nilai.Trim() != "")
            {
                if (Libs.GetStringToDecimal(txtUbahNilai.Text) > Libs.GetStringToDecimal(txtKKM.Value))
                {
                    txtUbahNilai.Text = txtKKM.Value;
                }

                if (lst_rapor_nilai_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa_AP_or_KD.Insert(
                        new Rapor_NilaiSiswa_AP_or_KD
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Nilai = rel_rapor_nilai,
                            Rel_StrukturNilai_AP = txtRelRaporStrukturNilai_AP.Value,
                            Rel_StrukturNilai_KD = txtRelRaporStrukturNilai_KD.Value,
                            Rel_Siswa = txtRelSiswa.Value,
                            Nilai = txtUbahNilai.Text
                        });
                }
                else
                {
                    DAO_Rapor_NilaiSiswa_AP_or_KD.Update(
                        new Rapor_NilaiSiswa_AP_or_KD
                        {
                            Kode = lst_rapor_nilai_siswa.FirstOrDefault().Kode,
                            Rel_Rapor_Nilai = rel_rapor_nilai,
                            Rel_StrukturNilai_AP = txtRelRaporStrukturNilai_AP.Value,
                            Rel_StrukturNilai_KD = txtRelRaporStrukturNilai_KD.Value,
                            Rel_Siswa = txtRelSiswa.Value,
                            Nilai = txtUbahNilai.Text
                        });
                }
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void btnDoSaveNilaiRapor_Click(object sender, EventArgs e)
        {
            Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                    QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetMapel()
                ).FirstOrDefault();

            if (m_nilai != null)
            {
                if (m_nilai.TahunAjaran != null)
                {
                    Rapor_NilaiSiswa m_rapor_nilai = DAO_Rapor_NilaiSiswa.GetAllByHeaderBySiswa_Entity(m_nilai.Kode.ToString(), txtRelSiswa.Value).FirstOrDefault();
                    if (m_rapor_nilai != null)
                    {
                        if (m_rapor_nilai.Rel_Siswa != null)
                        {
                            m_rapor_nilai.Rapor = txtNilaiRapor.Value;
                            DAO_Rapor_NilaiSiswa.Update(m_rapor_nilai, Libs.LOGGED_USER_M.UserID);
                            txtKeyAction.Value = JenisAction.DoUpdateNilaiRapor.ToString();
                        }
                    }
                }
            }
        }

        protected void lnkOKUpdateKembalikan_Click(object sender, EventArgs e)
        {
            List<Rapor_NilaiSiswa_AP_or_KD> lst_rapor_nilai_siswa = DAO_Rapor_NilaiSiswa_AP_or_KD.GetAllByTABySMByKelasByMapelBySiswaByStruktur_Entity(
                    QS.GetTahunAjaran(),
                    QS.GetSemester(),
                    QS.GetKelas(),
                    txtRelSiswa.Value,
                    QS.GetMapel(),
                    (
                        txtRelRaporStrukturNilai_AP.Value.Trim() != ""
                        ? txtRelRaporStrukturNilai_AP.Value
                        : txtRelRaporStrukturNilai_KD.Value
                    )
                );

            string rel_rapor_nilai = "";
            Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                    QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), QS.GetMapel()
                ).FirstOrDefault();
            if (m_nilai != null)
            {
                if (m_nilai.TahunAjaran != null)
                {
                    rel_rapor_nilai = m_nilai.Kode.ToString();
                }
            }

            if (rel_rapor_nilai.Trim() != "")
            {
                if (Libs.GetStringToDecimal(txtUbahNilai.Text) > Libs.GetStringToDecimal(txtKKM.Value))
                {
                    txtUbahNilai.Text = txtKKM.Value;
                }

                if (lst_rapor_nilai_siswa.Count > 0)
                {
                    DAO_Rapor_NilaiSiswa_AP_or_KD.Delete(
                        lst_rapor_nilai_siswa.FirstOrDefault().Kode.ToString(), Libs.LOGGED_USER_M.UserID
                    );
                }
            }

            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }
    }
}