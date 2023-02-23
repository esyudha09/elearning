using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SD;

namespace AI_ERP.Application_DAOs.Elearning.SD
{
    public static class DAO_Rapor_LTS
    {
        public const string SP_SELECT_ALL = "SD_Rapor_LTS_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SD_Rapor_LTS_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SD_Rapor_LTS_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET = "SD_Rapor_LTS_SELECT_BY_TA_BY_SM_BY_KELAS_DET";

        public const string SP_INSERT = "SD_Rapor_LTS_INSERT";

        public const string SP_UPDATE = "SD_Rapor_LTS_UPDATE";

        public const string SP_DELETE = "SD_Rapor_LTS_DELETE";

        public const string FONT_SIZE = "@fontsize";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string IsLocked = "IsLocked";
            public const string GuruKelas = "GuruKelas";
            public const string Kurikulum = "Kurikulum";
            public const string Tanggal = "Tanggal";
        }

        private static Rapor_LTS GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_LTS
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                IsLocked = Convert.ToBoolean((row[NamaField.IsLocked] == DBNull.Value ? false : row[NamaField.IsLocked])),
                GuruKelas = row[NamaField.GuruKelas].ToString(),
                Kurikulum = row[NamaField.Kurikulum].ToString(),
                Tanggal = (row[NamaField.Tanggal] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Tanggal]))
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.GuruKelas, m.GuruKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.GuruKelas, m.GuruKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
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

        public static List<Rapor_LTS> GetAllByTABySMByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_LTS> hasil = new List<Rapor_LTS>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }

        public class NilaiLTS
        {
            public string NamaKD { get; set; }
            public int Urutan { get; set; }
            public string NilaiTugas { get; set; }
            public string NilaiUH { get; set; }
        }

        public class Nilai_LTS
        {
            public int UrutanAP { get; set; }
            public int Urutan { get; set; }
            public string NamaKD { get; set; }
            public string Nilai { get; set; }
            public decimal BobotKP { get; set; }
        }

        public static Sekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault();
            return m_unit;
        }

        public static string GetHTMLReport_KTSP(
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

                            List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_det =
                                DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);

                            List<Rapor_LTS_Mapel_Ext> lst_rapor_lts_mapel = DAO_Rapor_LTS_Mapel.GetByTABySMByKelasDet(tahun_ajaran, semester, rel_kelas_det);

                            if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode == new Guid(rel_siswa));
                            foreach (var m_siswa in lst_siswa)
                            {

                                //header
                                string html_header =
                                       "<table style=\"width: 100%; margin: 0px;\">" +
                                            "<tr>" +
                                                "<td style=\"width: 60px;\">" +
                                                    "<img src=\"" + page.ResolveUrl("~/Application_CLibs/images/logo.png") + "\" />" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + " \">" +
                                                    "SEKOLAH DASAR ISLAM AL IZHAR PONDOK LABU<br />" +
                                                    "JL. RS. Fatmawati Kav. 49 Telp. 7695542<br />" +
                                                    "Jakarta" +
                                                "</td>" +
                                            "</tr>" +
                                       "</table>" +
                                       "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                            "LAPORAN PERKEMBANGAN BELAJAR TENGAH SEMESTER  " + (semester == "1" ? "I" : "II") +
                                       "</div>" +
                                       "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                            "TAHUN PELAJARAN " + tahun_ajaran +
                                       "</div>";

                                string html_header_nama =
                                       "<table style=\"margin: 15px; width: 100%;\">" +
                                            "<tr>" +
                                                "<td style=\"padding: 0px; width: 70px;" + FONT_SIZE + " \">" +
                                                    "Nama" +
                                                "</td>" +
                                                "<td style=\"padding: 0px; width: 20px;" + FONT_SIZE + " \">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 0px;" + FONT_SIZE + " \">" +
                                                    Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td style=\"padding: 0px; width: 70px;" + FONT_SIZE + " \">" +
                                                    "Kelas" +
                                                "</td>" +
                                                "<td style=\"padding: 0px; width: 20px;" + FONT_SIZE + " \">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 0px;" + FONT_SIZE + " \">" +
                                                    m_kelas_det.Nama +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>";

                                string html_table_body = "";

                                //nilai akademik
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                    );

                                if (lst_desain_rapor.Count == 1)
                                {
                                    Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            int jml_kolom = 5;
                                            string key_colspan_mapel = "@colspan_mapel";
                                            html_table_header0 += "<tr>" +
                                                                      "<td rowspan=\"3\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"3\" " + key_colspan_mapel + " style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "MATA PELAJARAN" +
                                                                      "</td>" +
                                                                      "<td colspan=\"" + (jml_kolom * 2).ToString() + "\" style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "PENCAPAIAN KOMPETENSI DASAR" +
                                                                      "</td>" +
                                                                  "</tr>";

                                            for (int i = 1; i <= jml_kolom; i++)
                                            {
                                                html_table_header1 += "<td colspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            i.ToString() +
                                                                      "</td>";
                                                html_table_header2 += "<td style=\"" + FONT_SIZE + "; font-size: 8pt; text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "Tugas" +
                                                                      "</td>";
                                                html_table_header2 += "<td style=\"" + FONT_SIZE + " font-size: 8pt; text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "UH" +
                                                                      "</td>";
                                            }

                                            html_table_header1 = "<tr>" +
                                                                    html_table_header1 +
                                                                 "</tr>";

                                            html_table_header2 = "<tr>" +
                                                                    html_table_header2 +
                                                                 "</tr>";

                                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                            bool ada_poin = false;
                                            int id = 1;
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                string html_kolom = "";
                                                string s_border_bottom_nomor = "";
                                                string s_border_top_nomor = "";

                                                //List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapelBySiswaForLTS_Entity(
                                                //        tahun_ajaran, semester, rel_kelas_det, m_desain.Rel_Mapel, m_siswa.Kode.ToString()
                                                //    );
                                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det =
                                                    lst_nilai_det.FindAll(
                                                            m0 => m0.Rel_Mapel == m_desain.Rel_Mapel &&
                                                                  m0.Rel_Siswa == m_siswa.Kode.ToString()
                                                        ).ToList();
                                                List<Rapor_LTS_Mapel_Ext> lst_rapor_lts_mapel_ =
                                                    lst_rapor_lts_mapel.FindAll(
                                                            m0 => m0.Rel_Mapel == m_desain.Rel_Mapel
                                                        ).ToList().OrderBy(m0 => m0.Urutan).ToList();

                                                Rapor_StrukturNilai m_sn = null;

                                                List<string> lst_nilai_1 = new List<string>(); //nilai tugas
                                                List<string> lst_nilai_1_kd = new List<string>(); //nilai tugas KD
                                                List<string> lst_nilai_1_nama_kd = new List<string>(); //nama tugas KD
                                                List<string> lst_nilai_1_bobot_kp = new List<string>(); //bobot tugas KP
                                                List<string> lst_nilai_2 = new List<string>(); //nilai uh
                                                List<string> lst_nilai_2_kd = new List<string>(); //nilai uh KD
                                                List<string> lst_nilai_2_nama_kd = new List<string>(); //nama uh KD
                                                List<string> lst_nilai_2_bobot_kp = new List<string>(); //bobot tugas KP

                                                List<NilaiLTS> lst_nilai_lts = new List<NilaiLTS>();
                                                List<Nilai_LTS> lst_nilai_lts_tugas = new List<Nilai_LTS>();
                                                List<Nilai_LTS> lst_nilai_lts_uh = new List<Nilai_LTS>();

                                                int id_nilai = 0;

                                                foreach (var item_lst_rapor_lts_mapel_ in lst_rapor_lts_mapel_)
                                                {
                                                    bool ada_item = false;
                                                    Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_AP);
                                                    Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KD.ToString());
                                                    Rapor_StrukturNilai_KP m_sn_kp = DAO_Rapor_StrukturNilai_KP.GetByID_Entity(item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KP.ToString());

                                                    Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                    Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());

                                                    foreach (var nilai_siswa_det in lst_nilai_siswa_det.FindAll(
                                                        m0 => m0.Rel_Rapor_StrukturNilai_AP == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_AP &&
                                                              m0.Rel_Rapor_StrukturNilai_KD == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KD &&
                                                              m0.Rel_Rapor_StrukturNilai_KP == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KP
                                                    ))
                                                    {
                                                        if (m_sn_kp != null && m_sn_kd != null)
                                                        {
                                                            if (m_sn_kp.Jenis != null && m_sn_kd.JenisPerhitungan != null)
                                                            {
                                                                ada_item = true;

                                                                if (m_sn_ap.Poin != null)
                                                                {
                                                                    m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(m_sn_ap.Rel_Rapor_StrukturNilai.ToString());
                                                                }

                                                                if (m_kp != null && m_kd != null)
                                                                {
                                                                    if (m_kp.Nama != null && m_kd.Nama != null)
                                                                    {
                                                                        if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "LK")
                                                                        {
                                                                            lst_nilai_1_kd.Add(m_sn_kd.Kode.ToString()); //m_sn_kd.Urutan.ToString()
                                                                            lst_nilai_1_nama_kd.Add(m_kd.Nama.ToString());
                                                                            lst_nilai_1.Add(nilai_siswa_det.Nilai);
                                                                            lst_nilai_1_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                            lst_nilai_lts_tugas.Add(new Nilai_LTS
                                                                            {
                                                                                UrutanAP = m_sn_ap.Urutan,
                                                                                Urutan = m_sn_kd.Urutan,
                                                                                NamaKD = m_kd.Nama,
                                                                                Nilai = nilai_siswa_det.Nilai,
                                                                                BobotKP = m_sn_kp.BobotNK
                                                                            });
                                                                        }
                                                                        else if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "UH" ||
                                                                            Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "UH")
                                                                        {
                                                                            lst_nilai_2_kd.Add(m_sn_kd.Kode.ToString()); //m_sn_kd.Urutan.ToString()
                                                                            lst_nilai_2_nama_kd.Add(m_kd.Nama.ToString());
                                                                            lst_nilai_2.Add(nilai_siswa_det.Nilai);
                                                                            lst_nilai_2_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                            lst_nilai_lts_uh.Add(new Nilai_LTS
                                                                            {
                                                                                UrutanAP = m_sn_ap.Urutan,
                                                                                Urutan = m_sn_kd.Urutan,
                                                                                NamaKD = m_kd.Nama,
                                                                                Nilai = nilai_siswa_det.Nilai,
                                                                                BobotKP = m_sn_kp.BobotNK
                                                                            });
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (!ada_item)
                                                    {
                                                        if (m_kp != null && m_kd != null)
                                                        {
                                                            if (m_kp.Nama != null && m_kd.Nama != null)
                                                            {
                                                                if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                    Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "LK")
                                                                {
                                                                    lst_nilai_1_kd.Add(m_sn_kd.Kode.ToString()); //m_sn_kd.Urutan.ToString()
                                                                    lst_nilai_1_nama_kd.Add(m_kd.Nama.ToString());
                                                                    lst_nilai_1.Add("");
                                                                    lst_nilai_1_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                    lst_nilai_lts_tugas.Add(new Nilai_LTS
                                                                    {
                                                                        UrutanAP = m_sn_ap.Urutan,
                                                                        Urutan = m_sn_kd.Urutan,
                                                                        NamaKD = m_kd.Nama,
                                                                        Nilai = "",
                                                                        BobotKP = m_sn_kp.BobotNK
                                                                    });
                                                                }
                                                                else if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "UH" ||
                                                                    Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                    Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "UH")
                                                                {
                                                                    lst_nilai_2_kd.Add(m_sn_kd.Kode.ToString()); //m_sn_kd.Urutan.ToString()
                                                                    lst_nilai_2_nama_kd.Add(m_kd.Nama.ToString());
                                                                    lst_nilai_2.Add("");
                                                                    lst_nilai_2_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                    lst_nilai_lts_uh.Add(new Nilai_LTS
                                                                    {
                                                                        UrutanAP = m_sn_ap.Urutan,
                                                                        Urutan = m_sn_kd.Urutan,
                                                                        NamaKD = m_kd.Nama,
                                                                        Nilai = "",
                                                                        BobotKP = m_sn_kp.BobotNK
                                                                    });
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (m_sn != null)
                                                {
                                                    if (m_sn.TahunAjaran != null)
                                                    {

                                                        if (m_sn.IsKelompokanKP && !m_sn.IsKelompokanKPNoLTS)
                                                        //contoh al-quran yang di PISAHKAN antara LK & UH nya berdasarkan aspek penilaian AP
                                                        {
                                                            List<int> lst_urutan_kd_tugas = lst_nilai_lts_tugas.Select(m => m.Urutan).Distinct().ToList();
                                                            List<int> lst_urutan_kd_uh = lst_nilai_lts_uh.Select(m => m.Urutan).Distinct().ToList();
                                                            List<int> lst_gabungan = new List<int>();

                                                            foreach (var item in lst_urutan_kd_tugas)
                                                            {
                                                                if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                {
                                                                    lst_gabungan.Add(item);
                                                                }
                                                            }
                                                            foreach (var item in lst_urutan_kd_uh)
                                                            {
                                                                if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                {
                                                                    lst_gabungan.Add(item);
                                                                }
                                                            }
                                                            lst_nilai_lts.Clear();
                                                            int urutan = 1;
                                                            foreach (var item in lst_gabungan.OrderBy(m => m).ToList())
                                                            {
                                                                string s_nilai_tugas = "";
                                                                string s_nilai_uh = "";

                                                                decimal nilai = 0;
                                                                foreach (var item_ in lst_nilai_lts_tugas.FindAll(m => m.Urutan == item))
                                                                {
                                                                    nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                }
                                                                s_nilai_tugas = Libs.GetFormatBilangan(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD);

                                                                nilai = 0;
                                                                foreach (var item_ in lst_nilai_lts_uh.FindAll(m => m.Urutan == item))
                                                                {
                                                                    nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                }
                                                                s_nilai_uh = Libs.GetFormatBilangan(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD);

                                                                lst_nilai_lts.Add(new NilaiLTS {
                                                                    Urutan = urutan,
                                                                    NamaKD = "",
                                                                    NilaiTugas = s_nilai_tugas,
                                                                    NilaiUH = s_nilai_uh
                                                                });
                                                                urutan++;
                                                            }

                                                            id_nilai = 0;
                                                            for (int i = 0; i < jml_kolom; i++)
                                                            {
                                                                if (i < lst_nilai_lts.Count)
                                                                {
                                                                    html_kolom += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        (
                                                                                            lst_nilai_lts[i].NilaiTugas.Trim() != ""
                                                                                            ? lst_nilai_lts[i].NilaiTugas
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                  "</td>";
                                                                    html_kolom += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        (
                                                                                            lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                            ? lst_nilai_lts[i].NilaiUH
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                  "</td>";
                                                                }
                                                                else
                                                                {
                                                                    html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                  "</td>";
                                                                    html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                  "</td>";
                                                                }
                                                            }
                                                        }
                                                        else if (m_sn.IsKelompokanKP && m_sn.IsKelompokanKPNoLTS)
                                                        //contoh al-quran yang di GABUNGKAN antara LK & UH nya berdasarkan aspek penilaian AP
                                                        {
                                                            Dictionary<int, string> lst_nilai_tugas = new Dictionary<int, string>();
                                                            Dictionary<int, string> lst_nilai_uh = new Dictionary<int, string>();

                                                            string s_nilai_tugas = "";
                                                            string s_nilai_uh = "";

                                                            int urutan_kd = 0;
                                                            decimal nilai = 0;

                                                            urutan_kd = 0;
                                                            foreach (var item__ in lst_nilai_lts_tugas.Select(m => m.NamaKD).Distinct())
                                                            {
                                                                urutan_kd++;
                                                                nilai = 0;
                                                                foreach (var item_ in lst_nilai_lts_tugas.FindAll(m => m.NamaKD == item__))
                                                                {
                                                                    nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                }
                                                                lst_nilai_tugas.Add(urutan_kd, Libs.GetFormatBilangan(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD));
                                                            }

                                                            urutan_kd = 0;
                                                            foreach (var item__ in lst_nilai_lts_uh.Select(m => m.NamaKD).Distinct())
                                                            {
                                                                urutan_kd++;
                                                                nilai = 0;
                                                                foreach (var item_ in lst_nilai_lts_uh.FindAll(m => m.NamaKD == item__))
                                                                {
                                                                    nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                }
                                                                lst_nilai_uh.Add(urutan_kd, Libs.GetFormatBilangan(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD));
                                                            }

                                                            int jumlah_kd = 0;
                                                            if (lst_nilai_uh.Count > lst_nilai_tugas.Count)
                                                            {
                                                                jumlah_kd = lst_nilai_uh.Count;
                                                            }
                                                            else if (lst_nilai_uh.Count < lst_nilai_tugas.Count)
                                                            {
                                                                jumlah_kd = lst_nilai_tugas.Count;
                                                            }
                                                            else
                                                            {
                                                                jumlah_kd = lst_nilai_tugas.Count;
                                                            }

                                                            lst_nilai_lts.Clear();
                                                            for (int i = 1; i <= jumlah_kd; i++)
                                                            {
                                                                s_nilai_tugas = "";
                                                                s_nilai_uh = "";
                                                                if (lst_nilai_tugas.ContainsKey(i))
                                                                {
                                                                    s_nilai_tugas = lst_nilai_tugas[i];
                                                                }
                                                                if (lst_nilai_uh.ContainsKey(i))
                                                                {
                                                                    s_nilai_uh = lst_nilai_uh[i];
                                                                }
                                                                lst_nilai_lts.Add(new NilaiLTS
                                                                {
                                                                    Urutan = i,
                                                                    NamaKD = "",
                                                                    NilaiTugas = s_nilai_tugas,
                                                                    NilaiUH = s_nilai_uh
                                                                });
                                                            }

                                                            id_nilai = 0;
                                                            for (int i = 0; i < jml_kolom; i++)
                                                            {
                                                                if (i < lst_nilai_lts.Count)
                                                                {
                                                                    html_kolom += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        (
                                                                                            lst_nilai_lts[i].NilaiTugas.Trim() != ""
                                                                                            ? lst_nilai_lts[i].NilaiTugas
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                  "</td>";
                                                                    html_kolom += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        (
                                                                                            lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                            ? lst_nilai_lts[i].NilaiUH
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                  "</td>";
                                                                }
                                                                else
                                                                {
                                                                    html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                  "</td>";
                                                                    html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                  "</td>";
                                                                }
                                                            }
                                                        }
                                                        else //mapel normal
                                                        {
                                                            lst_nilai_lts.Clear();
                                                            if (lst_nilai_1.Count > lst_nilai_2.Count)
                                                            {
                                                                id_nilai = 0;
                                                                foreach (var item in lst_nilai_1)
                                                                {
                                                                    string nilai_uh = "";
                                                                    for (int i = 0; i < lst_nilai_2_kd.Count; i++)
                                                                    {
                                                                        if (lst_nilai_2_kd[i] == lst_nilai_1_kd[id_nilai])
                                                                        {
                                                                            nilai_uh = lst_nilai_2[i];
                                                                            break;
                                                                        }
                                                                    }

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = (id_nilai + 1),
                                                                        NilaiTugas = lst_nilai_1[id_nilai],
                                                                        NilaiUH = nilai_uh
                                                                    });
                                                                    id_nilai++;
                                                                }
                                                            }
                                                            else if (lst_nilai_1.Count < lst_nilai_2.Count)
                                                            {
                                                                id_nilai = 0;
                                                                foreach (var item in lst_nilai_2)
                                                                {
                                                                    string nilai_tugas = "";
                                                                    for (int i = 0; i < lst_nilai_1_kd.Count; i++)
                                                                    {
                                                                        if (lst_nilai_1_kd[i] == lst_nilai_2_kd[id_nilai])
                                                                        {
                                                                            nilai_tugas = lst_nilai_1[i];
                                                                            break;
                                                                        }
                                                                    }

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = (id_nilai + 1),
                                                                        NilaiTugas = nilai_tugas,
                                                                        NilaiUH = lst_nilai_2[id_nilai],
                                                                    });
                                                                    id_nilai++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                id_nilai = 0;
                                                                foreach (var item in lst_nilai_2)
                                                                {
                                                                    string nilai_tugas = lst_nilai_1[id_nilai];

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = (id_nilai + 1),
                                                                        NilaiTugas = nilai_tugas,
                                                                        NilaiUH = lst_nilai_2[id_nilai],
                                                                    });
                                                                    id_nilai++;
                                                                }
                                                            }

                                                            id_nilai = 0;
                                                            for (int i = 0; i < jml_kolom; i++)
                                                            {
                                                                if (i < lst_nilai_lts.Count)
                                                                {
                                                                    html_kolom += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        (
                                                                                            lst_nilai_lts[i].NilaiTugas.Trim() != ""
                                                                                            ? Libs.GetFormatBilangan(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiTugas), Constantas.PEMBULATAN_DESIMAL_NILAI_SD)
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                  "</td>";
                                                                    html_kolom += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        (
                                                                                            lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                            ? Libs.GetFormatBilangan(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiUH), Constantas.PEMBULATAN_DESIMAL_NILAI_SD)
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                  "</td>";
                                                                }
                                                                else
                                                                {
                                                                    html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                  "</td>";
                                                                    html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                  "</td>";
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        id_nilai = 0;
                                                        for (int i = 1; i <= jml_kolom; i++)
                                                        {
                                                            html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                "&nbsp;" +
                                                                          "</td>";

                                                            html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                "&nbsp;" +
                                                                          "</td>";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    id_nilai = 0;
                                                    for (int i = 1; i <= jml_kolom; i++)
                                                    {
                                                        html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;" +
                                                                      "</td>";

                                                        html_kolom += "<td style=\"text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "&nbsp;" +
                                                                      "</td>";
                                                    }
                                                }
                                                
                                                if (m_desain.Poin.Trim() != "") ada_poin = true;
                                                string s_colspan_mapel = "";

                                                if (m_desain.Poin.Trim() == "")
                                                {
                                                    if (m_desain.Rel_Mapel.Trim() == "")
                                                    {
                                                        s_colspan_mapel = " colspan=\"" + (2 + (jml_kolom * 2)) + "\" ";
                                                        html_kolom = "";
                                                    }
                                                    else
                                                    {
                                                        s_colspan_mapel = " colspan=\"2\" ";
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_desain.Rel_Mapel.Trim() == "")
                                                    {
                                                        s_colspan_mapel = " colspan=\"" + (1 + (jml_kolom * 2)) + "\" ";
                                                        html_kolom = "";
                                                    }
                                                    else
                                                    {
                                                        s_colspan_mapel = "";
                                                    }
                                                }

                                                if (id < lst_desain_rapor_det.Count)
                                                {
                                                    if (lst_desain_rapor_det[id].Nomor == "" && m_desain.Nomor.Trim() != "")
                                                    {
                                                        s_border_bottom_nomor = " border-bottom-style: none; ";
                                                    }
                                                }
                                                if (id < lst_desain_rapor_det.Count)
                                                {
                                                    if (m_desain.Nomor.Trim() == "")
                                                    {
                                                        s_border_top_nomor = " border-top-style: none; ";
                                                        s_border_bottom_nomor = " border-bottom-style: none; ";
                                                    }
                                                }
                                                else if (id == lst_desain_rapor_det.Count)
                                                {
                                                    s_border_top_nomor = " border-top-style: none; ";
                                                }

                                                html_table_body += "<tr>" +
                                                                "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;" + s_border_bottom_nomor + s_border_top_nomor + "\">" +
                                                                    m_desain.Nomor +
                                                                "</td>" +
                                                                (
                                                                    m_desain.Poin.Trim() != ""
                                                                    ? "<td style=\"" + FONT_SIZE + "  width: 10px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; border-right-style: none; padding-left: 5px; padding-right: 5px;\">" +
                                                                        m_desain.Poin.Trim() +
                                                                      "</td>"
                                                                    : ""
                                                                ) +
                                                                "<td " + s_colspan_mapel + " style=\"" + FONT_SIZE + "  width: 200px; border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                    m_desain.NamaMapelRapor +
                                                                "</td>" +
                                                                html_kolom +
                                                            "</tr>";

                                                id++;
                                            }

                                            if (ada_poin)
                                            {
                                                html_table_header0 = html_table_header0.Replace(key_colspan_mapel, " colspan=\"2\" ");
                                            }
                                        }
                                    }

                                }
                                //end nilai akademik

                                //nilai sikap
                                string html_nilai_sikap = "";
                                string html_kolom_sikap = "";
                                int id_sikap = 1;
                                int jml_kolom_sikap = 4;

                                for (int i = jml_kolom_sikap; i > 0; i--)
                                {
                                    html_kolom_sikap += "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px;\">" +
                                                            i.ToString() +
                                                        "</td>";
                                }

                                List<Rapor_StrukturNilai> lst_sn_sikap = DAO_Rapor_StrukturNilai.GetMapelSikapByTAByKelas_Entity(tahun_ajaran, m_kelas.Kode.ToString());
                                foreach (var sn_sikap in lst_sn_sikap.FindAll(m => m.Semester == semester))
                                {
                                    List<Rapor_StrukturNilai_AP> lst_ap_sikap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(sn_sikap.Kode.ToString());
                                    foreach (var sn_ap_sikap in lst_ap_sikap)
                                    {
                                        List<Rapor_StrukturNilai_KD> lst_kd_sikap = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(sn_ap_sikap.Kode.ToString());
                                        foreach (var sn_kd_sikap in lst_kd_sikap)
                                        {

                                            List<Rapor_StrukturNilai_KP> lst_kp_sikap = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(sn_kd_sikap.Kode.ToString());
                                            foreach (var sn_kp_sikap in lst_kp_sikap)
                                            {
                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(sn_kp_sikap.Rel_Rapor_KomponenPenilaian.ToString());
                                                if (m_kp != null)
                                                {
                                                    if (m_kp.Nama != null)
                                                    {
                                                        string html_isi_nilai_sikap = "";
                                                        decimal nilai_sikap = DAO_Rapor_Sikap.GetNilaiSikap_Entity(
                                                                tahun_ajaran, semester, rel_kelas_det, rel_siswa,
                                                                sn_ap_sikap.Kode.ToString(), sn_kd_sikap.Kode.ToString(), sn_kp_sikap.Kode.ToString(),
                                                                sn_sikap.BobotSikapGuruKelas, sn_sikap.BobotSikapGuruMapel
                                                            );
                                                        int nilai_sikap_fix = 0;
                                                        if (nilai_sikap - (Math.Floor(nilai_sikap)) >= Convert.ToDecimal(0.5))
                                                        {
                                                            nilai_sikap_fix = Convert.ToInt16(Math.Ceiling(nilai_sikap));
                                                        }
                                                        else
                                                        {
                                                            nilai_sikap_fix = Convert.ToInt16(Math.Floor(nilai_sikap));
                                                        }
                                                        for (int i = jml_kolom_sikap; i > 0; i--)
                                                        {
                                                            html_isi_nilai_sikap += "<td style=\"" + FONT_SIZE + " width: 30px; text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px;\">" +
                                                                                        (
                                                                                            i == nilai_sikap_fix
                                                                                            ? "&#10004;" //"<i class=\"fa fa-check\"></i>"
                                                                                            : "&nbsp;"
                                                                                        ) +
                                                                                    "</td>";
                                                        }

                                                        html_nilai_sikap += "<tr>" +
                                                                                "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    id_sikap.ToString() +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                                    Libs.GetHTMLNoParagraphDiAwal(m_kp.Nama) +
                                                                                "</td>" +
                                                                                html_isi_nilai_sikap +
                                                                            "</tr>";
                                                        id_sikap++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                string html_sikap = "<tr>" +
                                                        "<td rowspan=\"2\" style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "No" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" rowspan=\"2\" style=\"" + FONT_SIZE + "  width: 200px; text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                            "Sikap dan Perilaku yang diamati" +
                                                        "</td>" +
                                                        "<td colspan=\"" + jml_kolom_sikap.ToString() + "\" style=\"" + FONT_SIZE + " width: 50px; text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                            "Pencapaian" +
                                                        "</td>" +
                                                        "<td rowspan=\"" + ((id_sikap - 1) + 2).ToString() + "\" colspan=\"" + (13 - (jml_kolom_sikap + 3)).ToString() + "\" style=\"" + FONT_SIZE + " text-align: left; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; border-style: none; padding-left: 20px;\">" +
                                                            "Keterangan Pencapaian : " +
                                                            "<br /><br />" +
                                                            "<div style=\"" + FONT_SIZE + "  margin: 0px; margin-bottom: 10px;\">" +
                                                                "4 : Sangat Baik" +
                                                            "</div>" +
                                                            "<div style=\"" + FONT_SIZE + "  margin: 0px; margin-bottom: 10px;\">" +
                                                                "3 : Baik" +
                                                            "</div>" +
                                                            "<div style=\"" + FONT_SIZE + "  margin: 0px; margin-bottom: 10px;\">" +
                                                                "2 : Cukup" +
                                                            "</div>" +
                                                            "<div style=\"" + FONT_SIZE + "  margin: 0px; margin-bottom: 10px;\">" +
                                                                "1 : Kurang" +
                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        html_kolom_sikap +
                                                    "</tr>" +
                                                    html_nilai_sikap;
                                //end nilai sikap

                                //absen
                                string s_sakit = "-";
                                string s_izin = "-";
                                string s_alpa = "-";
                                string s_terlambat = "-";

                                List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                        m_siswa.Kode.ToString(),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                    );
                                foreach (var absen in lst_absen)
                                {
                                    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                }

                                string html_absen = "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "No" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Kehadiran" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "Hari" +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "1" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Sakit" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_sakit +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "2" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Izin" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_izin +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "3" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Tanpa Keterangan" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_alpa +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "4" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Terlambat" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_terlambat +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>";
                                //end absen

                                html_table_header += html_table_header0 +
                                                     html_table_header1 +
                                                     html_table_header2;

                                string id_siswa = m_siswa.Kode.ToString().Replace("-", "_");

                                string s_nama_guru = "";
                                string s_tanggal = "";

                                Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                                if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                {
                                    s_nama_guru = m_mengetahui.NamaGuru;
                                    s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                }

                                if (s_nama_guru.Trim() == "")
                                {
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
                                                            s_nama_guru = m_pegawai.Nama;

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
                                }

                                string html_pembatas2 = "<tr>" +
                                                            "<td colspan=\"0\" style=\"border-style: none;\">" +
                                                                "&nbsp;" +
                                                            "</td>" +
                                                       "</tr>";

                                html +=
                                       html_header +                                       
                                       html_header_nama +
                                       "<table style=\"margin: 15px; border-collapse: collapse; " + (print_mode ? "" : " margin-left: 0px; margin-right: 0px; width: 100%; ") + "\">" +
                                            html_table_header +
                                            html_table_body +
                                            html_pembatas2 +
                                            html_sikap +
                                            html_pembatas2 +
                                            html_absen +
                                       "</table>" +                                                                              
                                       "<div id=\"qrcode_" + id_siswa + "\" style=\"float: right; width:250px; margin-top:0px; margin-right: 15px;" + FONT_SIZE + "\">" +
                                            "Jakarta, " + s_tanggal + "<br />" +
                                            "Guru Kelas" + 
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            s_nama_guru +
                                       "</div>" +
                                       (
                                        show_pagebreak
                                        ? "<div class=\"pagebreak\"></div>"
                                        : ""
                                       );

                                if (print_mode)
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 10pt;");
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

        public static string GetHTMLReport_KURTILAS(
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
            string html_table_header_mulok = "";
            string html_table_header0_mulok = "";
            string html_table_header1_mulok = "";
            string html_table_header2_mulok = "";
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

                            List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_det =
                                DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det);

                            List<Rapor_LTS_Mapel_Ext> lst_rapor_lts_mapel = DAO_Rapor_LTS_Mapel.GetByTABySMByKelasDet(tahun_ajaran, semester, rel_kelas_det);

                            if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode == new Guid(rel_siswa));
                            foreach (var m_siswa in lst_siswa)
                            {

                                //header
                                string html_header =
                                       "<table style=\"width: 100%; margin: 0px;\">" +
                                            "<tr>" +
                                                "<td style=\"width: 60px;\">" +
                                                    "<img src=\"" + page.ResolveUrl("~/Application_CLibs/images/logo.png") + "\" />" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + "\">" +
                                                    "SEKOLAH DASAR ISLAM AL IZHAR PONDOK LABU<br />" +
                                                    "JL. RS. Fatmawati Kav. 49 Telp. 7695542<br />" +
                                                    "Jakarta" +
                                                "</td>" +
                                            "</tr>" +
                                       "</table>" +
                                       "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                            "LAPORAN PERKEMBANGAN BELAJAR TENGAH SEMESTER  " + (semester == "1" ? "I" : "II") +
                                       "</div>" +
                                       "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                            "TAHUN PELAJARAN " + tahun_ajaran +
                                       "</div>" +
                                       "<table style=\"margin: 15px; width: 100%;\">" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + "\">" +
                                                    "Nama" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + "\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + "\">" +
                                                    Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td style=\"padding: 5px; width: 70px;" + FONT_SIZE + "\">" +
                                                    "Kelas" +
                                                "</td>" +
                                                "<td style=\"padding: 5px; width: 20px;" + FONT_SIZE + "\">" +
                                                    ":" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + "\">" +
                                                    m_kelas_det.Nama +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>";

                                string html_table_body = "";

                                //nilai akademik non mulok
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                    );

                                if (lst_desain_rapor.Count == 1)
                                {
                                    Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            int jml_kolom = 5;
                                            string key_colspan_mapel = "@colspan_mapel";
                                            html_table_header0 += "<tr>" +
                                                                      "<td rowspan=\"3\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"3\" " + key_colspan_mapel + " style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; width: 250px;\">" +
                                                                            "MATA PELAJARAN" +
                                                                      "</td>" +
                                                                      "<td colspan=\"" + (jml_kolom * 2).ToString() + "\" style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "PENCAPAIAN KOMPETENSI DASAR" +
                                                                      "</td>" +
                                                                  "</tr>";

                                            html_table_header1 += "<td colspan=\"5\" style=\"" + FONT_SIZE + "text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "PENGETAHUAN" +
                                                                  "</td>";
                                            html_table_header1 += "<td colspan=\"5\" style=\"" + FONT_SIZE + "text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "KETERAMPILAN" +
                                                                  "</td>";

                                            for (int i = 1; i <= jml_kolom; i++)
                                            {
                                                html_table_header2 += "<td style=\"" + FONT_SIZE + " text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            i.ToString() +
                                                                      "</td>";                                                
                                            }
                                            for (int i = 1; i <= jml_kolom; i++)
                                            {
                                                html_table_header2 += "<td style=\"" + FONT_SIZE + " text-align: center; width: 40px; max-width: 40px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            i.ToString() +
                                                                      "</td>";
                                            }
                                            
                                            html_table_header1 = "<tr>" +
                                                                    html_table_header1 +
                                                                 "</tr>";

                                            html_table_header2 = "<tr>" +
                                                                    html_table_header2 +
                                                                 "</tr>";

                                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                            bool ada_poin = false;
                                            int id = 1;
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                string html_kolom = "";
                                                string s_border_bottom_nomor = "";
                                                string s_border_top_nomor = "";

                                                //List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapelBySiswaForLTS_Entity(
                                                //        tahun_ajaran, semester, rel_kelas_det, m_desain.Rel_Mapel, m_siswa.Kode.ToString()
                                                //    );

                                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det =
                                                    lst_nilai_det.FindAll(
                                                            m0 => m0.Rel_Mapel == m_desain.Rel_Mapel &&
                                                                  m0.Rel_Siswa == m_siswa.Kode.ToString()
                                                        ).ToList();
                                                List<Rapor_LTS_Mapel_Ext> lst_rapor_lts_mapel_ =
                                                    lst_rapor_lts_mapel.FindAll(
                                                            m0 => m0.Rel_Mapel == m_desain.Rel_Mapel
                                                        ).ToList().OrderBy(m0 => m0.Urutan).ToList();

                                                List<string> lst_nilai_pengetahuan = new List<string>();
                                                List<string> lst_nilai_keterampilan = new List<string>();

                                                foreach (var item_lst_rapor_lts_mapel_ in lst_rapor_lts_mapel_)
                                                {
                                                    bool ada_item = false;
                                                    Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_AP);

                                                    foreach (var nilai_siswa_det in lst_nilai_siswa_det.FindAll(
                                                        m0 => m0.Rel_Rapor_StrukturNilai_AP == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_AP &&
                                                              m0.Rel_Rapor_StrukturNilai_KD == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KD &&
                                                              m0.Rel_Rapor_StrukturNilai_KP == item_lst_rapor_lts_mapel_.Rel_Rapor_StrukturNilai_KP
                                                    ))
                                                    {
                                                        if (m_sn_ap != null)
                                                        {
                                                            if (m_sn_ap.Poin != null)
                                                            {
                                                                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_sn_ap.Rel_Rapor_AspekPenilaian.ToString());
                                                                if (m_ap != null)
                                                                {
                                                                    if (m_ap.Nama != null)
                                                                    {
                                                                        ada_item = true;
                                                                        if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "PENGETAHUAN")
                                                                        {
                                                                            lst_nilai_pengetahuan.Add(nilai_siswa_det.Nilai);
                                                                        }
                                                                        else if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "KETERAMPILAN")
                                                                        {
                                                                            lst_nilai_keterampilan.Add(nilai_siswa_det.Nilai);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (!ada_item)
                                                    {
                                                        if (m_sn_ap != null)
                                                        {
                                                            if (m_sn_ap.Poin != null)
                                                            {
                                                                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_sn_ap.Rel_Rapor_AspekPenilaian.ToString());
                                                                if (m_ap != null)
                                                                {
                                                                    if (m_ap.Nama != null)
                                                                    {
                                                                        ada_item = true;
                                                                        if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "PENGETAHUAN")
                                                                        {
                                                                            lst_nilai_pengetahuan.Add("");
                                                                        }
                                                                        else if (Libs.GetHTMLSimpleText(m_ap.Nama).Trim().ToUpper() == "KETERAMPILAN")
                                                                        {
                                                                            lst_nilai_keterampilan.Add("");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //nilai pengetahuan
                                                int id_nilai = 0;
                                                for (int i = 1; i <= jml_kolom; i++)
                                                {
                                                    html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        (
                                                                            id_nilai < lst_nilai_pengetahuan.Count
                                                                            ? lst_nilai_pengetahuan[id_nilai]
                                                                            : "&nbsp;"
                                                                        ) +
                                                                  "</td>";
                                                    id_nilai++;
                                                }

                                                //nilai keterampilan
                                                id_nilai = 0;
                                                for (int i = 1; i <= jml_kolom; i++)
                                                {
                                                    html_kolom += "<td style=\"" + FONT_SIZE + "text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        (
                                                                            id_nilai < lst_nilai_keterampilan.Count
                                                                            ? lst_nilai_keterampilan[id_nilai]
                                                                            : "&nbsp;"
                                                                        ) +
                                                                  "</td>";
                                                    id_nilai++;
                                                }

                                                if (m_desain.Poin.Trim() != "") ada_poin = true;
                                                string s_colspan_mapel = "";

                                                if (m_desain.Poin.Trim() == "")
                                                {
                                                    if (m_desain.Rel_Mapel.Trim() == "")
                                                    {
                                                        s_colspan_mapel = " colspan=\"" + (2 + (jml_kolom * 2)) + "\" ";
                                                        html_kolom = "";
                                                    }
                                                    else
                                                    {
                                                        s_colspan_mapel = " colspan=\"2\" ";
                                                    }
                                                }
                                                else
                                                {
                                                    if (m_desain.Rel_Mapel.Trim() == "")
                                                    {
                                                        s_colspan_mapel = " colspan=\"" + (1 + (jml_kolom * 2)) + "\" ";
                                                        html_kolom = "";
                                                    }
                                                    else
                                                    {
                                                        s_colspan_mapel = "";
                                                    }
                                                }

                                                if (id < lst_desain_rapor_det.Count)
                                                {
                                                    if (lst_desain_rapor_det[id].Nomor == "" && m_desain.Nomor.Trim() != "")
                                                    {
                                                        s_border_bottom_nomor = " border-bottom-style: none; ";
                                                    }
                                                }
                                                if (id < lst_desain_rapor_det.Count)
                                                {
                                                    if (m_desain.Nomor.Trim() == "")
                                                    {
                                                        s_border_top_nomor = " border-top-style: none; ";
                                                        s_border_bottom_nomor = " border-bottom-style: none; ";
                                                    }
                                                }
                                                else if (id == lst_desain_rapor_det.Count)
                                                {
                                                    s_border_top_nomor = " border-top-style: none; ";
                                                }

                                                if (m_desain.Rel_Mapel.Trim() != "" && m_desain.Nomor.Trim() != "")
                                                {
                                                    html_table_body += "<tr>" +
                                                                            "<td style=\"" + FONT_SIZE + "text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;" + s_border_bottom_nomor + s_border_top_nomor + "\">" +
                                                                                m_desain.Nomor +
                                                                            "</td>" +
                                                                            (
                                                                                m_desain.Poin.Trim() != ""
                                                                                ? "<td style=\"" + FONT_SIZE + "width: 10px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; border-right-style: none;\">" +
                                                                                    m_desain.Poin.Trim() +
                                                                                  "</td>"
                                                                                : ""
                                                                            ) +
                                                                            "<td " + s_colspan_mapel + " style=\"" + FONT_SIZE + "border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                                m_desain.NamaMapelRapor +
                                                                            "</td>" +
                                                                            html_kolom +
                                                                        "</tr>";
                                                }

                                                id++;
                                            }

                                            if (ada_poin)
                                            {
                                                html_table_header0 = html_table_header0.Replace(key_colspan_mapel, " colspan=\"2\" ");
                                            }
                                        }
                                    }

                                }
                                //end nilai akademik non mulok

                                string html_table_body_mulok = "";

                                //nilai akademik mulok
                                lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                    );

                                if (lst_desain_rapor.Count == 1)
                                {
                                    Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            int jml_kolom = 5;
                                            string key_colspan_mapel = "@colspan_mapel";

                                            if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2019) //tahun ajaran >= 2019/2020
                                            {
                                                html_table_header0_mulok
                                                                  += "<tr>" +
                                                                      "<td rowspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"2\" " + key_colspan_mapel + " style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; width: 250px;\">" +
                                                                            "MUATAN LOKAL" +
                                                                      "</td>" +
                                                                      "<td colspan=\"" + (jml_kolom).ToString() + "\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "PENCAPAIAN KOMPETENSI DASAR" +
                                                                      "</td>";
                                                for (int i = 1; i <= jml_kolom; i++)
                                                {
                                                    html_table_header0_mulok += "<td style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: none; border-width: 0px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>";
                                                }
                                                html_table_header0_mulok += "</tr>";

                                                for (int i = 1; i <= jml_kolom; i++)
                                                {
                                                    html_table_header1_mulok += "<td style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    i.ToString() +
                                                                                "</td>";
                                                }
                                                for (int i = 1; i <= jml_kolom; i++)
                                                {
                                                    html_table_header1_mulok += "<td style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: none; border-width: 0px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>";
                                                }

                                                html_table_header1_mulok = "<tr>" +
                                                                                html_table_header1_mulok +
                                                                           "</tr>";
                                            }
                                            else if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) == 2018) //tahun ajaran 2018/2019
                                            {
                                                html_table_header0_mulok
                                                                  += "<tr>" +
                                                                      "<td rowspan=\"3\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td rowspan=\"3\" " + key_colspan_mapel + " style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; width: 250px;\">" +
                                                                            "MUATAN LOKAL" +
                                                                      "</td>" +
                                                                      "<td colspan=\"" + (jml_kolom * 2).ToString() + "\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                            "PENCAPAIAN KOMPETENSI DASAR" +
                                                                      "</td>" +
                                                                  "</tr>";

                                                for (int i = 1; i <= jml_kolom; i++)
                                                {
                                                    html_table_header1_mulok += "<td colspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    i.ToString() +
                                                                                "</td>";

                                                    html_table_header2_mulok += "<td style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-size: 8pt;\">" +
                                                                                    "Tugas" +
                                                                                "</td>";
                                                    html_table_header2_mulok += "<td style=\"" + FONT_SIZE + " text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-size: 8pt;\">" +
                                                                                    "UH" +
                                                                                "</td>";
                                                }

                                                html_table_header1_mulok = "<tr>" +
                                                                                html_table_header1_mulok +
                                                                           "</tr>";

                                                html_table_header2_mulok = "<tr>" +
                                                                                html_table_header2_mulok +
                                                                           "</tr>";
                                            }

                                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).
                                                FindAll(m => m.Poin.Trim() != "" && m.Nomor.Trim() == "").
                                                OrderBy(m => m.Urutan).ToList();
                                            bool ada_poin = false;
                                            int id = 1;
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                string html_kolom_mulok = "";
                                                
                                                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapelBySiswaForLTS_Entity(
                                                        tahun_ajaran, semester, rel_kelas_det, m_desain.Rel_Mapel, m_siswa.Kode.ToString()
                                                    );


                                                Rapor_StrukturNilai m_sn = null;

                                                List<string> lst_nilai_1 = new List<string>(); //nilai tugas
                                                List<string> lst_nilai_1_kd = new List<string>(); //nilai tugas KD
                                                List<string> lst_nilai_1_nama_kd = new List<string>(); //nama tugas KD
                                                List<string> lst_nilai_1_bobot_kp = new List<string>(); //bobot tugas KP
                                                List<string> lst_nilai_2 = new List<string>(); //nilai uh
                                                List<string> lst_nilai_2_kd = new List<string>(); //nilai uh KD
                                                List<string> lst_nilai_2_nama_kd = new List<string>(); //nama uh KD
                                                List<string> lst_nilai_2_bobot_kp = new List<string>(); //bobot tugas KP

                                                List<NilaiLTS> lst_nilai_lts = new List<NilaiLTS>();
                                                List<Nilai_LTS> lst_nilai_lts_tugas = new List<Nilai_LTS>();
                                                List<Nilai_LTS> lst_nilai_lts_uh = new List<Nilai_LTS>();

                                                int id_nilai = 0;

                                                foreach (var nilai_siswa_det in lst_nilai_siswa_det)
                                                {
                                                    Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(nilai_siswa_det.Rel_Rapor_StrukturNilai_KD.ToString());
                                                    Rapor_StrukturNilai_KP m_sn_kp = DAO_Rapor_StrukturNilai_KP.GetByID_Entity(nilai_siswa_det.Rel_Rapor_StrukturNilai_KP.ToString());

                                                    if (m_sn_kp != null && m_sn_kd != null)
                                                    {
                                                        if (m_sn_kp.Jenis != null && m_sn_kd.JenisPerhitungan != null)
                                                        {
                                                            Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(nilai_siswa_det.Rel_Rapor_StrukturNilai_AP);
                                                            if (m_sn_ap.Poin != null)
                                                            {
                                                                m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(m_sn_ap.Rel_Rapor_StrukturNilai.ToString());
                                                            }

                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                            Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                            if (m_kp != null && m_kd != null)
                                                            {
                                                                if (m_kp.Nama != null && m_kd.Nama != null)
                                                                {
                                                                    if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "LK")
                                                                    {
                                                                        lst_nilai_1_kd.Add(m_sn_kd.Urutan.ToString());
                                                                        lst_nilai_1_nama_kd.Add(m_kd.Nama.ToString());
                                                                        lst_nilai_1.Add(nilai_siswa_det.Nilai);
                                                                        lst_nilai_1_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                        lst_nilai_lts_tugas.Add(new Nilai_LTS
                                                                        {
                                                                            Urutan = m_sn_kd.Urutan,
                                                                            NamaKD = m_kd.Nama,
                                                                            Nilai = nilai_siswa_det.Nilai,
                                                                            BobotKP = m_sn_kp.BobotNK
                                                                        });
                                                                    }
                                                                    else if (
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "PH" ||
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "UH" ||
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "NILAI" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "UH" ||                                                                        
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "PH")
                                                                    {
                                                                        lst_nilai_2_kd.Add(m_sn_kd.Urutan.ToString());
                                                                        lst_nilai_2_nama_kd.Add(m_kd.Nama.ToString());
                                                                        lst_nilai_2.Add(nilai_siswa_det.Nilai);
                                                                        lst_nilai_2_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                        lst_nilai_lts_uh.Add(new Nilai_LTS
                                                                        {
                                                                            Urutan = m_sn_kd.Urutan,
                                                                            NamaKD = m_kd.Nama,
                                                                            Nilai = nilai_siswa_det.Nilai,
                                                                            BobotKP = m_sn_kp.BobotNK
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) >= 2019) //tahun ajaran >= 2019/2020
                                                {
                                                    if (m_sn != null)
                                                    {
                                                        if (m_sn.TahunAjaran != null)
                                                        {

                                                            if (m_sn.IsKelompokanKP) //contoh al-quran
                                                            {
                                                                List<int> lst_urutan_kd_tugas = lst_nilai_lts_tugas.Select(m => m.Urutan).Distinct().ToList();
                                                                List<int> lst_urutan_kd_uh = lst_nilai_lts_uh.Select(m => m.Urutan).Distinct().ToList();
                                                                List<int> lst_gabungan = new List<int>();

                                                                foreach (var item in lst_urutan_kd_tugas)
                                                                {
                                                                    if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                    {
                                                                        lst_gabungan.Add(item);
                                                                    }
                                                                }
                                                                foreach (var item in lst_urutan_kd_uh)
                                                                {
                                                                    if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                    {
                                                                        lst_gabungan.Add(item);
                                                                    }
                                                                }
                                                                lst_nilai_lts.Clear();
                                                                int urutan = 1;
                                                                foreach (var item in lst_gabungan.OrderBy(m => m).ToList())
                                                                {
                                                                    string s_nilai_tugas = "";
                                                                    string s_nilai_uh = "";

                                                                    decimal nilai = 0;
                                                                    foreach (var item_ in lst_nilai_lts_tugas.FindAll(m => m.Urutan == item))
                                                                    {
                                                                        nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                    }
                                                                    s_nilai_tugas = Math.Round(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

                                                                    nilai = 0;
                                                                    foreach (var item_ in lst_nilai_lts_uh.FindAll(m => m.Urutan == item))
                                                                    {
                                                                        nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                    }
                                                                    s_nilai_uh = Math.Round(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = urutan,
                                                                        NamaKD = "",
                                                                        NilaiTugas = s_nilai_tugas,
                                                                        NilaiUH = s_nilai_uh
                                                                    });
                                                                    urutan++;
                                                                }

                                                                id_nilai = 0;
                                                                for (int i = 0; i < jml_kolom; i++)
                                                                {
                                                                    if (i < lst_nilai_lts.Count)
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    (
                                                                                                        lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                                        ? Math.Round(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiUH), 0).ToString()
                                                                                                        : "&nbsp;"
                                                                                                    ) +
                                                                                            "</td>";
                                                                    }
                                                                    else
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                "&nbsp;" +
                                                                                            "</td>";
                                                                    }
                                                                }
                                                                for (int i = 0; i < jml_kolom; i++)
                                                                {
                                                                    html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: none; border-width: 0px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                            "&nbsp;" +
                                                                                        "</td>";
                                                                }
                                                            }
                                                            else //mapel normal
                                                            {
                                                                lst_nilai_lts.Clear();
                                                                if (lst_nilai_1.Count > lst_nilai_2.Count)
                                                                {
                                                                    id_nilai = 0;
                                                                    foreach (var item in lst_nilai_1)
                                                                    {
                                                                        string nilai_uh = "";
                                                                        for (int i = 0; i < lst_nilai_2_kd.Count; i++)
                                                                        {
                                                                            if (lst_nilai_2_kd[i] == lst_nilai_1_kd[id_nilai])
                                                                            {
                                                                                nilai_uh = lst_nilai_2[i];
                                                                                break;
                                                                            }
                                                                        }

                                                                        lst_nilai_lts.Add(new NilaiLTS
                                                                        {
                                                                            Urutan = (id_nilai + 1),
                                                                            NilaiTugas = lst_nilai_1[id_nilai],
                                                                            NilaiUH = nilai_uh
                                                                        });
                                                                        id_nilai++;
                                                                    }
                                                                }
                                                                else if (lst_nilai_1.Count < lst_nilai_2.Count)
                                                                {
                                                                    id_nilai = 0;
                                                                    foreach (var item in lst_nilai_2)
                                                                    {
                                                                        string nilai_tugas = "";
                                                                        for (int i = 0; i < lst_nilai_1_kd.Count; i++)
                                                                        {
                                                                            if (lst_nilai_1_kd[i] == lst_nilai_2_kd[id_nilai])
                                                                            {
                                                                                nilai_tugas = lst_nilai_1[i];
                                                                                break;
                                                                            }
                                                                        }

                                                                        lst_nilai_lts.Add(new NilaiLTS
                                                                        {
                                                                            Urutan = (id_nilai + 1),
                                                                            NilaiTugas = nilai_tugas,
                                                                            NilaiUH = lst_nilai_2[id_nilai],
                                                                        });
                                                                        id_nilai++;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    id_nilai = 0;
                                                                    foreach (var item in lst_nilai_2)
                                                                    {
                                                                        string nilai_tugas = lst_nilai_1[id_nilai];

                                                                        lst_nilai_lts.Add(new NilaiLTS
                                                                        {
                                                                            Urutan = (id_nilai + 1),
                                                                            NilaiTugas = nilai_tugas,
                                                                            NilaiUH = lst_nilai_2[id_nilai],
                                                                        });
                                                                        id_nilai++;
                                                                    }
                                                                }

                                                                id_nilai = 0;
                                                                for (int i = 0; i < jml_kolom; i++)
                                                                {
                                                                    if (i < lst_nilai_lts.Count)
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    (
                                                                                                        lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                                        ? lst_nilai_lts[i].NilaiUH
                                                                                                        : "&nbsp;"
                                                                                                    ) +
                                                                                            "</td>";
                                                                    }
                                                                    else
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                "&nbsp;" +
                                                                                            "</td>";
                                                                    }
                                                                }
                                                                for (int i = 0; i < jml_kolom; i++)
                                                                {
                                                                    html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: none; border-width: 0px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                            "&nbsp;" +
                                                                                        "</td>";
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            id_nilai = 0;
                                                            for (int i = 1; i <= jml_kolom; i++)
                                                            {
                                                                html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";
                                                            }
                                                            for (int i = 0; i < jml_kolom; i++)
                                                            {
                                                                html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: none; border-width: 0px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        id_nilai = 0;
                                                        for (int i = 1; i <= jml_kolom; i++)
                                                        {
                                                            html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                "</td>";
                                                        }
                                                        for (int i = 0; i < jml_kolom; i++)
                                                        {
                                                            html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: none; border-width: 0px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    "&nbsp;" +
                                                                                "</td>";
                                                        }
                                                    }

                                                    if (m_desain.Poin.Trim() != "") ada_poin = true;
                                                    string s_colspan_mapel = "";

                                                    if (m_desain.Poin.Trim() == "")
                                                    {
                                                        if (m_desain.Rel_Mapel.Trim() == "")
                                                        {
                                                            s_colspan_mapel = " colspan=\"" + (2 + (jml_kolom)) + "\" ";
                                                            html_kolom_mulok = "";
                                                        }
                                                        else
                                                        {
                                                            s_colspan_mapel = " colspan=\"2\" ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (m_desain.Rel_Mapel.Trim() == "")
                                                        {
                                                            s_colspan_mapel = " colspan=\"" + (1 + (jml_kolom)) + "\" ";
                                                            html_kolom_mulok = "";
                                                        }
                                                        else
                                                        {
                                                            s_colspan_mapel = "";
                                                        }
                                                    }

                                                    html_table_body_mulok += "<tr>" +
                                                                                "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    id +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; width: 250px;\">" +
                                                                                    m_desain.NamaMapelRapor +
                                                                                "</td>" +
                                                                                html_kolom_mulok +
                                                                            "</tr>";
                                                }
                                                else if (Libs.GetStringToInteger(tahun_ajaran.Substring(0, 4)) == 2018) //tahun ajaran 2018/2019
                                                {
                                                    if (m_sn != null)
                                                    {
                                                        if (m_sn.TahunAjaran != null)
                                                        {

                                                            if (m_sn.IsKelompokanKP) //contoh al-quran
                                                            {
                                                                List<int> lst_urutan_kd_tugas = lst_nilai_lts_tugas.Select(m => m.Urutan).Distinct().ToList();
                                                                List<int> lst_urutan_kd_uh = lst_nilai_lts_uh.Select(m => m.Urutan).Distinct().ToList();
                                                                List<int> lst_gabungan = new List<int>();

                                                                foreach (var item in lst_urutan_kd_tugas)
                                                                {
                                                                    if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                    {
                                                                        lst_gabungan.Add(item);
                                                                    }
                                                                }
                                                                foreach (var item in lst_urutan_kd_uh)
                                                                {
                                                                    if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                    {
                                                                        lst_gabungan.Add(item);
                                                                    }
                                                                }
                                                                lst_nilai_lts.Clear();
                                                                int urutan = 1;
                                                                foreach (var item in lst_gabungan.OrderBy(m => m).ToList())
                                                                {
                                                                    string s_nilai_tugas = "";
                                                                    string s_nilai_uh = "";

                                                                    decimal nilai = 0;
                                                                    foreach (var item_ in lst_nilai_lts_tugas.FindAll(m => m.Urutan == item))
                                                                    {
                                                                        nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                    }
                                                                    s_nilai_tugas = Math.Round(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

                                                                    nilai = 0;
                                                                    foreach (var item_ in lst_nilai_lts_uh.FindAll(m => m.Urutan == item))
                                                                    {
                                                                        nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                    }
                                                                    s_nilai_uh = Math.Round(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = urutan,
                                                                        NamaKD = "",
                                                                        NilaiTugas = s_nilai_tugas,
                                                                        NilaiUH = s_nilai_uh
                                                                    });
                                                                    urutan++;
                                                                }

                                                                id_nilai = 0;
                                                                for (int i = 0; i < jml_kolom; i++)
                                                                {
                                                                    if (i < lst_nilai_lts.Count)
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    (
                                                                                                        lst_nilai_lts[i].NilaiTugas.Trim() != ""
                                                                                                        ? Math.Round(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiTugas), 0).ToString()
                                                                                                        : "&nbsp;"
                                                                                                    ) +
                                                                                            "</td>";
                                                                        html_kolom_mulok += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    (
                                                                                                        lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                                        ? Math.Round(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiUH), 0).ToString()
                                                                                                        : "&nbsp;"
                                                                                                    ) +
                                                                                            "</td>";
                                                                    }
                                                                    else
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    "&nbsp;" +
                                                                                            "</td>";
                                                                        html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    "&nbsp;" +
                                                                                            "</td>";
                                                                    }
                                                                }
                                                            }
                                                            else //mapel normal
                                                            {
                                                                lst_nilai_lts.Clear();
                                                                if (lst_nilai_1.Count > lst_nilai_2.Count)
                                                                {
                                                                    id_nilai = 0;
                                                                    foreach (var item in lst_nilai_1)
                                                                    {
                                                                        string nilai_uh = "";
                                                                        for (int i = 0; i < lst_nilai_2_kd.Count; i++)
                                                                        {
                                                                            if (lst_nilai_2_kd[i] == lst_nilai_1_kd[id_nilai])
                                                                            {
                                                                                nilai_uh = lst_nilai_2[i];
                                                                                break;
                                                                            }
                                                                        }

                                                                        lst_nilai_lts.Add(new NilaiLTS
                                                                        {
                                                                            Urutan = (id_nilai + 1),
                                                                            NilaiTugas = lst_nilai_1[id_nilai],
                                                                            NilaiUH = nilai_uh
                                                                        });
                                                                        id_nilai++;
                                                                    }
                                                                }
                                                                else if (lst_nilai_1.Count < lst_nilai_2.Count)
                                                                {
                                                                    id_nilai = 0;
                                                                    foreach (var item in lst_nilai_2)
                                                                    {
                                                                        string nilai_tugas = "";
                                                                        for (int i = 0; i < lst_nilai_1_kd.Count; i++)
                                                                        {
                                                                            if (lst_nilai_1_kd[i] == lst_nilai_2_kd[id_nilai])
                                                                            {
                                                                                nilai_tugas = lst_nilai_1[i];
                                                                                break;
                                                                            }
                                                                        }

                                                                        lst_nilai_lts.Add(new NilaiLTS
                                                                        {
                                                                            Urutan = (id_nilai + 1),
                                                                            NilaiTugas = nilai_tugas,
                                                                            NilaiUH = lst_nilai_2[id_nilai],
                                                                        });
                                                                        id_nilai++;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    id_nilai = 0;
                                                                    foreach (var item in lst_nilai_2)
                                                                    {
                                                                        string nilai_tugas = lst_nilai_1[id_nilai];

                                                                        lst_nilai_lts.Add(new NilaiLTS
                                                                        {
                                                                            Urutan = (id_nilai + 1),
                                                                            NilaiTugas = nilai_tugas,
                                                                            NilaiUH = lst_nilai_2[id_nilai],
                                                                        });
                                                                        id_nilai++;
                                                                    }
                                                                }

                                                                id_nilai = 0;
                                                                for (int i = 0; i < jml_kolom; i++)
                                                                {
                                                                    if (i < lst_nilai_lts.Count)
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    (
                                                                                                        lst_nilai_lts[i].NilaiTugas.Trim() != ""
                                                                                                        ? lst_nilai_lts[i].NilaiTugas
                                                                                                        : "&nbsp;"
                                                                                                    ) +
                                                                                            "</td>";
                                                                        html_kolom_mulok += "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                    (
                                                                                                        lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                                        ? lst_nilai_lts[i].NilaiUH
                                                                                                        : "&nbsp;"
                                                                                                    ) +
                                                                                            "</td>";
                                                                    }
                                                                    else
                                                                    {
                                                                        html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                "&nbsp;" +
                                                                                            "</td>";
                                                                        html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                                "&nbsp;" +
                                                                                            "</td>";
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            id_nilai = 0;
                                                            for (int i = 1; i <= jml_kolom; i++)
                                                            {
                                                                html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";

                                                                html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        id_nilai = 0;
                                                        for (int i = 1; i <= jml_kolom; i++)
                                                        {
                                                            html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                "</td>";

                                                            html_kolom_mulok += "<td style=\"text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                        "&nbsp;" +
                                                                                "</td>";
                                                        }
                                                    }

                                                    if (m_desain.Poin.Trim() != "") ada_poin = true;
                                                    string s_colspan_mapel = "";

                                                    if (m_desain.Poin.Trim() == "")
                                                    {
                                                        if (m_desain.Rel_Mapel.Trim() == "")
                                                        {
                                                            s_colspan_mapel = " colspan=\"" + (2 + (jml_kolom * 2)) + "\" ";
                                                            html_kolom_mulok = "";
                                                        }
                                                        else
                                                        {
                                                            s_colspan_mapel = " colspan=\"2\" ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (m_desain.Rel_Mapel.Trim() == "")
                                                        {
                                                            s_colspan_mapel = " colspan=\"" + (1 + (jml_kolom * 2)) + "\" ";
                                                            html_kolom_mulok = "";
                                                        }
                                                        else
                                                        {
                                                            s_colspan_mapel = "";
                                                        }
                                                    }

                                                    html_table_body_mulok += "<tr>" +
                                                                                "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                                    id +
                                                                                "</td>" +
                                                                                "<td colspan=\"2\" style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; width: 250px;\">" +
                                                                                    m_desain.NamaMapelRapor +
                                                                                "</td>" +
                                                                                html_kolom_mulok +
                                                                            "</tr>";
                                                }

                                                id++;
                                            }

                                            if (ada_poin)
                                            {
                                                html_table_header0_mulok = html_table_header0_mulok.Replace(key_colspan_mapel, " colspan=\"2\" ");
                                            }
                                        }
                                    }

                                }
                                //end nilai akademik mulok

                                //absen
                                string s_sakit = "-";
                                string s_izin = "-";
                                string s_alpa = "-";
                                string s_terlambat = "-";

                                List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                        m_siswa.Kode.ToString(),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                    );
                                foreach (var absen in lst_absen)
                                {
                                    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                }

                                string html_absen = "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "No" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Kehadiran" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "Hari" +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "1" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Sakit" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_sakit +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "2" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Izin" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_izin +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "3" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Tanpa Keterangan" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_alpa +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "4" +
                                                        "</td>" +
                                                        "<td colspan=\"2\" style=\"" + FONT_SIZE + "  text-align: left; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 5px; padding-left: 5px; \">" +
                                                            "Terlambat" +
                                                        "</td>" +
                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            s_terlambat +
                                                        "</td>" +
                                                        "<td colspan=\"9\" style=\"border-style: none; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                            "&nbsp;" +
                                                        "</td>" +
                                                    "</tr>";
                                //end absen

                                html_table_header += html_table_header0 +
                                                     html_table_header1 +
                                                     html_table_header2;

                                html_table_header_mulok += html_table_header0_mulok +
                                                           html_table_header1_mulok +
                                                           html_table_header2_mulok;

                                string id_siswa = m_siswa.Kode.ToString().Replace("-", "_");

                                string s_nama_guru = "";
                                string s_tanggal = "";

                                Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                                if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                {
                                    s_nama_guru = m_mengetahui.NamaGuru;
                                    s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                }

                                if (s_nama_guru.Trim() == "")
                                {
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
                                                            s_nama_guru = m_pegawai.Nama;

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
                                }

                                string html_pembatas = "<tr>" +
                                                            "<td colspan=\"0\" style=\"border-style: none;\">" +
                                                                "&nbsp;" +
                                                            "</td>" +
                                                       "</tr>";
                                string html_pembatas2 = "<tr>" +
                                                            "<td colspan=\"0\" style=\"border-style: none;\">" +
                                                                "&nbsp;" +
                                                            "</td>" +
                                                       "</tr>";

                                html +=
                                       html_header +                                       
                                       "<table style=\"margin: 15px; border-collapse: collapse; " + (print_mode ? "" : " margin-left: 0px; margin-right: 0px; width: 100%; ") + "\">" +
                                            html_table_header +
                                            html_table_body +
                                            html_pembatas +
                                            html_table_header_mulok +
                                            html_table_body_mulok +
                                            html_pembatas2 +
                                            html_absen +
                                       "</table>" +                                       
                                       "<div id=\"qrcode_" + id_siswa + "\" style=\"float: right; width:250px; margin-top:30px; margin-right: 15px;" + FONT_SIZE + "\">" +
                                            "Jakarta, " + s_tanggal + "<br />" +
                                            "Guru Kelas" + "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            "<br />" +
                                            s_nama_guru +
                                       "</div>" +
                                       (
                                        show_pagebreak
                                        ? "<div class=\"pagebreak\"></div>"
                                        : ""
                                       );

                                if (print_mode)
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 10pt;");
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

        public static string GetHTMLBahasaKD(
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
            string html_table_header2 = "";
            
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

                            if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode == new Guid(rel_siswa));
                            foreach (var m_siswa in lst_siswa)
                            {

                                //header
                                string html_header =
                                       "<table style=\"width: 100%; margin: 0px; margin-top: 45px;\">" +
                                            "<tr>" +
                                                "<td style=\"width: 60px;\">" +
                                                    "<img src=\"" + page.ResolveUrl("~/Application_CLibs/images/logo.png") + "\" />" +
                                                "</td>" +
                                                "<td style=\"padding: 5px;" + FONT_SIZE + " \">" +
                                                    "SEKOLAH DASAR ISLAM AL IZHAR PONDOK LABU<br />" +
                                                    "JL. RS. Fatmawati Kav. 49 Telp. 7695542<br />" +
                                                    "Jakarta" +
                                                "</td>" +
                                            "</tr>" +
                                       "</table>";

                                string html_table_body = "";

                                //nilai akademik
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                    );

                                if (lst_desain_rapor.Count == 1)
                                {
                                    Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            html_table_header0 += "<tr>" +
                                                                      "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "No" +
                                                                      "</td>" +
                                                                      "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "Mata Pelajaran" +
                                                                      "</td>" +
                                                                      "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "Kompetensi Dasar" +
                                                                      "</td>" +
                                                                      "<td style=\"" + FONT_SIZE + " text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                        "KKM" +
                                                                      "</td>" +
                                                                  "</tr>";

                                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                            int id = 1;

                                            List<string> lst_kd = new List<string>();
                                            lst_kd.Clear();
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                string html_deskripsi_kd = "";
                                                string kkm = "";

                                                List<Rapor_LTS_Mapel> lst_lts_mapel = DAO_Rapor_LTS_Mapel.GetByTABySMByMapelByKelasDet(
                                                        tahun_ajaran, semester, m_desain.Rel_Mapel, rel_kelas_det
                                                    ).OrderBy(m => m.Urutan).ToList();

                                                foreach (var item in lst_lts_mapel)
                                                {
                                                    Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(
                                                            item.Rel_Rapor_StrukturNilai_KD.ToString()
                                                        );

                                                    if (m_sn_kd != null)
                                                    {
                                                        if (m_sn_kd.Poin != null)
                                                        {

                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                            if (m_kd != null)
                                                            {
                                                                if (m_kd.Nama != null)
                                                                {
                                                                    if (lst_kd.FindAll(m => m == Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)).ToString().Trim().ToLower()).Count == 0)
                                                                    {
                                                                        lst_kd.Add(Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)).ToString().Trim().ToLower());

                                                                        html_deskripsi_kd += "<li>" +
                                                                                                Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)) +
                                                                                             "</li>";
                                                                    }
                                                                }
                                                            }

                                                            Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString());
                                                            if (m_sn_ap != null)
                                                            {
                                                                if (m_sn_ap.Poin != null)
                                                                {

                                                                    Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(m_sn_ap.Rel_Rapor_StrukturNilai.ToString());
                                                                    if (m_sn != null)
                                                                    {

                                                                        if (m_sn.TahunAjaran != null)
                                                                        {
                                                                            if (m_sn.KKM > 10)
                                                                            {
                                                                                kkm = Math.Round(m_sn.KKM, 0).ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                kkm = Math.Round(m_sn.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();
                                                                            }                                                                            
                                                                        }

                                                                    }

                                                                }
                                                            }

                                                        }
                                                    }
                                                }

                                                if (html_deskripsi_kd.Trim() != "")
                                                {
                                                    html_deskripsi_kd = "<ol style=\"margin: 0px;\">" +
                                                                            html_deskripsi_kd +
                                                                        "</ol>";
                                                }

                                                if (m_desain.Rel_Mapel.Trim() == "")
                                                {
                                                    html_table_body += "<tr>" +
                                                                            "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; vertical-align: top;\">" +
                                                                                "" +
                                                                            "</td>" +
                                                                            "<td colspan=\"3\" style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                                m_desain.NamaMapelRapor +
                                                                            "</td>" +
                                                                        "</tr>";
                                                }
                                                else
                                                {
                                                    html_table_body += "<tr>" +
                                                                            "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; vertical-align: top;\">" +
                                                                                id.ToString() +
                                                                            "</td>" +
                                                                            "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                                m_desain.NamaMapelRapor +
                                                                            "</td>" +
                                                                            "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                                html_deskripsi_kd +
                                                                            "</td>" +
                                                                            "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                                kkm +
                                                                            "</td>" +
                                                                        "</tr>";
                                                }                                                

                                                if(m_desain.Rel_Mapel.Trim() != "") id++;
                                            }
                                        }
                                    }

                                }
                                //end nilai akademik

                                html_table_header += html_table_header0 +
                                                     html_table_header1 +
                                                     html_table_header2;

                                string id_siswa = m_siswa.Kode.ToString().Replace("-", "_");

                                string s_nama_guru = "";
                                string s_tanggal = "";

                                Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                                if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                                {
                                    s_nama_guru = m_mengetahui.NamaGuru;
                                    s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                                }

                                if (s_nama_guru.Trim() == "")
                                {
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
                                                            s_nama_guru = m_pegawai.Nama;

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
                                }

                                html +=
                                       html_header +
                                       "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                            "Kompetensi Dasar Laporan Tengah Semester " + (semester == "1" ? "I" : "II") +
                                       "</div>" +
                                       "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                            "Kelas " + m_kelas.Nama + " " +
                                            "Tahun Pelajaran " + tahun_ajaran +
                                       "</div>" +
                                       "<table class=\"print-friendly\" style=\"margin: 15px; border-collapse: collapse;\">" +
                                            html_table_header +
                                            html_table_body +
                                            "<tfoot>" +
                                                "<tr>" +
                                                    "<td colspan=\"13\">" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</tfoot>" +
                                       "</table>" +
                                       (
                                        show_pagebreak
                                        ? "<div class=\"pagebreak\"></div>"
                                        : ""
                                       );

                                if (print_mode)
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 12pt;");
                                }
                                else
                                {
                                    html = html.Replace(FONT_SIZE, "font-size: 11pt;");
                                }

                            }

                        }

                    }

                }
            }

            return html;
        }

        public static string GetHTMLBahasaKDNoCheck_KURTILAS(
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
            string html_table_header2 = "";

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

                            //header
                            string html_header =
                                   "<table style=\"width: 100%; margin: 0px; margin-top: 45px;\">" +
                                        "<tr>" +
                                            "<td style=\"width: 60px;\">" +
                                                "<img src=\"" + page.ResolveUrl("~/Application_CLibs/images/logo.png") + "\" />" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; padding: 5px;" + FONT_SIZE + " \">" +
                                                "Kompetensi Dasar Laporan Tengah Semester " + (semester == "1" ? "I" : "II") + "<br />" +
                                                "Kelas " + m_kelas.Nama + " Tahun Pelajaran " + tahun_ajaran +
                                            "</td>" +
                                        "</tr>" +
                                   "</table>";

                            string html_table_body = "";

                            //nilai akademik
                            List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                );

                            if (lst_desain_rapor.Count == 1)
                            {
                                Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                if (m_rapor_desain != null)
                                {
                                    if (m_rapor_desain.TahunAjaran != null)
                                    {
                                        html_table_header0 += "<tr>" +
                                                                  "<td rowspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-weight: bold;\">" +
                                                                    "Mata Pelajaran" +
                                                                  "</td>" +
                                                                  "<td colspan=\"4\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-weight: bold;\">" +
                                                                    "Kompetensi Dasar" +
                                                                  "</td>" +
                                                                  "<td rowspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-weight: bold;\">" +
                                                                    "KKM" +
                                                                  "</td>" +
                                                              "</tr>" +
                                                              "<tr>" +
                                                                  "<td colspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-weight: bold;\">" +
                                                                    "Aspek Pengetahuan" +
                                                                  "</td>" +
                                                                  "<td colspan=\"2\" style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; font-weight: bold;\">" +
                                                                    "Aspek Keterampilan" +
                                                                  "</td>" +
                                                              "</tr>";

                                        List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                        int id = 1;

                                        List<string> lst_kd = new List<string>();
                                        lst_kd.Clear();
                                        foreach (var m_desain in lst_desain_rapor_det)
                                        {
                                            string kkm = "";

                                            List<string> lst_deskripsi_kd_pengetahuan = new List<string>();
                                            List<string> lst_deskripsi_kd_keterampilan = new List<string>();

                                            List <Rapor_LTS_Mapel> lst_lts_mapel = DAO_Rapor_LTS_Mapel.GetByTABySMByMapelByKelasDet(
                                                    tahun_ajaran, semester, m_desain.Rel_Mapel, rel_kelas_det
                                                ).OrderBy(m => m.Urutan).ToList();

                                            foreach (var item in lst_lts_mapel)
                                            {
                                                Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(
                                                        item.Rel_Rapor_StrukturNilai_KD.ToString()
                                                    );

                                                if (m_sn_kd != null)
                                                {
                                                    if (m_sn_kd.Poin != null)
                                                    {
                                                        Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString());
                                                        if (m_sn_ap != null)
                                                        {
                                                            if (m_sn_ap.JenisPerhitungan != null)
                                                            {
                                                                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(
                                                                        m_sn_ap.Rel_Rapor_AspekPenilaian.ToString()
                                                                    );
                                                                if (m_ap != null)
                                                                {
                                                                    if (m_ap.Nama != null)
                                                                    {
                                                                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                                        if (m_kd != null)
                                                                        {
                                                                            if (m_kd.Nama != null)
                                                                            {
                                                                                if (lst_kd.FindAll(m => m == Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)).ToString().Trim().ToLower()).Count == 0)
                                                                                {
                                                                                    lst_kd.Add(Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)).ToString().Trim().ToLower());

                                                                                    if (
                                                                                            m_ap.Nama.ToUpper().IndexOf("PENGETAHUAN") >= 0 ||
                                                                                            m_ap.Nama.ToUpper().IndexOf("PENGETAHU") >= 0 ||
                                                                                            m_ap.Nama.ToUpper().IndexOf("KOMPETENSI DASAR") >= 0
                                                                                        )
                                                                                    {
                                                                                        lst_deskripsi_kd_pengetahuan.Add(Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)));
                                                                                    }
                                                                                    else if (
                                                                                            m_ap.Nama.ToUpper().IndexOf("KETERAMPILAN") >= 0 ||
                                                                                            m_ap.Nama.ToUpper().IndexOf("KETRAMPILAN") >= 0 ||
                                                                                            m_ap.Nama.ToUpper().IndexOf("PRAKTIK") >= 0
                                                                                        )
                                                                                    {
                                                                                        lst_deskripsi_kd_keterampilan.Add(Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)));
                                                                                    }
                                                                                }
                                                                            }
                                                                        }

                                                                        if (m_sn_ap != null)
                                                                        {
                                                                            if (m_sn_ap.Poin != null)
                                                                            {

                                                                                Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(m_sn_ap.Rel_Rapor_StrukturNilai.ToString());
                                                                                if (m_sn != null)
                                                                                {

                                                                                    if (m_sn.TahunAjaran != null)
                                                                                    {
                                                                                        if (m_sn.KKM > 10)
                                                                                        {
                                                                                            kkm = Math.Round(m_sn.KKM, 0).ToString();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            kkm = Math.Round(m_sn.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();
                                                                                        }
                                                                                    }

                                                                                }

                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (m_desain.Rel_Mapel.Trim() == "")
                                            {
                                                html_table_body += "<tr>" +
                                                                        "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; vertical-align: middle; font-weight: bold; text-align: center;\">" +
                                                                            m_desain.NamaMapelRapor +
                                                                        "</td>" +
                                                                        "<td colspan=\"4\" style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: middle; font-weight: bold; text-align: center;\">" +
                                                                            "Kompetensi Dasar" +
                                                                        "</td>" +
                                                                        "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: middle; font-weight: bold; text-align: center;\">" +
                                                                            "KKM" +
                                                                        "</td>" +
                                                                    "</tr>";
                                            }
                                            else
                                            {
                                                int jumlah = (
                                                    lst_deskripsi_kd_pengetahuan.Count > lst_deskripsi_kd_keterampilan.Count
                                                    ? lst_deskripsi_kd_pengetahuan.Count
                                                    : lst_deskripsi_kd_keterampilan.Count
                                                );
                                                int jumlah_pengetahuan = 0;
                                                int jumlah_keterampilan = 0;
                                                string html_item_tr = "";
                                                if (jumlah > 0)
                                                {
                                                    for (int i = 1; i <= jumlah; i++)
                                                    {
                                                        if (
                                                            lst_deskripsi_kd_pengetahuan.Count > 0 ||
                                                            lst_deskripsi_kd_keterampilan.Count > 0
                                                        )
                                                        {
                                                            html_item_tr += "<tr>";
                                                            if (i == 1)
                                                            {
                                                                html_item_tr += "<td " + (jumlah > 1 ? " rowspan=\"" + jumlah.ToString() + "\" " : "") + " style=\"" + FONT_SIZE + "  vertical-align: middle; text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                                    m_desain.NamaMapelRapor +
                                                                                "</td>";
                                                            }

                                                            if (
                                                                lst_deskripsi_kd_pengetahuan.Count > 0 &&
                                                                lst_deskripsi_kd_keterampilan.Count > 0
                                                            )
                                                            {
                                                                if (lst_deskripsi_kd_pengetahuan.Count >= i)
                                                                {
                                                                    jumlah_pengetahuan++;
                                                                    html_item_tr += "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        jumlah_pengetahuan.ToString() + "." +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        lst_deskripsi_kd_pengetahuan[i - 1] +
                                                                                    "</td>";
                                                                }
                                                                else
                                                                {
                                                                    html_item_tr += "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";
                                                                }
                                                                if (lst_deskripsi_kd_keterampilan.Count >= i)
                                                                {
                                                                    jumlah_keterampilan++;
                                                                    html_item_tr += "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        jumlah_keterampilan.ToString() + "." +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        lst_deskripsi_kd_keterampilan[i - 1] +
                                                                                    "</td>";
                                                                }
                                                                else
                                                                {
                                                                    html_item_tr += "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";
                                                                }
                                                            }
                                                            else if (
                                                                lst_deskripsi_kd_pengetahuan.Count > 0 &&
                                                                lst_deskripsi_kd_keterampilan.Count == 0
                                                            )
                                                            {
                                                                if (lst_deskripsi_kd_pengetahuan.Count >= i)
                                                                {
                                                                    jumlah_pengetahuan++;
                                                                    html_item_tr += "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        jumlah_pengetahuan.ToString() + "." +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        lst_deskripsi_kd_pengetahuan[i - 1] +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>";
                                                                }
                                                            }
                                                            else if (
                                                                lst_deskripsi_kd_pengetahuan.Count == 0 &&
                                                                lst_deskripsi_kd_keterampilan.Count > 0
                                                            )
                                                            {
                                                                if (lst_deskripsi_kd_keterampilan.Count >= i)
                                                                {
                                                                    jumlah_keterampilan++;
                                                                    html_item_tr += "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        "&nbsp;" +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 30px; max-width: 30px; border-style: solid; border-width: 1px; border-right-style: none; text-align: center; vertical-align: top;\">" +
                                                                                        jumlah_keterampilan.ToString() + "." +
                                                                                    "</td>" +
                                                                                    "<td style=\"width: 40%; border-style: solid; border-width: 1px; vertical-align: top; border-left-style: none;\">" +
                                                                                        lst_deskripsi_kd_keterampilan[i - 1] +
                                                                                    "</td>";
                                                                }
                                                            }

                                                            if (i == 1)
                                                            {
                                                                html_item_tr += "<td " + (jumlah > 1 ? " rowspan=\"" + jumlah.ToString() + "\" " : "") + " style=\"" + FONT_SIZE + "  vertical-align: middle; text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px;\">" +
                                                                                    kkm +
                                                                                "</td>";
                                                            }
                                                            html_item_tr += "</tr>";
                                                        }
                                                    }
                                                }

                                                html_table_body += html_item_tr;
                                            }

                                            if (m_desain.Rel_Mapel.Trim() != "") id++;
                                        }
                                    }
                                }

                            }
                            //end nilai akademik

                            html_table_header += html_table_header0 +
                                                 html_table_header1 +
                                                 html_table_header2;

                            string s_nama_guru = "";
                            string s_tanggal = "";

                            Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                            if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                            {
                                s_nama_guru = m_mengetahui.NamaGuru;
                                s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                            }

                            if (s_nama_guru.Trim() == "")
                            {
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
                                                        s_nama_guru = m_pegawai.Nama;

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
                            }

                            html +=
                                   html_header +
                                   "<table class=\"print-friendly\" style=\"margin: 15px; border-collapse: collapse;\">" +
                                        html_table_header +
                                        html_table_body +
                                        "<tfoot>" +
                                            "<tr>" +
                                                "<td colspan=\"13\">" +
                                                "</td>" +
                                            "</tr>" +
                                        "</tfoot>" +
                                   "</table>" +
                                   (
                                    show_pagebreak
                                    ? "<div class=\"pagebreak\"></div>"
                                    : ""
                                   );

                            if (print_mode)
                            {
                                html = html.Replace(FONT_SIZE, "font-size: 12pt;");
                            }
                            else
                            {
                                html = html.Replace(FONT_SIZE, "font-size: 11pt;");
                            }

                        }

                    }

                }
            }

            return html;
        }

        public static string GetHTMLBahasaKDNoCheck(
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
            string html_table_header2 = "";

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

                            //header
                            string html_header =
                                   "<table style=\"width: 100%; margin: 0px; margin-top: 45px;\">" +
                                        "<tr>" +
                                            "<td style=\"width: 60px;\">" +
                                                "<img src=\"" + page.ResolveUrl("~/Application_CLibs/images/logo.png") + "\" />" +
                                            "</td>" +
                                            "<td style=\"padding: 5px;" + FONT_SIZE + " \">" +
                                                "SEKOLAH DASAR ISLAM AL IZHAR PONDOK LABU<br />" +
                                                "JL. RS. Fatmawati Kav. 49 Telp. 7695542<br />" +
                                                "Jakarta" +
                                            "</td>" +
                                        "</tr>" +
                                   "</table>";

                            string html_table_body = "";

                            //nilai akademik
                            List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                );

                            if (lst_desain_rapor.Count == 1)
                            {
                                Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                if (m_rapor_desain != null)
                                {
                                    if (m_rapor_desain.TahunAjaran != null)
                                    {
                                        html_table_header0 += "<tr>" +
                                                                  "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "No" +
                                                                  "</td>" +
                                                                  "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "Mata Pelajaran" +
                                                                  "</td>" +
                                                                  "<td style=\"" + FONT_SIZE + " text-align: center; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "Kompetensi Dasar" +
                                                                  "</td>" +
                                                                  "<td style=\"" + FONT_SIZE + " text-align: center; width: 50px; max-width: 50px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px;\">" +
                                                                    "KKM" +
                                                                  "</td>" +
                                                              "</tr>";

                                        List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                        int id = 1;

                                        List<string> lst_kd = new List<string>();
                                        lst_kd.Clear();
                                        foreach (var m_desain in lst_desain_rapor_det)
                                        {
                                            string html_deskripsi_kd = "";
                                            string kkm = "";

                                            List<Rapor_LTS_Mapel> lst_lts_mapel = DAO_Rapor_LTS_Mapel.GetByTABySMByMapelByKelasDet(
                                                    tahun_ajaran, semester, m_desain.Rel_Mapel, rel_kelas_det
                                                ).OrderBy(m => m.Urutan).ToList();

                                            foreach (var item in lst_lts_mapel)
                                            {
                                                Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(
                                                        item.Rel_Rapor_StrukturNilai_KD.ToString()
                                                    );

                                                if (m_sn_kd != null)
                                                {
                                                    if (m_sn_kd.Poin != null)
                                                    {

                                                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                        if (m_kd != null)
                                                        {
                                                            if (m_kd.Nama != null)
                                                            {
                                                                if (lst_kd.FindAll(m => m == Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)).ToString().Trim().ToLower()).Count == 0)
                                                                {
                                                                    lst_kd.Add(Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)).ToString().Trim().ToLower());

                                                                    html_deskripsi_kd += "<li>" +
                                                                                            Libs.GetCleanDeskripsiKD(Libs.GetHTMLNoParagraphDiAwal(m_kd.Nama)) +
                                                                                         "</li>";
                                                                }
                                                            }
                                                        }

                                                        Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString());
                                                        if (m_sn_ap != null)
                                                        {
                                                            if (m_sn_ap.Poin != null)
                                                            {

                                                                Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(m_sn_ap.Rel_Rapor_StrukturNilai.ToString());
                                                                if (m_sn != null)
                                                                {

                                                                    if (m_sn.TahunAjaran != null)
                                                                    {
                                                                        if (m_sn.KKM > 10)
                                                                        {
                                                                            kkm = Math.Round(m_sn.KKM, 0).ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            kkm = Math.Round(m_sn.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SD).ToString();
                                                                        }
                                                                    }

                                                                }

                                                            }
                                                        }

                                                    }
                                                }
                                            }

                                            if (html_deskripsi_kd.Trim() != "")
                                            {
                                                html_deskripsi_kd = "<ol style=\"margin: 0px;\">" +
                                                                        html_deskripsi_kd +
                                                                    "</ol>";
                                            }

                                            if (m_desain.Rel_Mapel.Trim() == "")
                                            {
                                                html_table_body += "<tr>" +
                                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; vertical-align: top;\">" +
                                                                            "" +
                                                                        "</td>" +
                                                                        "<td colspan=\"3\" style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                            m_desain.NamaMapelRapor +
                                                                        "</td>" +
                                                                    "</tr>";
                                            }
                                            else
                                            {
                                                html_table_body += "<tr>" +
                                                                        "<td style=\"" + FONT_SIZE + "  text-align: center; width: 35px; max-width: 35px; border-style: solid; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-right: 0px; padding-left: 0px; vertical-align: top;\">" +
                                                                            id.ToString() +
                                                                        "</td>" +
                                                                        "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                            m_desain.NamaMapelRapor +
                                                                        "</td>" +
                                                                        "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                            html_deskripsi_kd +
                                                                        "</td>" +
                                                                        "<td style=\"" + FONT_SIZE + "  border-style: solid; border-left-style: none; border-width: 1px; padding-top: 2px; padding-bottom: 2px; padding-left: 5px; padding-right: 5px; vertical-align: top;\">" +
                                                                            kkm +
                                                                        "</td>" +
                                                                    "</tr>";
                                            }

                                            if (m_desain.Rel_Mapel.Trim() != "") id++;
                                        }
                                    }
                                }

                            }
                            //end nilai akademik

                            html_table_header += html_table_header0 +
                                                 html_table_header1 +
                                                 html_table_header2;

                            string s_nama_guru = "";
                            string s_tanggal = "";

                            Rapor_LTS_MengetahuiGuruKelas m_mengetahui = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FirstOrDefault();
                            if (DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).Count > 0)
                            {
                                s_nama_guru = m_mengetahui.NamaGuru;
                                s_tanggal = Libs.GetTanggalIndonesiaFromDate(m_mengetahui.Tanggal, false);
                            }

                            if (s_nama_guru.Trim() == "")
                            {
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
                                                        s_nama_guru = m_pegawai.Nama;

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
                            }

                            html +=
                                   html_header +
                                   "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                        "Kompetensi Dasar Laporan Tengah Semester " + (semester == "1" ? "I" : "II") +
                                   "</div>" +
                                   "<div style=\"margin: 0 auto; display: table; " + FONT_SIZE + "\">" +
                                        "Kelas " + m_kelas.Nama + " " +
                                        "Tahun Pelajaran " + tahun_ajaran +
                                   "</div>" +
                                   "<table class=\"print-friendly\" style=\"margin: 15px; border-collapse: collapse;\">" +
                                        html_table_header +
                                        html_table_body +
                                        "<tfoot>" +
                                            "<tr>" +
                                                "<td colspan=\"13\">" +
                                                "</td>" +
                                            "</tr>" +
                                        "</tfoot>" +
                                   "</table>" +
                                   (
                                    show_pagebreak
                                    ? "<div class=\"pagebreak\"></div>"
                                    : ""
                                   );

                            if (print_mode)
                            {
                                html = html.Replace(FONT_SIZE, "font-size: 12pt;");
                            }
                            else
                            {
                                html = html.Replace(FONT_SIZE, "font-size: 11pt;");
                            }

                        }

                    }

                }
            }

            return html;
        }

        private static void DoClosingLTS_KTSP(string tahun_ajaran, string semester, string rel_kelas_det, string guru_kelas, DateTime tanggal_lts)
        {
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

                            bool ada_lts = false;
                            string kode_lts = Guid.NewGuid().ToString();
                            Rapor_LTS m_lts = DAO_Rapor_LTS.GetAllByTABySMByKelasDet_Entity(
                                    tahun_ajaran, semester, rel_kelas_det
                                ).FirstOrDefault();
                            if (m_lts != null)
                            {
                                if (m_lts.TahunAjaran != null)
                                {
                                    ada_lts = true;
                                    kode_lts = m_lts.Kode.ToString();
                                }
                            }

                            //insert or update header
                            if (ada_lts)
                            {
                                DAO_Rapor_LTS.Update(new Rapor_LTS {
                                    Kode = new Guid(kode_lts),
                                    TahunAjaran = tahun_ajaran,
                                    Semester = semester,
                                    Rel_KelasDet = rel_kelas_det,
                                    GuruKelas = guru_kelas,
                                    Tanggal = tanggal_lts,
                                    Kurikulum = Libs.JenisKurikulum.SD.KTSP
                                });
                            }
                            else
                            {
                                DAO_Rapor_LTS.Insert(new Rapor_LTS
                                {
                                    Kode = new Guid(kode_lts),
                                    TahunAjaran = tahun_ajaran,
                                    Semester = semester,
                                    Rel_KelasDet = rel_kelas_det,
                                    GuruKelas = guru_kelas,
                                    Tanggal = tanggal_lts,
                                    Kurikulum = Libs.JenisKurikulum.SD.KTSP
                                });
                            }
                            //end insert or update header

                            //delete header
                            DAO_Rapor_LTS_Det.DeleteByHeader(kode_lts);
                            //end delete header

                            foreach (var m_siswa in lst_siswa)
                            {

                                //nilai akademik
                                List<Rapor_Desain> lst_desain_rapor = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                        tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.LTS
                                    );

                                if (lst_desain_rapor.Count == 1)
                                {
                                    Rapor_Desain m_rapor_desain = lst_desain_rapor.FirstOrDefault();
                                    if (m_rapor_desain != null)
                                    {
                                        if (m_rapor_desain.TahunAjaran != null)
                                        {
                                            int jml_kolom = 5;
                                            List<Rapor_Desain_Det> lst_desain_rapor_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(m_rapor_desain.Kode.ToString()).OrderBy(m => m.Urutan).ToList();
                                            foreach (var m_desain in lst_desain_rapor_det)
                                            {
                                                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapelBySiswaForLTS_Entity(
                                                        tahun_ajaran, semester, rel_kelas_det, m_desain.Rel_Mapel, m_siswa.Kode.ToString()
                                                    );

                                                Rapor_StrukturNilai m_sn = null;

                                                List<string> lst_nilai_1 = new List<string>(); //nilai tugas
                                                List<string> lst_nilai_1_kd = new List<string>(); //nilai tugas KD
                                                List<string> lst_nilai_1_nama_kd = new List<string>(); //nama tugas KD
                                                List<string> lst_nilai_1_bobot_kp = new List<string>(); //bobot tugas KP
                                                List<string> lst_nilai_2 = new List<string>(); //nilai uh
                                                List<string> lst_nilai_2_kd = new List<string>(); //nilai uh KD
                                                List<string> lst_nilai_2_nama_kd = new List<string>(); //nama uh KD
                                                List<string> lst_nilai_2_bobot_kp = new List<string>(); //bobot tugas KP

                                                List<NilaiLTS> lst_nilai_lts = new List<NilaiLTS>();
                                                List<Nilai_LTS> lst_nilai_lts_tugas = new List<Nilai_LTS>();
                                                List<Nilai_LTS> lst_nilai_lts_uh = new List<Nilai_LTS>();

                                                int id_nilai = 0;
                                                List<string> lst_nilai = new List<string>();
                                                lst_nilai.Clear();

                                                foreach (var nilai_siswa_det in lst_nilai_siswa_det)
                                                {
                                                    Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(nilai_siswa_det.Rel_Rapor_StrukturNilai_KD.ToString());
                                                    Rapor_StrukturNilai_KP m_sn_kp = DAO_Rapor_StrukturNilai_KP.GetByID_Entity(nilai_siswa_det.Rel_Rapor_StrukturNilai_KP.ToString());

                                                    if (m_sn_kp != null && m_sn_kd != null)
                                                    {
                                                        if (m_sn_kp.Jenis != null && m_sn_kd.JenisPerhitungan != null)
                                                        {
                                                            Rapor_StrukturNilai_AP m_sn_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(nilai_siswa_det.Rel_Rapor_StrukturNilai_AP);
                                                            if (m_sn_ap.Poin != null)
                                                            {
                                                                m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(m_sn_ap.Rel_Rapor_StrukturNilai.ToString());
                                                            }

                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                            Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                            if (m_kp != null && m_kd != null)
                                                            {
                                                                if (m_kp.Nama != null && m_kd.Nama != null)
                                                                {
                                                                    if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "LK")
                                                                    {
                                                                        lst_nilai_1_kd.Add(m_sn_kd.Kode.ToString()); //m_sn_kd.Urutan.ToString()
                                                                        lst_nilai_1_nama_kd.Add(m_kd.Nama.ToString());
                                                                        lst_nilai_1.Add(nilai_siswa_det.Nilai);
                                                                        lst_nilai_1_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                        lst_nilai_lts_tugas.Add(new Nilai_LTS
                                                                        {
                                                                            Urutan = m_sn_kd.Urutan,
                                                                            NamaKD = m_kd.Nama,
                                                                            Nilai = nilai_siswa_det.Nilai,
                                                                            BobotKP = m_sn_kp.BobotNK
                                                                        });
                                                                    }
                                                                    else if (Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "UH" ||
                                                                        Libs.GetHTMLSimpleText(m_kp.Nama.Trim().ToUpper()) == "PRAKTIK" ||
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).Substring(0, 2) == "UH")
                                                                    {
                                                                        lst_nilai_2_kd.Add(m_sn_kd.Kode.ToString()); //m_sn_kd.Urutan.ToString()
                                                                        lst_nilai_2_nama_kd.Add(m_kd.Nama.ToString());
                                                                        lst_nilai_2.Add(nilai_siswa_det.Nilai);
                                                                        lst_nilai_2_bobot_kp.Add(m_sn_kp.BobotNK.ToString());

                                                                        lst_nilai_lts_uh.Add(new Nilai_LTS
                                                                        {
                                                                            Urutan = m_sn_kd.Urutan,
                                                                            NamaKD = m_kd.Nama,
                                                                            Nilai = nilai_siswa_det.Nilai,
                                                                            BobotKP = m_sn_kp.BobotNK
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (m_sn != null)
                                                {
                                                    if (m_sn.TahunAjaran != null)
                                                    {

                                                        if (m_sn.IsKelompokanKP) //contoh al-quran
                                                        {
                                                            List<int> lst_urutan_kd_tugas = lst_nilai_lts_tugas.Select(m => m.Urutan).Distinct().ToList();
                                                            List<int> lst_urutan_kd_uh = lst_nilai_lts_uh.Select(m => m.Urutan).Distinct().ToList();
                                                            List<int> lst_gabungan = new List<int>();

                                                            foreach (var item in lst_urutan_kd_tugas)
                                                            {
                                                                if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                {
                                                                    lst_gabungan.Add(item);
                                                                }
                                                            }
                                                            foreach (var item in lst_urutan_kd_uh)
                                                            {
                                                                if (lst_gabungan.FindAll(m => m == item).Count == 0)
                                                                {
                                                                    lst_gabungan.Add(item);
                                                                }
                                                            }
                                                            lst_nilai_lts.Clear();
                                                            int urutan = 1;
                                                            foreach (var item in lst_gabungan.OrderBy(m => m).ToList())
                                                            {
                                                                string s_nilai_tugas = "";
                                                                string s_nilai_uh = "";

                                                                decimal nilai = 0;
                                                                foreach (var item_ in lst_nilai_lts_tugas.FindAll(m => m.Urutan == item))
                                                                {
                                                                    nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                }
                                                                s_nilai_tugas = Libs.GetFormatBilangan(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD);

                                                                nilai = 0;
                                                                foreach (var item_ in lst_nilai_lts_uh.FindAll(m => m.Urutan == item))
                                                                {
                                                                    nilai += (item_.BobotKP / 100) * Libs.GetStringToDecimal(item_.Nilai);
                                                                }
                                                                s_nilai_uh = Libs.GetFormatBilangan(nilai, Constantas.PEMBULATAN_DESIMAL_NILAI_SD);

                                                                lst_nilai_lts.Add(new NilaiLTS
                                                                {
                                                                    Urutan = urutan,
                                                                    NamaKD = "",
                                                                    NilaiTugas = s_nilai_tugas,
                                                                    NilaiUH = s_nilai_uh
                                                                });
                                                                urutan++;
                                                            }

                                                            id_nilai = 0;
                                                            for (int i = 0; i < jml_kolom; i++)
                                                            {
                                                                if (i < lst_nilai_lts.Count)
                                                                {
                                                                    lst_nilai.Add(lst_nilai_lts[i].NilaiTugas);
                                                                    lst_nilai.Add(lst_nilai_lts[i].NilaiUH);
                                                                }
                                                                else
                                                                {
                                                                    lst_nilai.Add("");
                                                                    lst_nilai.Add("");
                                                                }
                                                            }
                                                        }
                                                        else //mapel normal
                                                        {
                                                            lst_nilai_lts.Clear();
                                                            if (lst_nilai_1.Count > lst_nilai_2.Count)
                                                            {
                                                                id_nilai = 0;
                                                                foreach (var item in lst_nilai_1)
                                                                {
                                                                    string nilai_uh = "";
                                                                    for (int i = 0; i < lst_nilai_2_kd.Count; i++)
                                                                    {
                                                                        if (lst_nilai_2_kd[i] == lst_nilai_1_kd[id_nilai])
                                                                        {
                                                                            nilai_uh = lst_nilai_2[i];
                                                                            break;
                                                                        }
                                                                    }

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = (id_nilai + 1),
                                                                        NilaiTugas = lst_nilai_1[id_nilai],
                                                                        NilaiUH = nilai_uh
                                                                    });
                                                                    id_nilai++;
                                                                }
                                                            }
                                                            else if (lst_nilai_1.Count < lst_nilai_2.Count)
                                                            {
                                                                id_nilai = 0;
                                                                foreach (var item in lst_nilai_2)
                                                                {
                                                                    string nilai_tugas = "";
                                                                    for (int i = 0; i < lst_nilai_1_kd.Count; i++)
                                                                    {
                                                                        if (lst_nilai_1_kd[i] == lst_nilai_2_kd[id_nilai])
                                                                        {
                                                                            nilai_tugas = lst_nilai_1[i];
                                                                            break;
                                                                        }
                                                                    }

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = (id_nilai + 1),
                                                                        NilaiTugas = nilai_tugas,
                                                                        NilaiUH = lst_nilai_2[id_nilai],
                                                                    });
                                                                    id_nilai++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                id_nilai = 0;
                                                                foreach (var item in lst_nilai_2)
                                                                {
                                                                    string nilai_tugas = lst_nilai_1[id_nilai];

                                                                    lst_nilai_lts.Add(new NilaiLTS
                                                                    {
                                                                        Urutan = (id_nilai + 1),
                                                                        NilaiTugas = nilai_tugas,
                                                                        NilaiUH = lst_nilai_2[id_nilai],
                                                                    });
                                                                    id_nilai++;
                                                                }
                                                            }

                                                            id_nilai = 0;
                                                            for (int i = 0; i < jml_kolom; i++)
                                                            {
                                                                if (i < lst_nilai_lts.Count)
                                                                {
                                                                    lst_nilai.Add(
                                                                            (
                                                                                lst_nilai_lts[i].NilaiTugas.Trim() != ""
                                                                                ? Libs.GetFormatBilangan(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiTugas), Constantas.PEMBULATAN_DESIMAL_NILAI_SD)
                                                                                : ""
                                                                            )
                                                                        );
                                                                    lst_nilai.Add(
                                                                            (
                                                                                lst_nilai_lts[i].NilaiUH.Trim() != ""
                                                                                ? Libs.GetFormatBilangan(Libs.GetStringToDecimal(lst_nilai_lts[i].NilaiUH), Constantas.PEMBULATAN_DESIMAL_NILAI_SD)
                                                                                : ""
                                                                            )
                                                                        );
                                                                }
                                                                else
                                                                {
                                                                    lst_nilai.Add("");
                                                                    lst_nilai.Add("");
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        id_nilai = 0;
                                                        for (int i = 1; i <= jml_kolom; i++)
                                                        {
                                                            lst_nilai.Add("");
                                                            lst_nilai.Add("");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    id_nilai = 0;
                                                    for (int i = 1; i <= jml_kolom; i++)
                                                    {
                                                        lst_nilai.Add("");
                                                        lst_nilai.Add("");
                                                    }
                                                }

                                                //list detail nilai
                                                Rapor_LTS_Det m_rapor_det = new Rapor_LTS_Det();
                                                int nomor_nilai = 1;
                                                m_rapor_det.Kode = Guid.NewGuid();
                                                m_rapor_det.Rel_Rapor_LTS = new Guid(kode_lts);
                                                m_rapor_det.Rel_Mapel = m_desain.Rel_Mapel;
                                                m_rapor_det.Rel_Siswa = m_siswa.Kode.ToString();
                                                foreach (var nilai in lst_nilai)
                                                {
                                                    switch (nomor_nilai)
                                                    {
                                                        case 1:
                                                            m_rapor_det.Nilai1 = nilai;
                                                            break;
                                                        case 2:
                                                            m_rapor_det.Nilai2 = nilai;
                                                            break;
                                                        case 3:
                                                            m_rapor_det.Nilai3 = nilai;
                                                            break;
                                                        case 4:
                                                            m_rapor_det.Nilai4 = nilai;
                                                            break;
                                                        case 5:
                                                            m_rapor_det.Nilai5 = nilai;
                                                            break;
                                                        case 6:
                                                            m_rapor_det.Nilai6 = nilai;
                                                            break;
                                                        case 7:
                                                            m_rapor_det.Nilai7 = nilai;
                                                            break;
                                                        case 8:
                                                            m_rapor_det.Nilai8 = nilai;
                                                            break;
                                                        case 9:
                                                            m_rapor_det.Nilai9 = nilai;
                                                            break;
                                                        case 10:
                                                            m_rapor_det.Nilai10 = nilai;
                                                            break;
                                                    }

                                                    nomor_nilai++;
                                                }

                                                //save nilai det
                                                DAO_Rapor_LTS_Det.Insert(m_rapor_det);

                                            }
                                        }
                                    }
                                }

                            }

                        }

                    }

                }

            }
        }

        private static void DoClosingLTS_KURTILAS(string tahun_ajaran, string semester, string rel_kelas_det, string guru_kelas, DateTime tanggal_lts)
        {

        }

        public static void DoClosingLTS(string tahun_ajaran, string semester)
        {

            List<Rapor_Nilai> lst_rapor_nilai = DAO_Rapor_Nilai.GetPenilaianByTABySM_Entity(
                    tahun_ajaran, semester
                );

            foreach (var m_nilai in lst_rapor_nilai)
            {
                string kurikulum = "";
                string guru_kelas = "";
                DateTime tanggal_lts = DateTime.Now;

                Rapor_Pengaturan m_pengaturan = DAO_Rapor_Pengaturan.Get_Entity();
                if (m_pengaturan != null)
                {
                    if (m_pengaturan.KepalaSekolah != null)
                    {
                        kurikulum = DAO_Rapor_StrukturNilai.GetKurikulumByKelas(tahun_ajaran, semester, m_nilai.Rel_KelasDet);
                        Rapor_LTS_MengetahuiGuruKelas m_guru_kelas = DAO_Rapor_LTS_MengetahuiGuruKelas.GetByTABySMByKelasDet_Entity(
                                tahun_ajaran, semester, m_nilai.Rel_KelasDet
                            ).FirstOrDefault();
                        if (m_guru_kelas != null)
                        {
                            if (m_guru_kelas.NamaGuru != null)
                            {
                                guru_kelas = m_guru_kelas.NamaGuru;
                                tanggal_lts = m_guru_kelas.Tanggal;
                            }
                        }

                        if (kurikulum == Libs.JenisKurikulum.SD.KTSP)
                        {
                            DoClosingLTS_KTSP(tahun_ajaran, semester, m_nilai.Rel_KelasDet, guru_kelas, tanggal_lts);
                        }
                        else if (kurikulum == Libs.JenisKurikulum.SD.KURTILAS)
                        {
                            DoClosingLTS_KURTILAS(tahun_ajaran, semester, m_nilai.Rel_KelasDet, guru_kelas, tanggal_lts);
                        }

                    }
                }

            }

        }

    }
}