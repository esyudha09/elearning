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
    public static class DAO_PerpustakaanKunjungan
    {
        public const string SP_SELECT_ALL = "PerpustakaanKunjungan_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "PerpustakaanKunjungan_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_TANGGAL = "PerpustakaanKunjungan_SELECT_ALL_BY_TANGGAL";
        public const string SP_SELECT_ALL_BY_TANGGAL_FOR_SEARCH = "PerpustakaanKunjungan_SELECT_ALL_BY_TANGGAL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_TANGGAL_BY_UNIT = "PerpustakaanKunjungan_SELECT_ALL_BY_TANGGAL_BY_UNIT";
        public const string SP_SELECT_ALL_BY_TANGGAL_BY_UNIT_FOR_SEARCH = "PerpustakaanKunjungan_SELECT_ALL_BY_TANGGAL_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "PerpustakaanKunjungan_SELECT_BY_ID";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET = "PerpustakaanKunjungan_SELECT_BY_TAHUNAJARAN_BY_KELASDET";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_FOR_SEARCH = "PerpustakaanKunjungan_SELECT_BY_TAHUNAJARAN_BY_KELASDET_FOR_SEARCH";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_GURU = "PerpustakaanKunjungan_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_GURU";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_GURU_FOR_SEARCH = "PerpustakaanKunjungan_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_GURU_FOR_SEARCH";
        public const string SP_SELECT_BY_GURU = "PerpustakaanKunjungan_SELECT_BY_GURU";

        public const string SP_INSERT = "PerpustakaanKunjungan_INSERT";

        public const string SP_UPDATE = "PerpustakaanKunjungan_UPDATE";

        public const string SP_DELETE = "PerpustakaanKunjungan_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Tanggal = "Tanggal";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Guru = "Rel_Guru";
            public const string Keterangan = "Keterangan";
            public const string Status = "Status";
        }

        public static PerpustakaanKunjungan GetEntityFromDataRow(DataRow row)
        {
            return new PerpustakaanKunjungan
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Status = row[NamaField.Status].ToString()
            };
        }

        public static List<PerpustakaanKunjungan> GetAll_Entity()
        {
            List<PerpustakaanKunjungan> hasil = new List<PerpustakaanKunjungan>();
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

        public static List<PerpustakaanKunjungan> GetByGuru_Entity(string rel_guru)
        {
            List<PerpustakaanKunjungan> hasil = new List<PerpustakaanKunjungan>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Guru, rel_guru);

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
                
        public static PerpustakaanKunjungan GetByID_Entity(string kode)
        {
            PerpustakaanKunjungan hasil = new PerpustakaanKunjungan();
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
        public static void Insert(PerpustakaanKunjungan m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Status, m.Status));
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

        public static void Update(PerpustakaanKunjungan m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Status, m.Status));
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