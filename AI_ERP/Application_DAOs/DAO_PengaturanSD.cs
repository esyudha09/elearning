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
    public static class DAO_PengaturanSD
    {
        public const string SP_SELECT_ALL = "PengaturanSD_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "PengaturanSD_SELECT_BY_ID";
        
        public const string SP_INSERT = "PengaturanSD_INSERT";

        public const string SP_UPDATE = "PengaturanSD_UPDATE";

        public const string SP_DELETE = "PengaturanSD_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string HeaderLogo = "HeaderLogo";
            public const string HeaderKop = "HeaderKop";
            public const string HeaderAlamat = "HeaderAlamat";
        }

        public static PengaturanSD GetEntityFromDataRow(DataRow row)
        {
            return new PengaturanSD
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                HeaderLogo = row[NamaField.HeaderLogo].ToString(),
                HeaderKop = row[NamaField.HeaderKop].ToString(),
                HeaderAlamat = row[NamaField.HeaderAlamat].ToString()
            };
        }
        
        public static List<PengaturanSD> GetAll_Entity()
        {
            List<PengaturanSD> hasil = new List<PengaturanSD>();
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

        public static PengaturanSD GetByID_Entity(string kode)
        {
            PengaturanSD hasil = new PengaturanSD();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
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

        public static void Insert(PengaturanSD m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderLogo, m.HeaderLogo));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderKop, m.HeaderKop));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderAlamat, m.HeaderAlamat));
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

        public static void Update(PengaturanSD m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderLogo, m.HeaderLogo));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderKop, m.HeaderKop));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.HeaderAlamat, m.HeaderAlamat));
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

        public static void SaveData(PengaturanSD m)
        {
            if (GetAll_Entity().Count > 0)
            {
                Update(m);
            }
            else
            {
                Insert(m);
            }
        }
    }
}