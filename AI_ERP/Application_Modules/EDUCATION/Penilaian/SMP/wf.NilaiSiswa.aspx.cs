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
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_NilaiSiswa : System.Web.UI.Page
    {
        private const string LEBAR_COL_DEFAULT = "50";
        private const string LEBAR_COL_DEFAULT2 = "100";
        private const string LEBAR_COL_DEFAULT_NA = "100";
        private const string LEBAR_COL_DEFAULT_NO = "0.1";

        public enum JenisAction
        {
            DoUpdate,
            DoShowStatistik,
            DoShowPilihSiswa
        }

        protected class NILAI_COL
        {
            public int IdKolom { get; set; }
            public decimal Bobot { get; set; }
            public string BluePrintFormula { get; set; }
        }

        public static class AtributPenilaian
        {
            public static string TahunAjaran { get { return RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")); } }
            public static string Semester { get { return Libs.GetQueryString("s"); } }
            public static string Kelas { get { return Libs.GetQueryString("k"); } }
            public static string KelasDet { get { return Libs.GetQueryString("kd"); } }
            public static string Mapel { get { return Libs.GetQueryString("m"); } }
        }

        public static class QS
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

            public static string GetLevel()
            {
                return Libs.GetQueryString("k");
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

        protected string GetRangeCell(string cells)
        {
            string[] arr_cells = cells.Replace(" ", "").Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
            if (arr_cells.Length == 1)
            {
                return arr_cells[0];
            }
            else if (arr_cells.Length > 1)
            {
                return arr_cells[0] + ":" + arr_cells[arr_cells.Length - 1];
            }
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowSubHeaderGuru = true;
            this.Master.ShowHeaderSubTitle = false;
            this.Master.SelectMenuGuru_Penilaian();

            InitURLOnMenu();

            if (DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                   DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Application_Libs.Libs.JENIS_MAPEL.EKSKUL)
            {
                if (!IsPostBack)
                {
                    txtSemester.Value = QS.GetSemester();
                    div_button_settings.Visible = false;
                    if (QS.GetSemester().Trim() != "")
                    {
                        div_button_settings.Visible = true;
                        if (Libs.GetStringToInteger(QS.GetTahunAjaran().Substring(0, 4)) >= 2020)
                        {
                            LoadDataEkskul_2020(QS.GetSemester());
                        }
                        else
                        {
                            LoadDataEkskul(QS.GetSemester());
                        }
                    }
                    else
                    {
                        RenderCenterMenu();
                    }
                }

                this.Master.ShowSubHeaderGuru = false;
                this.Master.ShowHeaderSubTitle = false;

                string s_level = QS.GetLevel();
                string[] arr_level = s_level.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> lst_kelas = arr_level.ToList();
                _UI.InitModalListNilaiEkskul(
                    this.Page,
                    ltrListEkskul, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetMapel(), lst_kelas, QS.GetGuru()
                );
            }
            else
            {
                if (!IsPostBack)
                {
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
                }

                _UI.InitModalListNilai(
                    this.Page,
                    ltrListNilaiAkademik, 
                    ltrListSikap,
                    ltrListEkskul, 
                    ltrListNilaiRapor, 
                    QS.GetTahunAjaran(), 
                    QS.GetMapel(), 
                    QS.GetKelas(), 
                    QS.GetGuru()
                );
            }

            lnkShowStatistics.Visible = false;
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
                    li_nilai_akademik.Visible = false;
                    li_nilai_rapor.Visible = false;

                    li_nilai_akademik.Attributes.Remove("class");
                    li_nilai_ekskul.Attributes.Remove("class");
                    li_nilai_ekskul.Attributes.Add("class", "active");
                }
                else
                {
                    li_nilai_ekskul.Visible = false;
                }
            }
            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
                //this.Master.SetURLGuru_Penilaian("");
                //this.Master.SetURLGuru_Siswa("");
                //this.Master.SetURLGuru_TimeLine("");
            }
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
            string s_mapel = "";
            string s_kelas = "";
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(QS.GetKelas());

            if (DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Application_Libs.Libs.JENIS_MAPEL.EKSKUL)
            {
                foreach (string item_kelas in QS.GetLevel().Split(new string[] { ";" }, StringSplitOptions.None))
                {
                    Kelas m_level = DAO_Kelas.GetByID_Entity(item_kelas);
                    if (m_level != null)
                    {
                        if (m_level.Nama != null)
                        {
                            s_kelas += (s_kelas.Trim() != "" ? "," : "") +
                                       m_level.Nama;
                        }
                    }
                }
                if (s_kelas.Trim() == "")
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            s_kelas = m_kelas_det.Nama;
                        }
                    }
                }

                s_mapel = "";
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

                if (s_kelas.Trim() != "")
                {
                    if (s_mapel.Trim() != "")
                    {
                        s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                    "Kelas" +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: bold;\">" + s_kelas + "</span>" +
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
                                    "<span style=\"font-weight: bold;\">" + s_kelas + "</span>" +
                                    "&nbsp;" +
                                    tahun_ajaran +
                                    "<br />" +
                                    "<label style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">Guru Kelas atau Wali Kelas</label>" +
                                 "</label>";
                    }
                }

                ltrCaption.Text = s_html;
            }
            else
            {
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
                                        "&kd=" + Libs.GetQueryString("kd") +
                                        (
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                            ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                            : ""
                                        ) +
                                        (
                                            Libs.GetQueryString("ft").Trim() != ""
                                            ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                            : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                                        ) +
                                        (
                                            Libs.GetQueryString("g").Trim() != ""
                                            ? "&" + "g=" + Libs.GetQueryString("g").Trim()
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
                                    "&kd=" + Libs.GetQueryString("kd") +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                        ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                        : ""
                                    ) +
                                    (
                                        Libs.GetQueryString("ft").Trim() != ""
                                        ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                        : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                                    ) +
                                    (
                                        Libs.GetQueryString("g").Trim() != ""
                                        ? "&" + "g=" + Libs.GetQueryString("g").Trim()
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
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("ft").Trim() != ""
                                ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                            ) +
                            (
                                Libs.GetQueryString("g").Trim() != ""
                                ? "&" + "g=" + Libs.GetQueryString("g").Trim()
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
                            "&s=" + Libs.GetQueryString("s") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("ft").Trim() != ""
                                ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                            ) +
                            (
                                Libs.GetQueryString("g").Trim() != ""
                                ? "&" + "g=" + Libs.GetQueryString("g").Trim()
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
            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            "&s=" + Libs.GetQueryString("s") +
                            "&kd=" + Libs.GetQueryString("kd") +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("ft").Trim() != ""
                                ? "&" + "ft=" + Libs.GetQueryString("ft").Trim()
                                : "&ft=" + Libs.Encryptdata(arr_bg_image[1])
                            ) +
                            (
                                Libs.GetQueryString("g").Trim() != ""
                                ? "&" + "g=" + Libs.GetQueryString("g").Trim()
                                : ""
                            ) +
                            (
                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                ? "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : ""
                            )
                        )
                );
        }

        protected void LoadData(string semester)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = "";
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");
            string s_rata_rata_kp = "";
            string s_rata_rata_na = "";
            string s_rata_rata_rapor = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                }
            }

            List<NILAI_COL> lst_kolom_nilai_nkd = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_pb = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PH = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PTS = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PAS = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PH_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PTS_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PAS_bobot = new List<NILAI_COL>();

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            lnkPilihSiswa.Visible = false;
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
                                                 "</span>";

                            if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.PILIHAN)
                            {
                                lnkPilihSiswa.Visible = true;
                            }
                        }
                    }

                    m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
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

                    string js_statistik = "";

                    //struktur nilai
                    lst_kolom_nilai_nkd.Clear();
                    List<Rapor_StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas, rel_mapel
                        );

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
                    string css_bg_readonly = "#fff";
                    string css_bg_for_pengetahuan = "#fff";

                    int id_jml_fixed_row = 3;
                    int id_col_mulai_content = 4;
                    int id_col_all = id_col_mulai_content;
                    int id_col_nilai_rapor = 0;

                    List<string> lst_ap = new List<string>();
                    List<string> lst_kd = new List<string>();
                    List<string> lst_kp = new List<string>();
                    List<string> lst_kp_tugas = new List<string>();
                    List<string> lst_kp_uh_terakhir = new List<string>();
                    List<string> lst_kp_uh_non_terakhir = new List<string>();

                    string s_formula_ph_pts_pas = "";
                    string s_formula_item_ph_pts_pas = "";

                    string s_arr_js_siswa = "";
                    string s_arr_js_ap = "";
                    string s_arr_js_kd = "";
                    string s_arr_js_kp = "";
                    string s_arr_js_pb = "";
                    string s_arr_js_pb_asli_locked_cells = "";

                    if (lst_stuktur_nilai.Count == 1)
                    {
                        Rapor_StrukturNilai m_struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                        if (m_struktur_nilai != null)
                        {
                            if (m_struktur_nilai.TahunAjaran != null)
                            {
                                bool is_readonly = _UI.IsReadonlyNilai(
                                        m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas(), rel_mapel, m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester
                                    );
                                is_readonly = (Libs.GetQueryString("action") == "ubahoke" ? false : is_readonly);

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

                                if (m_struktur_nilai.KKM > 0)
                                {
                                    ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;KKM :" : "") +
                                                         "&nbsp;" +
                                                         "<span style=\"font-weight: bold;\">" + Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP) + "</span>";

                                    txtKKM.Value = Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP).ToString();
                                }

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

                                List<string> lst_nama_kd = new List<string>();
                                lst_nama_kd.Clear();

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
                                            s_rata_rata_na = "";

                                            //load kd
                                            int id_nk = 1;
                                            List<Rapor_StrukturNilai_KD> lst_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_struktur_ap.Kode.ToString());
                                            foreach (Rapor_StrukturNilai_KD m_struktur_kd in lst_struktur_kd)
                                            {
                                                jml_merge_kd = 0;
                                                int jml_pb = 0;

                                                Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                if (m_kd != null)
                                                {
                                                    if (m_kd.Nama != null)
                                                    {
                                                        lst_nama_kd.Add(m_kd.Nama);
                                                        s_formula = "";

                                                        string s_formula_kp = "";
                                                        s_rata_rata_kp = "";

                                                        //load kp
                                                        int id_kp = 1;
                                                        List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(m_struktur_kd.Kode.ToString());
                                                        foreach (Rapor_StrukturNilai_KP m_struktur_kp in lst_struktur_kp)
                                                        {
                                                            Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_struktur_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                            if (m_kp != null)
                                                            {
                                                                if (m_kp.Nama != null)
                                                                {
                                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                    (id_col_all).ToString();

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
                                                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                       "\"" +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
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
                                                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
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


                                                                    id_col_all++;
                                                                    jml_merge_kd++;
                                                                    jml_merge_ap++;

                                                                    //jika ada perbaikan
                                                                    if (m_struktur_kp.IsAdaPB)
                                                                    {
                                                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                        (id_col_all).ToString();

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
                                                                                           "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                           "\"" +
                                                                                                Libs.GetHTMLSimpleText(m_kd.Nama) +
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
                                                                                           "\"PB" +
                                                                                                Libs.GetHTMLSimpleText(m_kp.Nama) +
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
                                                                                                        "OR(" +
                                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                                        "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                    ")" +
                                                                                                    "*(" + (m_struktur_kp.BobotNK.ToString()) + "%)" +
                                                                                              ")"
                                                                                            : (
                                                                                                    m_struktur_kd.JenisPerhitungan ==
                                                                                                        ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                                                                    ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                                      "IF(" +
                                                                                                            "OR(" +
                                                                                                                Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                                                Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                                            "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                                      ")"
                                                                                                    : ""
                                                                                              )
                                                                                        );
                                                                        if (m_struktur_kd.JenisPerhitungan ==
                                                                          ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()) {
                                                                            s_rata_rata_kp += (s_rata_rata_kp.Trim() != "" ? "," : "") +
                                                                                              "IF(" +
                                                                                                    "OR(" +
                                                                                                        Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                                        Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                                    "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                              ")";
                                                                        }
                                                                        //end item formula kp

                                                                        //formula pb
                                                                        lst_kolom_pb.Add(new NILAI_COL {
                                                                            IdKolom = id_col_all,
                                                                            BluePrintFormula = "=" +
                                                                                               "IF(" +
                                                                                                    "OR(" +
                                                                                                        Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                        Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                                    "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                                ")",
                                                                            Bobot = m_struktur_kp.BobotNK
                                                                        });
                                                                        //end formula pb

                                                                        //array js pb
                                                                        s_arr_js_pb += (s_arr_js_pb.Trim() != "" ? "," : "") +
                                                                                       id_col_all.ToString();
                                                                        //end array js pb

                                                                        if (
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                            )
                                                                        {
                                                                            lst_kp_tugas.Add(
                                                                                    "IF(" +
                                                                                        "OR(" +
                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                        "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                    ")"
                                                                                );
                                                                        }
                                                                        else if (
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("UH", StringComparison.CurrentCulture) >= 0
                                                                            )
                                                                        {
                                                                            if (id_kp < lst_struktur_kp.Count)
                                                                            {
                                                                                lst_kp_uh_non_terakhir.Add(
                                                                                        "IF(" +
                                                                                            "OR(" +
                                                                                                Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                                Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                            "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                        ")"
                                                                                    );
                                                                            }
                                                                            else if (id_kp == lst_struktur_kp.Count)
                                                                            {
                                                                                lst_kp_uh_terakhir.Add(
                                                                                        "IF(" +
                                                                                            "OR(" +
                                                                                                Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                                Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                            "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                        ")"
                                                                                    );
                                                                            }
                                                                        }
                                                                        else if (
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("PTS", StringComparison.CurrentCulture) >= 0
                                                                            )
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    "IF(" +
                                                                                        "OR(" +
                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                        "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                    ")"
                                                                                );
                                                                        }
                                                                        else if (
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("PAS", StringComparison.CurrentCulture) >= 0 ||
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("PAT", StringComparison.CurrentCulture) >= 0
                                                                            )
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    "IF(" +
                                                                                        "OR(" +
                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"\", " +
                                                                                            Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"" +
                                                                                        "), 0, " + Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                                    ")"
                                                                                );
                                                                        }

                                                                        jml_pb++;
                                                                        id_col_all++;
                                                                        jml_merge_kd++;
                                                                        jml_merge_ap++;

                                                                    }
                                                                    else if(!m_struktur_kp.IsAdaPB)
                                                                    {
                                                                        //formula item kp
                                                                        s_formula_kp += (
                                                                                            m_struktur_kd.JenisPerhitungan ==
                                                                                                ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                                                                            ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                              "(" +
                                                                                                    "IF(" +
                                                                                                        "OR(" +
                                                                                                            Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                            Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                                        "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                                    ")" +
                                                                                                    "*(" + (m_struktur_kp.BobotNK.ToString()) + "%)" +
                                                                                              ")"
                                                                                            : (
                                                                                                    m_struktur_kd.JenisPerhitungan ==
                                                                                                        ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                                                                    ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                                      "IF(" +
                                                                                                        "OR(" +
                                                                                                            Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                            Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                                        "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                                      ")"
                                                                                                    : ""
                                                                                              )
                                                                                        );
                                                                        if (m_struktur_kd.JenisPerhitungan ==
                                                                          ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                        {
                                                                            s_rata_rata_kp += (s_rata_rata_kp.Trim() != "" ? "," : "") +
                                                                                              "IF(" +
                                                                                                "OR(" +
                                                                                                    Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                    Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                                "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                              ")";
                                                                        }

                                                                        if (
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                            )
                                                                        {
                                                                            lst_kp_tugas.Add(
                                                                                    "IF(" +
                                                                                        "OR(" +
                                                                                            Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                            Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                        "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                    ")"
                                                                                );
                                                                        }
                                                                        else if (
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("UH", StringComparison.CurrentCulture) >= 0
                                                                            )
                                                                        {
                                                                            if (id_kp < lst_struktur_kp.Count)
                                                                            {
                                                                                lst_kp_uh_non_terakhir.Add(
                                                                                        "IF(" +
                                                                                            "OR(" +
                                                                                                Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                            "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                        ")"
                                                                                    );
                                                                            }
                                                                            else if (id_kp == lst_struktur_kp.Count)
                                                                            {
                                                                                lst_kp_uh_terakhir.Add(
                                                                                        "IF(" +
                                                                                            "OR(" +
                                                                                                Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                            "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                        ")"
                                                                                    );
                                                                            }
                                                                        }
                                                                        else if (
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("PTS", StringComparison.CurrentCulture) >= 0
                                                                            )
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    "IF(" +
                                                                                        "OR(" +
                                                                                            Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                            Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                        "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                    ")"
                                                                                );
                                                                        }
                                                                        else if (
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("PAS", StringComparison.CurrentCulture) >= 0 ||
                                                                                m_kd.Nama.Trim().ToUpper().IndexOf("PAT", StringComparison.CurrentCulture) >= 0
                                                                            )
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                        "IF(" +
                                                                                            "OR(" +
                                                                                                Libs.GetColHeader(id_col_all) + "# = \"\", " +
                                                                                                Libs.GetColHeader(id_col_all) + "# = \"BL\"" +
                                                                                            "), 0, " + Libs.GetColHeader(id_col_all) + "#" +
                                                                                        ")"
                                                                                    );
                                                                        }
                                                                        //end item formula kp
                                                                    }
                                                                    //end jika ada perbaikan
                                                                }
                                                            }

                                                            id_kp++;

                                                        }
                                                        //end load kp

                                                        //generate formula untuk kp
                                                        if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            s_formula = "IF(" +
                                                                            "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") <= 0, \"\", " +
                                                                            "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") " +
                                                                         ")";
                                                        }
                                                        else if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            string s_formula_rata_rata = "";
                                                            s_formula_rata_rata = "(" + s_formula_kp + ")/" + (jml_merge_kd - jml_pb).ToString();
                                                            //s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_kp) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_kp) + "))";
                                                            s_formula_rata_rata = "IF(COUNT(" + s_rata_rata_kp + ") = 0, 0 , AVERAGE(" + s_rata_rata_kp + "))";

                                                            s_formula = "IF(" +
                                                                            "ROUND(" + s_formula_rata_rata + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") <= 0, \"\", " +
                                                                            "ROUND(" + s_formula_rata_rata + ", " +
                                                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
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
                                                                                "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"), 0, " +
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
                                                                                "OR(" + Libs.GetColHeader(id_col_all + 1) + "# = \"\", " + Libs.GetColHeader(id_col_all + 1) + "# = \"BL\"), 0, " +
                                                                                Libs.GetColHeader(id_col_all + 1) + "#" +
                                                                            ")" +
                                                                        ")"
                                                                    : ""
                                                                    )
                                                            );
                                                        if (m_struktur_ap.JenisPerhitungan ==
                                                                          ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            s_rata_rata_na += (s_rata_rata_na.Trim() != "" ? "+" : "") +
                                                                              Libs.GetColHeader(id_col_all + 1) + "# ";
                                                        }
                                                        //end tambahkan ke formula kd

                                                        //add content kolom nkd
                                                        lst_kolom_nilai_nkd.Add(new NILAI_COL
                                                        {
                                                            BluePrintFormula = "=" + s_formula,
                                                            IdKolom = id_col_all
                                                        });
                                                        //end content kolom nkd

                                                        //tambahkan nk setelah kp

                                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                        (id_col_all).ToString();

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
                                                                           "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                           "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\"";

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

                                            s_formula_ph_pts_pas = "";
                                            if (m_struktur_nilai.Is_PH_PTS_PAS)
                                            {
                                                if (m_ap.Nama.ToLower().IndexOf("pengetahuan") >= 0)
                                                {
                                                    //tambahkan PH, PTS, PAS
                                                    //tambahkan PH
                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    //add ap to var arr js
                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                    lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                    //add kd to var arr js
                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                    lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                    //add kp to var arr js
                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                    lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);
                                                    //end add ap, kd & kp

                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                     LEBAR_COL_DEFAULT;

                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PH" +
                                                                       "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PH" +
                                                                       "\"";

                                                    s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PH" +
                                                                       "\"";

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                    s_formula_item_ph_pts_pas = 
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_tugas.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_tugas.ToArray()) + "))" +
                                                        ",2)";                                                    
                                                    lst_kolom_nilai_PH.Add(new NILAI_COL
                                                    {
                                                        IdKolom = id_col_all,
                                                        Bobot = m_struktur_nilai.BobotPH,
                                                        BluePrintFormula = (
                                                                lst_kp_tugas.Count == 0
                                                                ? ""
                                                                : "=" + s_formula_item_ph_pts_pas
                                                            )
                                                    });
                                                    id_col_all++;

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    //add ap to var arr js
                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                    lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                    //add kd to var arr js
                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                    lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                    //add kp to var arr js
                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                    lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);
                                                    //end add ap, kd & kp

                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                     LEBAR_COL_DEFAULT;

                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PH" +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PH" +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PH" +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();
                                                    
                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                    s_formula_item_ph_pts_pas =
                                                        "ROUND(" +
                                                            "ROUND(" +
                                                                "IF(COUNT(" + String.Join(",", lst_kp_tugas.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_tugas.ToArray()) + "))" +
                                                            ",2)*(" + m_struktur_nilai.BobotPH.ToString() + "/100)" +
                                                        ",2)";
                                                    s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                    lst_kolom_nilai_PH_bobot.Add(new NILAI_COL
                                                    {
                                                        IdKolom = id_col_all,
                                                        Bobot = m_struktur_nilai.BobotPH,
                                                        BluePrintFormula = (
                                                                lst_kp_tugas.Count == 0
                                                                ? ""
                                                                : "=" + s_formula_item_ph_pts_pas
                                                            )
                                                    });
                                                    id_col_all++;
                                                    //end tambahkan PH

                                                    //tambahkan PTS
                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    //add ap to var arr js
                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                    lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                    //add kd to var arr js
                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                    lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                    //add kp to var arr js
                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                    lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);
                                                    //end add ap, kd & kp

                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                     LEBAR_COL_DEFAULT;

                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PTS" +
                                                                       "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PTS" +
                                                                       "\"";

                                                    s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PTS" +
                                                                       "\"";

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();                                                    

                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                    s_formula_item_ph_pts_pas =
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + "))" +
                                                        ",2)";
                                                    lst_kolom_nilai_PTS.Add(new NILAI_COL
                                                    {
                                                        IdKolom = id_col_all,
                                                        Bobot = m_struktur_nilai.BobotPTS,
                                                        BluePrintFormula = (
                                                                lst_kp_uh_non_terakhir.Count == 0
                                                                ? ""
                                                                : "=" + s_formula_item_ph_pts_pas
                                                            )
                                                    });
                                                    id_col_all++;

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    //add ap to var arr js
                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                    lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                    //add kd to var arr js
                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                    lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                    //add kp to var arr js
                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                    lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);
                                                    //end add ap, kd & kp

                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                     LEBAR_COL_DEFAULT;

                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PTS" +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PTS" +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"PTS" +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                    s_formula_item_ph_pts_pas =
                                                        "ROUND(" +
                                                            "ROUND(" +
                                                                "IF(COUNT(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + "))" +
                                                            ",2)*(" + m_struktur_nilai.BobotPTS.ToString() + "/100)" +
                                                        ",2)";
                                                    s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                    lst_kolom_nilai_PTS_bobot.Add(new NILAI_COL
                                                    {
                                                        IdKolom = id_col_all,
                                                        Bobot = m_struktur_nilai.BobotPTS,
                                                        BluePrintFormula = (
                                                                lst_kp_uh_non_terakhir.Count == 0
                                                                ? ""
                                                                : "=" + s_formula_item_ph_pts_pas
                                                            )
                                                    });
                                                    id_col_all++;
                                                    //end tambahkan PTS

                                                    //tambahkan PAS

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    //add ap to var arr js
                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                    lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                    //add kd to var arr js
                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                    lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                    //add kp to var arr js
                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                    lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);
                                                    //end add ap, kd & kp

                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                     LEBAR_COL_DEFAULT;

                                                    string s_label = "PAS";
                                                    if (lst_nama_kd.FindAll(m0 => m0.IndexOf("PAT") >= 0).Count > 0) s_label = "PAT";
                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +

                                                                       "\"" + s_label +
                                                                       "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"" + s_label +
                                                                       "\"";

                                                    s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"" + s_label +
                                                                       "\"";

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                    s_formula_item_ph_pts_pas =
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + "))" +
                                                        ",2)";
                                                    lst_kolom_nilai_PAS.Add(new NILAI_COL
                                                    {
                                                        IdKolom = id_col_all,
                                                        Bobot = m_struktur_nilai.BobotPAS,
                                                        BluePrintFormula = (
                                                                lst_kp_uh_terakhir.Count == 0
                                                                ? ""
                                                                : "=" + s_formula_item_ph_pts_pas
                                                            )
                                                    });
                                                    id_col_all++;

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    //add ap to var arr js
                                                    s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                    lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                    //add kd to var arr js
                                                    s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                    lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                    //add kp to var arr js
                                                    s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                                   "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                    lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);
                                                    //end add ap, kd & kp

                                                    s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                     LEBAR_COL_DEFAULT;

                                                    s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"" + s_label +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"" + s_label +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                       "\"" + s_label +
                                                                            "<br />" +
                                                                            "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                            "</sup>" +
                                                                       "\"";

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                     "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                    s_formula_item_ph_pts_pas =
                                                        "ROUND(" +
                                                            "ROUND(" +
                                                                "IF(COUNT(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + "))" +
                                                            ",2)*(" + m_struktur_nilai.BobotPAS.ToString() + "/100)" +
                                                        ",2)";
                                                    s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                    lst_kolom_nilai_PAS_bobot.Add(new NILAI_COL
                                                    {
                                                        IdKolom = id_col_all,
                                                        Bobot = m_struktur_nilai.BobotPAS,
                                                        BluePrintFormula = (
                                                                lst_kp_uh_terakhir.Count == 0
                                                                ? ""
                                                                : "=" + s_formula_item_ph_pts_pas
                                                            )
                                                    });
                                                    id_col_all++;
                                                    //end tambahkan PAS

                                                    jml_merge_kd++;
                                                    jml_merge_ap++;
                                                    //end tambahkan PH, PTS, PAS

                                                    if (jml_merge_ap > 0)
                                                    {
                                                        s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                        "{ row: 0, col: " + (id_col_all - jml_merge_ap - 5).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                                                    }
                                                }
                                            }

                                            //tambahkan na setelah kp

                                            js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                            (id_col_all).ToString();

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
                                                             LEBAR_COL_DEFAULT_NA;

                                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                               "\"NA<br />" +
                                                               "<span style='font-size: x-small;'>" +
                                                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                               "</span>" +
                                                                    //id_na.ToString() +
                                                                    (
                                                                        m_struktur_ap.BobotRapor > 0
                                                                        ? "<br />" +
                                                                            "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                            "</sup>"
                                                                        : ""
                                                                    ) +
                                                               "\"";

                                            s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                               "\"NA" +
                                                               "<span style='font-size: x-small;'>" +
                                                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                               "</span>" +
                                                                    //id_na.ToString() +
                                                                    (
                                                                        m_struktur_ap.BobotRapor > 0
                                                                        ? "<br />" +
                                                                            "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                            "</sup>"
                                                                        : ""
                                                                    ) +
                                                               "\"";

                                            s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                               "\"NA" +
                                                               "<span style='font-size: x-small;'>" +
                                                                    Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                               "</span>" +
                                                                    //id_na.ToString() +
                                                                    (
                                                                        m_struktur_ap.BobotRapor > 0
                                                                        ? "<br />" +
                                                                            "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                                Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                            "</sup>"
                                                                        : ""
                                                                    ) +
                                                               "\"";

                                            js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                            (id_col_all).ToString();
                                            //end tambahkan na setelah kp

                                            //generate formula nap
                                            //formula item kd dari nk
                                            if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                            {
                                                s_formula_kd = "IF(" +
                                                                    "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                                    "ROUND(" +
                                                                        "(" + s_formula_kd_gabung + ")" +
                                                                        ", " +
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
                                                                    ") " +
                                                               ")";
                                            }
                                            else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                            {
                                                string s_formula_rata_rata = "";
                                                s_formula_rata_rata = "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString();
                                                s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_na) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_na) + "))";

                                                s_formula_kd = "IF(" +
                                                                    s_formula_rata_rata + " <= 0, \"\", " +
                                                                    "ROUND(" +
                                                                        s_formula_rata_rata +
                                                                        ", " +
                                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
                                                                    ")" +
                                                               ")";
                                            }
                                            //end formula item kd dari nk
                                            //end generate formula

                                            //add content kolom nap
                                            if (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS)
                                            {
                                                lst_kolom_nilai_nap.Add(new NILAI_COL
                                                {
                                                    BluePrintFormula = "=" + (
                                                        s_formula_ph_pts_pas.Trim() != ""
                                                        ? "ROUND(" + s_formula_ph_pts_pas + ",0)"
                                                        : "ROUND(" + s_formula_kd + ",0)"
                                                    ),
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_ap.BobotRapor
                                                });
                                            }
                                            else if (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KTSP)
                                            {
                                                lst_kolom_nilai_nap.Add(new NILAI_COL
                                                {
                                                    BluePrintFormula = "=" + (
                                                        s_formula_ph_pts_pas.Trim() != ""
                                                        ? s_formula_ph_pts_pas
                                                        : s_formula_kd
                                                    ),
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_ap.BobotRapor
                                                });
                                            }
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

                                            if (
                                                (!m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") >= 0) ||
                                                (m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") < 0) ||
                                                (!m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") < 0)
                                            )
                                            {
                                                if (jml_merge_ap > 0)
                                                {
                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                    "{ row: 0, col: " + (id_col_all - jml_merge_ap).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                                                }
                                            }
                                        }
                                    }
                                }
                                //end load ap
                                
                                //tambahkan kolom nilai akhir
                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                (id_col_all).ToString();

                                id_col_nilai_rapor = id_col_all;
                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                 (
                                                     (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS)
                                                     ? "1"
                                                     : LEBAR_COL_DEFAULT
                                                 );

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
                                id_col_all++;

                                //perilaku belajar
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
                                
                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                        "{" +
                                        "  row: 0, col: " + (id_col_all - 2) + ", rowspan: 1, " +
                                        "  colspan: 2" +
                                        "} ";
                                s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                                "{" +
                                                "  row: 1, col: " + (id_col_all - 2) + ", rowspan: 1, " +
                                                "  colspan: 2" +
                                                "} ";
                                //end perilaku belajar
                                //id_col_all++;

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

                                //load data siswa
                                //list siswa

                                //get list nilai jika ada
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
                                int id_col_nilai_mulai = 4;
                                List<Siswa> lst_siswa = GetListSiswa(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det
                                    );
                                string s_js_arr_nilai = "";

                                //filter siswa yang dipilih
                                bool ada_pilihan = false;
                                List<string> lst_siswa_dipilih = new List<string>();
                                lst_siswa_dipilih.Clear();
                                
                                var m_siswa_pilihan = DAO_SiswaMapelPilihan.GetByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()).FirstOrDefault();
                                if (m_siswa_pilihan != null)
                                {
                                    if (m_siswa_pilihan.TahunAjaran != null)
                                    {
                                        lst_siswa_dipilih = DAO_SiswaMapelPilihan_Det.GetByHeader_Entity(m_siswa_pilihan.Kode.ToString()).Select(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim()).Distinct().ToList();
                                        ada_pilihan = true;
                                    }
                                }
                                if (ada_pilihan)
                                {
                                    lst_siswa = lst_siswa.FindAll(m0 => lst_siswa_dipilih.Contains(m0.Kode.ToString().ToUpper().Trim()));
                                }
                                //end filter siswa yang dipilih

                                foreach (Siswa m_siswa in lst_siswa)
                                {
                                    css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                                    css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                                    css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                                    css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                                    css_bg_readonly = (id % 2 == 0 ? " htBG9" : " htBG10");
                                    css_bg_for_pengetahuan = (id % 2 == 0 ? " htBG19" : " htBG20");

                                    s_js_arr_nilai = "";
                                    s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                                      "'" + m_siswa.Kode.ToString() + "'";

                                    for (int i = id_col_nilai_mulai; i < id_col_all - 7; i++)
                                    {
                                        string s_nilai = "";
                                        bool is_kolom_nk = false;
                                        bool is_kolom_na = false;

                                        //get formula nilai nk
                                        NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                                        if (m_nilai_col_nk != null)
                                        {
                                            if (m_nilai_col_nk.BluePrintFormula != null)
                                            {
                                                s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
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
                                                is_kolom_na = true;
                                            }
                                        }
                                        //end get formula nilai na

                                        string s_nilai_pb = "";
                                        string s_nilai_asli = "";
                                        bool is_kolom_pb = false;
                                        //---get nilainya disini
                                        if (lst_nilai_siswa_det != null)
                                        {
                                            Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                    m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                         m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i <= lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                         m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                         m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                                ).FirstOrDefault();
                                            if (m_nilai_det != null)
                                            {
                                                if (m_nilai_det.Nilai != null)
                                                {
                                                    s_nilai = m_nilai_det.Nilai;
                                                    s_nilai_pb = m_nilai_det.PB;
                                                }
                                            }
                                        }

                                        s_nilai_asli = s_nilai;

                                        //jika kolom pb
                                        NILAI_COL m_nilai_col_pb = lst_kolom_pb.FindAll(m => m.IdKolom == i).FirstOrDefault();
                                        if (m_nilai_col_pb != null)
                                        {
                                            if (m_nilai_col_pb.BluePrintFormula != null)
                                            {
                                                s_nilai = m_nilai_col_pb.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }

                                            if (s_nilai_pb.Trim() != "")
                                            {
                                                s_nilai = s_nilai_pb;
                                            }

                                            is_kolom_pb = true;
                                        }

                                        //---end get nilai

                                        if (is_kolom_nk) //styling kolom nk
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nkd + " htFontBlack " +
                                                             "htBorderRightNKD" + "\", readOnly: true }";
                                        }
                                        else if (
                                            lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0 ||

                                            lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0 ||

                                            lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0
                                        )
                                        {
                                            //nilainya
                                            if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Trim() != "")
                                                {
                                                    s_nilai = lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                }
                                            }

                                            if (lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                if (lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Trim() != "")
                                                {
                                                    s_nilai = lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                }
                                            }

                                            if (lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                if (lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Trim() != "")
                                                {
                                                    s_nilai = lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                }
                                            }

                                            if (lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                if (lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Trim() != "")
                                                {
                                                    s_nilai = lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                }
                                            }

                                            if (lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                if (lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Trim() != "")
                                                {
                                                    s_nilai = lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                }
                                            }

                                            if (lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                if (lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Trim() != "")
                                                {
                                                    s_nilai = lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                                FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                                }
                                            }

                                            //style nya
                                            if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                                lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                                lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                            {
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                                                 ", className: \"htCenter htMiddle " + css_bg_for_pengetahuan +
                                                                 "\", readOnly: true }";
                                            }
                                            else
                                            {
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                                                 ", className: \"htCenter htMiddle " + css_bg_for_pengetahuan + " htFontBlack " +
                                                                 "\", readOnly: true }";
                                            }
                                        }
                                        else if (is_kolom_na) //styling kolom na
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nap + " htFontBlack " +
                                                             "htBorderRightNAP" + "\", readOnly: true }";
                                        }
                                        else //styling kolom nilai
                                        {
                                            if (s_nilai_pb != "" && is_kolom_pb)
                                            {
                                                //kolom nilai aslinya dikunci
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (i - 1).ToString() + ", className: \"htCenter htMiddle " + css_bg_readonly + " htFontBlack \"," + "readOnly: true" + " }";
                                                //ini kolom nilai PB nya
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\"," + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                                                s_arr_js_pb_asli_locked_cells += (s_arr_js_pb_asli_locked_cells.Trim() != "" ? "," : "") +
                                                                                 "\"" + ((id + id_jml_fixed_row) - 1).ToString().ToString() + "|" + (i - 1).ToString() + "\"";
                                            }
                                            else
                                            {
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\"," + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                                            }
                                        }

                                        //set nilai
                                        s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                          "'" + s_nilai + "'";
                                        //end set nilai
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
                                        s_rata_rata_rapor = "";
                                        foreach (var item in lst_kolom_nilai_nap)
                                        {
                                            s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                                               "IF(" +
                                                                   Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                                   Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                                               ")";
                                            s_rata_rata_rapor += (s_rata_rata_rapor.Trim() != "" ? "+" : "") +
                                                                 Libs.GetColHeader(item.IdKolom + 1) + "#";

                                        }
                                        if (s_formula_rapor.Trim() != "")
                                        {
                                            string s_formula_rata_rata = "";
                                            s_formula_rata_rata = "(" +
                                                                    s_formula_rapor +
                                                                  ")/" + lst_kolom_nilai_nap.Count.ToString();
                                            s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_rapor) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_rapor) + "))";

                                            s_formula_rapor = s_formula_rata_rata;
                                        }
                                    }
                                    s_formula_rapor = "'" +
                                                        (s_formula_rapor.Trim() != "" ? "=" : "") +
                                                        "ROUND(" +
                                                            s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                            ", " +
                                                            Constantas.PEMBULATAN_DESIMAL_NILAI_SMP.ToString() +
                                                        ")" +
                                                      "'";

                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      s_formula_rapor;
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 7).ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                                    //end get nilai akhir by formula

                                    //nilai perilaku belajar
                                    string s_lts_hd = "";
                                    string s_lts_hd_max = "";
                                    if (lst_nilai_siswa != null)
                                    {
                                        if (lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).Count == 1)
                                        {
                                            var m_nilai_siswa = lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                                            s_lts_hd = m_nilai_siswa.LTS_HD.Replace("\"", "").Replace("'", "");
                                            s_lts_hd_max = m_nilai_siswa.LTS_MAX_HD.Replace("\"", "").Replace("'", "");
                                        }
                                    }
                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_lts_hd + "'"; //lts hd
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 6).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_lts_hd_max + "'"; //lts hd maks
                                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                     "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 5).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                    //end nilai perilaku belajar

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
                                                    "\"" + Libs.GetPersingkatNama(m_siswa.Nama.ToUpper(), 3) + "\", " +
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

                                s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
                                s_arr_js_ap = "[" + s_arr_js_ap + "]";
                                s_arr_js_kd = "[" + s_arr_js_kd + "]";
                                s_arr_js_kp = "[" + s_arr_js_kp + "]";
                                s_arr_js_pb = "[" + s_arr_js_pb + "]";
                                s_arr_js_pb_asli_locked_cells = "[" + s_arr_js_pb_asli_locked_cells + "]";

                                s_content = (s_content.Trim() != "" ? "," : "") +
                                            s_content;

                                string s_data = "var data_nilai = " +
                                                "[" +
                                                    s_kolom +
                                                    s_content +
                                                "];";

                                string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMP.NILAI_SISWA.ROUTE);
                                s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMP.NILAI_SISWA.DO_SAVE.FILE + "/Do");
                                
                                string s_cols_nap = "";
                                string s_cols_nkd = "";
                                string s_cols_pb = "";

                                foreach (var item in lst_kolom_nilai_nap)
                                {
                                    s_cols_nap += (s_cols_nap.Trim() != "" ? " || " : "") +
                                                  "col === " + item.IdKolom;
                                }
                                foreach (var item in lst_kolom_nilai_nkd)
                                {
                                    s_cols_nkd += (s_cols_nkd.Trim() != "" ? " || " : "") +
                                                  "col === " + item.IdKolom;
                                }
                                foreach (var item in lst_kolom_pb)
                                {
                                    s_cols_pb += (s_cols_pb.Trim() != "" ? " || " : "") +
                                                  "col === " + item.IdKolom;
                                }

                                string script = s_data +
                                                "var arr_s = " + s_arr_js_siswa + ";" +
                                                "var arr_ap = " + s_arr_js_ap + ";" +
                                                "var arr_kd = " + s_arr_js_kd + ";" +
                                                "var arr_kp = " + s_arr_js_kp + ";" +
                                                "var arr_pb = " + s_arr_js_pb + ";" +
                                                "var arr_pb_asli_locked_cells = " + s_arr_js_pb_asli_locked_cells + ";" +                                                
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
                                                        "if( (parseInt(col) > 3 && parseInt(col) < " + id_col_nilai_rapor + " && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)) || (parseInt(col) > 3 && parseInt(col) < " + id_col_nilai_rapor + " && this.instance.getData()[row][col] == 'BL') ){" +
                                                            "if(arr_pb_asli_locked_cells.indexOf(row + '|' + col) >= 0){" +
                                                                "if((row + 1) % 2 !== 0){" +
                                                                    "cellProperties.className = 'htBG9 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} else {" +
                                                                    "cellProperties.className = 'htBG10 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} " +
                                                            "} else { " +
                                                                "if((row + 1) % 2 !== 0){" +
                                                                    "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} else {" +
                                                                    "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} " +
                                                            "} " +
                                                        "} " +
                                                        "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                            "if(arr_pb_asli_locked_cells.indexOf(row + '|' + col) >= 0){" +
                                                                "if((row + 1) % 2 !== 0){" +
                                                                    "cellProperties.className = 'htBG9 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} else {" +
                                                                    "cellProperties.className = 'htBG10 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} " +
                                                            "} else { " +
                                                                "if((row + 1) % 2 !== 0){" +
                                                                    "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} else {" +
                                                                    "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} " +
                                                            "} " +
                                                        "} " +

                                                        ( //untuk nilai na
                                                            s_cols_nap.Trim() != ""
                                                            ? "if(" + s_cols_nap + "){" +
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

                                                        ( //untuk nilai pb
                                                            s_cols_pb.Trim() != ""
                                                            ? "if(" + s_cols_pb + "){" +
                                                                    "if(data_nilai[row][col].toString().substring(0, 1) === '='){" +
                                                                        "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                            "if((row + 1) % 2 !== 0){" +
                                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} else {" +
                                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} " +
                                                                        "} " +
                                                                        "else {" +
                                                                            "if((row + 1) % 2 !== 0){" +
                                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} else {" +
                                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} " +
                                                                        "} " +
                                                                    "} else {" +
                                                                        "if(parseInt(col) > 3 && parseFloat(data_nilai[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                            "if((row + 1) % 2 !== 0){" +
                                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} else {" +
                                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} " +
                                                                        "} " +
                                                                        "else {" +
                                                                            "if((row + 1) % 2 !== 0){" +
                                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} else {" +
                                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                            "} " +
                                                                        "} " +                                                                    
                                                                    "}" +
                                                              "}"
                                                            : ""
                                                        ) +

                                                        "if(col === " + id_col_nilai_rapor.ToString() + "){" + //untuk nilai rapor
                                                            "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                "if((row + 1) % 2 !== 0){" +
                                                                    "cellProperties.className = 'htBG7 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} else {" +
                                                                    "cellProperties.className = 'htBG8 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} " +
                                                            "} else {" +
                                                                "if((row + 1) % 2 !== 0){" +
                                                                    "cellProperties.className = 'htBG7 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} else {" +
                                                                    "cellProperties.className = 'htBG8 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                "} " +
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
                                                            "var ap = arr_ap[col];" +
                                                            "var kd = arr_kd[col];" +
                                                            "var kp = arr_kp[col];" +
                                                            "var n = data_nilai[row][col];" +
                                                            "var id_col_pb = (arr_pb.indexOf(col) >= 0 ? col : -1);" +

                                                            "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_nilai_rapor.ToString() + "});" +
                                                            "var formula = hot.getDataAtCell(row, " + id_col_nilai_rapor.ToString() + ");" +
                                                            "formula = formula.substr(1).toUpperCase();" +
                                                            "var newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_nilai_rapor.ToString() + ", id: cellId});" +
                                                            "var nr = (newValue.result);" +

                                                            "var lts_hd = data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "];" +
                                                            "var lts_maxhd = data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "];" +
                                                            
                                                            "var lts_ck_hd = (data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "]);" +
                                                            "var lts_ck_kw = (data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "]);" +
                                                            "var lts_ck_ps = (data_nilai[row][" + (id_col_nilai_rapor + 5).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 5).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 5).ToString() + "]);" +
                                                            "var lts_ck_pk = (data_nilai[row][" + (id_col_nilai_rapor + 6).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 6).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 6).ToString() + "]);" +
                                                            
                                                            "var s_url = '" + s_url_save + "' + " +
                                                                        "'?' + " +
                                                                        "'t=' + t + '&sm=' + sm + '&kdt=' + kdt + '&' + " +
                                                                        "'s=' + s + '&n=' + n + '&ap=' + ap + '&kd=' + kd + '&kp=' + kp + '&' + " +
                                                                        "'mp=' + mp + '&k=' + k + '&' + " +
                                                                        "'nr=' + nr + '&' + " +
                                                                        "'lts_hd=' + lts_hd + '&' + " +
                                                                        "'lts_maxhd=' + lts_maxhd + " +
                                                                        "(id_col_pb >= 0 ? '&pb=oke' : '') + " +
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


                                if (js_statistik.Trim() != "")
                                {
                                    js_statistik = "var arr_col = [" + js_statistik + "], ";
                                    js_statistik += "id_fixed_row = " + (id_jml_fixed_row).ToString() + ", ";
                                    js_statistik += "id_fixed_col = " + id_col_nilai_mulai.ToString() + ";";
                                }

                                ltrJSStatistik.Text = js_statistik;
                                ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";

                            }
                            //end if struktur tahun ajaran not null
                        }
                    }

                }//end if nama not null
            }
        }

        protected void LoadDataEkskul(string semester)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";
            string s_guru = Libs.LOGGED_USER_M.NoInduk;
            string s_gr = Libs.GetQueryString("gr").Trim();
            if (s_gr != "")
            {
                s_guru = s_gr;
                div_button_settings.Visible = false;
            }
            else
            {
                div_button_settings.Visible = true;
            }

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = Libs.GetQueryString("k");
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");
            string s_rata_rata_kp = "";
            string s_rata_rata_na = "";
            string s_rata_rata_rapor = "";

            string s_rel_kelas_1 = "";
            string s_rel_kelas_2 = "";
            string s_rel_kelas_3 = "";
            string[] arr_kelas = rel_kelas.Split(new string[] { ";" }, StringSplitOptions.None);
            int id_kelas = 1;
            foreach (string s_rel_kelas in arr_kelas)
            {
                if (id_kelas == 1)
                {
                    s_rel_kelas_1 = s_rel_kelas;
                }
                else if (id_kelas == 2)
                {
                    s_rel_kelas_2 = s_rel_kelas;
                }
                else if (id_kelas == 3)
                {
                    s_rel_kelas_3 = s_rel_kelas;
                }
                id_kelas++;
            }

            List<NILAI_COL> lst_kolom_nilai_nkd = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_pb = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PH = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PTS = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PAS = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PH_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PTS_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PAS_bobot = new List<NILAI_COL>();

            bool kelas_valid = false;
            Kelas m_kelas = DAO_Kelas.GetByID_Entity(s_rel_kelas_1);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null) kelas_valid = true;
            }
            if (!kelas_valid)
            {
                m_kelas = DAO_Kelas.GetByID_Entity(s_rel_kelas_2);
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null) kelas_valid = true;
                }
            }
            if (!kelas_valid)
            {
                m_kelas = DAO_Kelas.GetByID_Entity(s_rel_kelas_3);
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null) kelas_valid = true;
                }
            }

            lnkPilihSiswa.Visible = false;
            if (kelas_valid)
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
                                             "</span>";

                        if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.PILIHAN)
                        {
                            lnkPilihSiswa.Visible = true;
                        }
                    }
                }

                string s_kelas = "";
                foreach (string item_kelas in QS.GetLevel().Split(new string[] { ";" }, StringSplitOptions.None))
                {
                    Kelas m_level = DAO_Kelas.GetByID_Entity(item_kelas);
                    if (m_level != null)
                    {
                        if (m_level.Nama != null)
                        {
                            s_kelas += (s_kelas.Trim() != "" ? "," : "") +
                                       m_level.Nama;
                        }
                    }
                }

                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: normal;\">" + s_kelas + "</span>";

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

                string js_statistik = "";

                //struktur nilai
                lst_kolom_nilai_nkd.Clear();
                List<Rapor_StrukturNilai> lst_stuktur_nilai =
                    DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, semester).FindAll(
                            m0 => m0.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper() &&
                                    m0.Rel_Kelas.ToString().ToUpper() == s_rel_kelas_1.ToString().ToUpper() &&
                                    m0.Rel_Kelas2.ToString().ToUpper() == s_rel_kelas_2.ToString().ToUpper() &&
                                    m0.Rel_Kelas3.ToString().ToUpper() == s_rel_kelas_3.ToString().ToUpper()
                        );

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
                string css_bg_readonly = "#fff";
                string css_bg_for_pengetahuan = "#fff";

                int id_jml_fixed_row = 3;
                int id_col_mulai_content = 4;
                int id_col_all = id_col_mulai_content;
                int id_col_nilai_rapor = 0;

                List<string> lst_ap = new List<string>();
                List<string> lst_kd = new List<string>();
                List<string> lst_kp = new List<string>();
                List<string> lst_kp_tugas = new List<string>();
                List<string> lst_kp_uh_terakhir = new List<string>();
                List<string> lst_kp_uh_non_terakhir = new List<string>();

                string s_formula_ph_pts_pas = "";
                string s_formula_item_ph_pts_pas = "";

                string s_arr_js_siswa = "";
                string s_arr_js_ap = "";
                string s_arr_js_kd = "";
                string s_arr_js_kp = "";
                string s_arr_js_pb = "";
                string s_arr_js_pb_asli_locked_cells = "";

                if (lst_stuktur_nilai.Count == 1)
                {
                    Rapor_StrukturNilai m_struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                    if (m_struktur_nilai != null)
                    {
                        if (m_struktur_nilai.TahunAjaran != null)
                        {
                            //bool is_readonly = _UI.IsReadonlyNilai(
                            //        m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas(), rel_mapel, m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester
                            //    );
                            bool is_readonly = false;

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

                            if (m_struktur_nilai.KKM > 0)
                            {
                                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;KKM :" : "") +
                                                     "&nbsp;" +
                                                     "<span style=\"font-weight: bold;\">" + Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP) + "</span>";

                                txtKKM.Value = Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP).ToString();
                            }

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
                                        s_rata_rata_na = "";

                                        //load kd
                                        int id_nk = 1;
                                        List<Rapor_StrukturNilai_KD> lst_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_struktur_ap.Kode.ToString());
                                        foreach (Rapor_StrukturNilai_KD m_struktur_kd in lst_struktur_kd)
                                        {
                                            jml_merge_kd = 0;
                                            int jml_pb = 0;

                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                                            if (m_kd != null)
                                            {
                                                if (m_kd.Nama != null)
                                                {
                                                    s_formula = "";

                                                    string s_formula_kp = "";
                                                    s_rata_rata_kp = "";

                                                    //load kp
                                                    int id_kp = 1;
                                                    List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(m_struktur_kd.Kode.ToString());
                                                    foreach (Rapor_StrukturNilai_KP m_struktur_kp in lst_struktur_kp)
                                                    {
                                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_struktur_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                        if (m_kp != null)
                                                        {
                                                            if (m_kp.Nama != null)
                                                            {
                                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                (id_col_all).ToString();

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
                                                                                   "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                   "\"" +
                                                                                        Libs.GetHTMLSimpleText(m_kd.Nama) +
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
                                                                                        Libs.GetHTMLSimpleText(m_kp.Nama) +
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


                                                                id_col_all++;
                                                                jml_merge_kd++;
                                                                jml_merge_ap++;

                                                                //jika ada perbaikan
                                                                if (m_struktur_kp.IsAdaPB)
                                                                {
                                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                    (id_col_all).ToString();

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
                                                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                       "\"" +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
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
                                                                                       "\"PB" +
                                                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
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
                                                                    if (m_struktur_kd.JenisPerhitungan ==
                                                                      ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                    {
                                                                        s_rata_rata_kp += (s_rata_rata_kp.Trim() != "" ? "," : "") +
                                                                                          Libs.GetColHeader(id_col_all + 1) + "# ";
                                                                    }
                                                                    //end item formula kp

                                                                    //formula pb
                                                                    lst_kolom_pb.Add(new NILAI_COL
                                                                    {
                                                                        IdKolom = id_col_all,
                                                                        BluePrintFormula = "=" +
                                                                                           Libs.GetColHeader(id_col_all) + "# ",
                                                                        Bobot = m_struktur_kp.BobotNK
                                                                    });
                                                                    //end formula pb

                                                                    //array js pb
                                                                    s_arr_js_pb += (s_arr_js_pb.Trim() != "" ? "," : "") +
                                                                                   id_col_all.ToString();
                                                                    //end array js pb

                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetColHeader(id_col_all + 1) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("UH", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all + 1) + "#"
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all + 1) + "#"
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PTS", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetColHeader(id_col_all + 1) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PAS", StringComparison.CurrentCulture) >= 0 ||
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PAT", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetColHeader(id_col_all + 1) + "#"
                                                                            );
                                                                    }

                                                                    jml_pb++;
                                                                    id_col_all++;
                                                                    jml_merge_kd++;
                                                                    jml_merge_ap++;

                                                                }
                                                                else if (!m_struktur_kp.IsAdaPB)
                                                                {
                                                                    //formula item kp
                                                                    s_formula_kp += (
                                                                                        m_struktur_kd.JenisPerhitungan ==
                                                                                            ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                                                                        ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                          "(" +
                                                                                                "IF(" +
                                                                                                    Libs.GetColHeader(id_col_all) + "# " +
                                                                                                    "= \"\", 0 ," +
                                                                                                    Libs.GetColHeader(id_col_all) + "# " +
                                                                                                ")" +
                                                                                                "*(" + (m_struktur_kp.BobotNK.ToString()) + "%)" +
                                                                                          ")"
                                                                                        : (
                                                                                                m_struktur_kd.JenisPerhitungan ==
                                                                                                    ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                                                                ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                                  "IF(" +
                                                                                                        Libs.GetColHeader(id_col_all) + "# " +
                                                                                                        "= \"\", 0 ," +
                                                                                                        Libs.GetColHeader(id_col_all) + "# " +
                                                                                                  ")"
                                                                                                : ""
                                                                                          )
                                                                                    );
                                                                    if (m_struktur_kd.JenisPerhitungan ==
                                                                      ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                    {
                                                                        s_rata_rata_kp += (s_rata_rata_kp.Trim() != "" ? "," : "") +
                                                                                          Libs.GetColHeader(id_col_all) + "# ";
                                                                    }

                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetColHeader(id_col_all) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("UH", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all) + "#"
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all) + "#"
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PTS", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetColHeader(id_col_all) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PAS", StringComparison.CurrentCulture) >= 0 ||
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PAT", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all) + "#"
                                                                                );
                                                                    }
                                                                    //end item formula kp
                                                                }
                                                                //end jika ada perbaikan
                                                            }
                                                        }

                                                        id_kp++;

                                                    }
                                                    //end load kp

                                                    //generate formula untuk kp
                                                    if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                    {
                                                        s_formula = "IF(" +
                                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") <= 0, \"\", " +
                                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") " +
                                                                     ")";
                                                    }
                                                    else if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                    {
                                                        string s_formula_rata_rata = "";
                                                        s_formula_rata_rata = "(" + s_formula_kp + ")/" + (jml_merge_kd - jml_pb).ToString();
                                                        //s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_kp) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_kp) + "))";
                                                        s_formula_rata_rata = "IF(COUNT(" + s_rata_rata_kp + ") = 0, 0 , AVERAGE(" + s_rata_rata_kp + "))";

                                                        s_formula = "IF(" +
                                                                        "ROUND(" + s_formula_rata_rata + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") <= 0, \"\", " +
                                                                        "ROUND(" + s_formula_rata_rata + ", " +
                                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
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
                                                    if (m_struktur_ap.JenisPerhitungan ==
                                                                      ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                    {
                                                        s_rata_rata_na += (s_rata_rata_na.Trim() != "" ? "+" : "") +
                                                                          Libs.GetColHeader(id_col_all + 1) + "# ";
                                                    }
                                                    //end tambahkan ke formula kd

                                                    //add content kolom nkd
                                                    lst_kolom_nilai_nkd.Add(new NILAI_COL
                                                    {
                                                        BluePrintFormula = "=" + s_formula,
                                                        IdKolom = id_col_all
                                                    });
                                                    //end content kolom nkd

                                                    //tambahkan nk setelah kp

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

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
                                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                       "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\"";

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

                                        s_formula_ph_pts_pas = "";
                                        if (m_struktur_nilai.Is_PH_PTS_PAS)
                                        {
                                            if (m_ap.Nama.ToLower().IndexOf("pengetahuan") >= 0)
                                            {
                                                //tambahkan PH, PTS, PAS
                                                //tambahkan PH
                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "IF(COUNT(" + String.Join(",", lst_kp_tugas.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_tugas.ToArray()) + "))" +
                                                    ",2)";
                                                lst_kolom_nilai_PH.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPH,
                                                    BluePrintFormula = (
                                                            lst_kp_tugas.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_tugas.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_tugas.ToArray()) + "))" +
                                                        ",2)*(" + m_struktur_nilai.BobotPH.ToString() + "/100)" +
                                                    ",2)";
                                                s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                lst_kolom_nilai_PH_bobot.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPH,
                                                    BluePrintFormula = (
                                                            lst_kp_tugas.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;
                                                //end tambahkan PH

                                                //tambahkan PTS
                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "IF(COUNT(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + "))" +
                                                    ",2)";
                                                lst_kolom_nilai_PTS.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPTS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_non_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + "))" +
                                                        ",2)*(" + m_struktur_nilai.BobotPTS.ToString() + "/100)" +
                                                    ",2)";
                                                s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                lst_kolom_nilai_PTS_bobot.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPTS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_non_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;
                                                //end tambahkan PTS

                                                //tambahkan PAS

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "IF(COUNT(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + "))" +
                                                    ",2)";
                                                lst_kolom_nilai_PAS.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPAS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + "))" +
                                                        ",2)*(" + m_struktur_nilai.BobotPAS.ToString() + "/100)" +
                                                    ",2)";
                                                s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                lst_kolom_nilai_PAS_bobot.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPAS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;
                                                //end tambahkan PAS

                                                jml_merge_kd++;
                                                jml_merge_ap++;
                                                //end tambahkan PH, PTS, PAS

                                                if (jml_merge_ap > 0)
                                                {
                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                    "{ row: 0, col: " + (id_col_all - jml_merge_ap - 5).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                                                }
                                            }
                                        }

                                        //tambahkan na setelah kp

                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                        (id_col_all).ToString();

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
                                                         LEBAR_COL_DEFAULT_NA;

                                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                           "\"NA<br />" +
                                                           "<span style='font-size: x-small;'>" +
                                                                Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                           "</span>" +
                                                                //id_na.ToString() +
                                                                (
                                                                    m_struktur_ap.BobotRapor > 0
                                                                    ? "<br />" +
                                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                        "</sup>"
                                                                    : ""
                                                                ) +
                                                           "\"";

                                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                           "\"NA" +
                                                           "<span style='font-size: x-small;'>" +
                                                                Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                           "</span>" +
                                                                //id_na.ToString() +
                                                                (
                                                                    m_struktur_ap.BobotRapor > 0
                                                                    ? "<br />" +
                                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                        "</sup>"
                                                                    : ""
                                                                ) +
                                                           "\"";

                                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                           "\"NA" +
                                                           "<span style='font-size: x-small;'>" +
                                                                Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                           "</span>" +
                                                                //id_na.ToString() +
                                                                (
                                                                    m_struktur_ap.BobotRapor > 0
                                                                    ? "<br />" +
                                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                        "</sup>"
                                                                    : ""
                                                                ) +
                                                           "\"";

                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                        (id_col_all).ToString();
                                        //end tambahkan na setelah kp

                                        //generate formula nap
                                        //formula item kd dari nk
                                        if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                        {
                                            s_formula_kd = "IF(" +
                                                                "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                                "ROUND(" +
                                                                    "(" + s_formula_kd_gabung + ")" +
                                                                    ", " +
                                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
                                                                ") " +
                                                           ")";
                                        }
                                        else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                        {
                                            string s_formula_rata_rata = "";
                                            s_formula_rata_rata = "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString();
                                            s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_na) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_na) + "))";

                                            s_formula_kd = "IF(" +
                                                                s_formula_rata_rata + " <= 0, \"\", " +
                                                                "ROUND(" +
                                                                    s_formula_rata_rata +
                                                                    ", " +
                                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
                                                                ")" +
                                                           ")";
                                        }
                                        //end formula item kd dari nk
                                        //end generate formula

                                        //add content kolom nap
                                        if (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS)
                                        {
                                            lst_kolom_nilai_nap.Add(new NILAI_COL
                                            {
                                                BluePrintFormula = "=" + (
                                                    s_formula_ph_pts_pas.Trim() != ""
                                                    ? "ROUND(" + s_formula_ph_pts_pas + ",0)"
                                                    : "ROUND(" + s_formula_kd + ",0)"
                                                ),
                                                IdKolom = id_col_all,
                                                Bobot = m_struktur_ap.BobotRapor
                                            });
                                        }
                                        else if (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KTSP)
                                        {
                                            lst_kolom_nilai_nap.Add(new NILAI_COL
                                            {
                                                BluePrintFormula = "=" + (
                                                    s_formula_ph_pts_pas.Trim() != ""
                                                    ? s_formula_ph_pts_pas
                                                    : s_formula_kd
                                                ),
                                                IdKolom = id_col_all,
                                                Bobot = m_struktur_ap.BobotRapor
                                            });
                                        }
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

                                        if (
                                            (!m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") >= 0) ||
                                            (m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") < 0) ||
                                            (!m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") < 0)
                                        )
                                        {
                                            if (jml_merge_ap > 0)
                                            {
                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                "{ row: 0, col: " + (id_col_all - jml_merge_ap).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                                            }
                                        }
                                    }
                                }
                            }
                            //end load ap

                            //tambahkan kolom nilai akhir
                            js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                            (id_col_all).ToString();

                            id_col_nilai_rapor = id_col_all;
                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             (
                                                 (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS)
                                                 ? "1"
                                                 : LEBAR_COL_DEFAULT
                                             );

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

                            //load data siswa
                            //list siswa

                            //get list nilai jika ada
                            Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                                    tahun_ajaran, semester, rel_kelas, rel_mapel
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
                            int id_col_nilai_mulai = 4;
                            List<Siswa> lst_siswa = new List<Siswa>();
                            string s_js_arr_nilai = "";

                            var lst_mapel_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetMapelByGuruByTA_Entity(
                                    s_guru, QS.GetTahunAjaran()
                                ).FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim()); ;
                            var lst_formasi_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetKelasFormasiByGuruByTA_Entity(
                                    s_guru, QS.GetTahunAjaran()
                                ).FindAll(m0 => m0.Rel_Mapel.ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim());
                            var lst_formasi_mapel_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetFormasiMapelByGuruByTA_Entity(
                                    s_guru, QS.GetTahunAjaran()
                                ).FindAll(m0 => m0.Rel_Mapel.ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim());
                            foreach (var item_mapel in lst_mapel_ekskul)
                            {
                                var lst_formasi_kelas_ = lst_formasi_kelas.FindAll(
                                    m0 => m0.Rel_Mapel.ToUpper().Trim() == item_mapel.Kode.ToString().ToUpper().Trim()
                                );

                                foreach (var item_formasi_kelas in lst_formasi_kelas_)
                                {
                                    string s_kelas_ekskul = "";
                                    if (item_formasi_kelas.Rel_Kelas1.Trim() != "") s_kelas_ekskul += item_formasi_kelas.Rel_Kelas1.Trim() + ";";
                                    if (item_formasi_kelas.Rel_Kelas2.Trim() != "") s_kelas_ekskul += item_formasi_kelas.Rel_Kelas2.Trim() + ";";
                                    if (item_formasi_kelas.Rel_Kelas3.Trim() != "") s_kelas_ekskul += item_formasi_kelas.Rel_Kelas3.Trim() + ";";

                                    string kode_sn = "";

                                    foreach (var item_formasi_mapel_kelas in lst_formasi_mapel_kelas.FindAll(
                                        m1 => m1.Rel_Mapel.ToUpper().Trim() == item_formasi_kelas.Rel_Mapel.ToString().ToUpper().Trim() &&
                                              m1.Rel_Kelas.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas1.ToString().ToUpper().Trim() &&
                                              m1.Rel_Kelas2.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas2.ToString().ToUpper().Trim() &&
                                              m1.Rel_Kelas3.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas3.ToString().ToUpper().Trim() &&
                                              m1.Semester == semester
                                        ).OrderBy(m0 => m0.Semester)
                                    )
                                    {
                                        kode_sn = item_formasi_mapel_kelas.KodeSN;

                                        string id_per_semester = "ID_" + Guid.NewGuid().ToString().Replace("-", "");

                                        var formasi_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetByID_Entity(
                                                item_formasi_mapel_kelas.KodeFormasiEkskul.ToString()
                                            );

                                        if (formasi_ekskul != null)
                                        {
                                            if (formasi_ekskul.TahunAjaran != null)
                                            {
                                                var lst_siswa_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskulDet.GetByHeader_Entity(formasi_ekskul.Kode.ToString());
                                                var struktur_nilai_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetByID_Entity(formasi_ekskul.Rel_Rapor_StrukturNilai);

                                                if (struktur_nilai_ekskul != null && struktur_nilai_ekskul.TahunAjaran != null)
                                                {
                                                    foreach (var item_formasi_det in lst_siswa_ekskul)
                                                    {

                                                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                                                                    formasi_ekskul.TahunAjaran,
                                                                    formasi_ekskul.Semester,
                                                                    item_formasi_det.Rel_Siswa.ToString());
                                                        if (m_siswa != null)
                                                        {
                                                            if (m_siswa.Nama != null)
                                                            {
                                                                lst_siswa.Add(m_siswa);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                                css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                                css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                                css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                                css_bg_readonly = (id % 2 == 0 ? " htBG9" : " htBG10");
                                css_bg_for_pengetahuan = (id % 2 == 0 ? " htBG19" : " htBG20");

                                s_js_arr_nilai = "";
                                s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                                  "'" + m_siswa.Kode.ToString() + "'";

                                string kelas_det = "";
                                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet.ToString());
                                if (m_kelas_det != null)
                                {
                                    if (m_kelas_det.Nama != null)
                                    {
                                        kelas_det = m_kelas_det.Nama.Trim();
                                    }
                                }

                                for (int i = id_col_nilai_mulai; i < id_col_all; i++)
                                {
                                    string s_nilai = "";
                                    bool is_kolom_nk = false;
                                    bool is_kolom_na = false;

                                    //get formula nilai nk
                                    NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                                    if (m_nilai_col_nk != null)
                                    {
                                        if (m_nilai_col_nk.BluePrintFormula != null)
                                        {
                                            s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
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
                                            is_kolom_na = true;
                                        }
                                    }
                                    //end get formula nilai na

                                    string s_nilai_pb = "";
                                    string s_nilai_asli = "";
                                    bool is_kolom_pb = false;
                                    //---get nilainya disini
                                    if (lst_nilai_siswa_det != null)
                                    {
                                        Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                     m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i <= lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                            ).FirstOrDefault();
                                        if (m_nilai_det != null)
                                        {
                                            if (m_nilai_det.Nilai != null)
                                            {
                                                s_nilai = m_nilai_det.Nilai;
                                                s_nilai_pb = m_nilai_det.PB;
                                            }
                                        }
                                    }

                                    s_nilai_asli = s_nilai;

                                    //jika kolom pb
                                    NILAI_COL m_nilai_col_pb = lst_kolom_pb.FindAll(m => m.IdKolom == i).FirstOrDefault();
                                    if (m_nilai_col_pb != null)
                                    {
                                        if (m_nilai_col_pb.BluePrintFormula != null)
                                        {
                                            s_nilai = m_nilai_col_pb.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                        }

                                        if (s_nilai_pb.Trim() != "")
                                        {
                                            s_nilai = s_nilai_pb;
                                        }

                                        is_kolom_pb = true;
                                    }

                                    //---end get nilai

                                    if (is_kolom_nk) //styling kolom nk
                                    {
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nkd + " htFontBlack " +
                                                         "htBorderRightNKD" + "\", readOnly: true }";
                                    }
                                    else if (
                                        lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                        lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0 ||

                                        lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                        lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0 ||

                                        lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                        lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0
                                    )
                                    {
                                        //nilainya
                                        if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        //style nya
                                        if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                                             ", className: \"htCenter htMiddle " + css_bg_for_pengetahuan +
                                                             "\", readOnly: true }";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                                             ", className: \"htCenter htMiddle " + css_bg_for_pengetahuan + " htFontBlack " +
                                                             "\", readOnly: true }";
                                        }
                                    }
                                    else if (is_kolom_na) //styling kolom na
                                    {
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nap + " htFontBlack " +
                                                         "htBorderRightNAP" + "\", readOnly: true }";
                                    }
                                    else //styling kolom nilai
                                    {
                                        if (s_nilai_pb != "" && is_kolom_pb)
                                        {
                                            //kolom nilai aslinya dikunci
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (i - 1).ToString() + ", className: \"htCenter htMiddle " + css_bg_readonly + " htFontBlack \"," + "readOnly: true" + " }";
                                            //ini kolom nilai PB nya
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\"," + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                                            s_arr_js_pb_asli_locked_cells += (s_arr_js_pb_asli_locked_cells.Trim() != "" ? "," : "") +
                                                                             "\"" + ((id + id_jml_fixed_row) - 1).ToString().ToString() + "|" + (i - 1).ToString() + "\"";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\"," + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                                        }
                                    }

                                    //set nilai
                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_nilai + "'";
                                    //end set nilai
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
                                    s_rata_rata_rapor = "";
                                    foreach (var item in lst_kolom_nilai_nap)
                                    {
                                        s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                                           "IF(" +
                                                               Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                               Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                                           ")";
                                        s_rata_rata_rapor += (s_rata_rata_rapor.Trim() != "" ? "+" : "") +
                                                             Libs.GetColHeader(item.IdKolom + 1) + "#";

                                    }
                                    if (s_formula_rapor.Trim() != "")
                                    {
                                        string s_formula_rata_rata = "";
                                        s_formula_rata_rata = "(" +
                                                                s_formula_rapor +
                                                              ")/" + lst_kolom_nilai_nap.Count.ToString();
                                        s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_rapor) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_rapor) + "))";

                                        s_formula_rapor = s_formula_rata_rata;
                                    }
                                }
                                s_formula_rapor = "'" +
                                                    (s_formula_rapor.Trim() != "" ? "=" : "") +
                                                    "ROUND(" +
                                                        s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                        ", " +
                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMP.ToString() +
                                                    ")" +
                                                  "'";

                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                  s_formula_rapor;

                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                                //end get nilai akhir by formula

                                s_content += (s_content.Trim() != "" ? ", " : "") +
                                             "[" +
                                                "\"" + id.ToString() + "\", " +
                                                "\"" + m_siswa.NISSekolah + "\", " +
                                                "\"" + Libs.GetPersingkatNama(m_siswa.Nama.ToUpper(), 3) + "<label style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</label>\", " +
                                                "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                                (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                             "]";

                                //kolom style untuk fixed col header
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }," +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                                id++;
                            }
                            //end load data siswa

                            s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
                            s_arr_js_ap = "[" + s_arr_js_ap + "]";
                            s_arr_js_kd = "[" + s_arr_js_kd + "]";
                            s_arr_js_kp = "[" + s_arr_js_kp + "]";
                            s_arr_js_pb = "[" + s_arr_js_pb + "]";
                            s_arr_js_pb_asli_locked_cells = "[" + s_arr_js_pb_asli_locked_cells + "]";

                            s_content = (s_content.Trim() != "" ? "," : "") +
                                        s_content;

                            string s_data = "var data_nilai = " +
                                            "[" +
                                                s_kolom +
                                                s_content +
                                            "];";

                            string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMP.NILAI_SISWA.ROUTE);
                            s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMP.NILAI_SISWA.DO_SAVE.FILE + "/Do");

                            string s_cols_nap = "";
                            string s_cols_nkd = "";
                            string s_cols_pb = "";

                            foreach (var item in lst_kolom_nilai_nap)
                            {
                                s_cols_nap += (s_cols_nap.Trim() != "" ? " || " : "") +
                                              "col === " + item.IdKolom;
                            }
                            foreach (var item in lst_kolom_nilai_nkd)
                            {
                                s_cols_nkd += (s_cols_nkd.Trim() != "" ? " || " : "") +
                                              "col === " + item.IdKolom;
                            }
                            foreach (var item in lst_kolom_pb)
                            {
                                s_cols_pb += (s_cols_pb.Trim() != "" ? " || " : "") +
                                              "col === " + item.IdKolom;
                            }

                            string script = s_data +
                                            "var arr_s = " + s_arr_js_siswa + ";" +
                                            "var arr_ap = " + s_arr_js_ap + ";" +
                                            "var arr_kd = " + s_arr_js_kd + ";" +
                                            "var arr_kp = " + s_arr_js_kp + ";" +
                                            "var arr_pb = " + s_arr_js_pb + ";" +
                                            "var arr_pb_asli_locked_cells = " + s_arr_js_pb_asli_locked_cells + ";" +
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
                                                        "if(arr_pb_asli_locked_cells.indexOf(row + '|' + col) >= 0){" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG9 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG10 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} else { " +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} " +
                                                    "} " +
                                                    "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                        "if(arr_pb_asli_locked_cells.indexOf(row + '|' + col) >= 0){" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG9 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG10 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} else { " +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} " +
                                                    "} " +

                                                    ( //untuk nilai na
                                                        s_cols_nap.Trim() != ""
                                                        ? "if(" + s_cols_nap + "){" +
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

                                                    ( //untuk nilai pb
                                                        s_cols_pb.Trim() != ""
                                                        ? "if(" + s_cols_pb + "){" +
                                                                "if(data_nilai[row][col].toString().substring(0, 1) === '='){" +
                                                                    "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                    "else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                "} else {" +
                                                                    "if(parseInt(col) > 3 && parseFloat(data_nilai[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                    "else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                "}" +
                                                          "}"
                                                        : ""
                                                    ) +

                                                    "if(col === " + id_col_nilai_rapor.ToString() + "){" + //untuk nilai rapor
                                                        "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG7 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG8 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} else {" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG7 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG8 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
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
                                                        "var kdt = '" + rel_kelas.ToString() + "';" +
                                                        "var k = '" + rel_kelas.ToString() + "';" +
                                                        "var mp = '" + rel_mapel.ToString() + "';" +
                                                        "var s = arr_s[row];" +
                                                        "var ap = arr_ap[col];" +
                                                        "var kd = arr_kd[col];" +
                                                        "var kp = arr_kp[col];" +
                                                        "var n = data_nilai[row][col];" +
                                                        "var id_col_pb = (arr_pb.indexOf(col) >= 0 ? col : -1);" +

                                                        "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_nilai_rapor.ToString() + "});" +
                                                        "var formula = hot.getDataAtCell(row, " + id_col_nilai_rapor.ToString() + ");" +
                                                        "formula = formula.substr(1).toUpperCase();" +
                                                        "var newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_nilai_rapor.ToString() + ", id: cellId});" +
                                                        "var nr = (newValue.result);" +

                                                        "var s_url = '" + s_url_save + "' + " +
                                                                    "'?' + " +
                                                                    "'t=' + t + '&sm=' + sm + '&kdt=' + kdt + '&' + " +
                                                                    "'s=' + s + '&n=' + n + '&ap=' + ap + '&kd=' + kd + '&kp=' + kp + '&' + " +
                                                                    "'mp=' + mp + '&k=' + k + '&' + " +
                                                                    "'nr=' + nr + " +
                                                                    "(id_col_pb >= 0 ? '&pb=oke' : '') + " +
                                                                    "'&ssid=" + Libs.Enkrip(s_guru) + "'" +
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
                                                        "}); " +
                                                    "});" +
                                                    "this.render();" +
                                                "}, " +
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


                            if (js_statistik.Trim() != "")
                            {
                                js_statistik = "var arr_col = [" + js_statistik + "], ";
                                js_statistik += "id_fixed_row = " + (id_jml_fixed_row).ToString() + ", ";
                                js_statistik += "id_fixed_col = " + id_col_nilai_mulai.ToString() + ";";
                            }

                            ltrJSStatistik.Text = js_statistik;
                            ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";

                        }
                        //end if struktur tahun ajaran not null
                    }
                }
            }
        }

        protected void LoadDataEkskul_2020(string semester)
        {
            ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";
            string s_guru = Libs.LOGGED_USER_M.NoInduk;
            string s_gr = Libs.GetQueryString("gr").Trim();
            if (s_gr != "")
            {
                s_guru = s_gr;
                div_button_settings.Visible = false;
            }
            else
            {
                div_button_settings.Visible = true;
            }

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = Libs.GetQueryString("k");
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");
            string s_rata_rata_kp = "";
            string s_rata_rata_na = "";
            string s_rata_rata_rapor = "";

            string s_rel_kelas_1 = "";
            string s_rel_kelas_2 = "";
            string s_rel_kelas_3 = "";
            string[] arr_kelas = rel_kelas.Split(new string[] { ";" }, StringSplitOptions.None);
            int id_kelas = 1;
            foreach (string s_rel_kelas in arr_kelas)
            {
                if (id_kelas == 1)
                {
                    s_rel_kelas_1 = s_rel_kelas;
                }
                else if (id_kelas == 2)
                {
                    s_rel_kelas_2 = s_rel_kelas;
                }
                else if (id_kelas == 3)
                {
                    s_rel_kelas_3 = s_rel_kelas;
                }
                id_kelas++;
            }

            List<NILAI_COL> lst_kolom_nilai_nkd = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_nap = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_pb = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PH = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PTS = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PAS = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PH_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PTS_bobot = new List<NILAI_COL>();
            List<NILAI_COL> lst_kolom_nilai_PAS_bobot = new List<NILAI_COL>();

            bool kelas_valid = false;
            Kelas m_kelas = DAO_Kelas.GetByID_Entity(s_rel_kelas_1);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null) kelas_valid = true;
            }
            if (!kelas_valid)
            {
                m_kelas = DAO_Kelas.GetByID_Entity(s_rel_kelas_2);
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null) kelas_valid = true;
                }
            }
            if (!kelas_valid)
            {
                m_kelas = DAO_Kelas.GetByID_Entity(s_rel_kelas_3);
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null) kelas_valid = true;
                }
            }

            lnkPilihSiswa.Visible = false;
            if (kelas_valid)
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
                                             "</span>";

                        if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.PILIHAN)
                        {
                            lnkPilihSiswa.Visible = true;
                        }
                    }
                }

                string s_kelas = "";
                foreach (string item_kelas in QS.GetLevel().Split(new string[] { ";" }, StringSplitOptions.None))
                {
                    Kelas m_level = DAO_Kelas.GetByID_Entity(item_kelas);
                    if (m_level != null)
                    {
                        if (m_level.Nama != null)
                        {
                            s_kelas += (s_kelas.Trim() != "" ? "," : "") +
                                       m_level.Nama;
                        }
                    }
                }

                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                    "&nbsp;" +
                                    "<span style=\"font-weight: normal;\">" + s_kelas + "</span>";

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

                string js_statistik = "";

                //struktur nilai
                lst_kolom_nilai_nkd.Clear();
                List<Rapor_StrukturNilai> lst_stuktur_nilai =
                    DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, semester).FindAll(
                            m0 => m0.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper() &&
                                    m0.Rel_Kelas.ToString().ToUpper() == s_rel_kelas_1.ToString().ToUpper() &&
                                    m0.Rel_Kelas2.ToString().ToUpper() == s_rel_kelas_2.ToString().ToUpper() &&
                                    m0.Rel_Kelas3.ToString().ToUpper() == s_rel_kelas_3.ToString().ToUpper()
                        );

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
                string css_bg_readonly = "#fff";
                string css_bg_for_pengetahuan = "#fff";

                int id_jml_fixed_row = 3;
                int id_col_mulai_content = 4;
                int id_col_all = id_col_mulai_content;
                int id_col_nilai_rapor = 0;

                List<string> lst_ap = new List<string>();
                List<string> lst_kd = new List<string>();
                List<string> lst_kp = new List<string>();
                List<string> lst_kp_tugas = new List<string>();
                List<string> lst_kp_uh_terakhir = new List<string>();
                List<string> lst_kp_uh_non_terakhir = new List<string>();

                string s_formula_ph_pts_pas = "";
                string s_formula_item_ph_pts_pas = "";

                string s_arr_js_siswa = "";
                string s_arr_js_ap = "";
                string s_arr_js_kd = "";
                string s_arr_js_kp = "";
                string s_arr_js_pb = "";
                string s_arr_js_pb_asli_locked_cells = "";

                if (lst_stuktur_nilai.Count == 1)
                {
                    Rapor_StrukturNilai m_struktur_nilai = lst_stuktur_nilai.FirstOrDefault();
                    if (m_struktur_nilai != null)
                    {
                        if (m_struktur_nilai.TahunAjaran != null)
                        {
                            //bool is_readonly = _UI.IsReadonlyNilai(
                            //        m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas(), rel_mapel, m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester
                            //    );
                            bool is_readonly = false;

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

                            if (m_struktur_nilai.KKM > 0)
                            {
                                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;KKM :" : "") +
                                                     "&nbsp;" +
                                                     "<span style=\"font-weight: bold;\">" + Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP) + "</span>";

                                txtKKM.Value = Math.Round(m_struktur_nilai.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP).ToString();
                            }

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
                                        s_rata_rata_na = "";

                                        //load kd
                                        int id_nk = 1;
                                        List<Rapor_StrukturNilai_KD> lst_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_struktur_ap.Kode.ToString());
                                        foreach (Rapor_StrukturNilai_KD m_struktur_kd in lst_struktur_kd)
                                        {
                                            jml_merge_kd = 0;
                                            int jml_pb = 0;

                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                                            if (m_kd != null)
                                            {
                                                if (m_kd.Nama != null)
                                                {
                                                    s_formula = "";

                                                    string s_formula_kp = "";
                                                    s_rata_rata_kp = "";

                                                    //load kp
                                                    int id_kp = 1;
                                                    List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(m_struktur_kd.Kode.ToString());
                                                    foreach (Rapor_StrukturNilai_KP m_struktur_kp in lst_struktur_kp)
                                                    {
                                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_struktur_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                        if (m_kp != null)
                                                        {
                                                            if (m_kp.Nama != null)
                                                            {
                                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                (id_col_all).ToString();

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
                                                                                   "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                   "\"" +
                                                                                        Libs.GetHTMLSimpleText(m_kd.Nama) +
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
                                                                                        Libs.GetHTMLSimpleText(m_kp.Nama) +
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


                                                                id_col_all++;
                                                                jml_merge_kd++;
                                                                jml_merge_ap++;

                                                                //jika ada perbaikan
                                                                if (m_struktur_kp.IsAdaPB)
                                                                {
                                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                                    (id_col_all).ToString();

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
                                                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                                       "\"" +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
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
                                                                                       "\"PB" +
                                                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
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
                                                                    if (m_struktur_kd.JenisPerhitungan ==
                                                                      ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                    {
                                                                        s_rata_rata_kp += (s_rata_rata_kp.Trim() != "" ? "," : "") +
                                                                                          Libs.GetColHeader(id_col_all + 1) + "# ";
                                                                    }
                                                                    //end item formula kp

                                                                    //formula pb
                                                                    lst_kolom_pb.Add(new NILAI_COL
                                                                    {
                                                                        IdKolom = id_col_all,
                                                                        BluePrintFormula = "=" +
                                                                                           Libs.GetColHeader(id_col_all) + "# ",
                                                                        Bobot = m_struktur_kp.BobotNK
                                                                    });
                                                                    //end formula pb

                                                                    //array js pb
                                                                    s_arr_js_pb += (s_arr_js_pb.Trim() != "" ? "," : "") +
                                                                                   id_col_all.ToString();
                                                                    //end array js pb

                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetColHeader(id_col_all + 1) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("UH", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all + 1) + "#"
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all + 1) + "#"
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PTS", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetColHeader(id_col_all + 1) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PAS", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetColHeader(id_col_all + 1) + "#"
                                                                            );
                                                                    }

                                                                    jml_pb++;
                                                                    id_col_all++;
                                                                    jml_merge_kd++;
                                                                    jml_merge_ap++;

                                                                }
                                                                else if (!m_struktur_kp.IsAdaPB)
                                                                {
                                                                    //formula item kp
                                                                    s_formula_kp += (
                                                                                        m_struktur_kd.JenisPerhitungan ==
                                                                                            ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                                                                        ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                          "(" +
                                                                                                "IF(" +
                                                                                                    Libs.GetColHeader(id_col_all) + "# " +
                                                                                                    "= \"\", 0 ," +
                                                                                                    Libs.GetColHeader(id_col_all) + "# " +
                                                                                                ")" +
                                                                                                "*(" + (m_struktur_kp.BobotNK.ToString()) + "%)" +
                                                                                          ")"
                                                                                        : (
                                                                                                m_struktur_kd.JenisPerhitungan ==
                                                                                                    ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                                                                ? (s_formula_kp.Trim() != "" ? "+" : "") +
                                                                                                  "IF(" +
                                                                                                        Libs.GetColHeader(id_col_all) + "# " +
                                                                                                        "= \"\", 0 ," +
                                                                                                        Libs.GetColHeader(id_col_all) + "# " +
                                                                                                  ")"
                                                                                                : ""
                                                                                          )
                                                                                    );
                                                                    if (m_struktur_kd.JenisPerhitungan ==
                                                                      ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                    {
                                                                        s_rata_rata_kp += (s_rata_rata_kp.Trim() != "" ? "," : "") +
                                                                                          Libs.GetColHeader(id_col_all) + "# ";
                                                                    }

                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetColHeader(id_col_all) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("UH", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all) + "#"
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_struktur_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all) + "#"
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PTS", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetColHeader(id_col_all) + "#"
                                                                            );
                                                                    }
                                                                    else if (
                                                                            m_kd.Nama.Trim().ToUpper().IndexOf("PAS", StringComparison.CurrentCulture) >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetColHeader(id_col_all) + "#"
                                                                                );
                                                                    }
                                                                    //end item formula kp
                                                                }
                                                                //end jika ada perbaikan
                                                            }
                                                        }

                                                        id_kp++;

                                                    }
                                                    //end load kp

                                                    //generate formula untuk kp
                                                    if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                    {
                                                        s_formula = "IF(" +
                                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") <= 0, \"\", " +
                                                                        "ROUND((" + s_formula_kp + "), " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") " +
                                                                     ")";
                                                    }
                                                    else if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                    {
                                                        string s_formula_rata_rata = "";
                                                        s_formula_rata_rata = "(" + s_formula_kp + ")/" + (jml_merge_kd - jml_pb).ToString();
                                                        //s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_kp) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_kp) + "))";
                                                        s_formula_rata_rata = "IF(COUNT(" + s_rata_rata_kp + ") = 0, 0 , AVERAGE(" + s_rata_rata_kp + "))";

                                                        s_formula = "IF(" +
                                                                        "ROUND(" + s_formula_rata_rata + ", " + Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() + ") <= 0, \"\", " +
                                                                        "ROUND(" + s_formula_rata_rata + ", " +
                                                                                Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
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
                                                    if (m_struktur_ap.JenisPerhitungan ==
                                                                      ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                    {
                                                        s_rata_rata_na += (s_rata_rata_na.Trim() != "" ? "+" : "") +
                                                                          Libs.GetColHeader(id_col_all + 1) + "# ";
                                                    }
                                                    //end tambahkan ke formula kd

                                                    //add content kolom nkd
                                                    lst_kolom_nilai_nkd.Add(new NILAI_COL
                                                    {
                                                        BluePrintFormula = "=" + s_formula,
                                                        IdKolom = id_col_all
                                                    });
                                                    //end content kolom nkd

                                                    //tambahkan nk setelah kp

                                                    js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                    (id_col_all).ToString();

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
                                                                       "\"" + Libs.GetHTMLSimpleText(m_ap.Nama) + "\"";

                                                    s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                                       "\"" + Libs.GetHTMLSimpleText(m_kd.Nama) + "\"";

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

                                        s_formula_ph_pts_pas = "";
                                        if (m_struktur_nilai.Is_PH_PTS_PAS)
                                        {
                                            if (m_ap.Nama.ToLower().IndexOf("pengetahuan") >= 0)
                                            {
                                                //tambahkan PH, PTS, PAS
                                                //tambahkan PH
                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "IF(COUNT(" + String.Join(",", lst_kp_tugas.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_tugas.ToArray()) + "))" +
                                                    ",2)";
                                                lst_kolom_nilai_PH.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPH,
                                                    BluePrintFormula = (
                                                            lst_kp_tugas.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PH + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PH);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PH" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPH, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_tugas.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_tugas.ToArray()) + "))" +
                                                        ",2)*(" + m_struktur_nilai.BobotPH.ToString() + "/100)" +
                                                    ",2)";
                                                s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                lst_kolom_nilai_PH_bobot.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPH,
                                                    BluePrintFormula = (
                                                            lst_kp_tugas.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;
                                                //end tambahkan PH

                                                //tambahkan PTS
                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "IF(COUNT(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + "))" +
                                                    ",2)";
                                                lst_kolom_nilai_PTS.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPTS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_non_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PTS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PTS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPTS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_non_terakhir.ToArray()) + "))" +
                                                        ",2)*(" + m_struktur_nilai.BobotPTS.ToString() + "/100)" +
                                                    ",2)";
                                                s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                lst_kolom_nilai_PTS_bobot.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPTS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_non_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;
                                                //end tambahkan PTS

                                                //tambahkan PAS

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "IF(COUNT(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + "))" +
                                                    ",2)";
                                                lst_kolom_nilai_PAS.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPAS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                //add ap to var arr js
                                                s_arr_js_ap += (s_arr_js_ap.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_ap.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kd to var arr js
                                                s_arr_js_kd += (s_arr_js_kd.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kd.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);

                                                //add kp to var arr js
                                                s_arr_js_kp += (s_arr_js_kp.Trim() != "" ? "," : "") +
                                                               "\"" + Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS + "\"";
                                                lst_kp.Add(Constantas.SMP.SMP_GUID_PENGETAHUAN_PAS);
                                                //end add ap, kd & kp

                                                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                                                 LEBAR_COL_DEFAULT;

                                                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                                   "\"PAS" +
                                                                        "<br />" +
                                                                        "<sup class='badge' style='background-color: mediumvioletred; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_nilai.BobotPAS, 0).ToString() + "%" +
                                                                        "</sup>" +
                                                                   "\"";

                                                js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                                (id_col_all).ToString();

                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", rowspan: 3, colspan: 1 }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                                 "{ row: 2, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                                                s_formula_item_ph_pts_pas =
                                                    "ROUND(" +
                                                        "ROUND(" +
                                                            "IF(COUNT(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + ") = 0, 0 , AVERAGE(" + String.Join(",", lst_kp_uh_terakhir.ToArray()) + "))" +
                                                        ",2)*(" + m_struktur_nilai.BobotPAS.ToString() + "/100)" +
                                                    ",2)";
                                                s_formula_ph_pts_pas += (s_formula_ph_pts_pas.Trim() != "" ? "+" : "") + s_formula_item_ph_pts_pas;
                                                lst_kolom_nilai_PAS_bobot.Add(new NILAI_COL
                                                {
                                                    IdKolom = id_col_all,
                                                    Bobot = m_struktur_nilai.BobotPAS,
                                                    BluePrintFormula = (
                                                            lst_kp_uh_terakhir.Count == 0
                                                            ? ""
                                                            : "=" + s_formula_item_ph_pts_pas
                                                        )
                                                });
                                                id_col_all++;
                                                //end tambahkan PAS

                                                jml_merge_kd++;
                                                jml_merge_ap++;
                                                //end tambahkan PH, PTS, PAS

                                                if (jml_merge_ap > 0)
                                                {
                                                    s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                    "{ row: 0, col: " + (id_col_all - jml_merge_ap - 5).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                                                }
                                            }
                                        }

                                        //tambahkan na setelah kp

                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                        (id_col_all).ToString();

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
                                                         LEBAR_COL_DEFAULT_NA;

                                        s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                                           "\"NA<br />" +
                                                           "<span style='font-size: x-small;'>" +
                                                                Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                           "</span>" +
                                                                //id_na.ToString() +
                                                                (
                                                                    m_struktur_ap.BobotRapor > 0
                                                                    ? "<br />" +
                                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                        "</sup>"
                                                                    : ""
                                                                ) +
                                                           "\"";

                                        s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                                           "\"NA" +
                                                           "<span style='font-size: x-small;'>" +
                                                                Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                           "</span>" +
                                                                //id_na.ToString() +
                                                                (
                                                                    m_struktur_ap.BobotRapor > 0
                                                                    ? "<br />" +
                                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                        "</sup>"
                                                                    : ""
                                                                ) +
                                                           "\"";

                                        s_js_arr_kolom3 += (s_js_arr_kolom3.Trim() != "" ? ", " : "") +
                                                           "\"NA" +
                                                           "<span style='font-size: x-small;'>" +
                                                                Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                           "</span>" +
                                                                //id_na.ToString() +
                                                                (
                                                                    m_struktur_ap.BobotRapor > 0
                                                                    ? "<br />" +
                                                                        "<sup class='badge' style='background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;'>" +
                                                                            Math.Round(m_struktur_ap.BobotRapor, 0).ToString() + "%" +
                                                                        "</sup>"
                                                                    : ""
                                                                ) +
                                                           "\"";

                                        js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                                        (id_col_all).ToString();
                                        //end tambahkan na setelah kp

                                        //generate formula nap
                                        //formula item kd dari nk
                                        if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                        {
                                            s_formula_kd = "IF(" +
                                                                "(" + s_formula_kd_gabung + ") <= 0, \"\", " +
                                                                "ROUND(" +
                                                                    "(" + s_formula_kd_gabung + ")" +
                                                                    ", " +
                                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
                                                                ") " +
                                                           ")";
                                        }
                                        else if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                        {
                                            string s_formula_rata_rata = "";
                                            s_formula_rata_rata = "(" + s_formula_kd_gabung + ")/" + (id_nk - 1).ToString();
                                            s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_na) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_na) + "))";

                                            s_formula_kd = "IF(" +
                                                                s_formula_rata_rata + " <= 0, \"\", " +
                                                                "ROUND(" +
                                                                    s_formula_rata_rata +
                                                                    ", " +
                                                                    Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES.ToString() +
                                                                ")" +
                                                           ")";
                                        }
                                        //end formula item kd dari nk
                                        //end generate formula

                                        //add content kolom nap
                                        if (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS)
                                        {
                                            lst_kolom_nilai_nap.Add(new NILAI_COL
                                            {
                                                BluePrintFormula = "=" + (
                                                    s_formula_ph_pts_pas.Trim() != ""
                                                    ? "ROUND(" + s_formula_ph_pts_pas + ",0)"
                                                    : "ROUND(" + s_formula_kd + ",0)"
                                                ),
                                                IdKolom = id_col_all,
                                                Bobot = m_struktur_ap.BobotRapor
                                            });
                                        }
                                        else if (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KTSP)
                                        {
                                            lst_kolom_nilai_nap.Add(new NILAI_COL
                                            {
                                                BluePrintFormula = "=" + (
                                                    s_formula_ph_pts_pas.Trim() != ""
                                                    ? s_formula_ph_pts_pas
                                                    : s_formula_kd
                                                ),
                                                IdKolom = id_col_all,
                                                Bobot = m_struktur_ap.BobotRapor
                                            });
                                        }
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

                                        if (
                                            (!m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") >= 0) ||
                                            (m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") < 0) ||
                                            (!m_struktur_nilai.Is_PH_PTS_PAS && m_ap.Nama.ToLower().IndexOf("pengetahuan") < 0)
                                        )
                                        {
                                            if (jml_merge_ap > 0)
                                            {
                                                s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                                                "{ row: 0, col: " + (id_col_all - jml_merge_ap).ToString() + ", rowspan: 1, colspan: " + (jml_merge_ap - 1).ToString() + " }";
                                            }
                                        }
                                    }
                                }
                            }
                            //end load ap

                            //tambahkan kolom nilai akhir
                            js_statistik += (js_statistik.Trim() != "" ? ", " : "") +
                                            (id_col_all).ToString();

                            id_col_nilai_rapor = id_col_all;
                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             (
                                                 (DAO_Rapor_StrukturNilai.GetKurikulumByLevel(rel_kelas, tahun_ajaran, semester) == Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS)
                                                 ? "1"
                                                 : LEBAR_COL_DEFAULT
                                             );

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

                            //tambahkan kolom absensi
                            id_col_all++;
                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "SAKIT" +
                                               "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "SAKIT" +
                                               "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "SAKIT" +
                                               "\"";
                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 0, col: " + (id_col_all).ToString() + ", rowspan: 3, colspan: 1 }";
                            id_col_all++;
                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "IZIN" +
                                               "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "IZIN" +
                                               "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "IZIN" +
                                               "\"";
                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 0, col: " + (id_col_all).ToString() + ", rowspan: 3, colspan: 1 }";
                            id_col_all++;
                            s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                             LEBAR_COL_DEFAULT;

                            s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "ALPA" +
                                               "\"";

                            s_js_arr_kolom2 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "ALPA" +
                                               "\"";

                            s_js_arr_kolom3 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                               "\"" +
                                                "ALPA" +
                                               "\"";
                            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") +
                                             "{ row: 0, col: " + (id_col_all).ToString() + ", rowspan: 3, colspan: 1 }";
                            //end tambahkan kolom absensi

                            //tambahkan capaian kedisiplinan siswa
                            id_col_all++;
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

                            s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                            "{" +
                                            "  row: 0, col: " + (id_col_all - 3) + ", rowspan: 1, " +
                                            "  colspan: 4" +
                                            "} ";
                            s_merge_cells += (s_merge_cells.Trim() != "" ? ", " : "") +
                                            "{" +
                                            "  row: 1, col: " + (id_col_all - 3) + ", rowspan: 1, " +
                                            "  colspan: 4" +
                                            "} ";
                            //end tambahkan kolom capaian kedisiplinan siswa

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

                            //load data siswa
                            //list siswa

                            //get list nilai jika ada
                            Rapor_Nilai m_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                                    tahun_ajaran, semester, rel_kelas, rel_mapel
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
                            int id_col_nilai_mulai = 4;
                            List<Siswa> lst_siswa = new List<Siswa>();
                            string s_js_arr_nilai = "";

                            var lst_mapel_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetMapelByGuruByTA_Entity(
                                    s_guru, QS.GetTahunAjaran()
                                ).FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim()); ;
                            //var lst_formasi_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetKelasFormasiByGuruByTA_Entity(
                            //        s_guru, QS.GetTahunAjaran()
                            //    ).FindAll(m0 => m0.Rel_Mapel.ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim());
                            var lst_formasi_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetKelasFormasiByGuruByTABySemester_Entity(
                                    s_guru, QS.GetTahunAjaran(), semester
                                ).FindAll(m0 => m0.Rel_Mapel.ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim());
                            //var lst_formasi_mapel_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetFormasiMapelByGuruByTA_Entity(
                            //        s_guru, QS.GetTahunAjaran()
                            //    ).FindAll(m0 => m0.Rel_Mapel.ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim());
                            var lst_formasi_mapel_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetFormasiMapelByGuruByTABySemester_Entity(
                                    s_guru, QS.GetTahunAjaran(), semester
                                ).FindAll(m0 => m0.Rel_Mapel.ToUpper().Trim() == QS.GetMapel().ToString().ToUpper().Trim());
                            foreach (var item_mapel in lst_mapel_ekskul)
                            {
                                var lst_formasi_kelas_ = lst_formasi_kelas.FindAll(
                                    m0 => m0.Rel_Mapel.ToUpper().Trim() == item_mapel.Kode.ToString().ToUpper().Trim()
                                );

                                string[] arr_kelas_ekskul = AtributPenilaian.Kelas.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                if (arr_kelas_ekskul.Length == 3)
                                {
                                    lst_formasi_kelas_ = lst_formasi_kelas_.FindAll(
                                            m0 => m0.Rel_Kelas1.ToUpper().Trim() == arr_kelas_ekskul[0].ToUpper().Trim() &&
                                                  m0.Rel_Kelas2.ToUpper().Trim() == arr_kelas_ekskul[1].ToUpper().Trim() &&
                                                  m0.Rel_Kelas3.ToUpper().Trim() == arr_kelas_ekskul[2].ToUpper().Trim()
                                        );
                                }

                                foreach (var item_formasi_kelas in lst_formasi_kelas_)
                                {
                                    string s_kelas_ekskul = "";
                                    if (item_formasi_kelas.Rel_Kelas1.Trim() != "") s_kelas_ekskul += item_formasi_kelas.Rel_Kelas1.Trim() + ";";
                                    if (item_formasi_kelas.Rel_Kelas2.Trim() != "") s_kelas_ekskul += item_formasi_kelas.Rel_Kelas2.Trim() + ";";
                                    if (item_formasi_kelas.Rel_Kelas3.Trim() != "") s_kelas_ekskul += item_formasi_kelas.Rel_Kelas3.Trim() + ";";

                                    string kode_sn = "";

                                    foreach (var item_formasi_mapel_kelas in lst_formasi_mapel_kelas.FindAll(
                                        m1 => m1.Rel_Mapel.ToUpper().Trim() == item_formasi_kelas.Rel_Mapel.ToString().ToUpper().Trim() &&
                                              m1.Rel_Kelas.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas1.ToString().ToUpper().Trim() &&
                                              m1.Rel_Kelas2.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas2.ToString().ToUpper().Trim() &&
                                              m1.Rel_Kelas3.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas3.ToString().ToUpper().Trim() &&
                                              m1.Semester == semester
                                        ).OrderBy(m0 => m0.Semester)
                                    )
                                    {
                                        kode_sn = item_formasi_mapel_kelas.KodeSN;

                                        string id_per_semester = "ID_" + Guid.NewGuid().ToString().Replace("-", "");

                                        var formasi_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetByID_Entity(
                                                item_formasi_mapel_kelas.KodeFormasiEkskul.ToString()
                                            );

                                        if (formasi_ekskul != null)
                                        {
                                            if (formasi_ekskul.TahunAjaran != null)
                                            {
                                                var lst_siswa_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskulDet.GetByHeader_Entity(formasi_ekskul.Kode.ToString());
                                                var struktur_nilai_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetByID_Entity(formasi_ekskul.Rel_Rapor_StrukturNilai);

                                                if (struktur_nilai_ekskul != null && struktur_nilai_ekskul.TahunAjaran != null)
                                                {
                                                    foreach (var item_formasi_det in lst_siswa_ekskul)
                                                    {

                                                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                                                                    formasi_ekskul.TahunAjaran,
                                                                    formasi_ekskul.Semester,
                                                                    item_formasi_det.Rel_Siswa.ToString());
                                                        if (m_siswa != null)
                                                        {
                                                            if (m_siswa.Nama != null)
                                                            {
                                                                lst_siswa.Add(m_siswa);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                css_bg = (id % 2 == 0 ? " htBG1" : " htBG2");
                                css_bg_nkd = (id % 2 == 0 ? " htBG3" : " htBG4");
                                css_bg_nap = (id % 2 == 0 ? " htBG5" : " htBG6");
                                css_bg_nilaiakhir = (id % 2 == 0 ? " htBG7" : " htBG8");
                                css_bg_readonly = (id % 2 == 0 ? " htBG9" : " htBG10");
                                css_bg_for_pengetahuan = (id % 2 == 0 ? " htBG19" : " htBG20");

                                s_js_arr_nilai = "";
                                s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                                  "'" + m_siswa.Kode.ToString() + "'";

                                string kelas_det = "";
                                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet.ToString());
                                if (m_kelas_det != null)
                                {
                                    if (m_kelas_det.Nama != null)
                                    {
                                        kelas_det = m_kelas_det.Nama.Trim();
                                    }
                                }

                                for (int i = id_col_nilai_mulai; i < id_col_all - 7; i++)
                                {
                                    string s_nilai = "";
                                    bool is_kolom_nk = false;
                                    bool is_kolom_na = false;

                                    //get formula nilai nk
                                    NILAI_COL m_nilai_col_nk = lst_kolom_nilai_nkd.FindAll(m => m.IdKolom == i).FirstOrDefault();
                                    if (m_nilai_col_nk != null)
                                    {
                                        if (m_nilai_col_nk.BluePrintFormula != null)
                                        {
                                            s_nilai = m_nilai_col_nk.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
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
                                            is_kolom_na = true;
                                        }
                                    }
                                    //end get formula nilai na

                                    string s_nilai_pb = "";
                                    string s_nilai_asli = "";
                                    bool is_kolom_pb = false;
                                    //---get nilainya disini
                                    if (lst_nilai_siswa_det != null)
                                    {
                                        Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                     m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == (i <= lst_ap.Count ? lst_ap[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == (i <= lst_kd.Count ? lst_kd[i].Trim().ToUpper() : "") &&
                                                     m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == (i <= lst_kp.Count ? lst_kp[i].Trim().ToUpper() : "")
                                            ).FirstOrDefault();
                                        if (m_nilai_det != null)
                                        {
                                            if (m_nilai_det.Nilai != null)
                                            {
                                                s_nilai = m_nilai_det.Nilai;
                                                s_nilai_pb = m_nilai_det.PB;
                                            }
                                        }
                                    }

                                    s_nilai_asli = s_nilai;

                                    //jika kolom pb
                                    NILAI_COL m_nilai_col_pb = lst_kolom_pb.FindAll(m => m.IdKolom == i).FirstOrDefault();
                                    if (m_nilai_col_pb != null)
                                    {
                                        if (m_nilai_col_pb.BluePrintFormula != null)
                                        {
                                            s_nilai = m_nilai_col_pb.BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                        }

                                        if (s_nilai_pb.Trim() != "")
                                        {
                                            s_nilai = s_nilai_pb;
                                        }

                                        is_kolom_pb = true;
                                    }

                                    //---end get nilai

                                    if (is_kolom_nk) //styling kolom nk
                                    {
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nkd + " htFontBlack " +
                                                         "htBorderRightNKD" + "\", readOnly: true }";
                                    }
                                    else if (
                                        lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                        lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0 ||

                                        lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                        lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0 ||

                                        lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                        lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0
                                    )
                                    {
                                        //nilainya
                                        if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PH_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PTS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        if (lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            if (lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                        FirstOrDefault().BluePrintFormula.Trim() != "")
                                            {
                                                s_nilai = lst_kolom_nilai_PAS_bobot.FindAll(m0 => m0.IdKolom == i).
                                                            FirstOrDefault().BluePrintFormula.Replace("#", (id + id_jml_fixed_row).ToString());
                                            }
                                        }

                                        //style nya
                                        if (lst_kolom_nilai_PH.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PTS.FindAll(m0 => m0.IdKolom == i).Count > 0 ||
                                            lst_kolom_nilai_PAS.FindAll(m0 => m0.IdKolom == i).Count > 0)
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                                             ", className: \"htCenter htMiddle " + css_bg_for_pengetahuan +
                                                             "\", readOnly: true }";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() +
                                                             ", className: \"htCenter htMiddle " + css_bg_for_pengetahuan + " htFontBlack " +
                                                             "\", readOnly: true }";
                                        }
                                    }
                                    else if (is_kolom_na) //styling kolom na
                                    {
                                        s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                         "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg_nap + " htFontBlack " +
                                                         "htBorderRightNAP" + "\", readOnly: true }";
                                    }
                                    else //styling kolom nilai
                                    {
                                        if (s_nilai_pb != "" && is_kolom_pb)
                                        {
                                            //kolom nilai aslinya dikunci
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (i - 1).ToString() + ", className: \"htCenter htMiddle " + css_bg_readonly + " htFontBlack \"," + "readOnly: true" + " }";
                                            //ini kolom nilai PB nya
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\"," + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                                            s_arr_js_pb_asli_locked_cells += (s_arr_js_pb_asli_locked_cells.Trim() != "" ? "," : "") +
                                                                             "\"" + ((id + id_jml_fixed_row) - 1).ToString().ToString() + "|" + (i - 1).ToString() + "\"";
                                        }
                                        else
                                        {
                                            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                             "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + i.ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\"," + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";
                                        }
                                    }

                                    //set nilai
                                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_nilai + "'";
                                    //end set nilai
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
                                    s_rata_rata_rapor = "";
                                    foreach (var item in lst_kolom_nilai_nap)
                                    {
                                        s_formula_rapor += (s_formula_rapor.Trim() != "" ? "+" : "") +
                                                           "IF(" +
                                                               Libs.GetColHeader(item.IdKolom + 1) + "#" + " = \"\", 0, " +
                                                               Libs.GetColHeader(item.IdKolom + 1) + "#" +
                                                           ")";
                                        s_rata_rata_rapor += (s_rata_rata_rapor.Trim() != "" ? "+" : "") +
                                                             Libs.GetColHeader(item.IdKolom + 1) + "#";

                                    }
                                    if (s_formula_rapor.Trim() != "")
                                    {
                                        string s_formula_rata_rata = "";
                                        s_formula_rata_rata = "(" +
                                                                s_formula_rapor +
                                                              ")/" + lst_kolom_nilai_nap.Count.ToString();
                                        s_formula_rata_rata = "IF(COUNT(" + GetRangeCell(s_rata_rata_rapor) + ") = 0, 0 , AVERAGE(" + GetRangeCell(s_rata_rata_rapor) + "))";

                                        s_formula_rapor = s_formula_rata_rata;
                                    }
                                }
                                s_formula_rapor = "'" +
                                                    (s_formula_rapor.Trim() != "" ? "=" : "") +
                                                    "ROUND(" +
                                                        s_formula_rapor.Replace("#", (id + id_jml_fixed_row).ToString()) +
                                                        ", " +
                                                        Constantas.PEMBULATAN_DESIMAL_NILAI_SMP.ToString() +
                                                    ")" +
                                                  "'";

                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                  s_formula_rapor;

                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 3).ToString() + ", className: \"htCenter htMiddle " + css_bg_nilaiakhir + " htFontBlack htBorderRightFCL " + "\", readOnly: true }";
                                //end get nilai akhir by formula

                                string s_sakit = "";
                                string s_izin = "";
                                string s_alpa = "";
                                if (lst_nilai_siswa != null)
                                {
                                    if (lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).Count == 1)
                                    {
                                        var m_nilai_siswa = lst_nilai_siswa.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                                        s_sakit = m_nilai_siswa.Sakit.Replace("\"", "").Replace("'", "");
                                        s_izin = m_nilai_siswa.Izin.Replace("\"", "").Replace("'", "");
                                        s_alpa = m_nilai_siswa.Alpa.Replace("\"", "").Replace("'", "");
                                    }
                                }
                                //kolom absensi
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_sakit + "'"; //sakit
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 6).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_izin + "'"; //izin
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 5).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                      "'" + s_alpa + "'"; //alpa
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 4).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                //end kolom absensi

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
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 3).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                  "'" + s_lts_ck_ketepatan_waktu + "'"; //lts ck ketepatan waktu
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 2).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                  "'" + s_lts_ck_penggunaan_seragam + "'"; //lts ck kehadiran
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all - 1).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? ", " : "") +
                                                  "'" + s_lts_ck_penggunaan_kamera + "'"; //lts ck ketepatan waktu
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + (id_col_all).ToString() + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack " + "\", readOnly: false }";
                                //end nilai capaian kedisiplinan

                                s_content += (s_content.Trim() != "" ? ", " : "") +
                                             "[" +
                                                "\"" + id.ToString() + "\", " +
                                                "\"" + m_siswa.NISSekolah + "\", " +
                                                "\"" + Libs.GetPersingkatNama(m_siswa.Nama.ToUpper(), 3) + "<label style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</label>\", " +
                                                "\"" + m_siswa.JenisKelamin.Substring(0, 1).ToUpper() + "\" " +
                                                (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                                             "]";

                                //kolom style untuk fixed col header
                                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 0, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 1, className: \"htCenter htMiddle htFontBlack" + css_bg + "\", readOnly: true }," +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 2, className: \"htLeft htMiddle htFontBold htFontBlack" + css_bg + "\", readOnly: true, renderer: \"html\" }," +
                                                 "{ row: " + (id + id_jml_fixed_row - 1).ToString() + ", col: 3, className: \"htCenter htMiddle htBorderRightFCL htFontBlack" + css_bg + "\", readOnly: true }";

                                id++;
                            }
                            //end load data siswa

                            s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
                            s_arr_js_ap = "[" + s_arr_js_ap + "]";
                            s_arr_js_kd = "[" + s_arr_js_kd + "]";
                            s_arr_js_kp = "[" + s_arr_js_kp + "]";
                            s_arr_js_pb = "[" + s_arr_js_pb + "]";
                            s_arr_js_pb_asli_locked_cells = "[" + s_arr_js_pb_asli_locked_cells + "]";

                            s_content = (s_content.Trim() != "" ? "," : "") +
                                        s_content;

                            string s_data = "var data_nilai = " +
                                            "[" +
                                                s_kolom +
                                                s_content +
                                            "];";

                            string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMP.NILAI_SISWA.ROUTE);
                            s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMP.NILAI_SISWA.DO_SAVE.FILE + "/Do");

                            string s_cols_nap = "";
                            string s_cols_nkd = "";
                            string s_cols_pb = "";

                            foreach (var item in lst_kolom_nilai_nap)
                            {
                                s_cols_nap += (s_cols_nap.Trim() != "" ? " || " : "") +
                                              "col === " + item.IdKolom;
                            }
                            foreach (var item in lst_kolom_nilai_nkd)
                            {
                                s_cols_nkd += (s_cols_nkd.Trim() != "" ? " || " : "") +
                                              "col === " + item.IdKolom;
                            }
                            foreach (var item in lst_kolom_pb)
                            {
                                s_cols_pb += (s_cols_pb.Trim() != "" ? " || " : "") +
                                              "col === " + item.IdKolom;
                            }

                            string script = s_data +
                                            "var arr_s = " + s_arr_js_siswa + ";" +
                                            "var arr_ap = " + s_arr_js_ap + ";" +
                                            "var arr_kd = " + s_arr_js_kd + ";" +
                                            "var arr_kp = " + s_arr_js_kp + ";" +
                                            "var arr_pb = " + s_arr_js_pb + ";" +
                                            "var arr_pb_asli_locked_cells = " + s_arr_js_pb_asli_locked_cells + ";" +
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
                                                        "if(arr_pb_asli_locked_cells.indexOf(row + '|' + col) >= 0){" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG9 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG10 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} else { " +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} " +
                                                    "} " +
                                                    "else if(parseInt(col) > 3 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                        "if(arr_pb_asli_locked_cells.indexOf(row + '|' + col) >= 0){" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG9 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG10 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} else { " +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} " +
                                                    "} " +

                                                    ( //untuk nilai na
                                                        s_cols_nap.Trim() != ""
                                                        ? "if(" + s_cols_nap + "){" +
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

                                                    ( //untuk nilai pb
                                                        s_cols_pb.Trim() != ""
                                                        ? "if(" + s_cols_pb + "){" +
                                                                "if(data_nilai[row][col].toString().substring(0, 1) === '='){" +
                                                                    "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                    "else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                "} else {" +
                                                                    "if(parseInt(col) > 3 && parseFloat(data_nilai[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                    "else {" +
                                                                        "if((row + 1) % 2 !== 0){" +
                                                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} else {" +
                                                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontBlack' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                                        "} " +
                                                                    "} " +
                                                                "}" +
                                                          "}"
                                                        : ""
                                                    ) +

                                                    "if(col === " + id_col_nilai_rapor.ToString() + "){" + //untuk nilai rapor
                                                        "if(parseInt(col) > 3 && parseFloat(GetNilaiFromFormula(row, col)) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG7 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG8 htCenter htMiddle htFontRed htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} else {" +
                                                            "if((row + 1) % 2 !== 0){" +
                                                                "cellProperties.className = 'htBG7 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} else {" +
                                                                "cellProperties.className = 'htBG8 htCenter htMiddle htFontBlack htFontBold' + (" + txtKTSPColKD.ClientID + ".value.indexOf(col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                                            "} " +
                                                        "} " +
                                                    "} " +
                                                    "return cellProperties;" +
                                                  "}" +
                                                "}," +
                                                "afterChange: function(changes, source) {" +
                                                    "if (source === 'loadData') return;" +
                                                    "var i_count = 0;" +
                                                    "var i_index = 0;" +
                                                    "var i_inc = 533;" +
                                                    "$.each(changes, function(index, element) {" +
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
                                                            "var kdt = '" + rel_kelas.ToString() + "';" +
                                                            "var k = '" + rel_kelas.ToString() + "';" +
                                                            "var mp = '" + rel_mapel.ToString() + "';" +
                                                            "var s = arr_s[row];" +
                                                            "var ap = arr_ap[col];" +
                                                            "var kd = arr_kd[col];" +
                                                            "var kp = arr_kp[col];" +
                                                            "var n = data_nilai[row][col];" +
                                                            "var id_col_pb = (arr_pb.indexOf(col) >= 0 ? col : -1);" +

                                                            "var cellId = hot.plugin.utils.translateCellCoords({row: row, col: " + id_col_nilai_rapor.ToString() + "});" +
                                                            "var formula = hot.getDataAtCell(row, " + id_col_nilai_rapor.ToString() + ");" +
                                                            "formula = formula.substr(1).toUpperCase();" +
                                                            "var newValue = hot.plugin.parse(formula, {row: row, col: " + id_col_nilai_rapor.ToString() + ", id: cellId});" +
                                                            "var nr = (newValue.result);" +

                                                            "var sakit = data_nilai[row][" + (id_col_nilai_rapor + 1).ToString() + "];" +
                                                            "var izin = data_nilai[row][" + (id_col_nilai_rapor + 2).ToString() + "];" +
                                                            "var alpa = data_nilai[row][" + (id_col_nilai_rapor + 3).ToString() + "];" +

                                                            "var lts_ck_hd = (data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 4).ToString() + "]);" +
                                                            "var lts_ck_kw = (data_nilai[row][" + (id_col_nilai_rapor + 5).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 5).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 5).ToString() + "]);" +
                                                            "var lts_ck_ps = (data_nilai[row][" + (id_col_nilai_rapor + 6).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 6).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 6).ToString() + "]);" +
                                                            "var lts_ck_pk = (data_nilai[row][" + (id_col_nilai_rapor + 7).ToString() + "] === undefined || data_nilai[row][" + (id_col_nilai_rapor + 7).ToString() + "] === null ? '' : data_nilai[row][" + (id_col_nilai_rapor + 7).ToString() + "]);" +
                                                        
                                                            "var s_url = '" + s_url_save + "' + " +
                                                                                        "'?' + " +
                                                                                        "'t=' + t + '&sm=' + sm + '&kdt=' + kdt + '&' + " +
                                                                                        "'s=' + s + '&n=' + n + '&ap=' + ap + '&kd=' + kd + '&kp=' + kp + '&' + " +
                                                                                        "'mp=' + mp + '&k=' + k + '&' + " +
                                                                                        "'nr=' + nr + '&' + " +
                                                                                        "'lts_ck_hd=' + lts_ck_hd + '&' + " +
                                                                                        "'lts_ck_kw=' + lts_ck_kw + '&' + " +
                                                                                        "'lts_ck_ps=' + lts_ck_ps + '&' + " +
                                                                                        "'lts_ck_pk=' + lts_ck_pk + '&' + " +
                                                                                        "'sakit=' + sakit + '&' + " +
                                                                                        "'izin=' + izin + '&' + " +
                                                                                        "'alpa=' + alpa + " +
                                                                                        "(id_col_pb >= 0 ? '&pb=oke' : '') + " +
                                                                                        "'&ssid=" + Libs.Enkrip(s_guru) + "'" +
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
                                                                "}); " +

                                                            "}, i_count " +
                                                        "); " +

                                                    "});" +
                                                    "this.render();" +
                                                "}, " +
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


                            if (js_statistik.Trim() != "")
                            {
                                js_statistik = "var arr_col = [" + js_statistik + "], ";
                                js_statistik += "id_fixed_row = " + (id_jml_fixed_row).ToString() + ", ";
                                js_statistik += "id_fixed_col = " + id_col_nilai_mulai.ToString() + ";";
                            }

                            ltrJSStatistik.Text = js_statistik;
                            ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";

                        }
                        //end if struktur tahun ajaran not null
                    }
                }
            }
        }

        protected void lnkShowStatistics_Click(object sender, EventArgs e)
        {
            ShowStatistik();
            txtKeyAction.Value = JenisAction.DoShowStatistik.ToString();
        }

        protected void ShowStatistik()
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
                List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                        AtributPenilaian.TahunAjaran, AtributPenilaian.Semester, AtributPenilaian.Kelas, AtributPenilaian.Mapel
                    );

                if (lst_stuktur_nilai.Count == 1)
                {
                    AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai struktur_nilai = lst_stuktur_nilai.FirstOrDefault();

                    //load kurtilas ap
                    List<Rapor_StrukturNilai_AP> lst_aspek_penilaian =
                        DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(struktur_nilai.Kode.ToString());

                    bool ada_rowspan_ap = false;
                    bool ada_rowspan_kd = false;

                    int jumlah_rowspan_ap = 0;
                    int jumlah_rowspan_kd = 0;

                    string bg1 = "#FFFFFF";
                    string bg2 = "#fbfbfb";

                    int id_all = 0;
                    int id_fixed_col = 3;
                    foreach (Rapor_StrukturNilai_AP m_sn_ap in lst_aspek_penilaian)
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
                                        List<Rapor_StrukturNilai_KD> lst_kompetensi_dasar =
                                            DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(m_sn_ap.Kode.ToString());

                                        foreach (Rapor_StrukturNilai_KD m_sn_kd in lst_kompetensi_dasar)
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
                                                            List<Rapor_StrukturNilai_KP> lst_komponen_penilaian =
                                                                DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(m_sn_kd.Kode.ToString());

                                                            foreach (Rapor_StrukturNilai_KP m_sn_kp in lst_komponen_penilaian)
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
                                                                            "" +
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

                    //nilai rapor 
                    string bgrapor1 = "#B3E2ED";
                    
                    html += "<tr>";

                    html += "<td colspan=\"3\" style=\"font-size: smaller; color: black; padding: 5px; vertical-align: middle; background-color: #efefef; font-weight: bold; border-style: solid; border-width: 0.5px; border-color: #CCCCCC;\">" +
                                "NILAI RAPOR" +
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
                }
            }

            html += "</table>" +
                    "</div>";

            ltrStatistikPenilaian.Text = html;
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

        protected List<Siswa> GetListSiswa(string rel_sekolah, string rel_kelas_det)
        {
            return DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        rel_sekolah,
                        rel_kelas_det,
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
        }

        protected void ShowListPilihSiswa()
        {
            bool ada_pilihan = false;
            string rel_kelas = "";
            List<string> lst_siswa_dipilih = new List<string>();
            lst_siswa_dipilih.Clear();
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));

            var m = DAO_SiswaMapelPilihan.GetByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()).FirstOrDefault();
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    lst_siswa_dipilih = DAO_SiswaMapelPilihan_Det.GetByHeader_Entity(m.Kode.ToString()).Select(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim()).Distinct().ToList();
                    ada_pilihan = true;
                }
            }

            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                }
            }

            ltrListSiswaMapelPilihan.Text = "<div style=\"padding: 15px; padding-bottom: 25px;\">" +
                                                "<label style=\"margin: 0 auto; display: table; color: grey;\">..:: Data Kosong ::..</label>" +
                                            "</div>";


            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        Libs.GetQueryString("kd"),
                                        QS.GetTahunAjaran(),
                                        QS.GetSemester()
                                    );

                    if (lst_siswa.Count > 0)
                    {
                        string html = "";
                        int id = 0;
                        foreach (var m_siswa in lst_siswa)
                        {
                            string s_checked = "";
                            if (lst_siswa_dipilih.FindAll(m0 => m0 == m_siswa.Kode.ToString().ToUpper().Trim()).Count > 0)
                            {
                                s_checked = "checked=\"checked\" ";
                            }
                            if(!ada_pilihan) s_checked = "checked=\"checked\" "; //defaultnya ceklist semua

                            html += "<tr class=\"" + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                        "<td style=\"color: #bfbfbf; vertical-align: middle; padding: 10px; padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-align: center; width: 30px;\">" +
                                            (id + 1).ToString() +
                                        "</td>" +
                                        "<td style=\"vertical-align: middle; padding: 10px; padding-top: 5px; padding-bottom: 5px; text-align: justify;\">" +
                                            "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto; padding-left: 10px;\">" +
                                                "<label for=\"chk_siswa_" + m_siswa.Kode.ToString().Replace("-", "_") + "\">" +
                                                    "" +
                                                    "<input value=\"" + m_siswa.Kode.ToString() + "\" " +
                                                           " class=\"access-hide\" " +
                                                           " id=\"chk_siswa_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                           " name=\"chk_siswa[]\" " +
                                                           s_checked +
                                                           " type=\"checkbox\">" +
                                                    "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span>" +
                                                    "<span class=\"checkbox-circle-icon icon\">done</span>" +
                                                    m_siswa.Nama.ToUpper() +
                                                "</label>" +
                                            "</div>" +
                                        "</td>" +
                                    "</tr>";
                            id++;
                        }
                        ltrListSiswaMapelPilihan.Text = "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                            "<div style=\"font-weight: bold; color: grey; margin: 0px; padding: 0px; padding-left: 20px; padding-right: 20px;\">" +
                                                                "Pilih siswa yang mengikuti mata pelajaran : " +
                                                            "</div>" +
                                                            "<div style=\"font-weight: bold; color: grey; margin: 0px; padding: 0px; padding-left: 20px; padding-right: 20px; margin-top: 10px;\">" +
                                                                "<label onclick=\"SelectPilihanSiswa(true);\" " +
                                                                    "style=\"padding: 0px; padding-left: 15px; padding-right: 15px; cursor: pointer; color: #60c0c4; border-width: 1px; border-style: solid; border-color: #60c0c4; font-weight: bold; border-radius: 10px; font-size: x-small\">Ceklist Semua</label>" +
                                                                "&nbsp;" +
                                                                "<label onclick=\"SelectPilihanSiswa(false);\" " +
                                                                    "style=\"padding: 0px; padding-left: 15px; padding-right: 15px; cursor: pointer; color: palevioletred; border-width: 1px; border-style: solid; border-color: palevioletred; font-weight: bold; border-radius: 10px; font-size: x-small\">Hapus Ceklist</label>" +
                                                            "</div>" +
                                                            "<table class=\"table\" style=\"margin: 0px; margin-top: 10px;\">" +
                                                                html +
                                                            "</table>" +
                                                        "</div>";
                    }
                }
            }
        }

        protected void lnkPilihSiswa_Click(object sender, EventArgs e)
        {
            ShowListPilihSiswa();
            txtKeyAction.Value = JenisAction.DoShowPilihSiswa.ToString();
        }

        protected void lnkOKPilihSiswa_Click(object sender, EventArgs e)
        {
            if (txtParseSiswaPilihan.Value.Trim() != "")
            {
                bool b_ada_header = false;
                Guid kode = Guid.NewGuid();
                var m = DAO_SiswaMapelPilihan.GetByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()).FirstOrDefault();
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        DAO_SiswaMapelPilihan_Det.DeleteByHeader(m.Kode.ToString());
                        kode = m.Kode;
                        b_ada_header = true;
                    }
                }
                if (!b_ada_header)
                {
                    DAO_SiswaMapelPilihan.Insert(new SiswaMapelPilihan {
                        Kode = kode,
                        TahunAjaran = QS.GetTahunAjaran(),
                        Semester = QS.GetSemester(),
                        Rel_Mapel = QS.GetMapel(),
                        Rel_KelasDet = QS.GetKelas()
                    });
                }
                string[] arr_siswa = txtParseSiswaPilihan.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string rel_siswa in arr_siswa)
                {
                    if (rel_siswa.Trim() != "")
                    {
                        DAO_SiswaMapelPilihan_Det.Insert(new SiswaMapelPilihan_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_SiswaMapelPilihan = kode,
                            Rel_Siswa = rel_siswa
                        });
                    }
                }

                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }
    }
}