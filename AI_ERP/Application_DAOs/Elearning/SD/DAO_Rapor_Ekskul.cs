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
    public static class DAO_Rapor_Ekskul
    {
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SD_Rapor_Ekskul_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";
        
        public const string SP_INSERT = "SD_Rapor_Ekskul_INSERT";

        public const string SP_UPDATE = "SD_Rapor_Ekskul_UPDATE";

        public const string SP_DELETE = "SD_Rapor_Ekskul_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Kelas2 = "Rel_Kelas2";
            public const string Rel_Kelas3 = "Rel_Kelas3";
            public const string Rel_Kelas4 = "Rel_Kelas4";
            public const string Rel_Kelas5 = "Rel_Kelas5";
            public const string Rel_Kelas6 = "Rel_Kelas6";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Kurikulum = "Kurikulum";
        }

        private static Rapor_Ekskul GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Ekskul
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = row[NamaField.Rel_Kelas].ToString(),
                Rel_Kelas2 = row[NamaField.Rel_Kelas2].ToString(),
                Rel_Kelas3 = row[NamaField.Rel_Kelas3].ToString(),
                Rel_Kelas4 = row[NamaField.Rel_Kelas4].ToString(),
                Rel_Kelas5 = row[NamaField.Rel_Kelas5].ToString(),
                Rel_Kelas6 = row[NamaField.Rel_Kelas6].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Kurikulum = row[NamaField.Kurikulum].ToString()
            };
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

        public static void Insert(Rapor_Ekskul m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas2, m.Rel_Kelas2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas3, m.Rel_Kelas3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas4, m.Rel_Kelas4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas5, m.Rel_Kelas5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas6, m.Rel_Kelas6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
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

        public static void Update(Rapor_Ekskul m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas2, m.Rel_Kelas2));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas3, m.Rel_Kelas3));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas4, m.Rel_Kelas4));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas5, m.Rel_Kelas5));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas6, m.Rel_Kelas6));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
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

        public static List<Rapor_Ekskul> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_kelas2,
                string rel_kelas3,
                string rel_kelas4,
                string rel_kelas5,
                string rel_kelas6,
                string rel_mapel
            )
        {
            List<Rapor_Ekskul> hasil = new List<Rapor_Ekskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas2, rel_kelas2);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas3, rel_kelas3);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas4, rel_kelas4);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas5, rel_kelas5);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas6, rel_kelas6);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

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