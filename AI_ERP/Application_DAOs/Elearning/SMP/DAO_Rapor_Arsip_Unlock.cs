using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_Rapor_Arsip_Unlock
    {
        public const string SP_SELECT_ALL = "SMP_Rapor_Arsip_Unlock_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMP_Rapor_Arsip_Unlock_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SMP_Rapor_Arsip_Unlock_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "SMP_Rapor_Arsip_Unlock_SELECT_BY_HEADER";

        public const string SP_INSERT = "SMP_Rapor_Arsip_Unlock_INSERT";

        public const string SP_UPDATE = "SMP_Rapor_Arsip_Unlock_UPDATE";

        public const string SP_DELETE = "SMP_Rapor_Arsip_Unlock_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_Arsip = "Rel_Rapor_Arsip";
            public const string Tanggal = "Tanggal";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Guru = "Rel_Guru";
            public const string Alasan = "Alasan";
            public const string IsClosed = "IsClosed";
        }

        private static Rapor_Arsip_Unlock GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Arsip_Unlock
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Rapor_Arsip = new Guid(row[NamaField.Rel_Rapor_Arsip].ToString()),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Alasan = row[NamaField.Alasan].ToString(),
                IsClosed = (row[NamaField.IsClosed] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsClosed]))
            };
        }

        public static List<Rapor_Arsip_Unlock> GetAll_Entity()
        {
            List<Rapor_Arsip_Unlock> hasil = new List<Rapor_Arsip_Unlock>();
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

        public static Rapor_Arsip_Unlock GetByID_Entity(string kode)
        {
            Rapor_Arsip_Unlock hasil = new Rapor_Arsip_Unlock();
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

        public static void Insert(Rapor_Arsip_Unlock m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Arsip, m.Rel_Rapor_Arsip));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alasan, m.Alasan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsClosed, m.IsClosed));
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

        public static void Update(Rapor_Arsip_Unlock m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_Arsip, m.Rel_Rapor_Arsip));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alasan, m.Alasan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsClosed, m.IsClosed));
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