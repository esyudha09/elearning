using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_NilaiSiswa_KTSP
    {
        public const string SP_SELECT_BY_HEADER = "SMA_Rapor_NilaiSiswa_KTSP_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_HEADER_BY_SISWA = "SMA_Rapor_NilaiSiswa_KTSP_SELECT_BY_HEADER_BY_SISWA";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_NilaiSiswa_KURTILAS_SELECT_BY_ID";

        public const string SP_INSERT = "SMA_Rapor_NilaiSiswa_KTSP_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_NilaiSiswa_KTSP_UPDATE";

        public const string SP_DELETE = "SMA_Rapor_NilaiSiswa_KTSP_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Nilai = "Rel_Rapor_Nilai";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rapor_PPK = "Rapor_PPK";
            public const string Rapor_P = "Rapor_P";
            public const string Rapor_Sikap = "Rapor_Sikap";

            public const string LTS_HD = "LTS_HD";
            public const string LTS_MAX_HD = "LTS_MAX_HD";
            public const string LTS_S = "LTS_S";
            public const string LTS_I = "LTS_I";
            public const string LTS_A = "LTS_A";
            public const string LTS_LK = "LTS_LK";
            public const string LTS_RJ = "LTS_RJ";
            public const string LTS_RPKB = "LTS_RPKB";

            public const string SM_HD = "SM_HD";
            public const string SM_MAX_HD = "SM_MAX_HD";
            public const string SM_S = "SM_S";
            public const string SM_I = "SM_I";
            public const string SM_A = "SM_A";
            public const string SM_LK = "SM_LK";
            public const string SM_RJ = "SM_RJ";
            public const string SM_RPKB = "SM_RPKB";

            public const string LTS_CK_KEHADIRAN = "LTS_CK_KEHADIRAN";
            public const string LTS_CK_KETEPATAN_WKT = "LTS_CK_KETEPATAN_WKT";
            public const string LTS_CK_PENGGUNAAN_SRGM = "LTS_CK_PENGGUNAAN_SRGM";
            public const string LTS_CK_PENGGUNAAN_KMR = "LTS_CK_PENGGUNAAN_KMR";

            public const string SM_CK_KEHADIRAN = "SM_CK_KEHADIRAN";
            public const string SM_CK_KETEPATAN_WKT = "SM_CK_KETEPATAN_WKT";
            public const string SM_CK_PENGGUNAAN_SRGM = "SM_CK_PENGGUNAAN_SRGM";
            public const string SM_CK_PENGGUNAAN_KMR = "SM_CK_PENGGUNAAN_KMR";
        }

        private static Rapor_NilaiSiswa_KTSP GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiSiswa_KTSP
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Nilai = new Guid(row[NamaField.Rel_Rapor_Nilai].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rapor_PPK = row[NamaField.Rapor_PPK].ToString(),
                Rapor_P = row[NamaField.Rapor_P].ToString(),
                Rapor_Sikap = row[NamaField.Rapor_Sikap].ToString(),

                LTS_HD = row[NamaField.LTS_HD].ToString(),
                LTS_MAX_HD = row[NamaField.LTS_MAX_HD].ToString(),
                LTS_S = row[NamaField.LTS_S].ToString(),
                LTS_I = row[NamaField.LTS_I].ToString(),
                LTS_A = row[NamaField.LTS_A].ToString(),
                LTS_LK = row[NamaField.LTS_LK].ToString(),
                LTS_RJ = row[NamaField.LTS_RJ].ToString(),
                LTS_RPKB = row[NamaField.LTS_RPKB].ToString(),

                SM_HD = row[NamaField.SM_HD].ToString(),
                SM_MAX_HD = row[NamaField.SM_MAX_HD].ToString(),
                SM_S = row[NamaField.SM_S].ToString(),
                SM_I = row[NamaField.SM_I].ToString(),
                SM_A = row[NamaField.SM_A].ToString(),
                SM_LK = row[NamaField.SM_LK].ToString(),
                SM_RJ = row[NamaField.SM_RJ].ToString(),
                SM_RPKB = row[NamaField.SM_RPKB].ToString(),

                LTS_CK_KEHADIRAN = row[NamaField.LTS_CK_KEHADIRAN].ToString(),
                LTS_CK_KETEPATAN_WKT = row[NamaField.LTS_CK_KETEPATAN_WKT].ToString(),
                LTS_CK_PENGGUNAAN_SRGM = row[NamaField.LTS_CK_PENGGUNAAN_SRGM].ToString(),
                LTS_CK_PENGGUNAAN_KMR = row[NamaField.LTS_CK_PENGGUNAAN_KMR].ToString(),

                SM_CK_KEHADIRAN = row[NamaField.SM_CK_KEHADIRAN].ToString(),
                SM_CK_KETEPATAN_WKT = row[NamaField.SM_CK_KETEPATAN_WKT].ToString(),
                SM_CK_PENGGUNAAN_SRGM = row[NamaField.SM_CK_PENGGUNAAN_SRGM].ToString(),
                SM_CK_PENGGUNAAN_KMR = row[NamaField.SM_CK_PENGGUNAAN_KMR].ToString()
            };
        }

        public static void Delete(string Kode, string user_id)
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

        public static void Insert(Rapor_NilaiSiswa_KTSP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Nilai, m.Rel_Rapor_Nilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor_PPK, m.Rapor_PPK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor_P, m.Rapor_P));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor_Sikap, m.Rapor_Sikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_HD, m.LTS_HD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_MAX_HD, m.LTS_MAX_HD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_LK, m.LTS_LK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_RJ, m.LTS_RJ));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_RPKB, m.LTS_RPKB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KEHADIRAN, m.LTS_CK_KEHADIRAN));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KETEPATAN_WKT, m.LTS_CK_KETEPATAN_WKT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_SRGM, m.LTS_CK_PENGGUNAAN_SRGM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_KMR, m.LTS_CK_PENGGUNAAN_KMR));
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

        public static void Update(Rapor_NilaiSiswa_KTSP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Nilai, m.Rel_Rapor_Nilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor_PPK, m.Rapor_PPK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor_P, m.Rapor_P));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rapor_Sikap, m.Rapor_Sikap));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_HD, m.LTS_HD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_MAX_HD, m.LTS_MAX_HD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_LK, m.LTS_LK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_RJ, m.LTS_RJ));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_RPKB, m.LTS_RPKB));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KEHADIRAN, m.LTS_CK_KEHADIRAN));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_KETEPATAN_WKT, m.LTS_CK_KETEPATAN_WKT));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_SRGM, m.LTS_CK_PENGGUNAAN_SRGM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LTS_CK_PENGGUNAAN_KMR, m.LTS_CK_PENGGUNAAN_KMR));
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

        public static List<Rapor_NilaiSiswa_KTSP> GetAllByHeader_Entity(
                string rel_rapor_nilai
            )
        {
            List<Rapor_NilaiSiswa_KTSP> hasil = new List<Rapor_NilaiSiswa_KTSP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Nilai, rel_rapor_nilai);

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

        public static Rapor_NilaiSiswa_KTSP GetByID_Entity(
                string kode
            )
        {
            Rapor_NilaiSiswa_KTSP hasil = new Rapor_NilaiSiswa_KTSP();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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
                    hasil = GetEntityFromDataRow(row);
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

        public static List<Rapor_NilaiSiswa_KTSP> GetAllByHeaderBySiswa_Entity(
                string rel_rapor_nilai,
                string rel_siswa
            )
        {
            List<Rapor_NilaiSiswa_KTSP> hasil = new List<Rapor_NilaiSiswa_KTSP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_Nilai, rel_rapor_nilai);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);

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