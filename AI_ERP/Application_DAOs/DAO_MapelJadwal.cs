using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_MapelJadwal
    {
        public const string SP_SELECT_ALL = "MapelJadwal_SELECT_ALL";
        public const string SP_SELECT_ALL_TOP_20 = "MapelJadwal_SELECT_ALL_TOP_20";
        public const string SP_SELECT_BY_ID = "MapelJadwal_SELECT_BY_ID";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_SEKOLAH = "MapelJadwal_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_SEKOLAH";

        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE = "MapelJadwal_SELECT_PERIODE";

        public const string SP_INSERT = "MapelJadwal_INSERT";

        public const string SP_UPDATE = "MapelJadwal_UPDATE";

        public const string SP_DELETE = "MapelJadwal_DELETE";

        public class TahunAjaranSemester
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Kode = "Rel_Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string JenisPengaturan = "JenisPengaturan";
            public const string PeriodeDariTanggal = "PeriodeDariTanggal";
            public const string PeriodeSampaiTanggal = "PeriodeSampaiTanggal";
            public const string CopyPeriodeDariTanggal = "CopyPeriodeDariTanggal";
            public const string CopyPeriodeSampaiTanggal = "CopyPeriodeSampaiTanggal";
            public const string CreatedDate = "CreatedDate";
            public const string LastUpdated = "LastUpdated";
            public const string CreatedBy = "CreatedBy";
            public const string LastUpdatedBy = "LastUpdatedBy";
        }

        private static MapelJadwal GetEntityFromDataRow(DataRow row)
        {
            DateTime tanggal_copy_dari = DateTime.MinValue;
            DateTime tanggal_copy_sampai = DateTime.MinValue;

            if (row[NamaField.CopyPeriodeDariTanggal] == DBNull.Value) tanggal_copy_dari = DateTime.MinValue;
            if (row[NamaField.CopyPeriodeSampaiTanggal] == DBNull.Value) tanggal_copy_sampai = DateTime.MinValue;

            return new MapelJadwal
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Kode = row[NamaField.Rel_Kode].ToString(),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString(),
                JenisPengaturan = row[NamaField.JenisPengaturan].ToString(),
                PeriodeDariTanggal = Convert.ToDateTime(row[NamaField.PeriodeDariTanggal]),
                PeriodeSampaiTanggal = Convert.ToDateTime(row[NamaField.PeriodeSampaiTanggal]),
                CopyPeriodeDariTanggal = tanggal_copy_dari,
                CopyPeriodeSampaiTanggal = tanggal_copy_sampai,
                CreatedDate = Convert.ToDateTime(row[NamaField.CreatedDate]),
                LastUpdated = Convert.ToDateTime(row[NamaField.LastUpdated]),
                CreatedBy = row[NamaField.CreatedBy].ToString(),
                LastUpdatedBy = row[NamaField.LastUpdatedBy].ToString()
            };
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

        public static void Insert(MapelJadwal m, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kode, m.Rel_Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPengaturan, m.JenisPengaturan));
                if (m.PeriodeDariTanggal != DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PeriodeDariTanggal, m.PeriodeDariTanggal));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PeriodeDariTanggal, DBNull.Value));
                }
                if (m.PeriodeSampaiTanggal != DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PeriodeSampaiTanggal, m.PeriodeSampaiTanggal));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PeriodeSampaiTanggal, DBNull.Value));
                }
                if (m.CopyPeriodeDariTanggal != DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.CopyPeriodeDariTanggal, m.CopyPeriodeDariTanggal));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.CopyPeriodeDariTanggal, DBNull.Value));
                }
                if (m.CopyPeriodeSampaiTanggal != DateTime.MinValue)
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.CopyPeriodeSampaiTanggal, m.CopyPeriodeSampaiTanggal));
                }
                else
                {
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.CopyPeriodeSampaiTanggal, DBNull.Value));
                }
                comm.Parameters.Add(new SqlParameter("@" + NamaField.CreatedDate, m.CreatedDate));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LastUpdated, m.LastUpdated));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.CreatedBy, m.CreatedBy));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LastUpdatedBy, m.LastUpdatedBy));
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

        public static List<TahunAjaranSemester> GetDistinctTahunAjaranPeriode_Entity()
        {
            List<TahunAjaranSemester> hasil = new List<TahunAjaranSemester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new TahunAjaranSemester
                            {
                                TahunAjaran = row[0].ToString(),
                                Semester = row[1].ToString()
                            }
                        );
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

        public static void Update(MapelJadwal m, string user_id)
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
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kode, m.Rel_Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPengaturan, m.JenisPengaturan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PeriodeDariTanggal, m.PeriodeDariTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.PeriodeSampaiTanggal, m.PeriodeSampaiTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.CopyPeriodeDariTanggal, m.CopyPeriodeDariTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.CopyPeriodeSampaiTanggal, m.CopyPeriodeSampaiTanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.CreatedDate, m.CreatedDate));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LastUpdated, m.LastUpdated));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.CreatedBy, m.CreatedBy));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LastUpdatedBy, m.LastUpdatedBy));
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

        public static List<MapelJadwal> GetAllByTahunAjaranBySemesterBySekolah_Entity(
                string tahun_ajaran,
                string semester,
                string rel_sekolah
            )
        {
            List<MapelJadwal> hasil = new List<MapelJadwal>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUNAJARAN_BY_SEMESTER_BY_SEKOLAH;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
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

        public static List<MapelJadwal> GetAll_Entity()
        {
            List<MapelJadwal> hasil = new List<MapelJadwal>();
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

        public static List<MapelJadwal> GetTop20_Entity()
        {
            List<MapelJadwal> hasil = new List<MapelJadwal>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL_TOP_20;

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

        public static MapelJadwal GetByID_Entity(
                string kode
            )
        {
            MapelJadwal hasil = new MapelJadwal();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
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
    }
}