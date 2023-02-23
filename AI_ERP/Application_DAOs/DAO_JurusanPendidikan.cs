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
    public static class DAO_JurusanPendidikan
    {
        public const string SP_INSERT = "JurusanPendidikan_INSERT";
        public const string SP_DELETE = "JurusanPendidikan_DELETE";
        public const string SP_UPDATE = "JurusanPendidikan_UPDATE";
        public const string SP_SELECT_ALL = "JurusanPendidikan_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "JurusanPendidikan_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "JurusanPendidikan_SELECT_BY_ID";
        public const string SP_SELECT_BY_NAMA = "JurusanPendidikan_SELECT_BY_NAMA";

        public static class NamaField
        {
            public const string KODE = "Kode";
            public const string NAMA = "Nama";
            public const string KETERANGAN = "Keterangan";

            private static int GetMaxLength(string nama_field)
            {
                return Application_Libs.Libs.GetDbColumnMaxLength
                        (
                            nama_field,
                            SP_SELECT_BY_ID,
                            new List<SqlParameter>() {
                                new SqlParameter(){
                                    ParameterName = "@" +  NamaField.KODE,
                                    Value = "@_@"
                                }
                            }
                        );
            }
        }

        public static void Delete(Guid Kode)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, Kode));
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

        public static void Insert(JurusanPendidikan m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NAMA, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, m.Keterangan));
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

        public static void Update(JurusanPendidikan m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.NAMA, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KETERANGAN, m.Keterangan));
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

        public static DataTable GetAll()
        {
            DataTable dtResult = new DataTable();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;

                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return dtResult;
        }

        public static DataTable GetByID(string kode)
        {
            DataTable dtResult = new DataTable();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.KODE, kode);

                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return dtResult;
        }

        private static JurusanPendidikan GetEntityFromDataRow(DataRow row)
        {
            return new JurusanPendidikan
            {
                Kode = new Guid(row[NamaField.KODE].ToString()),
                Nama = row[NamaField.NAMA].ToString(),
                Keterangan = row[NamaField.KETERANGAN].ToString()
            };
        }

        public static List<JurusanPendidikan> GetAll_ListEntity()
        {
            List<JurusanPendidikan> lst = new List<JurusanPendidikan>();
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
                lst.Clear();
                foreach (DataRow row in dtResult.Rows)
                {
                    lst.Add(GetEntityFromDataRow(row));
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

            return lst;
        }

        public static JurusanPendidikan GetByID_Entity(string kode)
        {
            JurusanPendidikan hasil = new JurusanPendidikan();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.KODE, kode);

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
    }
}