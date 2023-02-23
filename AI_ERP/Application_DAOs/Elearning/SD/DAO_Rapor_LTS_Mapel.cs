using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SD;

namespace AI_ERP.Application_DAOs.Elearning.SD
{
    public static class DAO_Rapor_LTS_Mapel
    {
        public const string SP_SELECT_ALL = "SD_Rapor_LTS_Mapel_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "SD_Rapor_LTS_Mapel_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "SD_Rapor_LTS_Mapel_SELECT_BY_TA_BY_SM_BY_KELAS_DET_FOR_LTS";
        public const string SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET = "SD_Rapor_LTS_Mapel_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET";

        public const string SP_INSERT = "SD_Rapor_LTS_Mapel_INSERT";

        public const string SP_UPDATE = "SD_Rapor_LTS_Mapel_UPDATE";

        public const string SP_DELETE_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET = "SD_Rapor_LTS_Mapel_DELETE_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Rapor_StrukturNilai_AP = "Rel_Rapor_StrukturNilai_AP";
            public const string Rel_Rapor_StrukturNilai_KD = "Rel_Rapor_StrukturNilai_KD";
            public const string Rel_Rapor_StrukturNilai_KP = "Rel_Rapor_StrukturNilai_KP";
            public const string Urutan = "Urutan";

        }

        private static Rapor_LTS_Mapel GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_LTS_Mapel
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Rapor_StrukturNilai_KD= row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan]),
            };
        }

        private static Rapor_LTS_Mapel_Ext GetEntityFromDataRow_Ext(DataRow row)
        {
            return new Rapor_LTS_Mapel_Ext
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Rapor_StrukturNilai_AP = row[NamaField.Rel_Rapor_StrukturNilai_AP].ToString(),
                Rel_Rapor_StrukturNilai_KD = row[NamaField.Rel_Rapor_StrukturNilai_KD].ToString(),
                Rel_Rapor_StrukturNilai_KP = row[NamaField.Rel_Rapor_StrukturNilai_KP].ToString(),
                Urutan = Convert.ToInt16(row[NamaField.Urutan]),
            };
        }

        public static void DeleteByTABySMByMapelByKelasDet(string tahun_ajaran, string semester, string mapel, string kelas_det)
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
                comm.CommandText = SP_DELETE_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, kelas_det));
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

        public static void Insert(Rapor_LTS_Mapel m)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
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

        public static void Update(Rapor_LTS_Mapel m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_StrukturNilai_KD, m.Rel_Rapor_StrukturNilai_KD));
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

        public static List<Rapor_LTS_Mapel_Ext> GetByTABySMByKelasDet(string tahun_ajaran, string semester, string rel_kelas_det)
        {
            List<Rapor_LTS_Mapel_Ext> hasil = new List<Rapor_LTS_Mapel_Ext>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelas_det));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Ext(row));
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

        public static List<Rapor_LTS_Mapel> GetByTABySMByMapelByKelasDet(string tahun_ajaran, string semester, string mapel, string kelas_det)
        {
            List<Rapor_LTS_Mapel> hasil = new List<Rapor_LTS_Mapel>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_MAPEL_BY_KELASDET;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, kelas_det));

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