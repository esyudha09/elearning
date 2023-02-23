using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_Rapor_LTS
    {
        public const string SP_SELECT_ALL = "SD_Rapor_LTS_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SD_Rapor_LTS_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SD_Rapor_LTS_SELECT_BY_ID";

        public const string SP_INSERT = "SD_Rapor_LTS_INSERT";

        public const string SP_UPDATE = "SD_Rapor_LTS_UPDATE";

        public const string SP_DELETE = "SD_Rapor_LTS_DELETE";

        public const string FONT_SIZE = "@fontsize";

        public static Sekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
            return m_unit;
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string IsLocked = "IsLocked";
        }

        private static Rapor_LTS GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_LTS
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                IsLocked = Convert.ToBoolean((row[NamaField.IsLocked] == DBNull.Value ? false : row[NamaField.IsLocked]))
            };
        }

        public static void Delete(string Kode)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Insert(Rapor_LTS m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Update(Rapor_LTS m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static string GetHTMLReport(
            System.Web.UI.Page page, 
            string tahun_ajaran, 
            string semester, 
            string rel_kelas_det, 
            bool show_pagebreak, 
            string rel_siswa = "", 
            bool print_mode = false, 
            bool show_qrcode = false
        ){
            string html = "";
            string html_table_header = "";
            string html_table_header0 = "";
            string html_table_header1 = "";
            string html_table_header2 = "";
            string qrcode = "";

            Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester &&
                          m0.JenisRapor == "LTS"

                ).FirstOrDefault();

            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());

                    if (m_kelas != null)
                    {

                        if (m_kelas.Nama != null)
                        {

                            List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                m_kelas.Rel_Sekolah.ToString(),
                                rel_kelas_det,
                                tahun_ajaran,
                                semester
                            );

                            Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();

                            string s_walikelas = "";
                            string s_tanggal = "";

                            if (lst_formasi_guru_kelas != null)
                            {
                                if (lst_formasi_guru_kelas.Count > 0)
                                {
                                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                                    if (m_guru_kelas != null)
                                    {
                                        if (m_guru_kelas.TahunAjaran != null)
                                        {
                                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                                            if (m_pegawai != null)
                                            {
                                                if (m_pegawai.Nama != null)
                                                {
                                                    s_walikelas = m_pegawai.Nama;

                                                    if (m_rapor_arsip != null)
                                                    {
                                                        if (m_rapor_arsip.JenisRapor != null)
                                                        {
                                                            s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (s_walikelas.Trim() == "")
                            {
                                if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                {
                                    s_walikelas = m_mengetahui.NamaGuru;
                                    s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                }
                            }

                            //nilai akademik
                            List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det
                                ).FindAll(m => m.JenisRapor == "LTS");

                            List<Rapor_StrukturNilai> lst_sn = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, m_kelas.Kode.ToString()
                                );

                            List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det_ = new List<Rapor_NilaiSiswa_Det_Extend>();
                            lst_nilai_siswa_det_ = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Extend_Entity(
                                    tahun_ajaran, semester, rel_kelas_det
                                ).FindAll(m => m.Nilai.Trim() != "");

                            Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();

                            List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen_ = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                            lst_absen_ = DAO_SiswaAbsen.GetRekapAbsenRaporByPeriode_Entity(
                                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                );

                            List<DAO_SiswaAbsenMapel.AbsenMapel> lst_absen_mapel = DAO_SiswaAbsenMapel.GetAllByTABySMByPeriodeByKelas_Entity(
                                    tahun_ajaran, semester,
                                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                    (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue),
                                    rel_kelas_det
                                );

                            List<Rapor_StrukturNilai_KP> lst_sn_kp = DAO_Rapor_StrukturNilai_KP.GetAllByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, m_kelas_det.Rel_Kelas.ToString()
                                );

                            if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode == new Guid(rel_siswa));
                            foreach (var m_siswa in lst_siswa)
                            {
                                //header
                                string html_header =
                                       "<table style=\"width: 100%; margin: 0px; " + (print_mode ? "margin-top: 200px;" : "") + "\">" +
                                            "<tr>" +
                                                "<td style=\"font-weight: bold; padding: 5px; text-align: center; font-size: 12pt; \"Arial Black\", Gadget, Arial, sans-serif\">" +
                                                    "LAPORAN<br />" +
                                                    "HASIL BELAJAR TENGAH SEMESTER " + (semester.Trim() == "1" ? "I" : "II") + "<br />" +
                                                    "TAHUN PELAJARAN " + tahun_ajaran +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; text-align: center;\">" +
                                                    "<hr style=\"margin: 0px;\" />" +
                                                "</td>" +
                                            "</tr>" +
                                       "</table>" +
                                       "<table style=\"margin: 15px; width: 100%;\">" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    "Nama" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; font-weight: bold;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    "NIS" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    Libs.GetPerbaikiEjaanNama(m_siswa.NISSekolah) +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    "Kelas" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    m_kelas_det.Nama +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>";

                                string html_table_body = "";

                                if (lst_desain_rapor.Count == 1)
                                {
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            int jml_kolom = 5;
                                            string key_colspan_mapel = "@colspan_mapel";
                                            html_table_header0 += "<tr>" +
                                                                      "<td rowspan=\"2\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"2\" " + " style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "MATA PELAJARAN" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"2\" " + " style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;KKM&nbsp;" +
                                                                      "</td>" +
                                                                      "<td colspan=\"5\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "TUGAS" +
                                                                      "</td>" +
                                                                      "<td colspan=\"5\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "ULANGAN HARIAN" +
                                                                      "</td>" +
                                                                      "<td colspan=\"5\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "PRAKTIK" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"2\" " + " style=\"" + FONT_SIZE + " font-size: 8pt; line-height: 15px; font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;KE-<br />HADIRAN&nbsp;" +
                                                                      "</td>" +
                                                                  "</tr>";

                                            for (int i = 1; i <= jml_kolom; i++)
                                            {
                                                html_table_header1 += "<td style=\"" + FONT_SIZE + "text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            i.ToString() +
                                                                      "</td>";
                                            }
                                            for (int i = 1; i <= jml_kolom; i++)
                                            {
                                                html_table_header1 += "<td style=\"" + FONT_SIZE + "text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            i.ToString() +
                                                                      "</td>";
                                            }
                                            for (int i = 1; i <= jml_kolom; i++)
                                            {
                                                html_table_header1 += "<td style=\"" + FONT_SIZE + "text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            i.ToString() +
                                                                      "</td>";
                                            }

                                            html_table_header1 = "<tr>" +
                                                                    html_table_header1 +
                                                                 "</tr>";

                                            bool ada_poin = false;
                                            int id = 1;
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                string html_kolom = "";
                                                string s_border_bottom_nomor = "";
                                                string s_border_top_nomor = "";
                                                string s_kkm = "&nbsp;";

                                                Rapor_StrukturNilai m_struktur = lst_sn.FindAll(
                                                        m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == m_desain.Rel_Mapel.ToString().Trim().ToUpper()
                                                    ).FirstOrDefault();

                                                if (m_struktur != null)
                                                {
                                                    if (m_struktur.TahunAjaran != null)
                                                    {
                                                        s_kkm = Convert.ToInt16(m_struktur.KKM).ToString();
                                                    }
                                                }

                                                //kkm
                                                html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    s_kkm +
                                                              "</td>";

                                                List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det = new List<Rapor_NilaiSiswa_Det_Extend>();
                                                lst_nilai_siswa_det = lst_nilai_siswa_det_.FindAll(
                                                        m0 => m0.Rel_Mapel.ToString().Trim().ToUpper() == m_desain.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                              m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper()
                                                    ).FindAll(m => m.Nilai.Trim() != "");

                                                List<string> lst_distinct_kd = lst_nilai_siswa_det.Select(m => m.Rel_Rapor_KompetensiDasar).Distinct().ToList();
                                                int sisa_kolom_kosong = 5;

                                                //tugas
                                                sisa_kolom_kosong = 15;
                                                for (int i = 1; i <= 5; i++)
                                                {
                                                    var lst = lst_nilai_siswa_det.FindAll(m => 
                                                            (
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "TUGAS" ||
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0 
                                                            ) 
                                                            && m.UrutanKP == i
                                                        );
                                                    if (lst.Count == 1)
                                                    {
                                                        if (
                                                                lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                            )
                                                        {
                                                            html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                (
                                                                                    lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                    ? lst[0].PB
                                                                                    : lst[0].Nilai
                                                                                )
                                                                                +
                                                                          "</td>";
                                                        }
                                                        else
                                                        {
                                                            html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                "&nbsp;" +
                                                                          "</td>";
                                                        }
                                                        sisa_kolom_kosong--;
                                                    }
                                                    else
                                                    {
                                                        html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;" +
                                                                      "</td>";
                                                    }
                                                }
                                                
                                                //UH
                                                for (int i = 1; i <= 5; i++)
                                                {
                                                    var lst = lst_nilai_siswa_det.FindAll(m => 
                                                        (
                                                            Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "UH" ||
                                                            Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                        )
                                                        && m.UrutanKP == i);
                                                    if (lst.Count == 1)
                                                    {
                                                        if (
                                                                lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                            )
                                                        {
                                                            html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                (
                                                                                    lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                    ? lst[0].PB
                                                                                    : lst[0].Nilai
                                                                                ) +
                                                                          "</td>";
                                                        }
                                                        else
                                                        {
                                                            html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                "&nbsp;" +
                                                                          "</td>";
                                                        }
                                                        sisa_kolom_kosong--;
                                                    }
                                                    else
                                                    {
                                                        html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;" +
                                                                      "</td>";
                                                    }
                                                }

                                                //praktik
                                                for (int i = 1; i <= 5; i++)
                                                {
                                                    var lst = lst_nilai_siswa_det.FindAll(m => 
                                                            (                                                            
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "PRAKTIK" ||
                                                                Libs.GetHTMLSimpleText(m.NamaKD.Trim().ToUpper()) == "KETERAMPILAN"
                                                            )
                                                            && m.UrutanKP == i 
                                                        );
                                                    if (lst.Count == 1)
                                                    {
                                                        if (
                                                                lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == lst[0].Rel_Rapor_StrukturNilai_KP.ToString()).Count > 0 &&
                                                                lst_sn_kp.FindAll(m0 => m0.Kode.ToString() == lst[0].Rel_Rapor_StrukturNilai_KP.ToString() && m0.IsLTS == true).Count > 0
                                                            )
                                                        {
                                                            html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                (
                                                                                    lst[0].PB.Trim() != "" && lst[0].PB.Trim().Substring(0, 1) != "="
                                                                                    ? lst[0].PB
                                                                                    : lst[0].Nilai
                                                                                ) +
                                                                          "</td>";
                                                        }
                                                        else
                                                        {
                                                            html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                "&nbsp;" +
                                                                          "</td>";
                                                        }
                                                        sisa_kolom_kosong--;
                                                    }
                                                    else
                                                    {
                                                        html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;" +
                                                                      "</td>";
                                                    }
                                                }

                                                //absen
                                                string s_jumlah_hadir = "0";
                                                string s_jumlah_hadir_max = "0";
                                                DAO_SiswaAbsenMapel.AbsenMapel m_absen_mapel = lst_absen_mapel.FindAll(
                                                        m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == m_desain.Rel_Mapel.ToString().ToUpper().Trim() &&
                                                              m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                                    ).FirstOrDefault();
                                                if (m_absen_mapel != null)
                                                {
                                                    if (m_absen_mapel.Rel_Siswa != null)
                                                    {
                                                        s_jumlah_hadir = m_absen_mapel.JumlahHadir;
                                                        s_jumlah_hadir_max = m_absen_mapel.JumlahHadirMax;
                                                    }
                                                }
                                                html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    (
                                                                        s_jumlah_hadir + " / " + s_jumlah_hadir_max 
                                                                    ) +
                                                              "</td>";

                                                if (sisa_kolom_kosong == jml_kolom * 3)
                                                {
                                                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_desain.Rel_Mapel.ToString());
                                                    if (m_mapel != null)
                                                    {
                                                        if (m_mapel.Nama != null)
                                                        {
                                                            if (m_mapel.Jenis.Trim().ToLower() == "pilihan")
                                                            {
                                                                html_kolom = "";
                                                            }
                                                        }
                                                    }
                                                }

                                                if (html_kolom.Trim() != "")
                                                {
                                                    html_table_body += "<tr>" +
                                                                            "<td style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;" + s_border_bottom_nomor + s_border_top_nomor + "\">" +
                                                                                m_desain.Nomor +
                                                                            "</td>" +
                                                                            "<td style=\"" + FONT_SIZE + " width: 350px; border-style: solid; border-left-style: none; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-left: 5px;\">" +
                                                                                m_desain.NamaMapelRapor +
                                                                            "</td>" +
                                                                            html_kolom +
                                                                        "</tr>";
                                                    id++;
                                                }
                                            }

                                            if (ada_poin)
                                            {
                                                html_table_header0 = html_table_header0.Replace(key_colspan_mapel, " colspan=\"2\" ");
                                            }
                                        }
                                    }

                                }
                                //end nilai akademik

                                //absen
                                string s_sakit = "-";
                                string s_izin = "-";
                                string s_alpa = "-";
                                string s_terlambat = "-";

                                List<DAO_SiswaAbsen.AbsenRekapRaporSiswa> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRaporSiswa>();
                                lst_absen = lst_absen_.FindAll(m0 => m0.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper());
                                foreach (var absen in lst_absen)
                                {
                                    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                }

                                html_table_header += html_table_header0 +
                                                     html_table_header1 +
                                                     html_table_header2;

                                string id_siswa = m_siswa.Kode.ToString().Replace("-", "_");

                                string html_kedisiplinan = "";
                                string html_ketidakhadiran = "";

                                html_kedisiplinan = "<table style=\"width: 100%;\">" +
                                                        "<tr>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: left; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "1.&nbsp;Keterlambatan" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "&nbsp;:&nbsp;" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                (
                                                                    s_terlambat.Trim() != "0" && s_terlambat.Trim() != ""
                                                                    ? s_terlambat : "-"
                                                                ) +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "kali" +
                                                            "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: left; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "2.&nbsp;Kerapihan Seragam" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "&nbsp;:&nbsp;" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "-" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "kali" +
                                                            "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: left; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "3.&nbsp;Kelengkapan Seragam" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "&nbsp;:&nbsp;" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "-" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "kali" +
                                                            "</td>" +
                                                        "</tr>" +
                                                    "</table>";

                                html_ketidakhadiran = "<table style=\"width: 100%;\">" +
                                                        "<tr>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: left; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "1.&nbsp;Sakit" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "&nbsp;:&nbsp;" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                s_sakit +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "hari" +
                                                            "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: left; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "2.&nbsp;Izin" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "&nbsp;:&nbsp;" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                s_izin +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "hari" +
                                                            "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: left; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "3.&nbsp;Tanpa Keterangan" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "&nbsp;:&nbsp;" +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                s_alpa +
                                                            "</td>" +
                                                            "<td style=\"" + FONT_SIZE + "text-align: center; padding-bottom: 3px; padding-top: 3px;\">" +
                                                                "hari" +
                                                            "</td>" +
                                                        "</tr>" +
                                                    "</table>";

                                html +=
                                       html_header +
                                       "<table style=\"margin: 15px; border-collapse: collapse;\">" +
                                            html_table_header +
                                            html_table_body +
                                            "<tr>" +
                                                "<td colspan=\"8\" style=\"" + FONT_SIZE + "text-align: center; font-weight: bold; border-style: solid; border-width: 1px; \">" +
                                                    "Kedisiplinan" +
                                                "</td>" +
                                                "<td colspan=\"11\" style=\"" + FONT_SIZE + "text-align: center; font-weight: bold; border-style: solid; border-width: 1px; \">" +
                                                    "Ketidakhadiran" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan=\"8\" style=\"padding-left: 20px; " + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; \">" +
                                                    html_kedisiplinan +
                                                "</td>" +
                                                "<td colspan=\"11\" style=\"padding-left: 20px; " + FONT_SIZE + "text-align: center; border-style: solid; border-width: 1px; \">" +
                                                    html_ketidakhadiran +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td colspan=\"19\" style=\"font-size: 10pt;" + FONT_SIZE + " text-align: left; font-weight: bold; padding-left: 20px; padding-top: 15px;\">" +
                                                    "KKM : Kriteria Ketuntasan Minimal" +
                                                "</td>" +
                                            "</tr>" +
                                       "</table>" +
                                       "<div id=\"qrcode_" + id_siswa + "\" style=\"" + FONT_SIZE + " font-size: 10pt; float: right; width:300px; margin-top:45px; margin-right: 15px;\">" +
                                            "<table style=\"margin: 0px;\">" +
                                                "<tr>" +
                                                    "<td style=\"padding: 0px;\">" +
                                                        "Diberikan di" +
                                                    "</td>" +
                                                    "<td style=\"padding: 0px;\">" +
                                                        "&nbsp;&nbsp;:&nbsp;&nbsp;" +
                                                    "</td>" +
                                                    "<td style=\"padding: 0px; padding-left: 5px;\">" +
                                                        "Jakarta" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"padding: 0px;\">" +
                                                        "Tanggal" +
                                                    "</td>" +
                                                    "<td style=\"padding: 0px;\">" +
                                                        "&nbsp;&nbsp;:&nbsp;&nbsp;" +
                                                    "</td>" +
                                                    "<td style=\"padding: 0px; padding-left: 5px;\">" +
                                                        s_tanggal +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                            "WaliKelas" + "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<label style=\"font-weight: bold; font-size: 10pt;\">" +
                                                s_walikelas +
                                            "<label>" +
                                       "</div>" +
                                       "<div style=\"page-break-before:always\">&nbsp;</div>";

                                if (print_mode)
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 9pt;");
                                }
                                else
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 10pt;");
                                }

                                if (show_qrcode)
                                {
                                    qrcode += "<script type=\"text/javascript\">" +
                                                "var qrcode_" + id_siswa + " = new QRCode(document.getElementById(\"qrcode_" + id_siswa + "\"), {" +
                                                    "width : 100, " +
                                                    "height : 100 " +
                                                "});" +

                                                "function makeCode_" + id_siswa + "(){" +
                                                    "qrcode_" + id_siswa + ".makeCode(\"" + m_siswa.Kode.ToString() + "\");" +
                                                "}" +

                                                "makeCode_" + id_siswa + "();" +
                                             "</script>";
                                }

                            }

                        }

                    }

                }
            }

            return html +
                   qrcode;
        }


        public static string GetHTMLReportDeskripsi(
            System.Web.UI.Page page, 
            string tahun_ajaran, 
            string semester, 
            string rel_kelas_det, 
            bool show_pagebreak, 
            string rel_siswa = "", 
            bool print_mode = false
        ){
            string html = "";
            string html_table_header = "";
            string html_table_header0 = "";
            string html_table_header1 = "";
            string qrcode = "";

            Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester &&
                          m0.JenisRapor == "LTS"

                ).FirstOrDefault();

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());

                    if (m_kelas != null)
                    {

                        if (m_kelas.Nama != null)
                        {

                            List<Siswa> lst_siswa = DAO_Siswa.GetAllByTahunAjaranUnitKelas_Entity(
                                m_kelas.Rel_Sekolah.ToString(),
                                rel_kelas_det,
                                tahun_ajaran,
                                semester
                            );

                            if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode == new Guid(rel_siswa));
                            foreach (var m_siswa in lst_siswa)
                            {

                                //header
                                string html_header =
                                       "<table style=\"width: 100%; margin: 0px;\">" +
                                            "<tr>" +
                                                "<td style=\"font-weight: bold; padding: 5px; text-align: center; font-size: 12pt; \"Arial Black\", Gadget, Arial, sans-serif\">" +
                                                    "DESKRIPSI TUGAS, UH, DAN PRAKTIK PERSIAPAN LTS " + semester + "<br />" +
                                                    "SMP ISLAM Al-IZHAR PONDOK LABU<br />" +
                                                    "TAHUN PELAJARAN " + tahun_ajaran +
                                                "</td>" +
                                            "</tr>" +
                                       "</table>" +
                                       "<table style=\"margin: 15px; width: 100%;\">" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    "Kelas" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                    m_kelas_det.Nama +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>";

                                string html_table_body = "";

                                //nilai akademik
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    ).FindAll(m => m.JenisRapor == "LTS");

                                if (lst_desain_rapor.Count == 1)
                                {
                                    Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            html_table_header0 += "<tr>" +
                                                                      "<td rowspan=\"2\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"2\" " + " style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "MATA PELAJARAN" +
                                                                      "</td>" +
                                                                      "<td colspan=\"3\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "URAIAN" +
                                                                      "</td>" +
                                                                  "</tr>";

                                            html_table_header1 = "<tr>" +
                                                                    "<td style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "TUGAS" +
                                                                    "</td>" +
                                                                    "<td style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "ULANGAN HARIAN" +
                                                                    "</td>" +
                                                                    "<td style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "PRAKTIK" +
                                                                    "</td>" +
                                                                 "</tr>";

                                            html_table_header += html_table_header0 +
                                                                 html_table_header1;

                                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                            int id = 1;
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                string html_kolom = "";
                                                string s_border_bottom_nomor = "";
                                                string s_border_top_nomor = "";

                                                string s_tugas = "";
                                                string s_uh = "";
                                                string s_praktik = "";

                                                Rapor_StrukturNilai m_struktur = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                                                        tahun_ajaran, semester, m_kelas.Kode.ToString(), m_desain.Rel_Mapel.ToString()
                                                    ).FirstOrDefault();

                                                if (m_struktur != null)
                                                {
                                                    if (m_struktur.TahunAjaran != null)
                                                    {
                                                        List<Rapor_StrukturNilai_AP> lst_sn_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(m_struktur.Kode.ToString());
                                                        foreach (var sn_ap in lst_sn_ap)
                                                        {
                                                            List<Rapor_StrukturNilai_KD> lst_sn_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(sn_ap.Kode.ToString());
                                                            foreach (var sn_kd in lst_sn_kd)
                                                            {
                                                                Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                                if (m_kd != null)
                                                                {
                                                                    if (m_kd.Nama != null)
                                                                    {

                                                                        if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "PTS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "KETERAMPILAN"
                                                                        )
                                                                        {

                                                                            List<Rapor_NilaiSiswa_Det_Extend> lst_nilai_siswa_det = new List<Rapor_NilaiSiswa_Det_Extend>();
                                                                            lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapel_Entity(
                                                                                    tahun_ajaran, semester, rel_kelas_det, m_desain.Rel_Mapel
                                                                                ).FindAll(m => m.Nilai.Trim() != "");

                                                                            if (
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                            )
                                                                            {
                                                                                foreach (var sn_kp in DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd.Kode.ToString()))
                                                                                {
                                                                                    s_tugas += (
                                                                                                    Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())).Trim() != ""
                                                                                                    ? "<label style=\"font-weight: bold;\">" +
                                                                                                            Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())) +
                                                                                                      "</label>"
                                                                                                    : ""
                                                                                               ) +
                                                                                               (
                                                                                                    Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim()).Trim() != ""
                                                                                                    ? "<br />"
                                                                                                    : ""
                                                                                               ) +
                                                                                               Libs.GetHTMLSimpleText(sn_kp.Materi.Trim()).Trim() +
                                                                                               (
                                                                                                 lst_nilai_siswa_det.FindAll(m=>m.Rel_Rapor_StrukturNilai_KP == sn_kp.Kode.ToString()).Count > 0
                                                                                                 ? "&nbsp;<i class=\"fa fa-check-circle\" style=\"color: green;\"></i>"
                                                                                                 : ""
                                                                                               ) +
                                                                                               "<br /><br />";
                                                                                }
                                                                            }
                                                                            else if (
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH" ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                                            )
                                                                            {
                                                                                foreach (var sn_kp in DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd.Kode.ToString()))
                                                                                {
                                                                                    s_uh +=   (
                                                                                                    Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())).Trim() != ""
                                                                                                    ? "<label style=\"font-weight: bold;\">" +
                                                                                                            Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())) +
                                                                                                      "</label>"
                                                                                                    : ""
                                                                                              ) +
                                                                                              (
                                                                                                    Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim()).Trim() != ""
                                                                                                    ? "<br />"
                                                                                                    : ""
                                                                                              ) +
                                                                                              Libs.GetHTMLSimpleText(sn_kp.Materi.Trim()).Trim() +
                                                                                              (
                                                                                                 lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == sn_kp.Kode.ToString()).Count > 0
                                                                                                 ? "&nbsp;<i class=\"fa fa-check-circle\" style=\"color: green;\"></i>"
                                                                                                 : ""
                                                                                              ) +
                                                                                              "<br /><br />";
                                                                                }
                                                                            }
                                                                            else if (Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "PRAKTIK")
                                                                            {
                                                                                foreach (var sn_kp in DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd.Kode.ToString()))
                                                                                {
                                                                                    s_praktik += (
                                                                                                    Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())).Trim() != ""
                                                                                                    ? "<label style=\"font-weight: bold;\">" +
                                                                                                            Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())) +
                                                                                                      "</label>"
                                                                                                    : ""
                                                                                                 ) +
                                                                                                 (
                                                                                                        Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim()).Trim() != ""
                                                                                                        ? "<br />"
                                                                                                        : ""
                                                                                                 ) +
                                                                                                 Libs.GetHTMLSimpleText(sn_kp.Materi.Trim()).Trim() +
                                                                                                 (
                                                                                                     lst_nilai_siswa_det.FindAll(m => m.Rel_Rapor_StrukturNilai_KP == sn_kp.Kode.ToString()).Count > 0
                                                                                                     ? "&nbsp;<i class=\"fa fa-check-circle\" style=\"color: green;\"></i>"
                                                                                                     : ""
                                                                                                 ) +
                                                                                                 "<br /><br />";
                                                                                }
                                                                            }

                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                html_kolom = "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: left; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 5px; padding-left: 5px;\">" +
                                                                s_tugas +
                                                             "</td>" +
                                                             "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: left; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 5px; padding-left: 5px;\">" +
                                                                s_uh +
                                                             "</td>" +
                                                             "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: left; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 5px; padding-left: 5px;\">" +
                                                                s_praktik +
                                                             "</td>";

                                                if (html_kolom.Trim() != "")
                                                {
                                                    html_table_body += "<tr>" +
                                                                            "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;" + s_border_bottom_nomor + s_border_top_nomor + "\">" +
                                                                                m_desain.Nomor +
                                                                            "</td>" +
                                                                            "<td style=\"" + FONT_SIZE + " vertical-align: top; border-style: solid; border-left-style: none; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-left: 5px;\">" +
                                                                                m_desain.NamaMapelRapor +
                                                                            "</td>" +
                                                                            html_kolom +
                                                                        "</tr>";
                                                    id++;
                                                }
                                            }
                                        }
                                    }

                                }
                                //end nilai akademik

                                html +=
                                       html_header +
                                       "<table style=\"margin: 15px; border-collapse: collapse;\">" +
                                            html_table_header +
                                            html_table_body +
                                            "<tfoot>" +
                                                "<tr>" +
                                                    "<td colspan=\"5\">" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</tfoot>" +
                                       "</table>" +
                                       "<div style=\"page-break-before:always\">&nbsp;</div>";

                                if (print_mode)
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 9pt;");
                                }
                                else
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 10pt;");
                                }
                            }

                        }

                    }

                }
            }

            return html +
                   qrcode;
        }

        public static string GetHTMLReportDeskripsiNoCheck(
            System.Web.UI.Page page,
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            bool show_pagebreak,
            bool print_mode = false
        )
        {
            string html = "";
            string html_table_header = "";
            string html_table_header0 = "";
            string html_table_header1 = "";
            string qrcode = "";

            Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                          m0.Semester == semester &&
                          m0.JenisRapor == "LTS"

                ).FirstOrDefault();

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());

                    if (m_kelas != null)
                    {

                        if (m_kelas.Nama != null)
                        {

                            //header
                            string html_header =
                                   "<table style=\"width: 100%; margin: 0px;\">" +
                                        "<tr>" +
                                            "<td style=\"font-weight: bold; padding: 5px; text-align: center; font-size: 12pt; \"Arial Black\", Gadget, Arial, sans-serif\">" +
                                                "DESKRIPSI TUGAS, UH, DAN PRAKTIK PERSIAPAN LTS " + semester + "<br />" +
                                                "SMP ISLAM Al-IZHAR PONDOK LABU<br />" +
                                                "TAHUN PELAJARAN " + tahun_ajaran +
                                            "</td>" +
                                        "</tr>" +
                                   "</table>" +
                                   "<table style=\"margin: 15px; width: 100%;\">" +
                                        "<tr>" +
                                            "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                "Kelas" +
                                            "</td>" +
                                            "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                ":" +
                                            "</td>" +
                                            "<td style=\"padding: 5px;" + FONT_SIZE + " font-size: 10pt; padding-bottom: 2px; padding-top: 2px;\">" +
                                                m_kelas_det.Nama +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>";

                            string html_table_body = "";

                            //nilai akademik
                            List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det
                                ).FindAll(m => m.JenisRapor == "LTS");

                            if (lst_desain_rapor.Count == 1)
                            {
                                Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                if (m_rapor_desain != null)
                                {
                                    if (m_rapor_desain.TahunAjaran != null)
                                    {
                                        html_table_header0 += "<tr>" +
                                                                  "<td rowspan=\"2\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "No" +
                                                                  "</td>" +
                                                                  "<td rowspan=\"2\" " + " style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "MATA PELAJARAN" +
                                                                  "</td>" +
                                                                  "<td colspan=\"3\" style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "URAIAN" +
                                                                  "</td>" +
                                                              "</tr>";

                                        html_table_header1 = "<tr>" +
                                                                "<td style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "TUGAS" +
                                                                "</td>" +
                                                                "<td style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "ULANGAN HARIAN" +
                                                                "</td>" +
                                                                "<td style=\"" + FONT_SIZE + "font-weight: bold; text-align: center; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "PRAKTIK" +
                                                                "</td>" +
                                                             "</tr>";

                                        html_table_header += html_table_header0 +
                                                             html_table_header1;

                                        List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                        int id = 1;
                                        foreach (var m_desain in lst_desain_rapor_det)
                                        {
                                            string html_kolom = "";
                                            string s_border_bottom_nomor = "";
                                            string s_border_top_nomor = "";

                                            string s_tugas = "";
                                            string s_uh = "";
                                            string s_praktik = "";

                                            Rapor_StrukturNilai m_struktur = DAO_Rapor_StrukturNilai.GetAllByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), m_desain.Rel_Mapel.ToString()
                                                ).FirstOrDefault();

                                            if (m_struktur != null)
                                            {
                                                if (m_struktur.TahunAjaran != null)
                                                {
                                                    List<Rapor_StrukturNilai_AP> lst_sn_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(m_struktur.Kode.ToString());
                                                    foreach (var sn_ap in lst_sn_ap)
                                                    {
                                                        List<Rapor_StrukturNilai_KD> lst_sn_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(sn_ap.Kode.ToString());
                                                        foreach (var sn_kd in lst_sn_kd)
                                                        {
                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                            if (m_kd != null)
                                                            {
                                                                if (m_kd.Nama != null)
                                                                {
                                                                    if (
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0 ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "PTS" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "KETERAMPILAN"
                                                                    )
                                                                    {

                                                                        if (
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                            )
                                                                        {
                                                                            foreach (var sn_kp in DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd.Kode.ToString()))
                                                                            {
                                                                                s_tugas += (
                                                                                                Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())).Trim() != ""
                                                                                                ? "<label style=\"font-weight: bold;\">" +
                                                                                                        Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())) +
                                                                                                  "</label>"
                                                                                                : ""
                                                                                           ) +
                                                                                           (
                                                                                                Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim()).Trim() != ""
                                                                                                ? "<br />"
                                                                                                : ""
                                                                                           ) +
                                                                                           Libs.GetHTMLSimpleText(sn_kp.Materi.Trim()).Trim() +
                                                                                           (
                                                                                                sn_kp.IsLTS 
                                                                                                ? " (LTS)"
                                                                                                : ""
                                                                                           ) + 
                                                                                           "<br /><br />";
                                                                            }
                                                                        }
                                                                        else if (
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH" ||
                                                                                Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                                            )
                                                                        {
                                                                            foreach (var sn_kp in DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd.Kode.ToString()))
                                                                            {
                                                                                s_uh += (
                                                                                                Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())).Trim() != ""
                                                                                                ? "<label style=\"font-weight: bold;\">" +
                                                                                                        Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())) +
                                                                                                  "</label>"
                                                                                                : ""
                                                                                          ) +
                                                                                          (
                                                                                                Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim()).Trim() != ""
                                                                                                ? "<br />"
                                                                                                : ""
                                                                                          ) +
                                                                                          Libs.GetHTMLSimpleText(sn_kp.Materi.Trim()).Trim() +
                                                                                          (
                                                                                                sn_kp.IsLTS
                                                                                                ? " (LTS)"
                                                                                                : ""
                                                                                          ) +
                                                                                          "<br /><br />";
                                                                            }
                                                                        }
                                                                        else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "KETERAMPILAN"
                                                                        )
                                                                        {
                                                                            foreach (var sn_kp in DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd.Kode.ToString()))
                                                                            {
                                                                                s_praktik += (
                                                                                                Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())).Trim() != ""
                                                                                                ? "<label style=\"font-weight: bold;\">" +
                                                                                                        Libs.GetPerbaikiEjaanNama(Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim())) +
                                                                                                  "</label>"
                                                                                                : ""
                                                                                             ) +
                                                                                             (
                                                                                                    Libs.GetHTMLSimpleText(sn_kp.Deskripsi.Trim()).Trim() != ""
                                                                                                    ? "<br />"
                                                                                                    : ""
                                                                                             ) +
                                                                                             Libs.GetHTMLSimpleText(sn_kp.Materi.Trim()).Trim() +
                                                                                             (
                                                                                                sn_kp.IsLTS
                                                                                                ? " (LTS)"
                                                                                                : ""
                                                                                             ) +
                                                                                             "<br /><br />";
                                                                            }
                                                                        }

                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            html_kolom = "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: left; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 5px; padding-left: 5px;\">" +
                                                            s_tugas +
                                                         "</td>" +
                                                         "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: left; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 5px; padding-left: 5px;\">" +
                                                            s_uh +
                                                         "</td>" +
                                                         "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: left; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 5px; padding-left: 5px;\">" +
                                                            s_praktik +
                                                         "</td>";

                                            if (html_kolom.Trim() != "")
                                            {
                                                html_table_body += "<tr>" +
                                                                        "<td style=\"" + FONT_SIZE + " vertical-align: top; text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-right: 0px; padding-left: 0px;" + s_border_bottom_nomor + s_border_top_nomor + "\">" +
                                                                            m_desain.Nomor +
                                                                        "</td>" +
                                                                        "<td style=\"" + FONT_SIZE + " vertical-align: top; border-style: solid; border-left-style: none; border-width: 1px; padding-top: 5px; padding-bottom: 5px; padding-left: 5px;\">" +
                                                                            m_desain.NamaMapelRapor +
                                                                        "</td>" +
                                                                        html_kolom +
                                                                    "</tr>";
                                                id++;
                                            }
                                        }
                                    }
                                }

                            }
                            //end nilai akademik

                            html +=
                                   html_header +
                                   "<table style=\"margin: 15px; border-collapse: collapse;\">" +
                                        html_table_header +
                                        html_table_body +
                                        "<tfoot>" +
                                            "<tr>" +
                                                "<td colspan=\"5\">" +
                                                "</td>" +
                                            "</tr>" +
                                        "</tfoot>" +
                                   "</table>" +
                                   "<div style=\"page-break-before:always\">&nbsp;</div>";

                            if (print_mode)
                            {
                                html = html.Replace(FONT_SIZE, "font-size: 9pt;");
                            }
                            else
                            {
                                html = html.Replace(FONT_SIZE, "font-size: 10pt;");
                            }

                        }

                    }

                }
            }

            return html +
                   qrcode;
        }
    }
}