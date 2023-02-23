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
    public class DAO_Rapor_StrukturNilai_KTSP_KD
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_StrukturNilai_KTSP_KD_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_StrukturNilai_KTSP_KD_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_HEADER = "SMA_Rapor_StrukturNilai_KTSP_KD_SELECT_ALL_BY_HEADER";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_KTSP_KD_SELECT_BY_ID";        

        public const string SP_INSERT = "SMA_Rapor_StrukturNilai_KTSP_KD_INSERT";
        public const string SP_INSERT_LENGKAP = "SMA_Rapor_StrukturNilai_KTSP_KD_INSERT_LENGKAP";

        public const string SP_UPDATE = "SMA_Rapor_StrukturNilai_KTSP_KD_UPDATE";

        public const string SP_DELETE = "SMA_Rapor_StrukturNilai_KTSP_KD_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai = "Rel_Rapor_StrukturNilai";
            public const string Poin = "Poin";
            public const string Rel_Rapor_KompetensiDasar = "Rel_Rapor_KompetensiDasar";
            public const string BobotRaporPPK = "BobotRaporPPK";
            public const string BobotRaporP = "BobotRaporP";
            public const string JenisPerhitungan = "JenisPerhitungan";
            public const string Urutan = "Urutan";
        }

        private static Rapor_StrukturNilai_KTSP_KD GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KTSP_KD
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_StrukturNilai = new Guid(row[NamaField.Rel_Rapor_StrukturNilai].ToString()),
                Poin = row[NamaField.Poin].ToString(),
                Rel_Rapor_KompetensiDasar = new Guid(row[NamaField.Rel_Rapor_KompetensiDasar].ToString()),
                BobotRaporPPK = Convert.ToDecimal(row[NamaField.BobotRaporPPK]),
                BobotRaporP = Convert.ToDecimal(row[NamaField.BobotRaporP]),
                JenisPerhitungan = row[NamaField.JenisPerhitungan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<Rapor_StrukturNilai_KTSP_KD> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KTSP_KD> hasil = new List<Rapor_StrukturNilai_KTSP_KD>();
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

        public static List<Rapor_StrukturNilai_KTSP_KD> GetAllByHeader_Entity(string rel_rapor_strukturnilai)
        {
            List<Rapor_StrukturNilai_KTSP_KD> hasil = new List<Rapor_StrukturNilai_KTSP_KD>();
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

        public static Rapor_StrukturNilai_KTSP_KD GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KTSP_KD hasil = new Rapor_StrukturNilai_KTSP_KD();
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

        public static void Insert(Rapor_StrukturNilai_KTSP_KD m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KompetensiDasar, m.Rel_Rapor_KompetensiDasar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporPPK, m.BobotRaporPPK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporP, m.BobotRaporP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
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

        public static void InsertLengkap(Rapor_StrukturNilai_KTSP_KD m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KompetensiDasar, m.Rel_Rapor_KompetensiDasar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporPPK, m.BobotRaporPPK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporP, m.BobotRaporP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
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

        public static void Update(Rapor_StrukturNilai_KTSP_KD m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_KompetensiDasar, m.Rel_Rapor_KompetensiDasar));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporPPK, m.BobotRaporPPK));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporP, m.BobotRaporP));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
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