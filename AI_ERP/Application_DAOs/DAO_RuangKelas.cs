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
    public static class DAO_RuangKelas
    {
        public const string SP_SELECT_ALL = "RuangKelas_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "RuangKelas_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_NAMA = "RuangKelas_SELECT_ALL_BY_NAMA";
        public const string SP_SELECT_BY_ID = "RuangKelas_SELECT_BY_ID";

        public const string SP_INSERT = "RuangKelas_INSERT";

        public const string SP_UPDATE = "RuangKelas_UPDATE";

        public const string SP_DELETE = "RuangKelas_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Nama = "Nama";
            public const string Keterangan = "Keterangan";
            public const string Rel_Sekolah = "Rel_Sekolah";
        }

        public static RuangKelas GetEntityFromDataRow(DataRow row)
        {
            return new RuangKelas
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Nama = row[NamaField.Nama].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString()
            };
        }

        public static List<RuangKelas> GetAll_Entity()
        {
            List<RuangKelas> hasil = new List<RuangKelas>();
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

        public static RuangKelas GetByID_Entity(string kode)
        {
            RuangKelas hasil = new RuangKelas();
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
        public static void Insert(RuangKelas ruang_kelas, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Guid.NewGuid()));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, ruang_kelas.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, ruang_kelas.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, ruang_kelas.Rel_Sekolah));
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

        public static void Update(RuangKelas ruang_kelas, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, ruang_kelas.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, ruang_kelas.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, ruang_kelas.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, ruang_kelas.Rel_Sekolah));
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