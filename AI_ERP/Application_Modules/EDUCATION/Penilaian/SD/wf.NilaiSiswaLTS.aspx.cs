using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_NilaiSiswaLTS : System.Web.UI.Page
    {
        public string s_url_1 = "";
        public string s_url_2 = "";
        public string s_url_ok = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.ROUTE;
        public enum JenisAction
        {
            ShowPengaturanLTS,
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
            this.Master.ShowHeaderSubTitle = false;
            this.Master.ShowSubHeaderGuru = true;
            this.Master.SelectMenuGuru_Penilaian();

            _UI.InitModalListNilai(
                this.Page,
                ltrListNilaiAkademik,
                ltrListNilaiEkskul,
                ltrListNilaiSikap,
                ltrListNilaiVolunteer,
                ltrListNilaiRapor,
                QS.GetTahunAjaran(),
                QS.GetMapel(),
                QS.GetKelas(),
                QS.GetGuru()
            );

            if (!IsPostBack)
            {
                ShowListSiswa();
            }

            ShowDataSiswa();
            InitURLOnMenu();

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

            lnkOKCetakBahasaKD.Visible = false;
            lnkOKCetakSatuKelas.Visible = false;
            lnkOKCetakAktif.Visible = false;
            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
            {
                lnkOKCetakBahasaKD.Visible = true;
                lnkOKCetakSatuKelas.Visible = true;
                lnkOKCetakAktif.Visible = true;
            }
        }

        protected void btnShowNilaiSiswa_Click(object sender, EventArgs e)
        {
            ShowDataSiswa();
        }

        protected void ShowListSiswa()
        {
            ltrListSiswa.Text = "";            
            string s_siswa_url = "";
            int id = 1;
            foreach (Siswa m_siswa in GetListSiswa())
            {
                s_siswa_url += (s_siswa_url.Trim() != "" ? ";" : "") +
                               m_siswa.Kode.ToString();

                string url_image = Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + m_siswa.NIS + ".jpg");
                ltrListSiswa.Text += "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"width: 100%;\">" +
                                            "<table style=\"margin: 0px; width: 100%;\">" +
                                                "<tr>" +
                                                    "<td style=\"width: 30px; background-color: white; padding: 0px; vertical-align: middle; text-align: center; color: #bfbfbf;\">" +
                                                        id.ToString() +
                                                        "." +
                                                    "</td>" +
                                                    "<td style=\"background-color: white; padding: 0px; font-size: small; padding-top: 7px;\">" +
                                                        "<span style=\"color: grey; font-weight: bold;\">" +
                                                            Libs.GetPerbaikiEjaanNama(Libs.GetPersingkatNama(m_siswa.Nama.Trim().ToUpper(), 4)) +
                                                            (
                                                                m_siswa.Panggilan.Trim() != ""
                                                                ? "<br />" +
                                                                  "<span style=\"color: #bfbfbf; font-weight: normal\">" +
                                                                    Libs.GetNamaPanggilan(m_siswa.Panggilan) +
                                                                  "</span>"
                                                                : ""
                                                            ) +
                                                        "</span>" +
                                                    "</td>" +
                                                    "<td style=\"width: 50px; text-align: right; vertical-align: middle; padding-right: 0px;\">" +
                                                        "<label id=\"img_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                               "style=\"display: none; font-size: small; color: grey; font-weight: bold;\">" +

                                                            "<img src=\"" + ResolveUrl("~/Application_CLibs/images/giphy.gif") + "\" " +
                                                                    "style=\"height: 16px; width: 20px;\" />" +

                                                        "</label>" +
                                                        "<a id=\"lbl_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                                            "onclick=\"ShowProsesPilihSiswa('" + m_siswa.Kode.ToString() + "', true); " + txtIndexSiswa.ClientID + ".value = '" + (id - 1) + "'; " + btnShowNilaiSiswa.ClientID + ".click(); \"" +
                                                            "style=\"font-weight: bold; text-transform: none; padding-bottom: 2px; padding-top: 2px; background-color: #1DA1F2; color: white; border-radius: 15px; font-size: x-small;\" " +
                                                            "class=\"btn btn-flat waves-attach waves-effect\" " +
                                                            "title=\" Buka \">" +
                                                                "<i class=\"fa fa-folder-open\"></i>&nbsp;&nbsp;Buka" +
                                                        "</a>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"margin: 0px; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; margin-top: 5px; margin-bottom: 5px; border-color: #E9EFF5;\" />" +
                                        "</div>" +
                                    "</div>";

                id++;
            }

            string s_kurikulum = DAO_Rapor_StrukturNilai.GetKurikulumByKelas(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());            
            s_url_1 = ResolveUrl(
                        (s_url_ok + (s_siswa_url.Trim() != "" ? "?sis=" : "") + s_siswa_url) +
                        (s_kurikulum.Trim() != "" ? "&k=" + s_kurikulum : "") +
                        (QS.GetTahunAjaranPure().Trim() != "" ? "&t=" + QS.GetTahunAjaranPure() : "") +
                        (QS.GetSemester().Trim() != "" ? "&s=" + QS.GetSemester() : "") +
                        (QS.GetKelas().Trim() != "" ? "&kd=" + QS.GetKelas() : "")
                    );

            lnkOKCetakSatuKelas.Attributes["onclick"] = "window.open('" + s_url_1 + "', '_blank', 'left=0,top=0,width=800,height=900,toolbar=0,scrollbars=0,status=0;'); " +
                                                        "return false; ";

            txtCountSiswa.Value = GetListSiswa().Count.ToString();
            txtIndexSiswa.Value = "0";
        }

        protected List<Siswa> GetListSiswa()
        {
            return DAO_Siswa.GetByRombel_Entity(
                        QS.GetUnit(),
                        QS.GetKelas(),
                        QS.GetTahunAjaran(),
                        QS.GetSemester()
                    );
        }
        
        protected void ShowDataSiswa()
        {
            if (Libs.GetStringToInteger(txtIndexSiswa.Value) >= 0 &&
                Libs.GetStringToInteger(txtCountSiswa.Value) > 0)
            {
                txtIDSiswa.Value = "";
                Siswa m_siswa = GetListSiswa()[Libs.GetStringToInteger(txtIndexSiswa.Value)];
                if (m_siswa != null)
                {
                    if (m_siswa.Nama != null)
                    {

                        txtIDSiswa.Value = m_siswa.Kode.ToString();
                        lblNamaSiswaInfo.Text = Libs.GetPerbaikiEjaanNama(Libs.GetPersingkatNama(m_siswa.Nama, 4).ToUpper());
                        lblKelasSiswaInfo.Text = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet).Nama;
                        lblInfoPeriode.Text = "<span style=\"font-weight: bold;\">" +
                                                QS.GetTahunAjaran() +
                                              "</span>" +
                                              (
                                                txtSemester.Value != ""
                                                ? "&nbsp;Semester&nbsp;" +
                                                  "<span style=\"font-weight: bold;\">" +
                                                    txtSemester.Value +
                                                  "</span>"
                                                : ""
                                              );

                        string s_kurikulum = DAO_Rapor_StrukturNilai.GetKurikulumByKelas(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas());
                        
                        s_url_2 = ResolveUrl(
                            s_url_ok + "?sis=" + txtIDSiswa.Value + ";" +
                            (s_kurikulum.Trim() != "" ? "&k=" + s_kurikulum : "") +
                            (QS.GetTahunAjaranPure().Trim() != "" ? "&t=" + QS.GetTahunAjaranPure() : "") +
                            (QS.GetSemester().Trim() != "" ? "&s=" + QS.GetSemester() : "") +
                            (QS.GetKelas().Trim() != "" ? "&kd=" + QS.GetKelas() : "")
                        );

                        lnkOKCetakAktif.Attributes["onclick"] = "window.open('" + s_url_2 + "', '_blank', 'left=0,top=0,width=800,height=900,toolbar=0,scrollbars=0,status=0;'); " +
                                                                "return false; ";
                        lnkOKCetakBahasaKD.Attributes["onclick"] = "window.open('" + s_url_2 + "&j=bkd', '_blank', 'left=0,top=0,width=800,height=900,toolbar=0,scrollbars=0,status=0;'); " +
                                                                   "return false; ";

                        if (s_kurikulum == Libs.JenisKurikulum.SD.KURTILAS)
                        {
                            ltrLTS.Text = DAO_Rapor_LTS.GetHTMLReport_KURTILAS(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), false, m_siswa.Kode.ToString());
                        }
                        else if (s_kurikulum == Libs.JenisKurikulum.SD.KTSP)
                        {
                            ltrLTS.Text = DAO_Rapor_LTS.GetHTMLReport_KTSP(this.Page, QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas(), false, m_siswa.Kode.ToString());
                        }

                    }
                }
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
            string s_kelas = "";
            foreach (string item_kelas in QS.GetLevel().Split(new string[] { ";" }, StringSplitOptions.None))
            {
                Kelas m_kelas = DAO_Kelas.GetByID_Entity(item_kelas);
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null)
                    {
                        s_kelas += (s_kelas.Trim() != "" ? "," : "") +
                                   m_kelas.Nama;
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

        protected void lnkPilihKelas_Click(object sender, EventArgs e)
        {

        }

        protected void lnkOKPengaturan_Click(object sender, EventArgs e)
        {
            Rapor_LTS_MengetahuiGuruKelas m = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()).FirstOrDefault();
            if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()).Count == 0)
            {
                DAO_Rapor_LTS_MengetahuiGuruKelas.Insert(new Rapor_LTS_MengetahuiGuruKelas {
                    TahunAjaran = QS.GetTahunAjaran(),
                    Semester = QS.GetSemester(),
                    Rel_KelasDet = QS.GetKelas(),
                    NamaGuru = txtNamaGuru.Text,
                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLTS.Text)
                });
            }
            else
            {
                DAO_Rapor_LTS_MengetahuiGuruKelas.Update(new Rapor_LTS_MengetahuiGuruKelas
                {
                    TahunAjaran = QS.GetTahunAjaran(),
                    Semester = QS.GetSemester(),
                    Rel_KelasDet = QS.GetKelas(),
                    NamaGuru = txtNamaGuru.Text,
                    Tanggal = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalLTS.Text)
                });
            }
            ShowDataSiswa();
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void lnkPengaturanLTS_Click(object sender, EventArgs e)
        {
            txtTanggalLTS.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
            txtNamaGuru.Text = "";
            Rapor_LTS_MengetahuiGuruKelas m = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(QS.GetTahunAjaran(), QS.GetSemester(), QS.GetKelas()).FirstOrDefault();
            if (m != null)
            {
                txtNamaGuru.Text = m.NamaGuru;
                txtTanggalLTS.Text = Libs.GetTanggalIndonesiaFromDate(m.Tanggal, false);
            }
            txtKeyAction.Value = JenisAction.ShowPengaturanLTS.ToString();
        }
    }
}