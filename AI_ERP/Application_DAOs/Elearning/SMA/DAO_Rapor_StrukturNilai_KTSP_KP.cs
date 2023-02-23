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
    public class DAO_Rapor_StrukturNilai_KTSP_KP
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_StrukturNilai_KTSP_KP_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_StrukturNilai_KTSP_KP_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_HEADER = "SMA_Rapor_StrukturNilai_KTSP_KP_SELECT_ALL_BY_HEADER";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_KTSP_KP_SELECT_BY_ID";

        public const string SP_INSERT = "SMA_Rapor_StrukturNilai_KTSP_KP_INSERT";
        public const string SP_INSERT_LENGKAP = "SMA_Rapor_StrukturNilai_KTSP_KP_INSERT_LENGKAP";

        public const string SP_UPDATE = "SMA_Rapor_StrukturNilai_KTSP_KP_UPDATE";

        public const string SP_DELETE = "SMA_Rapor_StrukturNilai_KTSP_KP_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai_KTSP_KD = "Rel_Rapor_StrukturNilai_KTSP_KD";
            public const string Rel_Rapor_KomponenPenilaian = "Rel_Rapor_KomponenPenilaian";
            public const string BobotNKD = "BobotNKD";
            public const string Jenis = "Jenis";
            public const string Urutan = "Urutan";
        }

        private static Rapor_StrukturNilai_KTSP_KP GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KTSP_KP
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai_KTSP_KD = new Guid(row[NamaField.Rel_Rapor_StrukturNilai_KTSP_KD].ToString()),
                Rel_Rapor_KomponenPenilaian = new Guid(row[NamaField.Rel_Rapor_KomponenPenilaian].ToString()),
                BobotNKD = Convert.ToDecimal(row[NamaField.BobotNKD]),
                Jenis = row[NamaField.Jenis].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<Rapor_StrukturNilai_KTSP_KP> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KTSP_KP> hasil = new List<Rapor_StrukturNilai_KTSP_KP>();
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

        public static List<Rapor_StrukturNilai_KTSP_KP> GetAllByHeader_Entity(string rel_rapor_strukturnilai_ktsp_kd)
        {
            List<Rapor_StrukturNilai_KTSP_KP> hasil = new List<Rapor_StrukturNilai_KTSP_KP>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KD, rel_rapor_strukturnilai_ktsp_kd);

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

        public static Rapor_StrukturNilai_KTSP_KP GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KTSP_KP hasil = new Rapor_StrukturNilai_KTSP_KP();
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

        public static void Insert(Rapor_StrukturNilai_KTSP_KP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KD, m.Rel_Rapor_StrukturNilai_KTSP_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNKD, m.BobotNKD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
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

        public static void InsertLengkap(Rapor_StrukturNilai_KTSP_KP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KD, m.Rel_Rapor_StrukturNilai_KTSP_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNKD, m.BobotNKD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
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

        public static void Update(Rapor_StrukturNilai_KTSP_KP m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KTSP_KD, m.Rel_Rapor_StrukturNilai_KTSP_KD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KomponenPenilaian, m.Rel_Rapor_KomponenPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotNKD, m.BobotNKD));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
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