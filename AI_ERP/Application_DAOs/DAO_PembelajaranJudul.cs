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
    public static class DAO_PembelajaranJudul
    {
        public const string SP_SELECT_ALL = "PembelajaranJudul_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "PembelajaranJudul_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "PembelajaranJudul_SELECT_BY_ID";

        public const string SP_INSERT = "PembelajaranJudul_INSERT";

        public const string SP_UPDATE = "PembelajaranJudul_UPDATE";

        public const string SP_DELETE = "PembelajaranJudul_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Pembelajaran = "Rel_Pembelajaran";
            public const string Judul = "Judul";
            public const string Keterangan = "Keterangan";
            public const string Urutan = "Urutan";
        }

        public static PembelajaranJudul GetEntityFromDataRow(DataRow row)
        {
            return new PembelajaranJudul
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Pembelajaran = new Guid(row[NamaField.Rel_Pembelajaran].ToString()),
                Judul = row[NamaField.Judul].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Keterangan])
            };
        }

        public static List<PembelajaranJudul> GetAll_Entity()
        {
            List<PembelajaranJudul> hasil = new List<PembelajaranJudul>();
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

        public static PembelajaranJudul GetByID_Entity(string kode)
        {
            PembelajaranJudul hasil = new PembelajaranJudul();
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
        public static void Insert(PembelajaranJudul m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pembelajaran, m.Rel_Pembelajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Judul, m.Judul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));

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

        public static void Update(PembelajaranJudul m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pembelajaran, m.Rel_Pembelajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Judul, m.Judul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));

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