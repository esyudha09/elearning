using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Masters
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            InitMenu();
        }

        protected void InitMenu()
        {
            int id_kelas = 0;
            int id = 1;
            string[] arr_bg =
                { "#FD6933", "#0AC6AE", "#F7921E", "#4AA4A4", "#43B8C9", "#95D1C5", "#019ADD", "#31384B", "#18AEC7", "#5299CF", "#2D2C28", "#D5C5C6", "#262726", "#01ACAC", "#322D3A", "#3B4F5D", "#009E00", "#E90080", "#549092", "#00A9A9", "#9B993A" };
            string[] arr_bg_image =
                { "a.png", "b.png", "c.png", "d.png", "e.png", "f.png", "g.png", "h.png", "i.png", "j.png", "k.png", "l.png", "m.png", "n.png", "o.png", "p.png", "q.png", "r.png", "u.png", "s.png", "t.png" };
            string s_html = "";
            string bg_image = "";

            ltrMenuKelasSaya.Text = "";

            List<FormasiGuruKelas_ByGuru> lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow()).ToList();
            List<MapelEkskulAjar> lst_mapel_ajar = new List<MapelEkskulAjar>();
            lst_mapel_ajar.Clear();

            if (Libs.GetQueryString("act").Trim() == "")
            {
                lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, Libs.GetTahunAjaranNow()).ToList().FindAll(
                        m => m.TahunAjaran == Libs.GetTahunAjaranNow()
                    );
            }
            else
            {
                lst_kelasguru = DAO_FormasiGuruKelas.GetByGuruByTA_Entity(Libs.LOGGED_USER_M.NoInduk, "2018/2019").ToList();
            }

            if (lst_kelasguru.Count <= 100)
            {
                //untuk kelas perwalian
                List<string> lst_formasi_guru_kelas = new List<string>();
                lst_formasi_guru_kelas.Clear();
                foreach (FormasiGuruKelas_ByGuru m in lst_kelasguru.FindAll(m => m.Mapel.Trim() == ""))
                {
                    string bg_warna = arr_bg[(id % arr_bg.Length)];
                    string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                    bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);

                    List<Siswa> lst_siswa = DAO_Siswa.GetByRombel_Entity(
                            m.Rel_Sekolah.ToString(), m.Rel_KelasDet, m.TahunAjaran, m.Semester
                        );
                    Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m.Rel_Sekolah.ToString());
                    if (m_sekolah != null && lst_formasi_guru_kelas.FindAll(m0 => m0 == m.TahunAjaran + m.Rel_KelasDet.ToString()).Count == 0)
                    {
                        s_html += "<li style=\"background: url(" + bg_image + "); background-color: " + bg_warna + "; background-position: top right; background-size: 60px 60px; background-repeat: no-repeat; margin-left: -20px;\">" +
                                    "<a class=\"waves-attach\" " +
                                        "href=\"" +
                                                ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE) +
                                                "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                "&ft=" + Libs.Encryptdata(file_img) +
                                                (
                                                    m.KodeMapel.Trim() != ""
                                                    ? "&m=" + m.KodeMapel
                                                    : ""
                                                ) +
                                                "&kd=" + m.Rel_KelasDet +
                                             "\" " +
                                        "style=\"font-weight: bold; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;\"> " +
                                        m.KelasDet +
                                        "&nbsp; " +
                                        (
                                            m.Mapel.Trim() != ""
                                            ? "<br />" +
                                              "<span style=\"font-size: medium; color: white; font-size: x-small; font-weight: normal;\">" + m.Mapel + "</span>"
                                            : "<br />" +
                                              "<span class=\"badge\" style=\"font-size: medium; color: white; font-size: x-small; font-weight: bold; margin-top: 5px; margin-left: -2px;\">" +
                                                "Guru Kelas atau Wali Kelas" +
                                              "</span>"
                                        ) +
                                        "&nbsp; " +
                                        "<sup style=\"display: none; top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                            "<span style=\"font-weight: normal;\">" + lst_siswa.Count + "</span>" +
                                        "</sup>" +
                                    "</a>" +
                                  "</li>";
                        id++;
                        id_kelas++;
                    }

                    lst_formasi_guru_kelas.Add(m.TahunAjaran + m.Rel_KelasDet.ToString());
                }

                List<string> lst_formasi_guru_mapel = new List<string>();
                lst_formasi_guru_mapel.Clear();
                //untuk kelas bidang studi
                foreach (FormasiGuruKelas_ByGuru m in lst_kelasguru.FindAll(m => m.Mapel.Trim() != ""))
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.KodeMapel);
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

                            string bg_warna = arr_bg[(id % arr_bg.Length)];
                            string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                            bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);

                            List<Siswa> lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                    m.Rel_Sekolah.ToString(), m.Rel_KelasDet, m.TahunAjaran, m.Semester
                                );

                            //mapel non ekskul
                            if (
                                (
                                    DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) != Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                    (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.EKSTRAKURIKULER && m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                                )
                            )
                            {

                                s_html += "<li style=\"background: url(" + bg_image + "); background-color: " + bg_warna + "; background-position: top right; background-size: 60px 60px; background-repeat: no-repeat; margin-left: -20px;\">" +
                                            "<a class=\"waves-attach\" " +
                                                "href=\"" +
                                                        ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE) +
                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) +
                                                        "&ft=" + Libs.Encryptdata(file_img) +
                                                        (
                                                            m.KodeMapel.Trim() != ""
                                                            ? "&m=" + m.KodeMapel
                                                            : ""
                                                        ) +
                                                        "&kd=" + m.Rel_KelasDet +
                                                     "\" " +
                                                "style=\"font-weight: bold; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;\"> " +
                                                m.KelasDet +
                                                "&nbsp; " +
                                                (
                                                    m.Mapel.Trim() != ""
                                                    ? "<br />" +
                                                      "<span style=\"font-size: medium; color: white; font-size: x-small; font-weight: normal;\">" + m.Mapel + "</span>"
                                                    : "<br />" +
                                                      "<span class=\"badge\" style=\"font-size: medium; color: white; font-size: x-small; font-weight: bold; margin-top: 5px; margin-left: -2px;\">" +
                                                        "Guru Kelas atau Wali Kelas" +
                                                      "</span>"
                                                ) +
                                                "&nbsp; " +
                                                "<sup style=\"display: none; top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                    "<span style=\"font-weight: normal;\">" + lst_siswa.Count + "</span>" +
                                                "</sup>" +
                                            "</a>" +
                                          "</li>";
                                id++;
                                id_kelas++;
                            }
                        }
                        //mapel ekskul
                        else
                        {
                            if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                            {
                                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m.Rel_KelasDet);
                                Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());

                                if (m_kelas_det != null)
                                {
                                    if (m_kelas_det.Nama != null)
                                    {
                                        if (lst_mapel_ajar.FindAll(
                                                m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                      m0.Semester == m.Semester &&
                                                      m0.Rel_Mapel == m.KodeMapel &&
                                                      m0.Rel_Kelas == m_kelas_det.Rel_Kelas.ToString()
                                           ).Count == 0)
                                        {
                                            lst_mapel_ajar.Add(new MapelEkskulAjar
                                            {
                                                TahunAjaran = m.TahunAjaran,
                                                Semester = m.Semester,
                                                Rel_Mapel = m.KodeMapel,
                                                Rel_Kelas = m_kelas_det.Rel_Kelas.ToString(),
                                                UrutanLevel = m_kelas.UrutanLevel
                                            });
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
            //cek & list mapel ekskul
            //ekskul SD
            foreach (var mapel_ekskul in lst_mapel_ajar.Select(m => new { m.TahunAjaran, m.Semester, m.Rel_Mapel }).Distinct().ToList())
            {
                var lst_ajar_ekskul = lst_mapel_ajar.FindAll(
                        m => m.TahunAjaran == mapel_ekskul.TahunAjaran &&
                             m.Semester == mapel_ekskul.Semester &&
                             m.Rel_Mapel == mapel_ekskul.Rel_Mapel
                    ).OrderBy(m => m.UrutanLevel).ToList();

                jml_anak = 0;
                string kelas_ekskul = "";
                string kelas_ekskul_url = "";
                string s_panggilan = "";
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(mapel_ekskul.Rel_Mapel.ToString());

                foreach (var item_ajar_ekskul in lst_ajar_ekskul)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(item_ajar_ekskul.Rel_Kelas.ToString());

                    if (m_kelas != null && m_mapel != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            kelas_ekskul += (kelas_ekskul.Trim() != "" && m_kelas.Nama.Trim() != "" ? ", " : "") +
                                            m_kelas.Nama;
                            kelas_ekskul_url += m_kelas.Kode.ToString() + ";";

                            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_PilihEkstrakurikuler> lst_ekskul_sd =
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_PilihEkstrakurikuler.GetByTABySMByMapelByKelas_Entity(
                                            mapel_ekskul.TahunAjaran, mapel_ekskul.Semester, mapel_ekskul.Rel_Mapel, item_ajar_ekskul.Rel_Kelas.ToString()
                                        );

                            int id_siswa = 0;
                            foreach (var ekskul_sd in lst_ekskul_sd)
                            {
                                Siswa m_siswa = DAO_Siswa.GetByKode_Entity(ekskul_sd.TahunAjaran, ekskul_sd.Semester, ekskul_sd.Rel_Siswa.ToString());
                                if (m_siswa != null)
                                {
                                    if (m_siswa.Nama != null)
                                    {
                                        if (m_siswa.Panggilan.Trim() != "")
                                        {
                                            s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                  "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";
                                            id_siswa++;
                                            jml_anak++;
                                        }
                                        else
                                        {
                                            s_panggilan += "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                                                  "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id_siswa % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + Libs.GetNamaPanggilan(m_siswa.Nama).ToLower() + "</label>";
                                            id_siswa++;
                                            jml_anak++;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                if (s_panggilan.Trim() != "")
                {
                    s_panggilan = "<div style=\"width: 100%; text-align: center;\">" +
                                    s_panggilan +
                                  "</div>";
                }

                string bg_warna = arr_bg[(id % arr_bg_image.Length)];
                string file_img = arr_bg_image[(id % arr_bg_image.Length)];
                bg_image = ResolveUrl("~/Application_CLibs/images/kelas/" + file_img);
                string bg_image_card = ResolveUrl("~/Application_CLibs/images/kelas/bg.png");

                s_html += "<li style=\"background: url(" + bg_image + "); background-color: " + bg_warna + "; background-position: top right; background-size: 60px 60px; background-repeat: no-repeat; margin-left: -20px;\">" +
                                "<a class=\"waves-attach\" " +
                                    "href=\"" +
                                            ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.ROUTE) +
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
                                    "style=\"font-weight: bold; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;\"> " +
                                    kelas_ekskul +
                                    "&nbsp; " +
                                    (
                                        m_mapel.Nama.Trim() != ""
                                        ? "<br />" +
                                            "<span style=\"font-weight: normal; color: white;\">" +
                                                Libs.GetPerbaikiEjaanNama(DAO_Mapel.GetJenisMapel(mapel_ekskul.Rel_Mapel)) +
                                                "&nbsp;:&nbsp;" +
                                            "<span>" +
                                            "<span style=\"font-weight: bold; color: white;\">" +
                                                m_mapel.Nama +
                                            "<span>"
                                        : "<br />" +
                                            "<span class=\"badge\" style=\"font-size: medium; color: white; font-size: x-small; font-weight: bold; margin-top: 5px; margin-left: -2px;\">" +
                                                "Guru Kelas atau Wali Kelas" +
                                            "</span>"
                                    ) +
                                    "&nbsp; " +
                                    "<sup style=\"display: none; top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                        "<span style=\"font-weight: normal;\">" + jml_anak.ToString() + "</span>" +
                                    "</sup>" +
                                "</a>" +
                            "</li>";
                id++;
                id_kelas++;
            }
            //end ekskul sd
            //end cek & list mapel ekskul

            if (s_html.Trim() != "")
            {
                s_html = "<li>" +
                            "<a class=\"collapsed waves-attach\" data-toggle=\"collapse\" " +
								"href=\"#ui_menu_kelas_saya\"  " +
								"style=\"font-weight: bold; color: #808080;\">  " +
								"<i class=\"fa fa-university\" aria-hidden=\"true\" style=\"margin-right: 10px;\"></i> " +
								"&nbsp;" +
								"Kelas Saya" +
                                "&nbsp; " +
                                "<sup style=\"top: -10px; background-color: black; font-size: x-small; color: white; padding: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                    "<span style=\"font-weight: normal;\">" + id_kelas.ToString() + " kelas</span>" +
                                "</sup>" +
                                "<label class=\"fa fa-ellipsis-v pull-right\" style=\"margin-top: 25px;\"></label>  " +
                            "</a>  " +
                            "<ul class=\"menu-collapse collapse\" id=\"ui_menu_kelas_saya\">" +
                                s_html +
                            "</ul>" +
                         "</li>";
            }
            ltrMenuKelasSaya.Text = s_html;
        }

        public string HeaderTittle
        {
            get { return lblHeadingTitle.Text; }
            set
            {
                lblHeadingTitle.Text = value;
                lblHeaderAffix.Text = value;
            }
        }

        public bool HeaderCardVisible
        {
            get { return HeaderCard.Visible; }
            set { HeaderCard.Visible = value; }
        }

        public bool ShowHeaderTools
        {
            get { return div_Header.Visible; }
            set
            {
                div_Header.Visible = value;
                div_title.Visible = !value;
            }
        }

        public bool ShowLogoAtas
        {
            get { return imgAtas.Visible; }
            set
            {
                imgAtas.Visible = value;
            }
        }

        public bool ShowSubHeaderGuru
        {
            get { return sub_header_guru.Visible; }
            set
            {
                sub_header_guru.Visible = value;
            }
        }

        private const string CSS_SEL_MENU = "padding-left: 10px; padding-right: 10px; border-radius: 4px; background-color: rgb(178, 0, 0); background-color: #0F9D58; border-style: none; padding-bottom: 5px; padding-top: 5px; color: white;";

        public void SelectMenuGuru_TimeLine()
        {
            a_timeline.Attributes["style"] = CSS_SEL_MENU;
            a_siswa.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_penilaian.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";

            a_jurnal_guru.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_kunjunganperpus.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_informasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_evaluasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_administrasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
        }

        public void SelectMenuGuru_Siswa()
        {
            a_siswa.Attributes["style"] = CSS_SEL_MENU;
            a_timeline.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_penilaian.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";

            a_jurnal_guru.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_kunjunganperpus.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_informasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_evaluasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_administrasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
        }

        public void SelectMenuGuru_Penilaian()
        {
            a_penilaian.Attributes["style"] = CSS_SEL_MENU;
            a_timeline.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_siswa.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";

            a_jurnal_guru.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_kunjunganperpus.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_informasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_evaluasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
            a_administrasi.Attributes["style"] = "padding-left: 10px; padding-right: 10px; border-radius: 5px; color: #787878;";
        }

        public void SetURLGuru_TimeLine(string url)
        {
            if (url.Trim() == "")
            {
                a_timeline.Attributes["style"] = "display: none;";
            }
            else
            {
                a_timeline.Attributes["href"] = url;
            }
        }

        public void SetURLGuru_Siswa(string url)
        {
            if (url.Trim() == "")
            {
                a_siswa.Attributes["style"] = "display: none;";
            }
            else
            {
                a_siswa.Attributes["href"] = url;
            }
        }

        public void SetURLGuru_Penilaian(string url)
        {
            if (url.Trim() == "")
            {
                a_penilaian.Attributes["style"] = "display: none;";
            }
            else if (url.Trim() == "-")
            {
                a_penilaian.Attributes["href"] = "";
            }
            else
            {
                a_penilaian.Attributes["href"] = url;
            }
        }

        public TextBox txtCariData
        {
            get { return txtCari; }
            set { txtCari = value; }
        }

        public ScriptManager ScriptManager
        {
            get { return ScriptManager1; }
        }

        public bool ShowHeaderSubTitle
        {
            get {
                return div_header_title.Visible;
            }
            set {
                div_header_title.Visible = value;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {

        }
    }
}