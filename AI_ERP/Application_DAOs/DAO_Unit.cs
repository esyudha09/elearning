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
    public static class DAO_Unit
    {
        public const string SP_SELECT_ALL = "Unit_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "Unit_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "Unit_SELECT_BY_ID";
        
        public const string SP_INSERT = "Unit_INSERT";

        public const string SP_UPDATE = "Unit_UPDATE";

        public const string SP_DELETE = "Unit_DELETE";
        
        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Divisi = "Rel_Divisi";
            public const string Nama = "Nama";
            public const string Keterangan = "Keterangan";
            public const string Email = "Email";
            public const string Website = "Website";
        }

        public static Unit GetEntityFromDataRow(DataRow row)
        {
            return new Unit
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Divisi = new Guid(row[NamaField.Rel_Divisi].ToString()),
                Nama = row[NamaField.Nama].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Email = row[NamaField.Email].ToString(),
                Website = row[NamaField.Website].ToString()
            };
        }

        public static List<Unit> GetAll_Entity()
        {
            List<Unit> hasil = new List<Unit>();
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

        public static Unit GetByID_Entity(string kode)
        {
            Unit hasil = new Unit();
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

        public static void Insert(Unit unit, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Divisi, unit.Rel_Divisi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, unit.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, unit.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, unit.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Website, unit.Website));
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

        public static void Update(Unit unit, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Divisi, unit.Rel_Divisi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, unit.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, unit.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, unit.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Email, unit.Email));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Website, unit.Website));
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