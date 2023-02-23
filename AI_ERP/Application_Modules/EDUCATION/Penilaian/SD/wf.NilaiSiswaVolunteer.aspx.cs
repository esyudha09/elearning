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
    public partial class wf_NilaiSiswaVolunteer : System.Web.UI.Page
    {
        private const string LEBAR_COL_DEFAULT = "35";

        public enum JenisAction
        {
            ShowPilihanSemester,
            ShowPengaturanLTS,
            DoUpdate
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

            public static string GetMapel()
            {
                string m = Libs.GetQueryString("m");
                return m;
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                return s;
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
                if (QS.GetSemester().Trim() != "")
                {
                    div_button_settings.Visible = true;
                    LoadData();
                }
                else
                {
                    RenderCenterMenu();
                }

                List<string> lst_kelas = new List<string>();
                string[] arr_kelas = QS.GetLevel().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string kelas in arr_kelas)
                {
                    lst_kelas.Add(kelas);
                }

                _UI.InitModalListNilai(
                    this.Page,
                    ltrListNilaiAkademik, ltrListNilaiEkskul, ltrListNilaiSikap, ltrListNilaiVolunteer, ltrListNilaiRapor,
                    QS.GetTahunAjaran(), QS.GetMapel(), QS.GetKelas(), QS.GetGuru()
                );
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
            ltrCenter.Text = "<div data-toggle=\"modal\" href=\"#ui_modal_pilih_semester\" style=\"margin: 0 auto; display: table; cursor: pointer; color: grey; margin-top: 50px;\">" +
                                "<i class=\"fa fa-folder-open\" style=\"color: #b0b0b0; font-size: 40pt;\"></i>" +
                             "</div>" +
                             "<div data-toggle=\"modal\" href=\"#ui_modal_pilih_semester\" style=\"margin: 0 auto; display: table; cursor: pointer; color: grey;\">" +
                                "<span style=\"font-weight: bold;\">Buka Data Nilai</span>" +
                             "</div>" +
                             "<div data-toggle=\"modal\" href=\"#ui_modal_pilih_semester\" style=\"margin: 0 auto; display: table; cursor: pointer; color: grey;\">" +
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
            string rel_kelas = QS.GetLevel();
            string rel_kelas_det = QS.GetKelas();
            
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    rel_kelas = m_kelas_det.Rel_Kelas.ToString();
                }
            }

            //status bar
            ltrStatusBar.Text = "";
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                         "&nbsp;<span style=\"font-weight: bold;\">Volunteer</span>&nbsp;" +
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
            List<string> lst_kelas = new List<string>();
            lst_kelas.Clear();
            string[] arr_kelas = rel_kelas.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string kelas in arr_kelas)
            {
                lst_kelas.Add(kelas);
            }
            Rapor_Volunteer_Settings m_volunteer_settings = DAO_Rapor_Volunteer_Settings.GetByTABySM_Entity(
                    tahun_ajaran, semester
                ).FirstOrDefault();
            if (m_volunteer_settings != null)
            {
                if (m_volunteer_settings.TahunAjaran != null)
                {                    
                    bool is_readonly = _UI.IsReadonlyNilaiVolunteer(
                                        tahun_ajaran, semester,
                                        Libs.LOGGED_USER_M.NoInduk, QS.GetKelas()
                                    );
                    is_readonly = false;
                    if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()) is_readonly = true;

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

                    LoadFormat(semester, m_volunteer_settings, lst_kelas, is_readonly);
                }
            }

            mvMain.ActiveViewIndex = 0;
        }

        protected void LoadFormat(string semester, Rapor_Volunteer_Settings m_volunteer_settings, List<string> lst_kelas, bool is_readonly)
        {
            string tahun_ajaran = QS.GetTahunAjaran();
            string rel_kelas = QS.GetLevel();
            string rel_kelas_det = QS.GetKelas();
            
            string s_kolom = "";
            string s_content = "";
            string s_kolom_width = "";

            string s_merge_cells = "";
            string s_kolom_style = "";

            string s_js_arr_kolom1 = "";
            string s_js_arr_kolom2 = "";
            
            string css_bg = "#fff";
            string css_bg_nkd = "#fff";
            string css_bg_nap = "#fff";
            string css_bg_nilaiakhir = "#fff";

            int id_jml_fixed_row = 2;
            int id_col_mulai_content = 3;
            int id_col_all = id_col_mulai_content;
            int id_col_nilai_rapor = 0;

            string s_arr_js_volunteer = "";
            string s_arr_js_siswa = "";

            //init js arr siswa & arr volunteer
            for (int i = 0; i < id_col_mulai_content; i++)
            {
                s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                  "''";
            }
            for (int i = 0; i < id_jml_fixed_row; i++)
            {
                s_arr_js_volunteer += (s_arr_js_volunteer.Trim() != "" ? "," : "") +
                                      "''";
            }
            //end init js arr siswa & arr volunteer

            txtKKM.Value = Math.Round(decimal.Parse("0"), Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

            //load data siswa
            List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        QS.GetUnit(),
                        rel_kelas_det,
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
            int nomor_absen = 1;
            foreach (var siswa in lst_siswa)
            {
                s_kolom_width += (s_kolom_width.Trim() != "" ? ", " : "") +
                                 LEBAR_COL_DEFAULT;

                s_js_arr_kolom1 += (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                                   "\"" + Libs.GetHTMLSimpleText("Nomor Absen", true) + "\"";

                s_js_arr_kolom2 += (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                                   "\"" + nomor_absen.ToString() + "\"";

                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                 "{ row: 0, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";
                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                 "{ row: 1, col: " + id_col_all.ToString() + ", className: \"htCenter htMiddle htFontBold htFontBlack\", readOnly: true, renderer: \"html\" }";

                s_arr_js_siswa += (s_arr_js_siswa.Trim() != "" ? "," : "") +
                                  "'" + siswa.Kode.ToString() + "'";
                nomor_absen++;
                id_col_all++;
            }
            s_merge_cells += (s_merge_cells.Trim() != "" ? "," : "") + //merge cell nomor absen
                             "{ row: 0, col: " + id_col_mulai_content.ToString() + ", rowspan: 1, colspan: " + lst_siswa.Count.ToString() + " }";
            //end load data siswa

            //load settings mapel volunteer
            List<Rapor_Volunteer_Settings_Det> lst_volunteer_settings_det =
                        DAO_Rapor_Volunteer_Settings_Det.GetByHeader_Entity(
                            m_volunteer_settings.Kode.ToString()
                        );
            int id = 1;
            string s_js_arr_nilai = "";
            foreach (var item_settings in lst_volunteer_settings_det)
            {
                css_bg = (id % 2 != 0 ? " htBG1" : " htBG2");
                css_bg_nkd = (id % 2 != 0 ? " htBG3" : " htBG4");
                css_bg_nap = (id % 2 != 0 ? " htBG5" : " htBG6");
                css_bg_nilaiakhir = (id % 2 != 0 ? " htBG7" : " htBG8");

                string mapel_volunteer = "";
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(item_settings.Rel_Mapel);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        mapel_volunteer = m_mapel.Nama;
                    }
                }

                s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 0, className: \"htCenter htMiddle " + css_bg + " htFontBlack" + "\", readOnly: true }," +
                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 1, className: \"htLeft htFontBold htMiddle " + css_bg + " htFontBlack\",  readOnly: true }," +
                                 "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: 2, className: \"htCenter htMiddle " + css_bg + " htFontBlack htBorderRightFCL" + "\", readOnly: true }";

                //nilainya disini
                s_js_arr_nilai = "";
                int id_col = id_col_mulai_content;
                foreach (var siswa in lst_siswa)
                {
                    string s_nilai = "";
                    Rapor_VolunteerSiswa m_rapor_volunteer_siswa = DAO_Rapor_VolunteerSiswa.GetAllByTABySMByKelasDetByVSetBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelas_det, item_settings.Kode.ToString(), siswa.Kode.ToString()
                        ).FirstOrDefault();
                    if (m_rapor_volunteer_siswa != null)
                    {
                        if (m_rapor_volunteer_siswa.Nilai != null)
                        {
                            s_nilai = m_rapor_volunteer_siswa.Nilai;
                        }
                    }

                    s_js_arr_nilai += (s_js_arr_nilai.Trim() != "" ? "," : "") +
                                      "'" + s_nilai + "'";

                    s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                                    "{ row: " + ((id + id_jml_fixed_row) - 1).ToString() + ", col: " + id_col + ", className: \"htCenter htMiddle " + css_bg + " htFontBlack" + "\", " + (is_readonly ? "readOnly: true" : "readOnly: false") + " }";

                    id_col++;
                }
                s_arr_js_volunteer += (s_arr_js_volunteer.Trim() != "" ? "," : "") +
                                      "'" + item_settings.Kode.ToString() + "'";
                //end nilainya disini

                s_content += (s_content.Trim() != "" ? ", " : "") +
                            "[" +
                                "\"" + id.ToString() + "\", " +
                                "\"" + mapel_volunteer + "\", " +
                                "\"" + item_settings.Durasi.ToString() + "\" " +
                                (s_js_arr_nilai.Trim() != "" ? ", " : "") + s_js_arr_nilai +
                            "]";
                id++;
            }
            //end load settings mapel volunteer

            s_merge_cells = (s_merge_cells.Trim() != "" ? ", " : "") +
                            s_merge_cells;

            s_kolom_width = (s_kolom_width.Trim() != "" ? ", " : "") +
                             s_kolom_width;

            s_js_arr_kolom1 = (s_js_arr_kolom1.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom1;

            s_js_arr_kolom2 = (s_js_arr_kolom2.Trim() != "" ? ", " : "") +
                               s_js_arr_kolom2;

            s_kolom = "[" +
                            "\"#\", " +
                            "\"KEGIATAN\", " +
                            "\"DURASI (MENIT)\" " +
                            s_js_arr_kolom1 +
                      "], " +
                      "[" +
                            "\"#\", " +
                            "\"KEGIATAN\", " +
                            "\"DURASI (MENIT)\" " +
                            s_js_arr_kolom2 +
                      "]";

            s_kolom_style += (s_kolom_style.Trim() != "" ? ", " : "") +
                             "{ row: 0, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true }," +
                             "{ row: 1, col: 0, className: \"htCenter htMiddle htFontBlack\", readOnly: true }," +
                             "{ row: 0, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true }," +
                             "{ row: 1, col: 1, className: \"htCenter htMiddle htFontBlack\", readOnly: true }," +
                             "{ row: 0, col: 2, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }," +
                             "{ row: 1, col: 2, className: \"htCenter htMiddle htBorderRightFCL htFontBlack\", readOnly: true }";

            s_arr_js_volunteer = "[" + s_arr_js_volunteer + "]";
            s_arr_js_siswa = "[" + s_arr_js_siswa + "]";
            s_content = (s_content.Trim() != "" ? "," : "") +
                        s_content;

            string s_data = "var data_nilai = " +
                            "[" +
                                s_kolom +
                                s_content +
                            "];";

            string s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SD.NILAI_SISWA.ROUTE);
            s_url_save = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SD.NILAI_SISWA.DO_SAVE.FILE + "/DoSaveVolunteer");

            string script = s_data +
                            "var arr_s = " + s_arr_js_siswa + ";" +
                            "var arr_vset = " + s_arr_js_volunteer + ";" +
                            "var div_nilai = document.getElementById('div_nilai'), hot; " +
                            "var container = $('#div_nilai');  " +
                            "hot = new Handsontable(div_nilai, { " +
                                "data: data_nilai, " +
                                "colWidths: [35, 300, 80 " + s_kolom_width + "], " +
                                "rowHeaders: true, " +
                                "colHeaders: true, " +
                                "fixedColumnsLeft: 3, " +
                                "fixedRowsTop: 2, " +
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
                                    "if(parseInt(col) > 2 && parseFloat(this.instance.getData()[row][col]) < parseFloat(" + txtKKM.ClientID + ".value)){" +
                                        "if((row + 1) % 2 !== 0){" +
                                            "cellProperties.className = 'htBG1 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                        "} else {" +
                                            "cellProperties.className = 'htBG2 htCenter htMiddle htFontRed' + (" + txtKTSPColKD.ClientID + ".value.indexOf(';' + col.toString() + ';') >= 0 ? ' htBorderRightNKD ' : '');" +
                                        "} " +
                                    "} " +
                                    "else if(parseInt(col) > 2 && parseFloat(this.instance.getData()[row][col]) >= parseFloat(" + txtKKM.ClientID + ".value)){" +
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
                                        "var k = '" + QS.GetKelas().ToString() + "';" +
                                        "var s = arr_s[col];" +
                                        "var vset = arr_vset[row];" +
                                        "var n = data_nilai[row][col];" +

                                        "var s_url = '" + s_url_save + "' + " +
                                                    "'?' + " +
                                                    "'j=' + '' + '&' + " +
                                                    "'t=' + t + '&sm=' + sm + '&k=' + k + '&' + " +
                                                    "'s=' + s + '&n=' + n + '&' + " +
                                                    "'vset=' + vset + " +
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
                                "mergeCells: [ " +
                                    "{ row: 0, col: 0, rowspan: 2, colspan: 1 }, " +
                                    "{ row: 0, col: 1, rowspan: 2, colspan: 1 }, " +
                                    "{ row: 0, col: 2, rowspan: 2, colspan: 1 } " +
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
                            "hot.selectCell(" + id_jml_fixed_row.ToString() + "," + id_col_mulai_content.ToString() + "); ";

            ltrHOT.Text = "<script type=\"text/javascript\">" + script + "</script>";
        }

        protected void lnkPilihKelas_Click(object sender, EventArgs e)
        {

        }

        protected void lnkShowStatistics_Click(object sender, EventArgs e)
        {

        }

        protected void lnkNilaiVolunteer_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.VOLUNTEER.ROUTE +
                            "?t=" + QS.GetTahunAjaranPure() +
                            "&s=" + QS.GetSemester() +
                            "&kd=" + QS.GetKelas()
                        )
                );
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

        protected void lnkPilihKelas_Click1(object sender, EventArgs e)
        {

        }
    }
}