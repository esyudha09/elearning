using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Perpustakaan;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_DAOs.Perpustakaan;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;

namespace AI_ERP
{
    public partial class wf_Dashboard : System.Web.UI.Page
    {
        public const string SessionViewDataName_KunjunganPerpustakaan = "SESSIONDATAKUNJUNGANPERPUSTAKAAN";
        public enum JenisAction
        {
            ShowOptionBiodataSiswa,
            ShowOptionLedgerSMA,
            ShowInfoKunjungan,
            ShowBukaSemester,
            ShowPilihKelas,
            ShowAbsensiWalas,
            DoSaveAbsensi
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            if (!IsPostBack)
            {
                this.Master.HeaderTittle = "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/house.svg") + "\" " +
                                            "style=\"margin: 0 auto; height: 35px; width: 35px; margin-top: -8px; margin-right: 5px; float: left;\" />" +
                                       "&nbsp;&nbsp;" +
                                       "Beranda";
                this.Master.ShowHeaderTools = true;
                this.Master.HeaderCardVisible = false;

                InitContentAndURL();
                InitInput();
                InitKelasUnit();
                InitMenu();
                InitInfoPerpustakaan();
                ListKelasMataPelajaran();
            }

            BindKunjunganPerpustakaan();
        }

        protected void InitMenu()
        {
            List<Sekolah> lst_unit_mengajar = new List<Sekolah>();
            Kelas m_kelas = new Kelas();

            string s_tahun_ajaran = Libs.GetTahunAjaranNow();
            string s_semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
            //s_tahun_ajaran = "2020/2021";
            //s_semester = "2";

            ltrKelas.Text = "";
            int id = 1;
            string html_card = "";            
            string[] arr_warna = { "#446D8C", "#007d00", "#8C36B4", "#01B5B5", "#E0992F", "mediumvioletred", "#3367d6" };
            string[] arr_bg = 
                { "#FD6933", "#0AC6AE", "#F7921E", "#4AA4A4", "#43B8C9", "#95D1C5", "#019ADD", "#31384B", "#18AEC7", "#5299CF", "#2D2C28", "#D5C5C6", "#262726", "#01ACAC", "#322D3A", "#3B4F5D", "#009E00", "#E90080", "#549092", "#00A9A9", "#9B993A" };
            string[] arr_bg_image = 
                { "a.png", "b.png", "c.png", "d.png", "e.png", "f.png", "g.png", "h.png", "i.png", "j.png", "k.png", "l.png", "m.png", "n.png", "o.png", "p.png", "q.png", "r.png", "u.png", "s.png", "t.png" };

            List<string> lst_html_card = new List<string>();
            List<FormasiGuruKelas_ByGuru> lst_kelasguru = new List<FormasiGuruKelas_ByGuru>();
            lst_html_card.Clear();

            if (Libs.GetQueryString("act").Trim() == "")
            {
                lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, s_tahun_ajaran).ToList().FindAll(
                        m => m.TahunAjaran == s_tahun_ajaran
                    );
            }
            else
            {
                lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, "2018/2019").ToList();
            }            
            
            List<MapelEkskulAjar> lst_mapel_ajar_TK = new List<MapelEkskulAjar>();
            lst_mapel_ajar_TK.Clear();
            List<MapelEkskulAjar> lst_mapel_ajar_SD = new List<MapelEkskulAjar>();
            lst_mapel_ajar_SD.Clear();

            //untuk kelas perwalian
            List<string> lst_formasi_guru_kelas = new List<string>();
            lst_formasi_guru_kelas.Clear();

            if (lst_kelasguru.Count <= 100)
            {

                foreach (FormasiGuruKelas_ByGuru m in lst_kelasguru.FindAll(m => m.Mapel.Trim() == "").ToList())
                {
                    List<Siswa> lst_siswa = new List<Siswa>();
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet);

                    if (m_sekolah != null && lst_formasi_guru_kelas.FindAll(m0 => m0 == m.TahunAjaran + m.Rel_KelasDet.ToString()).Count == 0)
                    {
                        if (m_sekolah.Nama != null)
                        {

                            lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                    m.Rel_Sekolah.ToString(), m.Rel_KelasDet, m.TahunAjaran, m.Semester
                                );
                            
                            string s_panggilan = "";
                            int id_siswa = 0;
                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                if (m_siswa.Panggilan.Trim() != "")
                                {
                                    s_panggilan += "<div class=\"tooltip\">" +
                                                        "<label " +
                                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>" +
                                                        "<div class=\"right\">" +
                                                            "<div class=\"text-content\">" +
                                                                "<div style=\"margin: 0 auto; display: table;\">" +
                                                                    "<img " +
                                                                        "src=\"" +
                                                                                ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg")) +
                                                                             "\"" +
                                                                        " style=\"height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;\" />" +
                                                                "</div>" +
                                                                "<label style=\"margin: 0 auto; display: table; font-size: 11pt; font-weight: bold;\">" +
                                                                    Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                                "</label>" +
                                                                "<label style=\"margin: 0 auto; display: table; font-size: 9pt; font-weight: normal; color: yellow;\">" +
                                                                    Libs.GetPerbaikiEjaanNama(m_siswa.Panggilan) +
                                                                "</label>" +
                                                                (
                                                                    m_siswa.TempatLahir.Trim() != ""
                                                                    ? "<br />" +
                                                                      "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" + 
                                                                          "<b class=\"fa fa-calendar\"></b>&nbsp;" +
                                                                          AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                m_siswa.TempatLahir
                                                                          ) +
                                                                          (
                                                                                m_siswa.TanggalLahir != DateTime.MinValue
                                                                                ? ", " +
                                                                                    AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(m_siswa.TanggalLahir, false)
                                                                                : ""
                                                                          ) +
                                                                       "</label>"
                                                                    : ""
                                                                ) +
                                                                (
                                                                    m_siswa.NamaAyah.Trim() != ""
                                                                    ? "<br />" +
                                                                      "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                          "<b class=\"fa fa-male\"></b>&nbsp;&nbsp;" +
                                                                          AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                m_siswa.NamaAyah
                                                                          ) +
                                                                       "</label>"
                                                                    : ""
                                                                ) +
                                                                (
                                                                    m_siswa.NamaIbu.Trim() != ""
                                                                    ? "<br />" +
                                                                      "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                          "<b class=\"fa fa-female\"></b>&nbsp;&nbsp;" +
                                                                          AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                m_siswa.NamaIbu
                                                                          ) +
                                                                       "</label>"
                                                                    : ""
                                                                ) +
                                                            "</div>" +
                                                            "<i></i>" +
                                                        "</div>" +
                                                   "</div>";
                                    id_siswa++;
                                }
                                else
                                {
                                    s_panggilan += "<div class=\"tooltip\">" +
                                                        "<label " +
                                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama.Trim()) + "</label>" +
                                                        "<div class=\"right\">" +
                                                            "<div class=\"text-content\">" +
                                                                "<div style=\"margin: 0 auto; display: table;\">" +
                                                                    "<img " +
                                                                        "src=\"" +
                                                                                ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg")) +
                                                                                "\"" +
                                                                        " style=\"height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;\" />" +
                                                                "</div>" +
                                                                "<label style=\"margin: 0 auto; display: table; font-size: 11pt; font-weight: bold;\">" +
                                                                    Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                                "</label>" +
                                                                (
                                                                    m_siswa.TempatLahir.Trim() != ""
                                                                    ? "<br />" +
                                                                        "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                            "<b class=\"fa fa-calendar\"></b>&nbsp;" +
                                                                            AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                m_siswa.TempatLahir
                                                                            ) +
                                                                            (
                                                                                m_siswa.TanggalLahir != DateTime.MinValue
                                                                                ? ", " +
                                                                                    AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(m_siswa.TanggalLahir, false)
                                                                                : ""
                                                                            ) +
                                                                        "</label>"
                                                                    : ""
                                                                ) +
                                                                (
                                                                    m_siswa.NamaAyah.Trim() != ""
                                                                    ? "<br />" +
                                                                        "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                            "<b class=\"fa fa-male\"></b>&nbsp;&nbsp;" +
                                                                            AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                m_siswa.NamaAyah
                                                                            ) +
                                                                        "</label>"
                                                                    : ""
                                                                ) +
                                                                (
                                                                    m_siswa.NamaIbu.Trim() != ""
                                                                    ? "<br />" +
                                                                        "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                            "<b class=\"fa fa-female\"></b>&nbsp;&nbsp;" +
                                                                            AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                m_siswa.NamaIbu
                                                                            ) +
                                                                        "</label>"
                                                                    : ""
                                                                ) +
                                                            "</div>" +
                                                            "<i></i>" +
                                                        "</div>" +
                                                    "</div>";
                                    id_siswa++;
                                }
                            }
                            if (s_panggilan.Trim() != "")
                            {
                                s_panggilan = "<div style=\"width: 100%; text-align: center;\">" +
                                                s_panggilan +
                                              "</div>";
                            }
                            else
                            {
                                s_panggilan = "<div style=\"width: 100%; text-align: center; color: #bfbfbf;\">" +
                                                "<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;Data Siswa Kosong" +
                                              "</div>";
                            }

                            string bg_warna = arr_bg[(id % arr_bg_image.Length)];
                            string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                            string bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);
                            string bg_image_card = ""; // ResolveUrl("~/Application_CLibs/images/kelas/bg.png");

                            lst_html_card.Add(
                                        "<div class=\"col-xs-6\">" +
                                            "<div class=\"card\" style=\"margin-top: 10px; border-radius: 5px; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;\">" +
                                                "<div class=\"card-main\" style=\"background: url(" + bg_image_card + "); background-position: bottom right; background-repeat: no-repeat;\">" +
                                                    "<div class=\"card-action\" style=\"background: url(" + bg_image + "); background-position: top right; background-color: " + bg_warna + "; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;\">" +
                                                        "<p class=\"card-heading\" style=\"margin-bottom: 0px; margin-top: 15px; color: white;\">" +
                                                            "<span style=\"font-weight: bold; color: white; font-weight: bold; font-size: larger;\">" + m.KelasDet + "</span>" +
                                                            "&nbsp;" +
                                                            "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                "<span style=\"font-weight: normal;\">" + lst_siswa.Count + " siswa</span>" +
                                                            "</sup>" +
                                                            "<div style=\"font-size: medium; color: white;\">" + m.TahunAjaran + "</div>" +
                                                            (
                                                                m.Mapel.Trim() != ""
                                                                ? "<div style=\"font-size: medium; color: white; font-size: small;\">" + m.Mapel + "</div>"
                                                                : "<div style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">" +
                                                                    "<label class=\"badge\" style=\"margin-top: 10px; margin-left: -5px;\">Guru Kelas atau Wali Kelas</label>" +
                                                                    "</div>"
                                                            ) +
                                                        "</p>" +
                                                    "</div>" +
                                                    "<div class=\"card-inner\" style=\"margin-top: 0px; color: grey;\">" +
                                                        "<p class=\"card-heading\" style=\"margin-bottom: 5px;\">" +
                                                            s_panggilan +
                                                        "</p>" +
                                                    "</div>" +
                                                    "<div class=\"card-action\" style=\"padding-left: 10px; padding-right: 10px;\">" +
                                                        "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; margin-right: 0px; color: grey;\">" +
                                                            "<div class=\"tooltip\">" +
                                                                "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE) +
                                                                                "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                "&ft=" + Libs.Encryptdata(file_img) +
                                                                                (
                                                                                    m.KodeMapel.Trim() != ""
                                                                                    ? "&m=" + m.KodeMapel
                                                                                    : ""
                                                                                ) +
                                                                                "&kd=" + m.Rel_KelasDet +
                                                                            "\" " +
                                                                    "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                    "<i class=\"fa fa-folder-open\"></i>" +
                                                                "</a>" +
                                                                "<div class=\"top\">" +
                                                                    "Buka" +
                                                                    "<i></i>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                            "<div class=\"tooltip\">" +
                                                                "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LIBRARY.KUNJUNGAN_PERPUSTAKAAN.ROUTE) +
                                                                                "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                "&ft=" + Libs.Encryptdata(file_img) +
                                                                                (
                                                                                    m.KodeMapel.Trim() != ""
                                                                                    ? "&m=" + m.KodeMapel
                                                                                    : ""
                                                                                ) +
                                                                                "&kd=" + m.Rel_KelasDet +
                                                                            "\" " +
                                                                    "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                    "<i class=\"fa fa-address-book-o\"></i>" +
                                                                "</a>" +
                                                                "<div class=\"top\">" +
                                                                    "Perpustakaan" +
                                                                    "<i></i>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                        (
                                                            m.Mapel.Trim() != "" || 
                                                            (
                                                                !DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP) &&
                                                                !DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMA)
                                                            )
                                                            ? ""
                                                            : "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                                    "<div class=\"tooltip\">" +
                                                                        "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                            "href=\"" + (
                                                                                            m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP
                                                                                            ? ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.VOLUNTEER.ROUTE)
                                                                                            : (
                                                                                                m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA
                                                                                                ? ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.VOLUNTEER.ROUTE)
                                                                                                : ""
                                                                                              )
                                                                                        ) +
                                                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                        "&ft=" + Libs.Encryptdata(file_img) +
                                                                                        "&s=" + s_semester +
                                                                                        "&kd=" + m.Rel_KelasDet +
                                                                                    "\" " +
                                                                            "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                            "<i class=\"fa fa-handshake-o\"></i>" +
                                                                        "</a>" +
                                                                        "<div class=\"top\">" +
                                                                            "Volunteer" +
                                                                            "<i></i>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>"
                                                        ) +
                                                        (
                                                            m.Mapel.Trim() != "" ||
                                                            (
                                                                !DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP) &&
                                                                !DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMA)
                                                            )
                                                            ? ""
                                                            : "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                                    "<div class=\"tooltip\">" +
                                                                        "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                            "href=\"" + (
                                                                                            m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP
                                                                                            ? ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CATATAN_SISWA.ROUTE)
                                                                                            : (
                                                                                                m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA
                                                                                                ? ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.CATATAN_SISWA.ROUTE)
                                                                                                : ""
                                                                                              )
                                                                                        ) + 
                                                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                        "&ft=" + Libs.Encryptdata(file_img) +
                                                                                        "&s=" + s_semester +
                                                                                        "&kd=" + m.Rel_KelasDet +
                                                                                    "\" " +
                                                                            "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                            "<i class=\"fa fa-edit\"></i>" +
                                                                        "</a>" +
                                                                        "<div class=\"top\">" +
                                                                            "Catatan Wali Kelas" +
                                                                            "<i></i>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>"
                                                        ) +
                                                        (
                                                            m.Mapel.Trim() != "" || !DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP)
                                                            ? ""
                                                            : "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                                    "<div class=\"tooltip\">" +
                                                                        "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                            "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KEPRIBADIAN_SISWA.ROUTE) +
                                                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                        "&ft=" + Libs.Encryptdata(file_img) +
                                                                                        "&s=" + s_semester +
                                                                                        "&kd=" + m.Rel_KelasDet +
                                                                                    "\" " +
                                                                            "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                            "<i class=\"fa fa-users\"></i>" +
                                                                        "</a>" +
                                                                        "<div class=\"top\">" +
                                                                            "Kepribadian Siswa" +
                                                                            "<i></i>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>"
                                                        ) +
                                                        //(
                                                        //    (m.Mapel.Trim() != "" || 
                                                        //        (
                                                        //            !(DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP)) &&
                                                        //            !(DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMA))
                                                        //        )
                                                        //    )
                                                        //    ? ""
                                                        //    : "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                        //            "<div class=\"tooltip\">" +
                                                        //                "<label class=\"btn btn-flat waves-attach waves-effect\" " +
                                                        //                    "onclick=\"" +
                                                        //                                txtTahunAjaran.ClientID + ".value = '" + m.TahunAjaran + "'; " +
                                                        //                                txtSemester.ClientID + ".value = '" + m.Semester + "'; " +
                                                        //                                txtRelSekolah.ClientID + ".value = '" + m.Rel_Sekolah + "'; " +
                                                        //                                txtRelKelasDet.ClientID + ".value = '" + m.Rel_KelasDet + "'; " +
                                                        //                                btnSHowAbsensi.ClientID + ".click();" + 
                                                        //                            "\" " +
                                                        //                    "style=\"cursor: pointer; margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                        //                    "<i class=\"fa fa-calendar-check-o\"></i>" +
                                                        //                "</label>" +
                                                        //                "<div class=\"top\">" +
                                                        //                    "Absensi Siswa" +
                                                        //                    "<i></i>" +
                                                        //                "</div>" +
                                                        //            "</div>" +
                                                        //        "</div>"
                                                        //) +
                                                        (
                                                            "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                                "<div class=\"tooltip\">" +
                                                                    "<label class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                        "onclick=\"document.location.href='" +
                                                                                    ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_ABSENSI_SISWA.ROUTE) +
                                                                                    "?kd=" + m.Rel_KelasDet +
                                                                                "';\" " +
                                                                        "style=\"cursor: pointer; margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                        "<i class=\"fa fa-file-text-o\"></i>" +
                                                                    "</label>" +
                                                                    "<div class=\"top\">" +
                                                                        "Laporan Presensi Siswa" +
                                                                        "<i></i>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>"
                                                        ) +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>"
                                );

                            id++;

                        }
                    }

                    lst_formasi_guru_kelas.Add(m.TahunAjaran + m.Rel_KelasDet.ToString());
                }

                List<string> lst_formasi_guru_mapel = new List<string>();
                lst_formasi_guru_mapel.Clear();
                //untuk kelas bidang studi
                foreach (FormasiGuruKelas_ByGuru m in lst_kelasguru.FindAll(m => m.Mapel.Trim() != "").ToList())
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.KodeMapel);
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet);

                    //jika tidak sebagai guru kelas & guru matapelajaran
                    if (m_mapel != null && lst_formasi_guru_mapel.FindAll(m0 => m0 == m.TahunAjaran + m.Rel_KelasDet.ToString()).Count == 0)
                    {
                        Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                        bool b_valid = true;

                        if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                        {
                            var lst_mapel_nilai = AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                m.TahunAjaran,
                                DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet.ToString()).Rel_Kelas.ToString(),
                                Libs.LOGGED_USER_M.NoInduk
                            );
                            if (lst_mapel_nilai.Count == 0) b_valid = false;
                        }

                        if (b_valid && m_mapel.Nama != null && lst_formasi_guru_mapel.FindAll(m0 => m0 == m.TahunAjaran + m_mapel.Kode.ToString() + m.Rel_KelasDet.ToString()).Count == 0)
                        {
                            List<Siswa> lst_siswa = new List<Siswa>();

                            //mapel non ekskul
                            if (
                                (
                                    DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) != Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                    (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.EKSTRAKURIKULER && m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                                )
                            )
                            {

                                if (m_sekolah != null)
                                {
                                    if (m_sekolah.Nama != null)
                                    {

                                        bool ada_siswa = false;
                                        m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                                        
                                        if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN)
                                        {
                                            if (m_kelas != null)
                                            {
                                                if (m_kelas.Nama != null)
                                                {
                                                    if (m_kelas.Nama.Trim().ToUpper() != "X")
                                                    {
                                                        lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                                                m.TahunAjaran,
                                                                m.Semester,
                                                                m_mapel.Kode.ToString(),
                                                                m_kelas_det.Kode.ToString()
                                                            );
                                                        ada_siswa = true;
                                                    }
                                                    else
                                                    {
                                                        if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                                            Libs.LOGGED_USER_M.NoInduk,
                                                            m.TahunAjaran,
                                                            m.Semester,
                                                            m_kelas_det.Kode.ToString(),
                                                            m_mapel.Kode.ToString()
                                                        ))
                                                        {
                                                            lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                                                m.TahunAjaran,
                                                                m.Semester,
                                                                m_mapel.Kode.ToString(),
                                                                m_kelas_det.Kode.ToString()
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
                                                    m.TahunAjaran,
                                                    m.Semester,
                                                    m_mapel.Kode.ToString(),
                                                    m_kelas_det.Rel_Kelas.ToString(),
                                                    m_kelas_det.Kode.ToString()
                                                );

                                            ada_siswa = true;
                                        }
                                        if (!ada_siswa)
                                        {
                                            lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                                    m.Rel_Sekolah.ToString(), m.Rel_KelasDet, m.TahunAjaran, m.Semester
                                                );
                                        }
                                        
                                        string s_panggilan = "";
                                        int id_siswa = 0;
                                        foreach (Siswa m_siswa in lst_siswa)
                                        {
                                            if (m_siswa.Panggilan.Trim() != "")
                                            {
                                                s_panggilan += "<div class=\"tooltip\">" +
                                                                    "<label " +
                                                                        "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>" +
                                                                    "<div class=\"right\">" +
                                                                        "<div class=\"text-content\">" +
                                                                            "<div style=\"margin: 0 auto; display: table;\">" +
                                                                                "<img " +
                                                                                    "src=\"" +
                                                                                            ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg")) +
                                                                                         "\"" +
                                                                                    " style=\"height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;\" />" +
                                                                            "</div>" +
                                                                            "<label style=\"margin: 0 auto; display: table; font-size: 11pt; font-weight: bold;\">" +
                                                                                Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                                            "</label>" +
                                                                            "<label style=\"margin: 0 auto; display: table; font-size: 9pt; font-weight: normal; color: yellow;\">" +
                                                                                Libs.GetPerbaikiEjaanNama(m_siswa.Panggilan) +
                                                                            "</label>" +
                                                                            (
                                                                                m_siswa.TempatLahir.Trim() != ""
                                                                                ? "<br />" +
                                                                                  "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                                      "<b class=\"fa fa-calendar\"></b>&nbsp;" +
                                                                                      AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                            m_siswa.TempatLahir
                                                                                      ) +
                                                                                      (
                                                                                            m_siswa.TanggalLahir != DateTime.MinValue
                                                                                            ? ", " +
                                                                                                AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(m_siswa.TanggalLahir, false)
                                                                                            : ""
                                                                                      ) +
                                                                                   "</label>"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                m_siswa.NamaAyah.Trim() != ""
                                                                                ? "<br />" +
                                                                                  "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                                      "<b class=\"fa fa-male\"></b>&nbsp;&nbsp;" +
                                                                                      AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                            m_siswa.NamaAyah
                                                                                      ) +
                                                                                   "</label>"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                m_siswa.NamaIbu.Trim() != ""
                                                                                ? "<br />" +
                                                                                  "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                                      "<b class=\"fa fa-female\"></b>&nbsp;&nbsp;" +
                                                                                      AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                            m_siswa.NamaIbu
                                                                                      ) +
                                                                                   "</label>"
                                                                                : ""
                                                                            ) +
                                                                        "</div>" +
                                                                        "<i></i>" +
                                                                    "</div>" +
                                                               "</div>";

                                                if (id_siswa + 1 == 30 && lst_siswa.Count - 30 > 0)
                                                {
                                                    s_panggilan += "<label " +
                                                                        "style=\"border-style: solid; border-width: 2px; border-color: mediumvioletred; border-radius: 10px; font-weight: normal; background-color: mediumvioletred; color: white; padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" +
                                                                        (
                                                                            lst_siswa.Count - 30 > 0
                                                                            ? "+"
                                                                            : ""
                                                                        ) +
                                                                        (lst_siswa.Count - 30).ToString() +
                                                                        "&nbsp;siswa" +
                                                                   "</label>";
                                                    break;
                                                }

                                                id_siswa++;
                                            }
                                            else
                                            {
                                                s_panggilan += "<div class=\"tooltip\">" +
                                                                    "<label " +
                                                                        "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama.Trim()) + "</label>" +
                                                                    "<div class=\"right\">" +
                                                                        "<div class=\"text-content\">" +
                                                                            "<div style=\"margin: 0 auto; display: table;\">" +
                                                                                "<img " +
                                                                                    "src=\"" +
                                                                                            ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg")) +
                                                                                         "\"" +
                                                                                    " style=\"height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;\" />" +
                                                                            "</div>" +
                                                                            "<label style=\"margin: 0 auto; display: table; font-size: 11pt; font-weight: bold;\">" +
                                                                                Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                                            "</label>" +
                                                                            (
                                                                                m_siswa.TempatLahir.Trim() != ""
                                                                                ? "<br />" +
                                                                                  "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                                      "<b class=\"fa fa-calendar\"></b>&nbsp;" +
                                                                                      AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                            m_siswa.TempatLahir
                                                                                      ) +
                                                                                      (
                                                                                            m_siswa.TanggalLahir != DateTime.MinValue
                                                                                            ? ", " +
                                                                                                AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(m_siswa.TanggalLahir, false)
                                                                                            : ""
                                                                                      ) +
                                                                                   "</label>"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                m_siswa.NamaAyah.Trim() != ""
                                                                                ? "<br />" +
                                                                                  "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                                      "<b class=\"fa fa-male\"></b>&nbsp;&nbsp;" +
                                                                                      AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                            m_siswa.NamaAyah
                                                                                      ) +
                                                                                   "</label>"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                m_siswa.NamaIbu.Trim() != ""
                                                                                ? "<br />" +
                                                                                  "<label style=\"diplay: table; font-size: 10pt; font-weight: normal; color: white;\">" +
                                                                                      "<b class=\"fa fa-female\"></b>&nbsp;&nbsp;" +
                                                                                      AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                            m_siswa.NamaIbu
                                                                                      ) +
                                                                                   "</label>"
                                                                                : ""
                                                                            ) +
                                                                        "</div>" +
                                                                        "<i></i>" +
                                                                    "</div>" +
                                                               "</div>";

                                                if (id_siswa + 1 == 30 && lst_siswa.Count - 30 > 0)
                                                {
                                                    s_panggilan += "<label " +
                                                                        "style=\"border-style: solid; border-width: 2px; border-color: mediumvioletred; border-radius: 10px; font-weight: normal; background-color: mediumvioletred; color: white; padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + 
                                                                        (
                                                                            lst_siswa.Count - 30 > 0
                                                                            ? "+"
                                                                            : ""
                                                                        ) + 
                                                                        (lst_siswa.Count - 30).ToString() +
                                                                        "&nbsp;siswa" +
                                                                   "</label>";
                                                    break;
                                                }

                                                id_siswa++;
                                            }
                                        }
                                        if (s_panggilan.Trim() != "")
                                        {
                                            s_panggilan = "<div style=\"width: 100%; text-align: center;\">" +
                                                            s_panggilan +
                                                          "</div>";
                                        }
                                        else
                                        {
                                            s_panggilan = "<div style=\"width: 100%; text-align: center; color: #bfbfbf;\">" +
                                                            "<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;Data Siswa Kosong" +
                                                          "</div>";
                                        }

                                        string bg_warna = arr_bg[(id % arr_bg_image.Length)];
                                        string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                                        string bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);
                                        string bg_image_card = ""; // ResolveUrl("~/Application_CLibs/images/kelas/bg.png");
                                        string jenis_kelas = "Kelas Perwalian";
                                        string warna = "green";

                                        if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA && m.Mapel.Trim() != "")
                                        {
                                            if (m_kelas_det.IsKelasJurusan)
                                            {
                                                jenis_kelas = "Kelas Jurusan";
                                                warna = "mediumvioletred";
                                            }
                                            else if (m_kelas_det.IsKelasSosialisasi)
                                            {
                                                jenis_kelas = "Kelas Sosialisasi";
                                                warna = "#c71515";
                                            }
                                        }
                                        else
                                        {
                                            jenis_kelas = "";
                                            warna = "";
                                        }
                                        if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                        {
                                            jenis_kelas = "";
                                            warna = "";
                                        }

                                        lst_html_card.Add(
                                                "<div class=\"col-xs-6\">" +
                                                        "<div class=\"card\" style=\"margin-top: 10px; border-radius: 5px; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;\">" +
                                                            "<div class=\"card-main\" style=\"background: url(" + bg_image_card + "); background-position: bottom right; background-repeat: no-repeat;\">" +
                                                                "<div class=\"card-action\" style=\"background: url(" + bg_image + "); background-position: top right; background-color: " + bg_warna + "; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;\">" +
                                                                    "<p class=\"card-heading\" style=\"margin-bottom: 0px; margin-top: 15px; color: white;\">" +
                                                                        "<span style=\"font-weight: bold; color: white; font-weight: bold; font-size: larger;\">" +  
                                                                            (
                                                                                m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT
                                                                                ? "" //"<span style=\"font-weight: normal; color: yellow;\">Kelas</span>&nbsp;"
                                                                                : ""
                                                                            ) +
                                                                            m.KelasDet +
                                                                        "</span>" +
                                                                        "&nbsp;" +
                                                                        (
                                                                            jenis_kelas.Trim() != ""
                                                                            ? "<sup style=\"top: -10px; background-color: " + warna + "; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                                "<span style=\"font-weight: bold;\">" + jenis_kelas + "</span>" +
                                                                              "</sup>"
                                                                            : ""
                                                                        ) +
                                                                        "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            "<span style=\"font-weight: normal;\">" + lst_siswa.Count + " siswa</span>" +
                                                                        "</sup>" +
                                                                        "<div style=\"font-size: medium; color: white;\">" + m.TahunAjaran + "</div>" +
                                                                        (
                                                                            m.Mapel.Trim() != ""
                                                                            ? "<div style=\"font-size: medium; color: white; font-size: small;\">" + m.Mapel + "</div>"
                                                                            : "<div style=\"font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;\">" +
                                                                                "<label class=\"badge\" style=\"margin-top: 10px; margin-left: -5px;\">Guru Kelas atau Wali Kelas</label>" +
                                                                              "</div>"
                                                                        ) +
                                                                    "</p>" +
                                                                "</div>" +
                                                                "<div class=\"card-inner\" style=\"margin-top: 0px; color: grey;\">" +
                                                                    "<p class=\"card-heading\" style=\"margin-bottom: 5px;\">" +
                                                                        s_panggilan +
                                                                    "</p>" +
                                                                "</div>" +
                                                                "<div class=\"card-action\" style=\"padding-left: 10px; padding-right: 10px;\">" +
                                                                    "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; margin-right: 0px; color: grey;\">" +
                                                                        "<div class=\"tooltip\">" +
                                                                            "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                                "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE) +
                                                                                            "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                            "&ft=" + Libs.Encryptdata(file_img) +
                                                                                            (
                                                                                                m.KodeMapel.Trim() != ""
                                                                                                ? "&m=" + m.KodeMapel
                                                                                                : ""
                                                                                            ) +
                                                                                            "&kd=" + m.Rel_KelasDet +
                                                                                        "\" " +
                                                                                "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                                "<i class=\"fa fa-folder-open\"></i>" +
                                                                            "</a>" +
                                                                            "<div class=\"top\">" +
                                                                                "Buka" +
                                                                                "<i></i>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                    "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;\">" +
                                                                        "<div class=\"tooltip\">" +
                                                                            "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                                "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LIBRARY.KUNJUNGAN_PERPUSTAKAAN.ROUTE) +
                                                                                            "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                                                            "&ft=" + Libs.Encryptdata(file_img) +
                                                                                            (
                                                                                                m.KodeMapel.Trim() != ""
                                                                                                ? "&m=" + m.KodeMapel
                                                                                                : ""
                                                                                            ) +
                                                                                            "&kd=" + m.Rel_KelasDet +
                                                                                        "\" " +
                                                                                "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                                "<i class=\"fa fa-address-book-o\"></i>" +
                                                                            "</a>" +
                                                                            "<div class=\"top\">" +
                                                                                "Perpustakaan" +
                                                                                "<i></i>" +
                                                                            "</div>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>"
                                                );
                                        id++;

                                    }
                                }

                            }
                            //mapel ekskul
                            else
                            {
                                if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                {
                                    m_kelas_det = DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet);
                                    m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                                    Sekolah m_sekolah_ = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());

                                    if (m_kelas_det != null)
                                    {
                                        if (m_kelas_det.Nama != null)
                                        {
                                            if (lst_unit_mengajar.FindAll(ms => ms.Kode.ToString() == m_sekolah_.Kode.ToString()).Count == 0) lst_unit_mengajar.Add(m_sekolah_);

                                            if (m_sekolah_.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                                            {
                                                if (lst_mapel_ajar_TK.FindAll(
                                                    m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                          m0.Semester == m.Semester &&
                                                          m0.Rel_Mapel.Trim().ToUpper() == m.KodeMapel.Trim().ToUpper() &&
                                                          m0.Rel_Kelas == m_kelas_det.Rel_Kelas.ToString() &&
                                                          m0.Rel_KelasDet == m_kelas_det.Kode.ToString()
                                               ).Count == 0)
                                                {
                                                    lst_mapel_ajar_TK.Add(new MapelEkskulAjar
                                                    {
                                                        TahunAjaran = m.TahunAjaran,
                                                        Semester = m.Semester,
                                                        Rel_Mapel = m.KodeMapel,
                                                        Rel_Kelas = m_kelas_det.Rel_Kelas.ToString(),
                                                        Rel_KelasDet = m_kelas_det.Kode.ToString(),
                                                        UrutanLevel = m_kelas.UrutanLevel
                                                    });
                                                }
                                            }
                                            else if (m_sekolah_.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                                            {
                                                if (lst_mapel_ajar_SD.FindAll(
                                                    m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                          m0.Semester == m.Semester &&
                                                          m0.Rel_Mapel.Trim().ToUpper() == m.KodeMapel.Trim().ToUpper() &&
                                                          m0.Rel_Kelas == m_kelas_det.Rel_Kelas.ToString()
                                               ).Count == 0)
                                                {
                                                    lst_mapel_ajar_SD.Add(new MapelEkskulAjar
                                                    {
                                                        TahunAjaran = m.TahunAjaran,
                                                        Semester = m.Semester,
                                                        Rel_Mapel = m.KodeMapel.Trim().ToUpper(),
                                                        Rel_Kelas = m_kelas_det.Rel_Kelas.ToString(),
                                                        UrutanLevel = m_kelas.UrutanLevel
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        lst_formasi_guru_mapel.Add(m.TahunAjaran + m_mapel.Kode.ToString() + m.Rel_KelasDet.ToString());

                    }
                }
            }

            int jml_anak = 0;
            List<string> lst_kelas_ekskul = new List<string>();
            lst_unit_mengajar = lst_unit_mengajar.OrderBy(m => m.UrutanJenjang).ToList();
            foreach (var unit_mengajar in lst_unit_mengajar)
            {
                if (unit_mengajar.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                {
                    lst_mapel_ajar_TK = lst_mapel_ajar_TK.OrderBy(m => m.UrutanLevel).ToList();
                    var lst_mapel_ajar_TK_ = lst_mapel_ajar_TK.Select(m => new { m.TahunAjaran, m.Rel_Mapel, m.Rel_Kelas }).Distinct().ToList();
                    //cek & list mapel ekskul
                    foreach (var mapel_ekskul in lst_mapel_ajar_TK_)
                    {
                        lst_kelas_ekskul.Clear();

                        var lst_ajar_ekskul = lst_mapel_ajar_TK.FindAll(
                                m => m.TahunAjaran == mapel_ekskul.TahunAjaran &&
                                     m.Rel_Mapel == mapel_ekskul.Rel_Mapel &&
                                     m.Rel_Kelas == mapel_ekskul.Rel_Kelas
                            ).OrderBy(m => m.UrutanLevel).ToList();

                        jml_anak = 0;
                        string kelas_ekskul = "";
                        string kelas_ekskul_url = "";
                        string s_panggilan = "";
                        string s_tabs_panggilan = "";
                        string s_tabs_semester = "";
                        string s_tag_jml_siswa_semester_1 = "<sm1>";
                        string s_tag_jml_siswa_semester_2 = "<sm2>";

                        int i_jumlah_siswa_semester_1 = 0;
                        int i_jumlah_siswa_semester_2 = 0;

                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(mapel_ekskul.Rel_Mapel.ToString());
                        Sekolah m_sekolah = new Sekolah();

                        kelas_ekskul = "";
                        kelas_ekskul_url = "";

                        for (int i_semester = 1; i_semester <= 2; i_semester++)
                        {
                            string s_panggilan_semester = "";
                            string id_per_semester = "ID_" + Guid.NewGuid().ToString().Replace("-", "");

                            s_tabs_semester += "<li" + (s_semester == i_semester.ToString() ? " class=\"active\"" : "") + ">" +
                                                    "<a class=\"waves-attach\" data-toggle=\"tab\" href=\"#" + id_per_semester + "\">" +
                                                        "Semester <span style=\"font-weight: bold;\">" + i_semester.ToString() + "</span>" +
                                                        (i_semester == 1 ? s_tag_jml_siswa_semester_1 : "") +
                                                        (i_semester == 2 ? s_tag_jml_siswa_semester_2 : "") +
                                                    "</a>" +
                                               "</li>";
                            List<string> lst_rel_siswa = new List<string>();
                            lst_rel_siswa.Clear();

                            foreach (var item_ajar_ekskul in lst_ajar_ekskul.FindAll(m0 => m0.Semester == i_semester.ToString()))
                            {
                                m_kelas = DAO_Kelas.GetByID_Entity(item_ajar_ekskul.Rel_Kelas.ToString());

                                if (m_kelas != null && m_mapel != null)
                                {
                                    if (m_kelas.Nama != null)
                                    {
                                        m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                                        if (m_sekolah != null)
                                        {
                                            if (m_sekolah.Nama != null)
                                            {
                                                if (lst_kelas_ekskul.FindAll(m0 => m0 == m_kelas.Nama + mapel_ekskul.Rel_Mapel + i_semester.ToString()).Count == 0)
                                                {
                                                    kelas_ekskul += (kelas_ekskul.Trim() != "" && m_kelas.Nama.Trim() != "" ? ", " : "") +
                                                                        m_kelas.Nama;
                                                    kelas_ekskul_url += m_kelas.Kode.ToString() + ";";
                                                    lst_kelas_ekskul.Add(m_kelas.Nama + mapel_ekskul.Rel_Mapel + i_semester.ToString());
                                                }

                                                var lst_ekskul_tk =
                                                        AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_DesignDetEkskul.GetByTABySMByKelasByMapel_Entity(
                                                            mapel_ekskul.TahunAjaran, i_semester.ToString(), item_ajar_ekskul.Rel_Kelas.ToString(), mapel_ekskul.Rel_Mapel
                                                        );

                                                int id_siswa = 0;
                                                foreach (var ekskul_tk in lst_ekskul_tk)
                                                {
                                                    Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                                                            mapel_ekskul.TahunAjaran,
                                                            i_semester.ToString(),
                                                            ekskul_tk.Rel_Siswa.ToString()
                                                        );
                                                    if (m_siswa != null)
                                                    {
                                                        if (m_siswa.Nama != null)
                                                        {
                                                            if (lst_mapel_ajar_TK.FindAll(
                                                                m0 => m0.TahunAjaran == mapel_ekskul.TahunAjaran &&
                                                                      m0.Rel_Mapel == mapel_ekskul.Rel_Mapel &&
                                                                      m0.Rel_Kelas == mapel_ekskul.Rel_Kelas &&
                                                                      m0.Semester == i_semester.ToString() && 
                                                                      m0.Rel_KelasDet.ToUpper().Trim() == m_siswa.Rel_KelasDet.ToString().ToUpper().Trim()
                                                            ).Count > 0)
                                                            {
                                                                if (lst_rel_siswa.FindAll(m0 => m0 == m_siswa.Kode.ToString()).Count == 0)
                                                                {
                                                                    if (m_siswa.Panggilan.Trim() != "")
                                                                    {
                                                                        s_panggilan_semester +=
                                                                                       "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                                                        s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";
                                                                        id_siswa++;
                                                                        jml_anak++;

                                                                        if (i_semester == 1) i_jumlah_siswa_semester_1++;
                                                                        if (i_semester == 2) i_jumlah_siswa_semester_2++;
                                                                    }
                                                                    else
                                                                    {
                                                                        s_panggilan_semester +=
                                                                                        "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";

                                                                        s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";
                                                                        id_siswa++;
                                                                        jml_anak++;

                                                                        if (i_semester == 1) i_jumlah_siswa_semester_1++;
                                                                        if (i_semester == 2) i_jumlah_siswa_semester_2++;
                                                                    }
                                                                    lst_rel_siswa.Add(m_siswa.Kode.ToString());
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

                            if (s_panggilan_semester.Trim() != "")
                            {
                                s_tabs_panggilan += "<div class=\"tab-pane fade" + (Libs.GetSemesterByTanggal(DateTime.Now) == i_semester ? " active in" : "") + "\" id=\"" + id_per_semester + "\">" +
                                                        "<div>" +
                                                            s_panggilan_semester +
                                                        "</div>" +
                                                     "</div>";
                            }
                            else
                            {
                                s_tabs_panggilan += "<div class=\"tab-pane fade" + (Libs.GetSemesterByTanggal(DateTime.Now) == i_semester ? " active in" : "") + "\" id=\"" + id_per_semester + "\">" +
                                                        "<div style=\"margin: 0 auto; display: table; color: grey; height: 80px;\">" +
                                                            "<br />" +
                                                            "<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;Data Kosong" +
                                                        "</div>" +
                                                     "</div>";
                            }
                        }

                        if (s_tabs_panggilan.Trim() != "")
                        {
                            s_tabs_panggilan = "<div class=\"card\" style=\"margin-top: 0px; margin-bottom: 0px; box-shadow: none; border-style: none; border-color: #dddddd; box-shadow: none;\">" +
                                                    "<div class=\"card-main\">" +
                                                        "<nav class=\"tab-nav margin-top-no margin-bottom-no\">" +
                                                            "<ul class=\"nav nav-justified\">" +
                                                                s_tabs_semester
                                                                .Replace(
                                                                    s_tag_jml_siswa_semester_1,
                                                                    "&nbsp;&nbsp;" +
                                                                    "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                        "<span style=\"font-weight: normal;\"><span style=\"font-weight: bold;\">" + i_jumlah_siswa_semester_1.ToString() + "</span> siswa</span>" +
                                                                    "</sup>"
                                                                )
                                                                .Replace(
                                                                    s_tag_jml_siswa_semester_2,
                                                                    "&nbsp;&nbsp;" +
                                                                    "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                        "<span style=\"font-weight: normal;\"><span style=\"font-weight: bold;\">" + i_jumlah_siswa_semester_2.ToString() + "</span> siswa</span>" +
                                                                    "</sup>"
                                                                ) +
                                                            "</ul>" +
                                                        "</nav>" +
                                                        "<div class=\"card-inner\" style=\"max-height: 150px; overflow-y: auto; margin: 0px; padding : 15px;\">" +
                                                            "<div class=\"tab-content\">" +
                                                                s_tabs_panggilan +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>";
                            s_panggilan = s_tabs_panggilan;
                        }

                        if (s_panggilan.Trim() != "")
                        {
                            s_panggilan = "<div style=\"width: 100%; text-align: center;\">" +
                                            s_panggilan +
                                          "</div>";
                        }

                        string bg_warna = arr_bg[(id % arr_bg_image.Length)];
                        string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                        string bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);
                        string bg_image_card = ""; // ResolveUrl("~/Application_CLibs/images/kelas/bg.png");
                        string s_url_ekskul = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_EKSKUL.ROUTE);

                        lst_html_card.Add(
                            "<div class=\"col-xs-6\">" +
                                "<div class=\"card\" style=\"margin-top: 10px; border-radius: 5px; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;\">" +
                                    "<div class=\"card-main\" style=\"background: url(" + bg_image_card + "); background-position: bottom right; background-repeat: no-repeat;\">" +
                                        "<div class=\"card-action\" style=\"background: url(" + bg_image + "); background-position: top right; background-color: " + bg_warna +
                                                                            "; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;\">" +
                                            "<p class=\"card-heading\" style=\"margin-bottom: 0px; margin-top: 15px; color: white;\">" +
                                                (
                                                    kelas_ekskul.Length <= 2
                                                    ? "<span style=\"color: white; font-weight: normal; font-size: larger;\">Kelas</span>" +
                                                      "&nbsp;"
                                                    : ""
                                                ) +
                                                "<span style=\"color: white; font-weight: bold; font-size: larger;\">" + kelas_ekskul + "</span>" +
                                                "<div style=\"font-size: medium; color: white;\">" + mapel_ekskul.TahunAjaran + "</div>" +
                                                "<div style=\"font-size: medium; color: white; font-size: medium; font-weight: bold;\">" +
                                                    "<span style=\"font-weight: normal; color: white;\">" +
                                                        Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(mapel_ekskul.Rel_Mapel)) +
                                                        "&nbsp;:&nbsp;" +
                                                    "<span>" +
                                                    "<span style=\"font-weight: bold; color: white;\">" +
                                                        m_mapel.Nama +
                                                    "<span>" +
                                                "</div>" +
                                                "<sup class=\"badge\" style=\"display: none; margin-top: 10px; margin-left: -5px;\">" +
                                                    Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(mapel_ekskul.Rel_Mapel)) +
                                                "</sup>" +
                                            "</p>" +
                                        "</div>" +
                                        "<div class=\"card-inner\" style=\"margin-top: 0px; color: grey; margin-left: 0px; margin-right: 0px; margin-bottom: 0px;\">" +
                                            s_panggilan +
                                        "</div>" +
                                        "<div class=\"card-action\" style=\"padding-left: 10px; padding-right: 10px;\">" +
                                            "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; margin-right: 0px; color: grey;\">" +
                                                "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                    "href=\"" + s_url_ekskul +
                                                                "?t=" + RandomLibs.GetRndTahunAjaran(mapel_ekskul.TahunAjaran) +
                                                                "&ft=" + Libs.Encryptdata(file_img) +
                                                                (
                                                                    m_mapel.Kode.ToString().Trim() != ""
                                                                    ? "&m=" + m_mapel.Kode.ToString()
                                                                    : ""
                                                                ) +
                                                                (
                                                                    kelas_ekskul_url.Trim() != ""
                                                                    ? "&k=" + kelas_ekskul_url
                                                                    : ""
                                                                ) +
                                                            "\" " +
                                                    "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                    "<i class=\"fa fa-folder-open\"></i>" +
                                                    "&nbsp;&nbsp;Buka" +
                                                "</a>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>"
                        );
                        id++;
                    }
                    //end cek & list mapel ekskul
                }
                else if (unit_mengajar.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                {
                    lst_mapel_ajar_SD = lst_mapel_ajar_SD.OrderBy(m => m.UrutanLevel).ToList();
                    //cek & list mapel ekskul
                    foreach (var mapel_ekskul in lst_mapel_ajar_SD.Select(m => new { m.TahunAjaran, m.Semester, m.Rel_Mapel }).Distinct().ToList())
                    {
                        var lst_struktur_nilai = AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(
                                        mapel_ekskul.TahunAjaran, mapel_ekskul.Semester
                                    ).FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == mapel_ekskul.Rel_Mapel.ToString().ToUpper().Trim()).ToList();

                        foreach (var m_sn_ekskul in lst_struktur_nilai)
                        {
                            var lst_ajar_ekskul = lst_mapel_ajar_SD.FindAll(
                                    m => m.TahunAjaran == mapel_ekskul.TahunAjaran &&
                                         m.Semester == mapel_ekskul.Semester &&
                                         m.Rel_Mapel.ToString().Trim().ToUpper() == mapel_ekskul.Rel_Mapel.ToString().Trim().ToUpper() &&
                                         (
                                            m.Rel_Kelas.ToString().ToUpper().Trim() == m_sn_ekskul.Rel_Kelas.ToString().ToUpper().Trim() ||
                                            m.Rel_Kelas.ToString().ToUpper().Trim() == m_sn_ekskul.Rel_Kelas2.ToString().ToUpper().Trim() ||
                                            m.Rel_Kelas.ToString().ToUpper().Trim() == m_sn_ekskul.Rel_Kelas3.ToString().ToUpper().Trim() ||
                                            m.Rel_Kelas.ToString().ToUpper().Trim() == m_sn_ekskul.Rel_Kelas4.ToString().ToUpper().Trim() ||
                                            m.Rel_Kelas.ToString().ToUpper().Trim() == m_sn_ekskul.Rel_Kelas5.ToString().ToUpper().Trim() ||
                                            m.Rel_Kelas.ToString().ToUpper().Trim() == m_sn_ekskul.Rel_Kelas6.ToString().ToUpper().Trim()
                                         )
                                ).OrderBy(m => m.UrutanLevel).ToList();

                            jml_anak = 0;
                            string kelas_ekskul = "";
                            string kelas_ekskul_url = "";
                            string s_panggilan = "";
                            string s_tabs_panggilan = "";
                            string s_tabs_semester = "";
                            string s_tag_jml_siswa_semester_1 = "<sm1>";
                            string s_tag_jml_siswa_semester_2 = "<sm2>";

                            int i_jumlah_siswa_semester_1 = 0;
                            int i_jumlah_siswa_semester_2 = 0;

                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(mapel_ekskul.Rel_Mapel.ToString());
                            Sekolah m_sekolah = new Sekolah();

                            for (int i_semester = 1; i_semester <= 2; i_semester++)
                            {
                                kelas_ekskul = "";
                                kelas_ekskul_url = "";

                                string s_panggilan_semester = "";
                                string id_per_semester = "ID_" + Guid.NewGuid().ToString().Replace("-", "");

                                s_tabs_semester += "<li" + (Libs.GetSemesterByTanggal(DateTime.Now) == i_semester ? " class=\"active\"" : "") + ">" +
                                                        "<a class=\"waves-attach\" data-toggle=\"tab\" href=\"#" + id_per_semester + "\">" +
                                                            "Semester <span style=\"font-weight: bold;\">" + i_semester.ToString() + "</span>" +
                                                            (i_semester == 1 ? s_tag_jml_siswa_semester_1 : "") +
                                                            (i_semester == 2 ? s_tag_jml_siswa_semester_2 : "") +
                                                        "</a>" +
                                                   "</li>";

                                foreach (var item_ajar_ekskul in lst_ajar_ekskul)
                                {

                                    m_kelas = DAO_Kelas.GetByID_Entity(item_ajar_ekskul.Rel_Kelas.ToString());

                                    if (m_kelas != null && m_mapel != null)
                                    {
                                        if (m_kelas.Nama != null)
                                        {

                                            m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                                            if (m_sekolah != null)
                                            {
                                                if (m_sekolah.Nama != null)
                                                {
                                                    kelas_ekskul += (kelas_ekskul.Trim() != "" && m_kelas.Nama.Trim() != "" ? ", " : "") +
                                                                    m_kelas.Nama;
                                                    kelas_ekskul_url += m_kelas.Kode.ToString() + ";";

                                                    var lst_ekskul_sd =
                                                            AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_PilihEkstrakurikuler.GetByTABySMByMapelByKelas_Entity(
                                                                    mapel_ekskul.TahunAjaran, mapel_ekskul.Semester, mapel_ekskul.Rel_Mapel, item_ajar_ekskul.Rel_Kelas.ToString()
                                                                );

                                                    int id_siswa = 0;
                                                    foreach (var ekskul_sd in lst_ekskul_sd)
                                                    {
                                                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                                                            mapel_ekskul.TahunAjaran,
                                                            mapel_ekskul.Semester,
                                                            ekskul_sd.Rel_Siswa.ToString());
                                                        if (m_siswa != null)
                                                        {
                                                            if (m_siswa.Nama != null)
                                                            {
                                                                if (m_siswa.Panggilan.Trim() != "")
                                                                {
                                                                    s_panggilan_semester +=
                                                                                    "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                                                    s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";
                                                                    id_siswa++;
                                                                    jml_anak++;

                                                                    if (i_semester == 1) i_jumlah_siswa_semester_1++;
                                                                    if (i_semester == 2) i_jumlah_siswa_semester_2++;
                                                                }
                                                                else
                                                                {
                                                                    s_panggilan_semester +=
                                                                                    "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";

                                                                    s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";
                                                                    id_siswa++;
                                                                    jml_anak++;

                                                                    if (i_semester == 1) i_jumlah_siswa_semester_1++;
                                                                    if (i_semester == 2) i_jumlah_siswa_semester_2++;
                                                                }
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                        }
                                    }
                                }

                                if (s_panggilan_semester.Trim() != "")
                                {
                                    s_tabs_panggilan += "<div class=\"tab-pane fade" + (Libs.GetSemesterByTanggal(DateTime.Now) == i_semester ? " active in" : "") + "\" id=\"" + id_per_semester + "\">" +
                                                            "<div>" +
                                                                s_panggilan_semester +
                                                            "</div>" +
                                                         "</div>";
                                }
                                else
                                {
                                    s_tabs_panggilan += "<div class=\"tab-pane fade" + (Libs.GetSemesterByTanggal(DateTime.Now) == i_semester ? " active in" : "") + "\" id=\"" + id_per_semester + "\">" +
                                                            "<div>" +
                                                                "<div style=\"margin: 0 auto; display: table; color: grey; height: 80px;\">" +
                                                                    "<br />" +
                                                                    "<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;Data Kosong" +
                                                                "</div>" +
                                                            "</div>" +
                                                         "</div>";
                                }
                            }


                            if (s_tabs_panggilan.Trim() != "")
                            {
                                s_tabs_panggilan = "<div class=\"card\" style=\"margin-top: 0px; margin-bottom: 0px; box-shadow: none; border-style: none; border-width: 0px; border-color: #dddddd; box-shadow: none;\">" +
                                                        "<div class=\"card-main\">" +
                                                            "<nav class=\"tab-nav margin-top-no margin-bottom-no\">" +
                                                                "<ul class=\"nav nav-justified\">" +
                                                                    s_tabs_semester
                                                                    .Replace(
                                                                        s_tag_jml_siswa_semester_1,
                                                                        "&nbsp;&nbsp;" +
                                                                        "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            "<span style=\"font-weight: normal;\"><span style=\"font-weight: bold;\">" + i_jumlah_siswa_semester_1.ToString() + "</span> siswa</span>" +
                                                                        "</sup>"
                                                                    )
                                                                    .Replace(
                                                                        s_tag_jml_siswa_semester_2,
                                                                        "&nbsp;&nbsp;" +
                                                                        "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                            "<span style=\"font-weight: normal;\"><span style=\"font-weight: bold;\">" + i_jumlah_siswa_semester_2.ToString() + "</span> siswa</span>" +
                                                                        "</sup>"
                                                                    ) +
                                                                "</ul>" +
                                                            "</nav>" +
                                                            "<div class=\"card-inner\" style=\"max-height: 150px; overflow-y: auto; margin: 0px; padding : 15px;\">" +
                                                                "<div class=\"tab-content\">" +
                                                                    s_tabs_panggilan +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>";
                                s_panggilan = s_tabs_panggilan;
                            }

                            if (s_panggilan.Trim() != "")
                            {
                                s_panggilan = "<div style=\"width: 100%; text-align: center;\">" +
                                                s_panggilan +
                                              "</div>";
                            }

                            string bg_warna = arr_bg[(id % arr_bg_image.Length)];
                            string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                            string bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);
                            string bg_image_card = ""; // ResolveUrl("~/Application_CLibs/images/kelas/bg.png");
                            string s_url_ekskul = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.ROUTE);

                            if (kelas_ekskul.Trim() != "")
                            {
                                lst_html_card.Add(
                                    "<div class=\"col-xs-6\">" +
                                        "<div class=\"card\" style=\"margin-top: 10px; border-radius: 5px; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;\">" +
                                            "<div class=\"card-main\" style=\"background: url(" + bg_image_card + "); background-position: bottom right; background-repeat: no-repeat;\">" +
                                                "<div class=\"card-action\" style=\"background: url(" + bg_image + "); background-position: top right; background-color: " + bg_warna +
                                                                                    "; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;\">" +
                                                    "<p class=\"card-heading\" style=\"margin-bottom: 0px; margin-top: 15px; color: white;\">" +
                                                        (
                                                            kelas_ekskul.Length <= 2
                                                            ? "<span style=\"color: white; font-weight: normal; font-size: larger;\">Kelas</span>" +
                                                              "&nbsp;"
                                                            : ""
                                                        ) +
                                                        "<span style=\"color: white; font-weight: bold; font-size: larger;\">" + kelas_ekskul + "</span>" +
                                                        "<div style=\"font-size: medium; color: white;\">" + mapel_ekskul.TahunAjaran + "</div>" +
                                                        "<div style=\"font-size: medium; color: white; font-size: medium; font-weight: bold;\">" +
                                                            "<span style=\"font-weight: normal; color: white;\">" +
                                                                Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(mapel_ekskul.Rel_Mapel)) +
                                                                "&nbsp;:&nbsp;" +
                                                            "<span>" +
                                                            "<span style=\"font-weight: bold; color: white;\">" +
                                                                m_mapel.Nama +
                                                            "<span>" +
                                                        "</div>" +
                                                        "<sup class=\"badge\" style=\"display: none; margin-top: 10px; margin-left: -5px;\">" +
                                                            Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(mapel_ekskul.Rel_Mapel)) +
                                                        "</sup>" +
                                                    "</p>" +
                                                "</div>" +
                                                "<div class=\"card-inner\" style=\"margin-top: 0px; color: grey; margin-left: 0px; margin-right: 0px; margin-bottom: 0px;\">" +
                                                    s_panggilan +
                                                "</div>" +
                                                "<div class=\"card-action\" style=\"padding-left: 10px; padding-right: 10px;\">" +
                                                    "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; margin-right: 0px; color: grey;\">" +
                                                        "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                            "href=\"" + s_url_ekskul +
                                                                        "?t=" + RandomLibs.GetRndTahunAjaran(mapel_ekskul.TahunAjaran) +
                                                                        "&ft=" + Libs.Encryptdata(file_img) +
                                                                        (
                                                                            m_mapel.Kode.ToString().Trim() != ""
                                                                            ? "&m=" + m_mapel.Kode.ToString()
                                                                            : ""
                                                                        ) +
                                                                        (
                                                                            kelas_ekskul_url.Trim() != ""
                                                                            ? "&k=" + kelas_ekskul_url
                                                                            : ""
                                                                        ) +
                                                                    "\" " +
                                                            "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                            "<i class=\"fa fa-folder-open\"></i>" +
                                                            "&nbsp;&nbsp;Buka" +
                                                        "</a>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>"
                                );
                                id++;
                            }

                        }
                    }
                    //end cek & list mapel ekskul
                }
            }

            //formasi ekskul SMP
            var lst_mapel_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetMapelByGuruByTA_Entity(
                    Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow()
                );
            var lst_formasi_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetKelasFormasiByGuruByTA_Entity(
                    Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow()
                );
            var lst_formasi_mapel_kelas = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetFormasiMapelByGuruByTA_Entity(
                    Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow()
                );

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

                    string s_panggilan = "";
                    string kode_sn = "";

                    string s_tabs_ekskul_semester = "";
                    string s_tabs_panggilan = "";

                    int i_jumlah_siswa_semester = 0;
                    foreach (var item_formasi_mapel_kelas in lst_formasi_mapel_kelas.FindAll(
                        m1 => m1.Rel_Mapel.ToUpper().Trim() == item_formasi_kelas.Rel_Mapel.ToString().ToUpper().Trim() &&
                              m1.Rel_Kelas.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas1.ToString().ToUpper().Trim() &&
                              m1.Rel_Kelas2.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas2.ToString().ToUpper().Trim() &&
                              m1.Rel_Kelas3.ToUpper().Trim() == item_formasi_kelas.Rel_Kelas3.ToString().ToUpper().Trim()
                        ).OrderBy(m0 => m0.Semester)
                    )
                    {
                        i_jumlah_siswa_semester = 0;
                        kode_sn = item_formasi_mapel_kelas.KodeSN;
                        
                        string s_panggilan_semester = "";
                        string id_per_semester = "ID_" + Guid.NewGuid().ToString().Replace("-", "");
                        
                        var formasi_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_FormasiEkskul.GetByID_Entity(
                                item_formasi_mapel_kelas.KodeFormasiEkskul.ToString()
                            );

                        int id_siswa = 0;

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
                                                if (m_siswa.Panggilan.Trim() != "")
                                                {
                                                    if (m_siswa.Panggilan.Trim() != "")
                                                    {
                                                        s_panggilan_semester +=
                                                                        "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                                        s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";
                                                        id_siswa++;
                                                        jml_anak++;

                                                        i_jumlah_siswa_semester++;
                                                    }
                                                    else
                                                    {
                                                        s_panggilan_semester +=
                                                                        "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";

                                                        s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                                "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";
                                                        id_siswa++;
                                                        jml_anak++;

                                                        i_jumlah_siswa_semester++;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                                if (s_panggilan_semester.Trim() != "")
                                {
                                    s_tabs_panggilan += "<div class=\"tab-pane fade" + (Libs.GetSemesterByTanggal(DateTime.Now) == Libs.GetStringToInteger(item_formasi_mapel_kelas.Semester) ? " active in" : "") + "\" id=\"" + id_per_semester + "\">" +
                                                            "<div>" +
                                                                s_panggilan_semester +
                                                            "</div>" +
                                                         "</div>";
                                }
                                else
                                {
                                    s_tabs_panggilan += "<div class=\"tab-pane fade" + (Libs.GetSemesterByTanggal(DateTime.Now) == Libs.GetStringToInteger(item_formasi_mapel_kelas.Semester) ? " active in" : "") + "\" id=\"" + id_per_semester + "\">" +
                                                            "<div>" +
                                                                "<div style=\"margin: 0 auto; display: table; color: grey; height: 80px;\">" +
                                                                    "<br />" +
                                                                    "<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;Data Kosong" +
                                                                "</div>" +
                                                            "</div>" +
                                                         "</div>";
                                }
                            }
                        }

                        s_tabs_ekskul_semester += "<li" + (Libs.GetSemesterByTanggal(DateTime.Now) == Libs.GetStringToInteger(item_formasi_mapel_kelas.Semester) ? " class=\"active\"" : "") + ">" +
                                                        "<a class=\"waves-attach\" data-toggle=\"tab\" href=\"#" + id_per_semester + "\">" +
                                                            "Semester <span style=\"font-weight: bold;\">" + item_formasi_mapel_kelas.Semester.ToString() + "</span>" +
                                                            "&nbsp;&nbsp;" +
                                                            "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                "<span style=\"font-weight: normal;\"><span style=\"font-weight: bold;\">" + i_jumlah_siswa_semester.ToString() + "</span> siswa</span>" +
                                                            "</sup>" +
                                                        "</a>" +
                                                   "</li>";

                    }

                    if (s_tabs_panggilan.Trim() != "")
                    {
                        s_tabs_panggilan = "<div class=\"card\" style=\"margin-top: 0px; margin-bottom: 0px; box-shadow: none; border-style: none; border-width: 0px; border-color: #dddddd; box-shadow: none;\">" +
                                                "<div class=\"card-main\">" +
                                                    "<nav class=\"tab-nav margin-top-no margin-bottom-no\">" +
                                                        "<ul class=\"nav nav-justified\">" +
                                                            s_tabs_ekskul_semester +
                                                        "</ul>" +
                                                    "</nav>" +
                                                    "<div class=\"card-inner\" style=\"max-height: 150px; overflow-y: auto; margin: 0px; padding : 15px;\">" +
                                                        "<div class=\"tab-content\">" +
                                                            s_tabs_panggilan +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>";
                        s_panggilan = s_tabs_panggilan;
                    }

                    if (s_panggilan.Trim() != "")
                    {
                        s_panggilan = "<div style=\"width: 100%; text-align: center;\">" +
                                        s_panggilan +
                                      "</div>";
                    }

                    string bg_warna = arr_bg[(id % arr_bg_image.Length)];
                    string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                    string bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);
                    string bg_image_card = ""; // ResolveUrl("~/Application_CLibs/images/kelas/bg.png");
                    string s_url_ekskul = ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE);
                    string kelas_ekskul = AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetNamaKelasEkskul(kode_sn.ToString());

                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(item_formasi_kelas.Rel_Mapel.ToString());

                    lst_html_card.Add(
                        "<div class=\"col-xs-6\">" +
                            "<div class=\"card\" style=\"margin-top: 10px; border-radius: 5px; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;\">" +
                                "<div class=\"card-main\" style=\"background: url(" + bg_image_card + "); background-position: bottom right; background-repeat: no-repeat;\">" +
                                    "<div class=\"card-action\" style=\"background: url(" + bg_image + "); background-position: top right; background-color: " + bg_warna +
                                                                        "; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;\">" +
                                        "<p class=\"card-heading\" style=\"margin-bottom: 0px; margin-top: 15px; color: white;\">" +
                                            (
                                                kelas_ekskul.Length <= 2
                                                ? "<span style=\"color: white; font-weight: normal; font-size: larger;\">Kelas</span>" +
                                                  "&nbsp;"
                                                : ""
                                            ) +
                                            "<span style=\"color: white; font-weight: bold; font-size: larger;\">" + kelas_ekskul + "</span>" +
                                            "<div style=\"font-size: medium; color: white;\">" + Libs.GetTahunAjaranNow() + "</div>" +
                                            "<div style=\"font-size: medium; color: white; font-size: medium; font-weight: bold;\">" +
                                                "<span style=\"font-weight: normal; color: white;\">" +
                                                    Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(item_formasi_kelas.Rel_Mapel.ToString())) +
                                                    "&nbsp;:&nbsp;" +
                                                "<span>" +
                                                "<span style=\"font-weight: bold; color: white;\">" +
                                                    m_mapel.Nama +
                                                "<span>" +
                                            "</div>" +
                                            "<sup class=\"badge\" style=\"display: none; margin-top: 10px; margin-left: -5px;\">" +
                                                Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(item_formasi_kelas.Rel_Mapel.ToString())) +
                                            "</sup>" +
                                        "</p>" +
                                    "</div>" +
                                    "<div class=\"card-inner\" style=\"margin-top: 0px; color: grey; margin-left: 0px; margin-right: 0px; margin-bottom: 0px;\">" +
                                        s_panggilan +
                                    "</div>" +
                                    "<div class=\"card-action\" style=\"padding-left: 10px; padding-right: 10px;\">" +
                                        "<div class=\"card-action-btn pull-left\" style=\"margin-left: 0px; margin-right: 0px; color: grey;\">" +
                                            "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                "href=\"" + s_url_ekskul +
                                                            "?t=" + RandomLibs.GetRndTahunAjaran(Libs.GetTahunAjaranNow()) +
                                                            "&ft=" + Libs.Encryptdata(file_img) +
                                                            (
                                                                m_mapel.Kode.ToString().Trim() != ""
                                                                ? "&m=" + m_mapel.Kode.ToString()
                                                                : ""
                                                            ) +
                                                            (
                                                                s_kelas_ekskul.Trim() != ""
                                                                ? "&k=" + s_kelas_ekskul
                                                                : ""
                                                            ) +
                                                        "\" " +
                                                "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                "<i class=\"fa fa-folder-open\"></i>" +
                                                "&nbsp;&nbsp;Buka" +
                                            "</a>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>"
                    );
                    id++;
                }
            }
            //end formasi ekskul SMP

            int nomor_card = 1;
            foreach (var item_html_card in lst_html_card)
            {
                if (nomor_card % 2 == 0)
                {
                    html_card += item_html_card +
                                 "</div>";
                }
                else
                {
                    html_card += "<div class=\"row\">" +
                                 item_html_card;
                }
                nomor_card++;
            }
            if (lst_html_card.Count % 2 != 0)
            {
                html_card = html_card +
                            "</div>";
            }

            ltrKelas.Text = (
                                lst_kelasguru.Count > 0
                                ? "<label style=\"font-weight: bold; color: grey; margin-top: 15px;\">" +
                                        "<i class=\"fa fa-hashtag\"></i>" +
                                        "&nbsp;" +
                                        "Kelas Saya" +
                                  "</label>" +                                  
                                  "<hr style=\"margin-bottom: 10px; margin-top: 10px;\" />"
                                : ""
                            ) +

                          
                                                            "<div class=\"tooltip\">" +
                                                                "<a class=\"btn btn-flat waves-attach waves-effect\" " +
                                                                    "href=\"" + ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.ROUTE) +
                                                                         //"?rs=" + Libs.LOGGED_USER_M.NoInduk ""
                                                                            "\" " +
                                                                    "style=\"margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;\">" +
                                                                    "<i class=\"fa fa-folder-open\"></i> CBT" +
                                                                "</a>" +
                                                                "<div class=\"top\">" +
                                                                    "Buka" +
                                                                    "<i></i>" +
                                                                "</div>" +
                                                            "</div>" +
                                                   

                            "<div class=\"row\">" +
                                html_card +
                            "</div>";

            div_laporan.Visible = false;
            div_struktur_penilaian_smp_by_guru.Visible = false;
            if (Libs.LOGGED_USER_M.NoInduk.Trim() == "")
            {
                if (Libs.LOGGED_USER_M.UserID == SessionLogin.USR_GURU_TK)
                {
                    div_menu_ortu_1.Visible = false;
                    div_menu_ortu_2.Visible = false;
                    div_menu_ortu_3.Visible = false;

                    div_master_data.Visible = false;
                    div_pembelajaran_siswa_unit_kb.Visible = false;
                    div_pembelajaran_siswa_unit_tk.Visible = false;
                    div_pembelajaran_siswa_unit_sd.Visible = false;
                    div_pembelajaran_siswa_unit_smp.Visible = false;
                    div_pembelajaran_siswa_unit_sma.Visible = false;
                    div_penilaian_sma_1.Visible = true;
                }
                else if (Libs.LOGGED_USER_M.UserID == SessionLogin.USR_GURU)
                {
                    div_menu_ortu_1.Visible = false;
                    div_menu_ortu_2.Visible = false;
                    div_menu_ortu_3.Visible = false;

                    div_master_data.Visible = false;
                    div_pembelajaran_siswa_unit_kb.Visible = false;
                    div_pembelajaran_siswa_unit_tk.Visible = false;
                    div_pembelajaran_siswa_unit_sd.Visible = false;
                    div_pembelajaran_siswa_unit_smp.Visible = false;
                    div_pembelajaran_siswa_unit_sma.Visible = false;
                    div_penilaian_sma_1.Visible = false;
                }
                else if (Libs.LOGGED_USER_M.UserID == SessionLogin.USR_ORTU)
                {
                    div_menu_ortu_1.Visible = true;
                    div_menu_ortu_2.Visible = true;
                    div_menu_ortu_3.Visible = true;

                    div_master_data.Visible = false;
                    div_pembelajaran_siswa_unit_kb.Visible = false;
                    div_pembelajaran_siswa_unit_tk.Visible = false;
                    div_pembelajaran_siswa_unit_sd.Visible = false;
                    div_pembelajaran_siswa_unit_smp.Visible = false;
                    div_pembelajaran_siswa_unit_sma.Visible = false;
                }
                else
                {
                    div_menu_ortu_1.Visible = true;
                    div_menu_ortu_2.Visible = true;
                    div_menu_ortu_3.Visible = true;

                    div_master_data.Visible = true;
                    div_pembelajaran_siswa_unit_kb.Visible = false;
                    div_pembelajaran_siswa_unit_tk.Visible = true;
                    div_pembelajaran_siswa_unit_sd.Visible = true;
                    div_pembelajaran_siswa_unit_smp.Visible = true;
                    div_pembelajaran_siswa_unit_sma.Visible = true;
                }
            }
            else
            {
                
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_kb.Visible = false;
                div_pembelajaran_siswa_unit_tk.Visible = false;
                div_pembelajaran_siswa_unit_sd.Visible = false;
                div_pembelajaran_siswa_unit_smp.Visible = false;
                div_pembelajaran_siswa_unit_sma.Visible = false;

                div_struktur_penilaian_smp_by_guru.Visible = DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP);
                div_pilih_kelas_smp_by_guru.Visible = (lst_kelasguru.Count > 100 ? true : false);
            }

            if (Libs.LOGGED_USER_M.UserID == "rama.sundara")
            {
                div_menu_ortu_1.Visible = true;
                div_menu_ortu_2.Visible = true;
                div_menu_ortu_3.Visible = true;
                div_penilaian_smp_1.Visible = true;
                div_penilaian_sma_1.Visible = true;

                div_master_data.Visible = true;
                div_pembelajaran_siswa_unit_kb.Visible = true;
                div_pembelajaran_siswa_unit_tk.Visible = true;
                div_pembelajaran_siswa_unit_sd.Visible = true;
                div_pembelajaran_siswa_unit_smp.Visible = true;
                div_pembelajaran_siswa_unit_sma.Visible = true;
                div_struktur_penilaian_smp.Visible = true;
            }

            if (DAO_Pegawai.GetCountFormasiGuruByID_Entity(Libs.LOGGED_USER_M.NoInduk) > 0)
            {
                div_materi_pembelajaran_oleh_guru.Visible = true;
            }

            //div_materi_pembelajaran.Visible = false;
            //div_materi_pembelajaran_oleh_guru.Visible = false;
            if (DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP))
            {
                div_struktur_penilaian_smp_by_guru.Visible = true;
            }

            //materi pembelajaran oleh guru
            //div_materi_pembelajaran_oleh_guru.Visible = true;
            //div_materi_pembelajaran.Visible = true;        

            if (Libs.LOGGED_USER_M.UserID == "imranto" ||
                Libs.LOGGED_USER_M.UserID == "risa.khoerunnisa" ||
                Libs.LOGGED_USER_M.UserID == "gugum.prayoga" ||
                Libs.LOGGED_USER_M.UserID == "wuri.andayani" ||
                Libs.LOGGED_USER_M.UserID == "hadi.saputro" ||
                Libs.LOGGED_USER_M.UserID == "gina.zahra" ||
                Libs.LOGGED_USER_M.UserID == "divpend.smp")
            {
                div_struktur_penilaian_smp_by_guru.Visible = false;
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_smp.Visible = true;
                //div_materi_pembelajaran_oleh_guru.Visible = false;
                div_struktur_penilaian_smp.Visible = true;
            }
            else if (Libs.LOGGED_USER_M.UserID == "hasunahx") //bk
            {
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_struktur_penilaian_smp.Visible = false;
                div_desain_rapor_smp.Visible = false;
                div_proses_rapor_smp.Visible = false;
                div_proses_rapor_smp.Visible = false;
                div_pembelajaran_siswa_unit_smp.Visible = true;
                //div_materi_pembelajaran_oleh_guru.Visible = false;
            }
            else if (Libs.LOGGED_USER_M.UserID == "madinah" ||
                Libs.LOGGED_USER_M.UserID == "diajeng.ayu" ||
                Libs.LOGGED_USER_M.UserID == "ira.intasari" ||
                Libs.LOGGED_USER_M.UserID == "divpend.kb")
            {
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_kb.Visible = true;
                //div_materi_pembelajaran_oleh_guru.Visible = false;
                div_struktur_penilaian_sd.Visible = false;
            }
            else if (Libs.LOGGED_USER_M.UserID == "wiwid.fajarianto" ||
                Libs.LOGGED_USER_M.UserID == "maryamah" ||
                Libs.LOGGED_USER_M.UserID == "iskandar" ||
                Libs.LOGGED_USER_M.UserID == "naily.tanjung" ||
                Libs.LOGGED_USER_M.UserID == "divpend.tk")
            {
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_tk.Visible = true;
                //div_materi_pembelajaran_oleh_guru.Visible = false;
                div_struktur_penilaian_sd.Visible = false;
            }
            else if (
                    Libs.LOGGED_USER_M.UserID == "devi.rossari" ||
                    Libs.LOGGED_USER_M.UserID == "titin.juhartini" ||
                    Libs.LOGGED_USER_M.UserID == "tety.afianty" ||
                    Libs.LOGGED_USER_M.UserID == "tato.hendarto" ||
                    Libs.LOGGED_USER_M.UserID == "prihanti.handayani"
                ) {
                div_laporan.Visible = true;

                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_sd.Visible = false;
                //div_materi_pembelajaran_oleh_guru.Visible = false;
                div_struktur_penilaian_sd.Visible = false;
            }
            else if (Libs.LOGGED_USER_M.UserID == "endang.purwaningsih" ||
                Libs.LOGGED_USER_M.UserID == "lulu" ||
                //Libs.LOGGED_USER_M.UserID == "dian.safarulloh" ||
                Libs.LOGGED_USER_M.UserID == "sumar" ||
                Libs.LOGGED_USER_M.UserID == "hadiana" ||
                Libs.LOGGED_USER_M.UserID == "muhamad.badruzaman" ||
                Libs.LOGGED_USER_M.UserID == "divpend.sd"
                )
            {
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_sd.Visible = true;
                //div_materi_pembelajaran_oleh_guru.Visible = false;
                div_struktur_penilaian_sd.Visible = true;
            }
            else if (Libs.LOGGED_USER_M.UserID == "suci.ismawati" ||
                Libs.LOGGED_USER_M.UserID == "mulyadi" ||
                Libs.LOGGED_USER_M.UserID == "kamelia.muliani" ||
                Libs.LOGGED_USER_M.UserID == "muh.ridwan" ||
                Libs.LOGGED_USER_M.UserID == "maruto.santoso" ||
                Libs.LOGGED_USER_M.UserID == "maruto.santoso" ||
                Libs.LOGGED_USER_M.UserID == "leni.mulyani" ||
                Libs.LOGGED_USER_M.UserID == "diah.sulistiowati" ||
                Libs.LOGGED_USER_M.UserID == "ali.akbar" ||
                Libs.LOGGED_USER_M.UserID == "divpend.sma")
            {
                div_menu_ortu_1.Visible = false;
                div_menu_ortu_2.Visible = false;
                div_menu_ortu_3.Visible = false;

                div_row_data_master_data_1.Visible = false;
                div_data_divisi.Visible = false;
                div_data_unit_non_sekolah.Visible = false;
                div_data_unit_sekolah.Visible = false;
                div_data_kelas.Visible = false;

                div_row_data_master_data_2.Visible = false;
                div_data_karyawan.Visible = false;
                div_data_siswa.Visible = false;
                div_data_mapel.Visible = false;
                div_data_ruang_kelas.Visible = false;

                div_row_data_master_data_3.Visible = false;
                div_data_formasi_guru_kelas.Visible = false;
                div_data_formasi_guru_mapel.Visible = false;
                div_wali_kelas.Visible = false;
                div_jadwal_mapel.Visible = false;
                div_perpustakaan.Visible = false;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_sma.Visible = true;
            }
            else if (
                Libs.LOGGED_USER_M.UserID == "sri.sumardiyani" ||
                Libs.LOGGED_USER_M.UserID == "yayat.duryatna" ||
                Libs.LOGGED_USER_M.UserID == "muji" ||
                Libs.LOGGED_USER_M.UserID == "wanty.zahara" ||
                Libs.LOGGED_USER_M.UserID == "aisyah.pratiwi" ||
                Libs.LOGGED_USER_M.UserID == "dwi.andriyan"
            )
            {
                //div_materi_pembelajaran_oleh_guru.Visible = false;
                //div_materi_pembelajaran.Visible = false;
                div_perpustakaan.Visible = true;
                div_pengaturan_kunjungan_perpus.Visible = true;
                div_kunjungan_perpus.Visible = true;
                div_jadwal_kunjungan.Visible = true;
                div_master_data.Visible = true;
                div_row_data_master_data_1.Visible = false;
                div_row_data_master_data_2.Visible = false;
                div_row_data_master_data_3.Visible = false;
            }

            if (
                Libs.LOGGED_USER_M.UserID == "tety.afianty"
            )
            {
                //div_row_data_master_data_1.Visible = false;
                //div_row_data_master_data_3.Visible = false;
                //div_data_karyawan.Visible = false;
                //div_data_siswa.Visible = true;
                //div_data_mapel.Visible = false;
                //div_data_ruang_kelas.Visible = false;
                //div_row_data_master_data_2.Visible = true;
                //div_master_data.Visible = true;
                //div_perpustakaan.Visible = false;

                //div_materi_pembelajaran_oleh_guru.Visible = false;
                //div_materi_pembelajaran.Visible = false;
            }

            if (Libs.LOGGED_USER_M.UserID == SessionLogin.USR_ORTU)
            {
                div_menu_ortu_1.Visible = true;
                div_menu_ortu_2.Visible = true;
                div_menu_ortu_3.Visible = true;

                div_master_data.Visible = false;
                div_pembelajaran_siswa_unit_kb.Visible = false;
                div_pembelajaran_siswa_unit_tk.Visible = false;
                div_pembelajaran_siswa_unit_sd.Visible = false;
                div_pembelajaran_siswa_unit_smp.Visible = false;
                div_pembelajaran_siswa_unit_sma.Visible = false;
            }
            //div_materi_pembelajaran.Visible = false;

            if (Libs.LOGGED_USER_M.UserID == "ira.intasari" ||
                Libs.LOGGED_USER_M.UserID == "diajeng.ayu" ||
                Libs.LOGGED_USER_M.UserID == "madinah") div_laporan_presensi_kb.Visible = true;

            if (Libs.LOGGED_USER_M.UserID == "maryamah" ||
                Libs.LOGGED_USER_M.UserID == "naily.tanjung" ||
                Libs.LOGGED_USER_M.UserID == "wiwid.fajarianto") div_laporan_presensi_tk.Visible = true;

            if (Libs.LOGGED_USER_M.UserID == "lulu" ||
                Libs.LOGGED_USER_M.UserID == "endang.purwaningsih") div_laporan_presensi_sd.Visible = true;
            if (Libs.LOGGED_USER_M.UserID == "agus.riyadi" ||
                Libs.LOGGED_USER_M.UserID == "dian.safarulloh")
            {
                div_file_rapor_sd_lts.Visible = false;
                div_file_rapor_sd_semester.Visible = false;
                div_pengaturan_sd.Visible = false;
                div_pengaturan_umum_sd.Visible = false;
                div_row_master_sd_1.Visible = false;
                div_penilaian_sd_1.Visible = false;
                div_penilaian_sd2.Visible = false;

                div_file_rapor_sd_lts.Visible = false;
                div_file_rapor_sd_semester.Visible = false;
                div_pengaturan_sd.Visible = false;
                div_pengaturan_umum_sd.Visible = false;

                div_pembelajaran_sd.Visible = true;
                div_pembelajaran_siswa_unit_sd.Visible = true;
                div_penilaian_sd3.Visible = true;
                div_laporan_presensi_sd.Visible = true;
            }

            if (Libs.LOGGED_USER_M.UserID == "hadi.saputro" ||
                Libs.LOGGED_USER_M.UserID == "gugum.prayoga" ||
                Libs.LOGGED_USER_M.UserID == "gina.zahra" ||
                Libs.LOGGED_USER_M.UserID == "wuri.andayani") div_laporan_presensi_smp.Visible = true;

            if (Libs.LOGGED_USER_M.UserID == "muh.ridwan" ||
                Libs.LOGGED_USER_M.UserID == "maruto.santoso" ||
                Libs.LOGGED_USER_M.UserID == "leni.mulyani" ||
                Libs.LOGGED_USER_M.UserID == "diah.sulistiowati") {
                div_menu_sma_2.Visible = true;
                div_laporan_presensi_sma.Visible = true;
            }
        }

        protected void InitKelasUnit()
        {
            txtParseKelasUnit.Value = "";
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            foreach (Sekolah m_sekolah in lst_sekolah)
            {
                List<KelasDet> lst_kelas = DAO_KelasDet.GetBySekolah_Entity(m_sekolah.Kode.ToString());
                foreach (KelasDet m in lst_kelas)
                {
                    if (m.IsAktif)
                    {
                        txtParseKelasUnit.Value += m_sekolah.Kode.ToString() + "->";
                        txtParseKelasUnit.Value += m.Kode.ToString() +
                                                   "|" +
                                                   m.Nama +
                                                   ";";
                    }
                }
            }            
        }

        protected void InitInput()
        {
            lnkOKShowBiodataSiswa.Attributes["onclick"] = "pb_ok_biodata_siswa.style.display = '' ; " +
                                                          "this.style.display = 'none'; " + 
                                                          "setTimeout (function(){ " +
                                                                "document.location.href = " + 
                                                                    "'" + ResolveUrl(Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) + "' + " +
                                                                    "'?' + " +
                                                                    "'u=' + " + cboUnitSekolahBiodataSiswa.ClientID + ".value + " +
                                                                    "'&' + " +
                                                                    "'t=' + " + cboTahunAjaranBiodataSiswa.ClientID + ".value + " +
                                                                    "'&' + " +
                                                                    "'k=' + " + cboKelasBiodataSiswa.ClientID + ".value" +
                                                                "; " +
                                                          "} , " +
                                                          "500); return false;";

            lnkOKShowSemuaBiodataSiswa.Attributes["onclick"] = 
                                                          "setTimeout (function(){ " +
                                                                "document.location.href = " +
                                                                    "'" + 
                                                                    ResolveUrl(Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) + "?id=b6Y7887bjhdr" +
                                                                    "'" +
                                                                "; " +
                                                          "} , " +
                                                          "500); return false;";

            cboUnitSekolahBiodataSiswa.Attributes["onchange"] = "ShowKelasByUnit(this.value);";

            List<string> lst_tahun_ajaran = Libs.ListTahunAjaran().OrderByDescending(m => m).ToList();
            cboTahunAjaranBiodataSiswa.Items.Clear();
            foreach (string tahun_ajaran in lst_tahun_ajaran)
            {
                cboTahunAjaranBiodataSiswa.Items.Add(new ListItem
                {
                    Value = RandomLibs.GetRndTahunAjaran(tahun_ajaran),
                    Selected = (tahun_ajaran == Libs.GetTahunAjaranNow() ? true : false),
                    Text = tahun_ajaran
                });
            }

            cboTahunPelajaranLedgerNilai.Items.Clear();
            foreach (string tahun_ajaran in lst_tahun_ajaran)
            {
                cboTahunPelajaranLedgerNilai.Items.Add(new ListItem
                {
                    Value = RandomLibs.GetRndTahunAjaran(tahun_ajaran),
                    Text = tahun_ajaran
                });
            }

            //List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).OrderBy(m => m.UrutanJenjang).ToList();
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
            cboUnitSekolahBiodataSiswa.Items.Clear();
            foreach (Sekolah m in lst_sekolah)
            {
                cboUnitSekolahBiodataSiswa.Items.Add(new ListItem {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            ShowKelasByUnit(cboUnitSekolahBiodataSiswa.SelectedValue);

            Sekolah m_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
            if (m_sekolah != null)
            {
                cboKelasLedgerNilai.Items.Clear();
                List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m_sekolah.Kode.ToString()).OrderBy(m => m.UrutanLevel).ToList();
                cboKelasLedgerNilai.Items.Add("");
                foreach (Kelas m in lst_kelas)
                {
                    cboKelasLedgerNilai.Items.Add(new ListItem
                    {
                        Value = m.Kode.ToString(),
                        Text = m.Nama
                    });
                }
            }

            txtTahunPelajaranBukaSemester.Text = Libs.GetTahunAjaranNow();
            txtSemesterPelajaranBukaSemester.Text = Libs.GetSemesterByTanggal(DateTime.Now).ToString();
        }

        protected void InitInfoPerpustakaan()
        {
            string html = "";

            List<PerpustakaanKunjunganRutinDet> lst_kunjungan_det = DAO_PerpustakaanKunjunganRutinDet.GetByGuru_Entity(
                    Libs.LOGGED_USER_M.NoInduk
                );

            foreach (var m in lst_kunjungan_det.Select(m0 => new { m0.Rel_PerpustakaanKunjunganRutin, m0.Hari, m0.Waktu, m0.Rel_KelasDet, m0.Keterangan }).Distinct().ToList())
            {
                KelasDet m_kelasdet = DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet.ToString());
                PerpustakaanKunjunganRutin m_rutin = DAO_PerpustakaanKunjunganRutin.GetByID_Entity(m.Rel_PerpustakaanKunjunganRutin.ToString());
                if (m_rutin != null)
                {
                    if (m_rutin.TahunAjaran != null)
                    {

                        if (m_kelasdet != null)
                        {
                            if (m_kelasdet.Nama != null)
                            {
                                html += "<div style=\"background-color: #1b6c5e; font-weight: bold; color: grey; width: 100%; padding: 10px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 0px; margin-top: 0px; text-align: justify;\">" +
                                            "<div style=\"background-color: #1b6c5e; border-color: #1b6c5e; color: white; border-radius: .25rem; margin: 0px; padding: 10px; border-width: 1px; border-style: solid; padding-top: 0px; padding-bottom: 0px;\">" +
                                                "<label style=\"color: white; font-weight: bold; margin-bottom: 5px;\">" +
                                                    "<i class=\"fa fa-info-circle\"></i>" +
                                                    "&nbsp; " +
                                                    "Kunjungan Rutin : " +
                                                "</label>" +
                                                "<br />" +
                                                "<span style=\"color: white; font-weight: bold;\">" +
                                                    "<i class=\"fa fa-calendar\"></i>" +
                                                    "&nbsp; " +
                                                    Libs.GetNamaHariFromUrutHari(m.Hari) +
                                                    "&nbsp; " +
                                                    m.Waktu +
                                                "</span>" +
                                                ",&nbsp;" +
                                                "<span style=\"color: white; font-weight: normal;\">" +
                                                    "Kelas" +
                                                    "&nbsp;" +
                                                    "<span style=\"font-weight: bold;\">" +
                                                        m_kelasdet.Nama +
                                                    "</span>" +
                                                "</span>" +
                                                "<div style=\"margin-left: 20px;\">" +
                                                    "<span style=\"color: #b6b6b6; font-weight: normal;\">" +
                                                        "TP. " + m_rutin.TahunAjaran +
                                                        (
                                                            m_rutin.IsSemester_1 || m_rutin.IsSemester_2
                                                            ? ", semester " +
                                                              ( 
                                                                m_rutin.IsSemester_1 ? "1" : ""
                                                              ) +
                                                              (
                                                                m_rutin.IsSemester_1 && m_rutin.IsSemester_2 ? " & " : ""
                                                              ) +
                                                              (
                                                                m_rutin.IsSemester_2 ? "2" : ""
                                                              )
                                                            : ""
                                                        ) +
                                                    "</span>" +
                                                    (
                                                        m.Keterangan.Trim() != ""
                                                        ? "<br />" +
                                                          "<span style=\"color: grey; font-weight: normal;\">" +
                                                            m.Keterangan +
                                                          "</span>"
                                                        : ""
                                                    ) +
                                                "</div>" +
                                            "</div>" +
                                        "</div>";
                            }
                        }

                    }
                }             
            }

            if (lst_kunjungan_det.Count == 0 && DAO_PerpustakaanKunjungan.GetByGuru_Entity(Libs.LOGGED_USER_M.NoInduk).Count == 0)
            {
                html = "<div style=\"font-weight: bold; color: grey; width: 100%; background-color: white; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 0px; margin-top: 0px; text-align: justify; border-style: solid; border-width: 1px; border-color: #e9e9e9;\">" +
                            "<span style=\"margin: 0 auto; display: table; color: #bfbfbf; font-weight: normal;\">" +
                                "..:: Kosong ::.." +
                            "</span>" +
                        "</div>";
            }

            ltrInfoPerpustakaan.Text = html;
        }

        protected void InitContentAndURL()
        {
            ltrKalender.Text = Kalender.GetHTMLKalender(DateTime.Now.Month, DateTime.Now.Year);

            List<UserOrtuDet> lst_user_ortu_det = DAO_UserOrtuDet.SelectByUserID(Libs.LOGGED_USER_M.UserID);
            foreach (UserOrtuDet m_det in lst_user_ortu_det)
            {
                lblTagihan.Text = "";
                Siswa m = DAO_Siswa.GetByID_Entity("_", "_", m_det.NIS);
                if (m != null)
                {
                    List<SiswaBiayaDanDibayar> lst_biaya = DAO_SiswaBiaya.GetAllAsTagihan_Entity(DateTime.Now, m_det.NIS);
                    decimal tagihan = 0;
                    foreach (SiswaBiayaDanDibayar item in lst_biaya)
                    {
                        tagihan += (item.Jumlah + item.Denda) - (item.Dibayar + item.DendaDibayar);
                    }
                    lblTagihan.Text = Libs.GetFormatBilangan(tagihan) + ",-";
                }
            }

            foreach (UserOrtuDet m_det in lst_user_ortu_det)
            {
                lblSaldoKantin.Text = "";
                Siswa m = DAO_Siswa.GetByID_Entity("_", "_", m_det.NIS);
                if (m != null)
                {
                    if (m.Nama != null)
                    {
                        foreach (_MutasiKantin item in _DAO_MutasiKantin.GetByNIS_Entity(m_det.NIS))
                        {
                            lblSaldoKantin.Text = Libs.GetFormatBilangan(item.SaldoAkhir) + ",-";
                            return;
                        }
                    }
                }
            }
        }

        protected void ShowKelasByUnit(string kode_unit)
        {
            List<KelasDet> lst_kelas = DAO_KelasDet.GetBySekolah_Entity(kode_unit);
            cboKelasBiodataSiswa.Items.Clear();
            foreach (KelasDet m in lst_kelas)
            {
                if (m.IsAktif)
                {
                    cboKelasBiodataSiswa.Items.Add(new ListItem
                    {
                        Value = m.Kode.ToString(),
                        Text = m.Nama
                    });
                }
            }
        }

        protected void BindKunjunganPerpustakaan()
        {
            sql_ds_kunjungan_perpus.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds_kunjungan_perpus.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            sql_ds_kunjungan_perpus.SelectParameters.Clear();
            sql_ds_kunjungan_perpus.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
            sql_ds_kunjungan_perpus.SelectCommand = DAO_PerpustakaanKunjungan.SP_SELECT_BY_GURU;
            lvDataKunjunganPerpus.DataBind();
        }

        protected void lvDataKunjunganPerpus_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvDataKunjunganPerpus_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName_KunjunganPerpustakaan] = e.StartRowIndex;
        }

        protected void btnShowInfoKunjungan_Click(object sender, EventArgs e)
        {
            ShowInfoKunjungan();
            txtKeyAction.Value = JenisAction.ShowInfoKunjungan.ToString();
        }

        protected void ShowInfoKunjungan()
        {
            string html = "";

            if (txtIDKunjungan.Value.Trim() != "")
            {
                string s_kelas = "";
                PerpustakaanKunjungan m_kunjungan = DAO_PerpustakaanKunjungan.GetByID_Entity(txtIDKunjungan.Value);
                if (m_kunjungan != null)
                {
                    if (m_kunjungan.TahunAjaran != null)
                    {
                        KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(m_kunjungan.Rel_KelasDet);
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                s_kelas = m_kelas.Nama;
                            }
                        }

                        html = "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<label style=\"color: #bfbfbf; font-weight: normal; margin-bottom: 10px;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Tahun Pelajaran & Semester</label>" +
                                        "<div style=\"margin-left: 20px;\">" +                                            
                                            "<span style=\"color: grey; font-weight: bold;\">" + m_kunjungan.TahunAjaran + "</span>" +
                                            "&nbsp;" +
                                            "<sup class=\"badge\" style=\"font-size: x-small; color: grey; background-color: white; border-style: solid; border-width: 1px; border-color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                "Semester " + m_kunjungan.Semester +
                                            "</sup>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                (
                                    s_kelas.Trim() != ""
                                    ? "<div class=\"row\">" +
                                            "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                                "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                            "</div>" +
                                      "</div>" +
                                      "<div class=\"row\">" +
                                            "<div class=\"col-md-12\">" +
                                                "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Kelas</span>" +
                                                "<div style=\"margin-left: 20px;\">" +
                                                    "<span style=\"color: grey; font-weight: bold;\">" + s_kelas + "</span>" +
                                                "</div>" +
                                            "</div>" +
                                      "</div>"
                                    : ""
                                ) +
                                "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Tanggal Kunjungan</span>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" + Libs.GetTanggalIndonesiaFromDate(m_kunjungan.Tanggal, false) + "</span>" +
                                        "</div>" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Jam Ke/Waktu</span>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" +
                                                AI_ERP.Application_Modules.LIBRARY.wf_KunjunganPerpustakaan.GetJamKe(m_kunjungan.Kode.ToString(), false) + 
                                            "</span>" +
                                        "</div>" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                        "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                    "</div>" +
                               "</div>" +
                               "<div class=\"row\">" +
                                    "<div class=\"col-md-12\">" +
                                        "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Status</span>" +
                                        "<div style=\"margin-left: 20px;\">" +
                                            "<span style=\"color: grey; font-weight: bold;\">" +
                                                JenisStatusKunjungan.GetJenisStatus(Convert.ToInt16(m_kunjungan.Status)) +
                                            "</span>" +
                                        "</div>" +
                                    "</div>" +
                               "</div>" +
                               (
                                    Libs.GetHTMLSimpleText(m_kunjungan.Keterangan.Trim()) != ""
                                    ? "<div class=\"row\">" +
                                            "<div class=\"col-md-12\" style=\"padding: 0px;\">" +
                                                "<hr style=\"margin: 10px; border-color: #e6e6e6; border-width: 1px;\" />" +
                                            "</div>" +
                                      "</div>" + 
                                      "<div class=\"row\">" +
                                            "<div class=\"col-md-12\">" +
                                                "<span style=\"color: #bfbfbf; font-weight: normal;\"><i class=\"fa fa-hashtag\"></i>&nbsp;Keterangan</span>" +
                                                "<div style=\"margin-left: 20px;\">" +
                                                    "<span style=\"color: grey; font-weight: bold;\">" +
                                                        m_kunjungan.Keterangan +
                                                    "</span>" +
                                                "</div>" +
                                            "</div>" +
                                      "</div>"
                                    : ""
                               );
                    }
                }
            }

            ltrInfoKunjungan.Text = html;
            txtIDKunjungan.Value = "";
        }

        protected void lnkOKShowSemuaBiodataSiswa_Click(object sender, EventArgs e)
        {

        }

        protected class KelasGuru
        {
            public string Kode { get; set; }
            public string Nama { get; set; }
            public string Urutan { get; set; }
        }

        protected void ListKelasMataPelajaran()
        {
            cboListKelasEkskul.Items.Clear();
            cboListMapelEkskul.Items.Clear();
            cboTahunAjaranEkskul.Items.Clear();

            List<FormasiGuruKelas_ByGuru> lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow()).ToList();
            var lst_kelas_ekskul = lst_kelasguru.Select(m => new { m.Rel_KelasDet, m.KelasDet, m.Urutan }).Distinct().ToList();
            var lst_mapel_ekskul = lst_kelasguru.Select(m => new { m.KodeMapel, m.Mapel }).Distinct().ToList();
            var lst_tahun_ajaran = lst_kelasguru.Select(m => new { m.TahunAjaran }).Distinct().ToList();

            List<KelasGuru> lst_kelas_guru = new List<KelasGuru>();
            lst_kelas_guru.Clear();

            foreach (var item in lst_tahun_ajaran.OrderByDescending(m => m.TahunAjaran).ToList())
            {
                cboTahunAjaranEkskul.Items.Add(new ListItem
                {
                    Value = RandomLibs.GetRndTahunAjaran(item.TahunAjaran),
                    Text = item.TahunAjaran
                });
            }

            foreach (var item in lst_kelas_ekskul.OrderBy(m => m.Urutan).ToList())
            {
                cboListKelasEkskul.Items.Add(new ListItem
                {
                    Value = item.Rel_KelasDet,
                    Text = item.KelasDet
                });
            }

            foreach (var item in lst_mapel_ekskul)
            {
                cboListMapelEkskul.Items.Add(new ListItem
                {
                    Value = item.KodeMapel,
                    Text = item.Mapel
                });
            }
        }

        protected void ListUnitBukaSemester()
        {
            cboBukaSemesterCopyDari.Items.Clear();
            var lst_formasiguru = DAO_FormasiGuruKelas.GetAll_Entity().Select(m => new { m.TahunAjaran, m.Semester }).Distinct().OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester);
            foreach (var item in lst_formasiguru)
            {
                decimal periode = Libs.GetStringToDecimal((item.TahunAjaran.Replace("/", "") + item.Semester));
                decimal periode_cur = Libs.GetStringToDecimal((Libs.GetTahunAjaranNow().Replace("/", "") + Libs.GetSemesterByTanggal(DateTime.Now).ToString()));

                if (periode_cur > periode)
                {
                    cboBukaSemesterCopyDari.Items.Add(
                            new ListItem
                            {
                                Text = item.TahunAjaran + " semester " + item.Semester.ToString(),
                                Value = "tc=" + item.TahunAjaran + "&sc=" + item.Semester.ToString(),
                                Selected =
                                    (
                                        item.Semester == Libs.GetSemesterByTanggal(DateTime.Now).ToString() &&
                                        item.TahunAjaran == Libs.GetTahunAjaranNowPlus(-1)
                                        ? true
                                        : false
                                    )
                            }
                        );
                }
            }
            Libs.ListUnitSekolahToDropDown(cboUnitBukaSemester);
        }

        protected void btnShowListKelasMapel_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.ShowPilihKelas.ToString();
        }

        protected void btnShowBukaSemester_Click(object sender, EventArgs e)
        {
            ListUnitBukaSemester();
            txtKeyAction.Value = JenisAction.ShowBukaSemester.ToString();
        }

        protected void lnkOKAbsensi_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtParseAbsensi.Value.Trim() != "")
                {
                    Guid kode = Guid.NewGuid();
                    List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(
                            txtTahunAjaran.Value, txtSemester.Value, txtRelKelasDet.Value
                        ).FindAll(m0 => m0.Rel_Mapel.Trim() == "");

                    string jenis_rapor = GetJenisRapor();
                    string s_kode_jenis_rapor = "";
                    if (jenis_rapor == "LTS")
                    {
                        lst_absen_rekap = lst_absen_rekap.FindAll(m0 => m0.Jenis.ToUpper().Trim() == TipeRapor.LTS);
                        s_kode_jenis_rapor = TipeRapor.LTS;
                    }
                    else if (jenis_rapor == "Semester")
                    {
                        lst_absen_rekap = lst_absen_rekap.FindAll(m0 => m0.Jenis.ToUpper().Trim() == TipeRapor.SEMESTER);
                        s_kode_jenis_rapor = TipeRapor.SEMESTER;
                    }

                    SiswaAbsenRekap m = new SiswaAbsenRekap();
                    if (lst_absen_rekap.Count > 0)
                    {
                        kode = lst_absen_rekap.FirstOrDefault().Kode;
                        m = lst_absen_rekap.FirstOrDefault();
                        DAO_SiswaAbsenRekap.Update(m);
                    }
                    else
                    {
                        m.Kode = kode;
                        m.TahunAjaran = txtTahunAjaran.Value;
                        m.Semester = txtSemester.Value;
                        m.Rel_Mapel = "";
                        m.Rel_KelasDet = txtRelKelasDet.Value;
                        m.Jenis = s_kode_jenis_rapor;
                        DAO_SiswaAbsenRekap.Insert(m);
                    }
                    DAO_SiswaAbsenRekapDet.DeleteByHeader(kode.ToString());
                    string[] arr_absensi = txtParseAbsensi.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item_absensi in arr_absensi)
                    {
                        string[] arr_item_absensi = item_absensi.Split(new string[] { "|" }, StringSplitOptions.None);
                        if (arr_item_absensi.Length == 5)
                        {
                            DAO_SiswaAbsenRekapDet.Insert(new SiswaAbsenRekapDet
                            {
                                Kode = Guid.NewGuid(),
                                Rel_SiswaAbsenRekap = kode.ToString(),
                                Rel_Siswa = arr_item_absensi[0],
                                JumlahPertemuan = "",
                                Sakit = arr_item_absensi[1],
                                Izin = arr_item_absensi[2],
                                Terlambat = arr_item_absensi[3],
                                Alpa = arr_item_absensi[4],
                                Hadir = ""
                            });
                        }
                    }

                    txtKeyAction.Value = JenisAction.DoSaveAbsensi.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected string GetJenisRapor()
        {
            string jenis_rapor = "";
            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(txtRelSekolah.Value);
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    switch ((Libs.UnitSekolah)m_sekolah.UrutanJenjang)
                    {
                        case Libs.UnitSekolah.KB:
                            List<AI_ERP.Application_Entities.Elearning.KB.Rapor_Arsip> lst_m_kb = AI_ERP.Application_DAOs.Elearning.KB.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                m0 => m0.TahunAjaran == txtTahunAjaran.Value &&
                                      m0.Semester == txtSemester.Value
                            ).OrderByDescending(m0 => m0.TanggalRapor).ToList();
                            if (lst_m_kb.Count > 0)
                            {
                                AI_ERP.Application_Entities.Elearning.KB.Rapor_Arsip m_kb = lst_m_kb.FirstOrDefault();
                                if (m_kb != null)
                                {
                                    if (m_kb.TahunAjaran != null)
                                    {
                                        jenis_rapor = m_kb.JenisRapor;
                                    }
                                }
                            }
                            break;
                        case Libs.UnitSekolah.TK:
                            List<AI_ERP.Application_Entities.Elearning.TK.Rapor_Arsip> lst_m_tk = AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                m0 => m0.TahunAjaran == txtTahunAjaran.Value &&
                                      m0.Semester == txtSemester.Value
                            ).OrderByDescending(m0 => m0.TanggalRapor).ToList();
                            if (lst_m_tk.Count > 0)
                            {
                                AI_ERP.Application_Entities.Elearning.TK.Rapor_Arsip m_tk = lst_m_tk.FirstOrDefault();
                                if (m_tk != null)
                                {
                                    if (m_tk.TahunAjaran != null)
                                    {
                                        jenis_rapor = m_tk.JenisRapor;
                                    }
                                }
                            }
                            break;
                        case Libs.UnitSekolah.SD:
                            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Arsip> lst_m_sd = AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                m0 => m0.TahunAjaran == txtTahunAjaran.Value &&
                                      m0.Semester == txtSemester.Value
                            ).OrderByDescending(m0 => m0.TanggalRapor).ToList();
                            if (lst_m_sd.Count > 0)
                            {
                                AI_ERP.Application_Entities.Elearning.SD.Rapor_Arsip m_sd = lst_m_sd.FirstOrDefault();
                                if (m_sd != null)
                                {
                                    if (m_sd.TahunAjaran != null)
                                    {
                                        jenis_rapor = m_sd.JenisRapor;
                                    }
                                }
                            }
                            break;
                        case Libs.UnitSekolah.SMP:
                            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Arsip> lst_m_smp = AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                m0 => m0.TahunAjaran == txtTahunAjaran.Value &&
                                      m0.Semester == txtSemester.Value
                            ).OrderByDescending(m0 => m0.TanggalRapor).ToList();
                            if (lst_m_smp.Count > 0)
                            {
                                AI_ERP.Application_Entities.Elearning.SMP.Rapor_Arsip m_smp = lst_m_smp.FirstOrDefault();
                                if (m_smp != null)
                                {
                                    if (m_smp.TahunAjaran != null)
                                    {
                                        jenis_rapor = m_smp.JenisRapor;
                                    }
                                }
                            }
                            break;
                        case Libs.UnitSekolah.SMA:
                            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Arsip> lst_m_sma = AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                m0 => m0.TahunAjaran == txtTahunAjaran.Value &&
                                      m0.Semester == txtSemester.Value
                            ).OrderByDescending(m0 => m0.TanggalRapor).ToList();
                            if (lst_m_sma.Count > 0)
                            {
                                AI_ERP.Application_Entities.Elearning.SMA.Rapor_Arsip m_sma = lst_m_sma.FirstOrDefault();
                                if (m_sma != null)
                                {
                                    if (m_sma.TahunAjaran != null)
                                    {
                                        jenis_rapor = m_sma.JenisRapor;
                                    }
                                }
                            }
                            break;
                        case Libs.UnitSekolah.SALAH:
                            jenis_rapor = "";
                            break;
                    }
                }
            }

            return jenis_rapor;
        }

        protected void ShowAbsensiSiswa()
        {
            string jenis_rapor = GetJenisRapor();
            
            List<Siswa> lst_siswa = DAO_Siswa.GetByRombel_Entity(
                    txtRelSekolah.Value, txtRelKelasDet.Value, txtTahunAjaran.Value, txtSemester.Value
                );
            List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(
                            txtTahunAjaran.Value, txtSemester.Value, txtRelKelasDet.Value
                        ).FindAll(m0 => m0.Rel_Mapel.Trim() == "");
            if (jenis_rapor == "LTS")
            {
                lst_absen_rekap = lst_absen_rekap.FindAll(m0 => m0.Jenis.ToUpper().Trim() == TipeRapor.LTS);
            }
            else if (jenis_rapor == "Semester")
            {
                lst_absen_rekap = lst_absen_rekap.FindAll(m0 => m0.Jenis.ToUpper().Trim() == TipeRapor.SEMESTER);
            }

            string kode = "";
            if (lst_absen_rekap.Count > 0)
            {
                kode = lst_absen_rekap.FirstOrDefault().Kode.ToString();
            }
            List<SiswaAbsenRekapDet> lst_siswa_absen_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(kode);

            string s_html = "<table style=\"margin: 0px; width: 100%;\">" +
                                "<thead style=\"background: white; position: sticky; top: 0;\">" +
                                    "<tr>" +
                                        "<td style=\"padding: 5px; font-weight: bold; text-align: center; vertical-align: middle;\">No.</td>" +
                                        "<td style=\"padding: 5px; font-weight: bold; text-align: center; vertical-align: middle;\">Nama Siswa</td>" +
                                        "<td style=\"padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; border-bottom-style: solid; border-bottom-color: green; background-color: #d9efd9; border-bottom-width: 4px;\">Sakit</td>" +
                                        "<td style=\"padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; border-bottom-style: solid; border-bottom-color: darkorange; background-color: #fce5ca; border-bottom-width: 4px;\">Izin</td>" +
                                        "<td style=\"padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; border-bottom-style: solid; border-bottom-color: #d800ff; background-color: #ffd4fe; border-bottom-width: 4px;\">Terlambat</td>" +
                                        "<td style=\"padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; padding: 5px; font-weight: bold; text-align: center; vertical-align: middle; border-bottom-style: solid; border-bottom-color: red; background-color: #ffdddd; border-bottom-width: 4px;\">Tanpa<br />Keterangan</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td colspan=\"6\" style=\"padding: 0px;\"><hr style=\"margin: 0px;\" /></td>" +
                                    "</tr>" +
                                "</thead>";
            int id = 1;
            s_html += "<tbody>";
            foreach (Siswa m_siswa in lst_siswa)
            {
                string s_sakit = "";
                string s_izin = "";
                string s_alpa = "";
                string s_terlambat = "";

                if (lst_siswa_absen_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).Count == 1)
                {
                    var m_det = lst_siswa_absen_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()).FirstOrDefault();
                    s_sakit = m_det.Sakit;
                    s_izin = m_det.Izin;
                    s_alpa = m_det.Alpa;
                    s_terlambat = m_det.Terlambat;
                }

                string s_txt_sakit = "<input value=\"" + s_sakit.Replace("\"", "") + "\" type=\"text\" name=\"arr_txt_sakit[]\" style=\"border-style: solid; border-width: 1px; border-color: #30a55f; background-color: #d9efd9; color: #453b3b; font-weight: bold; text-align: center; width: 100%;\"";
                string s_txt_izin = "<input value=\"" + s_izin.Replace("\"", "") + "\" type=\"text\" name=\"arr_txt_izin[]\" style=\"border-style: solid; border-width: 1px; border-color: #be7d05; background-color: #fce5ca; color: black; font-weight: bold; text-align: center; width: 100%;\"";
                string s_txt_alpa = "<input value=\"" + s_alpa.Replace("\"", "") + "\" type=\"text\" name=\"arr_txt_alpa[]\" style=\"border-style: solid; border-width: 1px; border-color: #d31e26; background-color: #ffdddd; color: black; font-weight: bold; text-align: center; width: 100%;\"";
                string s_txt_terlambat = "<input value=\"" + s_terlambat.Replace("\"", "") + "\" type=\"text\" name=\"arr_txt_terlambat[]\" style=\"border-style: solid; border-width: 1px; border-color: #d768eb; background-color: #fddfff; color: black; font-weight: bold; text-align: center; width: 100%;\"";


                s_html += "<tr style=\"" + (id % 2 != 0 ? "background-color: #f3f2f2;" : "background-color: white;") + "\">" +
                                "<td style=\"padding: 5px; vertical-align: middle; text-align: center;\">" + id.ToString() + "</td>" +
                                "<td style=\"padding: 5px; vertical-align: middle; font-weight: bold;\">" + m_siswa.Nama.ToUpper().Trim() + "</td>" +
                                "<td style=\"padding: 5px; vertical-align: middle;\">" + s_txt_sakit + "</td>" +
                                "<td style=\"padding: 5px; vertical-align: middle;\">" + s_txt_izin + "</td>" +
                                "<td style=\"padding: 5px; vertical-align: middle;\">" + s_txt_terlambat + "</td>" +
                                "<td style=\"padding: 5px; vertical-align: middle;\">" + s_txt_alpa + "</td>" +
                          "</tr>" +
                          "<tr>" +
                                 "<td colspan=\"6\" style=\"padding: 0px;\">" +
                                    "<hr style=\"margin: 0px;\" />" +
                                    "<input type=\"hidden\" name=\"arr_txt_siswa[]\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
                                 "</td>" +
                          "</tr>";
                id++;
            }
            s_html += "</tbody>";
            s_html += "</table>";

            ltrAbsensi.Text = s_html;
        }

        protected void btnSHowAbsensi_Click(object sender, EventArgs e)
        {
            ShowAbsensiSiswa();
            txtKeyAction.Value = JenisAction.ShowAbsensiWalas.ToString();
        }
    }
}