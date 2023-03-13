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
    public static class DAO_Rapor_StrukturNilai_KURTILAS_AP
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_StrukturNilai_KURTILAS_AP_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_StrukturNilai_KURTILAS_AP_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_HEADER = "SMA_Rapor_StrukturNilai_KURTILAS_AP_SELECT_ALL_BY_HEADER";
        public const string SP_SELECT_ALL_BY_TA_BY_SM = "SMA_Rapor_StrukturNilai_KURTILAS_AP_SELECT_ALL_BY_TA_BY_SM";
        public const string SP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS = "SMA_Rapor_StrukturNilai_KURTILAS_AP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_KURTILAS_AP_SELECT_BY_ID";

        public const string SP_INSERT = "SMA_Rapor_StrukturNilai_KURTILAS_AP_INSERT";
        public const string SP_INSERT_LENGKAP = "SMA_Rapor_StrukturNilai_KURTILAS_AP_INSERT_LENGKAP";

        public const string SP_UPDATE = "SMA_Rapor_StrukturNilai_KURTILAS_AP_UPDATE";

        public const string SP_DELETE = "SMA_Rapor_StrukturNilai_KURTILAS_AP_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai = "Rel_Rapor_StrukturNilai";
            public const string Poin = "Poin";
            public const string Rel_Rapor_AspekPenilaian = "Rel_Rapor_AspekPenilaian";
            public const string JenisPerhitungan = "JenisPerhitungan";
            public const string BobotRapor = "BobotRapor";
            public const string Urutan = "Urutan";
            public const string KompetensiDasar = "KompetensiDasar";
        }

        private static Rapor_StrukturNilai_KURTILAS_AP GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KURTILAS_AP
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai = new Guid(row[NamaField.Rel_Rapor_StrukturNilai].ToString()),
                Poin = row[NamaField.Poin].ToString(),
                Rel_Rapor_AspekPenilaian = new Guid(row[NamaField.Rel_Rapor_AspekPenilaian].ToString()),
                JenisPerhitungan = row[NamaField.JenisPerhitungan].ToString(),
                BobotRapor = Convert.ToDecimal(row[NamaField.BobotRapor]),
                Urutan = Convert.ToInt16(row[NamaField.Urutan]),
                KompetensiDasar = row[NamaField.KompetensiDasar].ToString()
            };
        }

        public static List<Rapor_StrukturNilai_KURTILAS_AP> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KURTILAS_AP> hasil = new List<Rapor_StrukturNilai_KURTILAS_AP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static List<Rapor_StrukturNilai_KURTILAS_AP> GetAllByHeader_Entity(string rel_rapor_strukturnilai)
        {
            List<Rapor_StrukturNilai_KURTILAS_AP> hasil = new List<Rapor_StrukturNilai_KURTILAS_AP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_StrukturNilai, rel_rapor_strukturnilai);

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

        public static List<Rapor_StrukturNilai_KURTILAS_AP> GetAllByTABySM_Entity(string tahun_ajaran, string semester)
        {
            List<Rapor_StrukturNilai_KURTILAS_AP> hasil = new List<Rapor_StrukturNilai_KURTILAS_AP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_TA_BY_SM;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

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

        public static List<Rapor_StrukturNilai_KURTILAS_AP> GetAllByTABySMByKelas_Entity(string tahun_ajaran, string semester, string rel_kelas)
        {
            List<Rapor_StrukturNilai_KURTILAS_AP> hasil = new List<Rapor_StrukturNilai_KURTILAS_AP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_TA_BY_SM_BY_KELAS;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);

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

        public static Rapor_StrukturNilai_KURTILAS_AP GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KURTILAS_AP hasil = new Rapor_StrukturNilai_KURTILAS_AP();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
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

        public static void Insert(Rapor_StrukturNilai_KURTILAS_AP m, string user_id)
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

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_AspekPenilaian, m.Rel_Rapor_AspekPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRapor, m.BobotRapor));
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

        public static void InsertLengkap(Rapor_StrukturNilai_KURTILAS_AP m, string user_id)
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
                comm.CommandText = SP_INSERT_LENGKAP;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_AspekPenilaian, m.Rel_Rapor_AspekPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRapor, m.BobotRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));
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

        public static void Update(Rapor_StrukturNilai_KURTILAS_AP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai, m.Rel_Rapor_StrukturNilai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Poin, m.Poin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_AspekPenilaian, m.Rel_Rapor_AspekPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRapor, m.BobotRapor));
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
    }
}