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
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA
{
    public partial class wf_NilaiCatatanSiswa : System.Web.UI.Page
    {
        public enum JenisAction
        {
            ShowIsiCatatan,
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
                        bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[i]);
                        break;
                    }
                }
                ltrBGHeader.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat;";
                ltrHeaderPilihan.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat; background-position-x: 5px;";
                ltrHeaderTab.Text = "background-color: " + arr_bg_tab[_id] + "; ";
            }
            else
            {
                _id = 1;
                bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + arr_bg_image[_id]);
                ltrHeaderPilihan.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat; background-position-x: 5px;";
                ltrBGHeader.Text = "background: url(" + bg_image + "); background-color: " + arr_bg[_id] + "; background-size: 60px 60px; background-repeat: no-repeat;";
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

            ltrCaptionPilihan.Text = s_html;

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
            this.Master.SetURLGuru_Penilaian(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE +
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
                                : ""
                            ) +
                            (
                                Libs.GetQueryString("m").Trim() != ""
                                ? "&m=" + Libs.GetQueryString("m")
                                : ""
                            )
                        )
                );

            m_kelas = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    s_html = "<label style=\"margin-left: 45px; color: white;\">" +
                                "Kelas" +
                                "&nbsp;" +
                                "<span style=\"font-weight: bold;\">" + m_kelas.Nama + "</span>" +
                                "&nbsp;" +
                                QS.GetTahunAjaran() +
                                "&nbsp;" +
                                "Semester " +
                                QS.GetSemester() +
                             "</label>" +
                             "<br />" +
                             "<label style=\"margin-left: 45px; color: white; font-weight: bold;\">" +
                                "Catatan Wali Kelas" +
                             "</label>";
                }
            }

            ltrCaption.Text = s_html;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-edit\" style=\"font-size: 16pt\"></i>" +
                                       "&nbsp;&nbsp;" +
                                       "Catatan Wali Kelas";

            this.Master.ShowSubHeaderGuru = true;
            this.Master.ShowHeaderSubTitle = false;
            this.Master.SelectMenuGuru_Penilaian();

            InitURLOnMenu();
            if (!IsPostBack)
            {
                ShowListSiswa();

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

                cboPeriode.Items.Clear();
                cboPeriode.Items.Add(new ListItem { Value = "1", Text = QS.GetTahunAjaran() + " Semester 1" });
                cboPeriode.Items.Add(new ListItem { Value = "2", Text = QS.GetTahunAjaran() + " Semester 2" });
                Libs.SelectDropdownListByValue(cboPeriode, QS.GetSemester());
            }

            this.Master.SetURLGuru_Penilaian("");
            this.Master.SetURLGuru_Siswa("");
            this.Master.SetURLGuru_TimeLine("");
        }

        protected void ShowListSiswa()
        {
            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(QS.GetKelas());
            string html_catatan = "";
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

                                        Rapor_CatatanSiswa m_catatan_siswa = DAO_Rapor_CatatanSiswa.GetAllByTABySMByKelasDetSiswa_Entity(
                                                QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), m_siswa.Kode.ToString()
                                            ).FirstOrDefault();

                                        string catatan = "";
                                        if (m_catatan_siswa != null)
                                        {
                                            if (m_catatan_siswa.TahunAjaran != null)
                                            {
                                                catatan = m_catatan_siswa.Catatan;
                                            }
                                        }

                                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                                        html_catatan += "<div class=\"row\">" +
                                                            "<div class=\"col-xs-12\">" +
                                                                "<table style=\"margin: 0px; width: 100%;\">" +
                                                                    "<tr>" +
                                                                        "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                                            id.ToString() +
                                                                            "." +
                                                                        "</td>" +
                                                                        "<td style=\"width: 50px; background-color: white; padding: 0px; vertical-align: middle;\">" +
                                                                            "<input name=\"txt_siswa[]\" type=\"hidden\" value=\"" + m_siswa.Kode.ToString() + "\" />" +
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
                                                                        "<td colspan=\"3\" style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px; padding-right: 10px; padding-left: 10px;\">" +
                                                                            "<div class=\"row\" style=\"margin-left: 0px; margin-right: 1px;\">" +
                                                                                "<div onclick=\"" + txtRelSiswa.ClientID + ".value = '" + m_siswa.Kode.ToString() + "'; " + btnShowIsiCatatan.ClientID + ".click();\" class=\"col-xs-12\" " +
                                                                                      "style=\"background-color: #f4f4f4; background-color: white; cursor: pointer; margin: 0px; padding: 0px; border-style: solid; border-color: #e8e8e8; border-width: 1px; border-radius: 5px; padding: 7px;" +
                                                                                              (id == lst_siswa.Count ? " margin-bottom: 12px; " : "") + "\">" +
                                                                                    "<div style=\"cursor: pointer; color: black; min-height: 100px; padding: 3px;\">" +
                                                                                        (
                                                                                            catatan.Trim() == ""
                                                                                            ? "<span style=\"color: #bfbfbf;\">Isi Catatan...</span>"
                                                                                            : catatan.Trim().
                                                                                              Replace("<p>", "").Replace("</p>", "").
                                                                                              Replace("<P>", "").Replace("</P>", "")
                                                                                        ) +
                                                                                    "</div>" +
                                                                                "</div>" +
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
            ltrSiswa.Text = html_catatan;
        }

        protected void btnShowIsiCatatan_Click(object sender, EventArgs e)
        {
            if (txtRelSiswa.Value.Trim() != "")
            {
                Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                        QS.GetTahunAjaran(),
                        QS.GetSemester(),
                        txtRelSiswa.Value
                    );
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {
                        string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                        ltrFotoSiswa.Text = "<img src=\"" + ResolveUrl(url_image) + "\" " +
                                                "style=\"margin-top: 10px; height: 50px; width: 50px; border-radius: 100%;\">";
                        lblNIS.Text = m_siswa.NISSekolah;
                        lblNamaSiswa.Text = Libs.GetPerbaikiEjaanNama(m_siswa.Nama);
                        txtCatatan.Text = "";
                        txtCatatanVal.Value = "";

                        List<Rapor_CatatanSiswa> lst_catatan = DAO_Rapor_CatatanSiswa.GetAllByTABySMByKelasDetSiswa_Entity(
                                QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), txtRelSiswa.Value
                            );
                        if (lst_catatan.Count > 0)
                        {
                            Rapor_CatatanSiswa m_catatansiswa = lst_catatan.FirstOrDefault();
                            if (m_catatansiswa != null)
                            {
                                if (m_catatansiswa.TahunAjaran != null)
                                {
                                    txtCatatan.Text = m_catatansiswa.Catatan;
                                    txtCatatanVal.Value = m_catatansiswa.Catatan;
                                }
                            }
                        }

                        txtKeyAction.Value = JenisAction.ShowIsiCatatan.ToString();
                    }
                }
            }
        }

        protected void lnkOKSaveCatatan_Click(object sender, EventArgs e)
        {
            List<Rapor_CatatanSiswa> lst_catatan = DAO_Rapor_CatatanSiswa.GetAllByTABySMByKelasDetSiswa_Entity(
                    QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), txtRelSiswa.Value
                );
            Rapor_CatatanSiswa m_catatansiswa = new Rapor_CatatanSiswa();
            if (lst_catatan.Count > 0)
            {
                m_catatansiswa = lst_catatan.FirstOrDefault();
                m_catatansiswa.Catatan = txtCatatanVal.Value;
                DAO_Rapor_CatatanSiswa.Update(m_catatansiswa, Libs.LOGGED_USER_M.UserID);
            }
            else
            {
                m_catatansiswa.Kode = Guid.NewGuid();
                m_catatansiswa.TahunAjaran = QS.GetTahunAjaran();
                m_catatansiswa.Semester = QS.GetSemester();
                m_catatansiswa.Rel_KelasDet = QS.GetKelas();
                m_catatansiswa.Rel_Siswa = txtRelSiswa.Value;
                m_catatansiswa.Catatan = txtCatatanVal.Value;
                DAO_Rapor_CatatanSiswa.Insert(m_catatansiswa, Libs.LOGGED_USER_M.UserID);
            }
            ShowListSiswa();
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void lnkOKPilihan_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    Libs.FILE_PAGE_URL +
                    "?t=" + Libs.GetQueryString("t") +
                    "&s=" + cboPeriode.SelectedValue +
                    "&kd=" + Libs.GetQueryString("kd")
                );
        }
    }
}