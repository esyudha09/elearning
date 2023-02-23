using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_SMTP
    {
        public const string SP_SELECT_ALL = "SMTP_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "SMTP_SELECT_BY_ID";
        public const string SP_SELECT_BY_APP = "SMTP_SELECT_BY_APP";

        public const string SP_INSERT = "SMTP_INSERT";

        public const string SP_UPDATE = "SMTP_UPDATE";

        public const string SP_DELETE = "SMTP_DELETE";

        public static class NamaField
        {
            public const string KODE = "Kode";
            public const string REL_APLIKASI = "Rel_Aplikasi";
            public const string ADDRESS = "Address";
            public const string PORT = "Port";
            public const string DISPLAY_NAME = "DisplayName";
            public const string USER_NAME = "UserName";
            public const string PASSWORD = "Password";
            public const string MAX_KIRIM = "MaxKirim";
        }

        private static SMTP GetEntityFromDataRow(DataRow row)
        {
            return new SMTP
            {
                Kode = new Guid(row[NamaField.KODE].ToString()),
                Rel_Aplikasi = new Guid(row[NamaField.REL_APLIKASI].ToString()),
                Address = row[NamaField.ADDRESS].ToString(),
                Port = row[NamaField.PORT].ToString(),
                DisplayName = row[NamaField.DISPLAY_NAME].ToString(),
                UserName = row[NamaField.USER_NAME].ToString(),
                Password = row[NamaField.PASSWORD].ToString(),
                MaxKirim = Convert.ToInt16(row[NamaField.MAX_KIRIM])
            };
        }

        public static List<SMTP> GetData_Entity()
        {
            List<SMTP> lst = new List<SMTP>();
            SqlConnection conn = Libs.GetConnection_Mailer();
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

        public static SMTP GetDataByID_Entity(string kode)
        {
            SMTP hasil = new SMTP();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

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

        public static List<SMTP> GetDataByApp_Entity(string kode)
        {
            List<SMTP> hasil = new List<SMTP>();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_APP;
                comm.Parameters.AddWithValue("@" + NamaField.REL_APLIKASI, kode);

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

        public static DateTime GetNow()
        {
            DateTime hasil = new DateTime();
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "GETNOW";

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);

                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDateTime(row[0]);
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

        public static void Insert(SMTP m)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_APLIKASI, m.Rel_Aplikasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ADDRESS, m.Address));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PORT, m.Port));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DISPLAY_NAME, m.DisplayName));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.USER_NAME, m.UserName));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PASSWORD, m.Password));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.MAX_KIRIM, m.MaxKirim));

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

        public static void Delete(string kode)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.KODE, kode));

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

        public static void Update(SMTP m)
        {
            SqlConnection conn = Libs.GetConnection_Mailer();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.REL_APLIKASI, m.Rel_Aplikasi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ADDRESS, m.Address));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PORT, m.Port));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DISPLAY_NAME, m.DisplayName));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.USER_NAME, m.UserName));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PASSWORD, m.Password));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.MAX_KIRIM, m.MaxKirim));

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