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
    public static class DAO_FormasiWaliKelas
    {
        public const string SP_SELECT_ALL = "FormasiWaliKelas_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "FormasiWaliKelas_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "FormasiWaliKelas_SELECT_BY_ID";

        public const string SP_INSERT = "FormasiWaliKelas_INSERT";

        public const string SP_UPDATE = "FormasiWaliKelas_UPDATE";

        public const string SP_DELETE = "FormasiWaliKelas_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Walas = "Rel_Walas";
            public const string Rel_KelasDet = "Rel_KelasDet";
        }

        public static FormasiWaliKelas GetEntityFromDataRow(DataRow row)
        {
            return new FormasiWaliKelas
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Sekolah = new Guid(row[NamaField.Rel_Sekolah].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Walas = row[NamaField.Rel_Walas].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString()
            };
        }

        public static List<FormasiWaliKelas> GetAll_Entity()
        {
            List<FormasiWaliKelas> hasil = new List<FormasiWaliKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
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

        public static FormasiWaliKelas GetByID_Entity(string kode)
        {
            FormasiWaliKelas hasil = new FormasiWaliKelas();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
        public static void Insert(FormasiWaliKelas formasi_guru, string user_id)
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
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, formasi_guru.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, formasi_guru.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, formasi_guru.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Walas, formasi_guru.Rel_Walas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, formasi_guru.Rel_KelasDet));
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

        public static void Update(FormasiWaliKelas formasi_guru, string user_id)
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
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, formasi_guru.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, formasi_guru.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, formasi_guru.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, formasi_guru.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Walas, formasi_guru.Rel_Walas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, formasi_guru.Rel_KelasDet));
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