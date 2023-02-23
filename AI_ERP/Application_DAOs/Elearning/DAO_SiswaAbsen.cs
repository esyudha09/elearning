using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_SiswaAbsen
    {
        public const string SP_SELECT_BY_ID = "SiswaAbsen_SELECT_BY_ID";
        public const string SP_SELECT_BY_LINIMASA = "SiswaAbsen_SELECT_BY_LINIMASA";
        public const string SP_SELECT_BY_SEKOLAH_BY_KELASDET_BY_TANGGAL = "SiswaAbsen_SELECT_BY_SEKOLAH_BY_KELASDET_BY_TANGGAL";
        public const string SP_SELECT_BY_SEKOLAH_BY_KELASDET_BY_SISWA_BY_TANGGAL = "SiswaAbsen_SELECT_BY_SEKOLAH_BY_KELASDET_BY_SISWA_BY_TANGGAL";
        public const string SP_SELECT_PERIODE_BY_SEKOLAH_BY_KELASDET_BY_TA = "SiswaAbsen_SELECT_PERIODE_BY_SEKOLAH_BY_KELASDET_BY_TA";
        public const string SP_SELECT_BY_SISWA_BY_PERIODE = "SiswaAbsen_SELECT_BY_SISWA_BY_PERIODE";
        public const string SP_SELECT_REKAP_ABSEN_RAPOR_BY_PERIODE = "SiswaAbsen_SELECT_REKAP_ABSEN_RAPOR_BY_PERIODE";
        public const string SP_SELECT_REKAP_ABSEN_RAPOR_BY_SISWA_BY_PERIODE = "SiswaAbsen_SELECT_REKAP_ABSEN_RAPOR_BY_SISWA_BY_PERIODE";
        public const string SP_SELECT_SELECT_DISTINCT_KETERANGAN = "SiswaAbsen_SELECT_DISTINCT_KETERANGAN";
        public const string SP_SELECT_SELECT_PERIODE = "SiswaAbsen_SELECT_PERIODE";
        public const string SP_SELECT_SELECT_BY_UNIT_BY_KELASDET_BY_TANGGAL = "SiswaAbsen_SELECT_BY_UNIT_BY_KELASDET_BY_TANGGAL";

        public const string SP_INSERT = "SiswaAbsen_INSERT";

        public const string SP_UPDATE = "SiswaAbsen_UPDATE";

        public const string SP_DELETE = "SiswaAbsen_DELETE";
        public const string SP_DELETE_BY_LINIMASA = "SiswaAbsen_DELETE_BY_LINIMASA";

        public class AbsenRekapRapor
        {
            public string Absen { get; set; }
            public int Jumlah { get; set; }
        }

        public class AbsenRekapRaporSiswa : AbsenRekapRapor
        {
            public string Rel_Siswa { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Guru = "Rel_Guru";
            public const string Tanggal = "Tanggal";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string NamaSiswa = "NamaSiswa";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Mapel = "Mapel";
            public const string JenisMapel = "JenisMapel";
            public const string Absen = "Absen";
            public const string Keterangan = "Keterangan";
            public const string Rel_Linimasa = "Rel_Linimasa";
            public const string Kejadian = "Kejadian";
            public const string ButirSikap = "ButirSikap";
            public const string ButirSikapLain = "ButirSikapLain";
            public const string Sikap = "Sikap";
            public const string TindakLanjut = "TindakLanjut";

            public const string Is_Hadir = "Is_Hadir";
            public const string Is_Hadir_Time = "Is_Hadir_Time";
            public const string Is_Sakit = "Is_Sakit";
            public const string Is_Sakit_Time = "Is_Sakit_Time";
            public const string Is_Izin = "Is_Izin";
            public const string Is_Izin_Time = "Is_Izin_Time";
            public const string Is_Alpa = "Is_Alpa";
            public const string Is_Alpa_Time = "Is_Alpa_Time";
            public const string Is_Cat01 = "Is_Cat01";
            public const string Is_Cat01_Time = "Is_Cat01_Time";
            public const string Is_Cat02 = "Is_Cat02";
            public const string Is_Cat02_Time = "Is_Cat02_Time";
            public const string Is_Cat03 = "Is_Cat03";
            public const string Is_Cat03_Time = "Is_Cat03_Time";
            public const string Is_Cat04 = "Is_Cat04";
            public const string Is_Cat04_Time = "Is_Cat04_Time";
            public const string Is_Cat05 = "Is_Cat05";
            public const string Is_Cat05_Time = "Is_Cat05_Time";
            public const string Is_Cat06 = "Is_Cat06";
            public const string Is_Cat06_Time = "Is_Cat06_Time";
            public const string Is_Cat07 = "Is_Cat07";
            public const string Is_Cat07_Time = "Is_Cat07_Time";
            public const string Is_Cat08 = "Is_Cat08";
            public const string Is_Cat08_Time = "Is_Cat08_Time";
            public const string Is_Cat09 = "Is_Cat09";
            public const string Is_Cat09_Time = "Is_Cat09_Time";
            public const string Is_Cat10 = "Is_Cat10";
            public const string Is_Cat10_Time = "Is_Cat10_Time";
            public const string Is_Cat11 = "Is_Cat11";
            public const string Is_Cat11_Time = "Is_Cat11_Time";
            public const string Is_Cat12 = "Is_Cat12";
            public const string Is_Cat12_Time = "Is_Cat12_Time";
            public const string Is_Cat13 = "Is_Cat13";
            public const string Is_Cat13_Time = "Is_Cat13_Time";
            public const string Is_Cat14 = "Is_Cat14";
            public const string Is_Cat14_Time = "Is_Cat14_Time";
            public const string Is_Cat15 = "Is_Cat15";
            public const string Is_Cat15_Time = "Is_Cat15_Time";
            public const string Is_Cat16 = "Is_Cat16";
            public const string Is_Cat16_Time = "Is_Cat16_Time";
            public const string Is_Cat17 = "Is_Cat17";
            public const string Is_Cat17_Time = "Is_Cat17_Time";
            public const string Is_Cat18 = "Is_Cat18";
            public const string Is_Cat18_Time = "Is_Cat18_Time";
            public const string Is_Cat19 = "Is_Cat19";
            public const string Is_Cat19_Time = "Is_Cat19_Time";
            public const string Is_Cat20 = "Is_Cat20";
            public const string Is_Cat20_Time = "Is_Cat20_Time";

            public const string Is_Sakit_Keterangan = "Is_Sakit_Keterangan";
            public const string Is_Izin_Keterangan = "Is_Izin_Keterangan";
            public const string Is_Alpa_Keterangan = "Is_Alpa_Keterangan";

            public const string Is_Cat01_Keterangan = "Is_Cat01_Keterangan";
            public const string Is_Cat02_Keterangan = "Is_Cat02_Keterangan";
            public const string Is_Cat03_Keterangan = "Is_Cat03_Keterangan";
            public const string Is_Cat04_Keterangan = "Is_Cat04_Keterangan";
            public const string Is_Cat05_Keterangan = "Is_Cat05_Keterangan";
            public const string Is_Cat06_Keterangan = "Is_Cat06_Keterangan";
            public const string Is_Cat07_Keterangan = "Is_Cat07_Keterangan";
            public const string Is_Cat08_Keterangan = "Is_Cat08_Keterangan";
            public const string Is_Cat09_Keterangan = "Is_Cat09_Keterangan";
            public const string Is_Cat10_Keterangan = "Is_Cat10_Keterangan";
            public const string Is_Cat11_Keterangan = "Is_Cat11_Keterangan";
            public const string Is_Cat12_Keterangan = "Is_Cat12_Keterangan";
            public const string Is_Cat13_Keterangan = "Is_Cat13_Keterangan";
            public const string Is_Cat14_Keterangan = "Is_Cat14_Keterangan";
            public const string Is_Cat15_Keterangan = "Is_Cat15_Keterangan";
            public const string Is_Cat16_Keterangan = "Is_Cat16_Keterangan";
            public const string Is_Cat17_Keterangan = "Is_Cat17_Keterangan";
            public const string Is_Cat18_Keterangan = "Is_Cat18_Keterangan";
            public const string Is_Cat19_Keterangan = "Is_Cat19_Keterangan";
            public const string Is_Cat20_Keterangan = "Is_Cat20_Keterangan";
        }

        private static SiswaAbsen GetEntityFromDataRow(DataRow row)
        {
            return new SiswaAbsen
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Sekolah = new Guid(row[NamaField.Rel_Sekolah].ToString()),
                Rel_KelasDet = new Guid(row[NamaField.Rel_KelasDet].ToString()),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Absen = row[NamaField.Absen].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Rel_Linimasa = new Guid(row[NamaField.Rel_Linimasa].ToString()),
                Kejadian = row[NamaField.Kejadian].ToString(),
                ButirSikap = row[NamaField.ButirSikap].ToString(),
                ButirSikapLain = row[NamaField.ButirSikapLain].ToString(),
                Sikap = row[NamaField.Sikap].ToString(),
                TindakLanjut = row[NamaField.TindakLanjut].ToString(),

                Is_Hadir = row[NamaField.Is_Hadir].ToString(),
                Is_Hadir_Time = Convert.ToDateTime(row[NamaField.Is_Hadir_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Hadir_Time])),
                Is_Sakit = row[NamaField.Is_Sakit].ToString(),
                Is_Sakit_Time = Convert.ToDateTime(row[NamaField.Is_Sakit_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Sakit_Time])),
                Is_Izin = row[NamaField.Is_Izin].ToString(),
                Is_Izin_Time = Convert.ToDateTime(row[NamaField.Is_Izin_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Izin_Time])),
                Is_Alpa = row[NamaField.Is_Alpa].ToString(),
                Is_Alpa_Time = Convert.ToDateTime(row[NamaField.Is_Alpa_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Alpa_Time])),

                Is_Cat01 = row[NamaField.Is_Cat01].ToString(),
                Is_Cat01_Time = Convert.ToDateTime(row[NamaField.Is_Cat01_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat01_Time])),
                Is_Cat02 = row[NamaField.Is_Cat02].ToString(),
                Is_Cat02_Time = Convert.ToDateTime(row[NamaField.Is_Cat02_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat02_Time])),
                Is_Cat03 = row[NamaField.Is_Cat03].ToString(),
                Is_Cat03_Time = Convert.ToDateTime(row[NamaField.Is_Cat03_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat03_Time])),
                Is_Cat04 = row[NamaField.Is_Cat04].ToString(),
                Is_Cat04_Time = Convert.ToDateTime(row[NamaField.Is_Cat04_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat04_Time])),
                Is_Cat05 = row[NamaField.Is_Cat05].ToString(),
                Is_Cat05_Time = Convert.ToDateTime(row[NamaField.Is_Cat05_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat05_Time])),
                Is_Cat06 = row[NamaField.Is_Cat06].ToString(),
                Is_Cat06_Time = Convert.ToDateTime(row[NamaField.Is_Cat06_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat06_Time])),
                Is_Cat07 = row[NamaField.Is_Cat07].ToString(),
                Is_Cat07_Time = Convert.ToDateTime(row[NamaField.Is_Cat07_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat07_Time])),
                Is_Cat08 = row[NamaField.Is_Cat08].ToString(),
                Is_Cat08_Time = Convert.ToDateTime(row[NamaField.Is_Cat08_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat08_Time])),
                Is_Cat09 = row[NamaField.Is_Cat09].ToString(),
                Is_Cat09_Time = Convert.ToDateTime(row[NamaField.Is_Cat09_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat09_Time])),
                Is_Cat10 = row[NamaField.Is_Cat10].ToString(),
                Is_Cat10_Time = Convert.ToDateTime(row[NamaField.Is_Cat10_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat10_Time])),
                Is_Cat11 = row[NamaField.Is_Cat11].ToString(),
                Is_Cat11_Time = Convert.ToDateTime(row[NamaField.Is_Cat11_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat11_Time])),
                Is_Cat12 = row[NamaField.Is_Cat12].ToString(),
                Is_Cat12_Time = Convert.ToDateTime(row[NamaField.Is_Cat12_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat12_Time])),
                Is_Cat13 = row[NamaField.Is_Cat13].ToString(),
                Is_Cat13_Time = Convert.ToDateTime(row[NamaField.Is_Cat13_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat13_Time])),
                Is_Cat14 = row[NamaField.Is_Cat14].ToString(),
                Is_Cat14_Time = Convert.ToDateTime(row[NamaField.Is_Cat14_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat14_Time])),
                Is_Cat15 = row[NamaField.Is_Cat15].ToString(),
                Is_Cat15_Time = Convert.ToDateTime(row[NamaField.Is_Cat15_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat15_Time])),
                Is_Cat16 = row[NamaField.Is_Cat16].ToString(),
                Is_Cat16_Time = Convert.ToDateTime(row[NamaField.Is_Cat16_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat16_Time])),
                Is_Cat17 = row[NamaField.Is_Cat17].ToString(),
                Is_Cat17_Time = Convert.ToDateTime(row[NamaField.Is_Cat17_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat17_Time])),
                Is_Cat18 = row[NamaField.Is_Cat18].ToString(),
                Is_Cat18_Time = Convert.ToDateTime(row[NamaField.Is_Cat18_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat18_Time])),
                Is_Cat19 = row[NamaField.Is_Cat19].ToString(),
                Is_Cat19_Time = Convert.ToDateTime(row[NamaField.Is_Cat19_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat19_Time])),
                Is_Cat20 = row[NamaField.Is_Cat20].ToString(),
                Is_Cat20_Time = Convert.ToDateTime(row[NamaField.Is_Cat20_Time] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Is_Cat20_Time])),
                
                Is_Sakit_Keterangan = row[NamaField.Is_Sakit_Keterangan].ToString(),
                Is_Izin_Keterangan = row[NamaField.Is_Izin_Keterangan].ToString(),
                Is_Alpa_Keterangan = row[NamaField.Is_Alpa_Keterangan].ToString(),

                Is_Cat01_Keterangan = row[NamaField.Is_Cat01_Keterangan].ToString(),
                Is_Cat02_Keterangan = row[NamaField.Is_Cat02_Keterangan].ToString(),
                Is_Cat03_Keterangan = row[NamaField.Is_Cat03_Keterangan].ToString(),
                Is_Cat04_Keterangan = row[NamaField.Is_Cat04_Keterangan].ToString(),
                Is_Cat05_Keterangan = row[NamaField.Is_Cat05_Keterangan].ToString(),
                Is_Cat06_Keterangan = row[NamaField.Is_Cat06_Keterangan].ToString(),
                Is_Cat07_Keterangan = row[NamaField.Is_Cat07_Keterangan].ToString(),
                Is_Cat08_Keterangan = row[NamaField.Is_Cat08_Keterangan].ToString(),
                Is_Cat09_Keterangan = row[NamaField.Is_Cat09_Keterangan].ToString(),
                Is_Cat10_Keterangan = row[NamaField.Is_Cat10_Keterangan].ToString(),
                Is_Cat11_Keterangan = row[NamaField.Is_Cat11_Keterangan].ToString(),
                Is_Cat12_Keterangan = row[NamaField.Is_Cat12_Keterangan].ToString(),
                Is_Cat13_Keterangan = row[NamaField.Is_Cat13_Keterangan].ToString(),
                Is_Cat14_Keterangan = row[NamaField.Is_Cat14_Keterangan].ToString(),
                Is_Cat15_Keterangan = row[NamaField.Is_Cat15_Keterangan].ToString(),
                Is_Cat16_Keterangan = row[NamaField.Is_Cat16_Keterangan].ToString(),
                Is_Cat17_Keterangan = row[NamaField.Is_Cat17_Keterangan].ToString(),
                Is_Cat18_Keterangan = row[NamaField.Is_Cat18_Keterangan].ToString(),
                Is_Cat19_Keterangan = row[NamaField.Is_Cat19_Keterangan].ToString(),
                Is_Cat20_Keterangan = row[NamaField.Is_Cat20_Keterangan].ToString()
            };
        }

        private static SiswaAbsenReport GetEntityFromDataRowReport(DataRow row)
        {
            return new SiswaAbsenReport
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString(),
                Rel_KelasDet = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),

                Rel_Mapel = row[NamaField.Rel_Mapel].ToString().ToUpper().Trim(),
                Mapel = row[NamaField.Mapel].ToString(),
                JenisMapel = row[NamaField.JenisMapel].ToString(),

                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                NamaSiswa = row[NamaField.NamaSiswa].ToString(),

                Rel_Linimasa = row[NamaField.Rel_Linimasa].ToString(),
                
                Is_Hadir = row[NamaField.Is_Hadir].ToString(),
                Is_Sakit = row[NamaField.Is_Sakit].ToString(),
                Is_Izin = row[NamaField.Is_Izin].ToString(),
                Is_Alpa = row[NamaField.Is_Alpa].ToString(),
                
                Is_Cat01 = row[NamaField.Is_Cat01].ToString(),
                Is_Cat02 = row[NamaField.Is_Cat02].ToString(),
                Is_Cat03 = row[NamaField.Is_Cat03].ToString(),
                Is_Cat04 = row[NamaField.Is_Cat04].ToString(),
                Is_Cat05 = row[NamaField.Is_Cat05].ToString(),
                Is_Cat06 = row[NamaField.Is_Cat06].ToString(),
                Is_Cat07 = row[NamaField.Is_Cat07].ToString(),
                Is_Cat08 = row[NamaField.Is_Cat08].ToString(),
                Is_Cat09 = row[NamaField.Is_Cat09].ToString(),
                Is_Cat10 = row[NamaField.Is_Cat10].ToString(),
                
                Is_Sakit_Keterangan = row[NamaField.Is_Sakit_Keterangan].ToString(),
                Is_Izin_Keterangan = row[NamaField.Is_Izin_Keterangan].ToString(),
                Is_Alpa_Keterangan = row[NamaField.Is_Alpa_Keterangan].ToString(),

                Is_Cat01_Keterangan = row[NamaField.Is_Cat01_Keterangan].ToString(),
                Is_Cat02_Keterangan = row[NamaField.Is_Cat02_Keterangan].ToString(),
                Is_Cat03_Keterangan = row[NamaField.Is_Cat03_Keterangan].ToString(),
                Is_Cat04_Keterangan = row[NamaField.Is_Cat04_Keterangan].ToString(),
                Is_Cat05_Keterangan = row[NamaField.Is_Cat05_Keterangan].ToString(),
                Is_Cat06_Keterangan = row[NamaField.Is_Cat06_Keterangan].ToString(),
                Is_Cat07_Keterangan = row[NamaField.Is_Cat07_Keterangan].ToString(),
                Is_Cat08_Keterangan = row[NamaField.Is_Cat08_Keterangan].ToString(),
                Is_Cat09_Keterangan = row[NamaField.Is_Cat09_Keterangan].ToString(),
                Is_Cat10_Keterangan = row[NamaField.Is_Cat10_Keterangan].ToString()
            };
        }

        public static void Delete(string Kode, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static void DeleteByLinimasa(string rel_linimasa, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE_BY_LINIMASA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Linimasa, rel_linimasa));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static void Insert(SiswaAbsen m, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Absen, m.Absen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Linimasa, m.Rel_Linimasa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kejadian, m.Kejadian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ButirSikap, m.ButirSikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ButirSikapLain, m.ButirSikapLain));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sikap, m.Sikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TindakLanjut, m.TindakLanjut));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Hadir, m.Is_Hadir));
                if (m.Is_Hadir_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Hadir_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Hadir_Time, Convert.ToDateTime(m.Is_Hadir_Time)));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit, m.Is_Sakit));
                if (m.Is_Sakit_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit_Time, Convert.ToDateTime(m.Is_Sakit_Time)));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin, m.Is_Izin));
                if (m.Is_Izin_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin_Time, Convert.ToDateTime(m.Is_Izin_Time)));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa, m.Is_Alpa));
                if (m.Is_Alpa_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa_Time, Convert.ToDateTime(m.Is_Alpa_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01, m.Is_Cat01));
                if (m.Is_Cat01_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01_Time, Convert.ToDateTime(m.Is_Cat01_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02, m.Is_Cat02));
                if (m.Is_Cat02_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02_Time, Convert.ToDateTime(m.Is_Cat02_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03, m.Is_Cat03));
                if (m.Is_Cat03_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03_Time, Convert.ToDateTime(m.Is_Cat03_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04, m.Is_Cat04));
                if (m.Is_Cat04_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04_Time, Convert.ToDateTime(m.Is_Cat04_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05, m.Is_Cat05));
                if (m.Is_Cat05_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05_Time, Convert.ToDateTime(m.Is_Cat05_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06, m.Is_Cat06));
                if (m.Is_Cat06_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06_Time, Convert.ToDateTime(m.Is_Cat06_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07, m.Is_Cat07));
                if (m.Is_Cat07_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07_Time, Convert.ToDateTime(m.Is_Cat07_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08, m.Is_Cat08));
                if (m.Is_Cat08_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08_Time, Convert.ToDateTime(m.Is_Cat08_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09, m.Is_Cat09));
                if (m.Is_Cat09_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09_Time, Convert.ToDateTime(m.Is_Cat09_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10, m.Is_Cat10));
                if (m.Is_Cat10_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10_Time, Convert.ToDateTime(m.Is_Cat10_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11, m.Is_Cat11));
                if (m.Is_Cat11_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11_Time, Convert.ToDateTime(m.Is_Cat11_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12, m.Is_Cat12));
                if (m.Is_Cat12_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12_Time, Convert.ToDateTime(m.Is_Cat12_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13, m.Is_Cat13));
                if (m.Is_Cat13_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13_Time, Convert.ToDateTime(m.Is_Cat13_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14, m.Is_Cat14));
                if (m.Is_Cat14_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14_Time, Convert.ToDateTime(m.Is_Cat14_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15, m.Is_Cat15));
                if (m.Is_Cat15_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15_Time, Convert.ToDateTime(m.Is_Cat15_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16, m.Is_Cat10));
                if (m.Is_Cat16_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16_Time, Convert.ToDateTime(m.Is_Cat16_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17, m.Is_Cat10));
                if (m.Is_Cat17_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17_Time, Convert.ToDateTime(m.Is_Cat17_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18, m.Is_Cat10));
                if (m.Is_Cat18_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18_Time, Convert.ToDateTime(m.Is_Cat18_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19, m.Is_Cat19));
                if (m.Is_Cat19_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19_Time, Convert.ToDateTime(m.Is_Cat19_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20, m.Is_Cat20));
                if (m.Is_Cat20_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20_Time, Convert.ToDateTime(m.Is_Cat20_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit_Keterangan, m.Is_Sakit_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin_Keterangan, m.Is_Izin_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa_Keterangan, m.Is_Alpa_Keterangan));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01_Keterangan, m.Is_Cat01_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02_Keterangan, m.Is_Cat02_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03_Keterangan, m.Is_Cat03_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04_Keterangan, m.Is_Cat04_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05_Keterangan, m.Is_Cat05_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06_Keterangan, m.Is_Cat06_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07_Keterangan, m.Is_Cat07_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08_Keterangan, m.Is_Cat08_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09_Keterangan, m.Is_Cat09_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10_Keterangan, m.Is_Cat10_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11_Keterangan, m.Is_Cat11_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12_Keterangan, m.Is_Cat12_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13_Keterangan, m.Is_Cat13_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14_Keterangan, m.Is_Cat14_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15_Keterangan, m.Is_Cat15_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16_Keterangan, m.Is_Cat16_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17_Keterangan, m.Is_Cat17_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18_Keterangan, m.Is_Cat18_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19_Keterangan, m.Is_Cat19_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20_Keterangan, m.Is_Cat20_Keterangan));

                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static void Update(SiswaAbsen m, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Absen, m.Absen));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kejadian, m.Kejadian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ButirSikap, m.ButirSikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ButirSikapLain, m.ButirSikapLain));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Sikap, m.Sikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TindakLanjut, m.TindakLanjut));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Hadir, m.Is_Hadir));
                if (m.Is_Hadir_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Hadir_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Hadir_Time, Convert.ToDateTime(m.Is_Hadir_Time)));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit, m.Is_Sakit));
                if (m.Is_Sakit_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit_Time, Convert.ToDateTime(m.Is_Sakit_Time)));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin, m.Is_Izin));
                if (m.Is_Izin_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin_Time, Convert.ToDateTime(m.Is_Izin_Time)));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa, m.Is_Alpa));
                if (m.Is_Alpa_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa_Time, Convert.ToDateTime(m.Is_Alpa_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01, m.Is_Cat01));
                if (m.Is_Cat01_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01_Time, Convert.ToDateTime(m.Is_Cat01_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02, m.Is_Cat02));
                if (m.Is_Cat02_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02_Time, Convert.ToDateTime(m.Is_Cat02_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03, m.Is_Cat03));
                if (m.Is_Cat03_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03_Time, Convert.ToDateTime(m.Is_Cat03_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04, m.Is_Cat04));
                if (m.Is_Cat04_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04_Time, Convert.ToDateTime(m.Is_Cat04_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05, m.Is_Cat05));
                if (m.Is_Cat05_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05_Time, Convert.ToDateTime(m.Is_Cat05_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06, m.Is_Cat06));
                if (m.Is_Cat06_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06_Time, Convert.ToDateTime(m.Is_Cat06_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07, m.Is_Cat07));
                if (m.Is_Cat07_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07_Time, Convert.ToDateTime(m.Is_Cat07_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08, m.Is_Cat08));
                if (m.Is_Cat08_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08_Time, Convert.ToDateTime(m.Is_Cat08_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09, m.Is_Cat09));
                if (m.Is_Cat09_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09_Time, Convert.ToDateTime(m.Is_Cat09_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10, m.Is_Cat10));
                if (m.Is_Cat10_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10_Time, Convert.ToDateTime(m.Is_Cat10_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11, m.Is_Cat11));
                if (m.Is_Cat11_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11_Time, Convert.ToDateTime(m.Is_Cat11_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12, m.Is_Cat12));
                if (m.Is_Cat12_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12_Time, Convert.ToDateTime(m.Is_Cat12_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13, m.Is_Cat13));
                if (m.Is_Cat13_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13_Time, Convert.ToDateTime(m.Is_Cat13_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14, m.Is_Cat14));
                if (m.Is_Cat14_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14_Time, Convert.ToDateTime(m.Is_Cat14_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15, m.Is_Cat15));
                if (m.Is_Cat15_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15_Time, Convert.ToDateTime(m.Is_Cat15_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16, m.Is_Cat10));
                if (m.Is_Cat16_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16_Time, Convert.ToDateTime(m.Is_Cat16_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17, m.Is_Cat10));
                if (m.Is_Cat17_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17_Time, Convert.ToDateTime(m.Is_Cat17_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18, m.Is_Cat10));
                if (m.Is_Cat18_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18_Time, Convert.ToDateTime(m.Is_Cat18_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19, m.Is_Cat19));
                if (m.Is_Cat19_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19_Time, Convert.ToDateTime(m.Is_Cat19_Time)));
                }

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20, m.Is_Cat20));
                if (m.Is_Cat20_Time == DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20_Time, DBNull.Value));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20_Time, Convert.ToDateTime(m.Is_Cat20_Time)));
                }
                
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Sakit_Keterangan, m.Is_Sakit_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Izin_Keterangan, m.Is_Izin_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Alpa_Keterangan, m.Is_Alpa_Keterangan));

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat01_Keterangan, m.Is_Cat01_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat02_Keterangan, m.Is_Cat02_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat03_Keterangan, m.Is_Cat03_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat04_Keterangan, m.Is_Cat04_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat05_Keterangan, m.Is_Cat05_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat06_Keterangan, m.Is_Cat06_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat07_Keterangan, m.Is_Cat07_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat08_Keterangan, m.Is_Cat08_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat09_Keterangan, m.Is_Cat09_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat10_Keterangan, m.Is_Cat10_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat11_Keterangan, m.Is_Cat11_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat12_Keterangan, m.Is_Cat12_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat13_Keterangan, m.Is_Cat13_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat14_Keterangan, m.Is_Cat14_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat15_Keterangan, m.Is_Cat15_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat16_Keterangan, m.Is_Cat16_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat17_Keterangan, m.Is_Cat17_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat18_Keterangan, m.Is_Cat18_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat19_Keterangan, m.Is_Cat19_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Is_Cat20_Keterangan, m.Is_Cat20_Keterangan));

                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
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

        public static List<SiswaAbsen> GetAllBySekolahByKelasDetByTanggal_Entity(
                string rel_sekolah,
                string rel_kelasdet,
                DateTime tanggal
            )
        {
            List<SiswaAbsen> hasil = new List<SiswaAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SEKOLAH_BY_KELASDET_BY_TANGGAL;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.Tanggal, tanggal);

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

        public static List<SiswaAbsen> GetBySiswaByPeriode_Entity(
                string rel_siswa,
                DateTime tanggal_awal,
                DateTime tanggal_akhir
            )
        {
            List<SiswaAbsen> hasil = new List<SiswaAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SISWA_BY_PERIODE;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);
                comm.Parameters.AddWithValue("@TanggalAwal", tanggal_awal);
                comm.Parameters.AddWithValue("@TanggalAkhir", tanggal_akhir);

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

        public static List<PeriodeTahunAjaranAbsen> GetPeriode_Entity()
        {
            List<PeriodeTahunAjaranAbsen> hasil = new List<PeriodeTahunAjaranAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SELECT_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new PeriodeTahunAjaranAbsen {
                        TahunAjaran = row["TahunAjaran"].ToString(),
                        Semester = row["Semester"].ToString()
                    });
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

        public static List<SiswaAbsenReport> GetByUnitByKelasDetByTanggal_Entity(
                DateTime dari_tanggal,
                DateTime sampai_tanggal,
                string rel_sekolah,
                string rel_kelasdet
            )
        {
            List<SiswaAbsenReport> hasil = new List<SiswaAbsenReport>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SELECT_BY_UNIT_BY_KELASDET_BY_TANGGAL;
                comm.Parameters.AddWithValue("@DariTanggal", dari_tanggal);
                comm.Parameters.AddWithValue("@SampaiTanggal", sampai_tanggal);
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowReport(row));
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

        public static List<AbsenRekapRapor> GetRekapAbsenRaporBySiswaByPeriode_Entity(
                string rel_siswa,
                DateTime tanggal_awal,
                DateTime tanggal_akhir
            )
        {
            List<AbsenRekapRapor> hasil = new List<AbsenRekapRapor>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_REKAP_ABSEN_RAPOR_BY_SISWA_BY_PERIODE;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);
                comm.Parameters.AddWithValue("@TanggalAwal", tanggal_awal);
                comm.Parameters.AddWithValue("@TanggalAkhir", tanggal_akhir);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new AbsenRekapRapor {
                                Absen = row["Absen"].ToString(),
                                Jumlah = Convert.ToInt16(row["JumlahAbsen"])
                            }
                        );
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
        
        public static List<String> GetDistinctKeteranganKategori_Entity(
                string nama_field_kategori,
                string value_field_kategori,
                string nama_field_distinct
            )
        {
            List<String> hasil = new List<String>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SELECT_DISTINCT_KETERANGAN;
                comm.Parameters.AddWithValue("@NamaFieldKategori", nama_field_kategori);
                comm.Parameters.AddWithValue("@ValueFieldKategori", value_field_kategori);
                comm.Parameters.AddWithValue("@NamaFieldDistinct", nama_field_distinct);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(row[0].ToString());
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

        public static List<AbsenRekapRaporSiswa> GetRekapAbsenRaporByPeriode_Entity(
                DateTime tanggal_awal,
                DateTime tanggal_akhir
            )
        {
            List<AbsenRekapRaporSiswa> hasil = new List<AbsenRekapRaporSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_REKAP_ABSEN_RAPOR_BY_PERIODE;
                comm.Parameters.AddWithValue("@TanggalAwal", tanggal_awal);
                comm.Parameters.AddWithValue("@TanggalAkhir", tanggal_akhir);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new AbsenRekapRaporSiswa
                            {
                                Rel_Siswa = row["Rel_Siswa"].ToString(),
                                Absen = row["Absen"].ToString(),
                                Jumlah = Convert.ToInt16(row["JumlahAbsen"])
                            }
                        );
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

        public static List<SiswaAbsen> GetAllByLiniMasa_Entity(
                string rel_linimasa
            )
        {
            List<SiswaAbsen> hasil = new List<SiswaAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_LINIMASA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Linimasa, rel_linimasa);

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

        public static List<SiswaAbsen> GetAllBySekolahByKelasDetBySiswaByTanggal_Entity(
                string rel_sekolah,
                string rel_kelasdet,
                string rel_siswa,
                DateTime tanggal
            )
        {
            List<SiswaAbsen> hasil = new List<SiswaAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SEKOLAH_BY_KELASDET_BY_SISWA_BY_TANGGAL;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);
                comm.Parameters.AddWithValue("@" + NamaField.Tanggal, tanggal);

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

        public static List<PeriodeAbsen> GetPeriodeBySekolahByKelasDetByTA_Entity(
                string rel_sekolah,
                string rel_kelasdet,
                string tahun_ajaran
            )
        {
            List<PeriodeAbsen> hasil = new List<PeriodeAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_PERIODE_BY_SEKOLAH_BY_KELASDET_BY_TA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(new PeriodeAbsen {
                        Tahun = Convert.ToInt16(row["Tahun"]),
                        Bulan = Convert.ToInt16(row["Bulan"])
                    });
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

        public static List<SiswaAbsen> GetByID_Entity(
                string kode
            )
        {
            List<SiswaAbsen> hasil = new List<SiswaAbsen>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

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
    }
}