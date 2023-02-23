using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Guru_Timeline : System.Web.UI.Page
    {
        protected const string KEY_KET_KEDISIPLINAN = "@ket_kedisiplinan";
        public const string SEPARATOR_ABSEN = "{{:|:}}";

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
            DoShowConfirmHapusAbsen,
            DoShowInputAbsen,
            DoShowInputAbsenLTS,
            DoShowTanggalInputAbsen,
            DoShowDownloadRekapAbsen,
            DataTidakBisaDibuka
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

            public static string GetLevel()
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(Libs.GetQueryString("kd"));
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        return m_kelas_det.Rel_Kelas.ToString();
                    }
                }

                return "";
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetMapel()
            {
                return Libs.GetQueryString("m").Trim();
            }

            public static string GetPeriode()
            {
                return Libs.GetQueryString("p").Trim();
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                if (s.Trim() == "") return Libs.GetSemesterByTanggal(DateTime.Now).ToString();
                return Libs.GetQueryString("s").Trim();
            }

            public static string GetTahunAjaranPure()
            {
                string t = Libs.GetQueryString("t");
                return t;
            }
        }

        protected class HTMLInputKedisiplinan
        {
            public string CheckboxInput { get; set; }
            public string TextboxKeteranganInput { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowHeaderSubTitle = false;
            this.Master.ShowSubHeaderGuru = true;
            this.Master.SelectMenuGuru_TimeLine();

            InitURLOnMenu();
            if (!IsPostBack)
            {
                ShowLinimasa();
                txtTanggalMulai.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                txtTanggalAkhir.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                txtRel_KelasDet.Value = QS.GetKelas();
                txtMapel.Value = (QS.GetMapel() == "" ? "-" : QS.GetMapel());
                txtTahunAjaran.Value = Libs.GetQueryString("t");
                ShowListPeriodeAbsen();
                txtNIK.Value = Libs.LOGGED_USER_M.NoInduk;
            }

            ShowCaptionTimeline();            
        }

        protected void ShowCaptionTimeline()
        {
            string s_html = "";
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");

            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

            string s_mapel = "";
            if (rel_mapel.Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);                

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
            
            ltrCaptionTimeLine.Text = s_html;

            ShowAttrAbsenLTS();
        }

        protected void ShowAttrAbsenLTS()
        {
            lnkAbsenSiswaRapor.Visible = false;
            string rel_kelas = QS.GetLevel();
            Kelas m_kelas_0 = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas_0 != null)
            {
                if (m_kelas_0.Nama != null)
                {
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas_0.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {

                            if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() == "")
                            {
                                lnkAbsenSiswaRapor.Visible = true;
                                lnkAbsenSiswaRapor.Attributes["style"] = "background-color: #424242;";
                                if (DAO_SiswaAbsenRapor.GetAllByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), "1", QS.GetKelas()).Count > 0)
                                {
                                    lnkAbsenSiswaRapor.Attributes["style"] = "background-color: #e70707;";
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void InitURLOnMenu()
        {
            string ft = Libs.Decryptdata(Libs.GetQueryString("ft"));
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

            string url_penilaian = Libs.GetURLPenilaian(Libs.GetQueryString("kd"));
            string m = Libs.GetQueryString("m");

            div_tanggal_buka_absen_jamke.Visible = false;
            if (m.Trim() != "")
            {
                bool ada = false;
                Mapel mapel = DAO_Mapel.GetByID_Entity(m);
                if (mapel != null)
                {
                    if (mapel.Nama != null)
                    {
                        div_tanggal_buka_absen_jamke.Visible = true;
                        if (mapel.Jenis.Trim().ToLower() == Libs.JENIS_MAPEL.KHUSUS.Trim().ToLower())
                        {
                            Response.Redirect(
                                            ResolveUrl(
                                        Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                                        "?t=" + Libs.GetQueryString("t") +
                                        (
                                            Libs.GetQueryString("m").Trim() != ""
                                            ? "&m=" + Libs.GetQueryString("m")
                                            : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                        ) +
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
                                        "&kd=" + Libs.GetQueryString("kd")
                                    )
                                );
                        }
                        else
                        {
                            this.Master.SetURLGuru_TimeLine(
                                ResolveUrl(
                                        Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE +
                                        "?t=" + Libs.GetQueryString("t") +
                                        (
                                            Libs.GetQueryString("m").Trim() != ""
                                            ? "&m=" + Libs.GetQueryString("m")
                                            : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                        ) +
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
                                        "&kd=" + Libs.GetQueryString("kd")
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
                                    (
                                        Libs.GetQueryString("m").Trim() != ""
                                        ? "&m=" + Libs.GetQueryString("m")
                                        : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                                    ) +
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
                                    "&kd=" + Libs.GetQueryString("kd")
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
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                            ) +
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
                            "&kd=" + Libs.GetQueryString("kd")
                        )
                );
            }

            this.Master.SetURLGuru_Siswa(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.ROUTE +
                            "?t=" + Libs.GetQueryString("t") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                            ) +
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
                            "&kd=" + Libs.GetQueryString("kd")
                        )
                );
            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            url_penilaian +
                            "?t=" + Libs.GetQueryString("t") +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : "&" + Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS
                            ) +
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
                            "&kd=" + Libs.GetQueryString("kd")
                        )
                );
        }

        protected void SelectDropdownJam(string dari_jam, string sampai_jam)
        {
            bool ada = false;
            string nilai = dari_jam + "-" + sampai_jam;
            if (sampai_jam.Trim() == "") nilai = dari_jam;
            int id = 0;
            foreach (ListItem item in cboJamKe.Items)
            {
                if (item.Value == nilai)
                {                    
                    cboJamKe.SelectedIndex = id;
                    ada = true;
                    break;
                }
                id++;
            }

            if (!ada)
            {
                id = 0;
                foreach (ListItem item in cboJamKe.Items)
                {
                    if (item.Value == dari_jam)
                    {
                        cboJamKe.SelectedIndex = id;
                        break;
                    }
                    id++;
                }
            }
        }

        protected void lnkAbsenSiswa_Click(object sender, EventArgs e)
        {
            txtTanggalAbsen.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            txtKeyAction.Value = JenisAction.DoShowTanggalInputAbsen.ToString();
        }

        protected void ShowLinimasa()
        {
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas_det = Libs.GetQueryString("kd");

            ltrLiniMasa.Text = "<div class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">";
            ltrLiniMasa.Text += "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">";

            List<LinimasaKelas> lst_linimasa = DAO_LinimasaKelas.GetAllByTahunAjaranByKelasDet_Entity(tahun_ajaran, rel_kelas_det);
            List<PeriodeAbsen> lst_periode_absen = new List<PeriodeAbsen>();
            int bulan = 0; 
            int tahun = 0;

            if (QS.GetMapel().Trim() != "")
            {
                lst_periode_absen = DAO_SiswaAbsenMapel.GetPeriodeBySekolahByKelasDetByTAByMapel_Entity(
                        QS.GetUnit(), QS.GetKelas(), QS.GetTahunAjaran(), QS.GetMapel()
                    );
            }
            else
            {
                lst_periode_absen = DAO_SiswaAbsen.GetPeriodeBySekolahByKelasDetByTA_Entity(
                        QS.GetUnit(), QS.GetKelas(), QS.GetTahunAjaran()
                    );
            }

            if (QS.GetPeriode().Length == 6)
            {
                bulan = Libs.GetStringToInteger(QS.GetPeriode().Substring(4, 2));
                tahun = Libs.GetStringToInteger(QS.GetPeriode().Substring(0, 4));
            }
            else
            {
                if (lst_periode_absen.Count > 0)
                {
                    bulan = lst_periode_absen[0].Bulan;
                    tahun = lst_periode_absen[0].Tahun;
                }
                else
                {
                    bulan = DateTime.Now.Month;
                    tahun = DateTime.Now.Year;
                }                
            }
            if (bulan == 0 || tahun == 0)
            {
                if (lst_periode_absen.Count > 0)
                {
                    bulan = lst_periode_absen[0].Bulan;
                    tahun = lst_periode_absen[0].Tahun;
                }
            }
            if (bulan == 0 || tahun == 0)
            {
                bulan = DateTime.Now.Month;
                tahun = DateTime.Now.Year;
            }

            if (QS.GetPeriode().Trim() == "")
            {
                if (lst_periode_absen.Count > 0) {
                    lst_linimasa = DAO_LinimasaKelas.GetAllByTahunAjaranByKelasDetByPeriode_Entity(
                            tahun_ajaran, rel_kelas_det, lst_periode_absen[0].Bulan, lst_periode_absen[0].Tahun
                        );
                }
            }
            else
            {
                if (QS.GetPeriode().Length == 6)
                {
                    if (QS.GetMapel().Trim() != "")
                    {
                        lst_linimasa = DAO_LinimasaKelas.GetAllByTahunAjaranByKelasDetByPeriodeByMapel_Entity(
                            tahun_ajaran,
                            rel_kelas_det,
                            bulan,
                            tahun,
                            QS.GetMapel().Trim()
                        );
                    }
                    else
                    {
                        lst_linimasa = DAO_LinimasaKelas.GetAllByTahunAjaranByKelasDetByPeriode_Entity(
                            tahun_ajaran,
                            rel_kelas_det,
                            bulan,
                            tahun
                        );
                    }
                }
            }

            int id = 0;
            bool ada_linimasa = false;
            int jml_linimasa = 0;
            
            foreach (LinimasaKelas m_linimasa in lst_linimasa)
            {
                bool ada_siswa = false;
                List<Siswa> lst_siswa = new List<Siswa>();
                Kelas m_kelas = DAO_Kelas.GetByID_Entity(QS.GetLevel());
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());

                if (DAO_Mapel.GetJenisMapel(QS.GetMapel()) == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN)
                {
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            if (m_kelas.Nama.Trim().ToUpper() != "X")
                            {
                                lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                        m_linimasa.TahunAjaran,
                                        Libs.GetSemesterByTanggal(m_linimasa.Tanggal).ToString(),
                                        QS.GetMapel(),
                                        QS.GetKelas()
                                    );
                                ada_siswa = true;
                            }
                            else
                            {
                                if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                    Libs.LOGGED_USER_M.NoInduk,
                                    m_linimasa.TahunAjaran,
                                    Libs.GetSemesterByTanggal(m_linimasa.Tanggal).ToString(),
                                    QS.GetMapel(),
                                    QS.GetKelas()
                                ))
                                {
                                    lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                        m_linimasa.TahunAjaran,
                                        Libs.GetSemesterByTanggal(m_linimasa.Tanggal).ToString(),
                                        QS.GetMapel(),
                                        QS.GetKelas()
                                    );

                                    ada_siswa = true;
                                }
                            }
                        }
                    }
                }
                else if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                {
                    lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                            m_linimasa.TahunAjaran,
                            Libs.GetSemesterByTanggal(m_linimasa.Tanggal).ToString(),
                            QS.GetMapel(),
                            QS.GetLevel(),
                            QS.GetKelas()
                        );

                    ada_siswa = true;
                }
                if (!ada_siswa)
                {
                    lst_siswa = DAO_Siswa.GetByRombel_Entity(
                            QS.GetUnit(), QS.GetKelas(), m_linimasa.TahunAjaran, Libs.GetSemesterByTanggal(m_linimasa.Tanggal).ToString()
                        );
                }

                KedisiplinanSetup m_kedisiplinan_setup = DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(
                    m_linimasa.TahunAjaran,
                    Libs.GetSemesterByTanggal(m_linimasa.Tanggal).ToString(),
                    QS.GetUnit(),
                    QS.GetLevel()
                ).FirstOrDefault();
                
                string kode_lm = m_linimasa.Kode.ToString().Replace("-", "_");
                string icon = "";
                string caption_lm = Libs.JENIS_LINIMASA.GetDeskripsiJenisLiniMasa(m_linimasa.Jenis).Replace("Absensi", "Presensi");
                string deskripsi_lm = "";

                string s_jml_hadir = "";
                string s_jml_terlambat = "";
                string s_jml_ditugaskan = "";
                string s_jml_sakit = "";
                string s_jml_izin = "";
                string s_jml_alfa = "";

                string js_linimasa_click = "";
                bool ada_item_linimasa = false;

                switch (m_linimasa.Jenis)
                {
                    case Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN:
                        if (QS.GetMapel() == "")
                        {
                            ada_item_linimasa = true;
                            ada_linimasa = true;
                            caption_lm += "<br />" +
                                          "<span style=\"font-weight: normal; color: grey; font-weight: bold;\">" +
                                            "<i class=\"fa fa-calendar\"></i>" +
                                            "&nbsp;" +
                                            m_linimasa.Keterangan +
                                            (
                                                "<span style=\"color: #329CC3;\">" +
                                                    "<span style=\"font-weight: normal, color: grey;\">,</span>&nbsp;" +
                                                    Libs.GetNamaHariFromTanggal(Libs.GetDateFromTanggalIndonesiaStr(m_linimasa.Keterangan)) +
                                                "</span>"
                                            ) +
                                          "</span>";
                            icon = "<i class=\"fa fa-id-card-o\"></i>";
                            List<SiswaAbsen> lst_absen = DAO_SiswaAbsen.GetAllByLiniMasa_Entity(m_linimasa.Kode.ToString());

                            if (lst_siswa.Count != lst_absen.Count)
                            {
                                caption_lm += "&nbsp;<sup title=\" Jumlah siswa tidak sama dengan jumlah data presensi \"><i class=\"fa fa-exclamation-triangle\" style=\"color: red;\"></i></sup>";
                            }

                            if (lst_absen.Count == lst_absen.Count(m => m.Absen.Trim().ToUpper() == ""))
                            {
                                s_jml_hadir = lst_absen.Count(m => m.Is_Hadir.IndexOf("__") < 0 && m.Is_Hadir.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1)).ToString();
                                s_jml_sakit = lst_absen.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.SAKIT.ToString().Substring(0, 1)).ToString();
                                s_jml_izin = lst_absen.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.IZIN.ToString().Substring(0, 1)).ToString();
                                s_jml_alfa = lst_absen.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.ALPA.ToString().Substring(0, 1)).ToString();

                                deskripsi_lm = "<span style=\"font-weight: bold; color: #008abc;\">Hadir : " + s_jml_hadir + "</span>, " +
                                               (
                                                    lst_absen.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" && m.Is_Sakit_Keterangan.Trim() != "")
                                                    < Libs.GetStringToInteger(s_jml_sakit)
                                                        ? "<span style=\"font-weight: bold; color: red;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                                        : "<span style=\"font-weight: bold; color: green;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                               ) + ", " +
                                               (
                                                    lst_absen.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" && m.Is_Izin_Keterangan.Trim() != "")
                                                    < Libs.GetStringToInteger(s_jml_izin)
                                                        ? "<span style=\"font-weight: bold; color: red;\">" + "Izin : " + s_jml_izin + "</span>"
                                                        : "<span style=\"font-weight: bold; color: green;\">" + "Izin : " + s_jml_izin + "</span>"
                                               ) + ", " +
                                               (
                                                    lst_absen.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" && m.Is_Alpa_Keterangan.Trim() != "")
                                                    < Libs.GetStringToInteger(s_jml_alfa)
                                                        ? "<span style=\"font-weight: bold; color: red;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                                        : "<span style=\"font-weight: bold; color: green;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                               );


                                //deskripsi tambahan kedisiplinan
                                string deskripsi_lm_add = "";
                                if (m_kedisiplinan_setup != null)
                                {
                                    if (m_kedisiplinan_setup.TahunAjaran != null)
                                    {
                                        int id_kedisiplinan = 1;
                                        foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                                        {
                                            if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                                            {
                                                var val = prop.GetValue(m_kedisiplinan_setup, null);
                                                if (val != null)
                                                {
                                                    if (val.ToString() != "")
                                                    {
                                                        Kedisiplinan m_kedisiplinan = DAO_Kedisiplinan.GetByID_Entity(
                                                                    val.ToString()
                                                                );
                                                        if (m_kedisiplinan != null)
                                                        {
                                                            if (m_kedisiplinan.Keterangan != null)
                                                            {
                                                                int i_count = lst_absen.Count(
                                                                                        m => 
                                                                                        (
                                                                                             m.Is_Cat01.IndexOf("__") < 0 && m.Is_Cat01.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat02.IndexOf("__") < 0 && m.Is_Cat02.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat03.IndexOf("__") < 0 && m.Is_Cat03.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat04.IndexOf("__") < 0 && m.Is_Cat04.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat05.IndexOf("__") < 0 && m.Is_Cat05.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat06.IndexOf("__") < 0 && m.Is_Cat06.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat07.IndexOf("__") < 0 && m.Is_Cat07.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat08.IndexOf("__") < 0 && m.Is_Cat08.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat09.IndexOf("__") < 0 && m.Is_Cat09.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat10.IndexOf("__") < 0 && m.Is_Cat10.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat11.IndexOf("__") < 0 && m.Is_Cat11.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat12.IndexOf("__") < 0 && m.Is_Cat12.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat13.IndexOf("__") < 0 && m.Is_Cat13.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat14.IndexOf("__") < 0 && m.Is_Cat14.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat15.IndexOf("__") < 0 && m.Is_Cat15.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat16.IndexOf("__") < 0 && m.Is_Cat16.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat17.IndexOf("__") < 0 && m.Is_Cat17.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat18.IndexOf("__") < 0 && m.Is_Cat18.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat19.IndexOf("__") < 0 && m.Is_Cat19.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat20.IndexOf("__") < 0 && m.Is_Cat20.Trim() == m_kedisiplinan.Alias.Trim()
                                                                                        ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                     );
                                                                int i_count_uncheck = lst_absen.Count(
                                                                                        m => 
                                                                                        (
                                                                                             m.Is_Cat01.IndexOf("__") >= 0 && m.Is_Cat01.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat02.IndexOf("__") >= 0 && m.Is_Cat02.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat03.IndexOf("__") >= 0 && m.Is_Cat03.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat04.IndexOf("__") >= 0 && m.Is_Cat04.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat05.IndexOf("__") >= 0 && m.Is_Cat05.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat06.IndexOf("__") >= 0 && m.Is_Cat06.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat07.IndexOf("__") >= 0 && m.Is_Cat07.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat08.IndexOf("__") >= 0 && m.Is_Cat08.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat09.IndexOf("__") >= 0 && m.Is_Cat09.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat10.IndexOf("__") >= 0 && m.Is_Cat10.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat11.IndexOf("__") >= 0 && m.Is_Cat11.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat12.IndexOf("__") >= 0 && m.Is_Cat12.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat13.IndexOf("__") >= 0 && m.Is_Cat13.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat14.IndexOf("__") >= 0 && m.Is_Cat14.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat15.IndexOf("__") >= 0 && m.Is_Cat15.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat16.IndexOf("__") >= 0 && m.Is_Cat16.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat17.IndexOf("__") >= 0 && m.Is_Cat17.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat18.IndexOf("__") >= 0 && m.Is_Cat18.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat19.IndexOf("__") >= 0 && m.Is_Cat19.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat20.IndexOf("__") >= 0 && m.Is_Cat20.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim()
                                                                                        ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                    );
                                                                int i_count_ket = lst_absen.Count(
                                                                                        m => 
                                                                                        (
                                                                                             (m.Is_Cat01.IndexOf("__") >= 0 && m.Is_Cat01.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat01_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat02.IndexOf("__") >= 0 && m.Is_Cat02.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat02_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat03.IndexOf("__") >= 0 && m.Is_Cat03.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat03_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat04.IndexOf("__") >= 0 && m.Is_Cat04.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat04_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat05.IndexOf("__") >= 0 && m.Is_Cat05.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat05_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat06.IndexOf("__") >= 0 && m.Is_Cat06.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat06_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat07.IndexOf("__") >= 0 && m.Is_Cat07.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat07_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat08.IndexOf("__") >= 0 && m.Is_Cat08.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat08_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat09.IndexOf("__") >= 0 && m.Is_Cat09.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat09_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat10.IndexOf("__") >= 0 && m.Is_Cat10.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat10_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat11.IndexOf("__") >= 0 && m.Is_Cat11.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat11_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat12.IndexOf("__") >= 0 && m.Is_Cat12.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat12_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat13.IndexOf("__") >= 0 && m.Is_Cat13.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat13_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat14.IndexOf("__") >= 0 && m.Is_Cat14.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat14_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat15.IndexOf("__") >= 0 && m.Is_Cat15.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat15_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat16.IndexOf("__") >= 0 && m.Is_Cat16.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat16_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat17.IndexOf("__") >= 0 && m.Is_Cat17.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat17_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat18.IndexOf("__") >= 0 && m.Is_Cat18.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat18_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat19.IndexOf("__") >= 0 && m.Is_Cat19.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat19_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat20.IndexOf("__") >= 0 && m.Is_Cat20.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat20_Keterangan.Trim() != "")
                                                                                        ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                    );

                                                                deskripsi_lm_add += (deskripsi_lm_add.Trim() != "" ? ", " : "") +
                                                                                    (
                                                                                        i_count_ket < i_count_uncheck
                                                                                        ? "<span style=\"font-weight: bold; color: red;\">" +
                                                                                                m_kedisiplinan.Keterangan + " : " +
                                                                                                i_count.ToString() +
                                                                                          "</span>"
                                                                                        : "<span style=\"font-weight: bold; color: green;\">" +
                                                                                                m_kedisiplinan.Keterangan + " : " +
                                                                                                i_count.ToString() +
                                                                                          "</span>"
                                                                                    );
                                                                id_kedisiplinan++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                deskripsi_lm += (deskripsi_lm.Trim() != "" && deskripsi_lm_add.Trim() != "" ? ", " : "") +
                                                deskripsi_lm_add;
                                //end deskripsi tambahan kedisiplinan
                            }
                            else
                            {
                                s_jml_hadir = lst_absen.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1)).ToString();
                                s_jml_terlambat = lst_absen.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.TERLAMBAT.ToString().Substring(0, 1)).ToString();
                                s_jml_ditugaskan = lst_absen.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.DITUGASKAN.ToString().Substring(0, 1)).ToString();
                                s_jml_sakit = lst_absen.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.SAKIT.ToString().Substring(0, 1)).ToString();
                                s_jml_izin = lst_absen.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.IZIN.ToString().Substring(0, 1)).ToString();
                                s_jml_alfa = lst_absen.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.ALPA.ToString().Substring(0, 1)).ToString();

                                s_jml_hadir = lst_absen.Count(m => m.Is_Hadir.IndexOf("__") < 0 && m.Is_Hadir.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1)).ToString();
                                s_jml_sakit = lst_absen.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.SAKIT.ToString().Substring(0, 1)).ToString();
                                s_jml_izin = lst_absen.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.IZIN.ToString().Substring(0, 1)).ToString();
                                s_jml_alfa = lst_absen.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.ALPA.ToString().Substring(0, 1)).ToString();

                                deskripsi_lm = "Hadir : " + s_jml_hadir + ", " +
                                               "Terlambat : " + s_jml_terlambat + ", " +
                                               (
                                                    s_jml_ditugaskan.Trim() != ""
                                                    ? "Ditugaskan : " + s_jml_ditugaskan + ", "
                                                    : ""
                                               ) +
                                               "Sakit : " + s_jml_sakit + ", " +
                                               "Izin : " + s_jml_izin + ", " +
                                               "Alpa : " + s_jml_alfa;

                                //add untuk versi baru format input data absen
                                deskripsi_lm = "<span style=\"font-weight: bold; color: #008abc;\">Hadir : " + s_jml_hadir + "</span>, " +
                                               (
                                                    lst_absen.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" && m.Is_Sakit_Keterangan.Trim() != "")
                                                    < Libs.GetStringToInteger(s_jml_sakit) &&
                                                    Libs.GetStringToInteger(s_jml_sakit) > 0

                                                        ? "<span style=\"font-weight: bold; color: red;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                                        : "<span style=\"font-weight: bold; color: green;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                               ) + ", " +
                                               (
                                                    lst_absen.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" && m.Is_Izin_Keterangan.Trim() != "")
                                                    < Libs.GetStringToInteger(s_jml_izin) &&
                                                    Libs.GetStringToInteger(s_jml_izin) > 0
                                                        ? "<span style=\"font-weight: bold; color: red;\">" + "Izin : " + s_jml_izin + "</span>"
                                                        : "<span style=\"font-weight: bold; color: green;\">" + "Izin : " + s_jml_izin + "</span>"
                                               ) + ", " +
                                               (
                                                    lst_absen.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" && m.Is_Alpa_Keterangan.Trim() != "")
                                                    < Libs.GetStringToInteger(s_jml_alfa) &&
                                                    Libs.GetStringToInteger(s_jml_alfa) > 0
                                                        ? "<span style=\"font-weight: bold; color: red;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                                        : "<span style=\"font-weight: bold; color: green;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                               );

                                //deskripsi tambahan kedisiplinan
                                string deskripsi_lm_add = "";
                                if (m_kedisiplinan_setup != null)
                                {
                                    if (m_kedisiplinan_setup.TahunAjaran != null)
                                    {
                                        int id_kedisiplinan = 1;
                                        foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                                        {
                                            if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                                            {
                                                var val = prop.GetValue(m_kedisiplinan_setup, null);
                                                if (val != null)
                                                {
                                                    if (val.ToString() != "")
                                                    {
                                                        Kedisiplinan m_kedisiplinan = DAO_Kedisiplinan.GetByID_Entity(
                                                                    val.ToString()
                                                                );
                                                        if (m_kedisiplinan != null)
                                                        {
                                                            if (m_kedisiplinan.Keterangan != null)
                                                            {
                                                                int i_count = lst_absen.Count(
                                                                                        m => 
                                                                                        (
                                                                                             m.Is_Cat01.IndexOf("__") < 0 && m.Is_Cat01.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat02.IndexOf("__") < 0 && m.Is_Cat02.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat03.IndexOf("__") < 0 && m.Is_Cat03.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat04.IndexOf("__") < 0 && m.Is_Cat04.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat05.IndexOf("__") < 0 && m.Is_Cat05.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat06.IndexOf("__") < 0 && m.Is_Cat06.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat07.IndexOf("__") < 0 && m.Is_Cat07.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat08.IndexOf("__") < 0 && m.Is_Cat08.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat09.IndexOf("__") < 0 && m.Is_Cat09.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat10.IndexOf("__") < 0 && m.Is_Cat10.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat11.IndexOf("__") < 0 && m.Is_Cat11.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat12.IndexOf("__") < 0 && m.Is_Cat12.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat13.IndexOf("__") < 0 && m.Is_Cat13.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat14.IndexOf("__") < 0 && m.Is_Cat14.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat15.IndexOf("__") < 0 && m.Is_Cat15.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat16.IndexOf("__") < 0 && m.Is_Cat16.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat17.IndexOf("__") < 0 && m.Is_Cat17.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat18.IndexOf("__") < 0 && m.Is_Cat18.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat19.IndexOf("__") < 0 && m.Is_Cat19.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat20.IndexOf("__") < 0 && m.Is_Cat20.Trim() == m_kedisiplinan.Alias.Trim()
                                                                                        ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                    );
                                                                int i_count_uncheck = lst_absen.Count(
                                                                                        m => 
                                                                                        (
                                                                                             m.Is_Cat01.IndexOf("__") >= 0 && m.Is_Cat01.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat02.IndexOf("__") >= 0 && m.Is_Cat02.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat03.IndexOf("__") >= 0 && m.Is_Cat03.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat04.IndexOf("__") >= 0 && m.Is_Cat04.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat05.IndexOf("__") >= 0 && m.Is_Cat05.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat06.IndexOf("__") >= 0 && m.Is_Cat06.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat07.IndexOf("__") >= 0 && m.Is_Cat07.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat08.IndexOf("__") >= 0 && m.Is_Cat08.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat09.IndexOf("__") >= 0 && m.Is_Cat09.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat10.IndexOf("__") >= 0 && m.Is_Cat10.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat11.IndexOf("__") >= 0 && m.Is_Cat11.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat12.IndexOf("__") >= 0 && m.Is_Cat12.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat13.IndexOf("__") >= 0 && m.Is_Cat13.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat14.IndexOf("__") >= 0 && m.Is_Cat14.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat15.IndexOf("__") >= 0 && m.Is_Cat15.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat16.IndexOf("__") >= 0 && m.Is_Cat16.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat17.IndexOf("__") >= 0 && m.Is_Cat17.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat18.IndexOf("__") >= 0 && m.Is_Cat18.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat19.IndexOf("__") >= 0 && m.Is_Cat19.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                             m.Is_Cat20.IndexOf("__") >= 0 && m.Is_Cat20.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim()
                                                                                        ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                   );
                                                                int i_count_ket = lst_absen.Count(
                                                                                        m => 
                                                                                        (
                                                                                             (m.Is_Cat01.IndexOf("__") >= 0 && m.Is_Cat01.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat01_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat02.IndexOf("__") >= 0 && m.Is_Cat02.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat02_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat03.IndexOf("__") >= 0 && m.Is_Cat03.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat03_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat04.IndexOf("__") >= 0 && m.Is_Cat04.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat04_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat05.IndexOf("__") >= 0 && m.Is_Cat05.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat05_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat06.IndexOf("__") >= 0 && m.Is_Cat06.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat06_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat07.IndexOf("__") >= 0 && m.Is_Cat07.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat07_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat08.IndexOf("__") >= 0 && m.Is_Cat08.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat08_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat09.IndexOf("__") >= 0 && m.Is_Cat09.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat09_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat10.IndexOf("__") >= 0 && m.Is_Cat10.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat10_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat11.IndexOf("__") >= 0 && m.Is_Cat11.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat11_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat12.IndexOf("__") >= 0 && m.Is_Cat12.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat12_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat13.IndexOf("__") >= 0 && m.Is_Cat13.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat13_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat14.IndexOf("__") >= 0 && m.Is_Cat14.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat14_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat15.IndexOf("__") >= 0 && m.Is_Cat15.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat15_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat16.IndexOf("__") >= 0 && m.Is_Cat16.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat16_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat17.IndexOf("__") >= 0 && m.Is_Cat17.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat17_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat18.IndexOf("__") >= 0 && m.Is_Cat18.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat18_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat19.IndexOf("__") >= 0 && m.Is_Cat19.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat19_Keterangan.Trim() != "") ||
                                                                                             (m.Is_Cat20.IndexOf("__") >= 0 && m.Is_Cat20.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat20_Keterangan.Trim() != "")
                                                                                        ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                    );

                                                                deskripsi_lm_add += (deskripsi_lm_add.Trim() != "" ? ", " : "") +
                                                                                    (
                                                                                        i_count_ket < i_count_uncheck
                                                                                        ? "<span style=\"font-weight: bold; color: red;\">" +
                                                                                                m_kedisiplinan.Keterangan + " : " +
                                                                                                i_count.ToString() +
                                                                                          "</span>"
                                                                                        : "<span style=\"font-weight: bold; color: green;\">" +
                                                                                                m_kedisiplinan.Keterangan + " : " +
                                                                                                i_count.ToString() +
                                                                                          "</span>"
                                                                                    );
                                                                id_kedisiplinan++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                deskripsi_lm += (deskripsi_lm.Trim() != "" && deskripsi_lm_add.Trim() != "" ? ", " : "") +
                                                deskripsi_lm_add;
                                //end deskripsi tambahan kedisiplinan
                            }

                            js_linimasa_click = txtKodeLinimasa.ClientID + ".value = '" + m_linimasa.Kode.ToString() + "'; " + btnDoShowAbsenByLinimasa.ClientID + ".click();";
                        }
                        break;
                    case Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL:
                        if (QS.GetMapel() != "")
                        {
                            ada_item_linimasa = false;
                            //by no unduk guru
                            //List<SiswaAbsenMapel> lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByLiniMasa_Entity(m_linimasa.Kode.ToString()).FindAll(
                            //        m0 => m0.Rel_Guru == Libs.LOGGED_USER_M.NoInduk
                            //    ).FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == QS.GetMapel().ToUpper().Trim());
                            List<SiswaAbsenMapel> lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByLiniMasa_Entity(m_linimasa.Kode.ToString()).FindAll(
                                    m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == QS.GetMapel().ToUpper().Trim()
                                );

                            if (lst_absen_mapel.Count > 0)
                            {
                                caption_lm += "<br />" +
                                          "<span style=\"font-weight: normal; color: grey; font-weight: bold;\">" +
                                            "<i class=\"fa fa-calendar\"></i>" +
                                            "&nbsp;" +
                                            m_linimasa.Keterangan +
                                            (
                                                "<span style=\"color: #329CC3;\">" +
                                                    "<span style=\"font-weight: normal, color: grey;\">,</span>&nbsp;" +
                                                    Libs.GetNamaHariFromTanggal(Libs.GetDateFromTanggalIndonesiaStr(m_linimasa.Keterangan)) + 
                                                "</span>"
                                            ) +                                            
                                          "</span>";
                                icon = "<i class=\"fa fa-id-card-o\"></i>";

                                if (lst_siswa.Count != lst_absen_mapel.Count)
                                {
                                    caption_lm += "&nbsp;<sup title=\" Jumlah siswa tidak sama dengan jumlah data presensi \"><i class=\"fa fa-exclamation-triangle\" style=\"color: red;\"></i></sup>";
                                }

                                ada_item_linimasa = true;
                                ada_linimasa = true;

                                if (lst_absen_mapel.Count == lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == ""))
                                {
                                    s_jml_hadir = lst_absen_mapel.Count(m => m.Is_Hadir.IndexOf("__") < 0 && m.Is_Hadir.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1)).ToString();
                                    s_jml_sakit = lst_absen_mapel.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.SAKIT.ToString().Substring(0, 1)).ToString();
                                    s_jml_izin = lst_absen_mapel.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.IZIN.ToString().Substring(0, 1)).ToString();
                                    s_jml_alfa = lst_absen_mapel.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.ALPA.ToString().Substring(0, 1)).ToString();

                                    deskripsi_lm = "<span style=\"font-weight: bold; color: #008abc;\">Hadir : " + s_jml_hadir + "</span>, " +
                                                   (
                                                        lst_absen_mapel.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" && m.Is_Sakit_Keterangan.Trim() != "")
                                                        < Libs.GetStringToInteger(s_jml_sakit)
                                                            ? "<span style=\"font-weight: bold; color: red;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                                            : "<span style=\"font-weight: bold; color: green;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                                   ) + ", " +
                                                   (
                                                        lst_absen_mapel.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" && m.Is_Izin_Keterangan.Trim() != "")
                                                        < Libs.GetStringToInteger(s_jml_izin)
                                                            ? "<span style=\"font-weight: bold; color: red;\">" + "Izin : " + s_jml_izin + "</span>"
                                                            : "<span style=\"font-weight: bold; color: green;\">" + "Izin : " + s_jml_izin + "</span>"
                                                   ) + ", " +
                                                   (
                                                        lst_absen_mapel.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" && m.Is_Alpa_Keterangan.Trim() != "")
                                                        < Libs.GetStringToInteger(s_jml_alfa)
                                                            ? "<span style=\"font-weight: bold; color: red;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                                            : "<span style=\"font-weight: bold; color: green;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                                   );

                                    //deskripsi tambahan kedisiplinan
                                    string deskripsi_lm_add = "";
                                    if (m_kedisiplinan_setup != null)
                                    {
                                        if (m_kedisiplinan_setup.TahunAjaran != null)
                                        {
                                            int id_kedisiplinan = 1;
                                            foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                                            {
                                                if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                                                {
                                                    var val = prop.GetValue(m_kedisiplinan_setup, null);
                                                    if (val != null)
                                                    {
                                                        if (val.ToString() != "")
                                                        {
                                                            Kedisiplinan m_kedisiplinan = DAO_Kedisiplinan.GetByID_Entity(
                                                                        val.ToString()
                                                                    );
                                                            if (m_kedisiplinan != null)
                                                            {
                                                                if (m_kedisiplinan.Keterangan != null)
                                                                {
                                                                    int i_count = lst_absen_mapel.Count(
                                                                                            m => 
                                                                                            (
                                                                                                 m.Is_Cat01.IndexOf("__") < 0 && m.Is_Cat01.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat02.IndexOf("__") < 0 && m.Is_Cat02.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat03.IndexOf("__") < 0 && m.Is_Cat03.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat04.IndexOf("__") < 0 && m.Is_Cat04.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat05.IndexOf("__") < 0 && m.Is_Cat05.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat06.IndexOf("__") < 0 && m.Is_Cat06.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat07.IndexOf("__") < 0 && m.Is_Cat07.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat08.IndexOf("__") < 0 && m.Is_Cat08.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat09.IndexOf("__") < 0 && m.Is_Cat09.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat10.IndexOf("__") < 0 && m.Is_Cat10.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat11.IndexOf("__") < 0 && m.Is_Cat11.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat12.IndexOf("__") < 0 && m.Is_Cat12.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat13.IndexOf("__") < 0 && m.Is_Cat13.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat14.IndexOf("__") < 0 && m.Is_Cat14.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat15.IndexOf("__") < 0 && m.Is_Cat15.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat16.IndexOf("__") < 0 && m.Is_Cat16.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat17.IndexOf("__") < 0 && m.Is_Cat17.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat18.IndexOf("__") < 0 && m.Is_Cat18.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat19.IndexOf("__") < 0 && m.Is_Cat19.Trim() == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat20.IndexOf("__") < 0 && m.Is_Cat20.Trim() == m_kedisiplinan.Alias.Trim()
                                                                                            ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                        );
                                                                    int i_count_uncheck = lst_absen_mapel.Count(
                                                                                            m => 
                                                                                            (
                                                                                                 m.Is_Cat01.IndexOf("__") >= 0 && m.Is_Cat01.Trim().Replace("__" , "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat02.IndexOf("__") >= 0 && m.Is_Cat02.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat03.IndexOf("__") >= 0 && m.Is_Cat03.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat04.IndexOf("__") >= 0 && m.Is_Cat04.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat05.IndexOf("__") >= 0 && m.Is_Cat05.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat06.IndexOf("__") >= 0 && m.Is_Cat06.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat07.IndexOf("__") >= 0 && m.Is_Cat07.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat08.IndexOf("__") >= 0 && m.Is_Cat08.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat09.IndexOf("__") >= 0 && m.Is_Cat09.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat10.IndexOf("__") >= 0 && m.Is_Cat10.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat11.IndexOf("__") >= 0 && m.Is_Cat11.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat12.IndexOf("__") >= 0 && m.Is_Cat12.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat13.IndexOf("__") >= 0 && m.Is_Cat13.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat14.IndexOf("__") >= 0 && m.Is_Cat14.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat15.IndexOf("__") >= 0 && m.Is_Cat15.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat16.IndexOf("__") >= 0 && m.Is_Cat16.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat17.IndexOf("__") >= 0 && m.Is_Cat17.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat18.IndexOf("__") >= 0 && m.Is_Cat18.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat19.IndexOf("__") >= 0 && m.Is_Cat19.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() ||
                                                                                                 m.Is_Cat20.IndexOf("__") >= 0 && m.Is_Cat20.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim()
                                                                                            ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                        );
                                                                    int i_count_ket = lst_absen_mapel.Count(
                                                                                            m => 
                                                                                            (
                                                                                                 (m.Is_Cat01.IndexOf("__") >= 0 && m.Is_Cat01.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat01_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat02.IndexOf("__") >= 0 && m.Is_Cat02.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat02_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat03.IndexOf("__") >= 0 && m.Is_Cat03.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat03_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat04.IndexOf("__") >= 0 && m.Is_Cat04.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat04_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat05.IndexOf("__") >= 0 && m.Is_Cat05.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat05_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat06.IndexOf("__") >= 0 && m.Is_Cat06.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat06_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat07.IndexOf("__") >= 0 && m.Is_Cat07.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat07_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat08.IndexOf("__") >= 0 && m.Is_Cat08.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat08_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat09.IndexOf("__") >= 0 && m.Is_Cat09.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat09_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat10.IndexOf("__") >= 0 && m.Is_Cat10.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat10_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat11.IndexOf("__") >= 0 && m.Is_Cat11.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat11_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat12.IndexOf("__") >= 0 && m.Is_Cat12.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat12_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat13.IndexOf("__") >= 0 && m.Is_Cat13.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat13_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat14.IndexOf("__") >= 0 && m.Is_Cat14.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat14_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat15.IndexOf("__") >= 0 && m.Is_Cat15.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat15_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat16.IndexOf("__") >= 0 && m.Is_Cat16.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat16_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat17.IndexOf("__") >= 0 && m.Is_Cat17.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat17_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat18.IndexOf("__") >= 0 && m.Is_Cat18.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat18_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat19.IndexOf("__") >= 0 && m.Is_Cat19.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat19_Keterangan.Trim() != "") ||
                                                                                                 (m.Is_Cat20.IndexOf("__") >= 0 && m.Is_Cat20.Trim().Replace("__", "") == m_kedisiplinan.Alias.Trim() && m.Is_Cat20_Keterangan.Trim() != "")
                                                                                            ) && (m.Is_Hadir == "HDR" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1))
                                                                                        );

                                                                    deskripsi_lm_add += (deskripsi_lm_add.Trim() != "" ? ", " : "") +
                                                                                        (
                                                                                            i_count_ket < i_count_uncheck
                                                                                            ? "<span style=\"font-weight: bold; color: red;\">" +
                                                                                                    m_kedisiplinan.Keterangan + " : " +
                                                                                                    i_count.ToString() +
                                                                                              "</span>"
                                                                                            : "<span style=\"font-weight: bold; color: green;\">" +
                                                                                                    m_kedisiplinan.Keterangan + " : " +
                                                                                                    i_count.ToString() +
                                                                                              "</span>"
                                                                                        );
                                                                    id_kedisiplinan++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    deskripsi_lm += (deskripsi_lm.Trim() != "" && deskripsi_lm_add.Trim() != "" ? ", " : "") +
                                                    deskripsi_lm_add;
                                    //end deskripsi tambahan kedisiplinan
                                }
                                else
                                {
                                    s_jml_hadir = lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1)).ToString();
                                    s_jml_terlambat = lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.TERLAMBAT.ToString().Substring(0, 1)).ToString();
                                    s_jml_ditugaskan = lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.DITUGASKAN.ToString().Substring(0, 1)).ToString();
                                    s_jml_sakit = lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.SAKIT.ToString().Substring(0, 1)).ToString();
                                    s_jml_izin = lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.IZIN.ToString().Substring(0, 1)).ToString();
                                    s_jml_alfa = lst_absen_mapel.Count(m => m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.ALPA.ToString().Substring(0, 1)).ToString();

                                    s_jml_hadir = lst_absen_mapel.Count(m => m.Is_Hadir.IndexOf("__") < 0 && m.Is_Hadir.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.HADIR.ToString().Substring(0, 1)).ToString();
                                    s_jml_sakit = lst_absen_mapel.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.SAKIT.ToString().Substring(0, 1)).ToString();
                                    s_jml_izin = lst_absen_mapel.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.IZIN.ToString().Substring(0, 1)).ToString();
                                    s_jml_alfa = lst_absen_mapel.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" || m.Absen.Trim().ToUpper() == Libs.JENIS_ABSENSI.ALPA.ToString().Substring(0, 1)).ToString();

                                    deskripsi_lm = "Hadir : " + s_jml_hadir + ", " +
                                                   "Terlambat : " + s_jml_terlambat + ", " +
                                                   (
                                                        s_jml_ditugaskan.Trim() != ""
                                                        ? "Ditugaskan : " + s_jml_ditugaskan + ", "
                                                        : ""
                                                   ) +
                                                   "Sakit : " + s_jml_sakit + ", " +
                                                   "Izin : " + s_jml_izin + ", " +
                                                   "Alpa : " + s_jml_alfa;

                                    //add untuk versi baru format input data absen
                                    deskripsi_lm = "<span style=\"font-weight: bold; color: #008abc;\">Hadir : " + s_jml_hadir + "</span>, " +
                                                   (
                                                        lst_absen_mapel.Count(m => m.Is_Sakit.IndexOf("__") < 0 && m.Is_Sakit.Trim() != "" && m.Is_Sakit_Keterangan.Trim() != "")
                                                        < Libs.GetStringToInteger(s_jml_sakit) &&
                                                        Libs.GetStringToInteger(s_jml_sakit) > 0

                                                            ? "<span style=\"font-weight: bold; color: red;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                                            : "<span style=\"font-weight: bold; color: green;\">" + "Sakit : " + s_jml_sakit + "</span>"
                                                   ) + ", " +
                                                   (
                                                        lst_absen_mapel.Count(m => m.Is_Izin.IndexOf("__") < 0 && m.Is_Izin.Trim() != "" && m.Is_Izin_Keterangan.Trim() != "")
                                                        < Libs.GetStringToInteger(s_jml_izin) &&
                                                        Libs.GetStringToInteger(s_jml_izin) > 0
                                                            ? "<span style=\"font-weight: bold; color: red;\">" + "Izin : " + s_jml_izin + "</span>"
                                                            : "<span style=\"font-weight: bold; color: green;\">" + "Izin : " + s_jml_izin + "</span>"
                                                   ) + ", " +
                                                   (
                                                        lst_absen_mapel.Count(m => m.Is_Alpa.IndexOf("__") < 0 && m.Is_Alpa.Trim() != "" && m.Is_Alpa_Keterangan.Trim() != "")
                                                        < Libs.GetStringToInteger(s_jml_alfa) &&
                                                        Libs.GetStringToInteger(s_jml_alfa) > 0
                                                            ? "<span style=\"font-weight: bold; color: red;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                                            : "<span style=\"font-weight: bold; color: green;\">" + "Alpa : " + s_jml_alfa + "</span>"
                                                   );
                                }

                                js_linimasa_click = txtKodeLinimasa.ClientID + ".value = '" + m_linimasa.Kode.ToString() + "'; " + btnDoShowAbsenByLinimasa.ClientID + ".click();";

                                SiswaAbsenMapel absen_mapel = lst_absen_mapel.FirstOrDefault();
                                if (absen_mapel != null)
                                {
                                    if (absen_mapel.Rel_Guru != null)
                                    {
                                        SelectDropdownJam(absen_mapel.JamAwal, absen_mapel.JamAkhir);
                                    }
                                }
                            }
                        }
                        break;
                }

                if (ada_item_linimasa)
                {
                    jml_linimasa++;
                    string js_linimasaevent = " onclick=\"" + js_linimasa_click + "\" data-target=\"#lm_" + kode_lm + "\" data-toggle=\"tile\" ";
                    ltrLiniMasa.Text += (id > 0 ? "<hr style=\"margin: 0px; border-color: #E6ECF0;\" />" : "") +
                                        "<div>" +
                                            "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"margin-top: 20px; margin-right: 20px;\">" +
                                                "<div class=\"avatar avatar-sm\" style=\"display: none;\">" +
                                                    icon +
                                                "</div>" +
                                                "<div class=\"avatar avatar-sm\">" +
                                                    jml_linimasa.ToString() +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-inner\">" +
                                                "<div class=\"text-overflow\" style=\"white-space: pre-wrap;\">" +
                                                    "<label " + js_linimasaevent + " style=\"cursor: pointer; font-weight: bold; color: black;\">" + caption_lm + "</label>" +
                                                    (
                                                        m_linimasa.ACT == DAO_SiswaAbsenMapel.JADWAL_ABSEN.SESUAI_JADWAL 
                                                        ? "&nbsp;<i title=\" Sesuai Jadwal \" class=\"fa fa-check-circle\" style=\"color: green;\"></i>"
                                                        : (
                                                            m_linimasa.ACT == DAO_SiswaAbsenMapel.JADWAL_ABSEN.DILUAR_JADWAL
                                                            ? "&nbsp;<i title=\" Diluar Jadwal \" class=\"fa fa-exclamation-triangle\" style=\"color: orangered;\"></i>"
                                                            : ""
                                                          )
                                                    ) +
                                                    "<label onclick=\"" + txtKodeLinimasa.ClientID + ".value = '" + m_linimasa.Kode.ToString() + "'; " + btnDoShowConfirmDeleteAbsen.ClientID + ".click(); \" " +
                                                           "style=\"float: right; padding: 0px; padding-left: 15px; padding-right: 15px; cursor: pointer; color: palevioletred; border-width: 1px; border-style: solid; border-color: palevioletred; border-radius: 10px; font-size: x-small\">Hapus</label>" +
                                                    "<br />" +
                                                    "<span style=\"font-weight: normal; color: #657786;\">" + 
                                                        Libs.GetTanggalIndonesiaSingkatFromDate(m_linimasa.Tanggal, true) + 
                                                        "&nbsp;" +
                                                        "<i class=\"fa fa-clock-o\"></i>&nbsp;" +
                                                        "Update terakhir : " +
                                                        Libs.GetTanggalIndonesiaSingkatFromDate(m_linimasa.TanggalUpdate, true) +
                                                    "</span>" +
                                                    (
                                                        deskripsi_lm.Trim() != ""
                                                        ? "<br />" +
                                                          "<i style=\"font-weight: normal; color: #657786;\">" + deskripsi_lm + "</i>"
                                                        : ""
                                                    ) +                                                    
                                                "</div>" +                                                
                                            "</div>" +
                                        "</div>";
                    id++;
                }
            }
            ltrLiniMasa.Text += "</div>";
            ltrLiniMasa.Text += "</div>";

            if (jml_linimasa == 0 || (jml_linimasa > 0 && !ada_linimasa))
            {
                ltrLiniMasa.Text = "<div style=\"padding: 15px;\">" +
                                        "<label style=\"margin: 0 auto; display: table; color: #bfbfbf;\">" +
                                            "..:: Data Kosong ::.." +
                                        "</label>" +
                                   "</div>";

                ltrPeriode.Text = "<a data-toggle=\"modal\" href=\"#ui_modal_periode_absen\" class=\"btn-flat\" style=\"font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px;\">" +
                                        "<i class=\"fa fa-calendar\" style=\"color: black;\"></i>" +
                                        "&nbsp;" +
                                        "&nbsp;" +
                                        "<span style=\"color: black;\">" +
                                            "Periode" +
                                        "</span>" +
                                        "&nbsp;" +
                                        "<span style=\"color: mediumvioletred;\">" +
                                            Libs.Array_Bulan[bulan - 1] +
                                            "&nbsp;" +
                                            tahun.ToString() +
                                        "</span>" +
                                        "&nbsp;" +
                                        "&nbsp;" +
                                        "<span class=\"badge\" style=\"background-color: mediumvioletred;\">" +
                                            "0" +
                                        "</span>" +
                                        "&nbsp;" +
                                  "</a>";
            }
            else
            {
                ltrPeriode.Text = "<a data-toggle=\"modal\" href=\"#ui_modal_periode_absen\" class=\"btn-flat\" style=\"font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px;\">" +
                                        "<i class=\"fa fa-calendar\" style=\"color: black;\"></i>" +
                                        "&nbsp;" +
                                        "&nbsp;" +
                                        "<span style=\"color: black;\">" +
                                            "Periode" +
                                        "</span>" +
                                        "&nbsp;" +
                                        "<span style=\"color: mediumvioletred;\">" +
                                            Libs.Array_Bulan[bulan - 1] +
                                            "&nbsp;" +
                                            tahun.ToString() +
                                        "</span>" +
                                        "&nbsp;" +
                                        "&nbsp;" +
                                        "<span class=\"badge\" style=\"background-color: mediumvioletred;\">" +
                                            jml_linimasa.ToString() +
                                        "</span>" +
                                        "&nbsp;" +
                                  "</a>";
            }

            ShowListPeriodeAbsen();
        }

        protected string GetHTMLCheckboxKehadiran(
                string kode_siswa,
                string chk_id_hadir,
                string chk_id_sakit,
                string chk_id_izin,
                string chk_id_alpa,
                SiswaAbsen m_siswaabsen,
                SiswaAbsenMapel m_siswaabsen_mapel,
                string tipe_absen_0_walas_1_mapel,
                string kode_linimasa,
                bool ada_data
            )
        {
            string s_waktu_update_hadir = ""; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string s_waktu_update_sakit = ""; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string s_waktu_update_izin = ""; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string s_waktu_update_alpa = ""; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string s_check_hadir = "1";
            string s_check_sakit = "0";
            string s_check_izin = "0";
            string s_check_alpa = "0";

            if (!ada_data && kode_linimasa.Trim() != "") s_check_hadir = "0";

            if (tipe_absen_0_walas_1_mapel == "0")
            {
                if (m_siswaabsen != null)
                {
                    if (m_siswaabsen.TahunAjaran != null)
                    {
                        if (m_siswaabsen.Absen.Trim() != "")
                        {
                            if (m_siswaabsen.Absen.Substring(0, 1).ToUpper().Trim() == "H")
                            {
                                s_check_hadir = "1";
                            }
                            else
                            {
                                s_check_hadir = "0";
                            }
                            if (m_siswaabsen.Absen.Substring(0, 1).ToUpper().Trim() == "S") s_check_sakit = "1";
                            if (m_siswaabsen.Absen.Substring(0, 1).ToUpper().Trim() == "I") s_check_izin = "1";
                            if (m_siswaabsen.Absen.Substring(0, 1).ToUpper().Trim() == "A") s_check_alpa = "1";
                        }
                        else
                        {
                            s_check_hadir = (m_siswaabsen.Is_Hadir.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Hadir.Trim() != "" ? "1" : "0"));
                            s_check_sakit = (m_siswaabsen.Is_Sakit.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Sakit.Trim() != "" ? "1" : "0"));
                            s_check_izin = (m_siswaabsen.Is_Izin.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Izin.Trim() != "" ? "1" : "0"));
                            s_check_alpa = (m_siswaabsen.Is_Alpa.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Alpa.Trim() != "" ? "1" : "0"));
                        }

                        s_waktu_update_hadir = (m_siswaabsen.Is_Hadir_Time != DateTime.MinValue ? m_siswaabsen.Is_Hadir_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                        s_waktu_update_sakit = (m_siswaabsen.Is_Sakit_Time != DateTime.MinValue ? m_siswaabsen.Is_Sakit_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                        s_waktu_update_izin = (m_siswaabsen.Is_Izin_Time != DateTime.MinValue ? m_siswaabsen.Is_Izin_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                        s_waktu_update_alpa = (m_siswaabsen.Is_Alpa_Time != DateTime.MinValue ? m_siswaabsen.Is_Alpa_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                    }
                }
            }
            else if(tipe_absen_0_walas_1_mapel == "1")
            {
                if (m_siswaabsen_mapel != null)
                {
                    if (m_siswaabsen_mapel.TahunAjaran != null)
                    {
                        if (m_siswaabsen_mapel.Absen.Trim() != "")
                        {
                            if (m_siswaabsen_mapel.Absen.Substring(0, 1).ToUpper().Trim() == "H")
                            {
                                s_check_hadir = "1";
                            }
                            else
                            {
                                s_check_hadir = "0";
                            }
                            if (m_siswaabsen_mapel.Absen.Substring(0, 1).ToUpper().Trim() == "S") s_check_sakit = "1";
                            if (m_siswaabsen_mapel.Absen.Substring(0, 1).ToUpper().Trim() == "I") s_check_izin = "1";
                            if (m_siswaabsen_mapel.Absen.Substring(0, 1).ToUpper().Trim() == "A") s_check_alpa = "1";
                        }
                        else
                        {
                            s_check_hadir = (m_siswaabsen_mapel.Is_Hadir.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Hadir.Trim() != "" ? "1" : "0"));
                            s_check_sakit = (m_siswaabsen_mapel.Is_Sakit.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Sakit.Trim() != "" ? "1" : "0"));
                            s_check_izin = (m_siswaabsen_mapel.Is_Izin.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Izin.Trim() != "" ? "1" : "0"));
                            s_check_alpa = (m_siswaabsen_mapel.Is_Alpa.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Alpa.Trim() != "" ? "1" : "0"));
                        }

                        s_waktu_update_hadir = (m_siswaabsen_mapel.Is_Hadir_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Hadir_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                        s_waktu_update_sakit = (m_siswaabsen_mapel.Is_Sakit_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Sakit_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                        s_waktu_update_izin = (m_siswaabsen_mapel.Is_Izin_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Izin_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                        s_waktu_update_alpa = (m_siswaabsen_mapel.Is_Alpa_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Alpa_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                    }
                }
            }

            string s_lang_hadir = kode_siswa + "~" +
                                  "HDR~" +
                                  s_check_hadir + "~" +
                                  s_waktu_update_hadir;
            string s_lang_sakit = kode_siswa + "~" +
                                  "SKT~" +
                                  s_check_sakit + "~" +
                                  s_waktu_update_sakit;
            string s_lang_izin = kode_siswa + "~" +
                                  "IZN~" +
                                  s_check_izin + "~" +
                                  s_waktu_update_izin;
            string s_lang_alpa = kode_siswa + "~" +
                                  "APA~" +
                                  s_check_alpa + "~" +
                                  s_waktu_update_alpa;

            return 
            "<div class=\"row\">" +
                "<div class=\"col-xs-6\">" +
                    "<div class=\"checkbox checkbox-adv\">" +
                        "<label for=\"" + chk_id_hadir + "\">" +
                            "<input " + 
                                    (s_check_hadir == "1" ? "checked=\"checked\" " : "") +
                                    "name=\"chk_pilih_kedisplinan[]\" " +
                                    "value=\"" + kode_siswa + "\" " +
                                    "class=\"access-hide\" " +
                                    "id=\"" + chk_id_hadir + "\" " +
                                    "lang=\"" + s_lang_hadir + "\" " +
                                    "onchange=\"SetCheckKedisiplinan(this.id); " +
                                               "SetDisabledCheckKetidakhadiran(this.checked, '" + chk_id_sakit + "', '" + chk_id_izin + "', '" + chk_id_alpa + "'); " +
                                               "ShowTextboxKeterangan('" + chk_id_sakit + "', false); " +
                                               "ShowTextboxKeterangan('" + chk_id_izin + "', false); " +
                                               "ShowTextboxKeterangan('" + chk_id_alpa + "', false); " +
                                               "ShowTextboxKeteranganKedisiplinan('div_kedisiplinan_" + kode_siswa.Replace("-", "_") + "', this.checked); " +
                                             "\" " +
                                    "type=\"checkbox\">" +
                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                            "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                "&nbsp;&nbsp;" +
                                Libs.JENIS_ABSENSI.HADIR +
                            "</span>" +
                        "</label>" +
                    "</div>" +
                "</div>" +

                "<div class=\"col-xs-6\">" +
                    "<div class=\"checkbox checkbox-adv\">" +
                        "<label for=\"" + chk_id_sakit + "\">" +
                            "<input " + 
                                    (s_check_sakit == "1" ? "checked=\"checked\" " : "") +
                                    (s_check_hadir == "1" ? "disabled=\"disabled\" " : "") +
                                    "name=\"chk_pilih_kedisplinan[]\" " +
                                    "value=\"" + kode_siswa + "\" " +
                                    "class=\"access-hide\" " +
                                    "id=\"" + chk_id_sakit + "\" " +
                                    "lang=\"" + s_lang_sakit + "\" " +
                                    "onchange=\"SetCheckKedisiplinan(this.id); " +
                                               "ShowTextboxKeterangan(this.id, this.checked); " +
                                             "\" " +
                                    "type=\"checkbox\">" +
                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                            "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                "&nbsp;&nbsp;" +
                                Libs.JENIS_ABSENSI.SAKIT +
                            "</span>" +
                        "</label>" +
                    "</div>" +
                "</div>" +
            "</div>" +

            "<div class=\"row\">" +
                "<div class=\"col-xs-6\">" +
                    "<div class=\"checkbox checkbox-adv\">" +
                        "<label for=\"" + chk_id_izin + "\">" +
                            "<input " + 
                                    (s_check_izin == "1" ? "checked=\"checked\" " : "") +
                                    (s_check_hadir == "1" ? "disabled=\"disabled\" " : "") +
                                    "name=\"chk_pilih_kedisplinan[]\" " +
                                    "value=\"" + kode_siswa + "\" " +
                                    "class=\"access-hide\" " +
                                    "id=\"" + chk_id_izin + "\" " +
                                    "lang=\"" + s_lang_izin + "\" " +
                                    "onchange=\"SetCheckKedisiplinan(this.id); " +
                                               "ShowTextboxKeterangan(this.id, this.checked); " +
                                             "\" " +
                                    "type=\"checkbox\">" +
                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                            "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                "&nbsp;&nbsp;" +
                                Libs.JENIS_ABSENSI.IZIN +
                            "</span>" +
                        "</label>" +
                    "</div>" +
                "</div>" +

                "<div class=\"col-xs-6\">" +
                    "<div class=\"checkbox checkbox-adv\">" +
                        "<label for=\"" + chk_id_alpa + "\">" +
                            "<input " + 
                                    (s_check_alpa == "1" ? "checked=\"checked\" " : "") +
                                    (s_check_hadir == "1" ? "disabled=\"disabled\" " : "") +
                                    "name=\"chk_pilih_kedisplinan[]\" " +
                                    "value=\"" + kode_siswa + "\" " +
                                    "class=\"access-hide\" " +
                                    "id=\"" + chk_id_alpa + "\" " +
                                    "lang=\"" + s_lang_alpa + "\" " +
                                    "onchange=\"SetCheckKedisiplinan(this.id); " +
                                               "ShowTextboxKeterangan(this.id, this.checked); " +
                                             "\" " +
                                    "type=\"checkbox\">" +
                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                            "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                "&nbsp;&nbsp;" +
                                Libs.JENIS_ABSENSI.ALPA +
                            "</span>" +
                        "</label>" +
                    "</div>" +
                "</div>" +
            "</div>";
        }

        protected string GetKalimatNegatif(string kode_template_kedisiplinan, string kalimat)
        {
            switch (kalimat)
            {
                case "Ketepatan Waktu":
                    kalimat = "Terlambat";
                    break;
                case "Kamera Aktif":
                    kalimat = "Kamera Tidak Aktif";
                    break;
                case "Seragam Lengkap":
                    kalimat = "Seragam Tidak Lengkap";
                    break;
                default:
                    break;
            }
            return kalimat;
        }

        protected HTMLInputKedisiplinan GetHTMLInputKedisiplinan(
                string tahun_ajaran,
                string semester,
                string kode_siswa,
                SiswaAbsen m_siswaabsen,
                SiswaAbsenMapel m_siswaabsen_mapel,
                string tipe_absen_0_walas_1_mapel,
                string kode_linimasa,
                bool ada_data
            )
        {
            string html_chk = "";
            string html_txt = "";
            string html_chk_kedisiplinan = "";
            string html_txt_kedisiplinan = "";
            
            Siswa m_siswa = DAO_Siswa.GetByKode_Entity(tahun_ajaran, semester, kode_siswa);
            if (m_siswa != null)
            {
                if (m_siswa.Nama != null)
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Rel_Kelas != null)
                        {
                            KedisiplinanSetup m_kedisiplinan_setup = DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(
                                    tahun_ajaran, semester, m_siswa.Rel_Sekolah, m_kelas_det.Rel_Kelas.ToString()
                                ).FirstOrDefault();
                            if (m_kedisiplinan_setup != null)
                            {
                                if (m_kedisiplinan_setup.TahunAjaran != null)
                                {
                                    int id_kedisiplinan = 1;
                                    foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                                    {
                                        if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                                        {
                                            var val = prop.GetValue(m_kedisiplinan_setup, null);
                                            if (val != null)
                                            {
                                                if (val.ToString() != "")
                                                {
                                                    Kedisiplinan m_kedisiplinan = DAO_Kedisiplinan.GetByID_Entity(
                                                                val.ToString()
                                                            );
                                                    if (m_kedisiplinan != null)
                                                    {
                                                        if (m_kedisiplinan.Keterangan != null)
                                                        {
                                                            string s_waktu_update = ""; //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                            string s_check = "1";
                                                            string s_keterangan = "";
                                                            string chk_id = "chk_" + 
                                                                            m_kedisiplinan.Alias + "_" +
                                                                            kode_siswa;

                                                            if (!ada_data && kode_linimasa.Trim() != "") s_check = "0";

                                                            if (tipe_absen_0_walas_1_mapel == "0")
                                                            {
                                                                if (m_siswaabsen != null)
                                                                {
                                                                    if (m_siswaabsen.TahunAjaran != null)
                                                                    {
                                                                        if (m_siswaabsen.Is_Cat01.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat01.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat01.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat01_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat01_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat01_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat02.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat02.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat02.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat02_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat02_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat02_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat03.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat03.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat03.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat03_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat03_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat03_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat04.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat04.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat04.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat04_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat04_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat04_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat05.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat05.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat05.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat05_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat05_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat05_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat06.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat06.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat06.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat06_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat06_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat06_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat07.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat07.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat07.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat07_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat07_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat07_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat08.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat08.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat08.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat08_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat08_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat08_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat09.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat09.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat09.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat09_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat09_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat09_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat10.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat10.IndexOf("__") >= 0 ? "0" : (m_siswaabsen.Is_Cat10.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat10_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat10_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat10_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat11.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat11.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat11.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat11_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat11_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat11_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat12.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat12.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat12.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat12_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat12_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat12_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat13.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat13.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat13.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat13_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat13_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat13_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat14.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat14.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat14.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat14_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat14_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat14_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat15.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat15.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat15.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat15_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat15_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat15_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat16.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat16.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat16.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat16_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat16_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat16_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat17.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat17.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat17.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat17_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat17_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat17_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat18.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat18.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat18.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat18_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat18_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat18_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat19.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat19.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat19.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat19_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat19_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat19_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen.Is_Cat20.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen.Is_Cat20.IndexOf("__") >= 1 ? "1" : (m_siswaabsen.Is_Cat20.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen.Is_Cat11_Time != DateTime.MinValue ? m_siswaabsen.Is_Cat20_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen.Is_Cat20_Keterangan;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else if (tipe_absen_0_walas_1_mapel == "1")
                                                            {
                                                                if (m_siswaabsen_mapel != null)
                                                                {
                                                                    if (m_siswaabsen_mapel.TahunAjaran != null)
                                                                    {
                                                                        if (m_siswaabsen_mapel.Is_Cat01.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat01.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat01.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat01_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat01_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat01_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat02.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat02.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat02.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat02_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat02_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat02_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat03.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat03.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat03.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat03_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat03_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat03_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat04.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat04.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat04.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat04_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat04_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat04_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat05.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat05.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat05.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat05_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat05_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat05_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat06.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat06.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat06.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat06_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat06_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat06_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat07.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat07.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat07.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat07_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat07_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat07_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat08.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat08.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat08.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat08_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat08_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat08_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat09.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat09.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat09.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat09_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat09_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat09_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat10.IndexOf(m_kedisiplinan.Alias) >= 0)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat10.IndexOf("__") >= 0 ? "0" : (m_siswaabsen_mapel.Is_Cat10.Trim() != "" ? "1" : "0"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat10_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat10_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat10_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat11.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat11.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat11.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat11_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat11_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat11_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat12.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat12.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat12.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat12_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat12_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat12_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat13.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat13.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat13.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat13_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat13_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat13_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat14.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat14.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat14.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat14_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat14_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat14_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat15.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat15.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat15.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat15_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat15_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat15_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat16.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat16.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat16.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat16_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat16_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat16_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat17.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat17.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat17.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat17_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat17_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat17_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat18.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat18.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat18.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat18_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat18_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat18_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat19.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat19.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat19.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat19_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat19_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat19_Keterangan;
                                                                        }
                                                                        else if (m_siswaabsen_mapel.Is_Cat20.IndexOf(m_kedisiplinan.Alias) >= 1)
                                                                        {
                                                                            s_check = (m_siswaabsen_mapel.Is_Cat20.IndexOf("__") >= 1 ? "1" : (m_siswaabsen_mapel.Is_Cat20.Trim() != "" ? "1" : "1"));
                                                                            s_waktu_update = (m_siswaabsen_mapel.Is_Cat11_Time != DateTime.MinValue ? m_siswaabsen_mapel.Is_Cat20_Time.ToString("yyyy-MM-dd HH:mm:ss") : "");
                                                                            s_keterangan = m_siswaabsen_mapel.Is_Cat20_Keterangan;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            html_txt_kedisiplinan +=
                                                                        "<div style=\"" + (s_check == "0" ? "" : "display: none") + "\" class=\"row\" id=\"" + chk_id.Replace("chk", "div") + "\">" +
                                                                            "<div class=\"col-xs-3\" style=\"padding-top: 10px;\">" +
                                                                                "Ket. " +
                                                                                GetKalimatNegatif(m_kedisiplinan.Kode.ToString(), m_kedisiplinan.Keterangan.Trim()) +
                                                                            "</div>" +
                                                                            "<div class=\"col-xs-9\">" +
                                                                                "<input " +
                                                                                    "class=\"text-input\" " +
                                                                                    "style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\" " +
                                                                                    "name=\"txt_keterangan_kedisplinan[]\" " +
                                                                                    "value=\"" + s_keterangan.Replace("\"", "&#8222;") + "\" " +
                                                                                    "placeholder=\"Keterangan...\" " +
                                                                                    "class=\"access-hide\" " +
                                                                                    "id=\"" + chk_id.Replace("chk", "txt") + "\" " +
                                                                                    "lang=\"\" " +
                                                                                    "list=\"dl_" + m_kedisiplinan.Kode.ToString().Replace("-", "_") + "\"" +
                                                                                    " />" +
                                                                            "</div>" +
                                                                        "</div>";

                                                            string s_lang = kode_siswa + "~" +
                                                                            m_kedisiplinan.Alias + "~" +
                                                                            s_check + "~" +
                                                                            s_waktu_update;

                                                            html_chk_kedisiplinan +=
                                                                        (
                                                                            id_kedisiplinan % 2 == 1 ||
                                                                            id_kedisiplinan == 1
                                                                            ? (
                                                                                id_kedisiplinan % 2 == 1 &&
                                                                                id_kedisiplinan != 1
                                                                                ? "</div>"
                                                                                : ""
                                                                              ) +
                                                                              "<div class=\"row\" style=\"margin-right: 0px; margin-left: 0px;\">"
                                                                            : ""
                                                                        ) + "<div class=\"col-xs-6\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                                                "<div class=\"checkbox checkbox-adv\">" +
                                                                                    "<label for=\"" + chk_id + "\">" +
                                                                                        "<input  " + (s_check == "1" ? "checked=\"checked\" " : "") +
                                                                                                "name=\"chk_pilih_kedisplinan[]\" " +
                                                                                                "value=\"" + kode_siswa + "\" " +
                                                                                                "class=\"access-hide\" " +
                                                                                                "id=\"" + chk_id + "\" " +
                                                                                                "lang=\"" + s_lang + "\" " +
                                                                                                "onchange=\"SetCheckKedisiplinan(this.id); " +
                                                                                                           "ShowTextboxKeterangan(this.id, !this.checked);\" " +
                                                                                                "type=\"checkbox\" />" +
                                                                                        "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                                                        "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                                                                            m_kedisiplinan.Keterangan.Trim() +
                                                                                        "</span>" +
                                                                                    "</label>" +
                                                                                "</div>" +
                                                                            "</div>";

                                                            id_kedisiplinan++;

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (html_chk_kedisiplinan.Trim() != "")
            {
                html_chk = "<div class=\"row\">" +
                                html_chk_kedisiplinan +
                                "</div>" +
                           "</div>";
            }
            html_txt = "<div>" +
                            html_txt_kedisiplinan +
                       "</div>";

            return new HTMLInputKedisiplinan
            {
                CheckboxInput = html_chk,
                TextboxKeteranganInput = html_txt
            }; 
        }

        protected string GetHTMLTextBoxKeteranganKedisiplinan(
                string chk_id_hadir,
                string chk_id_sakit,
                string chk_id_izin,
                string chk_id_alpa,
                bool show_ket_sakit,
                bool show_ket_izin,
                bool show_ket_alpa,
                SiswaAbsenMapel m_absen_mapel,
                SiswaAbsen m_absen,
                string tipe_absen_0_walas_1_mapel,
                string tahun_ajaran,
                DateTime tanggal,
                string kode_siswa
            )
        {
            //kedisiplinan setup
            KedisiplinanSetup m_kedisiplinan_setup = DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(
                    tahun_ajaran,
                    Libs.GetSemesterByTanggal(tanggal).ToString(),
                    QS.GetUnit(),
                    QS.GetLevel()
                ).FirstOrDefault();
            int id_kategori = 0;
            if (m_kedisiplinan_setup != null)
            {
                if (m_kedisiplinan_setup.TahunAjaran != null)
                {
                    foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                    {
                        if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                        {
                            id_kategori++;
                        }
                    }
                }
            }


            string html = "<div class=\"row\" style=\"margin-top: 10px;\">" +
                            "<div class=\"col-xs-" + (id_kategori > 0 ? "12" : "12") + "\" " +
                                 "style=\"" + (id_kategori > 0 ? "" : "padding-left: 45px;") + "\">" +

                                //keterangan hadir
                                "<div style=\"display: none;\" class=\"row\" id=\"" + chk_id_hadir.Replace("chk", "div") + "\">" +
                                    "<div class=\"col-xs-3\" style=\"padding-top: 10px;\">" +
                                        "Hadir" +
                                    "</div>" +
                                    "<div class=\"col-xs-9\">" +
                                        "<input " +
                                            "class=\"text-input\" " +
                                            "style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\" " +
                                            "name=\"txt_keterangan_kedisplinan[]\" " +
                                            "value=\"\" " +
                                            "placeholder=\"Keterangan...\" " +
                                            "class=\"access-hide\" " +
                                            "id=\"" + chk_id_hadir.Replace("chk", "txt") + "\" " +
                                            "lang=\"\" " +
                                            " />" +
                                    "</div>" +
                                "</div>" +

                                //keterangan sakit
                                "<div style=\"" + (show_ket_sakit ? "" : "display: none") + "\" class=\"row\" id=\"" + chk_id_sakit.Replace("chk", "div") + "\">" +
                                    "<div class=\"col-xs-3\" style=\"padding-top: 10px; \">" +
                                        "Keterangan Sakit" +
                                    "</div>" +
                                    "<div class=\"col-xs-9\">" +
                                        "<input " +
                                            "class=\"text-input\" " +
                                            "style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\" " +
                                            "name=\"txt_keterangan_kedisplinan[]\" " +
                                            "value=\"" +
                                                (
                                                    m_absen_mapel != null
                                                    ? m_absen_mapel.Is_Sakit_Keterangan.Replace("\"", "&#8222;")
                                                    : (
                                                        m_absen != null
                                                        ? m_absen.Is_Sakit_Keterangan.Replace("\"", "&#8222;")
                                                        : ""
                                                      )
                                                ) +
                                                "\" " +
                                            "placeholder=\"Keterangan...\" " +
                                            "class=\"access-hide\" " +
                                            "id=\"" + chk_id_sakit.Replace("chk", "txt") + "\" " +
                                            "lang=\"\" " +
                                            "list = \"dl_sakit\" " +
                                            " />" +
                                    "</div>" +
                                "</div>" +

                                //keterangan izin
                                "<div style=\"" + (show_ket_izin ? "" : "display: none") + "\" class=\"row\" id=\"" + chk_id_izin.Replace("chk", "div") + "\">" +
                                    "<div class=\"col-xs-3\" style=\"padding-top: 10px; \">" +
                                        "Keterangan Izin" +
                                    "</div>" +
                                    "<div class=\"col-xs-9\">" +
                                        "<input " +
                                            "class=\"text-input\" " +
                                            "style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\" " +
                                            "name=\"txt_keterangan_kedisplinan[]\" " +
                                            "value=\"" +
                                                (
                                                    m_absen_mapel != null
                                                    ? m_absen_mapel.Is_Izin_Keterangan.Replace("\"", "&#8222;")
                                                    : (
                                                        m_absen != null
                                                        ? m_absen.Is_Izin_Keterangan.Replace("\"", "&#8222;")
                                                        : ""
                                                      )
                                                ) +
                                                "\" " +
                                            "placeholder=\"Keterangan...\" " +
                                            "class=\"access-hide\" " +
                                            "id=\"" + chk_id_izin.Replace("chk", "txt") + "\" " +
                                            "lang=\"\" " +
                                            "list = \"dl_izin\" " +
                                            " />" +
                                    "</div>" +
                                "</div>" +

                                //keterangan alpa
                                "<div style=\"" + (show_ket_alpa ? "" : "display: none") + "\" class=\"row\" id=\"" + chk_id_alpa.Replace("chk", "div") + "\">" +
                                    "<div class=\"col-xs-3\" style=\"padding-top: 10px; \">" +
                                        "Keterangan Alpa" +
                                    "</div>" +
                                    "<div class=\"col-xs-9\">" +
                                        "<input " +
                                            "class=\"text-input\" " +
                                            "style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\" " +
                                            "name=\"txt_keterangan_kedisplinan[]\" " +
                                            "value=\"" +
                                                (
                                                    m_absen_mapel != null
                                                    ? m_absen_mapel.Is_Alpa_Keterangan.Replace("\"", "&#8222;")
                                                    : (
                                                        m_absen != null
                                                        ? m_absen.Is_Alpa_Keterangan.Replace("\"", "&#8222;")
                                                        : ""
                                                      )
                                                ) +
                                                "\" " +
                                            "placeholder=\"Keterangan...\" " +
                                            "class=\"access-hide\" " +
                                            "id=\"" + chk_id_alpa.Replace("chk", "txt") + "\" " +
                                            "lang=\"\" " +
                                            "list = \"dl_alpa\" " +
                                            " />" +
                                    "</div>" +
                                "</div>" +

                            "</div>";
            html +=

                            (
                                id_kategori > 0
                                ? "<div " +
                                    "style=\"" +
                                        (
                                            m_absen_mapel != null
                                            ? (
                                                (
                                                    m_absen_mapel.Is_Hadir.IndexOf("__") < 0 ||
                                                    m_absen_mapel.Absen.Trim() == "H"
                                                )
                                                ? ""
                                                : "display: none;"
                                              )
                                            : (
                                                m_absen != null
                                                ? (
                                                    (
                                                        m_absen.Is_Hadir.IndexOf("__") < 0 ||
                                                        m_absen.Absen.Trim() == "H"
                                                    )
                                                    ? ""
                                                    : "display: none;"
                                                  )
                                                : ""
                                              )
                                        ) +
                                    "\" " +
                                    "id=\"div_kedisiplinan_" + kode_siswa.Replace("-", "_") + "_2\" class=\"col-xs-12\">" +
                                    KEY_KET_KEDISIPLINAN +
                                  "</div>"
                                : ""
                            ) +
                         "</div>";
            return html;
        }


        protected void ListDistinctKeterangan(string tahun_ajaran, DateTime tanggal)
        {
            string html_data_list = "";
            string html_option = "";

            if (QS.GetMapel() != "")
            {
                //list sakit
                html_option = "";
                var lst_sakit = DAO_SiswaAbsenMapel.GetDistinctKeteranganKategori_Entity("Is_Sakit", "SKT", "Is_Sakit_Keterangan");
                foreach (var item in lst_sakit)
                {
                    html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                }
                html_data_list += "<datalist id=\"dl_sakit\">" +
                                    html_option +
                                  "</datalist>";

                //list izin
                html_option = "";
                var lst_izin = DAO_SiswaAbsenMapel.GetDistinctKeteranganKategori_Entity("Is_Izin", "IZN", "Is_Izin_Keterangan");
                foreach (var item in lst_izin)
                {
                    html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                }
                html_data_list += "<datalist id=\"dl_izin\">" +
                                    html_option +
                                  "</datalist>";

                //list izin
                html_option = "";
                var lst_alpa = DAO_SiswaAbsenMapel.GetDistinctKeteranganKategori_Entity("Is_Alpa", "APA", "Is_Alpa_Keterangan");
                foreach (var item in lst_alpa)
                {
                    html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                }
                html_data_list += "<datalist id=\"dl_alpa\">" +
                                    html_option +
                                  "</datalist>";
            }
            else
            {
                //list sakit
                html_option = "";
                var lst_sakit = DAO_SiswaAbsen.GetDistinctKeteranganKategori_Entity("Is_Sakit", "SKT", "Is_Sakit_Keterangan");
                foreach (var item in lst_sakit)
                {
                    html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                }
                html_data_list += "<datalist id=\"dl_sakit\">" +
                                    html_option +
                                  "</datalist>";

                //list izin
                html_option = "";
                var lst_izin = DAO_SiswaAbsen.GetDistinctKeteranganKategori_Entity("Is_Izin", "IZN", "Is_Izin_Keterangan");
                foreach (var item in lst_izin)
                {
                    html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                }
                html_data_list += "<datalist id=\"dl_izin\">" +
                                    html_option +
                                  "</datalist>";

                //list izin
                html_option = "";
                var lst_alpa = DAO_SiswaAbsen.GetDistinctKeteranganKategori_Entity("Is_Alpa", "APA", "Is_Alpa_Keterangan");
                foreach (var item in lst_alpa)
                {
                    html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                }
                html_data_list += "<datalist id=\"dl_alpa\">" +
                                    html_option +
                                  "</datalist>";
            }


            //kedisiplinan setup
            KedisiplinanSetup m_kedisiplinan_setup = DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(
                    tahun_ajaran,
                    Libs.GetSemesterByTanggal(tanggal).ToString(),
                    QS.GetUnit(),
                    QS.GetLevel()
                ).FirstOrDefault();

            if (m_kedisiplinan_setup != null)
            {
                if (m_kedisiplinan_setup.TahunAjaran != null)
                {
                    int id_kategori = 1;
                    foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                    {
                        if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                        {
                            var val = prop.GetValue(m_kedisiplinan_setup, null);
                            if (val != null)
                            {
                                if (val.ToString() != "")
                                {
                                    Kedisiplinan m_kedisiplinan = DAO_Kedisiplinan.GetByID_Entity(
                                                val.ToString()
                                            );
                                    if (m_kedisiplinan != null)
                                    {
                                        if (m_kedisiplinan.Keterangan != null)
                                        {

                                            //list kategori
                                            html_option = "";
                                            List<string> lst_ket_kategori = new List<string>();
                                            if (QS.GetMapel() != "")
                                            {
                                                lst_ket_kategori = DAO_SiswaAbsenMapel.GetDistinctKeteranganKategori_Entity(
                                                                        "Is_Cat" + (id_kategori < 10 ? "0" : "") + id_kategori.ToString(),
                                                                        "__" + m_kedisiplinan.Alias,
                                                                        "Is_Cat" + (id_kategori < 10 ? "0" : "") + id_kategori.ToString() + "_Keterangan"
                                                                    );
                                            }
                                            else if (QS.GetMapel() == "")
                                            {
                                                lst_ket_kategori = DAO_SiswaAbsen.GetDistinctKeteranganKategori_Entity(
                                                                        "Is_Cat" + (id_kategori < 10 ? "0" : "") + id_kategori.ToString(),
                                                                        "__" + m_kedisiplinan.Alias,
                                                                        "Is_Cat" + (id_kategori < 10 ? "0" : "") + id_kategori.ToString() + "_Keterangan"
                                                                    );
                                            }

                                            foreach (var item in lst_ket_kategori)
                                            {
                                                html_option += "<option value=\"" + item + "\" label=\"" + item + "\"></option>";
                                            }
                                            html_data_list += "<datalist id=\"dl_" + m_kedisiplinan.Kode.ToString().Replace("-", "_") + "\">" +
                                                                html_option +
                                                              "</datalist>";
                                            id_kategori++;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ltrDataList.Text = html_data_list;
        }

        protected void ListAbsenSiswa(string lini_masa = "")
        {
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = QS.GetLevel();
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");

            DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAbsen.Text);
            DateTime dt_tanggal_absen = DateTime.Now;

            //get linimasa
            Guid kode_linimasa = Guid.NewGuid();
            if (lini_masa.Trim() == "")
            {
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetAllByTanggalByJenisByTahunAjaranByKelasDetByKeterangan_Entity(
                            dt_tanggal,
                            (
                                QS.GetMapel() != ""
                                ? Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL
                                : Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN
                            ),
                            tahun_ajaran,
                            rel_kelas_det,
                            Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false)
                        ).FirstOrDefault();
                if (m_linimasa != null)
                {
                    if (m_linimasa.Jenis != null)
                    {
                        kode_linimasa = m_linimasa.Kode;
                        dt_tanggal_absen = m_linimasa.Tanggal;
                        txtKeteranganAbsen.Text = m_linimasa.ACT_KETERANGAN;
                    }
                }
            }
            else
            {
                kode_linimasa = new Guid(lini_masa);
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(kode_linimasa.ToString());
                bool ada_linimasa = false;
                if (m_linimasa != null)
                {
                    if (m_linimasa.Jenis != null)
                    {
                        dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(m_linimasa.Keterangan);
                        dt_tanggal_absen = m_linimasa.Tanggal;
                        txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false);
                        txtKeteranganAbsen.Text = m_linimasa.ACT_KETERANGAN;
                        ada_linimasa = true;
                    }
                }
                if (!ada_linimasa)
                {
                    txtKeyAction.Value = JenisAction.DataTidakBisaDibuka.ToString();
                    return;
                }
            }
            txtKodeLinimasa.Value = kode_linimasa.ToString();
            //end get linimasa

            ltrListSiswaAbsen.Text = "";
            ListDistinctKeterangan(tahun_ajaran, dt_tanggal);

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            bool b_mapel_pilihan = false;
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());
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
                                                            Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
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
                                    QS.GetTahunAjaran(),
                                    Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                    QS.GetMapel(),
                                    QS.GetKelas()
                                );
                            }
                            else
                            {
                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() == "")
                                {

                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        QS.GetTahunAjaran(),
                                        Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                    );

                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() != "")
                                {
                                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                    {
                                        lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                                QS.GetMapel(),
                                                QS.GetLevel(),
                                                QS.GetKelas()
                                            );
                                    }
                                    else
                                    {
                                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            QS.GetTahunAjaran(),
                                            Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                        );

                                        if (lst_siswa.Count == 0)
                                        {
                                            lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                            );
                                        }
                                    }
                                }
                                else
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        QS.GetTahunAjaran(),
                                        Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                    );
                                }
                            }

                            int id = 1;

                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                      "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                string chk_id_hadir = "chk_hadir_" + m_siswa.Kode.ToString().Replace("-", "_");
                                string chk_id_sakit = "chk_sakit_" + m_siswa.Kode.ToString().Replace("-", "_");
                                string chk_id_izin = "chk_izin_" + m_siswa.Kode.ToString().Replace("-", "_");
                                string chk_id_alpa = "chk_alpa_" + m_siswa.Kode.ToString().Replace("-", "_");

                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                                {
                                    if (rel_mapel.Trim() == "")
                                    {
                                        List<SiswaAbsen> lst_siswaabsen = new List<SiswaAbsen>();

                                        lst_siswaabsen = DAO_SiswaAbsen.GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                dt_tanggal
                                            );

                                        SiswaAbsen m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_keterangan = "";
                                        string s_kejadian = "";
                                        string s_butir_sikap = "";
                                        string s_butir_sikap_lain = "";
                                        string s_sikap = "";
                                        string s_tindaklanjut = "";
                                        bool ada_data = false;
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                s_kejadian = m_absen.Kejadian.Replace("\"", "");
                                                s_butir_sikap = m_absen.ButirSikap.Replace("\"", "");
                                                s_butir_sikap_lain = m_absen.ButirSikapLain.Replace("\"", "");
                                                s_sikap = m_absen.Sikap.Replace("\"", "");
                                                s_tindaklanjut = m_absen.TindakLanjut.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                                ada_data = true;
                                            }
                                        }

                                        var html_input_kedisiplinan =
                                            GetHTMLInputKedisiplinan(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                                m_siswa.Kode.ToString(),
                                                m_absen,
                                                null,
                                                "0",
                                                lini_masa,
                                                ada_data
                                            );

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\" " + (!ada_data && lini_masa.Trim() != "" ? " style=\"border-left-style: solid; border-left-width: 8px; border-left-color: red;\" " : "") + ">" +
                                                                    "<div class=\"col-xs-12\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: black; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: black; font-weight: bold;\">" +
                                                                                        //Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        m_siswa.Nama.Trim().ToUpper() +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +

                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-" + (html_input_kedisiplinan.CheckboxInput.Trim() == "" ? "12" : "6") + "\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small; font-weight: bold; color: green;\">Kehadiran</div>" +

                                                                                            GetHTMLCheckboxKehadiran(
                                                                                                    m_siswa.Kode.ToString(),
                                                                                                    chk_id_hadir,
                                                                                                    chk_id_sakit,
                                                                                                    chk_id_izin,
                                                                                                    chk_id_alpa,
                                                                                                    m_absen,
                                                                                                    null,
                                                                                                    "0",
                                                                                                    lini_masa,
                                                                                                    ada_data
                                                                                                ) +

                                                                                        "</div>" +

                                                                                        (
                                                                                            html_input_kedisiplinan.CheckboxInput.Trim() != ""
                                                                                            ? "<div id=\"div_kedisiplinan_" + m_siswa.Kode.ToString().Replace("-", "_") + "_1\" " +
                                                                                                    "class=\"col-xs-6\" " +
                                                                                                    "style=\"vertical-align: middle; " +
                                                                                                            (
                                                                                                                m_absen != null
                                                                                                                ? (
                                                                                                                    (
                                                                                                                        m_absen.Is_Hadir.IndexOf("__") < 0 ||
                                                                                                                        m_absen.Absen.Trim() == "H"
                                                                                                                    )
                                                                                                                    ? ""
                                                                                                                    : "display: none;"
                                                                                                                  )
                                                                                                                : ""
                                                                                                            ) +
                                                                                                           "\"" +
                                                                                               ">" +
                                                                                                    "<div style=\"font-size: small; font-weight: bold; color: #0063c3;\">Kedisiplinan</div>" +

                                                                                                    html_input_kedisiplinan.CheckboxInput +

                                                                                              "</div>"
                                                                                            : ""
                                                                                        ) +

                                                                                      "</div>" +

                                                                                      (
                                                                                        m_absen != null
                                                                                        ? GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            (
                                                                                                m_absen.Is_Sakit.IndexOf("__") >= 0 
                                                                                                ? false 
                                                                                                : (
                                                                                                    m_absen.Is_Sakit.Trim() != "" 
                                                                                                    ? true 
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "S"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Izin.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Izin.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "I"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Alpa.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Alpa.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "A"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            null,
                                                                                            m_absen,
                                                                                            "0",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                            ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                        : GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            false,
                                                                                            false,
                                                                                            false,
                                                                                            null,
                                                                                            m_absen,
                                                                                            "0",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                            ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                      ) +

                                                                                      "<div class=\"row\" style=\"display: none;\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"vertical-align: middle;\">" +
                                                                                            "<input placeholder=\"Ketikan keterangan disini\" onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +

                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 0px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    else
                                    {
                                        //absen mapel
                                        List <SiswaAbsenMapel> lst_siswaabsen = DAO_SiswaAbsenMapel.GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                rel_mapel,
                                                dt_tanggal
                                            );

                                        SiswaAbsenMapel m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_keterangan = "";
                                        string s_kejadian = "";
                                        string s_butir_sikap = "";
                                        string s_butir_sikap_lain = "";
                                        string s_sikap = "";
                                        string s_tindaklanjut = "";
                                        bool ada_data = false;
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                s_kejadian = m_absen.Kejadian.Replace("\"", "");
                                                s_butir_sikap = m_absen.ButirSikap.Replace("\"", "");
                                                s_butir_sikap_lain = m_absen.ButirSikapLain.Replace("\"", "");
                                                s_sikap = m_absen.Sikap.Replace("\"", "");
                                                s_tindaklanjut = m_absen.TindakLanjut.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                                ada_data = true;
                                                SelectDropdownJam(m_absen.JamAkhir, m_absen.JamAkhir);
                                            }
                                        }

                                        var html_input_kedisiplinan =
                                            GetHTMLInputKedisiplinan(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                                m_siswa.Kode.ToString(),
                                                null,
                                                m_absen,
                                                "1",
                                                lini_masa,
                                                ada_data
                                            );

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\" " + (!ada_data && lini_masa.Trim() != "" ? " style=\"border-left-style: solid; border-left-width: 8px; border-left-color: red;\" " : "") + ">" +
                                                                    "<div class=\"col-xs-12\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: black; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: black; font-weight: bold;\">" +
                                                                                        //Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        m_siswa.Nama.Trim().ToUpper() +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +

                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-" + (html_input_kedisiplinan.CheckboxInput.Trim() == "" ? "12" : "6") + "\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small; font-weight: bold; color: green;\">Kehadiran</div>" +

                                                                                            GetHTMLCheckboxKehadiran(
                                                                                                    m_siswa.Kode.ToString(),
                                                                                                    chk_id_hadir,
                                                                                                    chk_id_sakit,
                                                                                                    chk_id_izin,
                                                                                                    chk_id_alpa,
                                                                                                    null,
                                                                                                    m_absen,
                                                                                                    "1",
                                                                                                    lini_masa,
                                                                                                    ada_data
                                                                                                ) +

                                                                                        "</div>" +
                                                                                        (
                                                                                            html_input_kedisiplinan.CheckboxInput.Trim() != ""
                                                                                            ? "<div id=\"div_kedisiplinan_" + m_siswa.Kode.ToString().Replace("-", "_") + "_1\" " +
                                                                                                   "class=\"col-xs-6\" " +
                                                                                                   "style=\"vertical-align: middle; " +
                                                                                                            (
                                                                                                                m_absen != null
                                                                                                                ? (
                                                                                                                    (
                                                                                                                        m_absen.Is_Hadir.IndexOf("__") < 0 ||
                                                                                                                        m_absen.Absen.Trim() == "H"
                                                                                                                    )
                                                                                                                    ? ""
                                                                                                                    : "display: none;"
                                                                                                                  )
                                                                                                                : ""
                                                                                                            ) +
                                                                                                           "\"" +
                                                                                              ">" +
                                                                                                    "<div style=\"font-size: small; font-weight: bold; color: #0063c3;\">Kedisiplinan</div>" +

                                                                                                    html_input_kedisiplinan.CheckboxInput +

                                                                                              "</div>"
                                                                                            : ""
                                                                                        ) +
                                                                                      "</div>" +

                                                                                      (
                                                                                        m_absen != null
                                                                                        ? GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            (
                                                                                                m_absen.Is_Sakit.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Sakit.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "S"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Izin.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Izin.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "I"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Alpa.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Alpa.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "A"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ), 
                                                                                            m_absen,
                                                                                            null,
                                                                                            "1",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                          ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                        : GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            false,
                                                                                            false,
                                                                                            false,
                                                                                            m_absen,
                                                                                            null,
                                                                                            "1",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                          ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                      ) +

                                                                                      "<div class=\"row\" style=\"display: none;\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"vertical-align: middle;\">" +
                                                                                            "<input placeholder=\"Ketikan keterangan disini\" onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +

                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 0px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    id++;

                                }
                                else
                                {

                                    if (rel_mapel.Trim() == "")
                                    {
                                        List<SiswaAbsen> lst_siswaabsen = new List<SiswaAbsen>();

                                        lst_siswaabsen = DAO_SiswaAbsen.GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                dt_tanggal
                                            );

                                        SiswaAbsen m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_keterangan = "";
                                        bool ada_data = false;
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                                ada_data = true;
                                            }
                                        }

                                        var html_input_kedisiplinan =
                                            GetHTMLInputKedisiplinan(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                                m_siswa.Kode.ToString(),
                                                m_absen,
                                                null,
                                                "0",
                                                lini_masa,
                                                ada_data
                                            );

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\" " + (!ada_data && lini_masa.Trim() != "" ? " style=\"border-left-style: solid; border-left-width: 8px; border-left-color: red;\" " : "") + ">" +
                                                                    "<div class=\"col-xs-12\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: black; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: black; font-weight: bold;\">" +
                                                                                        //Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        m_siswa.Nama.Trim().ToUpper() +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +

                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-" + (html_input_kedisiplinan.CheckboxInput.Trim() == "" ? "12" : "6") + "\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small; font-weight: bold; color: green;\">Kehadiran</div>" +

                                                                                            GetHTMLCheckboxKehadiran(
                                                                                                    m_siswa.Kode.ToString(),
                                                                                                    chk_id_hadir,
                                                                                                    chk_id_sakit,
                                                                                                    chk_id_izin,
                                                                                                    chk_id_alpa,
                                                                                                    m_absen,
                                                                                                    null,
                                                                                                    "0",
                                                                                                    lini_masa,
                                                                                                    ada_data
                                                                                                ) +

                                                                                        "</div>" +

                                                                                        (
                                                                                            html_input_kedisiplinan.CheckboxInput.Trim() != ""
                                                                                            ? "<div id=\"div_kedisiplinan_" + m_siswa.Kode.ToString().Replace("-", "_") + "_1\" " +
                                                                                                   "class=\"col-xs-6\" " +
                                                                                                   "style=\"vertical-align: middle; " +
                                                                                                            (
                                                                                                                m_absen != null
                                                                                                                ? (
                                                                                                                    (
                                                                                                                        m_absen.Is_Hadir.IndexOf("__") < 0 ||
                                                                                                                        m_absen.Absen.Trim() == "H"
                                                                                                                    )
                                                                                                                    ? ""
                                                                                                                    : "display: none;"
                                                                                                                  )
                                                                                                                : ""
                                                                                                            ) +
                                                                                                           "\"" +
                                                                                              ">" +
                                                                                                    "<div style=\"font-size: small; font-weight: bold; color: #0063c3;\">Kedisiplinan</div>" +

                                                                                                    html_input_kedisiplinan.CheckboxInput +

                                                                                              "</div>"
                                                                                            : ""
                                                                                        ) +

                                                                                      "</div>" +

                                                                                      (
                                                                                        m_absen != null
                                                                                        ? GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            (
                                                                                                m_absen.Is_Sakit.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Sakit.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "S"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Izin.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Izin.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "I"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Alpa.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Alpa.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "A"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            null,
                                                                                            m_absen,
                                                                                            "0",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                            ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                        : GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            false,
                                                                                            false,
                                                                                            false,
                                                                                            null,
                                                                                            m_absen,
                                                                                            "0",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                            ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                      ) +

                                                                                      "<div class=\"row\" style=\"display: none;\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"vertical-align: middle;\">" +
                                                                                            "<input placeholder=\"Ketikan keterangan disini\" onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +

                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 0px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    else
                                    {
                                        //absen mapel
                                        List<SiswaAbsenMapel> lst_siswaabsen = DAO_SiswaAbsenMapel.GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                rel_mapel,
                                                dt_tanggal
                                            );

                                        SiswaAbsenMapel m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_keterangan = "";
                                        bool ada_data = false;
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                                ada_data = true;
                                            }
                                        }

                                        var html_input_kedisiplinan =
                                            GetHTMLInputKedisiplinan(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                                m_siswa.Kode.ToString(),
                                                null,
                                                m_absen,
                                                "1",
                                                lini_masa,
                                                ada_data
                                            );

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\" " + (!ada_data && lini_masa.Trim() != "" ? " style=\"border-left-style: solid; border-left-width: 8px; border-left-color: red;\" " : "") + ">" +
                                                                    "<div class=\"col-xs-12\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: black; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: black; font-weight: bold;\">" +
                                                                                        //Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        m_siswa.Nama.Trim().ToUpper() +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: black;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +

                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-" + (html_input_kedisiplinan.CheckboxInput.Trim() == "" ? "12" : "6") + "\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small; font-weight: bold; color: green;\">Kehadiran</div>" +

                                                                                            GetHTMLCheckboxKehadiran(
                                                                                                    m_siswa.Kode.ToString(),
                                                                                                    chk_id_hadir,
                                                                                                    chk_id_sakit,
                                                                                                    chk_id_izin,
                                                                                                    chk_id_alpa,
                                                                                                    null,
                                                                                                    m_absen,
                                                                                                    "1",
                                                                                                    lini_masa,
                                                                                                    ada_data
                                                                                                ) +

                                                                                        "</div>" +
                                                                                        (
                                                                                            html_input_kedisiplinan.CheckboxInput.Trim() != ""
                                                                                            ? "<div id=\"div_kedisiplinan_" + m_siswa.Kode.ToString().Replace("-", "_") + "_1\" " +
                                                                                                   "class=\"col-xs-6\" " +
                                                                                                   "style=\"vertical-align: middle; " +
                                                                                                            (
                                                                                                                m_absen != null
                                                                                                                ? (
                                                                                                                    (
                                                                                                                        m_absen.Is_Hadir.IndexOf("__") < 0 ||
                                                                                                                        m_absen.Absen.Trim() == "H"
                                                                                                                    )
                                                                                                                    ? ""
                                                                                                                    : "display: none;"
                                                                                                                  )
                                                                                                                : ""
                                                                                                            ) +
                                                                                                           "\"" +
                                                                                              ">" +
                                                                                                    "<div style=\"font-size: small; font-weight: bold; color: #0063c3;\">Kedisiplinan</div>" +

                                                                                                    html_input_kedisiplinan.CheckboxInput +

                                                                                              "</div>"
                                                                                            : ""
                                                                                        ) +
                                                                                      "</div>" +

                                                                                      (
                                                                                        m_absen != null
                                                                                        ? GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            (
                                                                                                m_absen.Is_Sakit.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Sakit.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "S"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Izin.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Izin.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "I"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ),
                                                                                            (
                                                                                                m_absen.Is_Alpa.IndexOf("__") >= 0
                                                                                                ? false
                                                                                                : (
                                                                                                    m_absen.Is_Alpa.Trim() != ""
                                                                                                    ? true
                                                                                                    : (
                                                                                                        m_absen.Absen.Trim() != ""
                                                                                                        ? (
                                                                                                            m_absen.Absen.Substring(0, 1).ToUpper().Trim() == "A"
                                                                                                            ? true
                                                                                                            : false
                                                                                                          )
                                                                                                        : false
                                                                                                      )
                                                                                                  )
                                                                                            ), 
                                                                                            m_absen,
                                                                                            null,
                                                                                            "1",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                          ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                        : GetHTMLTextBoxKeteranganKedisiplinan(
                                                                                            chk_id_hadir,
                                                                                            chk_id_sakit,
                                                                                            chk_id_izin,
                                                                                            chk_id_alpa,
                                                                                            false,
                                                                                            false,
                                                                                            false,
                                                                                            m_absen,
                                                                                            null,
                                                                                            "1",
                                                                                            tahun_ajaran,
                                                                                            dt_tanggal,
                                                                                            m_siswa.Kode.ToString()
                                                                                          ).Replace(KEY_KET_KEDISIPLINAN, html_input_kedisiplinan.TextboxKeteranganInput)
                                                                                      ) +

                                                                                      "<div class=\"row\" style=\"display: none;\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"vertical-align: middle;\">" +
                                                                                            "<input placeholder=\"Ketikan keterangan disini\" onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +

                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 0px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    id++;

                                }
                            }

                        }
                    }

                }
            }

            ltrListSiswaAbsen.Text = ltrListSiswaAbsen.Text.Replace(KEY_KET_KEDISIPLINAN, "");
        }

        protected void ListAbsenSiswa_V1(string lini_masa = "")
        {
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = QS.GetLevel();
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");

            DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAbsen.Text);
            DateTime dt_tanggal_absen = DateTime.Now;

            //get linimasa
            Guid kode_linimasa = Guid.NewGuid();
            if (lini_masa.Trim() == "")
            {
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetAllByTanggalByJenisByTahunAjaranByKelasDetByKeterangan_Entity(
                            dt_tanggal,
                            (
                                QS.GetMapel() != ""
                                ? Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL
                                : Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN
                            ),
                            tahun_ajaran,
                            rel_kelas_det,
                            Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false)
                        ).FirstOrDefault();
                if (m_linimasa != null)
                {
                    if (m_linimasa.Jenis != null)
                    {
                        kode_linimasa = m_linimasa.Kode;
                        dt_tanggal_absen = m_linimasa.Tanggal;
                        txtKeteranganAbsen.Text = m_linimasa.ACT_KETERANGAN;
                    }
                }
            }
            else
            {
                kode_linimasa = new Guid(lini_masa);
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(kode_linimasa.ToString());
                bool ada_linimasa = false;
                if (m_linimasa != null)
                {
                    if (m_linimasa.Jenis != null)
                    {
                        dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(m_linimasa.Keterangan);
                        dt_tanggal_absen = m_linimasa.Tanggal;
                        txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(dt_tanggal, false);
                        txtKeteranganAbsen.Text = m_linimasa.ACT_KETERANGAN;
                        ada_linimasa = true;
                    }
                }
                if (!ada_linimasa)
                {
                    txtKeyAction.Value = JenisAction.DataTidakBisaDibuka.ToString();
                    return;
                }
            }
            txtKodeLinimasa.Value = kode_linimasa.ToString();
            //end get linimasa

            ltrListSiswaAbsen.Text = "";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            bool b_mapel_pilihan = false;
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());
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
                                                            Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
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
                                    QS.GetTahunAjaran(),
                                    Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                    QS.GetMapel(),
                                    QS.GetKelas()
                                );
                            }
                            else
                            {
                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() == "")
                                {

                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        QS.GetTahunAjaran(),
                                        Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                    );

                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() != "")
                                {
                                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                    {
                                        lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString(),
                                                QS.GetMapel(),
                                                QS.GetLevel(),
                                                QS.GetKelas()
                                            );
                                    }
                                    else
                                    {
                                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            QS.GetTahunAjaran(),
                                            Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                        );

                                        if (lst_siswa.Count == 0)
                                        {
                                            lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                            );
                                        }
                                    }
                                }
                                else
                                {
                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        QS.GetTahunAjaran(),
                                        Libs.GetSemesterByTanggal(dt_tanggal_absen).ToString()
                                    );
                                }
                            }
                            
                            int id = 1;

                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                string s_panggilan =  "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                      "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                                {

                                    if (rel_mapel.Trim() == "")
                                    {
                                        List<SiswaAbsen> lst_siswaabsen = new List<SiswaAbsen>();

                                        lst_siswaabsen = DAO_SiswaAbsen.GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                dt_tanggal
                                            );

                                        SiswaAbsen m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_absen = "";
                                        string s_keterangan = "";
                                        string s_kejadian = "";
                                        string s_butir_sikap = "";
                                        string s_butir_sikap_lain = "";
                                        string s_sikap = "";
                                        string s_tindaklanjut = "";
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_absen = m_absen.Absen.Trim().ToUpper().Substring(0, 1);
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                s_kejadian = m_absen.Kejadian.Replace("\"", "");
                                                s_butir_sikap = m_absen.ButirSikap.Replace("\"", "");
                                                s_butir_sikap_lain = m_absen.ButirSikapLain.Replace("\"", "");
                                                s_sikap = m_absen.Sikap.Replace("\"", "");
                                                s_tindaklanjut = m_absen.TindakLanjut.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                            }
                                        }

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: black; font-weight: bold;\">" +
                                                                                        Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +

                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-4\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Kehadiran</div>" +
                                                                                            "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" name=\"cbo_absen[]\" title=\" Absensi \" class=\"text-input\" style=\"width: 100%; margin-top: 5px; margin-bottom: 5px; font-weight: bold;\">" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.HADIR + "</option>" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.TERLAMBAT + "</option>" +
                                                                                                (
                                                                                                    m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA
                                                                                                    ? "<option " + (s_absen == Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.DITUGASKAN + "</option>"
                                                                                                    : ""
                                                                                                ) +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.SAKIT + "</option>" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.IZIN + "</option>" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.ALPA + "</option>" +
                                                                                            "</select>" +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-8\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Keterangan</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +
                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-4\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Kejadian/Perilaku</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_kejadian + "\" name=\"txt_kejadian[]\" title=\" Kejadian/Perilaku \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-3\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Butir Sikap</div>" +
                                                                                            Libs.BUTIR_SIKAP.GetHTMLDropdownButirSikap("", "cbo_butir_sikap[]", s_butir_sikap) +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-5\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Butir Sikap Lainnya</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_butir_sikap_lain + "\" name=\"txt_butir_sikap_lain[]\" title=\" Sikap Lainnya \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +
                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-4\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Nilai Sikap</div>" +
                                                                                            Libs.BUTIR_SIKAP.GetHTMLDropdownSikap("", "cbo_sikap[]", s_sikap) +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-8\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Tindak Lanjut</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_tindaklanjut + "\" name=\"txt_tindak_lanjut[]\" title=\" Tindak Lanjut \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +

                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    else
                                    {
                                        //absen mapel
                                        List<SiswaAbsenMapel> lst_siswaabsen = DAO_SiswaAbsenMapel.GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                rel_mapel,
                                                dt_tanggal
                                            );

                                        SiswaAbsenMapel m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_absen = "";
                                        string s_keterangan = "";
                                        string s_kejadian = "";
                                        string s_butir_sikap = "";
                                        string s_butir_sikap_lain = "";
                                        string s_sikap = "";
                                        string s_tindaklanjut = "";
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_absen = m_absen.Absen.Trim().ToUpper().Substring(0, 1);
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                s_kejadian = m_absen.Kejadian.Replace("\"", "");
                                                s_butir_sikap = m_absen.ButirSikap.Replace("\"", "");
                                                s_butir_sikap_lain = m_absen.ButirSikapLain.Replace("\"", "");
                                                s_sikap = m_absen.Sikap.Replace("\"", "");
                                                s_tindaklanjut = m_absen.TindakLanjut.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                            }
                                        }

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                                                        Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +

                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-4\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Kehadiran</div>" +
                                                                                            "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" name=\"cbo_absen[]\" title=\" Absensi \" class=\"text-input\" style=\"width: 100%; margin-top: 5px; margin-bottom: 5px; font-weight: bold;\">" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.HADIR + "</option>" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.TERLAMBAT + "</option>" +
                                                                                                (
                                                                                                    m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA
                                                                                                    ? "<option " + (s_absen == Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.DITUGASKAN + "</option>"
                                                                                                    : ""
                                                                                                ) +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.SAKIT + "</option>" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.IZIN + "</option>" +
                                                                                                "<option " + (s_absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.ALPA + "</option>" +
                                                                                            "</select>" +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-8\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Keterangan</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +
                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-4\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Kejadian/Perilaku</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_kejadian + "\" name=\"txt_kejadian[]\" title=\" Kejadian/Perilaku \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-3\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Butir Sikap</div>" +
                                                                                            Libs.BUTIR_SIKAP.GetHTMLDropdownButirSikap("", "cbo_butir_sikap[]", s_butir_sikap) +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-5\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Butir Sikap Lainnya</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_butir_sikap_lain + "\" name=\"txt_butir_sikap_lain[]\" title=\" Sikap Lainnya \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +
                                                                                      "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-4\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Nilai Sikap</div>" +
                                                                                            Libs.BUTIR_SIKAP.GetHTMLDropdownSikap("", "cbo_sikap[]", s_sikap) +
                                                                                        "</div>" +
                                                                                        "<div class=\"col-xs-8\" style=\"vertical-align: middle;\">" +
                                                                                            "<div style=\"font-size: small;\">Tindak Lanjut</div>" +
                                                                                            "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_tindaklanjut + "\" name=\"txt_tindak_lanjut[]\" title=\" Tindak Lanjut \" class=\"text-input\" style=\"width: 100%; margin-top: 10px; font-weight: bold; margin-top: 5px; margin-bottom: 5px;\">" +
                                                                                        "</div>" +
                                                                                      "</div>" +

                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    id++;

                                }
                                else
                                {

                                    if (rel_mapel.Trim() == "")
                                    {
                                        List<SiswaAbsen> lst_siswaabsen = new List<SiswaAbsen>();

                                        lst_siswaabsen = DAO_SiswaAbsen.GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                dt_tanggal
                                            );

                                        SiswaAbsen m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_absen = "";
                                        string s_keterangan = "";
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_absen = m_absen.Absen.Trim().ToUpper().Substring(0, 1);
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                            }
                                        }

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-5\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                                                        Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                    "<div class=\"col-xs-2\" style=\"vertical-align: middle;\">" +
                                                                        "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" name=\"cbo_absen[]\" title=\" Absensi \" class=\"text-input\" style=\"width: 100%; margin-top: 10px;\">" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.HADIR + "</option>" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.TERLAMBAT + "</option>" +
                                                                            (
                                                                                m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA
                                                                                ? "<option " + (s_absen == Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.DITUGASKAN + "</option>"
                                                                                : ""
                                                                            ) +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.SAKIT + "</option>" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.IZIN + "</option>" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.ALPA + "</option>" +
                                                                        "</select>" +
                                                                    "</div>" +
                                                                    "<div class=\"col-xs-5\" style=\"vertical-align: middle;\">" +
                                                                        "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px;\">" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    else
                                    {
                                        //absen mapel
                                        List<SiswaAbsenMapel> lst_siswaabsen = DAO_SiswaAbsenMapel.GetAllBySekolahByKelasDetBySiswaByMapelByTanggal_Entity(
                                                m_kelas.Rel_Sekolah.ToString(),
                                                rel_kelas_det,
                                                m_siswa.Kode.ToString(),
                                                rel_mapel,
                                                dt_tanggal
                                            );

                                        SiswaAbsenMapel m_absen = (lst_siswaabsen.Count > 0 && lst_siswaabsen.FirstOrDefault() != null ? lst_siswaabsen.FirstOrDefault() : null);
                                        string s_absen = "";
                                        string s_keterangan = "";
                                        if (m_absen != null)
                                        {
                                            if (m_absen.Absen != null)
                                            {
                                                s_absen = m_absen.Absen.Trim().ToUpper().Substring(0, 1);
                                                s_keterangan = m_absen.Keterangan.Replace("\"", "");
                                                txtTanggalAbsenBuka.Text = Libs.GetTanggalIndonesiaFromDate(m_absen.Tanggal, false);
                                            }
                                        }

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrListSiswaAbsen.Text += "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-5\">" +
                                                                        "<table style=\"margin: 0px; width: 100%;\">" +
                                                                            "<tr>" +
                                                                                "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                                    id.ToString() +
                                                                                    "." +
                                                                                "</td>" +
                                                                                "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                                    "<input name=\"txt_siswa_absen[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                                    "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                        "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                                "</td>" +
                                                                                "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                                    (
                                                                                        m_siswa.NISSekolah.Trim() != ""
                                                                                        ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                                            m_siswa.NISSekolah +
                                                                                          "</span>" +
                                                                                          "<br />"
                                                                                        : ""
                                                                                    ) +
                                                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                                                        Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                        (
                                                                                            m_siswa.Panggilan.Trim() != ""
                                                                                            ? "<span style=\"font-weight: normal\">" +
                                                                                                "&nbsp;" + s_panggilan +
                                                                                              "</span>"
                                                                                            : ""
                                                                                        ) +
                                                                                    "</span>" +
                                                                                "</td>" +
                                                                            "</tr>" +
                                                                        "</table>" +
                                                                    "</div>" +
                                                                    "<div class=\"col-xs-2\" style=\"vertical-align: middle;\">" +
                                                                        "<select onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" name=\"cbo_absen[]\" title=\" Absensi \" class=\"text-input\" style=\"width: 100%; margin-top: 10px;\">" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.HADIR.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.HADIR + "</option>" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.TERLAMBAT + "</option>" +
                                                                            (
                                                                                m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA
                                                                                ? "<option " + (s_absen == Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.DITUGASKAN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.DITUGASKAN + "</option>"
                                                                                : ""
                                                                            ) +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.SAKIT + "</option>" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.IZIN.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.IZIN + "</option>" +
                                                                            "<option " + (s_absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) ? " selected " : "") + " value=\"" + Libs.JENIS_ABSENSI.ALPA.Substring(0, 1) + "\">" + Libs.JENIS_ABSENSI.ALPA + "</option>" +
                                                                        "</select>" +
                                                                    "</div>" +
                                                                    "<div class=\"col-xs-5\" style=\"vertical-align: middle;\">" +
                                                                        "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_keterangan + "\" name=\"txt_keterangan_absen[]\" title=\" Keterangan \" class=\"text-input\" style=\"width: 100%; margin-top: 10px;\">" +
                                                                    "</div>" +
                                                                  "</div>" +
                                                                  "<div class=\"row\">" +
                                                                    "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                                    "</div>" +
                                                                  "</div>";
                                    }
                                    id++;

                                }
                            }

                        }
                    }

                }
            }
        }

        protected void lnkOKBuatPengumuman_Click(object sender, EventArgs e)
        {

        }

        protected void lnkOKBuatTugas_Click(object sender, EventArgs e)
        {

        }

        protected void lnkTampilanData_Click(object sender, EventArgs e)
        {

        }

        protected void lnkPilihKelas_Click(object sender, EventArgs e)
        {

        }

        protected void btnDoShowAbsen_Click(object sender, EventArgs e)
        {
            if(Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAbsen.Text) > DateTime.Now)
            {
                txtKeyAction.Value = "Tanggal tidak valid, maksimal tanggal absen harus hari ini.";
                return;
            }
            //ListAbsenSiswa_V1();
            ListAbsenSiswa();
            txtKeteranganAbsen.Text = "";
            txtTanggalAbsenBuka.Text = txtTanggalAbsen.Text;            
            txtKeyAction.Value = JenisAction.DoShowInputAbsen.ToString();
        }

        protected void btnDoShowAbsenByLinimasa_Click(object sender, EventArgs e)
        {
            //ListAbsenSiswa_V1(txtKodeLinimasa.Value);
            ListAbsenSiswa(txtKodeLinimasa.Value);
            txtKeyAction.Value = JenisAction.DoShowInputAbsen.ToString();
        }

        protected void btnDoShowLinimasa_Click(object sender, EventArgs e)
        {
            ShowLinimasa();
        }

        public static string GetSSID()
        {
            return Libs.Encryptdata(Libs.LOGGED_USER_M.NoInduk);
        }

        protected void lnkOKHapusAbsen_Click(object sender, EventArgs e)
        {
            if (txtKodeLinimasa.Value.Trim() != "")
            {
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(txtKodeLinimasa.Value);
                if (m_linimasa != null)
                {
                    if (m_linimasa.TahunAjaran != null)
                    {
                        KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_linimasa.Rel_KelasDet.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                if (m_linimasa.Jenis == Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL)
                                {
                                    DAO_SiswaAbsenMapel.DeleteByLinimasa(m_linimasa.Kode.ToString(), Libs.LOGGED_USER_M.UserID);
                                }
                                else if (m_linimasa.Jenis == Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN)
                                {
                                    DAO_SiswaAbsen.DeleteByLinimasa(m_linimasa.Kode.ToString(), Libs.LOGGED_USER_M.UserID);
                                }
                                DAO_LinimasaKelas.Delete(m_linimasa.Kode.ToString(), Libs.LOGGED_USER_M.UserID);
                                ShowLinimasa();
                                txtKeyAction.Value = JenisAction.DoDelete.ToString();
                            }
                        }
                    }
                }
            }
        }

        protected void btnDoShowConfirmDeleteAbsen_Click(object sender, EventArgs e)
        {
            if (txtKodeLinimasa.Value.Trim() != "")
            {
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(txtKodeLinimasa.Value);
                if (m_linimasa != null)
                {
                    if (m_linimasa.TahunAjaran != null)
                    {
                        KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_linimasa.Rel_KelasDet.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                if (m_linimasa.Jenis == Libs.JENIS_LINIMASA.ABSEN_SISWA_MAPEL)
                                {
                                    ltrMsgConfirmHapusAbsen.Text = "Anda yakin akan menghapus data absensi mata pelajaran : <br />" +
                                                                   "Kelas : " +
                                                                   "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                                                   "&nbsp;" +
                                                                   "Tanggal : " +
                                                                   "<span style=\"font-weight: bold;\">" + m_linimasa.Keterangan + "</span>";
                                }
                                else if (m_linimasa.Jenis == Libs.JENIS_LINIMASA.ABSEN_SISWA_HARIAN)
                                {
                                    ltrMsgConfirmHapusAbsen.Text = "Anda yakin akan menghapus data absensi harian siswa : <br />" +
                                                                   "Kelas : " +
                                                                   "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                                                   "&nbsp;" +
                                                                   "Tanggal : " +
                                                                   "<span style=\"font-weight: bold;\">" + m_linimasa.Keterangan + "</span>";
                                }                                
                                txtKeyAction.Value = JenisAction.DoShowConfirmHapusAbsen.ToString();
                            }                            
                        }                        
                    }
                }                
            }            
        }

        protected void lnkOKShowDownloadAbsen_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowDownloadRekapAbsen.ToString();
        }

        protected void ShowListPeriodeAbsen()
        {
            List<PeriodeAbsen> lst_periode_absen = new List<PeriodeAbsen>();
            if (QS.GetMapel().Trim() != "")
            {
                lst_periode_absen = DAO_SiswaAbsenMapel.GetPeriodeBySekolahByKelasDetByTAByMapel_Entity(
                        QS.GetUnit(), QS.GetKelas(), QS.GetTahunAjaran(), QS.GetMapel()
                    );
            }
            else
            {
                lst_periode_absen = DAO_SiswaAbsen.GetPeriodeBySekolahByKelasDetByTA_Entity(
                        QS.GetUnit(), QS.GetKelas(), QS.GetTahunAjaran()
                    );
            }
            
            string s_periode = Libs.GetQueryString("p");
            int bulan = (s_periode.Length == 6 ? Libs.GetStringToInteger(s_periode.Substring(4, 2)) : 0);
            int tahun = (s_periode.Length == 6 ? Libs.GetStringToInteger(s_periode.Substring(0, 4)) : 0);

            if (bulan == 0 || tahun == 0)
            {
                if (lst_periode_absen.Count > 0)
                {
                    bulan = lst_periode_absen[0].Bulan;
                    tahun = lst_periode_absen[0].Tahun;
                }
            }

            ltrPeriodeAbsen.Text = "";
            string html = "";
            int id = 1;

            foreach (var periode in lst_periode_absen)
            {
                html += "<div onclick=\"ResponseRedirect('" +
                          Libs.FILE_PAGE_URL + "?t=" + 
                          QS.GetTahunAjaranPure() + 
                          (Libs.GetQueryString("ft").Trim() != "" ? "&ft=" + Libs.GetQueryString("ft") : "") +
                          (Libs.GetQueryString("m").Trim() != "" ? "&m=" + Libs.GetQueryString("m") : "") +
                          (Libs.GetQueryString("kd").Trim() != "" ? "&kd=" + Libs.GetQueryString("kd") : "") +
                          "&p=" + periode.Tahun.ToString() + (periode.Bulan < 10 ? "0" : "") + periode.Bulan.ToString() +
                        "');\" class=\"row" + (id % 2 != 0 ? " standardrow" : " oddrow") + "\" style=\"cursor: pointer;\">" +
                            "<div class=\"col-xs-12\" style=\"cursor: pointer; padding-left: 0px;padding-right: 5px;\">" +
                                "<table style=\"margin: 0px; width: 99%;\">" +
                                    "<tr>" +
                                        "<td style=\"cursor: pointer; width: 30px; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                            id.ToString() +
                                            "." +
                                        "</td>" +
                                        "<td style=\"vertical-align: middle; cursor: pointer; padding: 0px; font-size: small; padding-top: 7px; padding-bottom: 7px; font-weight: bold;\">" +
                                            "<i class=\"fa fa-calendar\"></i>" +
                                            "&nbsp;&nbsp;" +
                                            Libs.Array_Bulan[periode.Bulan - 1] + " " + periode.Tahun.ToString() +
                                            (
                                                periode.Bulan == bulan && periode.Tahun == tahun
                                                ? "<i class=\"fa fa-check-circle\" style=\"float: right; color: green;\"></i>"
                                                : ""
                                            ) +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                            "</div>" +
                        "</div>";
                id++;
            }

            ltrPeriodeAbsen.Text = html;
        }

        protected void ShowListAbsenLTS()
        {
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = QS.GetLevel();
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");

            DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAbsen.Text);

            ltrListSiswaAbsenTS.Text = "";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            bool b_mapel_pilihan = false;
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());
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
                                                            Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
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
                                    QS.GetTahunAjaran(),
                                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                    QS.GetMapel(),
                                    QS.GetKelas()
                                );
                            }
                            else
                            {
                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() == "")
                                {

                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        QS.GetTahunAjaran(),
                                        QS.GetSemester()
                                    );

                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() != "")
                                {
                                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                    {
                                        lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                                QS.GetMapel(),
                                                QS.GetLevel(),
                                                QS.GetKelas()
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
                            
                            int id = 1;
                            List<SiswaAbsenLTS> lst_siswa_absen_lts = DAO_SiswaAbsenLTS.GetAllByTABySMByKelasDet_Entity(
                                    tahun_ajaran, "1", rel_kelas_det
                                );

                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                      "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                string s_sakit = "0";
                                string s_izin = "0";
                                string s_alpa = "0";

                                SiswaAbsenLTS m_absen_lts = lst_siswa_absen_lts.FindAll(m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()).FirstOrDefault();
                                if (m_absen_lts != null)
                                {
                                    if (m_absen_lts.TahunAjaran != null)
                                    {
                                        s_sakit = m_absen_lts.Sakit.ToString();
                                        s_izin = m_absen_lts.Izin.ToString();
                                        s_alpa = m_absen_lts.Alpa.ToString();
                                    }
                                }

                                string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                ltrListSiswaAbsenTS.Text += "<div class=\"row\">" +
                                                            "<div class=\"col-xs-6\">" +
                                                                "<table style=\"margin: 0px; width: 100%;\">" +
                                                                    "<tr>" +
                                                                        "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                            id.ToString() +
                                                                            "." +
                                                                        "</td>" +
                                                                        "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                            "<input name=\"txt_siswa_absen_lts[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                            "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                        "</td>" +
                                                                        "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                            (
                                                                                m_siswa.NISSekolah.Trim() != ""
                                                                                ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                                    m_siswa.NISSekolah +
                                                                                  "</span>" +
                                                                                  "<br />"
                                                                                : ""
                                                                            ) +
                                                                            "<span style=\"color: grey; font-weight: bold;\">" +
                                                                                Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                (
                                                                                    m_siswa.Panggilan.Trim() != ""
                                                                                    ? "<span style=\"font-weight: normal\">" +
                                                                                        "&nbsp;" + s_panggilan +
                                                                                      "</span>"
                                                                                    : ""
                                                                                ) +
                                                                            "</span>" +
                                                                        "</td>" +
                                                                    "</tr>" +
                                                                "</table>" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-2\" style=\"vertical-align: middle;margin-top: 10px;\">" +
                                                                "<span style=\"font-size: small;\">" +
                                                                    "Sakit<br />" +
                                                                "</span>" +
                                                                "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_sakit + "\" name=\"txt_sakit_lts[]\" title=\" Sakit \" class=\"text-input\" style=\"width: 100%; border-rasius: 100%; text-align: center;\">" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-2\" style=\"vertical-align: middle;margin-top: 10px;\">" +
                                                                "<span style=\"font-size: small;\">" +
                                                                    "Izin<br />" +
                                                                "</span>" +
                                                                "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_izin + "\" name=\"txt_izin_lts[]\" title=\" Izin \" class=\"text-input\" style=\"width: 100%; border-rasius: 100%; text-align: center;\">" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-2\" style=\"vertical-align: middle;margin-top: 10px;\">" +
                                                                "<span style=\"font-size: small;\">" +
                                                                    "Alpa<br />" +
                                                                "</span>" +
                                                                "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_alpa + "\" name=\"txt_alpa_lts[]\" title=\" Alpa \" class=\"text-input\" style=\"width: 100%; border-rasius: 100%; text-align: center;\">" +
                                                            "</div>" +
                                                          "</div>" +
                                                          "<div class=\"row\">" +
                                                            "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                            "</div>" +
                                                          "</div>";

                                id++;
                            }

                        }
                    }

                }
            }
        }

        protected void ShowListAbsenRapor()
        {
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(Libs.GetQueryString("t"));
            string rel_kelas = QS.GetLevel();
            string rel_kelas_det = Libs.GetQueryString("kd");
            string rel_mapel = Libs.GetQueryString("m");

            DateTime dt_tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalAbsen.Text);

            ltrListSiswaAbsenTS.Text = "";

            Kelas m_kelas = DAO_Kelas.GetByID_Entity(rel_kelas);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //list siswa
                    List<Siswa> lst_siswa = new List<Siswa>();

                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                    if (m_sekolah != null)
                    {
                        if (m_sekolah.Nama != null)
                        {
                            bool b_mapel_pilihan = false;
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(QS.GetMapel());
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
                                                            Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
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
                                    QS.GetTahunAjaran(),
                                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                    QS.GetMapel(),
                                    QS.GetKelas()
                                );
                            }
                            else
                            {
                                if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() == "")
                                {

                                    lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        QS.GetTahunAjaran(),
                                        QS.GetSemester()
                                    );

                                }
                                else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && QS.GetMapel().Trim() != "")
                                {
                                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                    {
                                        lst_siswa = DAO_FormasiGuruMapelDetSiswa.GetSiswaByTABySMByMapelByKelasByKelasDet_Entity(
                                                QS.GetTahunAjaran(),
                                                Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                                                QS.GetMapel(),
                                                QS.GetLevel(),
                                                QS.GetKelas()
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

                            int id = 1;
                            List<SiswaAbsenRapor> lst_siswa_absen_rapor = DAO_SiswaAbsenRapor.GetAllByTABySMByKelasDet_Entity(
                                    tahun_ajaran, "1", rel_kelas_det
                                );

                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                      "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                string s_sakit = "0";
                                string s_izin = "0";
                                string s_alpa = "0";

                                SiswaAbsenRapor m_absen_rapor = lst_siswa_absen_rapor.FindAll(m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()).FirstOrDefault();
                                if (m_absen_rapor != null)
                                {
                                    if (m_absen_rapor.TahunAjaran != null)
                                    {
                                        s_sakit = m_absen_rapor.Sakit.ToString();
                                        s_izin = m_absen_rapor.Izin.ToString();
                                        s_alpa = m_absen_rapor.Alpa.ToString();
                                    }
                                }

                                string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                ltrListSiswaAbsenTS.Text += "<div class=\"row\">" +
                                                            "<div class=\"col-xs-6\">" +
                                                                "<table style=\"margin: 0px; width: 100%;\">" +
                                                                    "<tr>" +
                                                                        "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                            id.ToString() +
                                                                            "." +
                                                                        "</td>" +
                                                                        "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                            "<input name=\"txt_siswa_absen_rapor[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                                                            "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                                                "style=\"margin-top: 10px; height: 32px; width: 32px; border-radius: 100%;\">" +
                                                                        "</td>" +
                                                                        "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                                            (
                                                                                m_siswa.NISSekolah.Trim() != ""
                                                                                ? "<span style=\"color: #bfbfbf; font-weight: normal; font-size: small;\">" +
                                                                                    m_siswa.NISSekolah +
                                                                                  "</span>" +
                                                                                  "<br />"
                                                                                : ""
                                                                            ) +
                                                                            "<span style=\"color: grey; font-weight: bold;\">" +
                                                                                Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 2) +
                                                                                (
                                                                                    m_siswa.Panggilan.Trim() != ""
                                                                                    ? "<span style=\"font-weight: normal\">" +
                                                                                        "&nbsp;" + s_panggilan +
                                                                                      "</span>"
                                                                                    : ""
                                                                                ) +
                                                                            "</span>" +
                                                                        "</td>" +
                                                                    "</tr>" +
                                                                "</table>" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-2\" style=\"vertical-align: middle;margin-top: 10px;\">" +
                                                                "<span style=\"font-size: small;\">" +
                                                                    "Sakit<br />" +
                                                                "</span>" +
                                                                "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_sakit + "\" name=\"txt_sakit_rapor[]\" title=\" Sakit \" class=\"text-input\" style=\"width: 100%; border-rasius: 100%; text-align: center;\">" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-2\" style=\"vertical-align: middle;margin-top: 10px;\">" +
                                                                "<span style=\"font-size: small;\">" +
                                                                    "Izin<br />" +
                                                                "</span>" +
                                                                "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_izin + "\" name=\"txt_izin_rapor[]\" title=\" Izin \" class=\"text-input\" style=\"width: 100%; border-rasius: 100%; text-align: center;\">" +
                                                            "</div>" +
                                                            "<div class=\"col-xs-2\" style=\"vertical-align: middle;margin-top: 10px;\">" +
                                                                "<span style=\"font-size: small;\">" +
                                                                    "Alpa<br />" +
                                                                "</span>" +
                                                                "<input onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" value=\"" + s_alpa + "\" name=\"txt_alpa_rapor[]\" title=\" Alpa \" class=\"text-input\" style=\"width: 100%; border-rasius: 100%; text-align: center;\">" +
                                                            "</div>" +
                                                          "</div>" +
                                                          "<div class=\"row\">" +
                                                            "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                                                "<hr style=\"margin: 0px; margin-top: 10px; border-color: #E9EFF5;\" />" +
                                                            "</div>" +
                                                          "</div>";

                                id++;
                            }

                        }
                    }

                }
            }
        }

        protected void lnkAbsenSiswaLTS_Click(object sender, EventArgs e)
        {
            ShowListAbsenLTS();
            txtJenisAbsen.Value = "0";
            txtKeyAction.Value = JenisAction.DoShowInputAbsenLTS.ToString();
        }

        protected void btnDoSaveAbsenLTS_Click(object sender, EventArgs e)
        {
            if (txtParseAbsenLTS.Value.Trim() != "")
            {
                string[] arr_absen_lts = txtParseAbsenLTS.Value.Split(new string[] { ";" }, StringSplitOptions.None);
                foreach (string item_arr_absen_lts in arr_absen_lts)
                {
                    string[] arr_absen_lts_item = item_arr_absen_lts.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (arr_absen_lts_item.Length == 4)
                    {
                        string rel_siswa = arr_absen_lts_item[0];
                        string s_sakit = arr_absen_lts_item[1];
                        string s_izin = arr_absen_lts_item[2];
                        string s_alpa = arr_absen_lts_item[3];

                        if (txtJenisAbsen.Value == "0")
                        {
                            DAO_SiswaAbsenLTS.Save_ByTABySMByKelasDetBySiswa(
                                    new SiswaAbsenLTS
                                    {
                                        Kode = Guid.NewGuid(),
                                        TahunAjaran = QS.GetTahunAjaran(),
                                        Semester = "1",
                                        Rel_KelasDet = QS.GetKelas(),
                                        Rel_Siswa = rel_siswa,
                                        Sakit = Libs.GetStringToInteger(s_sakit),
                                        Izin = Libs.GetStringToInteger(s_izin),
                                        Alpa = Libs.GetStringToInteger(s_alpa)
                                    }
                                );
                        }
                        else if (txtJenisAbsen.Value == "1")
                        {
                            DAO_SiswaAbsenRapor.Save_ByTABySMByKelasDetBySiswa(
                                    new SiswaAbsenRapor
                                    {
                                        Kode = Guid.NewGuid(),
                                        TahunAjaran = QS.GetTahunAjaran(),
                                        Semester = "1",
                                        Rel_KelasDet = QS.GetKelas(),
                                        Rel_Siswa = rel_siswa,
                                        Sakit = Libs.GetStringToInteger(s_sakit),
                                        Izin = Libs.GetStringToInteger(s_izin),
                                        Alpa = Libs.GetStringToInteger(s_alpa)
                                    }
                                );
                        }
                    }
                }
            }

            ShowAttrAbsenLTS();
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void lnkAbsenSiswaRapor_Click(object sender, EventArgs e)
        {
            ShowListAbsenRapor();
            txtJenisAbsen.Value = "1";
            txtKeyAction.Value = JenisAction.DoShowInputAbsenLTS.ToString();
        }

        protected void lnkOKDownloadLaporanPresensi_Click(object sender, EventArgs e)
        {
            string kd = Libs.GetQueryString("kd");
            if (kd.Trim() != "")
            {
                KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(kd);
                if (m_kelasdet != null)
                {
                    if (m_kelasdet.Nama != null)
                    {
                        Response.Redirect(
                            ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) +
                            "?unit=" + DAO_Sekolah.GetByID_Entity(DAO_Kelas.GetByID_Entity(m_kelasdet.Rel_Kelas.ToString()).Rel_Sekolah.ToString()).Kode.ToString() +
                            "&kd=" + kd +
                            (Libs.GetQueryString("m").Trim() != "" ? "&m=" + Libs.GetQueryString("m").Trim() : "")
                        );
                    }
                }
            }
        }
    }
}