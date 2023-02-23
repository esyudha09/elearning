using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;

using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_ListAbsensiSiswa : System.Web.UI.Page
    {
        public class KodeNama
        {
            public string Kode { get; set; }
            public string Nama { get; set; }
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

            public static string GetKelas()
            {
                return Libs.GetQueryString("kd");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetMapel()
            {
                return Libs.GetQueryString("m");
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

            public static string GetSemester()
            {
                string s = Libs.GetQueryString("s");
                if (s.Trim() == "") return Libs.GetSemesterByTanggal(DateTime.Now).ToString();
                return Libs.GetQueryString("s").Trim();
            }

            public static string GetJenisLaporan()
            {
                return Libs.GetQueryString("jl");
            }

            public static string GetJenisPresensi()
            {
                return Libs.GetQueryString("jp");
            }

            public static string GetSiswa()
            {
                return Libs.GetQueryString("sw");
            }

            public static string DariTanggal()
            {
                return Libs.GetQueryString("dt");
            }

            public static string SampaiTanggal()
            {
                return Libs.GetQueryString("st");
            }

            public static string GetPeriode()
            {
                return Libs.GetQueryString("p");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowReport();
        }

        protected void ShowReport()
        {
            string periode = QS.GetPeriode();
            string tahun_ajaran = periode.Substring(0, periode.Length - 1);
            string semeser = periode.Substring(periode.Length - 1, 1);
            string rel_sekolah = QS.GetUnit();
            string rel_kelasdet = QS.GetKelas();
            string jenis_laporan = QS.GetJenisLaporan();
            string jenis_presensi = QS.GetJenisPresensi();
            string rel_siswa = QS.GetSiswa();

            DateTime dari_tanggal = Libs.GetDateFromTanggalIndonesiaStr(QS.DariTanggal());
            DateTime sampai_tanggal = Libs.GetDateFromTanggalIndonesiaStr(QS.SampaiTanggal());

            List<SiswaAbsenReport> lst_siswa_absen = DAO_SiswaAbsen.GetByUnitByKelasDetByTanggal_Entity(
                    dari_tanggal, sampai_tanggal, rel_sekolah, rel_kelasdet
                );

            List<KedisiplinanSetup> lst_kedisiplinan_setup = DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(tahun_ajaran, semeser, rel_sekolah, QS.GetLevel());
            List<Kedisiplinan> lst_kedisiplinan = DAO_Kedisiplinan.GetAll_Entity();

            List<Siswa> lst_siswa = new List<Siswa>();
            var lst_o_mapel = lst_siswa_absen.Select(m0 => new { m0.Rel_Mapel, m0.Mapel }).Distinct().ToList();
            List<KodeNama> lst_mapel = new List<KodeNama>();
            foreach (var item in lst_o_mapel)
            {
                lst_mapel.Add(new KodeNama { Kode = item.Rel_Mapel, Nama = item.Mapel });
            }

            string s_html = "";
            string s_header_tanggal = "";
            string s_konten_presensi = "";

            if (!(dari_tanggal > sampai_tanggal))
            {
                int jumlah_hari = (sampai_tanggal - dari_tanggal).Days;
                if (jumlah_hari > 0)
                {
                    //header
                    List<string> lst_s_bulantahun = new List<string>();
                    List<string> lst_det_tanggal = new List<string>();
                    for (int i = 1; i <= jumlah_hari; i++)
                    {
                        DateTime dt_tanggal = (dari_tanggal.AddDays(i - 1));
                        lst_det_tanggal.Add(dt_tanggal.ToString("yyyy") + "|" + dt_tanggal.ToString("MM") + "|" + dt_tanggal.ToString("dd"));
                        if (lst_s_bulantahun.FindAll(m0 => m0 == dt_tanggal.ToString("yyyy") + "|" + dt_tanggal.ToString("MM")).Count == 0)
                        {
                            lst_s_bulantahun.Add(dt_tanggal.ToString("yyyy") + "|" + dt_tanggal.ToString("MM"));
                        }
                    }

                    string s_css_td = "border-color: #bfbfbf; text-align: center; background-color: white;";

                    s_header_tanggal += "<tr>" +
                                            "<th rowspan=\"2\" style=\"" + s_css_td + " background-color: #f4f4f4; border-right-width: 3px; border-right-style: solid; border-right-color: black; border-bottom-width: 3px; border-bottom-style: solid; border-bottom-color: black;\">" +
                                                "&nbsp;" +
                                            "</th>"; 
                    foreach (var item in lst_s_bulantahun)
                    {
                        int i_count = lst_det_tanggal.FindAll(m0 => m0.Substring(0, (item.ToString() + "|").Length) == item.ToString() + "|").Count;
                        s_header_tanggal
                              += "<td colspan=\"" + i_count.ToString() + "\" style=\"" + s_css_td + " font-weight: bold; top: 0; background-color: #f4f4f4; position: sticky; top: 0;\">" +
                                       Libs.Array_Bulan[Libs.GetStringToInteger(item.Substring(5, 2)) - 1] + " " + item.Substring(0, 4).ToString() +
                                 "</td>";
                    }
                    s_header_tanggal += "</tr>";

                    s_header_tanggal += "<tr>";
                    for (int i = 1; i <= jumlah_hari; i++)
                    {
                        string s_css_add = "";
                        DateTime dt_tanggal = (dari_tanggal.AddDays(i - 1));
                        if (dt_tanggal.DayOfWeek == DayOfWeek.Saturday || dt_tanggal.DayOfWeek == DayOfWeek.Sunday)
                        {
                            s_css_add = " background-color: #ff9b9b; color: black; font-weight: bold; ";
                        }
                        s_header_tanggal 
                               += "<td style=\"" + s_css_td + " background-color: #f4f4f4; " + s_css_add + " position: sticky; top: 27px; font-weight: bold; border-bottom-width: 3px; border-bottom-style: solid; border-bottom-color: black;\">" +
                                        dt_tanggal.ToString("dd") + 
                                  "</td>";
                    }
                    s_header_tanggal += "</tr>";
                    //end header

                    //list data siswa
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
                    bool ada_siswa = false;
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    foreach (var item_mapel in lst_mapel.OrderBy(m0 => m0.Nama).ToList())
                    {
                        if (item_mapel.Kode.Trim() != "")
                        {
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(item_mapel.Kode);
                            if (DAO_Mapel.GetJenisMapel(m_mapel.Kode.ToString()) == Libs.JENIS_MAPEL.WAJIB_B_PILIHAN)
                            {
                                if (m_kelas != null)
                                {
                                    if (m_kelas.Nama != null)
                                    {
                                        if (m_kelas.Nama.Trim().ToUpper() != "X")
                                        {
                                            lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                                    tahun_ajaran,
                                                    semeser,
                                                    m_mapel.Kode.ToString(),
                                                    m_kelas_det.Kode.ToString()
                                                );
                                            ada_siswa = true;
                                        }
                                        else
                                        {
                                            if (DAO_FormasiGuruMapelDet.IsSiswaPilihanByGuru(
                                                Libs.LOGGED_USER_M.NoInduk,
                                                tahun_ajaran,
                                                semeser,
                                                m_kelas_det.Kode.ToString(),
                                                m_mapel.Kode.ToString()
                                            ))
                                            {
                                                lst_siswa = DAO_FormasiGuruMapelDetSiswaDet.GetSiswaByTABySMByMapelByKelasDet_Entity(
                                                    tahun_ajaran,
                                                    semeser,
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
                                        tahun_ajaran,
                                        semeser,
                                        m_mapel.Kode.ToString(),
                                        m_kelas_det.Rel_Kelas.ToString(),
                                        m_kelas_det.Kode.ToString()
                                    );

                                ada_siswa = true;
                            }
                            if (!ada_siswa)
                            {
                                lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                        QS.GetUnit(), QS.GetKelas(), tahun_ajaran, semeser
                                    );
                            }
                        }
                        else
                        {
                            lst_siswa = DAO_Siswa.GetByRombel_Entity(
                                        QS.GetUnit(), QS.GetKelas(), tahun_ajaran, semeser
                                    );
                        }

                        //list presensi
                        s_konten_presensi +=
                                "<tr>" +
                                     "<th style=\"" + s_css_td + " font-weight: bold; text-align: left; white-space: nowrap; border-right-width: 3px; border-right-style: solid; border-right-color: black;\">" +
                                           (
                                                item_mapel.Kode.Trim() == ""
                                                ? "Presensi Wali Kelas"
                                                : item_mapel.Nama
                                           ) +
                                     "</th>" +
                                     "<td colspan=\"" + jumlah_hari + "\" style=\"" + s_css_td + " font-weight: bold; text-align: left;\">" +
                                           "&nbsp;" +
                                     "</td>" +
                                "</tr>";
                        //end list presensi

                        int nomor_siswa = 1;
                        foreach (var item_siswa in lst_siswa)
                        {
                            string s_bg = (nomor_siswa % 2 != 0 ? "#f4f4f4" : "white");
                            var lst_per_siswa = lst_siswa_absen.FindAll(
                                        m0 => m0.TahunAjaran == tahun_ajaran &&
                                              m0.Semester == semeser &&
                                              m0.Rel_Siswa.ToUpper().Trim() == item_siswa.Kode.ToString().ToUpper().Trim());

                            s_konten_presensi +=
                                "<tr>" +
                                    "<th style=\"" + s_css_td + " font-weight: normal; text-align: left; white-space: nowrap; text-align: right; background-color: " + (s_bg) + "; border-right-width: 3px; border-right-style: solid; border-right-color: black;\">" +
                                           item_siswa.Nama.ToUpper().Trim() +
                                     "</th>";
                            string s_konten_presensi_det = "";
                            for (int i = 1; i <= jumlah_hari; i++)
                            {
                                DateTime dt_tanggal = (dari_tanggal.AddDays(i - 1));
                                SiswaAbsenReport m_absenreport = new SiswaAbsenReport();
                                m_absenreport = lst_per_siswa.FindAll(
                                        m0 => m0.Tanggal.Year == dt_tanggal.Year &&
                                              m0.Tanggal.Month == dt_tanggal.Month &&
                                              m0.Tanggal.Day == dt_tanggal.Day &&
                                              m0.Rel_Mapel.ToUpper().Trim() == item_mapel.Kode.ToString().ToUpper().Trim()
                                    ).FirstOrDefault();

                                string s_kehadiran = "&nbsp;";
                                if (m_absenreport != null)
                                {
                                    if (m_absenreport.Rel_Siswa != null)
                                    {
                                        if (m_absenreport.Is_Hadir.IndexOf("__") < 0) s_kehadiran = "H";
                                        if (m_absenreport.Is_Sakit.IndexOf("__") < 0) s_kehadiran = "S";
                                        if (m_absenreport.Is_Izin.IndexOf("__") < 0) s_kehadiran = "I";
                                        if (m_absenreport.Is_Alpa.IndexOf("__") < 0) s_kehadiran = "A";
                                    }
                                }

                                string s_css_add = "";
                                if (dt_tanggal.DayOfWeek == DayOfWeek.Saturday || dt_tanggal.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    s_css_add = " background-color: #ff9b9b; color: white; ";
                                }
                                string s_td = "<table style=\"margin: 0px; padding: 0px; border-width: 0px; border-style: none;\">";
                                s_td += "<td style=\"" + 
                                                    s_css_td +
                                                    " background-color: " + (s_bg) + "; font-weight: bold; border-style: none; width: 15px;" +
                                                    (
                                                        s_kehadiran == "H"
                                                        ? " color: green; "
                                                        : (
                                                            s_kehadiran == "S"
                                                            ? " background-color: darkorange; color: white; "
                                                            : (
                                                                s_kehadiran == "I"
                                                                ? " background-color: #c000ff; color: white; "
                                                                : (
                                                                    s_kehadiran == "A"
                                                                    ? " background-color: red; color: white; "
                                                                    : ""
                                                                  )
                                                              )
                                                          )
                                                    ) +
                                                "\">" +
                                            s_kehadiran + 
                                        "</td>";
                                foreach (var item_kedisiplinan in lst_kedisiplinan_setup)
                                {
                                    if (item_kedisiplinan.Rel_Kedisiplinan_01.Trim() != "")
                                    {

                                    }
                                    if (item_kedisiplinan.Rel_Kedisiplinan_02.Trim() != "")
                                    {

                                    }
                                    if (item_kedisiplinan.Rel_Kedisiplinan_03.Trim() != "")
                                    {

                                    }
                                    if (item_kedisiplinan.Rel_Kedisiplinan_04.Trim() != "")
                                    {

                                    }
                                    if (item_kedisiplinan.Rel_Kedisiplinan_05.Trim() != "")
                                    {

                                    }
                                    //s_td += "<td style=\"" + s_css_td + " background-color: white; border-style: none;\">" +
                                    //            "<i class=\"fa fa-check-circle\" style=\"color: green;\"></i>" +
                                    //        "</td>";
                                }
                                s_td += "</table>";
                                if (dt_tanggal.DayOfWeek == DayOfWeek.Saturday || dt_tanggal.DayOfWeek == DayOfWeek.Sunday) s_td = "&nbsp;";
                                s_konten_presensi_det
                                       += "<td style=\"" + s_css_td + " background-color: " + (s_bg) + "; " + s_css_add + " padding: 0px;\">" +
                                                s_td + //absensinya disini
                                          "</td>";
                            }

                            s_konten_presensi += s_konten_presensi_det;
                            s_konten_presensi += "</tr>";

                            nomor_siswa++;
                        }
                    }
                    //end siswa
                }
                else
                {

                }
            }
            else
            {
                
            }

            s_html += "<table style=\"border-collapse: separate; padding: 0px;\" cellpadding=\"0\" cellspacing=\"0\">" +
                        "<thead>" +
                            s_header_tanggal +
                        "</thead>" +
                        s_konten_presensi +
                      "</table>";

            ltrListAbsensi.Text = s_html;
        }
    }
}