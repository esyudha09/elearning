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
    public partial class wf_NilaiSiswaEkskul : System.Web.UI.Page
    {
        const string NILAI_DEFAULT = "BAIK";
        protected List<Siswa> GetListSiswaEkskul(string tahun_ajaran, string semester, string rel_mapel)
        {
            List<Siswa> lst_hasil = new List<Siswa>();

            List<FormasiEkskulDet> lst_formasil_ekskul = DAO_FormasiEkskulDet.GetByGuruByMapelBySM_Entity(rel_mapel, tahun_ajaran, semester);
            foreach (FormasiEkskulDet item in lst_formasil_ekskul)
            {
                lst_hasil.Add(DAO_Siswa.GetByKode_Entity(
                    tahun_ajaran,
                    semester,
                    item.Rel_Siswa));
            }

            return lst_hasil;
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
            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/running.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Nilai Ekstrakurikuler";

            if (!IsPostBack)
            {
                LoadData();
            }

            if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_PREVIEW_NILAI)
            {
                this.Master.ShowSubHeaderGuru = false;
            }
            else if (Libs.GetQueryString("token") == Constantas.SMA.TOKEN_NILAI_EKSKUL)
            {
                this.Master.ShowSubHeaderGuru = false;
            }
            else
            {
                this.Master.ShowSubHeaderGuru = true;
            }
        }

        public Sekolah GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
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

            List<Siswa> lst_siswa = DAO_FormasiEkskulDet.GetListSiswaEkskul(
                    tahun_ajaran,
                    semester,
                    rel_mapel
                );
            if (lst_siswa.Count == 0)
            {
                lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelasPerwalian_Entity(
                        GetUnit().Kode.ToString(),
                        rel_kelas_det,
                        tahun_ajaran,
                        semester
                    );
            }

            //status bar
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
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
                ltrStatusBar.Text += (ltrStatusBar.Text.Trim() != "" ? "&nbsp;&nbsp;" : "") +
                                     "&nbsp;" +
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

            List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, rel_kelas, rel_mapel
                );
            if (lst_sn.Count == 0)
            {
                lst_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, "", rel_mapel
                );
            }

            List<DAO_Rapor_StrukturNilai.StrukturNilaiPredikat> lst_sn_predikat = new List<DAO_Rapor_StrukturNilai.StrukturNilaiPredikat>();
            if (lst_sn.Count == 1)
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m_sn = lst_sn.FirstOrDefault();
                if (m_sn != null)
                {
                    if (m_sn.TahunAjaran != null)
                    {
                        lst_sn_predikat = DAO_Rapor_StrukturNilai.GetPredikatByHeader_Entity(m_sn.Kode.ToString()).
                                          FindAll(m0 => !(m0.Predikat.Trim() == "" && m0.Deskripsi.Trim() == ""));
                    }
                }
            }

            string html_cbo_predikat = "";
            int id = 1;
            foreach (Siswa m_siswa in lst_siswa)
            {
                string s_predikat = NILAI_DEFAULT;
                string kode_predikat = "";
                string lts_hd = "";
                string s_sakit = "";
                string s_izin = "";
                string s_alpa = "";

                bool ada_nilai = false;
                bool ada_seleksi = false;
                List<Rapor_NilaiEkskulSiswa> lst_nilai_ekskul = DAO_Rapor_NilaiEkskulSiswa.GetByTABySMByMapelBySiswa_Entity(tahun_ajaran, semester, rel_mapel, m_siswa.Kode.ToString());
                if (lst_nilai_ekskul.Count > 0)
                {
                    kode_predikat = lst_nilai_ekskul.FirstOrDefault().Nilai;
                    lts_hd = lst_nilai_ekskul.FirstOrDefault().LTS_HD;
                    s_sakit = lst_nilai_ekskul.FirstOrDefault().Sakit;
                    s_izin = lst_nilai_ekskul.FirstOrDefault().Izin;
                    s_alpa = lst_nilai_ekskul.FirstOrDefault().Alpa;
                    ada_nilai = true;
                }

                html_cbo_predikat = "";
                foreach (DAO_Rapor_StrukturNilai.StrukturNilaiPredikat item in lst_sn_predikat)
                {
                    if (ada_nilai)
                    {
                        if (item.Kode.ToString().Trim().ToUpper() == kode_predikat.ToString().Trim().ToUpper())
                        {
                            html_cbo_predikat += "<option selected value=\"" + item.Kode.ToString() + "\">" +
                                                        item.Deskripsi +
                                                 "</option>";
                            ada_seleksi = true;
                        }
                        else
                        {
                            html_cbo_predikat += "<option value=\"" + item.Kode.ToString() + "\">" +
                                                        item.Deskripsi +
                                                 "</option>";
                        }
                    }
                    else
                    {
                        if (item.Deskripsi.Trim().ToUpper() == s_predikat.Trim().ToUpper())
                        {
                            html_cbo_predikat += "<option selected value=\"" + item.Kode.ToString() + "\">" +
                                                        item.Deskripsi +
                                                 "</option>";
                            kode_predikat = item.Kode.ToString();
                            ada_seleksi = true;
                        }
                        else
                        {
                            html_cbo_predikat += "<option value=\"" + item.Kode.ToString() + "\">" +
                                                        item.Deskripsi +
                                                 "</option>";
                        }
                    }
                }

                if (!ada_seleksi && lst_sn_predikat.Count > 0)
                {
                    kode_predikat = lst_sn_predikat.FirstOrDefault().Kode.ToString();
                }

                if (html_cbo_predikat.Trim() != "")
                {
                    html_cbo_predikat =
                        "<label style=\"font-size: small; color: grey;\">Nilai</label><br />" +
                        "<select onchange=\"SaveNilaiEkskul(" +
                                                "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                                "'" + AtributPenilaian.Semester + "', " +
                                                "'" + AtributPenilaian.Mapel + "', " +
                                                "'" + m_siswa.Kode.ToString() + "', " +
                                                "this.value, " +
                                                "txt_lts_hd_" + m_siswa.Kode.ToString().Replace("-", "_") + ".value, " +
                                                "txt_sakit_" + m_siswa.Kode.ToString().Replace("-", "_") + ".value, " +
                                                "txt_izin_" + m_siswa.Kode.ToString().Replace("-", "_") + ".value, " +
                                                "txt_alpa_" + m_siswa.Kode.ToString().Replace("-", "_") + ".value " +
                                           ");\" " +
                                "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                "name=\"cbo_nilai_ekskul[]\" " +
                                "title=\" Nilai Ekskul \" " +
                                "class=\"text-input\" " +
                                "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                            html_cbo_predikat +
                        "</select>";
                }

                string s_option_lts_hd = "";
                for (int i = 0; i <= 25; i++)
                {
                    s_option_lts_hd += "<option " + (lts_hd == i.ToString() ? " selected " : "") + " value=\"" + i.ToString() + "\">" +
                                            i.ToString() +
                                       "</option>";
                }

                string html_cbo_lts_hd =
                    //dropdown
                    //"<label style=\"font-size: small; color: grey;\">LTS HD</label><br />" +
                    //"<select " +
                    //        "onchange=\"SaveLTSHDEkskul(" +
                    //                        "'" + AtributPenilaian.TahunAjaranPure + "', " +
                    //                        "'" + AtributPenilaian.Semester + "', " +
                    //                        "'" + AtributPenilaian.Mapel + "', " +
                    //                        "'" + m_siswa.Kode.ToString() + "', " +
                    //                        "this.value " +
                    //                ");\" " + 
                    //        "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                    //        "name=\"cbo_absen_ekskul[]\" " +
                    //        "title=\" Absen Ekskul \" " +
                    //        "class=\"text-input\" " +
                    //        "id=\"txt_lts_hd_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                    //        "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                    //    s_option_lts_hd +
                    //"</select>";

                    "<label style=\"font-size: small; color: grey;\">LTS Kehadiran</label><br />" +
                    "<input type=\"text\" " +
                            "onchange=\"SaveLTSHDEkskul(" +
                                            "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                            "'" + AtributPenilaian.Semester + "', " +
                                            "'" + AtributPenilaian.Mapel + "', " +
                                            "'" + m_siswa.Kode.ToString() + "', " +
                                            "this.value " +
                                    ");\" " +
                            "onblur=\"SaveLTSHDEkskul(" +
                                            "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                            "'" + AtributPenilaian.Semester + "', " +
                                            "'" + AtributPenilaian.Mapel + "', " +
                                            "'" + m_siswa.Kode.ToString() + "', " +
                                            "this.value " +
                                    ");\" " +
                            "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                            "name=\"cbo_absen_ekskul[]\" " +
                            "title=\" Absen Ekskul \" " +
                            "class=\"text-input\" " +
                            "value=\"" + lts_hd + "\" " +
                            "id=\"txt_lts_hd_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                            "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\" />";

                string s_option_sakit = "";
                for (int i = 0; i <= 25; i++)
                {
                    s_option_sakit += "<option " + (s_sakit == i.ToString() ? " selected " : "") + " value=\"" + i.ToString() + "\">" +
                                            i.ToString() +
                                       "</option>";
                }
                string s_option_izin = "";
                for (int i = 0; i <= 25; i++)
                {
                    s_option_izin += "<option " + (s_izin == i.ToString() ? " selected " : "") + " value=\"" + i.ToString() + "\">" +
                                            i.ToString() +
                                       "</option>";
                }
                string s_option_alpa = "";
                for (int i = 0; i <= 25; i++)
                {
                    s_option_alpa += "<option " + (s_alpa == i.ToString() ? " selected " : "") + " value=\"" + i.ToString() + "\">" +
                                            i.ToString() +
                                       "</option>";
                }

                string html_cbo_sakit =
                        "<label style=\"font-size: small; color: grey;\">Jumlah<br />Sakit</label><br />" +
                        "<select " +
                                "onchange=\"SaveSakitEkskul(" +
                                                "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                                "'" + AtributPenilaian.Semester + "', " +
                                                "'" + AtributPenilaian.Mapel + "', " +
                                                "'" + m_siswa.Kode.ToString() + "', " +
                                                "this.value " +
                                        ");\" " +
                                "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                "name=\"cbo_absen_ekskul_sakit[]\" " +
                                "title=\" Absen Ekskul \" " +
                                "class=\"text-input\" " +
                                "id=\"txt_sakit_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                            s_option_sakit +
                        "</select>";

                string html_cbo_izin =
                        "<label style=\"font-size: small; color: grey;\">Jumlah<br />Izin</label><br />" +
                        "<select " +
                                "onchange=\"SaveIzinEkskul(" +
                                                "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                                "'" + AtributPenilaian.Semester + "', " +
                                                "'" + AtributPenilaian.Mapel + "', " +
                                                "'" + m_siswa.Kode.ToString() + "', " +
                                                "this.value " +
                                        ");\" " +
                                "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                "name=\"cbo_absen_ekskul_izin[]\" " +
                                "title=\" Absen Ekskul \" " +
                                "class=\"text-input\" " +
                                "id=\"txt_izin_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                            s_option_izin +
                        "</select>";

                string html_cbo_alpa =
                        "<label style=\"font-size: small; color: grey;\">Jumlah<br />Alpa</label><br />" +
                        "<select " +
                                "onchange=\"SaveAlpaEkskul(" +
                                                "'" + AtributPenilaian.TahunAjaranPure + "', " +
                                                "'" + AtributPenilaian.Semester + "', " +
                                                "'" + AtributPenilaian.Mapel + "', " +
                                                "'" + m_siswa.Kode.ToString() + "', " +
                                                "this.value " +
                                        ");\" " +
                                "onkeydown=\"if(event.keyCode === 13) event.preventDefault();\" " +
                                "name=\"cbo_absen_ekskul_alpa[]\" " +
                                "title=\" Absen Ekskul \" " +
                                "class=\"text-input\" " +
                                "id=\"txt_alpa_" + m_siswa.Kode.ToString().Replace("-", "_") + "\" " +
                                "style=\"width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px;\">" +
                            s_option_alpa +
                        "</select>";

                DAO_Rapor_NilaiEkskulSiswa.SaveNilaiEkskul(
                        tahun_ajaran,
                        semester,
                        rel_mapel,
                        m_siswa.Kode.ToString(),
                        kode_predikat,
                        lts_hd,
                        s_sakit,
                        s_izin,
                        s_alpa
                    );

                string kelas_det = "";
                string s_bg = (id % 2 != 0 ? "#F9F9F9" : "#FFFFFF");

                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(m_siswa.Rel_KelasDet);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {
                        kelas_det = m_kelas_det.Nama;
                    }
                }

                string s_panggilan = "<label title=\"&nbsp;" + Libs.GetPerbaikiEjaanNama(m_siswa.Nama) + "&nbsp;\" " +
                                            "style=\"border-style: solid; border-width: 2px; border-color: #e8e8e8; border-radius: 10px; " + (id % 2 == 0 ? " font-weight: bold; " : " background-color: #f4f4f4; color: black; ") + " padding-left: 7px; padding-right: 7px; margin-right: 5px; margin-bottom: 3px;\">" + m_siswa.Panggilan.Trim().ToLower() + "</label>";

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
                                        "<td style=\"padding: 0px; font-size: small; width: 200px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                            html_cbo_predikat +
                                        "</td>" +
                                        "<td style=\"padding: 0px; font-size: small; width: 100px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                            html_cbo_lts_hd +
                                        "</td>" +
                                        (
                                            Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020
                                            ? 
                                                "<td style=\"padding: 0px; font-size: small; width: 100px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                                    html_cbo_sakit +
                                                "</td>" +
                                                "<td style=\"padding: 0px; font-size: small; width: 100px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                                    html_cbo_izin +
                                                "</td>" +
                                                "<td style=\"padding: 0px; font-size: small; width: 100px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 7px;\">" +
                                                    html_cbo_alpa +
                                                "</td>"
                                            : ""
                                        ) +
                                    "</tr>";
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
        }
    }
}