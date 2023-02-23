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
    public partial class wf_NilaiSiswaSikap : System.Web.UI.Page
    {
        const string NILAI_DEFAULT = "BAIK";
        
        public enum JenisAction
        {
            DoUpdate,
            DataLoaded,
            ShowInputNilaiAkhir
        }

        public static class AtributPenilaian
        {
            public static string TahunAjaran { get { return RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t")); } }
            public static string TahunAjaranPure { get { return Libs.GetQueryString("t"); } }
            public static string Semester
            {
                get
                {
                    if (Libs.GetQueryString("s").Trim() == "") return "1";
                    return Libs.GetQueryString("s");
                }
            }
            public static string Kelas
            {
                get
                {
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
                }
            }

            public static string KelasLevel { get { return Libs.GetQueryString("k"); } }
            public static string KelasDet { get { return Libs.GetQueryString("kd"); } }
            public static string Mapel { get { return Libs.GetQueryString("m"); } }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowSubHeaderGuru = true;
            this.Master.ShowHeaderSubTitle = false;
            this.Master.SelectMenuGuru_Penilaian();
            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/stats.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Nilai Sikap";
            InitURLOnMenu();

            if (!IsPostBack)
            {
                LoadData();

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

                if (QS.GetMapel().Trim() == "")
                {
                    div_container.Attributes["class"] = "col-md-12";
                }
                else
                {
                    div_container.Attributes["class"] = "col-md-6 col-md-offset-3";
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

            if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
            {
                this.Master.ShowSubHeaderGuru = false;
            }
            else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_SIKAP)
            {
                this.Master.ShowSubHeaderGuru = false;
            }
            else
            {
                this.Master.ShowSubHeaderGuru = true;
            }
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

            public static string GetGuru()
            {
                string guru = Libs.GetQueryString("g");
                if (guru.Trim() == "") return Libs.LOGGED_USER_M.NoInduk;
                return guru;
            }
        }

        public bool IsReadOnly(DAO_Rapor_StrukturNilai.StrukturNilai m_struktur_nilai)
        {
            bool is_readonly = _UI.IsReadonlyNilai(
                                        m_struktur_nilai.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, QS.GetKelas(), m_struktur_nilai.Rel_Mapel.ToString(), m_struktur_nilai.TahunAjaran, m_struktur_nilai.Semester
                                    );
            if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI) is_readonly = false;
            return is_readonly;
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

        class NilaiSikapSiswa
        {
            public string Rel_Siswa { get; set; }
            public string Rel_Mapel { get; set; }
            public string Rel_SikapSpiritual { get; set; }
            public string Rel_SikapSosial { get; set; }
            public string PredikatSikapSpiritual { get; set; }
            public string PredikatSikapSosial { get; set; }
            public string DeskripsiSikapSpiritual { get; set; }
            public string DeskripsiSikapSosial { get; set; }
            public string SikapSpiritualAkhir { get; set; }
            public string SikapSosialAkhir { get; set; }
            public string PredikatSikapSpiritualAkhir { get; set; }
            public string PredikatSikapSosialAkhir { get; set; }
        }

        class JumlahPredikat
        {
            public string Rel_Sikap { get; set; }
            public string PredikatSikap { get; set; }
            public int Jumlah { get; set; }
        }

        protected void LoadData()
        {
            bool ada_statusbar = false;
            string tahun_ajaran = AtributPenilaian.TahunAjaran;
            string semester = AtributPenilaian.Semester;
            string rel_kelas = AtributPenilaian.Kelas;
            string rel_kelas_det = AtributPenilaian.KelasDet;
            string rel_mapel = AtributPenilaian.Mapel;

            string html_list_siswa = "";
            txtKodeSN.Value = "";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            
            //list siswa
            List<Siswa> lst_siswa = new List<Siswa>();
            List<NilaiSikapSiswa> lst_nilai_sikap_siswa = new List<NilaiSikapSiswa>();

            lst_nilai_sikap_siswa.Clear();
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

            //status bar
            ltrStatusBar.Text = "<span style=\"font-weight: normal;\">Nilai Sikap</span>&nbsp;";
            if (m_mapel != null)
            {
                if (m_mapel.Nama != null)
                {
                    ltrStatusBar.Text += "<span style=\"font-weight: bold;\">" + m_mapel.Nama + "</span>";
                    ada_statusbar = true;
                }
            }

            if (tahun_ajaran.Trim() != "")
            {
                ltrStatusBar.Text += "&nbsp;" +
                                     "<span style=\"font-weight: normal;\">" + tahun_ajaran + "</span>";
                ada_statusbar = true;
            }

            if (semester.Trim() != "")
            {
                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;Sm." : "") +
                                     "&nbsp;" +
                                     "<span style=\"font-weight: normal;\">" + semester + "</span>";
                ada_statusbar = true;
            }
            if (!ada_statusbar)
            {
                ltrStatusBar.Text = "Data penilaian tidak dapat dibuka";
            }
            //end status bar
            ltrDeskripsiSikap.Text = "<div style=\"margin: 0 auto; display: none;\">..:: Deskripsi Kosong ::..</div>";

            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_sn_mapel = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, rel_kelas, QS.GetMapel()
                );
            DAO_Rapor_StrukturNilai.StrukturNilai m_sn_mapel = lst_sn_mapel.FirstOrDefault();
            bool is_readonly = true;
            if (m_sn_mapel != null)
            {
                is_readonly = IsReadOnly(m_sn_mapel);
            }
            if (QS.GetMapel().Trim() == "" && Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()) is_readonly = false;

            if (is_readonly)
            {
                ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: white;\"></i>" +
                                    "&nbsp;&nbsp;" +
                                    ltrStatusBar.Text;
                div_statusbar.Attributes["style"] = "color: white; height: 40px; background-color: #ce3584; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
            }
            else
            {
                ltrStatusBar.Text = "<i class=\"fa fa-file-text-o\" style=\"color: green;\"></i>" +
                                    "&nbsp;&nbsp;" +
                                    ltrStatusBar.Text;
                div_statusbar.Attributes["style"] = "color: black; height: 40px; background-color: #eeeeee; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc; box-shadow: none; border-top-style: solid; border-top-color: #bfbfbf; border-top-width: 1px;";
            }

            List<Rapor_Desain> lst_rapor_desain = DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);
            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByTABySMByJenisRapor_Entity(tahun_ajaran, semester, DAO_Rapor_Desain.JenisRapor.Semester);

            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllMapelSikapByTAByKelas_Entity(
                    tahun_ajaran, semester, rel_kelas
                );

            List<DAO_Rapor_StrukturNilai.StrukturNilaiPredikat> lst_sn_predikat = new List<DAO_Rapor_StrukturNilai.StrukturNilaiPredikat>();
            if (lst_sn.Count > 0)
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m_sn = lst_sn.FirstOrDefault();
                if (m_sn != null)
                {
                    if (m_sn.TahunAjaran != null)
                    {
                        txtKodeSN.Value = m_sn.Kode.ToString();
                        lst_sn_predikat = DAO_Rapor_StrukturNilai.GetPredikatByHeader_Entity(m_sn.Kode.ToString()).
                                          FindAll(m0 => !(m0.Predikat.Trim() == "" && m0.Deskripsi.Trim() == ""));

                        ltrDeskripsiSikap.Text = "<div style=\"text-align: left;\">" +
                                                    "<span style=\"font-weight: bold;\">Sikap Spiritual</span>" +
                                                    "<div style=\"margin-bottom: 7px; margin-top: 5px; padding-left: 10px; padding-right: 10px; background-color: white; font-weight: normal; width: 100%; border-style: solid; border-color: #c0dfd7; border-width: 1px; border-radius: 5px; background-color: #F1F9F7; font-size: small;\">" +
                                                        m_sn.DeskripsiSikapSpiritual +
                                                    "</div>" +
                                                    "<span style=\"font-weight: bold;\">Sikap Sosial</span><br />" +
                                                    "<div style=\"margin-top: 5px; padding-left: 10px; padding-right: 10px; background-color: white; font-weight: normal; width: 100%; border-style: solid; border-color: #c0dfd7; border-width: 1px; border-radius: 5px; background-color: #F1F9F7; font-size: small;\">" +
                                                        m_sn.DeskripsiSikapSosial +
                                                    "</div>" +
                                                 "</div>";
                    }
                }
            }

            List<DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap> lst_nilai_sikap_by_kelas_det =
                    DAO_Rapor_NilaiSikapSiswa.GetByTABySMByMapelByKelasDet_Entity(
                            tahun_ajaran, semester, rel_kelas_det
                        );

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(
                    rel_kelas_det
                );

            string html_cbo_sikap_spiritual = "";
            string html_cbo_sikap_sosial = "";
            int id = 1;

            List<KelasDet> lst_kelas_jurusan =
                DAO_KelasDet.GetAll_Entity();
            foreach (Siswa m_siswa in lst_siswa)
            {
                lst_nilai_sikap_siswa.Clear();
                string nama_kelas = m_kelas_det.Nama + "-";
                string rel_kelas_jurusan = m_siswa.Rel_KelasDetJurusan;
                string rel_kelas_sosialisasi = m_siswa.Rel_KelasDetSosialisasi;
                if (rel_kelas_jurusan.Trim() != "")
                {
                    KelasDet m_kelas_jurusan = lst_kelas_jurusan
                        .FindAll(m0 => m0.Kode.ToString().Trim().ToUpper() == rel_kelas_jurusan.Trim().ToUpper()).FirstOrDefault();
                    if (m_kelas_jurusan != null)
                    {
                        if (m_kelas_jurusan.Nama != null)
                        {
                            nama_kelas = m_kelas_jurusan.Nama + "-";
                        }
                    }
                }

                string[] arr_nama_kelas = nama_kelas.Split(new string[] { "-" }, StringSplitOptions.None);
                string nama_kelas_ok = "";
                int id_str = 0;

                foreach (string item_nama_kelas in arr_nama_kelas)
                {
                    if (id_str == 2)
                    {
                        break;
                    }
                    nama_kelas_ok += (nama_kelas_ok.Trim() != "" ? "-" : "") + item_nama_kelas;
                    id_str++;
                }

                Rapor_Desain m_rapor_desain = lst_rapor_desain.
                    FindAll(
                        m0 => m0.Rel_Kelas.Trim().ToUpper() == nama_kelas_ok.Trim().ToUpper() &&
                              m0.JenisRapor == DAO_Rapor_Desain.JenisRapor.Semester
                    ).FirstOrDefault();

                string s_predikat = NILAI_DEFAULT;
                string sikap_spiritual = "";
                string sikap_sosial = "";

                bool ada_nilai = false;
                bool ada_seleksi = false;
                List<Rapor_NilaiSikapSiswa> lst_nilai_sikap = DAO_Rapor_NilaiSikapSiswa.GetByTABySMByMapelByKelasDetBySiswa_Entity(tahun_ajaran, semester, rel_mapel, rel_kelas_det, m_siswa.Kode.ToString());
                if (lst_nilai_sikap.Count > 0)
                {
                    sikap_spiritual = lst_nilai_sikap.FirstOrDefault().SikapSpiritual;
                    sikap_sosial = lst_nilai_sikap.FirstOrDefault().SikapSosial;
                    ada_nilai = true;
                }

                string id_cbo_sikap_spiritual = "cbo_spiritual_" + m_siswa.Kode.ToString().Replace("-", "_");
                string id_cbo_sikap_sosial = "cbo_sosial_" + m_siswa.Kode.ToString().Replace("-", "_");

                html_cbo_sikap_spiritual = "";                
                foreach (DAO_Rapor_StrukturNilai.StrukturNilaiPredikat item in lst_sn_predikat)
                {
                    if (ada_nilai)
                    {
                        if (item.Kode.ToString().Trim().ToUpper() == sikap_spiritual.ToString().Trim().ToUpper())
                        {
                            html_cbo_sikap_spiritual += "<option selected value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                        "</option>";
                            ada_seleksi = true;
                        }
                        else
                        {
                            html_cbo_sikap_spiritual += "<option value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                        "</option>";
                        }
                    }
                    else
                    {
                        if (item.Deskripsi.Trim().ToUpper() == s_predikat.Trim().ToUpper() && !is_readonly)
                        {
                            html_cbo_sikap_spiritual += "<option selected value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                        "</option>";
                            sikap_spiritual = item.Kode.ToString();
                            ada_seleksi = true;
                        }
                        else
                        {
                            html_cbo_sikap_spiritual += "<option value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                        "</option>";
                        }
                    }
                }
                if (!ada_seleksi && lst_sn_predikat.Count > 0)
                {
                    sikap_spiritual = lst_sn_predikat.FirstOrDefault().Kode.ToString();
                }

                html_cbo_sikap_sosial = "";
                foreach (DAO_Rapor_StrukturNilai.StrukturNilaiPredikat item in lst_sn_predikat)
                {
                    if (ada_nilai)
                    {
                        if (item.Kode.ToString().Trim().ToUpper() == sikap_sosial.ToString().Trim().ToUpper())
                        {
                            html_cbo_sikap_sosial += "<option selected value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                     "</option>";
                            ada_seleksi = true;
                        }
                        else
                        {
                            html_cbo_sikap_sosial += "<option value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                     "</option>";
                        }
                    }
                    else
                    {
                        if (item.Deskripsi.Trim().ToUpper() == s_predikat.Trim().ToUpper() && !is_readonly)
                        {
                            html_cbo_sikap_sosial += "<option selected value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                     "</option>";
                            sikap_sosial = item.Kode.ToString();
                            ada_seleksi = true;
                        }
                        else
                        {
                            html_cbo_sikap_sosial += "<option value=\"" + item.Kode.ToString() + "\">" +
                                                                item.Deskripsi +
                                                     "</option>";
                        }
                    }
                }                
                if (!ada_seleksi && lst_sn_predikat.Count > 0)
                {
                    sikap_spiritual = lst_sn_predikat.FirstOrDefault().Kode.ToString();
                    sikap_sosial = lst_sn_predikat.FirstOrDefault().Kode.ToString();                    
                }

                if (html_cbo_sikap_spiritual.Trim() != "")
                {
                    html_cbo_sikap_spiritual =
                        "<label style=\"font-size: small; color: grey;\">Sikap Spiritual</label><br />" +
                        "<select " +
                                (is_readonly ? " disabled " : "") +
                                "onchange=\"SaveNilaiSikap(" +
                                                "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                                "'" + AtributPenilaian.Semester + "', " +
                                                "'" + AtributPenilaian.Mapel + "', " +
                                                "'" + AtributPenilaian.KelasDet + "', " +
                                                "'" + m_siswa.Kode.ToString() + "', " +
                                                "this.value, " +
                                                "document.getElementById('" + id_cbo_sikap_sosial + "').value " +
                                           ");\" " +
                                "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                "id=\"" + id_cbo_sikap_spiritual + "\" " +
                                "name=\"cbo_nilai_spiritual[]\" " +
                                "title=\" Sikap Spiritual \" " +
                                "class=\"text-input\" " +
                                "style=\"font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px;\">" +
                            (
                                is_readonly 
                                ? "<option></option>"
                                : ""
                            ) +
                            html_cbo_sikap_spiritual +
                        "</select>";
                }

                if (html_cbo_sikap_sosial.Trim() != "")
                {
                    html_cbo_sikap_sosial =
                        "<label style=\"font-size: small; color: grey;\">Sikap Sosial</label><br />" +
                        "<select " +
                                (is_readonly ? " disabled " : "") +
                                "onchange=\"SaveNilaiSikap(" +
                                                "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                                "'" + AtributPenilaian.Semester + "', " +
                                                "'" + AtributPenilaian.Mapel + "', " +
                                                "'" + AtributPenilaian.KelasDet + "', " +
                                                "'" + m_siswa.Kode.ToString() + "', " +
                                                "document.getElementById('" + id_cbo_sikap_spiritual + "').value, " +
                                                "this.value" +
                                           ");\" " +
                                "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                "name=\"cbo_nilai_sosial[]\" " +
                                "id=\"" + id_cbo_sikap_sosial + "\" " +
                                "title=\" Sikap Sosial \" " +
                                "class=\"text-input\" " +
                                "style=\"font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px;\">" +
                            (
                                is_readonly
                                ? "<option></option>"
                                : ""
                            ) +
                            html_cbo_sikap_sosial +
                        "</select>";
                }

                if (!is_readonly)
                {
                    DAO_Rapor_NilaiSikapSiswa.SaveNilaiSikap(
                        tahun_ajaran,
                        semester,
                        rel_mapel,
                        rel_kelas_det,
                        m_siswa.Kode.ToString(),
                        sikap_spiritual,
                        sikap_sosial
                    );
                }

                string kelas_det = "";
                string s_bg = (id % 2 != 0 ? "#F9F9F9" : "#FFFFFF");

                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        kelas_det = m_kelas_det.Nama;
                    }
                }

                string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                if (
                        (QS.GetMapel() != "" && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()) ||
                        (QS.GetMapel() != "" && Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                    )
                {
                    html_list_siswa +=
                                        "<tr class=\"" + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                            "<td style=\"width: 60px; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf; padding-bottom: 7px; padding-top: 7px;\">" +
                                                id.ToString() +
                                                "." +
                                            "</td>" +
                                            "<td style=\"padding: 0px; font-size: small; padding-top: 7px; padding-right: 15px; padding-bottom: 7px; padding-top: 7px;\">" +
                                                (
                                                    m_siswa.NISSekolah.Trim() != ""
                                                    ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                        m_siswa.NISSekolah +
                                                        "</span>" +
                                                        "<br />"
                                                    : ""
                                                ) +
                                                "<span style=\"color: grey; font-weight: bold;\">" +
                                                    Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 4) +
                                                    (
                                                        m_siswa.Panggilan.Trim() != ""
                                                        ? "<span style=\"font-weight: normal\">" +
                                                            "&nbsp;" + s_panggilan +
                                                          "</span>"
                                                        : ""
                                                    ) +
                                                "</span>" +
                                                "<sup style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</sup>" +
                                            "</td>" +
                                            "<td style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                                html_cbo_sikap_spiritual +
                                            "</td>" +
                                            "<td style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                                html_cbo_sikap_sosial +
                                            "</td>" +
                                        "</tr>";
                }
                else
                {
                    if (m_rapor_desain != null)
                    {
                        List<Rapor_Desain_Det> lst_rapor_desain_det_ = lst_rapor_desain_det.FindAll(
                                m0 => m0.Rel_Rapor_Desain.ToString().ToUpper().Trim() == m_rapor_desain.Kode.ToString().ToUpper().Trim() &&
                                      m0.Rel_Mapel.Trim() != ""
                            );

                        List<DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap> lst_nilai_sikap_ = lst_nilai_sikap_by_kelas_det.FindAll(
                                m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                            );

                        //sikap per mata pelajaran
                        string css_cell_headers = "border-style: solid; border-width: 1px; text-align: center; font-weight: normal; padding: 3px; border-color: #bfbfbf;";
                        string css_cell_body = "border-style: solid; border-width: 1px; padding: 1px; padding-left: 5px; padding-right: 5px; border-color: #bfbfbf;";
                        string s_html_per_mapel = "";
                        string s_html_mapel = "";
                        string s_html_sikap = "";
                        string s_html_nilai_sikap = "";
                        foreach (var item_rapor_desain_det in lst_rapor_desain_det_)
                        {
                            if (item_rapor_desain_det.Rel_Mapel.Trim() != "")
                            {
                                DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap m_nilai_sikap = lst_nilai_sikap_.FindAll(
                                        m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_rapor_desain_det.Rel_Mapel.ToString().ToUpper().Trim()
                                    ).FirstOrDefault();

                                if (m_nilai_sikap != null)
                                {
                                    s_html_mapel += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 7pt; line-height: 1.5em; width: 150px; vertical-align: middle;\">" +
                                                        item_rapor_desain_det.NamaMapelRapor +
                                                    "</td>";

                                    s_html_sikap += "<td style=\"" + css_cell_headers + " font-size: 7pt; line-height: 1.5em; width: 75px; vertical-align: middle;\">" +
                                                        "Sikap<br />Spiritual" +
                                                    "</td>" +
                                                    "<td style=\"" + css_cell_headers + " font-size: 7pt; line-height: 1.5em; width: 75px; vertical-align: middle;\">" +
                                                        "Sikap<br />Sosial" +
                                                    "</td>";

                                    s_html_nilai_sikap +=
                                                    "<td style=\"" + css_cell_body + " font-size: 7pt; text-align: center; font-weight: bold; line-height: 1.5em; padding-top: 2px; width: 75px; vertical-align: middle;\">" +
                                                        m_nilai_sikap.PredikatSikapSpiritual +
                                                    "</td>" +
                                                    "<td style=\"" + css_cell_body + " font-size: 7pt; text-align: center; font-weight: bold; line-height: 1.5em; padding-top: 2px; width: 75px; vertical-align: middle;\">" +
                                                        m_nilai_sikap.PredikatSikapSosial +
                                                    "</td>";

                                    lst_nilai_sikap_siswa.Add(new NilaiSikapSiswa
                                    {
                                        Rel_Siswa = m_siswa.Kode.ToString().ToUpper().Trim(),
                                        Rel_Mapel = item_rapor_desain_det.Rel_Mapel.Trim().ToUpper(),
                                        PredikatSikapSpiritual = m_nilai_sikap.PredikatSikapSpiritual,
                                        PredikatSikapSosial = m_nilai_sikap.PredikatSikapSosial,
                                        Rel_SikapSpiritual = m_nilai_sikap.SikapSpiritual.Trim().ToUpper(),
                                        Rel_SikapSosial = m_nilai_sikap.SikapSosial.Trim().ToUpper()
                                    });
                                }
                            }
                        }

                        //nilai sikap walas
                        string s_nilai_spiritual_walas = "";
                        string s_nilai_sosial_walas = "";
                        string s_deskripsi_spiritual_akhir = "";
                        string s_deskripsi_sosial_akhir = "";
                        string s_predikat_spiritual_akhir = "";
                        string s_predikat_sosial_akhir = "";
                        DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap m_nilai_sikap_walas = lst_nilai_sikap_.FindAll(
                                        m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == ""
                                    ).FirstOrDefault();
                        string id_sikap_walas = "";
                        if (m_nilai_sikap_walas != null)
                        {
                            id_sikap_walas = m_nilai_sikap_walas.Kode.ToString();
                            lst_nilai_sikap_siswa.Add(new NilaiSikapSiswa
                            {
                                Rel_Siswa = m_siswa.Kode.ToString().ToUpper().Trim(),
                                Rel_Mapel = "",
                                PredikatSikapSpiritual = m_nilai_sikap_walas.PredikatSikapSpiritual,
                                PredikatSikapSosial = m_nilai_sikap_walas.PredikatSikapSosial,
                                Rel_SikapSpiritual = m_nilai_sikap_walas.SikapSpiritual.Trim().ToUpper(),
                                Rel_SikapSosial = m_nilai_sikap_walas.SikapSosial.Trim().ToUpper(),
                                DeskripsiSikapSpiritual = m_nilai_sikap_walas.DeskripsiSikapSpiritual.Trim(),
                                DeskripsiSikapSosial = m_nilai_sikap_walas.DeskripsiSikapSosial.Trim(),
                                SikapSpiritualAkhir = m_nilai_sikap_walas.SikapSpiritualAkhir.Trim(),
                                SikapSosialAkhir = m_nilai_sikap_walas.SikapSosialAkhir.Trim()
                            });
                            s_nilai_spiritual_walas = m_nilai_sikap_walas.PredikatSikapSpiritual;
                            s_nilai_sosial_walas = m_nilai_sikap_walas.PredikatSikapSosial;
                            s_deskripsi_spiritual_akhir = m_nilai_sikap_walas.DeskripsiSikapSpiritual;
                            s_deskripsi_sosial_akhir = m_nilai_sikap_walas.DeskripsiSikapSosial;
                            var m_sn_predikat =
                                lst_sn_predikat.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_sikap_walas.SikapSpiritualAkhir.ToUpper().Trim()).FirstOrDefault();
                            if (m_sn_predikat != null)
                            {
                                if (m_sn_predikat.Deskripsi != null)
                                {
                                    s_predikat_spiritual_akhir = m_sn_predikat.Deskripsi;
                                }
                            }

                            m_sn_predikat =
                                lst_sn_predikat.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_sikap_walas.SikapSosialAkhir.ToUpper().Trim()).FirstOrDefault();
                            if (m_sn_predikat != null)
                            {
                                if (m_sn_predikat.Deskripsi != null)
                                {
                                    s_predikat_sosial_akhir = m_sn_predikat.Deskripsi;
                                }
                            }
                        }

                        //modus
                        Dictionary<string, string> lst_distinct_spiritual =
                            lst_nilai_sikap_siswa.Select(m0 => new { m0.Rel_SikapSpiritual, m0.PredikatSikapSpiritual }).Distinct().
                            ToDictionary(m0 => m0.Rel_SikapSpiritual, m0 => m0.PredikatSikapSpiritual);
                        Dictionary<string, string> lst_distinct_sosial =
                            lst_nilai_sikap_siswa.Select(m0 => new { m0.Rel_SikapSosial, m0.PredikatSikapSosial }).Distinct().
                            ToDictionary(m0 => m0.Rel_SikapSosial, m0 => m0.PredikatSikapSosial);

                        List<JumlahPredikat> lst_jumlah_sikap_spiritual = new List<JumlahPredikat>();
                        foreach (var item_spiritual in lst_distinct_spiritual)
                        {
                            lst_jumlah_sikap_spiritual.Add(new JumlahPredikat
                            {
                                Rel_Sikap = item_spiritual.Key,
                                PredikatSikap = item_spiritual.Value,
                                Jumlah = lst_nilai_sikap_siswa.FindAll(
                                        m0 => m0.Rel_SikapSpiritual.Trim().ToUpper() == item_spiritual.Key.Trim().ToUpper()
                                    ).Count
                            });
                        }
                        lst_jumlah_sikap_spiritual = lst_jumlah_sikap_spiritual.OrderByDescending(
                                m0 => m0.Jumlah
                            ).ToList();
                        List<JumlahPredikat> lst_jumlah_sikap_sosial = new List<JumlahPredikat>();
                        foreach (var item_sosial in lst_distinct_sosial)
                        {
                            lst_jumlah_sikap_sosial.Add(new JumlahPredikat
                            {
                                Rel_Sikap = item_sosial.Key,
                                PredikatSikap = item_sosial.Value,
                                Jumlah = lst_nilai_sikap_siswa.FindAll(
                                        m0 => m0.Rel_SikapSosial.Trim().ToUpper() == item_sosial.Key.Trim().ToUpper()
                                    ).Count
                            });
                        }
                        lst_jumlah_sikap_sosial = lst_jumlah_sikap_sosial.OrderByDescending(
                                m0 => m0.Jumlah
                            ).ToList();

                        int id_sikap = 1;
                        int jml_tertinggi_sikap_spiritual = 0;
                        int jml_tertinggi_sikap_sosial = 0;

                        string modus_spiritual = "";
                        string modus_sosial = "";
                        foreach (var item in lst_jumlah_sikap_spiritual)
                        {
                            if (id_sikap == 1)
                            {
                                jml_tertinggi_sikap_spiritual = item.Jumlah;
                            }
                            if (item.Jumlah == jml_tertinggi_sikap_spiritual)
                            {
                                modus_spiritual += (modus_spiritual.Trim() != "" ? ", " : "") +
                                                   item.PredikatSikap + "&nbsp;<span class=\"badge\">" + item.Jumlah.ToString() + "</span>";
                            }
                            else
                            {
                                break;
                            }
                            id_sikap++;
                        }
                        id_sikap = 1;
                        foreach (var item in lst_jumlah_sikap_sosial)
                        {
                            if (id_sikap == 1)
                            {
                                jml_tertinggi_sikap_sosial = item.Jumlah;
                            }
                            if (item.Jumlah == jml_tertinggi_sikap_sosial)
                            {
                                modus_sosial += (modus_sosial.Trim() != "" ? ", " : "") +
                                                item.PredikatSikap + "&nbsp;<span class=\"badge\">" + item.Jumlah.ToString() + "</span>";
                            }
                            else
                            {
                                break;
                            }
                            id_sikap++;
                        }
                        //end modus

                        s_html_per_mapel += "<tr>" +
                                                s_html_mapel +
                                            "</tr>" +
                                            "<tr>" +
                                                s_html_sikap +
                                            "</tr>" +
                                            "<tr>" +
                                                s_html_nilai_sikap +
                                            "</tr>";
                        s_html_per_mapel = "<table style=\"border-collapse: collapse; width: 100%;\">" +
                                                s_html_per_mapel +
                                           "</table>";

                        string id_td_sikap_spiritual = "td_sikap_spiritual_" + m_siswa.Kode.ToString().Replace("-", "_");
                        string id_td_sikap_sosial = "td_sikap_sosial_" + m_siswa.Kode.ToString().Replace("-", "_");
                        string id_div_sikap_spiritual_modus = "div_sikap_spiritual_modus_" + m_siswa.Kode.ToString().Replace("-", "_");
                        string id_div_sikap_sosial_modus = "div_sikap_sosial_modus_" + m_siswa.Kode.ToString().Replace("-", "_");

                        html_list_siswa +=
                                            "<tr class=\"" + (id % 2 == 0 ? "standardrow" : "oddrow") + "\">" +
                                                "<td style=\"background-color: white; width: 60px; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf; padding-bottom: 7px; padding-top: 7px;\">" +
                                                    id.ToString() +
                                                    "." +
                                                "</td>" +
                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-right: 15px; padding-bottom: 15px; padding-top: 15px;\">" +
                                                    (
                                                        m_siswa.NISSekolah.Trim() != ""
                                                        ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                            m_siswa.NISSekolah +
                                                            "</span>" +
                                                            "<br />"
                                                        : ""
                                                    ) +
                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                        Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 4) +
                                                        (
                                                            m_siswa.Panggilan.Trim() != ""
                                                            ? "<span style=\"font-weight: normal\">" +
                                                                "&nbsp;" + s_panggilan +
                                                              "</span>"
                                                            : ""
                                                        ) +
                                                    "</span>";
                        html_list_siswa +=
                                                    "<sup style='float: right; color: mediumvioletred; font-weight: bold;'>" + kelas_det + "</sup>" +
                                                    "<div class=\"row\" style=\"padding: 10px; border-style: solid; border-width: 1px; border-color: #bfbfbf; margin-bottom: 5px; margin-left: 0px; margin-right: 0px;\">" +
                                                        "<div class=\"col-md-6\">" +
                                                            "<span style=\"font-weight: bold;\">Nilai Wali Kelas</span>" +
                                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                                "<tr>" +
                                                                    "<td id=\"" + id_td_sikap_spiritual + "\" style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;\">" +
                                                                        html_cbo_sikap_spiritual +
                                                                    "</td>" +
                                                                    "<td id=\"" + id_td_sikap_sosial + "\" style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;\">" +
                                                                        html_cbo_sikap_sosial +
                                                                    "</td>" +
                                                                "</tr>" +
                                                            "</table>" +
                                                        "</div>" +
                                                        "<div class=\"col-md-6\">" +
                                                            "<span style=\"font-weight: bold;\">Modus</span>" +
                                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                                "<tr>" +
                                                                    "<td style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;\">" +
                                                                        "<span style=\"color: grey; font-weight: normal;\">Sikap Spiritual</span><br />" +
                                                                        "<label id=\"" + id_div_sikap_spiritual_modus + "\" style=\"background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            modus_spiritual +
                                                                        "</label>" +
                                                                    "</td>" +
                                                                    "<td style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;\">" +
                                                                        "<span style=\"color: grey; font-weight: normal;\">Sikap Sosial</span><br />" +
                                                                        "<label id=\"" + id_div_sikap_sosial_modus + "\" style=\"background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            modus_sosial +
                                                                        "</label>" +
                                                                    "</td>" +
                                                                "</tr>" +
                                                            "</table>" +
                                                        "</div>" +
                                                    "</div>";
                        html_list_siswa +=
                                                    s_html_per_mapel +
                                                    "<div class=\"row\" style=\"padding: 10px; border-style: solid; border-width: 1px; border-color: #bfbfbf; margin-bottom: 5px; margin-left: 0px; margin-right: 0px; margin-top: 7px;\">" +
                                                        "<div class=\"col-md-6\">" +
                                                            "<span style=\"font-weight: bold;\">Nilai Akhir Sikap</span>" +
                                                            "&nbsp;&nbsp;&nbsp;" +
                                                            "<label class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "style=\"margin-left: 0px; color: #8f8f8f; font-weight: normal; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px; cursor: pointer; border-style: solid; border-width: 1px; font-size: small;\" " +
                                                                    "onclick=\"document.getElementById('" + txtNilaiWalasSpiritual.ClientID + "').value = '" + s_nilai_spiritual_walas + "'; " +
                                                                              "document.getElementById('" + txtNilaiWalasSosial.ClientID + "').value = '" + s_nilai_sosial_walas + "'; " +
                                                                              "document.getElementById('" + txtNilaiModusSpiritual.ClientID + "').value = document.getElementById('" + id_div_sikap_spiritual_modus + "').innerHTML; " +
                                                                              "document.getElementById('" + txtNilaiModusSosial.ClientID + "').value = document.getElementById('" + id_div_sikap_sosial_modus + "').innerHTML; " +
                                                                              txtIDSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " +
                                                                              txtIDNilaiSikap.ClientID + ".value = '" + id_sikap_walas + "'; " + btnShowIsiNilaiSikap.ClientID + ".click(); return false; \"><i class=\"fa fa-pencil\"></i>&nbsp;Isi Nilai Akhir&nbsp;</label>" +
                                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                                "<tr>" +
                                                                    "<td style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;\">" +
                                                                        "<span style=\"color: grey; font-weight: normal;\">Sikap Spiritual</span><br />" +
                                                                        "<label style=\"background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            Libs.GetHTMLSimpleText(s_predikat_spiritual_akhir) +
                                                                        "</label>" +
                                                                        "<label style=\"min-height: 100px; background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px; margin-top: 10px;\">" +
                                                                            Libs.GetHTMLSimpleText2(
                                                                                    s_deskripsi_spiritual_akhir
                                                                                ) +
                                                                        "</label>" +
                                                                    "</td>" +
                                                                "</tr>" +
                                                            "</table>" +
                                                        "</div>";
                        html_list_siswa +=
                                                        "<div class=\"col-md-6\">" +
                                                            "<label class=\"btn btn-flat waves-attach waves-effect\" style=\"margin-left: 0px; color: #8f8f8f; font-weight: normal; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px; cursor: pointer; border-style: solid; border-width: 1px; font-size: small; visibility: hidden;\"><i class=\"fa fa-pencil\"></i>&nbsp;Isi Nilai Akhir&nbsp;</label>" +
                                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                                "<tr>" +
                                                                    "<td style=\"padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;\">" +
                                                                        "<span style=\"color: grey; font-weight: normal;\">Sikap Sosial</span><br />" +
                                                                        "<label style=\"background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            Libs.GetHTMLSimpleText(s_predikat_sosial_akhir) +
                                                                        "</label>" +
                                                                        "<label style=\"min-height: 100px; background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px; margin-top: 10px;\">" +
                                                                            Libs.GetHTMLSimpleText2(
                                                                                    s_deskripsi_sosial_akhir
                                                                                ) +
                                                                        "</label>" +
                                                                    "</td>" +
                                                                "</tr>" +
                                                            "</table>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px; border-width: 2px;\" />" +
                                                "</td>" +
                                            "</tr>";
                    }
                }

                id++;
            }

            if (html_list_siswa.Trim() != "")
            {
                html_list_siswa =
                    "<div class=\"row\" style=\"margin-left: 0px; margin-right: 0px;\">" +
                        "<div class=\"col-xs-12\" style=\"margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 0px;\">" +
                            "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                "<table class=\"table\" style=\"margin: 0px; width: 100%;\">" +
                                    html_list_siswa +
                                "</table>" +
                            "</table>" +
                        "</div>" +
                    "</div>";
            }

            ltrNilaiSiswa.Text = html_list_siswa;
            txtKeyAction.Value = JenisAction.DataLoaded.ToString();
        }

        protected void btnShowIsiNilaiSikap_Click(object sender, EventArgs e)
        {
            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllMapelSikapByTAByKelas_Entity(
                    QS.GetTahunAjaran(), QS.GetSemester(), AtributPenilaian.Kelas
                );

            cboSikapSpiritual.Items.Clear();
            cboSikapSosial.Items.Clear();

            List<DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap> lst_nilai_sikap_by_kelas_det =
                DAO_Rapor_NilaiSikapSiswa.GetByTABySMByMapelByKelasDet_Entity(
                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()
                    );

            List<DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap> lst_nilai_sikap_ = lst_nilai_sikap_by_kelas_det.FindAll(
                    m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == txtIDSiswa.ToString().ToUpper().Trim() &&
                          m0.Rel_Mapel.Trim() == ""
                );

            txtDeskripsiSikapSpiritual.Text = "";
            txtDeskripsiSikapSpiritualVal.Value = "";
            txtDeskripsiSikapSosial.Text = "";
            txtDeskripsiSikapSosialVal.Value = "";
            if (txtIDNilaiSikap.Value.Trim() == "")
            {
                DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap m_nilai_sikap = lst_nilai_sikap_.FirstOrDefault();
                if (m_nilai_sikap != null)
                {
                    txtIDNilaiSikap.Value = m_nilai_sikap.Kode.ToString();
                }
            }
            if (txtIDNilaiSikap.Value.Trim() != "")
            {
                Rapor_NilaiSikapSiswa m_nilai_sikap_siswa = DAO_Rapor_NilaiSikapSiswa.GetByID_Entity(txtIDNilaiSikap.Value);
                if (m_nilai_sikap_siswa != null)
                {
                    if (m_nilai_sikap_siswa.Rel_Siswa != null)
                    {
                        Rapor_NilaiSikap m_nilai_sikap = DAO_Rapor_NilaiSikap.GetByID_Entity(m_nilai_sikap_siswa.Rel_Rapor_NilaiSikap.ToString());
                        string sikap_spiritual = "";
                        string sikap_sosial = "";
                        string deskripsi_sikap_spiritual_akhir = "";
                        string deskripsi_sikap_sosial_akhir = "";

                        bool ada_nilai = false;
                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                                QS.GetTahunAjaran(),
                                QS.GetSemester(),
                                m_nilai_sikap_siswa.Rel_Siswa
                            );

                        if (m_siswa != null)
                        {
                            if (m_siswa.Nama != null)
                            {
                                cboSikapSpiritual.Items.Add("");
                                cboSikapSosial.Items.Add("");
                                txtNamaSiswa.Text = m_siswa.Nama.Trim().ToUpper();
                                List<Rapor_NilaiSikapSiswa> lst_nilai_sikap = DAO_Rapor_NilaiSikapSiswa.GetByTABySMByMapelByKelasDetBySiswa_Entity(
                                    m_nilai_sikap.TahunAjaran, m_nilai_sikap.Semester, m_nilai_sikap.Rel_Mapel, m_nilai_sikap.Rel_KelasDet, m_siswa.Kode.ToString());
                                if (lst_nilai_sikap.Count > 0)
                                {
                                    sikap_spiritual = lst_nilai_sikap.FirstOrDefault().SikapSpiritualAkhir;
                                    sikap_sosial = lst_nilai_sikap.FirstOrDefault().SikapSosialAkhir;
                                    deskripsi_sikap_spiritual_akhir = lst_nilai_sikap.FirstOrDefault().DeskripsiSikapSpiritual;
                                    deskripsi_sikap_sosial_akhir = lst_nilai_sikap.FirstOrDefault().DeskripsiSikapSosial;
                                    ada_nilai = true;
                                }

                                string id_cbo_sikap_spiritual = "cbo_spiritual_" + m_siswa.Kode.ToString().Replace("-", "_");
                                string id_cbo_sikap_sosial = "cbo_sosial_" + m_siswa.Kode.ToString().Replace("-", "_");
                                var lst_sn_predikat = DAO_Rapor_StrukturNilai.GetPredikatByHeader_Entity(txtKodeSN.Value).
                                          FindAll(m0 => !(m0.Predikat.Trim() == "" && m0.Deskripsi.Trim() == "")).OrderBy(m0 => m0.Urutan);

                                foreach (DAO_Rapor_StrukturNilai.StrukturNilaiPredikat item in lst_sn_predikat)
                                {
                                    if (ada_nilai)
                                    {
                                        bool is_selected = false;
                                        if (item.Kode.ToString().Trim().ToUpper() == sikap_spiritual.ToString().Trim().ToUpper())
                                        {
                                            cboSikapSpiritual.Items.Add(new ListItem {
                                                Value = item.Kode.ToString(),
                                                Text = item.Deskripsi.ToString(),
                                                Selected = true
                                            });
                                            is_selected = true;
                                        }
                                        if(!is_selected)
                                        {
                                            cboSikapSpiritual.Items.Add(new ListItem
                                            {
                                                Value = item.Kode.ToString(),
                                                Text = item.Deskripsi.ToString(),
                                                Selected = false
                                            });
                                        }
                                    }
                                    else
                                    {
                                        cboSikapSpiritual.Items.Add(new ListItem
                                        {
                                            Value = item.Kode.ToString(),
                                            Text = item.Deskripsi.ToString(),
                                            Selected = false
                                        });
                                    }
                                }

                                foreach (DAO_Rapor_StrukturNilai.StrukturNilaiPredikat item in lst_sn_predikat)
                                {
                                    if (ada_nilai)
                                    {
                                        bool is_selected = false;
                                        if (item.Kode.ToString().Trim().ToUpper() == sikap_sosial.ToString().Trim().ToUpper())
                                        {
                                            cboSikapSosial.Items.Add(new ListItem
                                            {
                                                Value = item.Kode.ToString(),
                                                Text = item.Deskripsi.ToString(),
                                                Selected = true
                                            });
                                            is_selected = true;
                                        }
                                        if (!is_selected)
                                        {
                                            cboSikapSosial.Items.Add(new ListItem
                                            {
                                                Value = item.Kode.ToString(),
                                                Text = item.Deskripsi.ToString(),
                                                Selected = false
                                            });
                                        }
                                    }
                                    else
                                    {
                                        cboSikapSosial.Items.Add(new ListItem
                                        {
                                            Value = item.Kode.ToString(),
                                            Text = item.Deskripsi.ToString(),
                                            Selected = false
                                        });
                                    }
                                }

                                txtDeskripsiSikapSpiritual.Text = deskripsi_sikap_spiritual_akhir;
                                txtDeskripsiSikapSpiritualVal.Value = deskripsi_sikap_spiritual_akhir;
                                txtDeskripsiSikapSosial.Text = deskripsi_sikap_sosial_akhir;
                                txtDeskripsiSikapSosialVal.Value = deskripsi_sikap_sosial_akhir;

                                if (lst_sn.Count > 0)
                                {
                                    DAO_Rapor_StrukturNilai.StrukturNilai m_sn = lst_sn.FirstOrDefault();
                                    if (m_sn != null)
                                    {
                                        if (m_sn.TahunAjaran != null)
                                        {
                                            if (txtDeskripsiSikapSpiritualVal.Value.Trim() == "")
                                            {
                                                txtDeskripsiSikapSpiritual.Text = m_sn.DeskripsiSikapSpiritual;
                                                txtDeskripsiSikapSpiritualVal.Value = m_sn.DeskripsiSikapSpiritual;
                                            }
                                            if (txtDeskripsiSikapSosialVal.Value.Trim() == "")
                                            {
                                                txtDeskripsiSikapSosial.Text = m_sn.DeskripsiSikapSosial;
                                                txtDeskripsiSikapSosialVal.Value = m_sn.DeskripsiSikapSosial;
                                            }
                                        }
                                    }
                                }

                                txtKeyAction.Value = JenisAction.ShowInputNilaiAkhir.ToString();
                            }
                        }
                    }
                }                
            }            
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                DAO_Rapor_NilaiSikapSiswa.UpdateNilaiAkhir(
                        txtIDNilaiSikap.Value,
                        txtDeskripsiSikapSpiritualVal.Value,
                        txtDeskripsiSikapSosialVal.Value,
                        cboSikapSpiritual.SelectedValue,
                        cboSikapSosial.SelectedValue
                    );
                LoadData();
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }            
        }

        protected void btnDoLoadData_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}