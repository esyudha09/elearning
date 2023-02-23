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
    public static class DAO_PegawaiRiwayatKesehatanMCU
    {
        public const string SP_SELECT_BY_HEADER = "PegawaiRiwayatKesehatanMCU_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "PegawaiRiwayatKesehatanMCU_SELECT_BY_ID";

        public const string SP_INSERT = "PegawaiRiwayatKesehatanMCU_INSERT";

        public const string SP_UPDATE = "PegawaiRiwayatKesehatanMCU_UPDATE";

        public const string SP_DELETE = "PegawaiRiwayatKesehatanMCU_DELETE";
        public const string SP_DELETE_BY_HEADER = "PegawaiRiwayatKesehatanMCU_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Pegawai = "Rel_Pegawai";
            public const string Tanggal = "Tanggal";
            public const string Kesimpulan = "Kesimpulan";
            public const string Saran = "Saran";
            public const string Keterangan = "Keterangan";
            public const string Urutan = "Urutan";
        }

        public static PegawaiRiwayatKesehatanMCU GetEntityFromDataRow(DataRow row)
        {
            return new PegawaiRiwayatKesehatanMCU
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Pegawai = row[NamaField.Rel_Pegawai].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Kesimpulan = row[NamaField.Kesimpulan].ToString(),
                Saran = row[NamaField.Saran].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan])
            };
        }

        public static List<PegawaiRiwayatKesehatanMCU> GetAllByHeader_Entity(string rel_pegawai)
        {
            List<PegawaiRiwayatKesehatanMCU> hasil = new List<PegawaiRiwayatKesehatanMCU>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Pegawai, rel_pegawai);

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

        public static PegawaiRiwayatKesehatanMCU GetByID_Entity(string kode)
        {
            PegawaiRiwayatKesehatanMCU hasil = new PegawaiRiwayatKesehatanMCU();
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

        public static void DeleteByHeader(string rel_pegawai)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, rel_pegawai));
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

        public static void Insert(PegawaiRiwayatKesehatanMCU m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kesimpulan, m.Kesimpulan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Saran, m.Saran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));

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

        public static void Update(PegawaiRiwayatKesehatanMCU m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Pegawai, m.Rel_Pegawai));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kesimpulan, m.Kesimpulan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Saran, m.Saran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Urutan, m.Urutan));

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