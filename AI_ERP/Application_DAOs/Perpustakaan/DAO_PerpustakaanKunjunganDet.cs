using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Perpustakaan;

namespace AI_ERP.Application_DAOs.Perpustakaan
{
    public static class DAO_PerpustakaanKunjunganDet
    {
        public const string SP_SELECT_ALL = "PerpustakaanKunjunganDet_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "PerpustakaanKunjunganDet_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "PerpustakaanKunjunganDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "PerpustakaanKunjunganDet_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_TANGGAL_BY_WAKTU = "PerpustakaanKunjunganDet_SELECT_BY_TANGGAL_BY_WAKTU";

        public const string SP_INSERT = "PerpustakaanKunjunganDet_INSERT";

        public const string SP_UPDATE = "PerpustakaanKunjunganDet_UPDATE";

        public const string SP_DELETE = "PerpustakaanKunjunganDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "PerpustakaanKunjunganDet_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_PerpustakaanKunjungan = "Rel_PerpustakaanKunjungan";
            public const string JamKe = "JamKe";
            public const string Waktu = "Waktu";
        }

        public static PerpustakaanKunjunganDet GetEntityFromDataRow(DataRow row)
        {
            return new PerpustakaanKunjunganDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_PerpustakaanKunjungan = new Guid(row[NamaField.Rel_PerpustakaanKunjungan].ToString()),
                JamKe = row[NamaField.JamKe].ToString(),
                Waktu = row[NamaField.Waktu].ToString()
            };
        }

        public static List<PerpustakaanKunjunganDet> GetAll_Entity()
        {
            List<PerpustakaanKunjunganDet> hasil = new List<PerpustakaanKunjunganDet>();
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

        public static PerpustakaanKunjunganDet GetByID_Entity(string kode)
        {
            PerpustakaanKunjunganDet hasil = new PerpustakaanKunjunganDet();
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

        public static List<PerpustakaanKunjunganDet> GetByHeader_Entity(string rel_header)
        {
            List<PerpustakaanKunjunganDet> hasil = new List<PerpustakaanKunjunganDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_header == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_PerpustakaanKunjungan, rel_header);

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

        public static List<PerpustakaanKunjunganDet> GetByTanggalByWaktu_Entity(DateTime tanggal, string waktu)
        {
            List<PerpustakaanKunjunganDet> hasil = new List<PerpustakaanKunjunganDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TANGGAL_BY_WAKTU;
                comm.Parameters.AddWithValue("@Tanggal", tanggal);
                comm.Parameters.AddWithValue("@Waktu", waktu);

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

        public static void DeleteByHeader(string rel_header, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_PerpustakaanKunjungan, rel_header));
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

        public static void Insert(PerpustakaanKunjunganDet m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_PerpustakaanKunjungan, m.Rel_PerpustakaanKunjungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JamKe, m.JamKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Waktu, m.Waktu));

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

        public static void Update(PerpustakaanKunjunganDet m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_PerpustakaanKunjungan, m.Rel_PerpustakaanKunjungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JamKe, m.JamKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Waktu, m.Waktu));

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