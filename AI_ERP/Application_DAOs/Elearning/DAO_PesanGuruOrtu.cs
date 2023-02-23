using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_PesanGuruOrtu
    {
        public const string SP_SELECT_ALL = "RuangKelas_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "PesanGuruOrtu_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "RuangKelas_SELECT_BY_ID";
        public const string SP_SELECT_BY_GURU_BY_ORTU = "PesanGuruOrtu_SELECT_BY_GURU_BY_ORTU";        

        public const string SP_INSERT = "RuangKelas_INSERT";

        public const string SP_DELETE = "RuangKelas_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Tanggal = "Tanggal";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string Rel_Ortu = "Rel_Ortu";
            public const string Pesan = "Pesan";
        }

        private static PesanGuruOrtu GetEntityFromDataRow(DataRow row)
        {
            return new PesanGuruOrtu
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                Rel_Ortu = row[NamaField.Rel_Ortu].ToString(),
                Pesan = row[NamaField.Pesan].ToString()
            };
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

        public static void Insert(PesanGuruOrtu m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Ortu, m.Rel_Ortu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Pesan, m.Pesan));
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

        public static List<PesanGuruOrtu> GetAll_Entity()
        {
            List<PesanGuruOrtu> hasil = new List<PesanGuruOrtu>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

        public static List<PesanGuruOrtu> GetByGuruByOrtu_Entity(
                string rel_pegawai,
                string rel_ortu
            )
        {
            List<PesanGuruOrtu> hasil = new List<PesanGuruOrtu>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_ORTU;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Pegawai, rel_pegawai);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Ortu, rel_ortu);

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
    }
}