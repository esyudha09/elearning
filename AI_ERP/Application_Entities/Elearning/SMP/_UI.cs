using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Entities.Elearning.SMP
{
    public static class _UI
    {
        public static bool IsReadonlyNilai(
                string kode_struktur_nilai, string nik_guru, string rel_kelas_det,
                string rel_mapel, string tahun_ajaran, string semester
            )
        {
            bool is_readonly = false;
            if (DAO_Rapor_Arsip.GetAll_Entity().Count > 0)
            {
                Rapor_Arsip m_arsip = DAO_Rapor_Arsip.GetAll_Entity().OrderByDescending(m => m.TanggalClosing).FirstOrDefault();
                if (m_arsip != null)
                {
                    if (m_arsip.TahunAjaran != null)
                    {
                        if (m_arsip.TahunAjaran == tahun_ajaran && m_arsip.Semester == semester)
                        {
                            if (m_arsip.TanggalClosing < DateTime.Now)
                            {
                                is_readonly = true;
                            }
                        }
                        else
                        {
                            is_readonly = true;
                        }
                    }
                }
            }
            if (!is_readonly)
            {
                is_readonly = !DAO_Rapor_StrukturNilai.IsNilaiCanEdit(kode_struktur_nilai, nik_guru, rel_kelas_det);
            }
            if (DAO_IsiNilai_Log.GetBisaEdit_Entity(nik_guru, rel_mapel, rel_kelas_det, tahun_ajaran, semester).Count > 0)
            {
                is_readonly = false;
            }

            return is_readonly;
        }

        public static void InitModalListNilai(
                System.Web.UI.Page page,
                System.Web.UI.WebControls.Literal ltrListNilaiAkademik,
                System.Web.UI.WebControls.Literal ltrListNilaiSikap,
                System.Web.UI.WebControls.Literal ltrListNilaiEkskul, 
                System.Web.UI.WebControls.Literal ltrListNilaiRapor,
                string tahun_ajaran, string rel_mapel, string kelas_det, string no_induk_guru
            )
        {
            string s_html_nilai_akademik = "";
            string s_html_nilai_ekskul = "";
            string s_html_nilai_sikap = "";
            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(kelas_det);

            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    //list struktur nilai mapel non sikap
                    List<Rapor_StrukturNilai> lst_mapel_nilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                            tahun_ajaran,
                            m_kelas.Rel_Kelas.ToString(),
                            no_induk_guru
                        ).OrderByDescending(m => m.Semester).ToList();

                    string html_tile = "";
                    string html_tile_ekskul = "";
                    string html_tile_sikap = "";
                    List<string> lst_semester = lst_mapel_nilai.Select(m => m.Semester).Distinct().ToList();
                    int id_semester = 1;
                    foreach (var semester in lst_semester)
                    {
                        int id_akademik = 0;
                        int id_ekskul = 0;
                        int id_sikap = 0;
                        string list_mapel = "";
                        string list_mapel_ekskul = "";
                        string list_mapel_sikap = "";

                        //jika kelas yg dibuka adalah kelas guru mapel sedangkan dia juga sbg walikelas kelas tersebut
                        if (!Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel.Trim() != "")
                        {
                            lst_mapel_nilai = lst_mapel_nilai.FindAll(m => m.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper());
                        }

                        foreach (Rapor_StrukturNilai m in lst_mapel_nilai.FindAll(m => m.Semester == semester))
                        {
                            var lst_sn_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(m.Kode.ToString());
                            bool allow_mapel = false;
                            bool b_nilai_can_edit = !IsReadonlyNilai(
                                m.Kode.ToString(), no_induk_guru, m_kelas.Kode.ToString(), m.Rel_Mapel.ToString(), m.TahunAjaran, m.Semester
                            );

                            List<FormasiGuruKelas_ByGuru> lst_kelasguru = 
                                DAO_FormasiGuruKelas.GetByGuruByTA_Entity(no_induk_guru, tahun_ajaran).FindAll(m0 => m0.Semester == semester).ToList();
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());

                            if (lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == kelas_det.Trim().ToUpper() && m0.KodeMapel == "").Count > 0 ||
                                lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == kelas_det.Trim().ToUpper() && m0.KodeMapel.ToString().ToUpper() == m.Rel_Mapel.ToString().ToUpper()).Count > 0)
                            {
                                allow_mapel = true;
                            }
                            if (lst_sn_ap.Count == 0) allow_mapel = false;

                            if (m_mapel != null && allow_mapel)
                            {
                                if (m_mapel.Nama != null)
                                {

                                    if (m_mapel.Jenis != Libs.JENIS_MAPEL.SIKAP && m_mapel.Jenis != Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                    {
                                        string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE +
                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                                        "s=" + m.Semester + "&" +
                                                        "kd=" + kelas_det + "&" +
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
                                        list_mapel += ("<tr class=" + (id_akademik % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf;\">" +
                                                            (id_akademik + 1).ToString() +
                                                        "</td>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; font-weight: normal; color: grey;\">" +
                                                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                m_mapel.Nama +
                                                                "<br />" +
                                                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    "Semester " +
                                                                    m.Semester +
                                                                "</span>" +
                                                                ",&nbsp;" +
                                                                "<span style=\"font-weight: normal; font-size: small; color: #bfbfbf; font-weight: bold; text-transform: none; text-decoration: none; font-style: italic;\">" +
                                                                    DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) +
                                                                "</span>" +
                                                                (
                                                                    b_nilai_can_edit
                                                                    ? "&nbsp;<span style=\"float: right; font-weight: normal; color: green; font-size: small;\" title=\" Data nilai bisa dilihat & diisi/diubah \"><i class=\"fa fa-unlock\"></i></span>"
                                                                    : "&nbsp;<span style=\"float: right; font-weight: normal; color: orange; font-size: small;\" title=\" Data nilai hanya bisa Dilihat \"><i class=\"fa fa-lock\"></i></span>"
                                                                ) +
                                                            "</label>" +
                                                        "</td>" +
                                                      "</tr>" +
                                                      "<tr style=\"display: none;\">" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                      "</tr>");
                                        id_akademik++;
                                    }
                                    else if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                    {
                                        string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE +
                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                                        "s=" + m.Semester + "&" +
                                                        "kd=" + kelas_det + "&" +
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
                                                    ("<tr class=" + (id_ekskul % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf;\">" +
                                                            (id_ekskul + 1).ToString() +
                                                        "</td>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; font-weight: normal; color: grey;\">" +
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
                                                      "<tr style=\"display: none;\">" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                      "</tr>");
                                        id_ekskul++;
                                    }
                                    if (DAO_Rapor_StrukturNilai.GetAllMapelSikapByTAByKelas_Entity(
                                                        tahun_ajaran, semester, m_kelas.Rel_Kelas.ToString()
                                                    ).Count > 0)
                                    {
                                        string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_SIKAP.ROUTE +
                                                        "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                        "ft=" + Libs.GetQueryString("ft") + "&" +
                                                        "s=" + m.Semester + "&" +
                                                        "kd=" + m_kelas.Kode.ToString().ToString() + "&" +
                                                        "kdgk=" + kelas_det + "&" +
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
                                        list_mapel_sikap +=
                                                    ("<tr class=" + (id_sikap % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-size: x-small;\">" +
                                                            (id_sikap + 1).ToString() +
                                                        "</td>" +
                                                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; font-weight: normal; color: grey;\">" +
                                                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                                "<span style=\"font-weight: normal;\">Sikap</span> " +
                                                                m_mapel.Nama +
                                                                "<br />" +
                                                                "<span style=\"font-weight: normal; font-size: small; color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
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
                                                      "<tr style=\"display: none;\">" +
                                                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px;\" />" +
                                                        "</td>" +
                                                      "</tr>");
                                        id_sikap++;
                                    }
                                }
                            }
                        }

                        string id_ui = "ui_tile_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
                        html_tile  = "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
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
                                                                    "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
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
                        string id_ui_ekskul = "ui_tile_ekskul_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
                        html_tile_ekskul =
                                    "<div class=\"tile tile-collapse\" style=\"box-shadow: none;\">" +
                                        "<div data-parent=\"#div_semester_nilai_ekskul\" data-target=\"#" + id_ui_ekskul + "\" data-toggle=\"tile\" style=\"background-color: " + (id_semester % 2 == 0 ? "#ffffff" : "#fafafa") + "; margin-bottom: 0px;\">" +
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
                                                            (
                                                                id_ekskul > 0
                                                                ? list_mapel_ekskul
                                                                : ("<tr>" +
                                                                    "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold;\">" +
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

                        string id_ui_sikap = "ui_tile_sikap_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();

                        if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                        {
                            //nilai sikap wali kelas
                            string s_url_sikap_walas = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_SIKAP.ROUTE +
                                                       "?t=" + RandomLibs.GetRndTahunAjaran(tahun_ajaran) + "&" +
                                                       "ft=" + Libs.GetQueryString("ft") + "&" +
                                                       "s=" + semester + "&" +
                                                       "kd=" + kelas_det + "&" +
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
                            bool b_nilai_can_edit_sikap_by_walas = true;
                            list_mapel_sikap =
                                            ("<tr class=\"odddrow\">" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url_sikap_walas) + "');\" style=\"cursor: pointer; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-size: x-small;\">" +
                                                    "&nbsp;" +
                                                "</td>" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url_sikap_walas) + "');\" style=\"cursor: pointer; padding: 5px; font-weight: normal; color: grey;\">" +
                                                    "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url_sikap_walas) + "');\" style=\"cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                        "Nilai Sikap Dari Wali Kelas" +
                                                        "<br />" +
                                                        "<span style=\"font-weight: normal; font-size: small; color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                            "Semester " +
                                                            semester +
                                                        "</span>" +
                                                        (
                                                            b_nilai_can_edit_sikap_by_walas
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
                                            "</tr>") +
                                            list_mapel_sikap;
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
                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
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
                                                                    ? list_mapel_sikap
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
                        }
                        else
                        {
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
                                                "<div class=\"text-overflow\" style=\"font-weight: bold;\">Semester " + semester + "</div>" +
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
                                                                ? list_mapel_sikap
                                                                : ("<tr>" +
                                                                    "<td colspan=\"2\" style=\"background-color: white; text-align: center; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-weight: bold; font-size: x-small;\">" +
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
                        }

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

            string s_html_nilai_rapor = "";
            m_kelas = DAO_KelasDet.GetByID_Entity(kelas_det);
            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    List<DAO_Rapor_StrukturNilai.TahunAjaranSemester> lst_struktur_nilai = DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranSemester_Entity(
                            tahun_ajaran,
                            m_kelas.Rel_Kelas.ToString()
                        );

                    int id = 0;
                    foreach (var m_struktur in lst_struktur_nilai)
                    {
                        //if (m_struktur.TahunAjaran == "2020/2021")
                        //{
                        //    string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.ROUTE +
                        //               "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                        //               "ft=" + Libs.GetQueryString("ft") + "&" +
                        //               "s=" + m_struktur.Semester + "&" +
                        //               "kd=" + kelas_det + "&" +
                        //               "tr=" + TipeRapor.LTS + "&" +
                        //               AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_LTS_SMP + "&" +
                        //               (
                        //                    Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                        //                    ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                        //                    : ""
                        //               ) +
                        //               (
                        //                    Libs.GetQueryString("g").Trim() != ""
                        //                    ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                        //                    : ""
                        //               ) +
                        //               (
                        //                    Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                        //                    ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                        //                    : ""
                        //               ) +
                        //               (
                        //                    Libs.GetQueryString("m").Trim() != ""
                        //                    ? "m=" + Libs.GetQueryString("m").Trim() + "&"
                        //                    : ""
                        //               ) +
                        //               Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;

                        //    s_html_nilai_rapor += "<tr class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                        //                            "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                        //                                "<i class=\"fa fa-file-o\"></i>" +
                        //                            "</td>" +
                        //                            "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                        //                                "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                        //                                    "Nilai LTS" +
                        //                                    "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                        //                                        ", " +
                        //                                        "Semester " +
                        //                                        m_struktur.Semester +
                        //                                    "</span>" +
                        //                                "</label>" +
                        //                            "</td>" +
                        //                          "</tr>" +
                        //                          "<tr style=\"display: none;\">" +
                        //                            "<td colspan=\"2\" style=\"padding: 0px;\">" +
                        //                                "<hr style=\"margin: 0px;\" />" +
                        //                            "</td>" +
                        //                          "</tr>";
                        //    id++;
                        //}
                        //else
                        //{
                        //string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.ROUTE +
                        //           "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                        //           "ft=" + Libs.GetQueryString("ft") + "&" +
                        //           "s=" + m_struktur.Semester + "&" +
                        //           "kd=" + kelas_det + "&" +
                        //           "tr=" + TipeRapor.LTS + "&" +
                        //           (
                        //                Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                        //                ? Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT + "&"
                        //                : ""
                        //           ) +
                        //           (
                        //                Libs.GetQueryString("g").Trim() != ""
                        //                ? "g=" + Libs.GetQueryString("g").Trim() + "&"
                        //                : ""
                        //           ) +
                        //           (
                        //                Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas()
                        //                ? Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS + "&"
                        //                : ""
                        //           ) +
                        //           (
                        //                Libs.GetQueryString("m").Trim() != ""
                        //                ? "m=" + Libs.GetQueryString("m").Trim() + "&"
                        //                : ""
                        //           ) +
                        //           Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;

                        //s_html_nilai_rapor += "<tr class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                        //                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                        //                            "<i class=\"fa fa-file-o\"></i>" +
                        //                        "</td>" +
                        //                        "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                        //                            "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                        //                                "Lihat Nilai LTS & Rapor" +
                        //                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                        //                                    ", " +
                        //                                    "Semester " +
                        //                                    m_struktur.Semester +
                        //                                "</span>" +
                        //                            "</label>" +
                        //                        "</td>" +
                        //                      "</tr>" +
                        //                      "<tr style=\"display: none;\">" +
                        //                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                        //                            "<hr style=\"margin: 0px;\" />" +
                        //                        "</td>" +
                        //                      "</tr>";
                        //id++;
                        //}

                        if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                        {
                            string s_js_ledger = "window.open( " +
                                                 "'" +
                                                    page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIHAT_LEDGER.ROUTE) +
                                                        "?t=" + AI_ERP.Application_Libs.RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) +
                                                        "&kd=" + m_kelas.Kode.ToString() +
                                                        "&tr=" + TipeRapor.SEMESTER +
                                                 "' + '&s=' + '" + m_struktur.Semester + "', '_blank')";

                            s_html_nilai_rapor += "<tr style=\"\" class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                    "<td onclick=\"" + s_js_ledger + "\" style=\"cursor: pointer; padding: 10px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                        "<i class=\"fa fa-file-o\"></i>" +
                                                    "</td>" +
                                                    "<td onclick=\"" + s_js_ledger + "\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                        "<label onclick=\"" + s_js_ledger + "\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                            "Ledger Nilai Rapor" +
                                                            "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                ", " +
                                                                "Semester " +
                                                                m_struktur.Semester +
                                                            "</span>" +
                                                        "</label>" +
                                                    "</td>" +
                                                  "</tr>" +
                                                  "<tr style=\"\">" +
                                                    "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                        "<hr style=\"margin: 0px;\" />" +
                                                    "</td>" +
                                                  "</tr>";
                            id++;
                        }

                        //if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                        //{
                        //    List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        //        DAO_Kelas.GetByID_Entity(m_kelas.Rel_Kelas.ToString()).Rel_Sekolah.ToString(),
                        //        m_kelas.Kode.ToString(),
                        //        m_struktur.TahunAjaran,
                        //        m_struktur.Semester
                        //    );
                        //    string s_siswa = "";
                        //    string s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                        //    Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m0 => m0.TahunAjaran == m_struktur.TahunAjaran && m0.Semester == m_struktur.Semester).FirstOrDefault();
                        //    string s_kelas = DAO_Kelas.GetByID_Entity(m_kelas.Rel_Kelas.ToString()).Nama + "-";
                        //    foreach (var item_siswa in lst_siswa)
                        //    {
                        //        s_siswa += item_siswa.Kode.ToString() + ";";
                        //    }
                        //    if (m != null)
                        //    {
                        //        if (s_kelas.Length >= 4)
                        //        {
                        //            if (s_kelas.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KURTILAS)
                        //            {
                        //                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                        //            }
                        //            else if (s_kelas.Substring(0, 4) == "VII-" && m.KurikulumRaporLevel7 == Libs.JenisKurikulum.SMP.KTSP)
                        //            {
                        //                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                        //            }
                        //        }
                        //        if (s_kelas.Length >= 5)
                        //        {
                        //            if (s_kelas.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KURTILAS)
                        //            {
                        //                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                        //            }
                        //            else if (s_kelas.Substring(0, 5) == "VIII-" && m.KurikulumRaporLevel8 == Libs.JenisKurikulum.SMP.KTSP)
                        //            {
                        //                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                        //            }
                        //        }
                        //        if (s_kelas.Length >= 3)
                        //        {
                        //            if (s_kelas.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KURTILAS)
                        //            {
                        //                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP;
                        //            }
                        //            else if (s_kelas.Substring(0, 3) == "IX-" && m.KurikulumRaporLevel9 == Libs.JenisKurikulum.SMP.KTSP)
                        //            {
                        //                s_jenis_download_key = AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP;
                        //            }
                        //        }
                        //    }

                        //    string s_js_show_rapor =
                        //        "window.open(" +
                        //        "'" + page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.ROUTE) + "?'" +
                        //        " + '" + AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" + s_jenis_download_key + "'" +
                        //        " + '&t=' + '" + m_struktur.TahunAjaran.Replace("/", "-") + "'" +
                        //        " + '&kd=' + '" + m_kelas.Kode.ToString() + "'" +
                        //        " + '&hal=1' + '&s=' + '" + m_struktur.Semester + "' + '&sw=' + '" + s_siswa + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        //    "return false; ";

                        //    s_html_nilai_rapor += "<tr style=\"\" class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                        //                            "<td onclick=\"" + s_js_show_rapor + "\" style=\"cursor: pointer; padding: 10px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                        //                                "<i class=\"fa fa-file-o\"></i>" +
                        //                            "</td>" +
                        //                            "<td onclick=\"" + s_js_show_rapor + "\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                        //                                "<label style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                        //                                    "Lihat Nilai LTS & Rapor" +
                        //                                    "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                        //                                        ", " +
                        //                                        "Semester " +
                        //                                        m_struktur.Semester +
                        //                                    "</span>" +
                        //                                "</label>" +
                        //                            "</td>" +
                        //                          "</tr>" +
                        //                          "<tr style=\"\">" +
                        //                            "<td colspan=\"2\" style=\"padding: 0px;\">" +
                        //                                "<hr style=\"margin: 0px;\" />" +
                        //                            "</td>" +
                        //                          "</tr>";
                        //    id++;
                        //}

                        string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.ROUTE +
                                       "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                                       "ft=" + Libs.GetQueryString("ft") + "&" +
                                       "s=" + m_struktur.Semester + "&" +
                                       "kd=" + kelas_det + "&" +
                                       "tr=" + TipeRapor.LTS + "&" +
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
                                            Libs.GetQueryString("m").Trim() != ""
                                            ? "m=" + Libs.GetQueryString("m").Trim() + "&"
                                            : ""
                                       ) +
                                       Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;

                        s_html_nilai_rapor += "<tr class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 60px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                    "<i class=\"fa fa-file-o\"></i>" +
                                                "</td>" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                    "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                        "Lihat Nilai LTS & Rapor" +
                                                        "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                            ", " +
                                                            "Semester " +
                                                            m_struktur.Semester +
                                                        "</span>" +
                                                    "</label>" +
                                                "</td>" +
                                              "</tr>" +
                                              "<tr style=\"display: none;\">" +
                                                "<td colspan=\"2\" style=\"padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px;\" />" +
                                                "</td>" +
                                              "</tr>";
                        id++;

                    }

                }
            }

            ltrListNilaiRapor.Text = (
                                        s_html_nilai_rapor.Trim() != ""
                                        ? "<div class=\"table-responsive\" style=\"margin: 0px; box-shadow: none;\">" +
                                            "<hr style=\"margin: 0px; display: none;\" />" +
                                            "<table class=\"table\" style=\"width: 100%; margin: 0px;\">"
                                        : ""
                                     ) +
                                     "<tbody>" +
                                        s_html_nilai_rapor +
                                     "</tbody>" +
                                     (
                                        s_html_nilai_rapor.Trim() != ""
                                        ? "</table>" +
                                          "</div>"
                                        : ""
                                     );
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
                if (s_kelas.Trim() != "")
                {
                    s_url_kelas += s_kelas + ";";
                }
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

            string s_rel_kelas_1 = "";
            string s_rel_kelas_2 = "";
            string s_rel_kelas_3 = "";
            int id_kelas = 1;
            foreach (string rel_kelas in lst_kelas)
            {
                if (id_kelas == 1)
                {
                    s_rel_kelas_1 = rel_kelas;
                }
                else if (id_kelas == 2)
                {
                    s_rel_kelas_2 = rel_kelas;
                }
                else if (id_kelas == 3)
                {
                    s_rel_kelas_3 = rel_kelas;
                }
                id_kelas++;
            }

            Mapel mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (mapel != null)
            {
                if (mapel.Nama != null)
                {
                    if (show_all)
                    {
                        for (int i = Libs.GetStringToInteger(semester); i > 0; i--)
                        {
                            List<Rapor_StrukturNilai> lst_stuktur_nilai =
                                DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, i.ToString()).FindAll(
                                        m0 => m0.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper() &&
                                              m0.Rel_Kelas.ToString().ToUpper() == s_rel_kelas_1.ToString().ToUpper() &&
                                              m0.Rel_Kelas2.ToString().ToUpper() == s_rel_kelas_2.ToString().ToUpper() &&
                                              m0.Rel_Kelas3.ToString().ToUpper() == s_rel_kelas_3.ToString().ToUpper()
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
                                            m_sn.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, kelas_det.Kode.ToString(), rel_mapel.ToString(), tahun_ajaran, i.ToString()

                                        ))
                                            {
                                                b_nilai_can_edit = true;
                                                break;
                                            }
                                    }                                    
                                }
                                b_nilai_can_edit = true;

                                s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE +
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
                        List<Rapor_StrukturNilai> lst_stuktur_nilai =
                            DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, semester).FindAll(
                                    m0 => m0.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper() &&
                                          m0.Rel_Kelas.ToString().ToUpper() == s_rel_kelas_1.ToString().ToUpper() &&
                                          m0.Rel_Kelas2.ToString().ToUpper() == s_rel_kelas_2.ToString().ToUpper() &&
                                          m0.Rel_Kelas3.ToString().ToUpper() == s_rel_kelas_3.ToString().ToUpper()
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
                                        m_sn.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk, kelas_det.Kode.ToString(), rel_mapel.ToString(), tahun_ajaran, semester

                                    ))
                                    {
                                        b_nilai_can_edit = true;
                                        break;
                                    }
                                }
                            }
                            s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE +
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