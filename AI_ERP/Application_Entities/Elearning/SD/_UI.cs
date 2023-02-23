using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Entities.Elearning.SD
{
    public static class _UI
    {
        public static bool IsReadOnlyByLockedSetting(string tahun_ajaran, string semester)
        {
            if (DAO_Rapor_Arsip.GetAll_Entity().Count > 0)
            {
                if (DAO_Rapor_Arsip.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester).OrderByDescending(m => m.TanggalClosing).FirstOrDefault().TanggalClosing < DateTime.Now)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsReadonlyNilai(
                string kode_struktur_nilai, string nik_guru, string rel_kelas_det, 
                string rel_mapel, string rel_mapel_sikap, string rel_mapel_sikap_info, string tahun_ajaran, string semester
            )
        {
            bool is_readonly = IsReadOnlyByLockedSetting(tahun_ajaran, semester);
            
            
            if (rel_kelas_det.Trim() != "")
            {
                if (rel_mapel_sikap.Trim() == "")
                {
                    if (!is_readonly)
                    {
                        is_readonly = !DAO_Rapor_StrukturNilai.IsNilaiCanEdit(kode_struktur_nilai, nik_guru, rel_kelas_det);
                    }
                }
                if (DAO_Rapor_Arsip.GetAll_Entity().Count > 0)
                {
                    if (DAO_Rapor_Arsip.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == tahun_ajaran && m0.Semester == semester).OrderByDescending(m => m.TanggalClosing).FirstOrDefault().TanggalClosing < DateTime.Now)
                    {
                        is_readonly = true;
                    }
                }
                if (is_readonly)
                {
                    if (DAO_IsiNilai_Log.GetBisaEdit_Entity(nik_guru, rel_mapel, rel_kelas_det, tahun_ajaran, semester).Count > 0)
                    {
                        is_readonly = false;
                    }
                }
            }

            //sebagai guru kelas or wali kelas
            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && 
                rel_mapel_sikap.Trim() != "" && 
                rel_mapel_sikap.Trim().ToLower() != rel_mapel_sikap_info.Trim().ToLower() && 
                rel_mapel_sikap_info.Trim() != "")
            {
                is_readonly = true;
            }

            return is_readonly;
        }

        private static bool IsShowSikapSpiritual(string s_nama_mapel)
        {
            if (
                    s_nama_mapel.ToLower().IndexOf("quran") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("qur'an") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("agama") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("jasmani") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("kesehatan") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("olah") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("raga") >= 0 ||
                    s_nama_mapel.ToLower().IndexOf("pjok") >= 0
                )
            {
                return true;
            }

            if(s_nama_mapel.Trim() == "") return true;
            return false;
        }

        public static bool IsReadonlyNilaiVolunteer(
                string tahun_ajaran, string semester, string nik_guru, string rel_kelas_det
            )
        {
            bool is_readonly = false;
            if (DAO_Rapor_Arsip.GetAll_Entity().Count > 0)
            {
                if (DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                        m => m.TahunAjaran == tahun_ajaran &&
                             m.Semester == semester
                    ).OrderByDescending(m => m.TanggalClosing).FirstOrDefault().TanggalClosing < DateTime.Now)
                {
                    is_readonly = true;
                }
            }

            return is_readonly;
        }

        public static void InitModalListNilai(
                System.Web.UI.Page page,
                System.Web.UI.WebControls.Literal ltrListNilaiAkademik,
                System.Web.UI.WebControls.Literal ltrListNilaiEkskul,
                System.Web.UI.WebControls.Literal ltrListNilaiSikap,
                System.Web.UI.WebControls.Literal ltrListNilaiVolunteer,
                System.Web.UI.WebControls.Literal ltrListNilaiRapor,
                string tahun_ajaran, string rel_mapel, string rel_kelas_det, string no_induk_guru
            )
        {
            //return;
            //if (rel_kelas_det.Trim() != "")
            //{
            //    if (DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama.Trim().ToUpper() != "V-A") return;
            //}
            string s_html_nilai_akademik = "";
            string s_html_nilai_sikap = "";
            string s_html_nilai_ekskul = "";
            string s_html_nilai_volunteer = "";
            string s_html_nilai_rapor = "";
            string s_url = "";
            string s_semester = Libs.GetSemesterByTanggal(DateTime.Now).ToString();

            string s_nama_mapel = "";
            if (rel_mapel.Trim() != "")
            {
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_nama_mapel = m_mapel.Nama;
                    }
                }
            }

            bool b_nilai_can_edit = true;
            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(rel_kelas_det);

            string list_mapel = "";
            string list_mapel_sikap_rapor = "";
            string list_mapel_sikap_lts = "";
            string list_mapel_ekskul = "";
            string list_volunteer = "";
            string list_rapor = "";

            int id_akademik = 0;
            int id_sikap = 0;
            int id_ekskul = 0;
            int id_volunteer = 0;
            int id_rapor = 0;

            string html_tile = "";
            string html_tile_sikap = "";
            string html_tile_ekskul = "";
            string html_tile_volunteer = "";
            string html_tile_rapor = "";

            string jenis_sikap = "";
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    List<Rapor_StrukturNilai> lst_mapel_nilai_sn = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                tahun_ajaran,
                                m_kelas.Rel_Kelas.ToString(),
                                no_induk_guru
                            ).OrderByDescending(m => m.Semester).ToList();
                    html_tile = "";
                    html_tile_sikap = "";
                    html_tile_ekskul = "";
                    List<string> lst_semester = lst_mapel_nilai_sn.Select(m => m.Semester).Distinct().ToList();
                    int id_semester = 1;
                    foreach (var semester in lst_semester)
                    {
                        bool b_valid = true;
                        if (b_valid)
                        {
                            List<Rapor_StrukturNilai> lst_mapel_nilai = new List<Rapor_StrukturNilai>();
                            //list struktur nilai mapel non sikap
                            //List<Rapor_StrukturNilai> lst_mapel_nilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                            //        tahun_ajaran,
                            //        m_kelas.Rel_Kelas.ToString(),
                            //        no_induk_guru
                            //    ).FindAll(m => m.Semester == semester);
                            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                            {
                                lst_mapel_nilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByKelasDetByGuru_Entity(
                                    tahun_ajaran,
                                    m_kelas.Rel_Kelas.ToString(),
                                    m_kelas.Kode.ToString(),
                                    no_induk_guru
                                ).FindAll(m => m.Semester == semester);
                            }
                            else
                            {
                                lst_mapel_nilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByKelasDetASGuruMapel_Entity(
                                    tahun_ajaran,
                                    m_kelas.Rel_Kelas.ToString(),
                                    m_kelas.Kode.ToString(),
                                    no_induk_guru
                                ).FindAll(m => m.Semester == semester);
                            }

                            id_akademik = 0;
                            id_sikap = 0;
                            id_ekskul = 0;
                            id_rapor = 0;

                            list_mapel = "";
                            list_mapel_sikap_lts = "";
                            list_mapel_sikap_rapor = "";
                            list_mapel_ekskul = "";
                            list_volunteer = "";
                            list_rapor = "";

                            foreach (Rapor_StrukturNilai m in lst_mapel_nilai)
                            {
                                b_nilai_can_edit = !IsReadonlyNilai(
                                        m.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, m_kelas.Kode.ToString(), m.Rel_Mapel.ToString(), "", "", m.TahunAjaran, m.Semester

                                    );

                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());

                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {

                                        if (m_mapel.Jenis != Libs.JENIS_MAPEL.SIKAP &&
                                            m_mapel.Jenis != Libs.JENIS_MAPEL.EKSTRAKURIKULER &&
                                            m_mapel.Jenis != Libs.JENIS_MAPEL.VOLUNTEER
                                        )
                                        {
                                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE +
                                                     "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                     "ft=" + Libs.GetQueryString("ft") + "&" +
                                                     "s=" + m.Semester + "&" +
                                                     "kd=" + rel_kelas_det + "&" +
                                                     (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                        : ""
                                                     ) +
                                                     (
                                                        Libs.GetQueryString("g").Trim() != ""
                                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                        : ""
                                                     ) +
                                                     (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                        : ""
                                                     ) +
                                                     "m=" + m.Rel_Mapel.ToString();

                                            list_mapel += ("<tr>" +
                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                (
                                                                    lst_mapel_nilai.Count > 1
                                                                    ? (id_akademik + 1).ToString() +
                                                                        ".&nbsp;"
                                                                    : "<i class=\"fa fa-file-o\"></i>"
                                                                ) +
                                                            "</td>" +
                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                    m_mapel.Nama +
                                                                    "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                        ", " +
                                                                        "Semester " +
                                                                        m.Semester +
                                                                    "</span>" +
                                                                    (
                                                                        b_nilai_can_edit
                                                                        ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                        : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                    ) +
                                                                "</label>" +
                                                            "</td>" +
                                                            "</tr>" +
                                                            "<tr>" +
                                                            "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                "<hr style=\"margin: 0px;\" />" +
                                                            "</td>" +
                                                            "</tr>"
                                                          );
                                            id_akademik++;
                                        }

                                    }
                                }
                            }

                            string id_ui = "ui_tile_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
                            html_tile = "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                            "<div data-parent=\"#div_semester_nilai_akademik\" data-target=\"#" + id_ui + "\" data-toggle=\"tile\" style=\"background-color: " + (id_semester % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                                "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                    "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                        "<div class=\"avatar avatar-sm\">" +
                                                            "<i class=\"fa fa-calendar\"></i>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"tile-inner\">" +
                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-active-show collapse\" id=\"" + id_ui + "\" style=\"height: 0px;\">" +
                                                "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                        "<hr style=\"margin: 0px; display: none;\" />" +
                                                        "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                            "<tbody>" +
                                                                (
                                                                    id_akademik > 0
                                                                    ? list_mapel
                                                                    : ("<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                            "..:: Data Kosong ::.." +
                                                                        "</td>" +
                                                                      "</tr>" +
                                                                      "<tr style=\"display: none;\">" +
                                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                            "<hr style=\"margin: 0px;\" />" +
                                                                        "</td>" +
                                                                      "</tr>")
                                                                ) +
                                                            "</tbody>" +
                                                        "</table>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                         "</div>";
                            s_html_nilai_akademik += html_tile;


                            //list struktur nilai sikap
                            List<Rapor_StrukturNilai> lst_mapel_nilai_sikap = DAO_Rapor_StrukturNilai.GetMapelSikapByTAByKelas_Entity(
                                    tahun_ajaran,
                                    m_kelas.Rel_Kelas.ToString()
                                ).FindAll(m => m.Semester == semester);

                            foreach (Rapor_StrukturNilai m in lst_mapel_nilai_sikap)
                            {
                                b_nilai_can_edit = !IsReadonlyNilai(
                                        m.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, m_kelas.Kode.ToString(), m.Rel_Mapel.ToString(), "", "", m.TahunAjaran, m.Semester
                                    );
                                if (!Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && s_nama_mapel.Trim() != "")
                                {
                                    if (lst_mapel_nilai.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToUpper().Trim()).Count > 0)
                                    {
                                        b_nilai_can_edit = !IsReadonlyNilai(
                                            lst_mapel_nilai.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToUpper().Trim()).FirstOrDefault().Kode.ToString(), 
                                            Libs.LOGGED_USER_M.NoInduk, m_kelas.Kode.ToString(), rel_mapel, "", "", m.TahunAjaran, m.Semester
                                        );
                                    }
                                }

                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());

                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {
                                        bool b_addsikap = true;
                                        jenis_sikap = m_mapel.Nama;

                                        if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                                        {
                                            if (jenis_sikap.ToLower().IndexOf("spiritual") >= 0)
                                            {
                                                b_addsikap = IsShowSikapSpiritual(s_nama_mapel);
                                                if (!b_addsikap && Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()) b_addsikap = true;
                                            }
                                        }

                                        if (b_addsikap)
                                        {
                                            //sikap rapor
                                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_SIKAP_SEMESTER.ROUTE +
                                                    "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                                    "s=" + m.Semester + "&" +
                                                    "kd=" + rel_kelas_det + "&" +
                                                    (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                        : ""
                                                    ) +
                                                    (
                                                        Libs.GetQueryString("g").Trim() != ""
                                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                        : ""
                                                    ) +
                                                    (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                        : ""
                                                    ) +
                                                    (
                                                        rel_mapel.Trim() != "" && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                        ? "m=" + rel_mapel + "&"
                                                        : ""
                                                    ) +
                                                    "ms=" + m.Rel_Mapel.ToString();
                                            list_mapel_sikap_rapor +=
                                                                (
                                                                    "<tr>" +
                                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                            "<i class=\"fa fa-file-o\"></i>" +
                                                                        "</td>" +
                                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                                m_mapel.Nama +
                                                                                " (Rapor)" +
                                                                                (
                                                                                    Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                                                    ? "<span style=\"color: #329CC3;\">" +
                                                                                        " GURU KELAS" +
                                                                                        "</span>"
                                                                                    : ""
                                                                                ) +
                                                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                    ", " +
                                                                                    "Semester " +
                                                                                    m.Semester +
                                                                                "</span>" +
                                                                            (
                                                                                b_nilai_can_edit
                                                                                ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                                : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                            ) +
                                                                            "</label>" +
                                                                        "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                            "<hr style=\"margin: 0px;\" />" +
                                                                        "</td>" +
                                                                        "</tr>"
                                                                );
                                            //end sikap rapor
                                            id_sikap++;

                                            //sikap LTS
                                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE +
                                                    "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                                    "s=" + m.Semester + "&" +
                                                    "kd=" + rel_kelas_det + "&" +
                                                    (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                        : ""
                                                    ) +
                                                    (
                                                        Libs.GetQueryString("g").Trim() != ""
                                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                        : ""
                                                    ) +
                                                    (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                        : ""
                                                    ) +
                                                    (
                                                        rel_mapel.Trim() != "" && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                        ? "m=" + rel_mapel + "&"
                                                        : ""
                                                    ) +
                                                    "ms=" + m.Rel_Mapel.ToString();
                                            list_mapel_sikap_lts +=
                                                            (
                                                                "<tr>" +
                                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                                    "</td>" +
                                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                        "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                            m_mapel.Nama +
                                                                            " (LTS)" +
                                                                            (
                                                                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                                                ? "<span style=\"color: #329CC3;\">" +
                                                                                    " GURU KELAS" +
                                                                                    "</span>"
                                                                                : ""
                                                                            ) +
                                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                ", " +
                                                                                "Semester " +
                                                                                m.Semester +
                                                                            "</span>" +
                                                                        (
                                                                            b_nilai_can_edit
                                                                            ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                            : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                        ) +
                                                                        "</label>" +
                                                                    "</td>" +
                                                                    "</tr>" +
                                                                    "<tr>" +
                                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px;\" />" +
                                                                    "</td>" +
                                                                    "</tr>"
                                                            );
                                            //end sikap LTS
                                            id_sikap++;
                                        }

                                        if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                                        {
                                            string kurikulum = DAO_Rapor_StrukturNilai.GetKurikulumByKelas(
                                                    tahun_ajaran, semester, rel_kelas_det
                                                );
                                            if (kurikulum == Libs.JenisKurikulum.SD.KURTILAS)
                                            {
                                                //list struktur nilai mapel sikap bidang studi
                                                List<Rapor_StrukturNilai> lst_mapel_sikap_onnilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                                        tahun_ajaran,
                                                        m_kelas.Rel_Kelas.ToString(),
                                                        no_induk_guru
                                                    ).FindAll(m0 => m0.Semester == semester);

                                                foreach (Rapor_StrukturNilai m0 in lst_mapel_sikap_onnilai)
                                                {
                                                    m_mapel = DAO_Mapel.GetByID_Entity(m0.Rel_Mapel.ToString());

                                                    b_addsikap = true;
                                                    if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020)
                                                    {
                                                        if (jenis_sikap.ToLower().IndexOf("spiritual") >= 0)
                                                        {
                                                            b_addsikap = IsShowSikapSpiritual(m_mapel.Nama);
                                                        }
                                                    }

                                                    if (b_addsikap)
                                                    {
                                                        bool b_is_can_edit = b_nilai_can_edit;
                                                        b_is_can_edit = false;
                                                        //if (m_mapel != null && !b_nilai_can_edit)
                                                        if (m_mapel != null)
                                                        {
                                                            if (m_mapel.Nama != null)
                                                            {
                                                                var lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapelAsOther_Entity(
                                                                    no_induk_guru, m0.TahunAjaran, m0.Semester, rel_kelas_det, m0.Rel_Mapel.ToString()
                                                                );

                                                                bool is_guru_matpel = false;
                                                                //cek is guru matpel
                                                                is_guru_matpel = (
                                                                    DAO_FormasiGuruMapelDet.GetByTABySMByKelasByMapel_Entity(
                                                                        tahun_ajaran, semester, m_kelas.Rel_Kelas.ToString(), m0.Rel_Mapel.ToString()
                                                                    ).FindAll(m1 => m1.Rel_FormasiGuruMapel.ToString().Trim() != "" &&
                                                                                    m1.Rel_FormasiGuruMapel.ToString().Trim() != Constantas.GUID_NOL).Count > 0
                                                                    ? true
                                                                    : false
                                                                );
                                                                //end cek is guru matpel

                                                                if ((
                                                                        m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB ||
                                                                        m_mapel.Jenis == Libs.JENIS_MAPEL.PILIHAN) &&
                                                                        lst_guru_mapel.Count > 0 &&
                                                                        is_guru_matpel
                                                                    )
                                                                {
                                                                    //sikap rapor
                                                                    s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_SIKAP_SEMESTER.ROUTE +
                                                                            "?t=" + RandomLibs.GetRndTahunAjaran(m0.TahunAjaran) + "&" +
                                                                            "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                            "s=" + m0.Semester + "&" +
                                                                            "kd=" + rel_kelas_det + "&" +
                                                                            (
                                                                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                                ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                Libs.GetQueryString("g").Trim() != ""
                                                                                ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                                                ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                                                : ""
                                                                            ) +
                                                                            "m=" + m0.Rel_Mapel.ToString() + "&" +
                                                                            "ms=" + m.Rel_Mapel.ToString();

                                                                    list_mapel_sikap_rapor +=
                                                                                   (
                                                                                        "<tr>" +
                                                                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                                                    "<i class=\"fa fa-file-o\"></i>" +
                                                                                                "</td>" +
                                                                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                                                    "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                                                        jenis_sikap +
                                                                                                        " (Rapor)&nbsp;" +
                                                                                                        "<span style=\"color: #FF0000;\">" +
                                                                                                            m_mapel.Nama +
                                                                                                        "</span>" +
                                                                                                        "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                                            ", " +
                                                                                                            "Semester " +
                                                                                                            m0.Semester +
                                                                                                        "</span>" +
                                                                                                        (
                                                                                                            b_is_can_edit
                                                                                                            ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                                                            : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                                                        ) +
                                                                                                    "</label>" +
                                                                                                "</td>" +
                                                                                              "</tr>" +
                                                                                              "<tr>" +
                                                                                                "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                                                    "<hr style=\"margin: 0px;\" />" +
                                                                                                "</td>" +
                                                                                              "</tr>"
                                                                                    );
                                                                    //end sikap rapor
                                                                    id_sikap++;

                                                                    //sikap LTS
                                                                    s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE +
                                                                            "?t=" + RandomLibs.GetRndTahunAjaran(m0.TahunAjaran) + "&" +
                                                                            "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                            "s=" + m0.Semester + "&" +
                                                                            "kd=" + rel_kelas_det + "&" +
                                                                            (
                                                                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                                ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                Libs.GetQueryString("g").Trim() != ""
                                                                                ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                                                : ""
                                                                            ) +
                                                                            (
                                                                                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                                                ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                                                : ""
                                                                            ) +
                                                                            "m=" + m0.Rel_Mapel.ToString() + "&" +
                                                                            "ms=" + m.Rel_Mapel.ToString();

                                                                    list_mapel_sikap_lts +=
                                                                                    (
                                                                                        "<tr>" +
                                                                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                                                    "<i class=\"fa fa-file-o\"></i>" +
                                                                                                "</td>" +
                                                                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                                                    "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                                                        jenis_sikap +
                                                                                                        " (LTS)&nbsp;" +
                                                                                                        "<span style=\"color: #FF0000;\">" +
                                                                                                            m_mapel.Nama +
                                                                                                        "</span>" +
                                                                                                        "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                                            ", " +
                                                                                                            "Semester " +
                                                                                                            m0.Semester +
                                                                                                        "</span>" +
                                                                                                    (
                                                                                                        b_is_can_edit
                                                                                                        ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                                                        : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                                                    ) +
                                                                                                    "</label>" +
                                                                                                "</td>" +
                                                                                              "</tr>" +
                                                                                              "<tr>" +
                                                                                                "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                                                    "<hr style=\"margin: 0px;\" />" +
                                                                                                "</td>" +
                                                                                              "</tr>"
                                                                                    );
                                                                    //end sikap LTS
                                                                    id_sikap++;
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (kurikulum == Libs.JenisKurikulum.SD.KTSP)
                                            {
                                                //list struktur nilai mapel sikap bidang studi
                                                List<Rapor_StrukturNilai> lst_mapel_sikap_onnilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                                        tahun_ajaran,
                                                        m_kelas.Rel_Kelas.ToString(),
                                                        no_induk_guru
                                                    ).FindAll(m0 => m0.Semester == semester);

                                                foreach (Rapor_StrukturNilai m0 in lst_mapel_sikap_onnilai)
                                                {
                                                    m_mapel = DAO_Mapel.GetByID_Entity(m0.Rel_Mapel.ToString());

                                                    if (m_mapel != null && !b_nilai_can_edit)
                                                    {
                                                        if (m_mapel.Nama != null)
                                                        {
                                                            var lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapelAsOther_Entity(
                                                                no_induk_guru, m0.TahunAjaran, m0.Semester, rel_kelas_det, m0.Rel_Mapel.ToString()
                                                            );

                                                            bool is_guru_matpel = false;
                                                            //cek is guru matpel
                                                            is_guru_matpel = (
                                                                DAO_FormasiGuruMapelDet.GetByTABySMByKelasByMapel_Entity(
                                                                    tahun_ajaran, semester, m_kelas.Rel_Kelas.ToString(), m0.Rel_Mapel.ToString()
                                                                ).FindAll(m1 => m1.Rel_FormasiGuruMapel.ToString().Trim() != "" &&
                                                                                m1.Rel_FormasiGuruMapel.ToString().Trim() != Constantas.GUID_NOL).Count > 0
                                                                ? true
                                                                : false
                                                            );
                                                            //end cek is guru matpel

                                                            if ((
                                                                    m_mapel.Jenis == Libs.JENIS_MAPEL.WAJIB ||
                                                                    m_mapel.Jenis == Libs.JENIS_MAPEL.PILIHAN) &&
                                                                    lst_guru_mapel.Count > 0 &&
                                                                    is_guru_matpel
                                                                )
                                                            {
                                                                //sikap rapor
                                                                s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_SIKAP_SEMESTER.ROUTE +
                                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m0.TahunAjaran) + "&" +
                                                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                        "s=" + m0.Semester + "&" +
                                                                        "kd=" + rel_kelas_det + "&" +
                                                                        (
                                                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                            ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                                            : ""
                                                                        ) +
                                                                        (
                                                                            Libs.GetQueryString("g").Trim() != ""
                                                                            ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                                            : ""
                                                                        ) +
                                                                        (
                                                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                                            ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                                            : ""
                                                                        ) +
                                                                        "m=" + m0.Rel_Mapel.ToString() + "&" +
                                                                        "ms=" + m.Rel_Mapel.ToString();

                                                                list_mapel_sikap_rapor +=
                                                                               (
                                                                                    "<tr>" +
                                                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                                                "<i class=\"fa fa-file-o\"></i>" +
                                                                                            "</td>" +
                                                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                                                "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                                                    "Sikap (Rapor)&nbsp;" +
                                                                                                    "<span style=\"color: #FF0000;\">" +
                                                                                                        m_mapel.Nama +
                                                                                                    "</span>" +
                                                                                                    "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                                        ", " +
                                                                                                        "Semester " +
                                                                                                        m0.Semester +
                                                                                                    "</span>" +
                                                                                                    (
                                                                                                        b_nilai_can_edit
                                                                                                        ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                                                        : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                                                    ) +
                                                                                                "</label>" +
                                                                                            "</td>" +
                                                                                          "</tr>" +
                                                                                          "<tr>" +
                                                                                            "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                                                "<hr style=\"margin: 0px;\" />" +
                                                                                            "</td>" +
                                                                                          "</tr>"
                                                                                );
                                                                //end sikap rapor
                                                                id_sikap++;

                                                                //sikap LTS
                                                                s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.ROUTE +
                                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m0.TahunAjaran) + "&" +
                                                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                        "s=" + m0.Semester + "&" +
                                                                        "kd=" + rel_kelas_det + "&" +
                                                                        (
                                                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                            ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                                            : ""
                                                                        ) +
                                                                        (
                                                                            Libs.GetQueryString("g").Trim() != ""
                                                                            ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                                            : ""
                                                                        ) +
                                                                        (
                                                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                                            ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                                            : ""
                                                                        ) +
                                                                        "m=" + m0.Rel_Mapel.ToString() + "&" +
                                                                        "ms=" + m.Rel_Mapel.ToString();

                                                                list_mapel_sikap_lts +=
                                                                                (
                                                                                    "<tr>" +
                                                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                                                                "<i class=\"fa fa-file-o\"></i>" +
                                                                                            "</td>" +
                                                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                                                                "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                                                    "Sikap (LTS)&nbsp;" +
                                                                                                    "<span style=\"color: #FF0000;\">" +
                                                                                                        m_mapel.Nama +
                                                                                                    "</span>" +
                                                                                                    "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                                        ", " +
                                                                                                        "Semester " +
                                                                                                        m0.Semester +
                                                                                                    "</span>" +
                                                                                                (
                                                                                                    b_nilai_can_edit
                                                                                                    ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                                                    : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                                                ) +
                                                                                                "</label>" +
                                                                                            "</td>" +
                                                                                          "</tr>" +
                                                                                          "<tr>" +
                                                                                            "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                                                "<hr style=\"margin: 0px;\" />" +
                                                                                            "</td>" +
                                                                                          "</tr>"
                                                                                );
                                                                //end sikap LTS
                                                                id_sikap++;
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //end list struktur nilai sikap

                            string id_ui_sikap = "ui_tile_sikap_lts_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
                            html_tile_sikap = 
                                        "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                            "<div data-parent=\"#div_semester_nilai_sikap\" data-target=\"#" + id_ui_sikap + "\" data-toggle=\"tile\" style=\"background-color: " + (id_semester % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                                "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                    "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                        "<div class=\"avatar avatar-sm\">" +
                                                            "<i class=\"fa fa-calendar\"></i>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"tile-inner\">" +
                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + " (LTS)</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-active-show collapse\" id=\"" + id_ui_sikap + "\" style=\"height: 0px;\">" +
                                                "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                        "<hr style=\"margin: 0px; display: none;\" />" +
                                                        "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                            "<tbody>" +
                                                                (
                                                                    id_sikap > 0
                                                                    ? list_mapel_sikap_lts
                                                                    : ("<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                            "..:: Data Kosong ::.." +
                                                                        "</td>" +
                                                                      "</tr>" +
                                                                      "<tr style=\"display: none;\">" +
                                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                            "<hr style=\"margin: 0px;\" />" +
                                                                        "</td>" +
                                                                      "</tr>")
                                                                ) +
                                                            "</tbody>" +
                                                        "</table>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                         "</div>";
                            if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2020) html_tile_sikap = "";

                            id_ui_sikap = "ui_tile_sikap_rapor_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
                            html_tile_sikap +=
                                        "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                            "<div data-parent=\"#div_semester_nilai_sikap\" data-target=\"#" + id_ui_sikap + "\" data-toggle=\"tile\" style=\"background-color: " + (id_semester % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                                "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                    "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                        "<div class=\"avatar avatar-sm\">" +
                                                            "<i class=\"fa fa-calendar\"></i>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"tile-inner\">" +
                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + " (Rapor)</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-active-show collapse\" id=\"" + id_ui_sikap + "\" style=\"height: 0px;\">" +
                                                "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                        "<hr style=\"margin: 0px; display: none;\" />" +
                                                        "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                            "<tbody>" +
                                                                (
                                                                    id_sikap > 0
                                                                    ? list_mapel_sikap_rapor
                                                                    : ("<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                            "..:: Data Kosong ::.." +
                                                                        "</td>" +
                                                                      "</tr>" +
                                                                      "<tr style=\"display: none;\">" +
                                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                            "<hr style=\"margin: 0px;\" />" +
                                                                        "</td>" +
                                                                      "</tr>")
                                                                ) +
                                                            "</tbody>" +
                                                        "</table>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                         "</div>";
                            s_html_nilai_sikap += html_tile_sikap;

                            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                            {
                                s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PILIH_EKSKUL.ROUTE +
                                        "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                        "s=" + semester + "&" +
                                        "kd=" + rel_kelas_det + "&" +
                                        (
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                            ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                            : ""
                                        ) +
                                        (
                                            Libs.GetQueryString("g").Trim() != ""
                                            ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                            : ""
                                        ) +
                                        (
                                            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                            ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                            : ""
                                        ) +
                                        Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;
                                list_mapel_ekskul +=
                                                (
                                                    "<tr>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                            "<i class=\"fa fa-file-o\"></i>" +
                                                        "</td>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                "Pilih Ekstrakurikuler" +
                                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    ", " +
                                                                    "Semester " + semester +
                                                                "</span>" +
                                                                //(
                                                                //    b_nilai_can_edit
                                                                //    ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                //    : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                //) +
                                                            "</label>" +
                                                        "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                      "</tr>"
                                                );
                            }

                            //list struktur nilai mapel ekskul
                            List<Rapor_StrukturNilai> lst_mapel_ekskul = new List<Rapor_StrukturNilai>();
                            if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                            {
                                lst_mapel_ekskul = DAO_Rapor_StrukturNilai.GetAllEkskulByTAByKelasByGuru_Entity(
                                    tahun_ajaran,
                                    m_kelas.Rel_Kelas.ToString(),
                                    no_induk_guru
                                ).FindAll(m => m.Semester == semester);
                            }
                            else
                            {
                                lst_mapel_ekskul = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                    tahun_ajaran,
                                    m_kelas.Rel_Kelas.ToString(),
                                    no_induk_guru
                                ).FindAll(m => m.Semester == semester);
                            }

                            foreach (Rapor_StrukturNilai m in lst_mapel_ekskul)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());

                                if (m_mapel != null)
                                {
                                    if (m_mapel.Nama != null)
                                    {
                                        b_nilai_can_edit = !IsReadonlyNilai(
                                            m.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, m_kelas.Kode.ToString(), m_mapel.Kode.ToString(), "", "", m.TahunAjaran, m.Semester
                                        );

                                        if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                        {
                                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.ROUTE +
                                                     "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                     "ft=" + Libs.GetQueryString("ft") + "&" +
                                                     "s=" + m.Semester + "&" +
                                                     "kd=" + rel_kelas_det + "&" +
                                                     (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                        : ""
                                                     ) +
                                                     (
                                                        Libs.GetQueryString("g").Trim() != ""
                                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                        : ""
                                                     ) +
                                                     (
                                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                        : ""
                                                     ) +
                                                     "m=" + m.Rel_Mapel.ToString();
                                            list_mapel_ekskul +=
                                                    (
                                                      "<tr>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                            (
                                                                lst_mapel_nilai.Count > 1
                                                                ? (id_ekskul + 1).ToString() +
                                                                  ".&nbsp;"
                                                                : "<i class=\"fa fa-file-o\"></i>"
                                                            ) +
                                                        "</td>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                m_mapel.Nama +
                                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    ", " +
                                                                    "Semester " +
                                                                    m.Semester +
                                                                "</span>" +
                                                                (
                                                                    b_nilai_can_edit
                                                                    ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                    : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                ) +
                                                            "</label>" +
                                                        "</td>" +
                                                      "</tr>" +
                                                      "<tr>" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                      "</tr>"
                                                    );
                                            id_ekskul++;
                                        }

                                    }
                                }
                            }
                            //end list struktur nilai mapel ekskul

                            //list volunteer
                            b_nilai_can_edit = IsReadOnlyByLockedSetting(tahun_ajaran, semester);
                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.VOLUNTEER.ROUTE +
                                                 "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                                 "ft=" + Libs.GetQueryString("ft") + "&" +
                                                 "s=" + semester + "&" +
                                                 "kd=" + rel_kelas_det + "&" +
                                                 (
                                                    Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                    ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                                    : ""
                                                 ) +
                                                 (
                                                    Libs.GetQueryString("g").Trim() != ""
                                                    ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                                    : ""
                                                 ) +
                                                 (
                                                    Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                                    ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                                    : ""
                                                 );
                            list_volunteer +=
                                    "<tr>" +
                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                            "<i class=\"fa fa-file-o\"></i>" +
                                        "</td>" +
                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                "Volunteer" +
                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                    ", " +
                                                    "Semester " +
                                                    semester +
                                                "</span>" +
                                                (
                                                    b_nilai_can_edit
                                                    ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                    : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                ) +
                                            "</label>" +
                                        "</td>" +
                                      "</tr>" +
                                      "<tr>" +
                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                            "<hr style=\"margin: 0px;\" />" +
                                        "</td>" +
                                      "</tr>";
                            id_volunteer++;
                            //end list volunteer

                            //s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_LTS.ROUTE +
                            //        "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                            //        "ft=" + Libs.GetQueryString("ft") + "&" +
                            //        "s=" + semester + "&" +
                            //        "kd=" + rel_kelas_det + "&" +
                            //        (
                            //            Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                            //            ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                            //            : ""
                            //        ) +
                            //        (
                            //            Libs.GetQueryString("g").Trim() != ""
                            //            ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                            //            : ""
                            //        ) +
                            //        (
                            //            Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                            //            ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                            //            : ""
                            //        ) +
                            //        (
                            //            rel_mapel.Trim() != "" && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                            //            ? "m=" + rel_mapel + "&"
                            //            : ""
                            //        ) +
                            //        Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;
                            //if (tahun_ajaran == "2020/2021")
                            //{
                            //    Kelas m_kls_0 = DAO_Kelas.GetByID_Entity(m_kelas.Rel_Kelas.ToString());
                            //    s_url = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.ROUTE +
                            //            "?" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" +
                            //            AI_ERP.Application_Modules.EDUCATION.Penilaian.SD.wf_ListNilaiSiswa.GetJenisDownloadNilaiRapor(
                            //                    tahun_ajaran,
                            //                    semester,
                            //                    m_kls_0.Nama.ToString(),
                            //                    "2"
                            //                ) +
                            //            "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                            //            "s=" + semester + "&" +
                            //            "kd=" + rel_kelas_det + "&" +
                            //            "tr=" + TipeRapor.LTS;

                            //    list_rapor +=
                            //                (
                            //                    "<tr " + (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "" : "") + ">" +
                            //                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                            //                            "<i class=\"fa fa-file-o\"></i>" +
                            //                        "</td>" +
                            //                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                            //                            "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                            //                                "Nilai LTS" +
                            //                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                            //                                    ", " +
                            //                                    "Semester " +
                            //                                    semester +
                            //                                "</span>" +
                            //                            "</label>" +
                            //                        "</td>" +
                            //                    "</tr>" +
                            //                    "<tr>" +
                            //                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                            //                            "<hr style=\"margin: 0px;\" />" +
                            //                        "</td>" +
                            //                    "</tr>"
                            //                );
                            //}
                            //else
                            //{
                            //    list_rapor +=
                            //                (
                            //                    "<tr " + (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "" : "") + ">" +
                            //                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                            //                            "<i class=\"fa fa-file-o\"></i>" +
                            //                        "</td>" +
                            //                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                            //                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                            //                                "Nilai LTS" +
                            //                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                            //                                    ", " +
                            //                                    "Semester " +
                            //                                    semester +
                            //                                "</span>" +
                            //                            "</label>" +
                            //                        "</td>" +
                            //                    "</tr>" +
                            //                    "<tr>" +
                            //                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                            //                            "<hr style=\"margin: 0px;\" />" +
                            //                        "</td>" +
                            //                    "</tr>"
                            //                );
                            //}
                            //id_rapor++;

                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CATATAN_SISWA.ROUTE +
                                    "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                    "s=" + semester + "&" +
                                    "kd=" + rel_kelas_det + "&" +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                        : ""
                                    ) +
                                    (
                                        Libs.GetQueryString("g").Trim() != ""
                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                        : ""
                                    ) +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                        : ""
                                    );

                            list_rapor +=
                                            (
                                                "<tr " + (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "" : " style=\"display: none;\" ") + ">" +
                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                    "</td>" +
                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                        "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                            "Catatan untuk Siswa" +
                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                ", " +
                                                                "Semester " +
                                                                semester +
                                                            "</span>" +
                                                        "</label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr " + (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "" : " style=\"display: none;\" ") + ">" +
                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                "</tr>"
                                            );
                            id_rapor++;

                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIHAT_LEDGER.ROUTE +
                                    "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                    "s=" + semester + "&" +
                                    "kd=" + rel_kelas_det;

                            list_rapor +=
                                            (
                                                "<tr>" +
                                                    "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                    "</td>" +
                                                    "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                        "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                            "Ledger Nilai Rapor" +
                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                ", " +
                                                                "Semester " +
                                                                semester +
                                                            "</span>" +
                                                        "</label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                "</tr>"
                                            );

                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIHAT_LEDGER.ROUTE +
                                    "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                    "s=" + semester + "&" +
                                    "skp=1" + "&" +
                                    "kd=" + rel_kelas_det;

                            list_rapor +=
                                            (
                                                "<tr>" +
                                                    "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                    "</td>" +
                                                    "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                        "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                            "Ledger Nilai Sikap" +
                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                ", " +
                                                                "Semester " +
                                                                semester +
                                                            "</span>" +
                                                        "</label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                "</tr>"
                                            );

                            if (semester == s_semester)
                            {
                                s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIHAT_LEDGER.ROUTE +
                                        "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                        "s=" + semester + "&" +
                                        "kd=" + rel_kelas_det + "&" +
                                        "r=1";

                                list_rapor +=
                                                (
                                                    "<tr>" +
                                                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                            "<i class=\"fa fa-file-o\"></i>" +
                                                        "</td>" +
                                                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                            "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                "Ledger Nilai Rapor (Rata-Rata)" +
                                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    ", " +
                                                                    "Semester " +
                                                                    semester +
                                                                "</span>" +
                                                            "</label>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                    "</tr>"
                                                );
                            }

                            id_rapor++;

                            Kelas m_kls = DAO_Kelas.GetByID_Entity(m_kelas.Rel_Kelas.ToString());
                            //s_url = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.ROUTE +
                            //        "?" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" +
                            //        AI_ERP.Application_Modules.EDUCATION.Penilaian.SD.wf_ListNilaiSiswa.GetJenisDownloadNilaiRapor(
                            //                tahun_ajaran,
                            //                semester,
                            //                m_kls.Nama.ToString(),
                            //                "0"
                            //            ) +
                            //        "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                            //        "s=" + semester + "&" +
                            //        "kd=" + rel_kelas_det + "&" +
                            //        "tr=" + TipeRapor.SEMESTER;

                            //list_rapor +=
                            //                (
                            //                    "<tr>" +
                            //                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                            //                            "<i class=\"fa fa-file-o\"></i>" +
                            //                        "</td>" +
                            //                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                            //                            "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                            //                                "Lihat Nilai Rapor" +
                            //                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                            //                                    ", " +
                            //                                    "Semester " +
                            //                                    semester +
                            //                                "</span>" +
                            //                            "</label>" +
                            //                        "</td>" +
                            //                    "</tr>" +
                            //                    "<tr>" +
                            //                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                            //                            "<hr style=\"margin: 0px;\" />" +
                            //                        "</td>" +
                            //                    "</tr>"
                            //                );
                            //id_rapor++;

                            if (Libs.GetStringToDecimal(tahun_ajaran.Substring(0, 4)) < 2020)
                            {
                                s_url = AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.ROUTE +
                                        "?" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" +
                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.SD.wf_ListNilaiSiswa.GetJenisDownloadNilaiRapor(
                                            tahun_ajaran,
                                            semester,
                                            m_kls.Nama.ToString(),
                                            "1"
                                        ) +
                                        "&t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                        "s=" + semester.ToString() + "&" +
                                        "kd=" + rel_kelas_det;

                                list_rapor +=
                                                (
                                                    "<tr>" +
                                                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                            "<i class=\"fa fa-file-o\"></i>" +
                                                        "</td>" +
                                                        "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                            "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                "Lihat Uraian Rapor" +
                                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    ", " +
                                                                    "Semester " +
                                                                    semester +
                                                                "</span>" +
                                                            "</label>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                    "</tr>"
                                                );
                                id_rapor++;
                            }

                            s_url = Libs.FILE_PAGE_URL +
                                    "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                    "s=" + id_semester.ToString() + "&" +
                                    "kd=" + rel_kelas_det + "&" +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                        : ""
                                    ) +
                                    (
                                        Libs.GetQueryString("g").Trim() != ""
                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                        : ""
                                    ) +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                        : ""
                                    ) +
                                    Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_RAPOR;

                            list_rapor +=
                                            (
                                                "<tr style=\"display: none;\">" +
                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                    "</td>" +
                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                        "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                            "Nilai Rapor Semester" +
                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                ", " +
                                                                "Semester " +
                                                                id_semester.ToString() +
                                                            "</span>" +
                                                        "</label>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr style=\"display: none;\">" +
                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                "</tr>"
                                            );
                        }

                        s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.ROUTE +
                                    "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                    "s=" + semester + "&" +
                                    "kd=" + rel_kelas_det + "&" +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                        ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                                        : ""
                                    ) +
                                    (
                                        Libs.GetQueryString("g").Trim() != ""
                                        ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                                        : ""
                                    ) +
                                    (
                                        Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                        ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                                        : ""
                                    ) +
                                    (
                                        rel_mapel.Trim() != "" && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                                        ? "m=" + rel_mapel + "&"
                                        : ""
                                    ) +
                                    Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;
                        list_rapor +=
                                        (
                                            "<tr " + (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "" : "") + ">" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                    "<i class=\"fa fa-file-o\"></i>" +
                                                "</td>" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                    "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                        "Lihat Nilai LTS & Rapor" +
                                                        "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                            ", " +
                                                            "Semester " +
                                                            semester +
                                                        "</span>" +
                                                    "</label>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px;\" />" +
                                                "</td>" +
                                            "</tr>"
                                        );
                        id_rapor++;

                        //list nilai ekskul
                        string id_ui_ekskul = "ui_tile_ekskul_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester;
                        html_tile_ekskul =
                                    "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                        "<div data-parent=\"#div_semester_nilai_ekskul\" data-target=\"#" + id_ui_ekskul + "\" data-toggle=\"tile\" style=\"background-color: " + (1 % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                            "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"avatar avatar-sm\">" +
                                                        "<i class=\"fa fa-calendar\"></i>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-inner\">" +
                                                "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"tile-active-show collapse\" id=\"" + id_ui_ekskul + "\" style=\"height: 0px;\">" +
                                            "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                    "<hr style=\"margin: 0px; display: none;\" />" +
                                                    "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                        "<tbody>" +
                                                            list_mapel_ekskul +
                                                            (
                                                                id_ekskul > 0
                                                                ? ""
                                                                : ("<tr>" +
                                                                    "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                        "..:: Data Kosong ::.." +
                                                                    "</td>" +
                                                                  "</tr>" +
                                                                  "<tr style=\"display: none;\">" +
                                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px;\" />" +
                                                                    "</td>" +
                                                                  "</tr>")
                                                            ) +
                                                        "</tbody>" +
                                                    "</table>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                     "</div>";
                        s_html_nilai_ekskul += html_tile_ekskul;
                        //end list nilai ekskul

                        //list nilai volunteer
                        string id_ui_volunteer = "ui_tile_volunteer_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester;
                        html_tile_volunteer =
                                    "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                        "<div data-parent=\"#div_semester_nilai_volunteer\" data-target=\"#" + id_ui_volunteer + "\" data-toggle=\"tile\" style=\"background-color: " + (1 % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                            "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"avatar avatar-sm\">" +
                                                        "<i class=\"fa fa-calendar\"></i>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-inner\">" +
                                                "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"tile-active-show collapse\" id=\"" + id_ui_volunteer + "\" style=\"height: 0px;\">" +
                                            "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                    "<hr style=\"margin: 0px; display: none;\" />" +
                                                    "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                        "<tbody>" +
                                                            (
                                                                id_volunteer > 0
                                                                ? list_volunteer
                                                                : ("<tr>" +
                                                                    "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                        "..:: Data Kosong ::.." +
                                                                    "</td>" +
                                                                  "</tr>" +
                                                                  "<tr style=\"display: none;\">" +
                                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px;\" />" +
                                                                    "</td>" +
                                                                  "</tr>")
                                                            ) +
                                                        "</tbody>" +
                                                    "</table>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                     "</div>";
                        s_html_nilai_volunteer += html_tile_volunteer;
                        //end list nilai volunteer

                        string id_ui_rapor = "ui_tile_rapor_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester;
                        html_tile_rapor =
                                    "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                        "<div data-parent=\"#div_semester_nilai_rapor\" data-target=\"#" + id_ui_rapor + "\" data-toggle=\"tile\" style=\"background-color: " + (1 % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                            "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"avatar avatar-sm\">" +
                                                        "<i class=\"fa fa-calendar\"></i>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-inner\">" +
                                                "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"tile-active-show collapse\" id=\"" + id_ui_rapor + "\" style=\"height: 0px;\">" +
                                            "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                    "<hr style=\"margin: 0px; display: none;\" />" +
                                                    "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                        "<tbody>" +
                                                            (
                                                                id_rapor > 0
                                                                ? list_rapor
                                                                : ("<tr>" +
                                                                    "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                        "..:: Data Kosong ::.." +
                                                                    "</td>" +
                                                                  "</tr>" +
                                                                  "<tr style=\"display: none;\">" +
                                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                        "<hr style=\"margin: 0px;\" />" +
                                                                    "</td>" +
                                                                  "</tr>")
                                                            ) +
                                                        "</tbody>" +
                                                    "</table>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                     "</div>";
                        s_html_nilai_rapor += html_tile_rapor;

                        id_semester++;
                    }
                }
            }

            ltrListNilaiAkademik.Text = "<div id=\"div_semester_nilai_akademik\" class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            s_html_nilai_akademik +
                                        "</div>";

            ltrListNilaiSikap.Text = "<div id=\"div_semester_nilai_sikap\" class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            s_html_nilai_sikap +
                                        "</div>";

            ltrListNilaiEkskul.Text = "<div id=\"div_semester_nilai_ekskul\" class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            s_html_nilai_ekskul +
                                        "</div>";

            ltrListNilaiVolunteer.Text = "<div id=\"div_semester_nilai_volunteer\" class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            s_html_nilai_volunteer +
                                        "</div>";

            ltrListNilaiRapor.Text = "<div id=\"div_semester_nilai_rapor\" class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            s_html_nilai_rapor +
                                        "</div>";
        }

        public static void InitModalListNilaiEkskul(
                System.Web.UI.Page page,
                System.Web.UI.WebControls.Literal ltrListNilaiEkskul,
                string tahun_ajaran, string semester, string rel_mapel, List<string> lst_kelas, string no_induk_guru
            )
        {
            string s_html_nilai_ekskul = "";
            string list_mapel = "";
            string s_url = "";
            string s_url_kelas = "";
            
            foreach (string s_kelas in lst_kelas)
            {
                s_url_kelas += s_kelas + ";";
            }

            List<DAO_Rapor_StrukturNilai.TahunAjaranSemester> lst_mapel_nilai_sn = 
                DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran).OrderByDescending(m => m.TahunAjaran).ThenByDescending(m => m.Semester).ToList();

            bool show_all = (semester.Trim() == "" ? true : false);
            semester = (semester == "" && semester.Trim() == "" ? Libs.GetSemesterByTanggal(DateTime.Now).ToString() : semester);
            if (lst_mapel_nilai_sn.Count > 0 && semester.Trim() == "")
            {
                semester = lst_mapel_nilai_sn.FirstOrDefault().Semester;
            }

            int id = 1;
            bool b_nilai_can_edit = true;

            Mapel mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (mapel != null)
            {
                if (mapel.Nama != null)
                {
                    if (show_all)
                    {
                        for (int i = Libs.GetStringToInteger(semester); i > 0; i--)
                        {
                            List<Rapor_StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                                    tahun_ajaran, i.ToString(), lst_kelas, rel_mapel
                                );
                            list_mapel = "";

                            if (lst_stuktur_nilai.Count == 1)
                            {
                                Rapor_StrukturNilai m_sn = lst_stuktur_nilai.FirstOrDefault();
                                //list nilai ekskul
                                id = 1;
                                b_nilai_can_edit = false;
                                foreach (string kelas_level in lst_kelas)
                                {
                                    foreach (var kelas_det in DAO_KelasDet.GetByKelas_Entity(kelas_level))
                                    {
                                        if (!IsReadonlyNilai(
                                            m_sn.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, kelas_det.Kode.ToString(), rel_mapel.ToString(), "", "", tahun_ajaran, i.ToString()

                                        ))
                                            {
                                                b_nilai_can_edit = true;
                                                break;
                                            }
                                    }                                    
                                }
                                s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.ROUTE +
                                            "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                            "ft=" + Libs.GetQueryString("ft") + "&" +
                                            "s=" + i.ToString() + "&" +
                                            "m=" + rel_mapel +
                                            (AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "&" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS : "") +
                                            (AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit() ? "&" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT : "") +
                                            "&k=" + s_url_kelas;

                                list_mapel += ("<tr>" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                    "</td>" +
                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                        "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                            mapel.Nama +
                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                ", " +
                                                                "Semester " + i.ToString() +
                                                            "</span>" +
                                                            (
                                                                b_nilai_can_edit
                                                                ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                            ) +
                                                        "</label>" +
                                                    "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                   "</tr>"
                                              );

                                string id_ui = "ui_tile_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + i.ToString();
                                string html_tile =
                                            "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                                "<div data-parent=\"#div_semester_nilai_ekskul\" data-target=\"#" + id_ui + "\" data-toggle=\"tile\" style=\"background-color: " + (id % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                                    "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                        "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                            "<div class=\"avatar avatar-sm\">" +
                                                                "<i class=\"fa fa-calendar\"></i>" +
                                                            "</div>" +
                                                        "</div>" +
                                                    "</div>" +
                                                    "<div class=\"tile-inner\">" +
                                                        "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + i.ToString() + "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"tile-active-show collapse\" id=\"" + id_ui + "\" style=\"height: 0px;\">" +
                                                    "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                        "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                            "<hr style=\"margin: 0px; display: none;\" />" +
                                                            "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                                "<tbody>" +
                                                                    (
                                                                        id > 0
                                                                        ? list_mapel
                                                                        : ("<tr>" +
                                                                            "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                                "..:: Data Kosong ::.." +
                                                                            "</td>" +
                                                                          "</tr>" +
                                                                          "<tr style=\"display: none;\">" +
                                                                            "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                                "<hr style=\"margin: 0px;\" />" +
                                                                            "</td>" +
                                                                          "</tr>")
                                                                    ) +
                                                                "</tbody>" +
                                                            "</table>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                             "</div>";
                                s_html_nilai_ekskul += html_tile;
                                id++;
                            }
                        }
                    }
                    else
                    {
                        List<Rapor_StrukturNilai> lst_stuktur_nilai = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                                    tahun_ajaran, semester, lst_kelas, rel_mapel
                                );

                        if (lst_stuktur_nilai.Count == 1)
                        {
                            Rapor_StrukturNilai m_sn = lst_stuktur_nilai.FirstOrDefault();
                            //list nilai ekskul
                            id = 1;
                            b_nilai_can_edit = false;
                            foreach (string kelas_level in lst_kelas)
                            {
                                foreach (var kelas_det in DAO_KelasDet.GetByKelas_Entity(kelas_level))
                                {
                                    if (!IsReadonlyNilai(
                                        m_sn.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, kelas_det.Kode.ToString(), rel_mapel.ToString(), "", "", tahun_ajaran, semester

                                    ))
                                    {
                                        b_nilai_can_edit = true;
                                        break;
                                    }
                                }
                            }
                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.ROUTE +
                                        "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                        "s=" + semester.ToString() + "&" +
                                        "m=" + rel_mapel +
                                        (AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() ? "&" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS : "") +
                                        (AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit() ? "&" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT : "") +
                                        "&k=" + s_url_kelas;

                            list_mapel += ("<tr>" +
                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                    "<i class=\"fa fa-file-o\"></i>" +
                                                "</td>" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                    "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                        mapel.Nama +
                                                        "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                            ", " +
                                                            "Semester " + semester.ToString() +
                                                        "</span>" +
                                                        (
                                                            b_nilai_can_edit
                                                            ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                            : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                        ) +
                                                    "</label>" +
                                                "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px;\" />" +
                                                "</td>" +
                                               "</tr>"
                                          );

                            string id_ui = "ui_tile_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
                            string html_tile =
                                        "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                            "<div data-parent=\"#div_semester_nilai_ekskul\" data-target=\"#" + id_ui + "\" data-toggle=\"tile\" style=\"background-color: " + (id % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
                                                "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 50px; margin-left: 0px;\">" +
                                                    "<div class=\"col-xs-12\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                        "<div class=\"avatar avatar-sm\">" +
                                                            "<i class=\"fa fa-calendar\"></i>" +
                                                        "</div>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class=\"tile-inner\">" +
                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
                                                "</div>" +
                                            "</div>" +
                                            "<div class=\"tile-active-show collapse\" id=\"" + id_ui + "\" style=\"height: 0px;\">" +
                                                "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                    "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                                        "<hr style=\"margin: 0px; display: none;\" />" +
                                                        "<table class=\"table\" style=\"width: 100%; margin: 0px;\">" +
                                                            "<tbody>" +
                                                                (
                                                                    id > 0
                                                                    ? list_mapel
                                                                    : ("<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
                                                                            "..:: Data Kosong ::.." +
                                                                        "</td>" +
                                                                      "</tr>" +
                                                                      "<tr style=\"display: none;\">" +
                                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                                            "<hr style=\"margin: 0px;\" />" +
                                                                        "</td>" +
                                                                      "</tr>")
                                                                ) +
                                                            "</tbody>" +
                                                        "</table>" +
                                                    "</div>" +
                                                "</div>" +
                                            "</div>" +
                                         "</div>";
                            s_html_nilai_ekskul += html_tile;
                            id++;
                        }

                    }

                    ltrListNilaiEkskul.Text = "<div id=\"div_semester_nilai_ekskul\" class=\"tile-wrap\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                                s_html_nilai_ekskul +
                                            "</div>";
                }
            }
            //end list nilai ekskul
        }
    }
}