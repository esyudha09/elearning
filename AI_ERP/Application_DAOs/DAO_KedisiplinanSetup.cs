using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_KedisiplinanSetup
    {
        public const string SP_SELECT_ALL = "KedisiplinanSetup_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "KedisiplinanSetup_SELECT_BY_ID";
        public const string SP_SELECT_BY_SEKOLAH_BY_KELAS = "KedisiplinanSetup_SELECT_BY_SEKOLAH_BY_KELAS";
        public const string SP_SELECT_BY_TA_BY_SM_BY_SEKOLAH_BY_KELAS = "KedisiplinanSetup_SELECT_BY_TA_BY_SM_BY_SEKOLAH_BY_KELAS";

        public const string SP_INSERT = "KedisiplinanSetup_INSERT";

        public const string SP_UPDATE = "KedisiplinanSetup_UPDATE";

        public const string SP_DELETE = "KedisiplinanSetup_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Kedisiplinan_01 = "Rel_Kedisiplinan_01";
            public const string Rel_Kedisiplinan_02 = "Rel_Kedisiplinan_02";
            public const string Rel_Kedisiplinan_03 = "Rel_Kedisiplinan_03";
            public const string Rel_Kedisiplinan_04 = "Rel_Kedisiplinan_04";
            public const string Rel_Kedisiplinan_05 = "Rel_Kedisiplinan_05";
            public const string Rel_Kedisiplinan_06 = "Rel_Kedisiplinan_06";
            public const string Rel_Kedisiplinan_07 = "Rel_Kedisiplinan_07";
            public const string Rel_Kedisiplinan_08 = "Rel_Kedisiplinan_08";
            public const string Rel_Kedisiplinan_09 = "Rel_Kedisiplinan_09";
            public const string Rel_Kedisiplinan_10 = "Rel_Kedisiplinan_10";
        }

        public static KedisiplinanSetup GetEntityFromDataRow(DataRow row)
        {
            return new KedisiplinanSetup
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Sekolah = row[NamaField.Semester].ToString(),
                Rel_Kelas = row[NamaField.Semester].ToString(),
                Rel_Kedisiplinan_01 = row[NamaField.Rel_Kedisiplinan_01].ToString(),
                Rel_Kedisiplinan_02 = row[NamaField.Rel_Kedisiplinan_02].ToString(),
                Rel_Kedisiplinan_03 = row[NamaField.Rel_Kedisiplinan_03].ToString(),
                Rel_Kedisiplinan_04 = row[NamaField.Rel_Kedisiplinan_04].ToString(),
                Rel_Kedisiplinan_05 = row[NamaField.Rel_Kedisiplinan_05].ToString(),
                Rel_Kedisiplinan_06 = row[NamaField.Rel_Kedisiplinan_06].ToString(),
                Rel_Kedisiplinan_07 = row[NamaField.Rel_Kedisiplinan_07].ToString(),
                Rel_Kedisiplinan_08 = row[NamaField.Rel_Kedisiplinan_08].ToString(),
                Rel_Kedisiplinan_09 = row[NamaField.Rel_Kedisiplinan_09].ToString(),
                Rel_Kedisiplinan_10 = row[NamaField.Rel_Kedisiplinan_10].ToString()
            };
        }

        public static List<KedisiplinanSetup> GetAll_Entity()
        {
            List<KedisiplinanSetup> hasil = new List<KedisiplinanSetup>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;

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

        public static List<KedisiplinanSetup> GetBySekolahByKelas_Entity(string rel_sekolah, string rel_kelas)
        {
            List<KedisiplinanSetup> hasil = new List<KedisiplinanSetup>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SEKOLAH_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

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

        public static List<KedisiplinanSetup> GetByTABySMBySekolahByKelas_Entity(string tahun_ajaran, string semester, string rel_sekolah, string rel_kelas)
        {
            List<KedisiplinanSetup> hasil = new List<KedisiplinanSetup>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_SEKOLAH_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

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

        public static KedisiplinanSetup GetByID_Entity(string kode)
        {
            KedisiplinanSetup hasil = new KedisiplinanSetup();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
            try
            {
                conn.Open();
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

        public static void Delete(string Kode)
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

        public static void Insert(KedisiplinanSetup KedisiplinanSetup)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, KedisiplinanSetup.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, KedisiplinanSetup.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, KedisiplinanSetup.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, KedisiplinanSetup.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_01, KedisiplinanSetup.Rel_Kedisiplinan_01));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_02, KedisiplinanSetup.Rel_Kedisiplinan_02));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_03, KedisiplinanSetup.Rel_Kedisiplinan_03));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_04, KedisiplinanSetup.Rel_Kedisiplinan_04));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_05, KedisiplinanSetup.Rel_Kedisiplinan_05));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_06, KedisiplinanSetup.Rel_Kedisiplinan_06));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_07, KedisiplinanSetup.Rel_Kedisiplinan_07));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_08, KedisiplinanSetup.Rel_Kedisiplinan_08));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_09, KedisiplinanSetup.Rel_Kedisiplinan_09));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_10, KedisiplinanSetup.Rel_Kedisiplinan_10));
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

        public static void Update(KedisiplinanSetup KedisiplinanSetup)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, KedisiplinanSetup.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, KedisiplinanSetup.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, KedisiplinanSetup.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, KedisiplinanSetup.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, KedisiplinanSetup.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_01, KedisiplinanSetup.Rel_Kedisiplinan_01));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_02, KedisiplinanSetup.Rel_Kedisiplinan_02));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_03, KedisiplinanSetup.Rel_Kedisiplinan_03));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_04, KedisiplinanSetup.Rel_Kedisiplinan_04));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_05, KedisiplinanSetup.Rel_Kedisiplinan_05));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_06, KedisiplinanSetup.Rel_Kedisiplinan_06));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_07, KedisiplinanSetup.Rel_Kedisiplinan_07));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_08, KedisiplinanSetup.Rel_Kedisiplinan_08));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_09, KedisiplinanSetup.Rel_Kedisiplinan_09));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kedisiplinan_10, KedisiplinanSetup.Rel_Kedisiplinan_10));
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
    }
}