﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.KB;

namespace AI_ERP.Application_DAOs.Elearning.KB
{
    public static class DAO_Rapor_Kriteria
    {
        public const string SP_SELECT_ALL = "KB_Rapor_Kriteria_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "KB_Rapor_Kriteria_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "KB_Rapor_Kriteria_SELECT_BY_ID";

        public const string SP_INSERT = "KB_Rapor_Kriteria_INSERT";

        public const string SP_UPDATE = "KB_Rapor_Kriteria_UPDATE";

        public const string SP_DELETE = "KB_Rapor_Kriteria_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Alias = "Alias";
            public const string Nama = "Nama";
            public const string Keterangan = "Keterangan";
        }

        private static Rapor_Kriteria GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Kriteria
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Alias = row[NamaField.Alias].ToString(),
                Nama = row[NamaField.Nama].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString()
            };
        }

        public static List<Rapor_Kriteria> GetAll_Entity()
        {
            List<Rapor_Kriteria> hasil = new List<Rapor_Kriteria>();
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

        public static Rapor_Kriteria GetByID_Entity(string kode)
        {
            Rapor_Kriteria hasil = new Rapor_Kriteria();
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

        public static void Insert(Rapor_Kriteria m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alias, m.Alias));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
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

        public static void Update(Rapor_Kriteria m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alias, m.Alias));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, m.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
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