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
    public static class DAO_UserManagement
    {
        public const string SP_SELECT_ALL = "UserManagement_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "UserManagement_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "UserManagement_SELECT_BY_ID";
        public const string SP_SELECT_BY_USER_ID = "UserManagement_SELECT_BY_USER_ID";

        public const string SP_INSERT = "UserManagement_INSERT";

        public const string SP_UPDATE = "UserManagement_UPDATE";

        public const string SP_DELETE = "UserManagement_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string UserID = "UserID";
            public const string IsActive = "IsActive";
            public const string JenisUser = "JenisUser";
        }

        public static UserManagement GetEntityFromDataRow(DataRow row)
        {
            return new UserManagement
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                UserID = row[NamaField.UserID].ToString(),
                IsActive = Convert.ToBoolean((row[NamaField.IsActive] == DBNull.Value ? false : row[NamaField.IsActive])),
                JenisUser = row[NamaField.JenisUser].ToString()
            };
        }

        public static UserManagement GetByID_Entity(string kode)
        {
            UserManagement hasil = new UserManagement();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

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

        public static UserManagement GetByUserID_Entity(string user_id)
        {
            UserManagement hasil = new UserManagement();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_USER_ID;
                comm.Parameters.AddWithValue("@" + NamaField.UserID, user_id);

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

        public static void Insert(UserManagement m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UserID, m.UserID));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsActive, m.IsActive));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisUser, m.JenisUser));

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

        public static void Update(UserManagement m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UserID, m.UserID));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsActive, m.IsActive));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisUser, m.JenisUser));

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