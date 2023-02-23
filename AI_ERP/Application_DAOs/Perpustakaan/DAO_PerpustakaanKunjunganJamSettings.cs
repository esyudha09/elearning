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
    public static class DAO_PerpustakaanKunjunganJamSettings
    {
        public const string SP_SELECT_ALL = "PerpustakaanKunjunganJamSettings_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "PerpustakaanKunjunganJamSettings_SELECT_BY_ID";
        public const string SP_SELECT_BY_SEKOLAH = "PerpustakaanKunjunganJamSettings_SELECT_BY_SEKOLAH";

        public const string SP_INSERT = "PerpustakaanKunjunganJamSettings_INSERT";

        public const string SP_UPDATE = "PerpustakaanKunjunganJamSettings_UPDATE";

        public const string SP_DELETE = "PerpustakaanKunjunganJamSettings_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string JamKe = "JamKe";
            public const string Waktu = "Waktu";
        }

        public static PerpustakaanKunjunganJamSettings GetEntityFromDataRow(DataRow row)
        {
            return new PerpustakaanKunjunganJamSettings
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Sekolah = new Guid(row[NamaField.Rel_Sekolah].ToString()),
                JamKe = row[NamaField.JamKe].ToString(),
                Waktu = row[NamaField.Waktu].ToString()
            };
        }

        public static List<PerpustakaanKunjunganJamSettings> GetAll_Entity()
        {
            List<PerpustakaanKunjunganJamSettings> hasil = new List<PerpustakaanKunjunganJamSettings>();
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

        public static PerpustakaanKunjunganJamSettings GetByID_Entity(string kode)
        {
            PerpustakaanKunjunganJamSettings hasil = new PerpustakaanKunjunganJamSettings();
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

        public static List<PerpustakaanKunjunganJamSettings> GetByUnit_Entity(string rel_sekolah)
        {
            List<PerpustakaanKunjunganJamSettings> hasil = new List<PerpustakaanKunjunganJamSettings>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SEKOLAH;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Sekolah, rel_sekolah);

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
        public static void Insert(PerpustakaanKunjunganJamSettings m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JamKe, m.JamKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Waktu, m.Waktu));
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

        public static void Update(PerpustakaanKunjunganJamSettings m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JamKe, m.JamKe));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Waktu, m.Waktu));
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