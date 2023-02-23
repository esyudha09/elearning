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
    public partial class wf_PilihEkstrakurikuler : System.Web.UI.Page
    {
        public enum JenisAction
        {
            ShowPilihanEkskul,
            AdaEkskul,
            DoUpdate
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

            ltrBGHeader.Text = ltrHeaderPilihan.Text;

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
            ltrCaptionMain.Text = s_html;

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
                ShowListMapelEkskul();
                ShowListSiswa();
                _UI.InitModalListNilai(
                    this.Page,
                    ltrListNilaiAkademik, ltrListNilaiEkskul, ltrListNilaiSikap, ltrListNilaiVolunteer, ltrListNilaiRapor,
                    QS.GetTahunAjaran(), QS.GetMapel(), QS.GetKelas(), QS.GetGuru()
                );
            }
        }

        protected void ShowListMapelEkskul()
        {
            List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(
                                DAO_Sekolah.GetAll_Entity().FindAll(m0 => m0.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault().Kode.ToString()
                            ).OrderBy(m0 => m0.Nama).ToList();

            ltrMapelEkskul.Text = "";
            string html = "";
            int id = 1;

            html += "<div onclick=\"DoPilihEkskul('', '');\" class=\"row\" style=\"cursor: pointer;\">" +
                        "<div class=\"col-xs-12\" style=\"cursor: pointer; padding-left: 0px;padding-right: 5px; border-style: solid; border-width: 1px; border-color: orange;\">" +
                            "<table style=\"margin: 0px; width: 99%;\">" +
                                "<tr>" +
                                    "<td style=\"cursor: pointer; width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                        "&nbsp" +
                                    "</td>" +
                                    "<td style=\"cursor: pointer; background-color: white; padding: 0px; font-size: small; padding-top: 7px; padding-bottom: 7px; font-weight: bold; color: darkorange;\">" +
                                        "<i class=\"fa fa-times\"></i>" +
                                        "&nbsp;&nbsp;Hapus / Kosongkan Ekstrakurikuler" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                        "</div>" +
                    "</div>";
            foreach (var mapel in lst_mapel)
            {
                if (DAO_Mapel.GetJenisMapel(mapel.Kode.ToString()) == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                {
                    html += "<div onclick=\"DoPilihEkskul('" + mapel.Kode.ToString() + "', '" + mapel.Nama.Replace("'", "`") + "');\" class=\"row" + (id % 2 != 0 ? " standardrow" : " oddrow") + "\" style=\"cursor: pointer;\">" +
                                "<div class=\"col-xs-12\" style=\"cursor: pointer; padding-left: 0px;padding-right: 5px;\">" +
                                    "<table style=\"margin: 0px; width: 99%;\">" +
                                        "<tr>" +
                                            "<td style=\"cursor: pointer; width: 30px; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                id.ToString() +
                                                "." +
                                            "</td>" +
                                            "<td style=\"vertical-align: middle; cursor: pointer; padding: 0px; font-size: small; padding-top: 7px; padding-bottom: 7px; font-weight: bold;\">" +
                                                "<i class=\"fa fa-file-o\"></i>" +
                                                "&nbsp;&nbsp;" +
                                                mapel.Nama +
                                                "<i name=\"arr_mapel[]\" id=\"" + mapel.Kode.ToString() + "\" class=\"fa fa-check-circle\" style=\"display: none; float: right; color: green;\"></i>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</div>" +
                            "</div>";
                    id++;
                }
            }

            ltrMapelEkskul.Text = html;
        }

        protected void ShowListSiswa()
        {
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    string rel_kelas_det = QS.GetKelas();
                    string rel_kelas = m_kelas_det.Rel_Kelas.ToString();
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
                                    else
                                    {
                                        lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                            m_kelas.Rel_Sekolah.ToString(),
                                            rel_kelas_det,
                                            QS.GetTahunAjaran(),
                                            QS.GetSemester()
                                        );
                                    }


                                    int id = 1;
                                    foreach (Siswa m_siswa in lst_siswa)
                                    {
                                        string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                              "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

                                        Rapor_PilihEkstrakurikuler m_pilih_ekskul = DAO_Rapor_PilihEkstrakurikuler.GetByTABySMByKelasDetBySiswa_Entity(
                                                QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString()
                                            ).FirstOrDefault();

                                        string kode_pilih_ekskul = "";
                                        string kode_ekskul_1 = "";
                                        string kode_ekskul_2 = "";
                                        string kode_ekskul_3 = "";
                                        string kode_ekskul_4 = "";
                                        string nama_ekskul_1 = "";
                                        string nama_ekskul_2 = "";
                                        string nama_ekskul_3 = "";
                                        string nama_ekskul_4 = "";
                                        if (m_pilih_ekskul != null)
                                        {
                                            if (m_pilih_ekskul.TahunAjaran != null)
                                            {
                                                kode_pilih_ekskul = m_pilih_ekskul.Kode.ToString();
                                                kode_ekskul_1 = m_pilih_ekskul.Rel_Mapel1;
                                                kode_ekskul_2 = m_pilih_ekskul.Rel_Mapel2;
                                                kode_ekskul_3 = m_pilih_ekskul.Rel_Mapel3;
                                                kode_ekskul_4 = m_pilih_ekskul.Rel_Mapel4;
                                            }
                                        }
                                        if (kode_pilih_ekskul.Trim() == "") kode_pilih_ekskul = Guid.NewGuid().ToString();
                                        if (kode_ekskul_1.Trim() != "") nama_ekskul_1 = DAO_Mapel.GetByID_Entity(kode_ekskul_1).Nama;
                                        if (kode_ekskul_2.Trim() != "") nama_ekskul_2 = DAO_Mapel.GetByID_Entity(kode_ekskul_2).Nama;
                                        if (kode_ekskul_3.Trim() != "") nama_ekskul_3 = DAO_Mapel.GetByID_Entity(kode_ekskul_3).Nama;
                                        if (kode_ekskul_3.Trim() != "") nama_ekskul_3 = DAO_Mapel.GetByID_Entity(kode_ekskul_3).Nama;
                                        if (kode_ekskul_4.Trim() != "") nama_ekskul_4 = DAO_Mapel.GetByID_Entity(kode_ekskul_4).Nama;

                                        string html_cbo_mapel_1 = "";
                                        string html_cbo_mapel_2 = "";
                                        string html_cbo_mapel_3 = "";
                                        string html_cbo_mapel_4 = "";

                                        string html_item_cbo_mapel_1 = "";
                                        string html_item_cbo_mapel_2 = "";
                                        string html_item_cbo_mapel_3 = "";
                                        string html_item_cbo_mapel_4 = "";

                                        //List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(
                                        //        AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD)
                                        //    ).FindAll(
                                        //        m0 => m0.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                                        //              m0.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER
                                        //    );
                                        List<Mapel> lst_mapel = DAO_Mapel.GetAllByStrukturNilai_Entity(
                                                QS.GetTahunAjaran(),
                                                QS.GetSemester(),
                                                m_kelas.Kode.ToString()
                                            ).FindAll(
                                                m0 => m0.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                                                      m0.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER
                                            );

                                        foreach (Mapel m_mapel in lst_mapel)
                                        {
                                            html_item_cbo_mapel_1 += "<option " + (
                                                                            m_mapel.Kode.ToString().Trim().ToUpper() == kode_ekskul_1.Trim().ToUpper()
                                                                            ? " selected "
                                                                            : ""
                                                                        ) + 
                                                                     " value=\"" + m_mapel.Kode.ToString() + "\">" +
                                                                        m_mapel.Nama +
                                                                     "</option>";
                                                                        
                                        }

                                        foreach (Mapel m_mapel in lst_mapel)
                                        {
                                            html_item_cbo_mapel_2 += "<option " + (
                                                                            m_mapel.Kode.ToString().Trim().ToUpper() == kode_ekskul_2.Trim().ToUpper()
                                                                            ? " selected "
                                                                            : ""
                                                                        ) +
                                                                     " value=\"" + m_mapel.Kode.ToString() + "\">" +
                                                                        m_mapel.Nama +
                                                                     "</option>";

                                        }

                                        foreach (Mapel m_mapel in lst_mapel)
                                        {
                                            html_item_cbo_mapel_3 += "<option " + (
                                                                            m_mapel.Kode.ToString().Trim().ToUpper() == kode_ekskul_3.Trim().ToUpper()
                                                                            ? " selected "
                                                                            : ""
                                                                        ) +
                                                                     " value=\"" + m_mapel.Kode.ToString() + "\">" +
                                                                        m_mapel.Nama +
                                                                     "</option>";

                                        }

                                        foreach (Mapel m_mapel in lst_mapel)
                                        {
                                            html_item_cbo_mapel_4 += "<option " + (
                                                                            m_mapel.Kode.ToString().Trim().ToUpper() == kode_ekskul_4.Trim().ToUpper()
                                                                            ? " selected "
                                                                            : ""
                                                                        ) +
                                                                     " value=\"" + m_mapel.Kode.ToString() + "\">" +
                                                                        m_mapel.Nama +
                                                                     "</option>";

                                        }

                                        string kode_siswa = m_siswa.Kode.ToString().Replace("-", "_");

                                        html_cbo_mapel_1 = "<select onchange=\"SetPilihMatpelEkskul(this.lang, '" + m_siswa.Kode.ToString() + "', this.id);\" " +
                                                                    "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                                                    "onfocus=\"SetNilaiAwalMapelEkskul(this.value);\" " +
                                                                    "name=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "lang=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "id=\"cbo_mata_ekskul_" + kode_siswa + "_1\" " +
                                                                    "title=\" Mata Ekskul \" " +
                                                                    "class=\"text-input\" " +
                                                                    "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                                                                "<option></option>" +
                                                                html_item_cbo_mapel_1 +
                                                           "</select>";

                                        html_cbo_mapel_2 = "<select onchange=\"SetPilihMatpelEkskul(this.lang, '" + m_siswa.Kode.ToString() + "', this.id);\" " +
                                                                    "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                                                    "onfocus=\"SetNilaiAwalMapelEkskul(this.value);\" " +
                                                                    "name=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "lang=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "id=\"cbo_mata_ekskul_" + kode_siswa + "_2\" " +
                                                                    "title=\" Mata Ekskul \" " +
                                                                    "class=\"text-input\" " +
                                                                    "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                                                                "<option></option>" +
                                                                html_item_cbo_mapel_2 +
                                                           "</select>";

                                        html_cbo_mapel_3 = "<select onchange=\"SetPilihMatpelEkskul(this.lang, '" + m_siswa.Kode.ToString() + "', this.id);\" " +
                                                                    "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                                                    "onfocus=\"SetNilaiAwalMapelEkskul(this.value);\" " +
                                                                    "name=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "lang=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "id=\"cbo_mata_ekskul_" + kode_siswa + "_3\" " +
                                                                    "title=\" Mata Ekskul \" " +
                                                                    "class=\"text-input\" " +
                                                                    "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                                                                "<option></option>" +
                                                                html_item_cbo_mapel_3 +
                                                           "</select>";

                                        html_cbo_mapel_4 = "<select onchange=\"SetPilihMatpelEkskul(this.lang, '" + m_siswa.Kode.ToString() + "', this.id);\" " +
                                                                    "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                                                    "onfocus=\"SetNilaiAwalMapelEkskul(this.value);\" " +
                                                                    "name=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "lang=\"cbo_mata_ekskul_" + kode_siswa + "[]\" " +
                                                                    "id=\"cbo_mata_ekskul_" + kode_siswa + "_4\" " +
                                                                    "title=\" Mata Ekskul \" " +
                                                                    "class=\"text-input\" " +
                                                                    "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                                                                "<option></option>" +
                                                                html_item_cbo_mapel_4 +
                                                           "</select>";

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        ltrSiswa.Text += "<div class=\"row\">" +
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
                                                                        "<td colspan=\"3\" " +
                                                                            "style=\"" +
                                                                                        "background-color: white; padding: 0px; font-size: small; padding-top: 7px; padding-right: 10px; padding-left: 10px; " +
                                                                                        (
                                                                                            id == lst_siswa.Count
                                                                                            ? " padding-bottom: 15px; "
                                                                                            : ""
                                                                                        ) +
                                                                                   "\">" +
                                                                            "<div class=\"row\" style=\"margin-left: 0px; margin-right: 1px;\">" +
                                                                                html_cbo_mapel_1 +
                                                                            "</div>" +
                                                                            "<div class=\"row\" style=\"margin-left: 0px; margin-right: 1px; margin-top: 5px;\">" +
                                                                                html_cbo_mapel_2 +
                                                                            "</div>" +
                                                                            "<div class=\"row\" style=\"margin-left: 0px; margin-right: 1px; margin-top: 5px;\">" +
                                                                                html_cbo_mapel_3 +
                                                                            "</div>" +
                                                                            "<div class=\"row\" style=\"margin-left: 0px; margin-right: 1px; margin-top: 5px;\">" +
                                                                                html_cbo_mapel_4 +
                                                                            "</div>" +
                                                                        "</td>" +
                                                                    "</tr>" +
                                                                "</table>" +
                                                            "</div>" +
                                                        "</div>";
                                        id++;

                                    }

                                }
                            }

                        }
                    }

                }
            }            
        }

        protected void btnOKSavePilihEkskul_Click(object sender, EventArgs e)
        {
            try
            {
                bool ada_pilih_ekskul = false;
                bool valid_input = true;
                Rapor_PilihEkstrakurikuler m_ekskul = new Rapor_PilihEkstrakurikuler();
                if (DAO_Rapor_PilihEkstrakurikuler.GetByTABySMByKelasDetBySiswa_Entity(
                        QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), txtRelSiswa.Value
                    ).Count > 0
                )
                {
                    m_ekskul = DAO_Rapor_PilihEkstrakurikuler.GetByTABySMByKelasDetBySiswa_Entity(
                            QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), txtRelSiswa.Value
                        ).FirstOrDefault();
                    ada_pilih_ekskul = true;
                }
                if (m_ekskul != null)
                {
                    if (m_ekskul.TahunAjaran != null)
                    {
                        ada_pilih_ekskul = true;
                    }
                }

                if (ada_pilih_ekskul)
                {
                    m_ekskul.Rel_Mapel1 = txtPilihEkskul1.Value;
                    m_ekskul.Rel_Mapel2 = txtPilihEkskul2.Value;
                    m_ekskul.Rel_Mapel3 = txtPilihEkskul3.Value;
                    m_ekskul.Rel_Mapel4 = txtPilihEkskul4.Value;
                    m_ekskul.Rel_Mapel5 = "";

                    if (
                        (
                            txtPilihEkskul1.Value == txtPilihEkskul2.Value &&
                            txtPilihEkskul1.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul1.Value == txtPilihEkskul3.Value &&
                            txtPilihEkskul1.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul1.Value == txtPilihEkskul4.Value &&
                            txtPilihEkskul1.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul2.Value == txtPilihEkskul3.Value &&
                            txtPilihEkskul2.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul2.Value == txtPilihEkskul4.Value &&
                            txtPilihEkskul2.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul3.Value == txtPilihEkskul4.Value &&
                            txtPilihEkskul3.Value.Trim() != ""
                        )
                    )
                    {
                        valid_input = false;
                    }

                    if (valid_input)
                    {
                        DAO_Rapor_PilihEkstrakurikuler.Update(m_ekskul);
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                    else
                    {
                        txtKeyAction.Value = JenisAction.AdaEkskul.ToString();
                    }
                }
                else
                {
                    m_ekskul = new Rapor_PilihEkstrakurikuler();
                    m_ekskul.Kode = Guid.NewGuid();
                    m_ekskul.TahunAjaran = QS.GetTahunAjaran();
                    m_ekskul.Semester = QS.GetSemester();
                    m_ekskul.Rel_Siswa = txtRelSiswa.Value;
                    m_ekskul.Rel_KelasDet = QS.GetKelas();
                    m_ekskul.Rel_Mapel1 = txtPilihEkskul1.Value;
                    m_ekskul.Rel_Mapel2 = txtPilihEkskul2.Value;
                    m_ekskul.Rel_Mapel3 = txtPilihEkskul3.Value;
                    m_ekskul.Rel_Mapel4 = txtPilihEkskul4.Value;
                    m_ekskul.Rel_Mapel5 = "";

                    if (
                        (
                            txtPilihEkskul1.Value == txtPilihEkskul2.Value &&
                            txtPilihEkskul1.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul1.Value == txtPilihEkskul3.Value &&
                            txtPilihEkskul1.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul1.Value == txtPilihEkskul4.Value &&
                            txtPilihEkskul1.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul2.Value == txtPilihEkskul3.Value &&
                            txtPilihEkskul2.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul2.Value == txtPilihEkskul4.Value &&
                            txtPilihEkskul2.Value.Trim() != ""
                        ) ||
                        (
                            txtPilihEkskul3.Value == txtPilihEkskul4.Value &&
                            txtPilihEkskul3.Value.Trim() != ""
                        )
                    )
                    {
                        valid_input = false;
                    }

                    if (valid_input)
                    {
                        DAO_Rapor_PilihEkstrakurikuler.Insert(m_ekskul);
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                    else
                    {
                        txtKeyAction.Value = JenisAction.AdaEkskul.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }            
        }
    }
}