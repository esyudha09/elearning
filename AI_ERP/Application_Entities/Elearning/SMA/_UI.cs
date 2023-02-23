using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Entities.Elearning.SMA
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
            string s_html_nilai_sikap = "";
            string s_html_nilai_ekskul = "";
            KelasDet m_kelas = DAO_KelasDet.GetByID_Entity(kelas_det);

            if (m_kelas != null)
            {
                if (m_kelas.Nama != null)
                {
                    if (Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas())
                    {

                        List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_mapel_nilai_sn = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                tahun_ajaran,
                                m_kelas.Rel_Kelas.ToString(),
                                no_induk_guru
                            ).OrderByDescending(m => m.Semester).ToList();

                        string html_tile = "";
                        string html_tile_sikap = "";
                        string html_tile_ekskul = "";
                        List<string> lst_semester = lst_mapel_nilai_sn.Select(m => m.Semester).Distinct().ToList();
                        int id_semester = 1;
                        foreach (var semester in lst_semester)
                        {
                            List<DAO_FormasiGuruMapel.FormasiGuruMapel_Ext> lst_mapel_nilai = DAO_FormasiGuruMapel.GetByTABySMByKelasByKelasDet_Entity(
                                tahun_ajaran,
                                semester,
                                m_kelas.Rel_Kelas.ToString(),
                                m_kelas.Kode.ToString()
                            ).OrderByDescending(m => m.Semester).ToList();

                            int id_akademik = 0;
                            int id_sikap = 0;
                            int id_ekskul = 0;
                            string list_mapel = "";
                            string list_mapel_sikap = "";
                            string list_mapel_ekskul = "";

                            List<Rapor_StrukturNilai_KURTILAS_AP> lst_sn_ap = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByTABySM_Entity(
                                tahun_ajaran, semester
                            );

                            List<FormasiGuruKelas_ByGuru> lst_kelasguru =
                                    DAO_FormasiGuruKelas.GetByGuruByTA_Entity(no_induk_guru, tahun_ajaran).FindAll(m0 => m0.Semester == semester).ToList();

                            //jika kelas yg dibuka adalah kelas guru mapel sedangkan dia juga sbg walikelas kelas tersebut
                            if (!Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel.Trim() != "")
                            {
                                lst_mapel_nilai = lst_mapel_nilai.FindAll(m => m.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper());
                            }

                            foreach (DAO_FormasiGuruMapel.FormasiGuruMapel_Ext m in lst_mapel_nilai.FindAll(m => m.Semester == semester))
                            {
                                DAO_Rapor_StrukturNilai.StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(m.TahunAjaran, m.Semester, m.Rel_Kelas, m.Rel_Mapel).FirstOrDefault();
                                if (m_sn != null)
                                {
                                    if (m_sn.TahunAjaran != null)
                                    {

                                        int i_count = 0;
                                        i_count = lst_sn_ap.FindAll(m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() == m_sn.Kode.ToString().ToUpper().Trim()).Count;

                                        bool allow_mapel = false;
                                        bool b_nilai_can_edit = !IsReadonlyNilai(
                                            m_sn.Kode.ToString(), no_induk_guru, m_kelas.Kode.ToString(), m.Rel_Mapel.ToString(), m.TahunAjaran, m.Semester
                                        );

                                        List<FormasiGuruKelas_ByGuru> lst_kelasguru_ =
                                            lst_kelasguru.FindAll(m0 => m0.Semester == semester).ToList();
                                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());

                                        if (lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == kelas_det.Trim().ToUpper() && m0.KodeMapel == "").Count > 0 ||
                                            lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == kelas_det.Trim().ToUpper() && m0.KodeMapel.ToString().ToUpper() == m.Rel_Mapel.ToString().ToUpper()).Count > 0)
                                        {
                                            allow_mapel = true;
                                        }
                                        if (i_count == 0) allow_mapel = false;


                                        if (m_mapel != null && allow_mapel)
                                        {
                                            if (m_mapel.Nama != null)
                                            {

                                                if (m_mapel.Jenis != Libs.JENIS_MAPEL.SIKAP && m_mapel.Jenis != Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                                {
                                                    string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE +
                                                                    "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                    "s=" + m.Semester + "&" +
                                                                    "kd=" + m.Rel_KelasDet.ToString() + "&" +
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
                                                    list_mapel += ("<tr class=" + (id_akademik % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-size: x-small;\">" +
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
                                                                            "<br />" +
                                                                            "<span style=\"font-weight: normal; font-size: small; color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                                m.Kelas + ", " +
                                                                                "<span style=\"font-style: italic;\">" +
                                                                                    m.JenisKelas +
                                                                                "<span>" +
                                                                            "</span>" +
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
                                                    string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE +
                                                                    "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                    "s=" + m.Semester + "&" +
                                                                    "kd=" + m.Rel_KelasDet.ToString() + "&" +
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
                                                    list_mapel_ekskul +=
                                                                ("<tr class=" + (id_ekskul % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                                    "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-size: x-small;\">" +
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
                                                    string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_SIKAP.ROUTE +
                                                                    "?t=" + RandomLibs.GetRndTahunAjaran(m.TahunAjaran) + "&" +
                                                                    "ft=" + Libs.GetQueryString("ft") + "&" +
                                                                    "s=" + m.Semester + "&" +
                                                                    "kd=" + m.Rel_KelasDet.ToString() + "&" +
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

                            string id_ui_sikap = "ui_tile_sikap_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();

                            //nilai sikap wali kelas
                            string s_url_sikap_walas = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_SIKAP.ROUTE +
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

                            id_semester++;
                        }
                    }
                    else
                    {
                        List<DAO_Rapor_StrukturNilai.StrukturNilai> lst_mapel_nilai = DAO_Rapor_StrukturNilai.GetAllByTAByKelasByGuru_Entity(
                                tahun_ajaran,
                                m_kelas.Rel_Kelas.ToString(),
                                no_induk_guru
                            ).OrderByDescending(m => m.Semester).ToList();

                        string html_tile = "";
                        string html_tile_sikap = "";
                        string html_tile_ekskul = "";
                        List<string> lst_semester = lst_mapel_nilai.Select(m => m.Semester).Distinct().ToList();
                        int id_semester = 1;
                        foreach (var semester in lst_semester)
                        {
                            int id_akademik = 0;
                            int id_sikap = 0;
                            int id_ekskul = 0;
                            string list_mapel = "";
                            string list_mapel_sikap = "";
                            string list_mapel_ekskul = "";

                            List<Rapor_StrukturNilai_KURTILAS_AP> lst_sn_ap = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByTABySM_Entity(
                                tahun_ajaran, semester
                            );

                            List<FormasiGuruKelas_ByGuru> lst_kelasguru =
                                    DAO_FormasiGuruKelas.GetByGuruByTA_Entity(no_induk_guru, tahun_ajaran).FindAll(m0 => m0.Semester == semester).ToList();

                            //jika kelas yg dibuka adalah kelas guru mapel sedangkan dia juga sbg walikelas kelas tersebut
                            if (!Libs.URL_IDENTIFIER.IsAdaIDUrlIdGuruKelas() && rel_mapel.Trim() != "")
                            {
                                lst_mapel_nilai = lst_mapel_nilai.FindAll(m => m.Rel_Mapel.ToString().ToUpper() == rel_mapel.ToString().ToUpper());
                            }

                            foreach (DAO_Rapor_StrukturNilai.StrukturNilai m in lst_mapel_nilai.FindAll(m => m.Semester == semester))
                            {
                                int i_count = 0;
                                i_count = lst_sn_ap.FindAll(m0 => m0.Rel_Rapor_StrukturNilai.ToString().ToUpper().Trim() == m.Kode.ToString().ToUpper().Trim()).Count;

                                bool allow_mapel = false;
                                bool b_nilai_can_edit = !IsReadonlyNilai(
                                    m.Kode.ToString(), no_induk_guru, m_kelas.Kode.ToString(), m.Rel_Mapel.ToString(), m.TahunAjaran, m.Semester
                                );

                                List<FormasiGuruKelas_ByGuru> lst_kelasguru_ =
                                    lst_kelasguru.FindAll(m0 => m0.Semester == semester).ToList();
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());

                                if (lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == kelas_det.Trim().ToUpper() && m0.KodeMapel == "").Count > 0 ||
                                    lst_kelasguru.FindAll(m0 => m0.Rel_KelasDet.Trim().ToUpper() == kelas_det.Trim().ToUpper() && m0.KodeMapel.ToString().ToUpper() == m.Rel_Mapel.ToString().ToUpper()).Count > 0)
                                {
                                    allow_mapel = true;
                                }
                                if (i_count == 0) allow_mapel = false;


                                if (m_mapel != null && allow_mapel)
                                {
                                    if (m_mapel.Nama != null)
                                    {

                                        if (m_mapel.Jenis != Libs.JENIS_MAPEL.SIKAP && m_mapel.Jenis != Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                        {
                                            string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE +
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
                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-size: x-small;\">" +
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
                                            string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE +
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
                                                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 5px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle; color: #bfbfbf; font-size: x-small;\">" +
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
                                            string s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_SIKAP.ROUTE +
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
                            s_html_nilai_ekskul += html_tile_ekskul;

                            string id_ui_sikap = "ui_tile_sikap_" + tahun_ajaran.Replace("/", "_").Replace("\\", "_") + "_" + semester.ToString();
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

                            id_semester++;
                        }
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
                        string s_url = "";
                        //if (m_struktur.TahunAjaran == "2020/2021")
                        //{
                        //    s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE +
                        //               "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                        //               "ft=" + Libs.GetQueryString("ft") + "&" +
                        //               "s=" + m_struktur.Semester + "&" +
                        //               "kd=" + kelas_det + "&" +
                        //               "tr=" + TipeRapor.LTS + "&" +
                        //               "j=" + Downloads.JenisDownload.RAPOR_LTS_SMA + "&" +
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
                        //    s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_LTS.ROUTE +
                        //               "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                        //               "ft=" + Libs.GetQueryString("ft") + "&" +
                        //               "s=" + m_struktur.Semester + "&" +
                        //               "kd=" + kelas_det + "&" +
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
                        //                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                        //                                "<i class=\"fa fa-file-o\"></i>" +
                        //                            "</td>" +
                        //                            "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                        //                                "<label onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
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
                        
                        s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.LIHAT_LEDGER.ROUTE +
                                "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                                "ft=" + Libs.GetQueryString("ft") + "&" +
                                "s=" + m_struktur.Semester + "&" +
                                "kd=" + kelas_det + "&" +
                                "tr=" + TipeRapor.SEMESTER + "&" +
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
                                Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LEDGER;

                        s_html_nilai_rapor += "<tr class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                                                    "<i class=\"fa fa-file-o\"></i>" +
                                                "</td>" +
                                                "<td onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                                                    "<label onclick=\"window.open('" + page.ResolveUrl(s_url) + "', '_blank');\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                                                        "Ledger Nilai Rapor" +
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

                        s_url = Libs.FILE_PAGE_URL +
                                "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                                "ft=" + Libs.GetQueryString("ft") + "&" +
                                "s=" + m_struktur.Semester + "&" +
                                "kd=" + kelas_det + "&" +
                                "tr=" + TipeRapor.SEMESTER + "&" +
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
                                Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_RAPOR;

                        //string s_sw = "";
                        //List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                        //        GetUnit(), kelas_det, m_struktur.TahunAjaran, m_struktur.Semester
                        //    );
                        //foreach (var item in lst_siswa)
                        //{
                        //    s_sw += (s_sw.Trim() != "" ? ";" : "") + 
                        //            item.Kode.ToString();
                        //}
                        //string s_js_link_rapor = "window.open(" +
                        //                    "'" + page.ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.ROUTE) + "?'" +
                        //                        " + 'j=' + '" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_SEMESTER_SMA + "' " +
                        //                        " + '&t=' + '" + m_struktur.TahunAjaran + "'" +
                        //                        " + '&kd=' + '" + kelas_det + "'" +
                        //                        " + '&hal=1'" +
                        //                        " + '&s=' + '" + m_struktur.Semester + "' + '&sw=' + '" + s_sw + "', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0'); " +
                        //                "return false; ";

                        //s_html_nilai_rapor += "<tr style=\"\" class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                        //                        "<td onclick=\"" + s_js_link_rapor + "\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
                        //                            "<i class=\"fa fa-file-o\"></i>" +
                        //                        "</td>" +
                        //                        "<td onclick=\"" + s_js_link_rapor + "\" style=\"cursor: pointer; padding: 10px; font-weight: normal; color: grey;\">" +
                        //                            "<label onclick=\"" + s_js_link_rapor + "\" style=\"color: grey; cursor: pointer; width: 100%; font-weight: bold;\">" +
                        //                                "Nilai Rapor Semester" +
                        //                                "<span style=\"font-weight: normal; font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                        //                                    ", " +
                        //                                    "Semester " +
                        //                                    m_struktur.Semester +
                        //                                "</span>" +
                        //                            "</label>" +
                        //                        "</td>" +
                        //                      "</tr>" +
                        //                      "<tr style=\"\">" +
                        //                        "<td colspan=\"2\" style=\"padding: 0px;\">" +
                        //                            "<hr style=\"margin: 0px;\" />" +
                        //                        "</td>" +
                        //                      "</tr>";
                        //id++;

                        s_url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.ROUTE +
                                "?t=" + RandomLibs.GetRndTahunAjaran(m_struktur.TahunAjaran) + "&" +
                                "ft=" + Libs.GetQueryString("ft") + "&" +
                                "s=" + m_struktur.Semester + "&" +
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
                                (
                                    Libs.GetQueryString("m").Trim() != ""
                                    ? "m=" + Libs.GetQueryString("m").Trim() + "&"
                                    : ""
                                ) +
                                Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS_LTS;

                        s_html_nilai_rapor += "<tr class=" + (id % 2 == 0 ? "standardrow" : "oddrow") + ">" +
                                                "<td onclick=\"ResponseRedirect('" + page.ResolveUrl(s_url) + "');\" style=\"cursor: pointer; padding: 10px; width: 30px; padding-left: 15px; padding-right: 0px; vertical-align: middle;\">" +
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

        public static string GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault().Kode.ToString();
        }
    }
}