using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_NilaiEkskul
    {
        public const string SP_SELECT_BY_TA_BY_SM = "SMA_Rapor_NilaiEkskul_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_BY_TA_BY_SM_BY_MAPEL = "SMA_Rapor_NilaiEkskul_SELECT_BY_TA_BY_SM_BY_MAPEL";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_NilaiEkskul_SELECT_BY_ID";
        
        public const string SP_INSERT = "SMA_Rapor_NilaiEkskul_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_NilaiEkskul_UPDATE";

        public const string SP_DELETE_BY_TA_BY_SM_BY_MAPEL = "SMA_Rapor_NilaiEkskul_DELETE_BY_TA_BY_SM_BY_MAPEL";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Mapel = "Rel_Mapel";
        }

        private static Rapor_NilaiEkskul GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_NilaiEkskul
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString()
            };
        }

        public static List<Rapor_NilaiEkskul> GetByTABySMByMapel_Entity(string tahun_ajaran, string semester, string rel_mapel)
        {
            List<Rapor_NilaiEkskul> hasil = new List<Rapor_NilaiEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);

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

        public static List<Rapor_NilaiEkskul> GetByTABySM_Entity(string tahun_ajaran, string semester)
        {
            List<Rapor_NilaiEkskul> hasil = new List<Rapor_NilaiEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

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
        
        public static Rapor_NilaiEkskul GetByID_Entity(string kode)
        {
            Rapor_NilaiEkskul hasil = new Rapor_NilaiEkskul();
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

        public static void DeleteByTABySMByMapel(string tahun_ajaran, string semester, string rel_mapel)
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
                comm.CommandText = SP_DELETE_BY_TA_BY_SM_BY_MAPEL;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, rel_mapel));
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

        public static void Insert(Rapor_NilaiEkskul m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
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

        public static void Update(Rapor_NilaiEkskul m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
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